#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;
using Wisej.Web;
using Captain.Common.Utilities;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Captain.Common.Model.Data;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using DevExpress.XtraReports.Native;
using Wisej.AspNetReportViewer;
using Wisej.Core;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using DevExpress.Utils.Frames;
using Syncfusion.XlsIO.Parser.Biff_Records;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB2012_AdhocRDLCForm : Form, IWisejHandler
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private CaptainModel _model = null;

        // AspNetReportViewer reportViewer = new AspNetReportViewer();
        private LocalReportHelper _report;

        public CASB2012_AdhocRDLCForm(DataTable Result_table, DataTable summary_table, string report_name, string report_to_process, string reportpath, string strUserId, string strScreenName)
        {
            InitializeComponent();

            // reportViewer = new AspNetReportViewer();
            // this.reportViewer.Init += new System.EventHandler(this.reportViewer1_Init);
            // this.reportViewer.UserName = "ssrs";
            //this.reportViewer.Password = "password";
            // this.pnlReportview.Controls.Add(reportViewer);

            _model = new CaptainModel();
            UserId = strUserId;
            Report_To_Process = report_to_process;
            Result_Table = Result_table;
            Report_Name = report_name;
            Summary_table = summary_table;
            Reportpath = reportpath;
            strScreen = strScreenName;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = "Report Viewer";
            if (strScreenName == "RNG")
                btnGenExcel.Visible = true;
            ////if (Summary_table != null)
            ////    button1.Visible = true;
            ///

            //rvViewer_HostedPageLoad();
            // load the data.
            var data = CreateDataSource();

            // load the report.
            this._data = Result_table; // CreateDataSource();
            string strFolderPath = Reportpath + Report_Name;
            //this.reportViewer1.ReportPath = strFolderPath;
        }

        private object _data;
        private int performBack;
        public string ReportPath { get; set; }

        byte[] bytesglobal;
        private async System.Threading.Tasks.Task<MemoryStream> GetMemoryStream()
        {
            string strFolderPath = Reportpath + Report_Name;
            var reportHelper = LocalReportHelper.CreateWithSummaryReport(strFolderPath, Summary_table);
            reportHelper.AddDataSource("ZipCodeDataset", this._data);
            byte[] bytes = reportHelper.Render("PDF");
            bytesglobal = bytes;
            _report = reportHelper;
            return new MemoryStream(bytes);
        }

        MemoryStream msGlobal = new MemoryStream();
        private async Task LoadReport()
        {
            await Application.StartTask(async () =>
            {
                this.ShowLoader = true;
                MemoryStream ms = await GetMemoryStream();
                msGlobal = ms;
                this.pdfViewer1.PdfStream = ms;

            });
        }

        #region Overrides of Form

        /// <summary>
        /// Fires the <see cref="E:Wisej.Web.Form.Load" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override async void OnLoad(EventArgs e)
        {
            bool Main_Report_Processing_Completed = false;
            if (Report_To_Process == "Result Table")
            {
                try
                {
                    await LoadReport();
                    Application.Update(this, () => { this.ShowLoader = false; });
                    boolStatus = false;
                    Main_Report_Processing_Completed = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "CAP Systems");
                    boolStatus = false;
                    Main_Report_Processing_Completed = true;
                }
            }

            // }
            // if (Main_Report_Processing_Completed)
            //}

            //while (this.performBack > 0)
            //{
            //    this.performBack--;
            //    this.reportViewer1.WrappedControl.PerformBack();
            //}
            base.OnLoad(e);
        }

        #endregion

        private async void reportViewer1_Load(object sender, System.EventArgs e)
        {
            //rvViewer_HostedPageLoad();
            bool Main_Report_Processing_Completed = false;

            //if (!this.reportViewer1.IsPostBack)
            //{
            // if (boolStatus)
            //{
            if (Report_To_Process == "Result Table")
            {
                try
                {
                    ////var report = this.reportViewer1.WrappedControl.LocalReport;
                    ////report.DataSources.Add(new ReportDataSource("ZipCodeDataset", this._data));
                    //this.pdfViewer1.PdfStream = GetMemoryStream();
                    await LoadReport();
                    Application.Update(this, () => { this.ShowLoader = false; });
                    boolStatus = false;
                    Main_Report_Processing_Completed = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "CAP Systems");
                    boolStatus = false;
                    Main_Report_Processing_Completed = true;
                }
            }

            // }
            // if (Main_Report_Processing_Completed)
            //this._report.SubreportProcessing += new SubreportProcessingEventHandler(ZipCode_SubreportProcessing);
            //}

            //while (this.performBack > 0)
            //{
            //    this.performBack--;
            //    this.reportViewer1.WrappedControl.PerformBack();
            //}
        }

        private void reportViewer1_Init(object sender, System.EventArgs e)
        {
        }

        private void reportViewer1_Toggle(object sender, CancelEventArgs e)
        {
        }

        private void reportViewer1_BookmarkNavigation(object sender, BookmarkNavigationEventArgs e)
        {
        }

        #region properties

        public string Report_Name { get; set; }

        public string Report_To_Process { get; set; }

        public DataTable Result_Table { get; set; }

        public DataTable Summary_table { get; set; }

        public string Reportpath { get; set; }

        public string strScreen { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool boolStatus = true;
        public string propReportPath { get; set; }

        public bool Compress => throw new NotImplementedException();

        //  TODO: Wisej....

        private object CreateDataSource()
        {
            //var data = new List<Model>();
            //for (int i = 0; i < 200; i++)
            //{
            //	data.Add(new Model { Name = $"Jack {i}", LastName = "White" });
            //	data.Add(new Model { Name = $"Joe {i}", LastName = "Black" });
            //}

            //var data = new DataTable();
            //data.Columns.Add("Name");
            //data.Columns.Add("LastName");
            //data.Rows.Add("Jack", "White");
            //data.Rows.Add("Joe", "Black");


            var data = new DataTable();
            data.Columns.Add("MST_AGENCY");
            //data.Columns.Add("LastName");
            //data.Rows.Add("Jack", "White");
            // data.Rows.Add("Joe", "Black");


            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");
            data.Rows.Add("Agency 01");

            return data;
        }

        private void rvViewer_HostedPageLoad()
        {
            bool Main_Report_Processing_Completed = false;
            if (boolStatus)
            {
                if (Report_To_Process == "Result Table")
                {
                    //reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;


                    //string strFolderPath = "C:\\CapReports\\" + Report_Name;      // Run at Local System
                    //string strFolderPath = Consts.Common.ReportFolderLocation +  Report_Name;    // Run at Server
                    //string strFolderPath = Consts.Common.Tmp_ReportFolderLocation + Report_Name;    // Run at Server

                    // string strFolderPath = Reportpath + Report_Name;    // Run at Server

                    //string strFolderPath = "F:\\CapreportsRDLC\\" + Report_Name;      // Run at Local System
                    //string strFolderPath = "\\\\cap-dev\\F-Drive\\CapreportsRDLC\\" + Report_Name;    // Run at Server

                    //string strFolderPath = "C:\\CapReports\\MSTSNP.rdlc";      // Run at Local System
                    //string strFolderPath = Consts.Common.ReportFolderLocation + "\\MSTSNP.rdlc";    // Run at Server

                    //rvViewer.LocalReport.ReportPath = strFolderPath;
                    //this.reportViewer1.ReportPath = strFolderPath;
                    ReportDataSource CaptainDataSource = new ReportDataSource("ZipCodeDataset", Result_Table);

                    //ITG:ALAA: CHANGED.
                    //var report = this.reportViewer1.WrappedControl.LocalReport;
                    //var _data = CreateDataSource();
                    //report.DataSources.Add(new ReportDataSource("ZipCodeDataset", _data));

                    //this.reportViewer1.WrappedControl.LocalReport.DataSources.Add(CaptainDataSource);
                    //this.reportViewer1.Update();

                    //CaptainDataSourceTableAdapters.ZIPCODETableAdapter objZipcodeTableAdapter = new CaptainDataSourceTableAdapters.ZIPCODETableAdapter();
                    // CaptainDataSource.ZIPCODEDataTable ZipcodeDataTable = new CaptainDataSource.ZIPCODEDataTable();// objZipcodeTableAdapter.GetZIPCODEData();// new CaptainDataSource.ZIPCODEDataTable();


                    //StreamReader subReport = File.OpenText(@"C:\\CapReports\\SummaryReport.rdlc");
                    //this.rvViewer.LocalReport.LoadSubreportDefinition("SubReport", subReport);


                    //Assembly _assembly = Assembly.GetExecutingAssembly();
                    //StreamReader subReport = new StreamReader(@"C:\\CapReports\\SummaryReport.rdlc");

                    //this.rvViewer.LocalReport.LoadSubreportDefinition("CompanyHeader", subReport);

                    //this.rvViewer.RefreshReport();


                    try
                    {
                        //reportViewer.DataSources.Add(CaptainDataSource);
                        // rvViewer.LocalReport.Refresh();
                        boolStatus = false;
                        Main_Report_Processing_Completed = true;
                    }

                    catch (Exception ex)
                    {
                        AlertBox.Show(ex.Message, MessageBoxIcon.Error);
                        boolStatus = false;
                        Main_Report_Processing_Completed = true;
                    }


                    ////////rvViewer.LocalReport.DataSources.Add(CaptainDataSource);
                    ////////rvViewer.LocalReport.Refresh();
                    ////////boolStatus = false;
                    ////////Main_Report_Processing_Completed = true;


                    //this.rvViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ZipCode_SubreportProcessing);
                }
                //else
                //{
                //    rvViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                //    string strFolderPath = "C:\\CapReports\\Summary_Report.rdlc";      // Run at Local System
                //    //string strFolderPath = Consts.Common.ReportFolderLocation +  Report_Name;    // Run at Server

                //    //string strFolderPath = "F:\\CapreportsRDLC\\" + Report_Name;      // Run at Local System
                //    //string strFolderPath = "\\\\cap-dev\\F-Drive\\CapreportsRDLC\\" + Report_Name;    // Run at Server

                //    //string strFolderPath = "C:\\CapReports\\MSTSNP.rdlc";      // Run at Local System
                //    //string strFolderPath = Consts.Common.ReportFolderLocation + "\\MSTSNP.rdlc";    // Run at Server

                //    rvViewer.LocalReport.ReportPath = strFolderPath;

                //    //CaptainDataSourceTableAdapters.ZIPCODETableAdapter objZipcodeTableAdapter = new CaptainDataSourceTableAdapters.ZIPCODETableAdapter();
                //    //  CaptainDataSource.ZIPCODEDataTable ZipcodeDataTable = new CaptainDataSource.ZIPCODEDataTable();// objZipcodeTableAdapter.GetZIPCODEData();// new CaptainDataSource.ZIPCODEDataTable();

                //    ReportDataSource CaptainDataSource = new ReportDataSource("ZipCodeDataset", Summary_table);

                //    rvViewer.LocalReport.DataSources.Add(CaptainDataSource);
                //    rvViewer.LocalReport.Refresh();
                //    boolStatus = false;
                //}
            }

            //if (Main_Report_Processing_Completed)
            //  this.rvViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ZipCode_SubreportProcessing);
            //rvViewer.Drillthrough += new DrillthroughEventHandler(ZipCode_DrillthroughReport);


            //this.rvViewer.LocalReport.SubreportProcessing += new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler(ZipCode_SubreportProcessing);
            //this._report.SubreportProcessing += new SubreportProcessingEventHandler(ZipCode_SubreportProcessing);


            //rvViewer.Drillthrough += new DrillthroughEventHandler(ZipCode_DrillthroughReport);
        }


        private void ZipCode_DrillthroughReport(object sender, DrillthroughEventArgs e)
        {
            LocalReport localReport = (LocalReport)e.Report;

            localReport.DataSources.Add(new ReportDataSource("OrderDetailsDataSet_OrderDetails", Summary_table));

            LocalReport report = (LocalReport)e.Report;

            ReportDataSource CaptainDataSource = new ReportDataSource("ZipCodeDataset", Summary_table);

            report.DataSources.Add(new ReportDataSource("MyData", CaptainDataSource));
        }

        private void Sub_Repport()
        {
            ////rvViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            //string strFolderPath = "C:\\CapReports\\Summary_Report.rdlc";      // Run at Local System

            string strFolderPath = Reportpath + "Summary_Report.rdlc"; // Run at Local System


            //string strFolderPath = Consts.Common.ReportFolderLocation +  Report_Name;    // Run at Server

            //string strFolderPath = "F:\\CapreportsRDLC\\" + Report_Name;      // Run at Local System
            //string strFolderPath = "\\\\cap-dev\\F-Drive\\CapreportsRDLC\\" + Report_Name;    // Run at Server

            //string strFolderPath = "C:\\CapReports\\MSTSNP.rdlc";      // Run at Local System
            //string strFolderPath = Consts.Common.ReportFolderLocation + "\\MSTSNP.rdlc";    // Run at Server

            ////rvViewer.LocalReport.ReportPath = strFolderPath;

            //CaptainDataSourceTableAdapters.ZIPCODETableAdapter objZipcodeTableAdapter = new CaptainDataSourceTableAdapters.ZIPCODETableAdapter();
            //  CaptainDataSource.ZIPCODEDataTable ZipcodeDataTable = new CaptainDataSource.ZIPCODEDataTable();// objZipcodeTableAdapter.GetZIPCODEData();// new CaptainDataSource.ZIPCODEDataTable();

            ReportDataSource CaptainDataSource = new ReportDataSource("ZipCodeDataset", Summary_table);

            ////rvViewer.LocalReport.DataSources.Add(CaptainDataSource);
            ////rvViewer.LocalReport.Refresh();
            boolStatus = false;
        }

        private void CASB2012_AdhocRDLCForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Result_Table.Clear();         // Check it Once later
            //Result_Table.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Warning[] warnings;
            //string[] streamids;
            //string mimeType;
            //string encoding;
            //string extension;

            //byte[] bytes = rvViewer.LocalReport.Render(
            //   "EXCELOPENXML", null, out mimeType, out encoding,
            //    out extension,
            //   out streamids, out warnings);

            //FileStream fs = new FileStream(@"c:\output.xls",
            //   FileMode.Create);
            //fs.Write(bytes, 0, bytes.Length);
            //fs.Close();

            string strreportName = Report_Name.Replace("SYSTEM ", "");
            strreportName = strreportName.Replace(".rdlc", "");

            string PdfName = propReportPath + UserId + "\\" + strreportName;
            string Random_Filename = string.Empty;
            try
            {
                string Tmpstr = PdfName + ".txt";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".txt";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".txt";

            FileStream fs = null;


            if (!File.Exists(PdfName))
            {
                using (fs = File.Create(PdfName))
                {
                }

                string data = string.Empty;
                using (StreamWriter sw = new StreamWriter(PdfName))
                {
                    int i = 0;
                    for (i = 0; i < Result_Table.Columns.Count - 1; i++)
                    {
                        sw.Write(Result_Table.Columns[i].ColumnName + "|");
                    }

                    sw.Write(Result_Table.Columns[i].ColumnName);
                    sw.WriteLine();

                    foreach (DataRow row in Result_Table.Rows)
                    {
                        object[] array = row.ItemArray;

                        for (i = 0; i < array.Length - 1; i++)
                        {
                            //if (array[i].ToString().Contains(":"))                            
                            //    sw.Write(LookupDataAccess.Getdate(array[i].ToString()) + "|");                        
                            //else
                            sw.Write(array[i].ToString() + "|");
                        }

                        //if (array[i].ToString().Contains(":"))
                        //    sw.Write(LookupDataAccess.Getdate(array[i].ToString()));
                        //else
                        sw.Write(array[i].ToString());
                        sw.WriteLine();
                    }

                    if (Summary_table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Summary_table.Rows)
                        {
                            if (row["RO_ROW_ID"].ToString().Trim() == string.Empty)
                            {
                                sw.WriteLine();
                                sw.WriteLine();
                                sw.WriteLine();
                                sw.WriteLine();
                            }

                            if (row["RO_IS_HEADER"].ToString().ToUpper() == "TRUE")
                            {
                                sw.WriteLine(row["RO_CHILD_DESC1"].ToString() + "|" + row["RO_CHILD_COUNT1"].ToString() + "|" + row["RO_CHILD_DESC2"].ToString() + "|" + row["RO_CHILD_COUNT2"].ToString());
                                sw.WriteLine();
                                sw.WriteLine();
                            }
                            else
                            {
                                if (row["RO_ROW_ID"].ToString().Trim() != string.Empty)
                                {
                                    sw.WriteLine(row["RO_CHILD_DESC1"].ToString() + "|" + row["RO_CHILD_COUNT1"].ToString() + "|" + row["RO_CHILD_DESC2"].ToString() + "|" + row["RO_CHILD_COUNT2"].ToString());
                                }
                            }
                        }

                        sw.WriteLine();
                    }

                    AlertBox.Show("Notepad Generated Successfully");
                }
            }
        }


        //TODO: Wisej....

        //private void rvViewer_HostedControlLoad(object sender, Wisej.Web.Hosts.AspControlEventArgs e)
        //{
        //    if (Result_Table.Rows.Count > 20000)
        //    {
        //        if (Result_Table.Rows.Count > 60000)
        //        {
        //            RenderingExtension extension = rvViewer.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals("Excel", StringComparison.CurrentCultureIgnoreCase));
        //            System.Reflection.FieldInfo info = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //            info.SetValue(extension, false);
        //        }
        //        btnNotePad.Visible = true;
        //        btnPreviewReport.Visible = true;
        //    }
        //    else
        //    {
        //        btnNotePad.Visible = false;
        //        btnPreviewReport.Visible = false;
        //    }

        //    if (Result_Table.Rows.Count > 60000)
        //        btnGenExcel.Visible = false;
        //    else if(strScreen == "RNG") btnGenExcel.Visible = true;

        //}

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void btnPreviewReport_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(UserId, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        public void GenerateExcel(byte[] excelbytes)
        {
            try
            {


                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = excelbytes;
                //rvViewer.LocalReport.Render(
                //   "Excel", null, out mimeType, out encoding,
                //    out extension,
                //   out streamids, out warnings);

                string strreportName = Report_Name.Replace("SYSTEM ", "");
                strreportName = strreportName.Replace(".rdlc", "");


                try
                {
                    if (!Directory.Exists(propReportPath + UserId + "\\MERGEFILES\\"))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + UserId + "\\MERGEFILES\\");
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                string PdfName = propReportPath + UserId + "\\MERGEFILES\\" + strreportName + ".xls";



                FileStream fs = new FileStream(PdfName, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                AlertBox.Show("Excel file Generated Successfully. File Name is " + strreportName + ".xls");




            }
            catch (Exception ex)
            {
                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void btnGenExcel_Click(object sender, EventArgs e)
        {
            Application.Navigate(this.GetPostbackURL("export=excel"));
        }


        bool IWisejHandler.Compress => false;

        void IWisejHandler.ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            var export = request["export"];

            switch (export)
            {
                case "word":
                case "excel":
                    var format = export == "word" ? "docx" : "xlsx";
                    var renderFormat = GetRenderFormat(format);
                    var bytes = this._report.Render(renderFormat);

                    if (export == "excel")
                    {
                        if (bytes != null)
                        {
                            GenerateExcel(bytes);
                        }
                    }

                    var filename = Path.GetFileNameWithoutExtension(_report.GetReportPath());
                    response.ContentType = $"application/{GetMimeType(format)}";
                    response.AppendHeader("Access-Control-Allow-Origin", "*");
                    response.AppendHeader("Content-Disposition", $"attachment; filename=\"{filename}.{format}\"");
                    response.AppendHeader("Content-Length", bytes.Length.ToString());
                    response.OutputStream.Write(bytes, 0, bytes.Length);


                    break;
            }
        }

        private object GetMimeType(string format)
        {
            string mimetype = format; // string.Empty;

            switch (mimetype)
            {
                case "docx":
                    mimetype = "vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "xlsx":
                    mimetype = "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                default:
                    mimetype = "PDF";
                    break;
            }

            return mimetype;
        }

        private string GetRenderFormat(string format)
        {
            string renderFormat = string.Empty;

            // Default to PDF if the format is not supported or not specified.
            switch (format)
            {
                case "docx":
                    renderFormat = "WORDOPENXML";
                    break;
                case "xlsx":
                    renderFormat = "EXCELOPENXML";
                    break;
                default:
                    renderFormat = "PDF";
                    break;
            }

            return renderFormat;
        }

        private void CASB2012_AdhocRDLCForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            var toolButton = e.Tool.Name;
            switch (toolButton)
            {
                case "excel-button":
                    Application.Navigate(this.GetPostbackURL("export=excel"));
                    break;
                case "word-button":
                    Application.Navigate(this.GetPostbackURL("export=word"));
                    break;
                case "pdf-button":
                    var stream = new MemoryStream(bytesglobal);
                    string FileName = Report_Name.Replace("rdlc", "pdf");
                    Application.Download(stream, FileName);
                    //Application.DownloadAndOpen(this.GetPostbackURL("export=word"));
                    //Application.DownloadAndOpen("_blank",this.pdfViewer1.PdfStream, "pdffile.pdf");
                    //Application.Navigate(this.GetPostbackURL("export=pdf"));
                    break;
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Form1 RDLC_Form = new Form1(Result_Table, Summary_table, Report_To_Process, "Result Table");
        //    //RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //    RDLC_Form.ShowDialog();

        //}
    }
}
