#region Using
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using CarlosAg.ExcelXmlWriter;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess.Native.Sql;
using Microsoft.Reporting.WebForms;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using Wisej.Web;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PdfViewerNewForm : Form
    {
        FileStream stream = null;
        private CaptainModel _model = null;
        public PdfViewerNewForm(string strFileName)
        {
            //string strpath = GetSiteUrl();
            //strpath = strpath.Replace("///", "/");
            //strFileName = strFileName.Replace(" ", "%20");
            ////URI uri = new URI(string.replace(" ", "%20"));
            //strpath = strpath + "ViewPdfForm.aspx?Name=" + strFileName;

            InitializeComponent();

            //pnlDetails.Visible = false;
            ////this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
            //this.pnlDetails.Dock = Wisej.Web.DockStyle.None;

            //System.Drawing.Image IMG;
            //if (strFileName.ToUpper().Contains(".JPG") || strFileName.ToUpper().Contains(".JPEG") || strFileName.ToUpper().Contains(".PNG") || strFileName.ToUpper().Contains(".BMP"))
            //{
            Tools["excel-button"].Visible = false;
            pdfViewer.Visible = false;
            htmlPanel1.Visible = true;
            //htmlPanel1.Dock = DockStyle.Fill;

            string strpath = GetSiteUrl();
            strpath = strpath.Replace("///", "/");
            strFileName = strFileName.Replace(" ", "%20");
            //URI uri = new URI(string.replace(" ", "%20"));
            strpath = strpath + "ViewPdfForm.aspx?Name=" + strFileName;

            //       string str = "<HTML><head><style>input[type=checkbox]{display: none;}" +
            //               ".container img {margin: 100px;  transition: transform 0.25s ease;  cursor: zoom-in;} " +
            //               "input[type=checkbox]:checked ~ label > img { transform: scale(3);cursor: zoom-out;}</style></head><body><div class='container'><input type='checkbox' " +
            //"id='zoomCheck'><label for='zoomCheck'><img src='"+ strpath + "'width=800px height=800px></img></label></div></body></HTML>";//width=800vw height=800vh

            if (strFileName.ToUpper().Contains(".JPG") || strFileName.ToUpper().Contains(".JPEG") || strFileName.ToUpper().Contains(".PNG") || strFileName.ToUpper().Contains(".BMP"))
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new System.Drawing.Size(1031, 829);

                string str = "<HTML><head><style>input[type=checkbox]{display: none;}" + ".container img {margin: 100px; transition: transform 0.25s ease; cursor: zoom-in;} " + "input[type=checkbox]:checked ~ label > img { transform: scale(3);cursor: zoom-out;}</style></head><body><div><img src='" + strpath + "'width=1024px height=768px></img></label></div></body></HTML>";//1024 x 768

                this.htmlPanel1.Html = str;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;

                this.htmlPanel1.Html = "<HTML><iframe src=" + strpath + " style='display: block; height: 100vh; width: 100vw;border: none;   '></iframe></HTML>";
            }
            this.htmlPanel1.Dock = DockStyle.Fill;
            this.pnlDetails.Dock = DockStyle.None;

            //picboxImage.Visible = true;
            //pdfViewer.Visible = false;
            //try
            //{
            //    //image.BeginInit();
            //    //image.CacheOption = BitmapCacheOption.OnLoad;
            //    if (picboxImage.Image != null) picboxImage.Image.Dispose();
            //    stream = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
            //    picboxImage.Image = System.Drawing.Image.FromStream(stream);
            //    //stream.Dispose();
            //    picboxImage.Visible = true;
            //}
            //catch (Exception ex)
            //{
            //}

            //}
            //else if (strFileName.ToUpper().Contains(".PDF"))
            //{
            //    //this.htmlBox1.Html = "<HTML><iframe src=" + strpath + " height=100% width=100%></iframe></HTML>";

            //    picboxImage.Visible = false;
            //    pdfViewer.Visible = true;
            //    this.pdfViewer.PdfSource = strFileName;
            //}
        }

        public PdfViewerNewForm(string strFileName, string strtype)
        {
            //Alaa ::
            //string strpath = GetSiteUrl();
            //strpath = strpath.Replace("///", "/");
            //strFileName = strFileName.Replace(" ", "%20");
            //URI uri = new URI(string.replace(" ", "%20"));
            //strpath = strpath + "CaptchaImage.aspx";

            InitializeComponent();
            //pnlDetails.Visible = false;
            //this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
            //this.pnlDetails.Dock = Wisej.Web.DockStyle.None;

            //System.Drawing.Image IMG;
            //if (strFileName.ToUpper().Contains(".JPG") || strFileName.ToUpper().Contains(".JPEG") || strFileName.ToUpper().Contains(".PNG") || strFileName.ToUpper().Contains(".BMP"))
            //{
            //    try
            //    {
            //        IMG = System.Drawing.Image.FromFile(strFileName);
            //        this.htmlBox1.Html = "<HTML><iframe src=" + strpath + " height= 100% width = 100%></iframe></HTML>";
            //        this.Size = new Size(IMG.Width, IMG.Height);
            //        this.htmlBox1.Dock = DockStyle.Fill;
            //        this.Dock = DockStyle.Fill;
            //    }
            //    catch (Exception ex)
            //    {
            //        this.htmlBox1.Html = "<HTML><iframe src=" + strpath + " height=100% width=100%></iframe></HTML>";
            //    }

            //}
            //else
            //{
            //this.htmlBox1.Html = "<HTML><iframe src=" + strpath + " height=100% width=100%></iframe></HTML>";
            // }
        }

        public PdfViewerNewForm(string strFileName, string strApplicantNo, string struserid, string strFileType)
        {
            InitializeComponent();

            pdfViewer.Visible = false;

            htmlPanel1.Visible = true;
            htmlPanel1.Dock = DockStyle.Fill;

            string strpath = GetSiteUrl();
            strpath = strpath.Replace("///", "/");
            strFileName = strFileName.Replace(" ", "%20");
            // URI uri = new URI(string.replace(" ", "%20"));
            strpath = strpath + "signature.aspx?id=" + strFileName + "&actid=" + strApplicantNo + "&userid=" + struserid + "&filetype=" + strFileType;

            this.htmlPanel1.Html = "<HTML><iframe src=" + strpath + " style='display: block; height: 100vh; width: 100vw;border: none;'></iframe></HTML>";

            pnlDetails.Visible = false;
            Tools["excel-button"].Visible = false;


            // this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
            //this.pnlDetails.Dock = Wisej.Web.DockStyle.None;
            //this.htmlPanel1.ImageSource = strpath; // "<HTML><iframe src=" + strpath + " height=440px width=761></iframe></HTML>";
        }

        public PdfViewerNewForm(string strFileName, DataSet Result_dataSet, DataTable Result_table, string report_name, string report_to_process, string reportpath, string strUserId, bool Detail_Rep_Required, string scr_Code)
        {
            //string strpath = "http://localhost:60865///"; //GetSiteUrl(); //
            //strpath = strpath.Replace("///", "/");
            //strFileName = strFileName.Replace(" ", "%20");
            ////URI uri = new URI(string.replace(" ", "%20"));
            //strpath = strpath + "ViewPdfForm.aspx?Name=" + strFileName;

            InitializeComponent();
            htmlPanel1.Visible = false;
            pdfViewer.Visible = true;

            UserId = strUserId;
            Scr_Code = scr_Code;
            Report_To_Process = report_to_process;
            Result_Table = Result_table;
            Report_Name = report_name;
            Result_DataSet = Result_dataSet;
            ReportPath = reportpath;

            if (Detail_Rep_Required)
            {
                //this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
                pnlDetails.Visible = true;
                switch (Scr_Code)
                {
                    case "RNGB0004":
                        Btn_Bypass.Visible = Btn_SNP_Details.Visible = Btn_MST_Details.Visible = true;
                        Tools["excel-button"].Visible = false;
                        break; // 
                    case "RNGB0014":
                        //this.Btn_Bypass.Location = new System.Drawing.Point(2, 507);
                        Btn_Bypass.Text = "Detail Report";
                        Btn_Bypass.Visible = true;
                        Tools["excel-button"].Visible = false;
                        break;
                }
            }
            else
            {
                //this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
                pnlDetails.Visible = false;
                Tools["excel-button"].Visible = false;
                //this.pdfViewer.PdfSource = strFileName;
            }
            this.pdfViewer.PdfSource = strFileName;
            //this.htmlBox1.Html = "<HTML><iframe src=" + strpath + " height=100% width=100%></iframe></HTML>";
        }

        #region Added by Vikash on 01/02/2024 as per RNG ServicesROMA Services Report - Enhancements and ROMA Outcome Report - Enhancements documents

        public PdfViewerNewForm(string strFileName, DataTable datatable, DataTable Result_table, string report_name, string report_to_process, string reportpath, string strUserId, bool Detail_Rep_Required, string scr_Code, string isServPlanTarget, string b, bool SummarybyGroup, string isAddDate)
        {
            //string strpath = "http://localhost:60865///"; //GetSiteUrl(); //
            //strpath = strpath.Replace("///", "/");
            //strFileName = strFileName.Replace(" ", "%20");
            ////URI uri = new URI(string.replace(" ", "%20"));
            //strpath = strpath + "ViewPdfForm.aspx?Name=" + strFileName;

            InitializeComponent();
            htmlPanel1.Visible = false;
            pdfViewer.Visible = true;

            UserId = strUserId;
            Scr_Code = scr_Code;
            Report_To_Process = report_to_process;
            Result_Table = Result_table;
            Report_Name = report_name;
            dataTable = datatable;
            ReportPath = reportpath;
            summarybyGroup = SummarybyGroup;
            _isAddDate = isAddDate;
            _isServPlanTarget = isServPlanTarget;

            if (Detail_Rep_Required)
            {
                pnlDetails.Visible = true;
                switch (Scr_Code)
                {
                    case "RNGB0004":
                        Btn_Bypass.Visible = Btn_SNP_Details.Visible = Btn_MST_Details.Visible = true;
                        Tools["excel-button"].Visible = false;
                        break; // 
                    case "RNGB0014":
                        //this.Btn_Bypass.Location = new System.Drawing.Point(2, 507);
                        Btn_Bypass.Text = "Detail Report";
                        Btn_Bypass.Visible = false;//true;
                        Tools["excel-button"].Visible = true; //false;
                        break;
                    case "RNGS0014":
                        Btn_Bypass.Text = "Detail Report";
                        Btn_Bypass.Visible = false;//**Btn_Bypass.Visible = true;
                        Tools["excel-button"].Visible = true;
                        break;
                }
            }
            else
            {
                pnlDetails.Visible = false;
                Tools["excel-button"].Visible = false;
            }
            this.pdfViewer.PdfSource = strFileName;
        }

        public PdfViewerNewForm(string strFileName, DataTable datatable, DataTable Result_table, string report_name, string report_to_process, string reportpath, string strUserId, bool Detail_Rep_Required, string scr_Code, bool RepRefChecked, bool SummarybyGroup, string isAddDate, string isServPlanTarget)
        {
            //string strpath = "http://localhost:60865///"; //GetSiteUrl(); //
            //strpath = strpath.Replace("///", "/");
            //strFileName = strFileName.Replace(" ", "%20");
            ////URI uri = new URI(string.replace(" ", "%20"));
            //strpath = strpath + "ViewPdfForm.aspx?Name=" + strFileName;

            InitializeComponent();
            htmlPanel1.Visible = false;
            pdfViewer.Visible = true;

            UserId = strUserId;
            Scr_Code = scr_Code;
            Report_To_Process = report_to_process;
            Result_Table = Result_table;
            Report_Name = report_name;
            dataTable = datatable;
            ReportPath = reportpath;
            _RepRefChecked = RepRefChecked;
            summarybyGroup = SummarybyGroup;
            _isAddDate = isAddDate;
            _isServPlanTarget = isServPlanTarget;

            if (Detail_Rep_Required)
            {
                pnlDetails.Visible = true;
                switch (Scr_Code)
                {
                    case "RNGB0004":
                        Btn_Bypass.Visible = Btn_SNP_Details.Visible = Btn_MST_Details.Visible = true;
                        Tools["excel-button"].Visible = false;
                        break; // 
                    case "RNGB0014":
                        Btn_Bypass.Text = "Detail Report";
                        Btn_Bypass.Visible = false;// true;
                        Tools["excel-button"].Visible = true; //false;
                        break;
                    case "RNGS0014":
                        Btn_Bypass.Text = "Detail Report";
                        Btn_Bypass.Visible = false; //**Btn_Bypass.Visible = true;
                        Tools["excel-button"].Visible = true;
                        break;
                }
            }
            else
            {
                pnlDetails.Visible = false;
                Tools["excel-button"].Visible = false;
            }
            this.pdfViewer.PdfSource = strFileName;
        }

        #endregion

        public PdfViewerNewForm(string strFileName, DataSet Result_dataSet, DataTable Result_table, string report_name, string report_to_process, string reportpath, string strUserId, bool Detail_Rep_Required, string scr_Code, string bypass_name, string mst_Rep_name, string ind_rep_name, BaseForm baseform)
        {
            //string strpath = "http://localhost:60865///"; //GetSiteUrl(); //
            //strpath = strpath.Replace("///", "/");
            //strFileName = strFileName.Replace(" ", "%20");
            ////URI uri = new URI(string.replace(" ", "%20"));
            //strpath = strpath + "ViewPdfForm.aspx?Name=" + strFileName;
            InitializeComponent();
            htmlPanel1.Visible = false;
            pdfViewer.Visible = true;

            _model = new CaptainModel();

            UserId = strUserId;
            Scr_Code = scr_Code;
            Report_To_Process = report_to_process;
            Result_Table = Result_table;
            Report_Name = report_name;
            Result_DataSet = Result_dataSet;
            ReportPath = reportpath;
            Bypass_Rep_Name = bypass_name;
            App_Rep_Name = mst_Rep_name;
            Ind_Rep_Name = ind_rep_name;
            _baseForm = baseform;

            if (Detail_Rep_Required)
            {
                //this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
                pnlDetails.Visible = true;
                switch (Scr_Code)
                {
                    case "RNGB0004":
                        Btn_Bypass.Visible = Btn_SNP_Details.Visible = Btn_MST_Details.Visible = true;
                        Tools["excel-button"].Visible = false;
                        break; // 
                    case "RNGB0014":
                        //this.Btn_Bypass.Location = new System.Drawing.Point(2, 507);
                        Btn_Bypass.Text = "Detail Report";
                        Btn_Bypass.Visible = true;
                        Tools["excel-button"].Visible = false;
                        break;
                }
            }
            else
            {
                //this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
                pnlDetails.Visible = false;
                Tools["excel-button"].Visible = false;
                //this.pdfViewer.PdfSource = strFileName;
            }
            this.pdfViewer.PdfSource = strFileName;
            //this.htmlBox1.Html = "<HTML><iframe src=" + strpath + " height=100% width=100%></iframe></HTML>";
        }

        //public PdfViewerNewForm(string strFileName, ReportDataSource dataSource)
        //{
        //    //string strpath = "http://localhost:60865///"; //GetSiteUrl(); //

        //    InitializeComponent();
        //    htmlPanel1.Visible = false;
        //    pdfViewer.Visible = true;
            
        //    _dataSource = dataSource;

        //    Btn_Bypass.Visible = Btn_SNP_Details.Visible = Btn_MST_Details.Visible = false;

        //    this.pdfViewer.PdfSource = strFileName;
        //}
        public string UserId
        {
            get; set;
        }
        public string Bypass_Rep_Name
        {
            get; set;
        }
        public string App_Rep_Name
        {
            get; set;
        }
        public string Ind_Rep_Name
        {
            get; set;
        }
        public string _isAddDate
        {
            get; set;
        }

        public BaseForm _baseForm
        {
            get; set;
        }

        public static string GetSiteUrl()
        {
            string url = string.Empty;
            url = Application.Url;
            //HttpRequest request = HttpContext.Current.Request;


            //if (request.IsSecureConnection)
            //    url = "https://";
            //else
            //    url = "http://";

            //return url + HttpContext.Current.Request.Url.Authority + "/" + HttpContext.Current.Request.ApplicationPath + "/";

            return url;

        }

        #region properties

        public ReportDataSource _dataSource
        {
        get; set; 
        }

        public string Scr_Code
        {
            get; set;
        }

        public string Report_Name
        {
            get; set;
        }

        public string Report_To_Process
        {
            get; set;
        }

        public DataTable Result_Table
        {
            get; set;
        }

        public DataTable Summary_table
        {
            get; set;
        }

        public DataSet Result_DataSet
        {
            get; set;
        }

        public DataTable dataTable
        {
            get; set;
        }

        public bool Detail_Rep_Required
        {
            get; set;
        }

        public bool summarybyGroup
        {
            get; set;
        }

        public bool _RepRefChecked
        {
            get; set;
        }

        public string Main_Rep_Name
        {
            get; set;
        }

        public string ReportPath
        {
            get; set;
        }

        public string _isServPlanTarget
        {
            get; set;
        }

        #endregion

        private void Btn_Bypass_Click(object sender, EventArgs e)
        {

            switch (Scr_Code)
            {
                case "RNGB0004":
                    //Report_Name = "RNGB0004_Bypass_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc";
                    Report_Name = Bypass_Rep_Name;
                    break;
                case "RNGB0014":
                    Report_Name = Report_Name;
                    break;
                case "RNGS0014":
                    Report_Name = Report_Name;
                    break;
            }

            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Summary_table, Result_Table, Report_Name, "Result Table", ReportPath);
            CASB2012_AdhocRDLCForm RDLC_Form;
            if (Scr_Code == "RNGB0004")
            {
                //RDLC_Form = new CASB2012_AdhocRDLCForm(Result_DataSet.Tables[2], Summary_table, Report_Name, "Result Table", ReportPath, UserId, "RNG");
                //RDLC_Form.ShowDialog();

                if (Result_DataSet.Tables[2].Rows.Count>0)
                {
                    GenerateBypassReport_DevExpress(Result_DataSet.Tables[2], Summary_table, Report_Name, ReportPath, UserId);
                    //GenerateBypassReport(Result_DataSet.Tables[2], Summary_table, Report_Name, ReportPath, UserId);
                }

            }
            else if (Scr_Code == "RNGB0014")
            {
                RDLC_Form = new CASB2012_AdhocRDLCForm(Result_Table, Summary_table, Report_Name, "Result Table", ReportPath, UserId, "RNG");
                RDLC_Form.ShowDialog();
            }
            //**else
            //**GenerateDetailReport(dataTable, Result_Table, Summary_table, Report_Name, ReportPath, UserId, _RepRefChecked);
            //**RDLC_Form = new CASB2012_AdhocRDLCForm(Result_Table, Summary_table, Report_Name, "Result Table", ReportPath, UserId, "RNG");
            //RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);

        }

        private void Btn_SNP_Details_Click(object sender, EventArgs e)
        {
            //Report_Name = "RNGB0004_SNP_IND_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc";
            Report_Name = Ind_Rep_Name;
            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Result_DataSet.Tables[3], Summary_table, Report_Name, "Result Table", ReportPath, UserId, "RNG"); //8, 5
            //RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
            //RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
            //RDLC_Form.ShowDialog();

            if (Result_DataSet.Tables[3].Rows.Count > 0)
            {
                GenerateIndividualDetailsReport_DevExpress(Result_DataSet.Tables[3], Summary_table, Report_Name, ReportPath, UserId, Ind_Rep_Name);
                //GenerateIndividualDetailsReport(Result_DataSet.Tables[3], Summary_table, Report_Name, ReportPath, UserId, Ind_Rep_Name);
            }
        }

        private void Btn_MST_Details_Click(object sender, EventArgs e)
        {
            //Report_Name = "RNGB0004_MST_FAM_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm")+ ".rdlc";
            Report_Name = App_Rep_Name;
            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Result_DataSet.Tables[4], Summary_table, Report_Name, "Result Table", ReportPath, UserId, "RNG"); //8, 8
            ////RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
            //RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
            //RDLC_Form.ShowDialog();

            if (Result_DataSet.Tables[4].Rows.Count > 0)
            {
                GenerateFamilyDetailsReport_DevExpress(Result_DataSet.Tables[4], Summary_table, Report_Name, ReportPath, UserId, Ind_Rep_Name);
                //GenerateFamilyDetailsReport(Result_DataSet.Tables[4], Summary_table, Report_Name, ReportPath, UserId, Ind_Rep_Name);
            }

        }

        private void PdfViewerNewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stream != null)
                stream.Dispose();
        }

        private void PdfViewerNewForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            var toolButton = e.Tool.Name;

            if (toolButton == "excel-button")
            {
                if(Scr_Code == "RNGS0014")
                    GenerateServicesDetailReport(dataTable, Result_Table, Summary_table, Report_Name, ReportPath, UserId, _RepRefChecked, _isServPlanTarget);
                else if(Scr_Code == "RNGB0014")
                    GenerateOutcomesDetailReport(dataTable, Result_Table, Summary_table, Report_Name, ReportPath, UserId, _RepRefChecked, _isServPlanTarget);
            }
        }

        private void GenerateOutcomesDetailReport(DataTable dataTable, DataTable result_Table, DataTable summary_table, string report_Name, string reportPath, string userId, bool repRefChecked, string ServTargetChceked)
        {
            string PdfName = "RNGB0014_Detail_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = reportPath + userId + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(reportPath + userId.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(reportPath + userId.Trim());
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
                PdfName = PdfName + ".xls";

            WorksheetRow excelrowSpace;
            WorksheetRow excelrowSpace2;
            WorksheetRow excelrowSpace3;
            WorksheetRow excelrow;
            WorksheetRow excelrow2;
            WorksheetCell cell;

            Workbook book = new Workbook();

            Worksheet sheet;

            #region Styles
            WorksheetStyle mainstyle = book.Styles.Add("MainHeaderStyles");
            mainstyle.Font.FontName = "Calibri";
            mainstyle.Font.Size = 11;//12;
            mainstyle.Font.Bold = true;
            mainstyle.Font.Color = "#2c2c2c";
            mainstyle.Interior.Color = "#d2e1ef";//"#0070c0";
            mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;
            mainstyle.Alignment.WrapText = true;

            WorksheetStyle mainstyle2 = book.Styles.Add("MainHeaderStyles2");
            mainstyle2.Font.FontName = "Calibri";
            mainstyle2.Font.Size = 12;
            mainstyle2.Font.Bold = true;
            mainstyle2.Font.Color = "#FFFFFF";
            mainstyle2.Interior.Color = "#0070c0";
            mainstyle2.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyle2.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            mainstyle2.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Calibri";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style1.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle stylecenter = book.Styles.Add("Normalcenter");
            stylecenter.Font.FontName = "Calibri";
            stylecenter.Font.Bold = true;
            stylecenter.Font.Size = 10;
            stylecenter.Font.Bold = true;
            stylecenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            stylecenter.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style3 = book.Styles.Add("NormalLeft");
            style3.Font.FontName = "Calibri";
            style3.Font.Size = 10;
            style3.Interior.Color = "#f2f2f2";
            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style3.Alignment.Vertical = StyleVerticalAlignment.Top;
            style3.Alignment.WrapText = true;

            WorksheetStyle style4 = book.Styles.Add("NormalLeftRed");
            style4.Font.FontName = "Calibri";
            style4.Font.Size = 10;
            style4.Interior.Color = "#f2f2f2";
            style4.Font.Color = "#f00808";
            style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style4.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style5 = book.Styles.Add("NormalRight");
            style5.Font.FontName = "Calibri";
            style5.Font.Size = 10;
            style5.Interior.Color = "#f2f2f2";
            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style5.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style55 = book.Styles.Add("NormalRightServTot");
            style55.Font.FontName = "Calibri";
            style55.Font.Size = 10;
            style55.Font.Bold = true;
            style55.Interior.Color = "#ffe8bd";
            style55.Interior.Pattern = StyleInteriorPattern.Solid;
            style55.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style55.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style555 = book.Styles.Add("NormalRightGrpTot");
            style555.Font.FontName = "Calibri";
            style555.Font.Size = 10;
            style555.Font.Bold = true;
            style555.Interior.Color = "#e0ebd8";
            style555.Interior.Pattern = StyleInteriorPattern.Solid;
            style555.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style555.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style6 = book.Styles.Add("NormalRightRed");
            style6.Font.FontName = "Calibri";
            style6.Font.Size = 10;
            style6.Interior.Color = "#f2f2f2";
            style6.Font.Color = "#f00808";
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style6.Alignment.Vertical = StyleVerticalAlignment.Top;

            #endregion

            sheet = book.Worksheets.Add("Data");

            sheet.Table.DefaultColumnWidth = 220.5F;

            if (repRefChecked)
            {
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(300);
                if (_isServPlanTarget == "Y")
                    sheet.Table.Columns.Add(300);
                else
                    sheet.Table.Columns.Add(80);
                //sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(550);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(70);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
            }
            else
            {
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(300);
                if (_isServPlanTarget == "Y")
                    sheet.Table.Columns.Add(300);
                else
                    sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(550);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(70);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
            }

            try
            {

                int RepCnt = 0, RepCnt1 = 0, RepCnt2 = 0, RepCnt3 = 0, RepCnt4 = 0;
                int RefCnt = 0, RefCnt1 = 0, RefCnt2 = 0, RefCnt3 = 0, RefCnt4 = 0; 
                int RepOutcomeTotal = 0, RepOutcomeTotal1 = 0, RepOutcomeTotal2 = 0, RepOutcomeTotal3 = 0, RepOutcomeTotal4 = 0;
                int RepGroupTotal = 0, RepGroupTotal1 = 0, RepGroupTotal2 = 0, RepGroupTotal3 = 0, RepGroupTotal4 = 0;
                int RefGroupTotal = 0, RefGroupTotal1 = 0, RefGroupTotal2 = 0, RefGroupTotal3 = 0, RefGroupTotal4 = 0;

                int RepTotals = 0, RefTotals = 0;
                int RepOutcomeTotals = 0, RefOutcomeTotals = 0;
                int RepGroupTotals = 0, RefGroupTotals = 0;

                excelrowSpace = sheet.Table.Rows.Add();
                cell = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                if (repRefChecked)
                    cell.MergeAcross = 23;
                else
                    cell.MergeAcross = 16;

                if(result_Table.Rows.Count > 0)
                {
                    DataView Domainsview = new DataView(result_Table);
                    DataTable distinctDomains = Domainsview.ToTable(true, "SortUnDup_Group");

                    string prevdistValue = string.Empty;
                    string prevDomain = string.Empty;
                    string prevOutcome = string.Empty;
                    string prevGroup = string.Empty;


                    foreach (DataRow disticntDomainrows in distinctDomains.Rows)
                    {
                        if (prevdistValue != disticntDomainrows["SortUnDup_Group"].ToString().Trim())
                        {
                            DataRow[] drDomains = result_Table.Select("SortUnDup_Group = '" + disticntDomainrows["SortUnDup_Group"] + "'");

                            if (drDomains.Length > 0)
                            {
                                DataTable dt1 = drDomains.CopyToDataTable();

                                RepCnt = 0; RepCnt1 = 0; RepCnt2 = 0; RepCnt3 = 0; RepCnt4 = 0;
                                RefCnt = 0; RefCnt1 = 0; RefCnt2 = 0; RefCnt3 = 0; RefCnt4 = 0;
                                RepOutcomeTotal = 0; RepOutcomeTotal1 = 0; RepOutcomeTotal2 = 0; RepOutcomeTotal3 = 0; RepOutcomeTotal4 = 0;
                                RepGroupTotal = 0; RepGroupTotal1 = 0; RepGroupTotal2 = 0; RepGroupTotal3 = 0; RepGroupTotal4 = 0;
                                RefGroupTotal = RefGroupTotal1 = RefGroupTotal2 = RefGroupTotal3 = RefGroupTotal4 = 0;

                                if (dt1.Rows.Count > 4)
                                {

                                    DataView Outcomesview = new DataView(dt1);
                                    DataTable distinctOutcomes = Outcomesview.ToTable(true, "SortUnDup_Table");
                                    int OutGrop = 3;
                                    int GrpGrp = 3;

                                    DataRow[] drGroupDesc;
                                    if (summarybyGroup)
                                    {
                                        drGroupDesc = dataTable.Select("Res_Group='" + disticntDomainrows["SortUnDup_Group"] + "' AND Res_Row_Type='TblDetails'");
                                    }
                                    else
                                    {
                                        drGroupDesc = dataTable.Select("Res_Group='" + disticntDomainrows["SortUnDup_Group"] + "' AND Res_Row_Type='TblDesc'");
                                    }

                                    DataTable dtGroupDesc = new DataTable();

                                    if (drGroupDesc.Length > 0)
                                    {
                                        dtGroupDesc = drGroupDesc.CopyToDataTable();
                                    }

                                    foreach (DataRow disticntOutcomerows in distinctOutcomes.Rows)
                                    {
                                        DataRow[] drOutcomes = dt1.Select("SortUnDup_Table='" + disticntOutcomerows["SortUnDup_Table"].ToString() + "'");

                                        if (drOutcomes.Length > 0)
                                        {
                                            DataTable dtOutcomes = drOutcomes.CopyToDataTable();

                                            DataView dv = new DataView(dtOutcomes);
                                            dv.Sort = "SortUnDup_Group,SortUnDup_Table,SortUnDup_OutcomeCode";

                                            dtOutcomes = dv.ToTable();

                                            foreach (DataRow dr in dtOutcomes.Rows)
                                            {
                                                #region Outcome Totals

                                                if (prevOutcome != dr["SortUnDup_OutcomeCode"].ToString() && dr["SortUnDup_OutcomeCode"].ToString().Trim() != "" && prevOutcome.Trim() != "" && prevOutcome.Trim() != "Goal" && prevOutcome.Trim() != "Total:")
                                                {
                                                    if (repRefChecked)
                                                    {
                                                        if (disticntOutcomerows["SortUnDup_Table"].ToString() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Outcome Totals: ", DataType.String, "NormalRightServTot");

                                                            RepOutcomeTotals = Convert.ToInt32(RepCnt.ToString()) + Convert.ToInt32(RepCnt1.ToString()) + Convert.ToInt32(RepCnt2.ToString()) + Convert.ToInt32(RepCnt3.ToString()) + Convert.ToInt32(RepCnt4.ToString());

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotals.ToString().Trim() == "" ? "" : RepOutcomeTotals.ToString().Trim(), DataType.String, "NormalRightServTot");

                                                            if (RepCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal = RepGroupTotal + RepCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt1 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt1.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal1 = RepGroupTotal1 + RepCnt1;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt2 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt2.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal2 = RepGroupTotal2 + RepCnt2;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt3 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt3.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal3 = RepGroupTotal3 + RepCnt3;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt4 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt4.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal4 = RepGroupTotal4 + RepCnt4;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            RefOutcomeTotals = Convert.ToInt32(RefCnt.ToString()) + Convert.ToInt32(RefCnt1.ToString()) + Convert.ToInt32(RefCnt2.ToString()) + Convert.ToInt32(RefCnt3.ToString()) + Convert.ToInt32(RefCnt4.ToString());

                                                            cell = excelrow2.Cells.Add(RefOutcomeTotals.ToString().Trim() == "" ? "" : RefOutcomeTotals.ToString().Trim(), DataType.String, "NormalRightServTot");

                                                            if (RefCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal = RefGroupTotal + RefCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt1 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt1.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal1 = RefGroupTotal1 + RefCnt1;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt2 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt2.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal2 = RefGroupTotal2 + RefCnt2;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt3 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt3.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal3 = RefGroupTotal3 + RefCnt3;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt4 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt4.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal4 = RefGroupTotal4 + RefCnt4;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 23;
                                                        }
                                                        RepCnt = 0; RepCnt1 = 0; RepCnt2 = 0; RepCnt3 = 0; RepCnt4 = 0;                                                       
                                                        RefCnt = 0; RefCnt1 = 0; RefCnt2 = 0; RefCnt3 = 0; RefCnt4 = 0;
                                                    }

                                                    if (!repRefChecked)
                                                    {
                                                        if (disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Outcome Totals: ", DataType.String, "NormalRightServTot");

                                                            RepOutcomeTotals = Convert.ToInt32(RepOutcomeTotal.ToString()) + Convert.ToInt32(RepOutcomeTotal1.ToString()) + Convert.ToInt32(RepOutcomeTotal2.ToString()) + Convert.ToInt32(RepOutcomeTotal3.ToString()) + Convert.ToInt32(RepOutcomeTotal4.ToString());

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotals.ToString().Trim() == "" ? "" : RepOutcomeTotals.ToString().Trim(), DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal = RepGroupTotal + RepOutcomeTotal;

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal1.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal1 = RepGroupTotal1 + RepOutcomeTotal1;

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal2.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal2 = RepGroupTotal2 + RepOutcomeTotal2;
                                                            
                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal3.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal3 = RepGroupTotal3 + RepOutcomeTotal3;
                                                            
                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal4.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal4 = RepGroupTotal4 + RepOutcomeTotal4;

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 16;

                                                        }
                                                        RepOutcomeTotal = 0; RepOutcomeTotal1 = 0; RepOutcomeTotal2 = 0; RepOutcomeTotal3 = 0; RepOutcomeTotal4 = 0; 
                                                    }
                                                }

                                                prevOutcome = dr["SortUnDup_OutcomeCode"].ToString();

                                                #endregion

                                                #region Group Totals

                                                if (prevGroup != dr["SortUnDup_Table"].ToString().Trim() && dr["SortUnDup_Table"].ToString().Trim() != "" && prevGroup.Trim() != "")
                                                {
                                                    //if (repRefChecked && ((RepGroupTotal != 0 && RepGroupTotal1 != 0 && RepGroupTotal2 != 0 && RepGroupTotal3 != 0 && RepGroupTotal4 != 0) || (RefGroupTotal != 0 && RefGroupTotal1 != 0 && RefGroupTotal2 != 0 && RefGroupTotal3 != 0 && RefGroupTotal4 != 0)))
                                                        if (repRefChecked && ((RepGroupTotal != 0 || RepGroupTotal1 != 0 || RepGroupTotal2 != 0 || RepGroupTotal3 != 0 || RepGroupTotal4 != 0) || (RefGroupTotal != 0 || RefGroupTotal1 != 0 || RefGroupTotal2 != 0 || RefGroupTotal3 != 0 || RefGroupTotal4 != 0)))
                                                        {
                                                        if (disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Group Totals: ", DataType.String, "NormalRightGrpTot");

                                                            RepGroupTotals = Convert.ToInt32(RepGroupTotal.ToString()) + Convert.ToInt32(RepGroupTotal1.ToString()) + Convert.ToInt32(RepGroupTotal2.ToString()) + Convert.ToInt32(RepGroupTotal3.ToString()) + Convert.ToInt32(RepGroupTotal4.ToString());

                                                            cell = excelrow2.Cells.Add(RepGroupTotals.ToString().Trim() == "" ? "" : RepGroupTotals.ToString().Trim(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal1.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal2.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal3.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal4.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalRightGrpTot");

                                                            RefGroupTotals = Convert.ToInt32(RefGroupTotal.ToString()) + Convert.ToInt32(RefGroupTotal1.ToString()) + Convert.ToInt32(RefGroupTotal2.ToString()) + Convert.ToInt32(RefGroupTotal3.ToString()) + Convert.ToInt32(RefGroupTotal4.ToString());

                                                            cell = excelrow2.Cells.Add(RefGroupTotals.ToString().Trim() == "" ? "" : RefGroupTotals.ToString().Trim(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RefGroupTotal.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RefGroupTotal1.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RefGroupTotal2.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RefGroupTotal3.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RefGroupTotal4.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 23;
                                                        }
                                                        RepGroupTotal = 0; RepGroupTotal1 = 0; RepGroupTotal2 = 0; RepGroupTotal3 = 0; RepGroupTotal4 = 0;
                                                        RefGroupTotal = 0; RefGroupTotal1 = 0; RefGroupTotal2 = 0; RefGroupTotal3 = 0; RefGroupTotal4 = 0;
                                                    }
                                                    //if (!repRefChecked && (RepGroupTotal != 0 && RepGroupTotal1 != 0 && RepGroupTotal2 != 0 && RepGroupTotal3 != 0 && RepGroupTotal4 != 0))
                                                        if (!repRefChecked && (RepGroupTotal != 0 || RepGroupTotal1 != 0 || RepGroupTotal2 != 0 || RepGroupTotal3 != 0 || RepGroupTotal4 != 0))
                                                        {
                                                        if (disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Group Total: ", DataType.String, "NormalRightGrpTot");

                                                            RepGroupTotals = Convert.ToInt32(RepGroupTotal.ToString()) + Convert.ToInt32(RepGroupTotal1.ToString()) + Convert.ToInt32(RepGroupTotal2.ToString()) + Convert.ToInt32(RepGroupTotal3.ToString()) + Convert.ToInt32(RepGroupTotal4.ToString());

                                                            cell = excelrow2.Cells.Add(RepGroupTotals.ToString().Trim() == "" ? "" : RepGroupTotals.ToString().Trim(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal1.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal2.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal3.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add(RepGroupTotal4.ToString(), DataType.String, "NormalRightGrpTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 16;
                                                        }
                                                        RepGroupTotal = 0; RepGroupTotal1 = 0; RepGroupTotal2 = 0; RepGroupTotal3 = 0; RepGroupTotal4 = 0;
                                                    }
                                                }
                                                prevGroup = dr["SortUnDup_Table"].ToString().Trim();

                                                #endregion

                                                #region Outcome Headers and Data

                                                if (repRefChecked)
                                                {
                                                    #region Headers

                                                    if (dr["SortUnDup_Table"].ToString().Trim() == "000GrpDesc" && prevDomain != dr["SortUnDup_Group_Desc"].ToString())
                                                    {
                                                        excelrowSpace2 = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace2.Cells.Add("", DataType.String, "NormalLeft");
                                                        cell.MergeAcross = 16;

                                                        excelrowSpace = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("Domain: " + (dr["SortUnDup_Group"].ToString() == "" ? "" : (dr["SortUnDup_Group"].ToString().Trim() + " ")) + (dr["SortUnDup_Group_Desc"].ToString() == "" ? "" : dr["SortUnDup_Group_Desc"].ToString().Trim()), DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("#Served", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("#Served", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");

                                                        excelrowSpace3 = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("Total", DataType.String, "MainHeaderStyles");

                                                        DataRow[] drHeader = dataTable.Select("Res_Table IS NULL AND Res_Row_Type='GrpHead'");
                                                        if (drHeader.Length > 0)
                                                        {
                                                            DataTable dtHeader = drHeader.CopyToDataTable();

                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed1_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed1_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed2_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed2_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed3_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed3_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed4_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed4_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");

                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed5_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed5_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                        }

                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");

                                                        cell = excelrowSpace3.Cells.Add("Total", DataType.String, "MainHeaderStyles");

                                                        DataRow[] drHeader2 = dataTable.Select("Res_Table IS NULL AND Res_Row_Type='GrpHead'");
                                                        if (drHeader2.Length > 0)
                                                        {
                                                            DataTable dtHeader2 = drHeader2.CopyToDataTable();

                                                            cell = excelrowSpace3.Cells.Add(drHeader2[0]["Res_Hed1_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed1_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader2[0]["Res_Hed2_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed2_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader2[0]["Res_Hed3_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed3_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader2[0]["Res_Hed4_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed4_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");

                                                            cell = excelrowSpace3.Cells.Add(drHeader2[0]["Res_Hed5_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed5_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                        }

                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");

                                                        prevDomain = dr["SortUnDup_Group_Desc"].ToString();

                                                        excelrow = sheet.Table.Rows.Add();

                                                        cell = excelrow.Cells.Add("Group", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");

                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow.Cells.Add("Service Plan", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Outcome", DataType.String, "MainHeaderStyles");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow.Cells.Add("Outcome", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");
                                                        }

                                                        //cell = excelrow.Cells.Add("Outcome", DataType.String, "MainHeaderStyles");

                                                        //cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Hierarchy", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("App#", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Name", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        if (_isAddDate == "MSADDDATE")
                                                            cell = excelrow.Cells.Add("Outcome Add Date", DataType.String, "MainHeaderStyles");
                                                        else if (_isAddDate == "MSDATE")
                                                            cell = excelrow.Cells.Add("Outcome Date", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Ref. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Ref. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Ref. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Ref. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Ref. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Ref. Period", DataType.String, "MainHeaderStyles");

                                                        if (_isAddDate == "MSADDDATE")
                                                            cell = excelrow.Cells.Add("Outcome Add Date", DataType.String, "MainHeaderStyles");
                                                        else if (_isAddDate == "MSDATE")
                                                            cell = excelrow.Cells.Add("Outcome Date", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Modified By", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Intake Site", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Outcome Posting Site", DataType.String, "MainHeaderStyles");
                                                    }

                                                    #endregion

                                                    #region Counts

                                                    if (dr["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Table"].ToString() == "" ? "" : dr["SortUnDup_Table"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        if(dtGroupDesc.Rows.Count > 0) 
                                                        {
                                                            DataRow[] drGrpDesc = dtGroupDesc.Select("Res_Table = '" + disticntOutcomerows["SortUnDup_Table"].ToString() + "'");

                                                            if (drGrpDesc.Length > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(drGrpDesc[0]["Res_Table_Desc"].ToString(), DataType.String, "NormalLeft");
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");


                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_SP_Desc"].ToString() == "" ? "" : dr["SortUnDup_SP_Desc"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_OutcomeCode"].ToString() == "" ? "" : dr["SortUnDup_OutcomeCode"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Goal_Desc"].ToString() == "" ? "" : dr["SortUnDup_Goal_Desc"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Agy"].ToString() + "-" + dr["SortUnDup_Dept"].ToString() + "-" + dr["SortUnDup_Prog"].ToString(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_App"].ToString() == "" ? "" : dr["SortUnDup_App"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Name"].ToString() == "" ? "" : dr["SortUnDup_Name"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        if (dr["SortUnDup_RepDate"].ToString().Trim() != "")
                                                        {
                                                            RepTotals = Convert.ToInt32(dr["R1"].ToString().Trim()) + Convert.ToInt32(dr["R2"].ToString().Trim()) + Convert.ToInt32(dr["R3"].ToString().Trim()) + Convert.ToInt32(dr["R4"].ToString().Trim()) + Convert.ToInt32(dr["R5"].ToString().Trim());

                                                            cell = excelrow2.Cells.Add(RepTotals.ToString() == "" ? "" : RepTotals.ToString().Trim(), DataType.String, "NormalRight");

                                                            cell = excelrow2.Cells.Add(dr["R1"].ToString() == "" ? "" : dr["R1"].ToString().Trim(), DataType.String, "NormalRight");

                                                            RepCnt = RepCnt + (dr["R1"].ToString() == "" ? 0 : Convert.ToInt32(dr["R1"].ToString().Trim()));

                                                            cell = excelrow2.Cells.Add(dr["R2"].ToString() == "" ? "" : dr["R2"].ToString().Trim(), DataType.String, "NormalRight");

                                                            RepCnt1 = RepCnt1 + (dr["R2"].ToString() == "" ? 0 : Convert.ToInt32(dr["R2"].ToString().Trim()));

                                                            cell = excelrow2.Cells.Add(dr["R3"].ToString() == "" ? "" : dr["R3"].ToString().Trim(), DataType.String, "NormalRight");

                                                            RepCnt2 = RepCnt2 + (dr["R3"].ToString() == "" ? 0 : Convert.ToInt32(dr["R3"].ToString().Trim()));

                                                            cell = excelrow2.Cells.Add(dr["R4"].ToString() == "" ? "" : dr["R4"].ToString().Trim(), DataType.String, "NormalRight");

                                                            RepCnt3 = RepCnt3 + (dr["R4"].ToString() == "" ? 0 : Convert.ToInt32(dr["R4"].ToString().Trim()));

                                                            cell = excelrow2.Cells.Add(dr["R5"].ToString() == "" ? "" : dr["R5"].ToString().Trim(), DataType.String, "NormalRight");

                                                            RepCnt4 = RepCnt4 + (dr["R5"].ToString() == "" ? 0 : Convert.ToInt32(dr["R5"].ToString().Trim()));
                                                        }
                                                        else
                                                        {
                                                            //RepTotals = 0;

                                                            cell = excelrow2.Cells.Add("0", DataType.String, "NormalRight");

                                                            cell = excelrow2.Cells.Add("0", DataType.String, "NormalRight");

                                                            //RepCnt = 0;

                                                            cell = excelrow2.Cells.Add("0", DataType.String, "NormalRight");

                                                           // RepCnt1 = 0;

                                                            cell = excelrow2.Cells.Add("0", DataType.String, "NormalRight");

                                                            //RepCnt2 = 0;

                                                            cell = excelrow2.Cells.Add("0", DataType.String, "NormalRight");

                                                            //RepCnt3 = 0;

                                                            cell = excelrow2.Cells.Add("0", DataType.String, "NormalRight");

                                                            //RepCnt4 = 0;
                                                        }

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_RepDate"].ToString() == "" ? "" : Convert.ToDateTime(dr["SortUnDup_RepDate"]).ToString("MM/dd/yyyy"), DataType.String, "NormalRight");

                                                        RefTotals = Convert.ToInt32(dr["R1"].ToString().Trim()) + Convert.ToInt32(dr["R2"].ToString().Trim()) + Convert.ToInt32(dr["R3"].ToString().Trim()) + Convert.ToInt32(dr["R4"].ToString().Trim()) + Convert.ToInt32(dr["R5"].ToString().Trim());

                                                        cell = excelrow2.Cells.Add(RefTotals.ToString() == "" ? "" : RefTotals.ToString().Trim(), DataType.String, "NormalRight");

                                                        cell = excelrow2.Cells.Add(dr["R1"].ToString() == "" ? "" : dr["R1"].ToString().Trim(), DataType.String, "NormalRight");

                                                        RefCnt = RefCnt + (dr["R1"].ToString() == "" ? 0 : Convert.ToInt32(dr["R1"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R2"].ToString() == "" ? "" : dr["R2"].ToString().Trim(), DataType.String, "NormalRight");

                                                        RefCnt1 = RefCnt1 + (dr["R2"].ToString() == "" ? 0 : Convert.ToInt32(dr["R2"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R3"].ToString() == "" ? "" : dr["R3"].ToString().Trim(), DataType.String, "NormalRight");

                                                        RefCnt2 = RefCnt2 + (dr["R3"].ToString() == "" ? 0 : Convert.ToInt32(dr["R3"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R4"].ToString() == "" ? "" : dr["R4"].ToString().Trim(), DataType.String, "NormalRight");

                                                        RefCnt3 = RefCnt3 + (dr["R4"].ToString() == "" ? 0 : Convert.ToInt32(dr["R4"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R5"].ToString() == "" ? "" : dr["R5"].ToString().Trim(), DataType.String, "NormalRight");

                                                        RefCnt4 = RefCnt4 + (dr["R5"].ToString() == "" ? 0 : Convert.ToInt32(dr["R5"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_RefDate"].ToString() == "" ? "" : Convert.ToDateTime(dr["SortUnDup_RefDate"]).ToString("MM/dd/yyyy"), DataType.String, "NormalRight");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_UserId"].ToString() == "" ? "" : dr["SortUnDup_UserId"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Isite"].ToString() == "" ? "" : dr["SortUnDup_Isite"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Psite"].ToString() == "" ? "" : dr["SortUnDup_Psite"].ToString().Trim(), DataType.String, "NormalLeft");
                                                    }

                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Headers

                                                    if (dr["SortUnDup_Table"].ToString().Trim() == "000GrpDesc" && prevDomain != dr["SortUnDup_Group_Desc"].ToString())
                                                    {
                                                        excelrowSpace2 = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace2.Cells.Add("", DataType.String, "NormalLeft");
                                                        cell.MergeAcross = 16;

                                                        excelrowSpace = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("Domain: " + (dr["SortUnDup_Group"].ToString() == "" ? "" : (dr["SortUnDup_Group"].ToString().Trim() + " ")) + (dr["SortUnDup_Group_Desc"].ToString() == "" ? "" : dr["SortUnDup_Group_Desc"].ToString().Trim()), DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("#Served", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");

                                                        excelrowSpace3 = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("Total", DataType.String, "MainHeaderStyles");

                                                        DataRow[] drHeader = dataTable.Select("Res_Table IS NULL AND Res_Row_Type='GrpHead'");
                                                        if (drHeader.Length > 0)
                                                        {
                                                            DataTable dtHeader = drHeader.CopyToDataTable();

                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed1_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed1_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed2_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed2_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed3_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed3_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed4_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed4_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");

                                                            cell = excelrowSpace3.Cells.Add(drHeader[0]["Res_Hed5_Cnt"].ToString().Trim() == "" ? "" : drHeader[0]["Res_Hed5_Cnt"].ToString().Trim(), DataType.String, "MainHeaderStyles");
                                                        }

                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace3.Cells.Add("", DataType.String, "MainHeaderStyles");

                                                        prevDomain = dr["SortUnDup_Group_Desc"].ToString();

                                                        excelrow = sheet.Table.Rows.Add();

                                                        cell = excelrow.Cells.Add("Group", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");

                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow.Cells.Add("Service Plan", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Outcome", DataType.String, "MainHeaderStyles");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow.Cells.Add("Outcome", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");
                                                        }

                                                        //cell = excelrow.Cells.Add("Outcome", DataType.String, "MainHeaderStyles");

                                                        //cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Hierarchy", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("App#", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Name", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        if (_isAddDate == "MSADDDATE")
                                                            cell = excelrow.Cells.Add("Outcome Add Date", DataType.String, "MainHeaderStyles");
                                                        else if (_isAddDate == "MSDATE")
                                                            cell = excelrow.Cells.Add("Outcome Date", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Modified By", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Intake Site", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Outcome Posting Site", DataType.String, "MainHeaderStyles");
                                                    }

                                                    #endregion

                                                    #region Counts

                                                    if (dr["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Table"].ToString() == "" ? "" : dr["SortUnDup_Table"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        if (dtGroupDesc.Rows.Count > 0)
                                                        {
                                                            DataRow[] drGrpDesc = dtGroupDesc.Select("Res_Table = '" + disticntOutcomerows["SortUnDup_Table"].ToString() + "'");

                                                            if (drGrpDesc.Length > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(drGrpDesc[0]["Res_Table_Desc"].ToString(), DataType.String, "NormalLeft");
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_SP_Desc"].ToString() == "" ? "" : dr["SortUnDup_SP_Desc"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_OutcomeCode"].ToString() == "" ? "" : dr["SortUnDup_OutcomeCode"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Goal_Desc"].ToString() == "" ? "" : dr["SortUnDup_Goal_Desc"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Agy"].ToString() + "-" + dr["SortUnDup_Dept"].ToString() + "-" + dr["SortUnDup_Prog"].ToString(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_App"].ToString() == "" ? "" : dr["SortUnDup_App"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Name"].ToString() == "" ? "" : dr["SortUnDup_Name"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        RepTotals = Convert.ToInt32(dr["R1"].ToString().Trim()) + Convert.ToInt32(dr["R2"].ToString().Trim()) + Convert.ToInt32(dr["R3"].ToString().Trim()) + Convert.ToInt32(dr["R4"].ToString().Trim()) + Convert.ToInt32(dr["R5"].ToString().Trim());

                                                        cell = excelrow2.Cells.Add(RepTotals.ToString() == "" ? "" : RepTotals.ToString().Trim(), DataType.String, "NormalRight");

                                                        cell = excelrow2.Cells.Add(dr["R1"].ToString() == "" ? "" : dr["R1"].ToString().Trim(), DataType.String, "NormalRight");
                                                        RepOutcomeTotal = RepOutcomeTotal + (dr["R1"].ToString() == "" ? 0 : Convert.ToInt32(dr["R1"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R2"].ToString() == "" ? "" : dr["R2"].ToString().Trim(), DataType.String, "NormalRight");
                                                        RepOutcomeTotal1 = RepOutcomeTotal1 + (dr["R2"].ToString() == "" ? 0 : Convert.ToInt32(dr["R2"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R3"].ToString() == "" ? "" : dr["R3"].ToString().Trim(), DataType.String, "NormalRight");
                                                        RepOutcomeTotal2 = RepOutcomeTotal2 + (dr["R3"].ToString() == "" ? 0 : Convert.ToInt32(dr["R3"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R4"].ToString() == "" ? "" : dr["R4"].ToString().Trim(), DataType.String, "NormalRight");
                                                        RepOutcomeTotal3 = RepOutcomeTotal3 + (dr["R4"].ToString() == "" ? 0 : Convert.ToInt32(dr["R4"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["R5"].ToString() == "" ? "" : dr["R5"].ToString().Trim(), DataType.String, "NormalRight");
                                                        RepOutcomeTotal4 = RepOutcomeTotal4 + (dr["R5"].ToString() == "" ? 0 : Convert.ToInt32(dr["R5"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_OutCome_Date"].ToString() == "" ? "" : Convert.ToDateTime(dr["SortUnDup_OutCome_Date"]).ToString("MM/dd/yyyy"), DataType.String, "NormalRight");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_UserId"].ToString() == "" ? "" : dr["SortUnDup_UserId"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Isite"].ToString() == "" ? "" : dr["SortUnDup_Isite"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Psite"].ToString() == "" ? "" : dr["SortUnDup_Psite"].ToString().Trim(), DataType.String, "NormalLeft");
                                                    }

                                                    #endregion

                                                }

                                                #endregion
                                            }

                                            #region Last Outcome Totals

                                            if (disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                            {
                                                OutGrop++;
                                                if (OutGrop == distinctOutcomes.Rows.Count)
                                                {
                                                    if (repRefChecked)
                                                    {
                                                        if (disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Outcome Totals: ", DataType.String, "NormalRightServTot");

                                                            RepOutcomeTotals = Convert.ToInt32(RepCnt.ToString()) + Convert.ToInt32(RepCnt1.ToString()) + Convert.ToInt32(RepCnt2.ToString()) + Convert.ToInt32(RepCnt3.ToString()) + Convert.ToInt32(RepCnt4.ToString());

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotals.ToString().Trim() == "" ? "" : RepOutcomeTotals.ToString().Trim(), DataType.String, "NormalRightServTot");

                                                            if (RepCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal = RepGroupTotal + RepCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt1 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal1 = RepGroupTotal1 + RepCnt1;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt2 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal2 = RepGroupTotal2 + RepCnt2;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt3 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal3 = RepGroupTotal3 + RepCnt3;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RepCnt4 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGroupTotal4 = RepGroupTotal4 + RepCnt4;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            RefOutcomeTotals = Convert.ToInt32(RefCnt.ToString()) + Convert.ToInt32(RefCnt1.ToString()) + Convert.ToInt32(RefCnt2.ToString()) + Convert.ToInt32(RefCnt3.ToString()) + Convert.ToInt32(RefCnt4.ToString());

                                                            cell = excelrow2.Cells.Add(RefOutcomeTotals.ToString().Trim() == "" ? "" : RefOutcomeTotals.ToString().Trim(), DataType.String, "NormalRightServTot");

                                                            if (RefCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal = RefGroupTotal + RefCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt1 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal1 = RefGroupTotal1 + RefCnt1;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt2 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal2 = RefGroupTotal2 + RefCnt2;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt3 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal3 = RefGroupTotal3 + RefCnt3;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt4 > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt4.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGroupTotal4 = RefGroupTotal4 + RefCnt4;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 23;
                                                        }
                                                    }

                                                    if (!repRefChecked)
                                                    {
                                                        if (disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Outcome Totals: ", DataType.String, "NormalRightServTot");

                                                            RepOutcomeTotals = Convert.ToInt32(RepOutcomeTotal.ToString()) + Convert.ToInt32(RepOutcomeTotal1.ToString()) + Convert.ToInt32(RepOutcomeTotal2.ToString()) + Convert.ToInt32(RepOutcomeTotal3.ToString()) + Convert.ToInt32(RepOutcomeTotal4.ToString());

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotals.ToString().Trim() == "" ? "" : RepOutcomeTotals.ToString().Trim(), DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal = RepGroupTotal + RepOutcomeTotal;

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal1.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal1 = RepGroupTotal1 + RepOutcomeTotal1;

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal2.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal2 = RepGroupTotal2 + RepOutcomeTotal2;

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal3.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal3 = RepGroupTotal3 + RepOutcomeTotal3;

                                                            cell = excelrow2.Cells.Add(RepOutcomeTotal4.ToString(), DataType.String, "NormalRightServTot");
                                                            RepGroupTotal4 = RepGroupTotal4 + RepOutcomeTotal4;

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 16;
                                                        }

                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Last Group Totals

                                            if (disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && disticntOutcomerows["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                            {
                                                GrpGrp++;
                                                if (GrpGrp == distinctOutcomes.Rows.Count)
                                                {
                                                    if (repRefChecked)
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("Group Totals: ", DataType.String, "NormalRightGrpTot");

                                                        RepGroupTotals = Convert.ToInt32(RepGroupTotal.ToString()) + Convert.ToInt32(RepGroupTotal1.ToString()) + Convert.ToInt32(RepGroupTotal2.ToString()) + Convert.ToInt32(RepGroupTotal3.ToString()) + Convert.ToInt32(RepGroupTotal4.ToString());

                                                        cell = excelrow2.Cells.Add(RepGroupTotals.ToString().Trim() == "" ? "" : RepGroupTotals.ToString().Trim(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal1.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal2.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal3.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal4.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalRightGrpTot");

                                                        RefGroupTotals = Convert.ToInt32(RefGroupTotal.ToString()) + Convert.ToInt32(RefGroupTotal1.ToString()) + Convert.ToInt32(RefGroupTotal2.ToString()) + Convert.ToInt32(RefGroupTotal3.ToString()) + Convert.ToInt32(RefGroupTotal4.ToString());

                                                        cell = excelrow2.Cells.Add(RefGroupTotals.ToString().Trim() == "" ? "" : RefGroupTotals.ToString().Trim(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RefGroupTotal.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RefGroupTotal1.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RefGroupTotal2.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RefGroupTotal3.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RefGroupTotal4.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                    }

                                                    if (!repRefChecked)
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("Group Total: ", DataType.String, "NormalRightGrpTot");

                                                        RepGroupTotals = Convert.ToInt32(RepGroupTotal.ToString()) + Convert.ToInt32(RepGroupTotal1.ToString()) + Convert.ToInt32(RepGroupTotal2.ToString()) + Convert.ToInt32(RepGroupTotal3.ToString()) + Convert.ToInt32(RepGroupTotal4.ToString());

                                                        cell = excelrow2.Cells.Add(RepGroupTotals.ToString().Trim() == "" ? "" : RepGroupTotals.ToString().Trim(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal1.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal2.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal3.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGroupTotal4.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            prevdistValue = disticntDomainrows["SortUnDup_Group"].ToString().Trim();
                        }
                    }
                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);
                book.Save(stream);
                stream.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }
            }
            catch (Exception ex)
            {
                excelrow2 = sheet.Table.Rows.Add();
                cell = excelrow2.Cells.Add("Aborted due to Exception...", DataType.String, "NormalLeft");

                FileStream stream1 = new FileStream(PdfName, FileMode.Create);
                book.Save(stream1);
                stream1.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name1 = fiDownload.Name;
                using (FileStream fileStream1 = fiDownload.OpenRead())
                {
                    Application.Download(fileStream1, name1);
                }
            }
        }

        string Random_Filename = string.Empty;
        private void GenerateServicesDetailReport(DataTable dataTable, DataTable dtResult, DataTable dtSummary, string pdfName, string propReportPath, string userID, bool bothChecked, string ServTargetChceked)
        {
            string PdfName = "RNGS0014_Detail_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = propReportPath + userID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + userID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + userID.Trim());
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
                PdfName = PdfName + ".xls";

            WorksheetRow excelrowSpace;
            WorksheetRow excelrowSpace2;
            WorksheetRow excelrowSpace3;
            WorksheetRow excelrow;
            WorksheetRow excelrow2;
            WorksheetCell cell;

            Workbook book = new Workbook();

            Worksheet sheet;

            #region Styles
            WorksheetStyle mainstyle = book.Styles.Add("MainHeaderStyles");
            mainstyle.Font.FontName = "Calibri";
            mainstyle.Font.Size = 11;//12;
            mainstyle.Font.Bold = true;
            mainstyle.Font.Color = "#2c2c2c";
            mainstyle.Interior.Color = "#d2e1ef";//"#0070c0";
            mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;
            mainstyle.Alignment.WrapText = true;

            WorksheetStyle mainstyle2 = book.Styles.Add("MainHeaderStyles2");
            mainstyle2.Font.FontName = "Calibri";
            mainstyle2.Font.Size = 12;
            mainstyle2.Font.Bold = true;
            mainstyle2.Font.Color = "#FFFFFF";
            mainstyle2.Interior.Color = "#0070c0";
            mainstyle2.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyle2.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            mainstyle2.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Calibri";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style1.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle stylecenter = book.Styles.Add("Normalcenter");
            stylecenter.Font.FontName = "Calibri";
            stylecenter.Font.Bold = true;
            stylecenter.Font.Size = 10;
            stylecenter.Font.Bold = true;
            stylecenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            stylecenter.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style3 = book.Styles.Add("NormalLeft");
            style3.Font.FontName = "Calibri";
            style3.Font.Size = 10;
            style3.Interior.Color = "#f2f2f2";
            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style3.Alignment.Vertical = StyleVerticalAlignment.Top;
            style3.Alignment.WrapText = true;

            WorksheetStyle style4 = book.Styles.Add("NormalLeftRed");
            style4.Font.FontName = "Calibri";
            style4.Font.Size = 10;
            style4.Interior.Color = "#f2f2f2";
            style4.Font.Color = "#f00808";
            style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style4.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style5 = book.Styles.Add("NormalRight");
            style5.Font.FontName = "Calibri";
            style5.Font.Size = 10;
            style5.Interior.Color = "#f2f2f2";
            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style5.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style55 = book.Styles.Add("NormalRightServTot");
            style55.Font.FontName = "Calibri";
            style55.Font.Size = 10;
            style55.Font.Bold = true;
            style55.Interior.Color = "#ffe8bd";
            style55.Interior.Pattern = StyleInteriorPattern.Solid;
            style55.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style55.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style555 = book.Styles.Add("NormalRightGrpTot");
            style555.Font.FontName = "Calibri";
            style555.Font.Size = 10;
            style555.Font.Bold = true;
            style555.Interior.Color = "#e0ebd8";
            style555.Interior.Pattern = StyleInteriorPattern.Solid;
            style555.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style555.Alignment.Vertical = StyleVerticalAlignment.Top;

            WorksheetStyle style6 = book.Styles.Add("NormalRightRed");
            style6.Font.FontName = "Calibri";
            style6.Font.Size = 10;
            style6.Interior.Color = "#f2f2f2";
            style6.Font.Color = "#f00808";
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style6.Alignment.Vertical = StyleVerticalAlignment.Top;

            #endregion

            /*Worksheet*/
            sheet = book.Worksheets.Add("Data");
            //**sheet.Table.DefaultRowHeight = 16F;//14.25F;

            sheet.Table.DefaultColumnWidth = 220.5F;

            if (bothChecked)
            {
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(300);//(200);
                if (_isServPlanTarget == "Y")
                    sheet.Table.Columns.Add(300);
                else
                    sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(500);//(300);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(70);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
            }
            else
            {
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(300);//(200);
                if (_isServPlanTarget == "Y")
                    sheet.Table.Columns.Add(300);
                else
                    sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(550);//(300);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(70);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
            }


            try
            {
                int RepCnt = 0, RefCnt = 0, RepGrpTot = 0, RefGrpTot = 0, RepServTotal = 0;
                excelrowSpace = sheet.Table.Rows.Add();
                cell = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell.MergeAcross = 12;

                string _hasIndicator = "";
                if (dtResult.Rows.Count > 0)
                {
                    /*******************************************************************************/
                    /*DataRow[] drhasIndicator = dtResult.Select("SortUnDup_Table NOT IN('000GrpDesc' , '001GrpDesc', 'ZZZGrpDesc')");
                    if (drhasIndicator.Length > 0)
                    {
                        DataTable dtR = drhasIndicator.CopyToDataTable();
                        _hasIndicator = drhasIndicator[0]["SortUnDup_Count_Indicator"].ToString();
                    }*/


                    DataView view = new DataView(dtResult);
                    DataTable distinctValues = view.ToTable(true, "SortUnDup_Group");

                    string prevdistValue = string.Empty;
                    string prevDomain = string.Empty;
                    string TotalServices = string.Empty;
                    string prevService = string.Empty;
                    string prevGroup = string.Empty;

                    foreach (DataRow drdis in distinctValues.Rows)
                    {
                        if (prevdistValue != drdis["SortUnDup_Group"].ToString().Trim())
                        {
                            DataRow[] dr1 = dtResult.Select("SortUnDup_Group = '" + drdis["SortUnDup_Group"] + "'");

                            if (dr1.Length > 0)
                            {

                                DataTable dt1 = new DataTable();
                                dt1 = dr1.CopyToDataTable();

                                RepCnt = 0;
                                RefCnt = 0;
                                RepGrpTot = 0;
                                RefGrpTot = 0;
                                RepServTotal = 0;

                                if (dt1.Rows.Count > 3)
                                {
                                    //----------------------------------- GET REPT TOTAL & REFF TOTAL ---------------------------------------------//

                                    DataRow[] drRepCnt = dataTable.Select("Res_Group='" + drdis["SortUnDup_Group"] + "' AND Res_Row_Type='TblDetails' AND Res_Count_Type = 'PrdCnt'");
                                    DataRow[] drReffCnt = dataTable.Select("Res_Group='" + drdis["SortUnDup_Group"] + "' AND Res_Row_Type='TblDetails' AND Res_Count_Type = 'RefCnt'");

                                    DataRow[] drGroupDesc;
                                    if (summarybyGroup)
                                    {
                                        drGroupDesc = dataTable.Select("Res_Group='" + drdis["SortUnDup_Group"] + "' AND Res_Row_Type='TblDetails'");
                                    }
                                    else
                                    {
                                       drGroupDesc = dataTable.Select("Res_Group='" + drdis["SortUnDup_Group"] + "' AND Res_Row_Type='TblDesc'");
                                    }

                                    DataTable dtGroupDesc = new DataTable();

                                    if (drGroupDesc.Length > 0)
                                    {
                                        dtGroupDesc = drGroupDesc.CopyToDataTable();
                                    }

                                    DataTable dtRep = new DataTable();

                                    if (drRepCnt.Length > 0)
                                    {
                                        dtRep = drRepCnt.CopyToDataTable();
                                    }

                                    DataTable dtRef = new DataTable();

                                    if (drReffCnt.Length > 0)
                                    {
                                        dtRef = drReffCnt.CopyToDataTable();
                                    }

                                    //--------------------------------------------------------------------------------------------------------------//

                                    DataView dvServgrp = new DataView(dt1);
                                    DataTable dtServDist = dvServgrp.ToTable(true, "SortUnDup_Table");
                                    int srvGrop = 3;
                                    int GrpGrp = 3;

                                    foreach (DataRow drServgrp in dtServDist.Rows)
                                    {

                                        DataRow[] drServices = dt1.Select("SortUnDup_Table='" + drServgrp["SortUnDup_Table"].ToString() + "'");

                                        if (drServices.Length > 0)
                                        {
                                            DataTable dtSerRes = drServices.CopyToDataTable();

                                            DataView dv = new DataView(dtSerRes);
                                            dv.Sort = "SortUnDup_Group,SortUnDup_Table,SortUnDup_OutcomeCode";

                                            dtSerRes = dv.ToTable();

                                            bool isService = false; 
                                            int servcnt = 0;

                                            foreach (DataRow dr in dtSerRes.Rows)
                                            {

                                                #region Service Totals

                                                // servcnt++;
                                                if (prevService != dr["SortUnDup_OutcomeCode"].ToString() && dr["SortUnDup_OutcomeCode"].ToString().Trim() != "" && prevService.Trim() != "" && prevService.Trim() != "Service")
                                                {
                                                    if (bothChecked)
                                                    {
                                                        if (drServgrp["SortUnDup_Table"].ToString() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Service Totals: ", DataType.String, "NormalRightServTot");

                                                            if (RepCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGrpTot = RepGrpTot + RepCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGrpTot = RefGrpTot + RefCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 12;
                                                        }
                                                        RepCnt = 0;
                                                        RefCnt = 0;
                                                    }

                                                    if (!bothChecked)
                                                    {
                                                        if (drServgrp["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Service Total: ", DataType.String, "NormalRightServTot");

                                                            cell = excelrow2.Cells.Add(RepServTotal.ToString(), DataType.String, "NormalRightServTot");

                                                            RepGrpTot = RepGrpTot + RepServTotal;

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 10;

                                                            //isService = true;
                                                            //servcnt++;
                                                        }
                                                        RepServTotal = 0;
                                                    }
                                                }

                                                prevService = dr["SortUnDup_OutcomeCode"].ToString();

                                                #endregion

                                                #region Group Totals

                                                //if (servcnt == drServices.Length)
                                                //{
                                                    if (prevGroup != dr["SortUnDup_Table"].ToString().Trim() && dr["SortUnDup_Table"].ToString().Trim() != "" && prevGroup.Trim() != "")
                                                    {
                                                        if (bothChecked && (RepGrpTot != 0 || RefGrpTot != 0))
                                                        {
                                                            if (drServgrp["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                            {
                                                                excelrow2 = sheet.Table.Rows.Add();

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("Group Totals: ", DataType.String, "NormalRightGrpTot");

                                                                cell = excelrow2.Cells.Add(RepGrpTot.ToString(), DataType.String, "NormalRightGrpTot");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightGrpTot");

                                                                cell = excelrow2.Cells.Add(RefGrpTot.ToString(), DataType.String, "NormalRightGrpTot");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                excelrowSpace3 = sheet.Table.Rows.Add();
                                                                cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                                cell.MergeAcross = 12;
                                                            }
                                                            RepGrpTot = RefGrpTot = 0;
                                                        }
                                                        if (!bothChecked && RepGrpTot != 0)
                                                        {
                                                            if (drServgrp["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                            {
                                                                excelrow2 = sheet.Table.Rows.Add();

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("Group Total: ", DataType.String, "NormalRightGrpTot");

                                                                cell = excelrow2.Cells.Add(RepGrpTot.ToString(), DataType.String, "NormalRightGrpTot");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                                excelrowSpace3 = sheet.Table.Rows.Add();
                                                                cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                                cell.MergeAcross = 10;
                                                            }
                                                            RepGrpTot = 0;
                                                        }
                                                    }
                                                    prevGroup = dr["SortUnDup_Table"].ToString().Trim();
                                                //}

                                                #endregion


                                                #region Details

                                                if (bothChecked)
                                                {
                                                    if (dr["SortUnDup_Table"].ToString().Trim() == "000GrpDesc" && prevDomain != dr["SortUnDup_Group_Desc"].ToString())
                                                    {
                                                        excelrowSpace2 = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace2.Cells.Add("", DataType.String, "NormalLeft");
                                                        cell.MergeAcross = 12;

                                                        excelrowSpace = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("Domain: " + (dr["SortUnDup_Group"].ToString() == "" ? "" : (dr["SortUnDup_Group"].ToString().Trim() + " ")) + (dr["SortUnDup_Group_Desc"].ToString() == "" ? "" : dr["SortUnDup_Group_Desc"].ToString().Trim()), DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("#Served", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("#Served", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");

                                                        prevDomain = dr["SortUnDup_Group_Desc"].ToString();

                                                        excelrow = sheet.Table.Rows.Add();

                                                        cell = excelrow.Cells.Add("Group", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");

                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow.Cells.Add("Service Plan", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Service", DataType.String, "MainHeaderStyles");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow.Cells.Add("Service", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");
                                                        }
                                                        
                                                        cell = excelrow.Cells.Add("Hierarchy", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("App#", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Name", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        if(_isAddDate == "CAADDDATE")
                                                            cell = excelrow.Cells.Add("Service Add Date", DataType.String, "MainHeaderStyles");
                                                        else if (_isAddDate == "CADATE")
                                                            cell = excelrow.Cells.Add("Service Date", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Ref. Period", DataType.String, "MainHeaderStyles");

                                                        if (_isAddDate == "CAADDDATE")
                                                            cell = excelrow.Cells.Add("Service Add Date", DataType.String, "MainHeaderStyles");
                                                        else if (_isAddDate == "CADATE")
                                                            cell = excelrow.Cells.Add("Service Date", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Intake Site", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Service Posting Site", DataType.String, "MainHeaderStyles");
                                                    }

                                                    if (dr["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Table"].ToString() == "" ? "" : dr["SortUnDup_Table"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        DataRow[] drGrpDesc = dtGroupDesc.Select("Res_Table = '" + drServgrp["SortUnDup_Table"].ToString() + "'");

                                                        if (drGrpDesc.Length > 0)
                                                        {
                                                            cell = excelrow2.Cells.Add(drGrpDesc[0]["Res_Table_Desc"].ToString(), DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        //**cell = excelrow2.Cells.Add(dr["SortUnDup_Group_Desc"].ToString() == "" ? "" : dr["SortUnDup_Group_Desc"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_SP_Desc"].ToString() == "" ? "" : dr["SortUnDup_SP_Desc"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_OutcomeCode"].ToString() == "" ? "" : dr["SortUnDup_OutcomeCode"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Goal_Desc"].ToString() == "" ? "" : dr["SortUnDup_Goal_Desc"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Agy"].ToString() + "-" + dr["SortUnDup_Dept"].ToString() + "-" + dr["SortUnDup_Prog"].ToString(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_App"].ToString() == "" ? "" : dr["SortUnDup_App"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Name"].ToString() == "" ? "" : dr["SortUnDup_Name"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_RepCount"].ToString() == "" ? "" : dr["SortUnDup_RepCount"].ToString().Trim(), DataType.String, "NormalRight");

                                                        RepCnt = RepCnt + (dr["SortUnDup_RepCount"].ToString() == "" ? 0 : Convert.ToInt32(dr["SortUnDup_RepCount"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_RepDate"].ToString() == "" ? "" : Convert.ToDateTime(dr["SortUnDup_RepDate"]).ToString("MM/dd/yyyy"), DataType.String, "NormalRight");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_RefCount"].ToString() == "" ? "" : dr["SortUnDup_RefCount"].ToString().Trim(), DataType.String, "NormalRight");

                                                        RefCnt = RefCnt + (dr["SortUnDup_RefCount"].ToString() == "" ? 0 : Convert.ToInt32(dr["SortUnDup_RefCount"].ToString().Trim()));

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_RefDate"].ToString() == "" ? "" : Convert.ToDateTime(dr["SortUnDup_RefDate"]).ToString("MM/dd/yyyy"), DataType.String, "NormalRight");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Isite"].ToString() == "" ? "" : dr["SortUnDup_Isite"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Psite"].ToString() == "" ? "" : dr["SortUnDup_Psite"].ToString().Trim(), DataType.String, "NormalLeft");
                                                    }
                                                }

                                                if (!bothChecked)
                                                {
                                                    if (dr["SortUnDup_Table"].ToString().Trim() == "000GrpDesc" && prevDomain != dr["SortUnDup_Group_Desc"].ToString())
                                                    {
                                                        excelrowSpace2 = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace2.Cells.Add("", DataType.String, "NormalLeft");
                                                        cell.MergeAcross = 10;

                                                        excelrowSpace = sheet.Table.Rows.Add();
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("Domain: " + (dr["SortUnDup_Group"].ToString() == "" ? "" : (dr["SortUnDup_Group"].ToString().Trim() + " ")) + (dr["SortUnDup_Group_Desc"].ToString() == "" ? "" : dr["SortUnDup_Group_Desc"].ToString().Trim()), DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("#Served", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");
                                                        cell = excelrowSpace.Cells.Add("", DataType.String, "MainHeaderStyles");

                                                        prevDomain = dr["SortUnDup_Group_Desc"].ToString();

                                                        excelrow = sheet.Table.Rows.Add();

                                                        cell = excelrow.Cells.Add("Group", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");

                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow.Cells.Add("Service Plan", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Service", DataType.String, "MainHeaderStyles");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow.Cells.Add("Service", DataType.String, "MainHeaderStyles");
                                                            cell = excelrow.Cells.Add("Description", DataType.String, "MainHeaderStyles");
                                                        }

                                                        cell = excelrow.Cells.Add("Hierarchy", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("App#", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Name", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Rept. Period", DataType.String, "MainHeaderStyles");

                                                        if (_isAddDate == "CAADDDATE")
                                                            cell = excelrow.Cells.Add("Service Add Date", DataType.String, "MainHeaderStyles");
                                                        else if(_isAddDate == "CADATE")
                                                            cell = excelrow.Cells.Add("Service Date", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Intake Site", DataType.String, "MainHeaderStyles");

                                                        cell = excelrow.Cells.Add("Service Posting Site", DataType.String, "MainHeaderStyles");
                                                    }

                                                    if (dr["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && dr["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Table"].ToString() == "" ? "" : dr["SortUnDup_Table"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        DataRow[] drGrpDesc = dtGroupDesc.Select("Res_Table = '" + drServgrp["SortUnDup_Table"].ToString() + "'");

                                                        if (drGrpDesc.Length > 0)
                                                        {
                                                            cell = excelrow2.Cells.Add(drGrpDesc[0]["Res_Table_Desc"].ToString(), DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");


                                                        //**cell = excelrow2.Cells.Add(dr["SortUnDup_Group_Desc"].ToString() == "" ? "" : dr["SortUnDup_Group_Desc"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        if (ServTargetChceked == "Y")
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_SP_Desc"].ToString() == "" ? "" : dr["SortUnDup_SP_Desc"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }
                                                        else
                                                        {
                                                            cell = excelrow2.Cells.Add(dr["SortUnDup_OutcomeCode"].ToString() == "" ? "" : dr["SortUnDup_OutcomeCode"].ToString().Trim(), DataType.String, "NormalLeft");
                                                        }

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Goal_Desc"].ToString() == "" ? "" : dr["SortUnDup_Goal_Desc"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Agy"].ToString() + "-" + dr["SortUnDup_Dept"].ToString() + "-" + dr["SortUnDup_Prog"].ToString(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_App"].ToString() == "" ? "" : dr["SortUnDup_App"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Name"].ToString() == "" ? "" : dr["SortUnDup_Name"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("1", DataType.String, "NormalRight");

                                                        RepServTotal = RepServTotal + 1;

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_OutCome_Date"].ToString() == "" ? "" : Convert.ToDateTime(dr["SortUnDup_OutCome_Date"]).ToString("MM/dd/yyyy"), DataType.String, "NormalRight");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Isite"].ToString() == "" ? "" : dr["SortUnDup_Isite"].ToString().Trim(), DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add(dr["SortUnDup_Psite"].ToString() == "" ? "" : dr["SortUnDup_Psite"].ToString().Trim(), DataType.String, "NormalLeft");
                                                    }
                                                }

                                                #endregion
                                            }

                                            #region Last Service Totals

                                            if (drServgrp["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                            {
                                                srvGrop++;
                                                if (srvGrop == dtServDist.Rows.Count)
                                                {
                                                    if (bothChecked)
                                                    {
                                                        if (drServgrp["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Service Totals: ", DataType.String, "NormalRightServTot");

                                                            if (RepCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RepCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RepGrpTot = RepGrpTot + RepCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            /*DataRow[] drServTot = dtRep.Select("Res_Table = '" + drServgrp["SortUnDup_Table"].ToString() + "' AND TRIM(Res_Outcome_code) = '" + prevService.Trim() + "'");

                                                            if (drServTot.Length > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(drServTot[0]["Res_Unit_Cnt"].ToString(), DataType.String, "NormalRightServTot");

                                                                RepGrpTot = RepGrpTot + Convert.ToInt32(drServTot[0]["Res_Unit_Cnt"].ToString());
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");*/

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");

                                                            if (RefCnt > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(RefCnt.ToString(), DataType.String, "NormalRightServTot");
                                                                RefGrpTot = RefGrpTot + RefCnt;
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");
                                                            /*DataRow[] drServRefTot = dtRef.Select("Res_Table = '" + drServgrp["SortUnDup_Table"].ToString() + "' AND TRIM(Res_Table_Desc) = '" + prevService.Trim() + "'");

                                                            if (drServRefTot.Length > 0)
                                                            {
                                                                cell = excelrow2.Cells.Add(drServRefTot[0]["Res_Unit_Cnt"].ToString(), DataType.String, "NormalRightServTot");

                                                                RefGrpTot = RefGrpTot + Convert.ToInt32(drServRefTot[0]["Res_Unit_Cnt"].ToString());
                                                            }
                                                            else
                                                                cell = excelrow2.Cells.Add("", DataType.String, "NormalRightServTot");*/


                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 12;
                                                        }
                                                    }

                                                    if (!bothChecked)
                                                    {
                                                        if (drServgrp["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                                        {
                                                            excelrow2 = sheet.Table.Rows.Add();

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("Service Total: ", DataType.String, "NormalRightServTot");

                                                            //DataRow[] drServTot = dtRep.Select("Res_Table = '" + drServgrp["SortUnDup_Table"].ToString() + "' AND TRIM(Res_Table_Desc) = '" + prevService.Trim() + "'");
                                                            //DataRow[] drServTot = dtRep.Select("Res_Table = '" + drServgrp["SortUnDup_Table"].ToString() + "' AND TRIM(Res_Outcome_code) = '" + prevService.Trim() + "'");

                                                            //if (drServTot.Length > 0)
                                                            //{
                                                            //    cell = excelrow2.Cells.Add(drServTot[0]["Res_Unit_Cnt"].ToString(), DataType.String, "NormalRightServTot");

                                                            //    RepGrpTot = RepGrpTot + Convert.ToInt32(drServTot[0]["Res_Unit_Cnt"].ToString());
                                                            //}
                                                            //else
                                                            cell = excelrow2.Cells.Add(RepServTotal.ToString(), DataType.String, "NormalRightServTot");

                                                            RepGrpTot = RepGrpTot + RepServTotal;

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                            excelrowSpace3 = sheet.Table.Rows.Add();
                                                            cell = excelrowSpace3.Cells.Add("", DataType.String, "NormalLeft");
                                                            cell.MergeAcross = 10;
                                                        }

                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Last Group Totals

                                            if (drServgrp["SortUnDup_Table"].ToString().Trim() != "000GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "001GrpDesc" && drServgrp["SortUnDup_Table"].ToString().Trim() != "ZZZGrpDesc")
                                            {
                                                GrpGrp++;
                                                if (GrpGrp == dtServDist.Rows.Count)
                                                {

                                                    if (bothChecked)
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("Group Totals: ", DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGrpTot.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RefGrpTot.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");
                                                    }
                                                    if (!bothChecked)
                                                    {
                                                        excelrow2 = sheet.Table.Rows.Add();

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("Group Total: ", DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add(RepGrpTot.ToString(), DataType.String, "NormalRightGrpTot");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                                                        cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");
                                                    }
                                                }
                                            }

                                            #endregion
                                        }
                                    }
                                }
                            }
                            prevdistValue = drdis["SortUnDup_Group"].ToString().Trim();
                        }
                    }
                }
                
                FileStream stream = new FileStream(PdfName, FileMode.Create);
                book.Save(stream);
                stream.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }
            }
            catch (Exception ex)
            {
                excelrow2 = sheet.Table.Rows.Add();
                cell = excelrow2.Cells.Add("Aborted due to Exception...", DataType.String, "NormalLeft");

                FileStream stream1 = new FileStream(PdfName, FileMode.Create);
                book.Save(stream1);
                stream1.Close();
            }

        }

        private void GenerateBypassReport(DataTable dtResult,DataTable dtSummary,string pdfName, string propReportPath, string userID)
        {
            string PdfName = "RNGS0004_Bypass_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = propReportPath + userID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + userID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + userID.Trim());
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
                PdfName = PdfName + ".xls";

            Workbook book = new Workbook();

            #region Stylesheet Options

            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = book.Styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s15
            // -----------------------------------------------
            WorksheetStyle s15 = book.Styles.Add("s15");
            s15.Font.FontName = "Calibri";
            s15.Font.Size = 11;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = book.Styles.Add("s16");
            s16.Font.Bold = true;
            s16.Font.FontName = "Calibri";
            s16.Font.Color = "#000000";
            s16.Font.Size = 12;
            s16.Interior.Color = "#B0C4DE";
            s16.Interior.Pattern = StyleInteriorPattern.Solid;
            s16.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s16.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16.Alignment.WrapText = true;
            s16.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s16.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s16L
            // -----------------------------------------------
            WorksheetStyle s16L = book.Styles.Add("s16L");
            s16L.Font.Bold = true;
            s16L.Font.FontName = "Calibri";//"Arial";
            s16L.Font.Color = "#000000";
            s16L.Font.Size = 12;
            s16L.Interior.Color = "#B0C4DE";
            s16L.Interior.Pattern = StyleInteriorPattern.Solid;
            s16L.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s16L.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16L.Alignment.WrapText = true;
            s16L.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s16L.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = book.Styles.Add("s17");
            s17.Font.FontName = "Calibri";
            s17.Font.Color = "#000000";
            s17.Font.Size = 11;
            s17.Interior.Color = "#FFFFFF";
            s17.Interior.Pattern = StyleInteriorPattern.Solid;
            s17.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s17.Alignment.Vertical = StyleVerticalAlignment.Top;
            s17.Alignment.WrapText = true;
            s17.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s17C
            // -----------------------------------------------
            WorksheetStyle s17C = book.Styles.Add("s17C");
            s17C.Font.FontName = "Calibri";
            s17C.Font.Color = "#000000";
            s17C.Font.Size = 11;
            s17C.Interior.Color = "#FFFFFF";
            s17C.Interior.Pattern = StyleInteriorPattern.Solid;
            s17C.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s17C.Alignment.Vertical = StyleVerticalAlignment.Top;
            s17C.Alignment.WrapText = true;
            s17C.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s17R
            // -----------------------------------------------
            WorksheetStyle s17R = book.Styles.Add("s17R");
            s17R.Font.FontName = "Calibri";
            s17R.Font.Color = "#FF0000";
            s17R.Font.Size = 11;
            s17R.Interior.Color = "#FFFFFF";
            s17R.Interior.Pattern = StyleInteriorPattern.Solid;
            s17R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s17R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s17R.Alignment.WrapText = true;
            s17R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = book.Styles.Add("s18");
            s18.Font.FontName = "Calibri";
            s18.Font.Color = "#000000";
            s18.Font.Size = 11;
            s18.Interior.Color = "#FFFFFF";
            s18.Interior.Pattern = StyleInteriorPattern.Solid;
            s18.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s18.Alignment.Vertical = StyleVerticalAlignment.Top;
            s18.Alignment.WrapText = true;
            s18.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = book.Styles.Add("s19");
            s19.Font.FontName = "Calibri";
            s19.Font.Size = 11;
            s19.Interior.Color = "#FFFFFF";
            s19.Interior.Pattern = StyleInteriorPattern.Solid;
            s19.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19.Alignment.WrapText = true;


            #endregion


            Worksheet sheet = book.Worksheets.Add("ByPass_Report");
            sheet.Table.DefaultRowHeight = 15F;
            //sheet.Table.ExpandedColumnCount = 14;
            //sheet.Table.ExpandedRowCount = 350;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "s15";

            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 45;//18;
            column0.StyleID = "s15";
            //column0.Span = 2;
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            //column1.Index = 4;
            column1.Width = 70;//36;
            column1.StyleID = "s15";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 50;//49;
            column2.StyleID = "s15";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 40;//162;
            column3.StyleID = "s15";
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Width = 65;//64;
            column4.StyleID = "s15";
            //column4.Span = 1;
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            //column5.Index = 9;
            column5.Width = 200;//36;
            column5.StyleID = "s15";
            WorksheetColumn column6 = sheet.Table.Columns.Add();
            column6.Width = 70;//64;
            column6.StyleID = "s15";
            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 70;//122;
            column7.StyleID = "s15";
            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 60;//165;
            column8.StyleID = "s15";
            WorksheetColumn column9 = sheet.Table.Columns.Add();
            column9.Width = 150;//79;
            column9.StyleID = "s15";
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Width = 300;//64;
            column10.StyleID = "s15";
            WorksheetColumn column11 = sheet.Table.Columns.Add();
            column11.Width = 150;//122;
            column11.StyleID = "s15";
            WorksheetColumn column12 = sheet.Table.Columns.Add();
            column12.Width = 80;//165;
            column12.StyleID = "s15";
            WorksheetColumn column13 = sheet.Table.Columns.Add();
            column13.Width = 80;//79;
            column13.StyleID = "s15";

            /*WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 18;
            column0.StyleID = "s15";
            column0.Span = 2;
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Index = 4;
            column1.Width = 36;
            column1.StyleID = "s15";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 49;
            column2.StyleID = "s15";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 162;
            column3.StyleID = "s15";
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Width = 64;
            column4.StyleID = "s15";
            column4.Span = 1;
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            column5.Index = 9;
            column5.Width = 36;
            column5.StyleID = "s15";
            WorksheetColumn column6 = sheet.Table.Columns.Add();
            column6.Width = 64;
            column6.StyleID = "s15";
            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 122;
            column7.StyleID = "s15";
            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 165;
            column8.StyleID = "s15";
            WorksheetColumn column9 = sheet.Table.Columns.Add();
            column9.Width = 79;
            column9.StyleID = "s15";
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Width = 64;
            column10.StyleID = "s15";*/
            // -----------------------------------------------

            try
            {
                WorksheetRow Row0 = sheet.Table.Rows.Add();
                Row0.Cells.Add("Agency", DataType.String, "s16");
                Row0.Cells.Add("Department", DataType.String, "s16");
                Row0.Cells.Add("Program", DataType.String, "s16");
                Row0.Cells.Add("Year", DataType.String, "s16");
                Row0.Cells.Add("App#", DataType.String, "s16L");
                Row0.Cells.Add("Client Name", DataType.String, "s16L");
                Row0.Cells.Add("Family ID", DataType.String, "s16L");
                Row0.Cells.Add("Client ID", DataType.String, "s16L");
                Row0.Cells.Add("Site", DataType.String, "s16L");
                Row0.Cells.Add("Data Field", DataType.String, "s16L");
                Row0.Cells.Add("Response", DataType.String, "s16L");
                Row0.Cells.Add("Exclusion Reason", DataType.String, "s16L");
                Row0.Cells.Add("Updated Date", DataType.String, "s16L");
                Row0.Cells.Add("Updated By", DataType.String, "s16L");

                foreach (DataRow dr in dtResult.Rows)
                {
                    Row0 = sheet.Table.Rows.Add();
                    Row0.Height = 30;
                    Row0.Cells.Add(dr["Byp_Agy"].ToString(), DataType.String, "s17C");
                    Row0.Cells.Add(dr["Byp_Dept"].ToString(), DataType.String, "s17C");
                    Row0.Cells.Add(dr["Byp_Prog"].ToString(), DataType.String, "s17C");
                    Row0.Cells.Add(dr["Byp_Year"].ToString(), DataType.String, "s17C");
                    //WorksheetCell cell;
                    //cell = Row0.Cells.Add();
                    //cell.StyleID = "s17";
                    //cell.Data.Type = DataType.String;
                    Row0.Cells.Add(dr["Byp_App"].ToString(), DataType.String, "s17");
                    Row0.Cells.Add(dr["Byp_Client_Name"].ToString(), DataType.String, "s17");
                    Row0.Cells.Add(dr["Byp_Fam_ID"].ToString(), DataType.String, "s17");
                    Row0.Cells.Add(dr["Byp_CLID"].ToString(), DataType.String, "s17");
                    Row0.Cells.Add(dr["Byp_Site"].ToString(), DataType.String, "s17");
                    Row0.Cells.Add(dr["Byp_Attribute"].ToString(), DataType.String, "s17");

                    if (dr["Byp_Exc_Reason"].ToString() == "No income Verification" && (dr["Byp_Attribute"].ToString() == "HH Size" || dr["Byp_Attribute"].ToString() == "HH Income Level"))
                        Row0.Cells.Add("", DataType.String, "s17");
                    else
                        Row0.Cells.Add(dr["Byp_Att_Resp"].ToString(), DataType.String, "s17");

                    if (dr["Byp_Exc_Reason"].ToString() == "Incomplete Data" || dr["Byp_Exc_Reason"].ToString() == "No income Verification")
                        Row0.Cells.Add(dr["Byp_Exc_Reason"].ToString(), DataType.String, "s17R");
                    else
                        Row0.Cells.Add(dr["Byp_Exc_Reason"].ToString(), DataType.String, "s17");
                    //Row0.Cells.Add(dr["Byp_Att_Resp"].ToString(), DataType.String, "s17");
                    Row0.Cells.Add(dr["Byp_updated_date"].ToString() == "" ? "" : Convert
                        .ToDateTime(dr["Byp_updated_date"]).ToString("MM/dd/yyyy"), DataType.String, "s17");
                    Row0.Cells.Add(dr["Byp_updated_by"].ToString(), DataType.String, "s17");

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);
                book.Save(stream);
                stream.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }
                AlertBox.Show("Report Generated Successfully");
            }
            catch(Exception ex)
            {

            }
        }

        private void GenerateFamilyDetailsReport(DataTable dtResult, DataTable dtSummary, string pdfName, string propReportPath, string userID, string FamTypeSw)
        {
            string PdfName = "RNGS0004_MST_FAM_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = propReportPath + userID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + userID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + userID.Trim());
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
                PdfName = PdfName + ".xls";

            Workbook book = new Workbook();

            #region Stylesheet Options
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = book.Styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s15
            // -----------------------------------------------
            WorksheetStyle s15 = book.Styles.Add("s15");
            s15.Font.FontName = "Calibri";
            s15.Font.Size = 11;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = book.Styles.Add("s16");
            s16.Font.Bold = true;
            s16.Font.FontName = "Calibri";
            s16.Font.Color = "#000000";
            s16.Font.Size = 12;
            s16.Interior.Color = "#B0C4DE";
            s16.Interior.Pattern = StyleInteriorPattern.Solid;
            s16.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s16.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16.Alignment.WrapText = true;
            s16.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s16.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s16L
            // -----------------------------------------------
            WorksheetStyle s16L = book.Styles.Add("s16L");
            s16L.Font.Bold = true;
            s16L.Font.FontName = "Calibri";//"Arial";
            s16L.Font.Color = "#000000";
            s16L.Font.Size = 12;
            s16L.Interior.Color = "#B0C4DE";
            s16L.Interior.Pattern = StyleInteriorPattern.Solid;
            s16L.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s16L.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16L.Alignment.WrapText = true;
            s16L.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s16L.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = book.Styles.Add("s17");
            s17.Font.FontName = "Calibri";
            s17.Font.Color = "#000000";
            s17.Font.Size = 11;
            s17.Interior.Color = "#FFFFFF";
            s17.Interior.Pattern = StyleInteriorPattern.Solid;
            s17.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s17.Alignment.Vertical = StyleVerticalAlignment.Top;
            s17.Alignment.WrapText = true;
            s17.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = book.Styles.Add("s18");
            s18.Font.FontName = "Calibri";
            s18.Font.Color = "#000000";
            s18.Font.Size = 11;
            s18.Interior.Color = "#FFFFFF";
            s18.Interior.Pattern = StyleInteriorPattern.Solid;
            s18.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s18.Alignment.Vertical = StyleVerticalAlignment.Top;
            s18.Alignment.WrapText = true;
            s18.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = book.Styles.Add("s19");
            s19.Font.FontName = "Calibri";
            s19.Font.Color = "#000000";
            s19.Interior.Color = "#FFFFFF";
            s19.Font.Size = 11;
            s19.Interior.Pattern = StyleInteriorPattern.Solid;
            s19.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s19.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19.Alignment.WrapText = true;
            s19.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s19R
            // -----------------------------------------------
            WorksheetStyle s19R = book.Styles.Add("s19R");
            s19R.Font.FontName = "Calibri";
            s19R.Font.Color = "#FF0000";
            s19R.Interior.Color = "#FFFFFF";
            s19R.Font.Size = 11;
            s19R.Interior.Pattern = StyleInteriorPattern.Solid;
            s19R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s19R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19R.Alignment.WrapText = true;
            s19R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = book.Styles.Add("s20");
            s20.Font.FontName = "Calibri";
            s20.Font.Color = "#000000";
            s20.Interior.Color = "#FFFFFF";
            s20.Font.Size = 11;
            s20.Interior.Pattern = StyleInteriorPattern.Solid;
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Top;
            s20.Alignment.WrapText = true;
            s20.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //s20.NumberFormat = "[$-10809]dd/mm/yyyy\\ hh:mm:ss";
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = book.Styles.Add("s21");
            s21.Font.FontName = "Calibri";
            s21.Font.Size = 11;
            s21.Interior.Color = "#FFFFFF";
            s21.Interior.Pattern = StyleInteriorPattern.Solid;
            s21.Alignment.Vertical = StyleVerticalAlignment.Top;
            s21.Alignment.WrapText = true;


            #endregion

            Worksheet sheet = book.Worksheets.Add("Family_Details_Report");
            sheet.Table.DefaultRowHeight = 15F;
            //sheet.Table.ExpandedColumnCount = 14;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "s15";

            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 45;
            column0.StyleID = "s15";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 70;
            column1.StyleID = "s15";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 50;
            column2.StyleID = "s15";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 40;
            column3.StyleID = "s15";
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Width = 65;
            column4.StyleID = "s15";
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            column5.Width = 200;
            column5.StyleID = "s15";
            WorksheetColumn column6 = sheet.Table.Columns.Add();
            column6.Width = 70;
            column6.StyleID = "s15";
            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 70;
            column7.StyleID = "s15";
            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 70;
            column8.StyleID = "s15";
            WorksheetColumn column9 = sheet.Table.Columns.Add();
            column9.Width = 100;
            column9.StyleID = "s15";
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Width = 100;
            column10.StyleID = "s15";
            WorksheetColumn column11 = sheet.Table.Columns.Add();
            column11.Width = 150;
            column11.StyleID = "s15";
            WorksheetColumn column12 = sheet.Table.Columns.Add();
            column12.Width = 100;
            column12.StyleID = "s15";
            WorksheetColumn column13 = sheet.Table.Columns.Add();
            column13.Width = 100;
            column13.StyleID = "s15";
            WorksheetColumn column13Language = sheet.Table.Columns.Add();
            column13Language.Width = 105;
            column13Language.StyleID = "s15";
            WorksheetColumn column14 = sheet.Table.Columns.Add();
            column14.Width = 100;
            column14.StyleID = "s15";
            WorksheetColumn column15 = sheet.Table.Columns.Add();
            column15.Width = 70;
            column15.StyleID = "s15";
            WorksheetColumn column16 = sheet.Table.Columns.Add();
            column16.Width = 70;
            column16.StyleID = "s15";
            WorksheetColumn column17 = sheet.Table.Columns.Add();
            column17.Width = 120;
            column17.StyleID = "s15";
            WorksheetColumn column18 = sheet.Table.Columns.Add();
            column18.Width = 40;
            column18.StyleID = "s15";
            WorksheetColumn column19 = sheet.Table.Columns.Add();
            column19.Width = 40;
            column19.StyleID = "s15";
            WorksheetColumn column20 = sheet.Table.Columns.Add();
            column20.Width = 40;
            column20.StyleID = "s15";
            WorksheetColumn column21 = sheet.Table.Columns.Add();
            column21.Width = 40;
            column21.StyleID = "s15";
            WorksheetColumn column22 = sheet.Table.Columns.Add();
            column22.Width = 40;
            column22.StyleID = "s15";
            WorksheetColumn column23 = sheet.Table.Columns.Add();
            column23.Width = 40;
            column23.StyleID = "s15";
            WorksheetColumn column24 = sheet.Table.Columns.Add();
            column24.Width = 40;
            column24.StyleID = "s15";
            WorksheetColumn column25 = sheet.Table.Columns.Add();
            column25.Width = 40;
            column25.StyleID = "s15";
            WorksheetColumn column26 = sheet.Table.Columns.Add();
            column26.Width = 40;
            column26.StyleID = "s15";
            WorksheetColumn column27 = sheet.Table.Columns.Add();
            column27.Width = 80;
            column27.StyleID = "s15";
            WorksheetColumn column28 = sheet.Table.Columns.Add();
            column28.Width = 80;
            column28.StyleID = "s15";
            WorksheetColumn column29 = sheet.Table.Columns.Add();
            column29.Width = 90;
            column29.StyleID = "s15";
            WorksheetColumn column30 = sheet.Table.Columns.Add();
            column30.Width = 70;
            column30.StyleID = "s15";
            WorksheetColumn column31 = sheet.Table.Columns.Add();
            column31.Width = 130;
            column31.StyleID = "s15";
            WorksheetColumn column32 = sheet.Table.Columns.Add();
            column32.Width = 70;
            column32.StyleID = "s15";

            /* Worksheet sheet = book.Worksheets.Add("Sheet1");
             sheet.Table.DefaultRowHeight = 15F;
             sheet.Table.ExpandedColumnCount = 33;
             //sheet.Table.ExpandedRowCount = 300;
             sheet.Table.FullColumns = 1;
             sheet.Table.FullRows = 1;
             sheet.Table.StyleID = "s15";
             WorksheetColumn column0 = sheet.Table.Columns.Add();
             column0.Width = 18;
             column0.StyleID = "s15";
             column0.Span = 2;
             WorksheetColumn column1 = sheet.Table.Columns.Add();
             column1.Index = 4;
             column1.Width = 49;
             column1.StyleID = "s15";
             column1.Span = 1;
             WorksheetColumn column2 = sheet.Table.Columns.Add();
             column2.Index = 6;
             column2.Width = 144;
             column2.StyleID = "s15";
             WorksheetColumn column3 = sheet.Table.Columns.Add();
             column3.Width = 64;
             column3.StyleID = "s15";
             column3.Span = 1;
             WorksheetColumn column4 = sheet.Table.Columns.Add();
             column4.Index = 9;
             column4.Width = 54;
             column4.StyleID = "s15";
             WorksheetColumn column5 = sheet.Table.Columns.Add();
             column5.Width = 86;
             column5.StyleID = "s15";
             column5.Span = 2;
             WorksheetColumn column6 = sheet.Table.Columns.Add();
             column6.Index = 13;
             column6.Width = 57;
             column6.StyleID = "s15";
             WorksheetColumn column7 = sheet.Table.Columns.Add();
             column7.Width = 72;
             column7.StyleID = "s15";
             WorksheetColumn column8 = sheet.Table.Columns.Add();
             column8.Width = 82;
             column8.StyleID = "s15";
             WorksheetColumn column9 = sheet.Table.Columns.Add();
             column9.Width = 57;
             column9.StyleID = "s15";
             WorksheetColumn column10 = sheet.Table.Columns.Add();
             column10.Width = 54;
             column10.StyleID = "s15";
             WorksheetColumn column11 = sheet.Table.Columns.Add();
             column11.Width = 97;
             column11.StyleID = "s15";
             WorksheetColumn column12 = sheet.Table.Columns.Add();
             column12.Width = 57;
             column12.StyleID = "s15";
             column12.Span = 8;
             WorksheetColumn column13 = sheet.Table.Columns.Add();
             column13.Index = 28;
             column13.Width = 79;
             column13.StyleID = "s15";
             WorksheetColumn column14 = sheet.Table.Columns.Add();
             column14.Width = 64;
             column14.StyleID = "s15";
             column14.Span = 2;
             WorksheetColumn column15 = sheet.Table.Columns.Add();
             column15.Index = 32;
             column15.Width = 72;
             column15.StyleID = "s15";
             column15.Span = 1;*/
            // -----------------------------------------------

            try
            {
                WorksheetRow Row0 = sheet.Table.Rows.Add();
                Row0.Cells.Add("Agency", DataType.String, "s16");
                Row0.Cells.Add("Department", DataType.String, "s16");
                Row0.Cells.Add("Program", DataType.String, "s16");
                Row0.Cells.Add("Year", DataType.String, "s16");
                Row0.Cells.Add("App#", DataType.String, "s16L");
                Row0.Cells.Add("Client Name", DataType.String, "s16L");
                Row0.Cells.Add("Family ID", DataType.String, "s16L");
                Row0.Cells.Add("Client ID", DataType.String, "s16L");
                Row0.Cells.Add("Date", DataType.String, "s16L");
                Row0.Cells.Add("Reference Period", DataType.String, "s16");
                Row0.Cells.Add("Report Period", DataType.String, "s16");
                Row0.Cells.Add("Family Type", DataType.String, "s16L");
                Row0.Cells.Add("Fam.Size", DataType.String, "s16");
                Row0.Cells.Add("Housing Type", DataType.String, "s16L");
                Row0.Cells.Add("Primary Language", DataType.String, "s16L"); //Added this column by Vikash on 06/17/2024 as per 2024 Enhancements Document
                Row0.Cells.Add("Income Type(s)", DataType.String, "s16L");
                Row0.Cells.Add("FPL", DataType.String, "s16");
                Row0.Cells.Add("Ver.Date", DataType.String, "s16L");
                Row0.Cells.Add("Non-Cash Ben", DataType.String, "s16L");
                Row0.Cells.Add("13a", DataType.String, "s16");
                Row0.Cells.Add("13b", DataType.String, "s16");
                Row0.Cells.Add("13c", DataType.String, "s16");
                Row0.Cells.Add("13d", DataType.String, "s16");
                Row0.Cells.Add("13e", DataType.String, "s16");
                Row0.Cells.Add("13f", DataType.String, "s16");
                Row0.Cells.Add("13g", DataType.String, "s16");
                Row0.Cells.Add("13h", DataType.String, "s16");
                Row0.Cells.Add("13i", DataType.String, "s16");
                Row0.Cells.Add("Updated Date", DataType.String, "s16L");
                Row0.Cells.Add("Updated By", DataType.String, "s16L");
                Row0.Cells.Add("County", DataType.String, "s16L");
                Row0.Cells.Add("ZIP", DataType.String, "s16L");
                Row0.Cells.Add("Site", DataType.String, "s16L");
                Row0.Cells.Add("Act Program", DataType.String, "s16L");

                List<CommonEntity> FamilyType = _model.lookupDataAccess.GetAgyFamilyTypes();

                foreach (DataRow dr in dtResult.Rows)
                {
                    WorksheetRow Row1 = sheet.Table.Rows.Add();
                    Row1.Height = 30;
                    Row1.Cells.Add(dr["Fam_Agy"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_Dept"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_Prog"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_Year"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_App"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Client_Name"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_FamilyID"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_CLID"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Date"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["Fam_Date"]).ToString("MM/dd/yyyy"), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Reference_Period"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_Report_Period"].ToString(), DataType.String, "s17");

                    string ColorType = string.Empty;
                    if (FamilyType.Count > 0 && _baseForm.BaseAgencyControlDetails.FTypeSwitch == "Y")
                    {
                        int inthousehold = 0;
                        if (dr["Fam_Size"].ToString() != "No income Verification *")
                            inthousehold = dr["Fam_Size"].ToString() == string.Empty ? 0 : Convert.ToInt32(dr["Fam_Size"].ToString());

                        foreach (CommonEntity Entity in FamilyType)
                        {
                            if (Entity.Desc.Trim() == dr["Fam_Type"].ToString().Trim())
                            {
                                decimal decimalfamilytype = Entity.Extension.Trim() == null ? 0 : Convert.ToDecimal(Entity.Extension.Trim());
                                string ScreenType = Entity.AgyCode;
                                switch (ScreenType)
                                {
                                    case ">":
                                        if (inthousehold > decimalfamilytype)
                                            ColorType = string.Empty;
                                        else
                                            ColorType = "Red";
                                        break;
                                    case "=":
                                        if (decimalfamilytype == inthousehold)
                                            ColorType = string.Empty;
                                        else
                                            ColorType = "Red";
                                        break;
                                    case "<":
                                        if (inthousehold < decimalfamilytype)
                                            ColorType = string.Empty;
                                        else
                                            ColorType = "Red";
                                        break;
                                    default:
                                        ColorType = string.Empty;
                                        break;
                                }

                                break;
                            }
                        }
                    }
                    if (ColorType == "Red")
                        Row1.Cells.Add(dr["Fam_Type"].ToString(), DataType.String, "s19R");
                    else
                        Row1.Cells.Add(dr["Fam_Type"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Size"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_Hou_Type"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Lang"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Inc_Type"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_FPL"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_Ver_Date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Fam_Ver_Date"]).ToString("MM/dd/yyyy"), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Cash_Ben"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_13a"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13b"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13c"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13d"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13e"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13f"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13g"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13h"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Fam_13i"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Byp_updated_date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Byp_updated_date"]).ToString("MM/dd/yyyy"), DataType.String, "s19");
                    Row1.Cells.Add(dr["Byp_updated_by"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_County"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Zip"].ToString(), DataType.Number, "s19");
                    Row1.Cells.Add(dr["Fam_site"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Fam_Acty_Prog"].ToString(), DataType.String, "s19");

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);
                book.Save(stream);
                stream.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }
                AlertBox.Show("Report Generated Successfully");
            }
            catch (Exception ex)
            {

            }
        }

        private void GenerateIndividualDetailsReport(DataTable dtResult, DataTable dtSummary, string pdfName, string propReportPath, string userID, string FamTypeSw)
        {
            string PdfName = "RNGB0004_SNP_IND_RdlC" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = propReportPath + userID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + userID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + userID.Trim());
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
                PdfName = PdfName + ".xls";

            Workbook book = new Workbook();

            #region Stylesheet Options
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = book.Styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s15
            // -----------------------------------------------
            WorksheetStyle s15 = book.Styles.Add("s15");
            s15.Font.FontName = "Calibri";
            s15.Font.Size = 11;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = book.Styles.Add("s16");
            s16.Font.Bold = true;
            s16.Font.FontName = "Calibri";//"Arial";
            s16.Font.Color = "#000000";
            s16.Interior.Color = "#B0C4DE";
            s16.Font.Size = 12;
            s16.Interior.Pattern = StyleInteriorPattern.Solid;
            s16.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s16.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16.Alignment.WrapText = true;
            s16.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s16.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s16.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s16L
            // -----------------------------------------------
            WorksheetStyle s16L = book.Styles.Add("s16L");
            s16L.Font.Bold = true;
            s16L.Font.FontName = "Calibri";//"Arial";
            s16L.Font.Color = "#000000";
            s16L.Font.Size = 12;
            s16L.Interior.Color = "#B0C4DE";
            s16L.Interior.Pattern = StyleInteriorPattern.Solid;
            s16L.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s16L.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16L.Alignment.WrapText = true;
            s16L.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s16L.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s16L.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = book.Styles.Add("s17");
            s17.Font.FontName = "Calibri";
            s17.Font.Color = "#000000";
            s17.Font.Size = 11;
            s17.Interior.Color = "#FFFFFF";
            s17.Interior.Pattern = StyleInteriorPattern.Solid;
            s17.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s17.Alignment.Vertical = StyleVerticalAlignment.Top;
            s17.Alignment.WrapText = true;
            s17.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = book.Styles.Add("s18");
            s18.Font.FontName = "Calibri";
            s18.Font.Color = "#000000";
            s18.Font.Size = 11;
            s18.Interior.Color = "#FFFFFF";
            s18.Interior.Pattern = StyleInteriorPattern.Solid;
            s18.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s18.Alignment.Vertical = StyleVerticalAlignment.Top;
            s18.Alignment.WrapText = true;
            s18.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = book.Styles.Add("s19");
            s19.Font.FontName = "Calibri";
            s19.Font.Color = "#000000";
            s19.Font.Size = 11;
            s19.Interior.Color = "#FFFFFF";
            s19.Interior.Pattern = StyleInteriorPattern.Solid;
            s19.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s19.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19.Alignment.WrapText = true;
            s19.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s19R
            // -----------------------------------------------
            WorksheetStyle s19R = book.Styles.Add("s19R");
            s19R.Font.FontName = "Calibri";
            s19R.Font.Color = "#FF0000";
            s19R.Font.Size = 11;
            s19R.Interior.Color = "#FFFFFF";
            s19R.Interior.Pattern = StyleInteriorPattern.Solid;
            s19R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s19R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19R.Alignment.WrapText = true;
            s19R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = book.Styles.Add("s20");
            s20.Font.FontName = "Calibri";
            s20.Font.Color = "#000000";
            s20.Font.Size = 11;
            s20.Interior.Color = "#FFFFFF";
            s20.Interior.Pattern = StyleInteriorPattern.Solid;
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Top;
            s20.Alignment.WrapText = true;
            s20.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //s20.NumberFormat = "[$-10809]dd/mm/yyyy\\ hh:mm:ss";
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = book.Styles.Add("s21");
            s21.Font.FontName = "Calibri";
            s21.Font.Size = 11;
            s21.Interior.Color = "#FFFFFF";
            s21.Interior.Pattern = StyleInteriorPattern.Solid;
            s21.Alignment.Vertical = StyleVerticalAlignment.Top;
            s21.Alignment.WrapText = true;


            #endregion

            Worksheet sheet = book.Worksheets.Add("Individual_Details_Report");
            sheet.Table.DefaultRowHeight = 15F;
            //sheet.Table.ExpandedColumnCount = 14;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "s15";

            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 45;
            column0.StyleID = "s15";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 70;
            column1.StyleID = "s15";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 50;
            column2.StyleID = "s15";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 40;
            column3.StyleID = "s15";
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Width = 65;
            column4.StyleID = "s15";
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            column5.Width = 200;
            column5.StyleID = "s15";
            WorksheetColumn column6 = sheet.Table.Columns.Add();
            column6.Width = 70;
            column6.StyleID = "s15";
            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 70;
            column7.StyleID = "s15";
            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 80;
            column8.StyleID = "s15";
            WorksheetColumn column9 = sheet.Table.Columns.Add();
            column9.Width = 80;
            column9.StyleID = "s15";
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Width = 100;
            column10.StyleID = "s15";
            WorksheetColumn column11 = sheet.Table.Columns.Add();
            column11.Width = 100;
            column11.StyleID = "s15";
            WorksheetColumn column12 = sheet.Table.Columns.Add();
            column12.Width = 100;
            column12.StyleID = "s15";
            WorksheetColumn column13 = sheet.Table.Columns.Add();
            column13.Width = 50;
            column13.StyleID = "s15";
            WorksheetColumn column14 = sheet.Table.Columns.Add();
            column14.Width = 120;
            column14.StyleID = "s15";
            WorksheetColumn column15 = sheet.Table.Columns.Add();
            column15.Width = 120;
            column15.StyleID = "s15";
            WorksheetColumn column16 = sheet.Table.Columns.Add();
            column16.Width = 150;
            column16.StyleID = "s15";
            WorksheetColumn column17 = sheet.Table.Columns.Add();
            column17.Width = 80;
            column17.StyleID = "s15";
            WorksheetColumn column18 = sheet.Table.Columns.Add();
            column18.Width = 150;
            column18.StyleID = "s15";
            WorksheetColumn column19 = sheet.Table.Columns.Add();
            column19.Width = 80;
            column19.StyleID = "s15";
            WorksheetColumn column20 = sheet.Table.Columns.Add();
            column20.Width = 150;
            column20.StyleID = "s15";
            WorksheetColumn column21 = sheet.Table.Columns.Add();
            column21.Width = 150;
            column21.StyleID = "s15";
            WorksheetColumn column22 = sheet.Table.Columns.Add();
            column22.Width = 150;
            column22.StyleID = "s15";
            WorksheetColumn column23 = sheet.Table.Columns.Add();
            column23.Width = 80;
            column23.StyleID = "s15";
            WorksheetColumn column24 = sheet.Table.Columns.Add();
            column24.Width = 80;
            column24.StyleID = "s15";
            WorksheetColumn column25 = sheet.Table.Columns.Add();
            column25.Width = 80;
            column25.StyleID = "s15";
            WorksheetColumn column26 = sheet.Table.Columns.Add();
            column26.Width = 80;
            column26.StyleID = "s15";
            WorksheetColumn column27 = sheet.Table.Columns.Add();
            column27.Width = 120;
            column27.StyleID = "s15";
            WorksheetColumn column28 = sheet.Table.Columns.Add();

            // -----------------------------------------------

            try
            {
                WorksheetRow Row0 = sheet.Table.Rows.Add();
                Row0.Cells.Add("Agency", DataType.String, "s16");
                Row0.Cells.Add("Department", DataType.String, "s16");
                Row0.Cells.Add("Program", DataType.String, "s16");
                Row0.Cells.Add("Year", DataType.String, "s16");
                Row0.Cells.Add("App#", DataType.String, "s16L");
                Row0.Cells.Add("Client Name", DataType.String, "s16L");
                Row0.Cells.Add("Family ID", DataType.String, "s16L");
                Row0.Cells.Add("Client ID", DataType.String, "s16L");
                Row0.Cells.Add("Relation", DataType.String, "s16L");
                Row0.Cells.Add("Date", DataType.String, "s16L");
                Row0.Cells.Add("Reference Period", DataType.String, "s16");
                Row0.Cells.Add("Report Period", DataType.String, "s16");
                Row0.Cells.Add("Gender", DataType.String, "s16L");
                Row0.Cells.Add("Age", DataType.String, "s16");
                Row0.Cells.Add("Ethnicity", DataType.String, "s16L");
                Row0.Cells.Add("Race", DataType.String, "s16L");
                Row0.Cells.Add("Education", DataType.String, "s16L");
                Row0.Cells.Add("Health Ins.", DataType.String, "s16");
                Row0.Cells.Add("Health Codes", DataType.String, "s16L");
                Row0.Cells.Add("Disabled", DataType.String, "s16");
                Row0.Cells.Add("Military Status", DataType.String, "s16L");
                Row0.Cells.Add("Work Status", DataType.String, "s16L");
                Row0.Cells.Add("Disc. Youth", DataType.String, "s16L");
                Row0.Cells.Add("Updated Date", DataType.String, "s16L");
                Row0.Cells.Add("Updated By", DataType.String, "s16L");
                Row0.Cells.Add("County", DataType.String, "s16L");
                Row0.Cells.Add("ZIP", DataType.String, "s16L");
                Row0.Cells.Add("Site", DataType.String, "s16L");

                foreach (DataRow dr in dtResult.Rows)
                {
                    WorksheetRow Row1 = sheet.Table.Rows.Add();
                    Row1.Height = 30;
                    Row1.Cells.Add(dr["Ind_Agy"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Dept"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Prog"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Year"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_App"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Client_Name"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Fam_ID"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_CLID"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Relation"].ToString().Trim(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Ind_Date"].ToString()).ToString("MM/dd/yyyy"), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Reference_Period"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Report_Period"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Gender"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Age"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Ethnic"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Race"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Education"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Health"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Health_Codes"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Disabled"].ToString(), DataType.String, "s17");
                    Row1.Cells.Add(dr["Ind_Vet"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Farmer"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Youth"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Byp_updated_date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Byp_updated_date"].ToString()).ToString("MM/dd/yyyy"), DataType.String, "s19");
                    Row1.Cells.Add(dr["Byp_updated_by"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_County"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Zip"].ToString(), DataType.String, "s19");
                    Row1.Cells.Add(dr["Ind_Site"].ToString(), DataType.String, "s19");

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);
                book.Save(stream);
                stream.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }
                AlertBox.Show("Report Generated Successfully");
            }
            catch(Exception ex)
            {

            }
        }

        #region Vikash added on 02/05/2025 to convert excels to DevExpress format

        #region ByPass Report

        private void GenerateBypassReport_DevExpress(DataTable dtResult, DataTable dtSummary, string pdfName, string propReportPath, string userID)
        {
            #region FileNameBuild

            string PdfName = "RNGS0004_Bypass_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = propReportPath + userID + "\\" + PdfName;

            Random_Filename = null;
            string xlFileName = PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + _baseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseForm.UserID.Trim());
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

            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets[0];
                    SheetDetails.Name = "By_Pass_Report";
                    SheetDetails.ActiveView.ShowGridlines = false;
                    wb.Unit = DevExpress.Office.DocumentUnit.Point;

                    int _Rowindex = 0, _Columnindex = 0;

                    #region Column Widths

                    SheetDetails.Columns[0].Width = 70;//Agency
                    SheetDetails.Columns[1].Width = 70;//Dept
                    SheetDetails.Columns[2].Width = 70;//Prog
                    SheetDetails.Columns[3].Width = 40;//Year
                    SheetDetails.Columns[4].Width = 60;//App#
                    SheetDetails.Columns[5].Width = 150;//Client Name
                    SheetDetails.Columns[6].Width = 60;//Family ID
                    SheetDetails.Columns[7].Width = 60;//Client ID
                    SheetDetails.Columns[8].Width = 80;//Site
                    SheetDetails.Columns[9].Width = 100;//Data Field
                    SheetDetails.Columns[10].Width = 150;//Response
                    SheetDetails.Columns[11].Width = 150;//Exclusion Reason
                    SheetDetails.Columns[12].Width = 80;//Updated Date
                    SheetDetails.Columns[13].Width = 80;//Updated By

                    #endregion

                    #region Column Headers

                    _Rowindex = 0;
                    _Columnindex = 0;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Agency";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Department";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Program";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Year";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App#";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Client Name";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Family ID";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Client ID";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Data Field";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Response";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Exclusion Reason";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Updated Date";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Updated By";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;

                    #endregion

                    SheetDetails.FreezeRows(_Rowindex);

                    #region Data Printing

                    foreach (DataRow dr in dtResult.Rows)
                    {
                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Agy"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Dept"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Prog"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Year"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_App"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Client_Name"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Fam_ID"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_CLID"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Site"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Attribute"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        if (dr["Byp_Exc_Reason"].ToString() == "No income Verification" && (dr["Byp_Attribute"].ToString() == "HH Size" || dr["Byp_Attribute"].ToString() == "HH Income Level"))
                        {
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                            _Columnindex++;
                        }
                        else
                        {
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Att_Resp"].ToString();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                            _Columnindex++;
                        }

                        if (dr["Byp_Exc_Reason"].ToString() == "Incomplete Data" || dr["Byp_Exc_Reason"].ToString() == "No income Verification")
                        {
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Exc_Reason"].ToString();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                            _Columnindex++;
                        }
                        else
                        {
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_Exc_Reason"].ToString();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                            _Columnindex++;
                        }

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_updated_date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Byp_updated_date"]).ToString("MM/dd/yyyy");
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_updated_by"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;
                    }

                    #endregion

                    SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                    wb.SaveDocument(_excelPath, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                    try
                    {
                        string localFilePath = _excelPath;
                        /// Need to check for null value of localFilePath, etc...
                        FileInfo fiDownload = new FileInfo(localFilePath);
                        /// Need to check for file exists, is local file, is allow to read, etc...
                        string name = fiDownload.Name;
                        using (FileStream fileStream = fiDownload.OpenRead())
                        {
                            Application.Download(fileStream, name);
                            AlertBox.Show("Report Generated Successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Individual Details Report

        private void GenerateIndividualDetailsReport_DevExpress(DataTable dtResult, DataTable dtSummary, string pdfName, string propReportPath, string userID, string FamTypeSw)
        {
            #region FileNameBuild

            string PdfName = "RNGB0004_SNP_IND_RdlC" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = propReportPath + userID + "\\" + PdfName;

            Random_Filename = null;
            string xlFileName = PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + _baseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseForm.UserID.Trim());
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

            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets[0];
                    SheetDetails.Name = "Individual_Details_Report";
                    SheetDetails.ActiveView.ShowGridlines = false;
                    wb.Unit = DevExpress.Office.DocumentUnit.Point;

                    int _Rowindex = 0, _Columnindex = 0;

                    #region Column Widths

                    SheetDetails.Columns[0].Width = 70;//Agency
                    SheetDetails.Columns[1].Width = 70;//Dept
                    SheetDetails.Columns[2].Width = 70;//Prog
                    SheetDetails.Columns[3].Width = 40;//Year
                    SheetDetails.Columns[4].Width = 60;//App#
                    SheetDetails.Columns[5].Width = 150;//Client Name
                    SheetDetails.Columns[6].Width = 60;//Family ID
                    SheetDetails.Columns[7].Width = 60;//Client ID
                    SheetDetails.Columns[8].Width = 80;//Relation
                    SheetDetails.Columns[9].Width = 60;//Date
                    SheetDetails.Columns[10].Width = 100;//Reference Period
                    SheetDetails.Columns[11].Width = 80;//Report Period
                    SheetDetails.Columns[12].Width = 60;//Gender
                    SheetDetails.Columns[13].Width = 40;//Age
                    SheetDetails.Columns[14].Width = 120;//Ethnicity
                    SheetDetails.Columns[15].Width = 120;//Race
                    SheetDetails.Columns[16].Width = 90;//Education
                    SheetDetails.Columns[17].Width = 60;//Health Ins.
                    SheetDetails.Columns[18].Width = 200;//Health Codes
                    SheetDetails.Columns[19].Width = 60;//Disabled
                    SheetDetails.Columns[20].Width = 120;//Military Status
                    SheetDetails.Columns[21].Width = 120;//Work Status
                    SheetDetails.Columns[22].Width = 120;//Disc. Youth
                    SheetDetails.Columns[23].Width = 80;//Updated Date
                    SheetDetails.Columns[24].Width = 80;//Updated By
                    SheetDetails.Columns[25].Width = 80;//County
                    SheetDetails.Columns[26].Width = 60;//ZIP
                    SheetDetails.Columns[27].Width = 80;//Site

                    #endregion

                    #region Column Headers

                    _Rowindex = 0;
                    _Columnindex = 0;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Agency";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Department";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Program";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Year";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App#";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Client Name";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Family ID";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Client ID";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Relation";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Date";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Reference Period";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Report Period";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Gender";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Age";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Ethnicity";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Race";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Education";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Health Ins.";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Health Codes";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Disabled";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Military Status";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Work Status";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Disc. Youth";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Updated Date";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Updated By";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "County";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "ZIP";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    #endregion

                    SheetDetails.FreezeRows(_Rowindex);

                    #region Data Printing

                    foreach (DataRow dr in dtResult.Rows)
                    {
                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Agy"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Dept"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Prog"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Year"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_App"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Client_Name"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Fam_ID"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_CLID"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Relation"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Ind_Date"].ToString()).ToString("MM/dd/yyyy");
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Reference_Period"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Report_Period"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Gender"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Age"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Ethnic"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Race"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Education"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Health"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Health_Codes"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Disabled"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Vet"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Farmer"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Youth"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_updated_date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Byp_updated_date"].ToString()).ToString("MM/dd/yyyy");
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_updated_by"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_County"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Zip"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Ind_Site"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                    }

                    #endregion

                    SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                    wb.SaveDocument(_excelPath, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                    try
                    {
                        string localFilePath = _excelPath;
                        /// Need to check for null value of localFilePath, etc...
                        FileInfo fiDownload = new FileInfo(localFilePath);
                        /// Need to check for file exists, is local file, is allow to read, etc...
                        string name = fiDownload.Name;
                        using (FileStream fileStream = fiDownload.OpenRead())
                        {
                            Application.Download(fileStream, name);
                            AlertBox.Show("Report Generated Successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Family Details Report

        private void GenerateFamilyDetailsReport_DevExpress(DataTable dtResult, DataTable dtSummary, string pdfName, string propReportPath, string userID, string FamTypeSw)
        {
            #region FileNameBuild

            string PdfName = "RNGS0004_MST_FAM_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm");

            PdfName = propReportPath + userID + "\\" + PdfName;

            Random_Filename = null;
            string xlFileName = PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + _baseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseForm.UserID.Trim());
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

            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets[0];
                    SheetDetails.Name = "Family_Details_Report";
                    SheetDetails.ActiveView.ShowGridlines = false;
                    wb.Unit = DevExpress.Office.DocumentUnit.Point;

                    int _Rowindex = 0, _Columnindex = 0;

                    #region Column Widths

                    SheetDetails.Columns[0].Width = 70;//Agency
                    SheetDetails.Columns[1].Width = 70;//Dept
                    SheetDetails.Columns[2].Width = 70;//Prog
                    SheetDetails.Columns[3].Width = 40;//Year
                    SheetDetails.Columns[4].Width = 60;//App#
                    SheetDetails.Columns[5].Width = 150;//Client Name
                    SheetDetails.Columns[6].Width = 60;//Family ID
                    SheetDetails.Columns[7].Width = 60;//Client ID
                    SheetDetails.Columns[8].Width = 60;//Date
                    SheetDetails.Columns[9].Width = 100;//Reference Period
                    SheetDetails.Columns[10].Width = 80;//Report Period
                    SheetDetails.Columns[11].Width = 120;//Family Type
                    SheetDetails.Columns[12].Width = 60;//Fam.Size
                    SheetDetails.Columns[13].Width = 100;//Housing Type
                    SheetDetails.Columns[14].Width = 100;//Primary Language
                    SheetDetails.Columns[15].Width = 120;//Income Type(s)
                    SheetDetails.Columns[16].Width = 60;//FPL
                    SheetDetails.Columns[17].Width = 60;//Ver.Date
                    SheetDetails.Columns[18].Width = 120;//Non-Cash Ben
                    SheetDetails.Columns[19].Width = 40;//13a
                    SheetDetails.Columns[20].Width = 40;//13b
                    SheetDetails.Columns[21].Width = 40;//13c
                    SheetDetails.Columns[22].Width = 40;//13d
                    SheetDetails.Columns[23].Width = 40;//13e
                    SheetDetails.Columns[24].Width = 40;//13f
                    SheetDetails.Columns[25].Width = 40;//13g
                    SheetDetails.Columns[26].Width = 40;//13h
                    SheetDetails.Columns[27].Width = 40;//13i
                    SheetDetails.Columns[28].Width = 80;//Updated Date
                    SheetDetails.Columns[29].Width = 80;//Updated By
                    SheetDetails.Columns[30].Width = 80;//County
                    SheetDetails.Columns[31].Width = 80;//ZIP
                    SheetDetails.Columns[32].Width = 80;//Site
                    SheetDetails.Columns[33].Width = 70;//Act Program

                    #endregion

                    #region Column Headers

                    _Rowindex = 0;
                    _Columnindex = 0;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Agency";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Department";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Program";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Year";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App#";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Client Name";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Family ID";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Client ID";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Date";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Reference Period";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Report Period";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Family Type";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Fam.Size";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Housing Type";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Primary Language";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Income Type(s)";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "FPL";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Ver.Date";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Non-Cash Ben";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13a";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13b";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13c";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13d";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13e";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13f";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13g";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13h";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "13i";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Updated Date";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Updated By";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "County";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "ZIP";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                    _Columnindex++;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Act Program";
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;

                    #endregion

                    SheetDetails.FreezeRows(_Rowindex);

                    #region Data Printing

                    List<CommonEntity> FamilyType = _model.lookupDataAccess.GetAgyFamilyTypes();

                    foreach (DataRow dr in dtResult.Rows)
                    {
                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Agy"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Dept"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Prog"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Year"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_App"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Client_Name"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_FamilyID"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_CLID"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Fam_Date"].ToString()).ToString("MM/dd/yyyy");
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Reference_Period"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Report_Period"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        string ColorType = string.Empty;
                        if (FamilyType.Count > 0 && _baseForm.BaseAgencyControlDetails.FTypeSwitch == "Y")
                        {
                            int inthousehold = 0;
                            if (dr["Fam_Size"].ToString() != "No income Verification *")
                                inthousehold = dr["Fam_Size"].ToString() == string.Empty ? 0 : Convert.ToInt32(dr["Fam_Size"].ToString());

                            foreach (CommonEntity Entity in FamilyType)
                            {
                                if (Entity.Desc.Trim() == dr["Fam_Type"].ToString().Trim())
                                {
                                    decimal decimalfamilytype = Entity.Extension.Trim() == null ? 0 : Convert.ToDecimal(Entity.Extension.Trim());
                                    string ScreenType = Entity.AgyCode;
                                    switch (ScreenType)
                                    {
                                        case ">":
                                            if (inthousehold > decimalfamilytype)
                                                ColorType = string.Empty;
                                            else
                                                ColorType = "Red";
                                            break;
                                        case "=":
                                            if (decimalfamilytype == inthousehold)
                                                ColorType = string.Empty;
                                            else
                                                ColorType = "Red";
                                            break;
                                        case "<":
                                            if (inthousehold < decimalfamilytype)
                                                ColorType = string.Empty;
                                            else
                                                ColorType = "Red";
                                            break;
                                        default:
                                            ColorType = string.Empty;
                                            break;
                                    }

                                    break;
                                }
                            }
                        }
                        if (ColorType == "Red")
                        {
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Type"].ToString();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                            _Columnindex++;
                        }
                        else
                        {
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Type"].ToString();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                            _Columnindex++;
                        }

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Size"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Hou_Type"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Lang"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Inc_Type"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_FPL"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Ver_Date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Fam_Ver_Date"]).ToString("MM/dd/yyyy");
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Cash_Ben"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13a"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13b"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13c"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13d"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13e"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13f"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13g"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13h"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_13i"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_updated_date"].ToString() == "" ? "" : Convert.ToDateTime(dr["Byp_updated_date"]).ToString("MM/dd/yyyy");
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Byp_updated_by"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_County"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Zip"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_site"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["Fam_Acty_Prog"].ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 10;
                        _Columnindex++;
                    }

                    #endregion

                    SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                    wb.SaveDocument(_excelPath, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                    try
                    {
                        string localFilePath = _excelPath;
                        /// Need to check for null value of localFilePath, etc...
                        FileInfo fiDownload = new FileInfo(localFilePath);
                        /// Need to check for file exists, is local file, is allow to read, etc...
                        string name = fiDownload.Name;
                        using (FileStream fileStream = fiDownload.OpenRead())
                        {
                            Application.Download(fileStream, name);
                            AlertBox.Show("Report Generated Successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion


        #endregion
    }
}