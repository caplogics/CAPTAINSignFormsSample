#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Microsoft.Reporting.WebForms;
using Captain.Common.Utilities;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB0004_DG_RDLCForm : Form
    {
        public CASB0004_DG_RDLCForm(string scr_Code, DataSet Result_dataSet, DataTable Result_table, DataTable summary_table,string main_Rep_Name, string report_name, string report_to_process, bool detail_Rep_Required, string reportPath,string strUserId)
        {
            InitializeComponent();

            this.Text = "Report Viewer";
            Scr_Code = scr_Code;
            Report_To_Process = report_to_process;
            Result_Table = Result_table;
            Main_Rep_Name = main_Rep_Name;
            Report_Name = report_name;
            Summary_table = summary_table;
            Result_DataSet = Result_dataSet;
            Detail_Rep_Required = detail_Rep_Required;
            ReportPath = reportPath;
            UserId = strUserId;
            //if (Summary_table != null)
            //    button1.Visible = true;

            if (Detail_Rep_Required)
            {
                switch (Scr_Code)
                {
                    case "CASB0004": Btn_Bypass.Visible = Btn_SNP_Details.Visible = Btn_MST_Details.Visible = true; break; // 
                    case "CASB0014": this.Btn_Bypass.Location = new System.Drawing.Point(2, 507);
                                     Btn_Bypass.Text = "Detail Report"; Btn_Bypass.Visible = true; break;
                }
            }
            else
            {
               // this.rvViewer.Size = new System.Drawing.Size(791, 536); ---- Commented by Vikash
                //switch (Scr_Code)
                //{
                //    case "CASB0004": Btn_SNP_Details.Visible = Btn_MST_Details.Visible = true; break; // Btn_Bypass.Visible = 
                //    case "CASB0014": this.rvViewer.Size = new System.Drawing.Size(791, 536); break;
                //}
            }
        }

        #region properties

        public string Scr_Code { get; set; }
        
        public string Report_Name { get; set; }

        public string Report_To_Process { get; set; }

        public DataTable Result_Table { get; set; }

        public DataTable Summary_table { get; set; }

        public DataSet Result_DataSet { get; set; }

        public bool Detail_Rep_Required { get; set; }

        public string Main_Rep_Name { get; set; }

        public string ReportPath { get; set; }

        #endregion
        public string UserId { get; set; }
        public bool boolStatus = true;

        //********** Commented by Vikash
        //private void rvViewer_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    bool Main_Report_Processing_Completed = false;
        //    if (boolStatus)
        //    {
        //        if (Report_To_Process == "Result Table")
        //        {
        //            rvViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;


        //            //string strFolderPath = "C:\\CapReports\\" + Report_Name;      // Run at Local System
        //            //string strFolderPath = Consts.Common.ReportFolderLocation + Report_Name;    // Run at Server
        //            //string strFolderPath = Consts.Common.Tmp_ReportFolderLocation + Report_Name;    // Run at Server

        //            string strFolderPath = ReportPath + Report_Name;    // Run at Server

        //            rvViewer.LocalReport.ReportPath = strFolderPath;
        //            ReportDataSource CaptainDataSource = new ReportDataSource("ZipCodeDataset", Result_Table);

        //            rvViewer.LocalReport.DataSources.Add(CaptainDataSource);
        //            rvViewer.LocalReport.Refresh();
        //            boolStatus = false;
        //            Main_Report_Processing_Completed = true;
        //        }
        //    }

        //    if (Main_Report_Processing_Completed)
        //        this.rvViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ZipCode_SubreportProcessing);
        //}


        private void ZipCode_DrillthroughReport(object sender, DrillthroughEventArgs e)
        {
            LocalReport localReport = (LocalReport)e.Report;
            localReport.DataSources.Add(new ReportDataSource("OrderDetailsDataSet_OrderDetails", Summary_table));

            LocalReport report = (LocalReport)e.Report;
            ReportDataSource CaptainDataSource = new ReportDataSource("ZipCodeDataset", Summary_table);
            report.DataSources.Add(new ReportDataSource("MyData", CaptainDataSource));
        }

        int ireport = 0;
        void ZipCode_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (Summary_table != null)
            {
                ReportDataSource datasource = new ReportDataSource("ZipCodeDataset", Summary_table);
                ireport = ireport + 1;
                e.DataSources.Clear();
                e.DataSources.Add(new ReportDataSource("ZipCodeDataset", Summary_table)); // objDataSource.Tables["GetCASESNPadpyn"]));
            }
        }


        //******** Commented by Vikash
        //private void Sub_Repport()
        //{
        //    rvViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

        //    //string strFolderPath = "C:\\CapReports\\Summary_Report.rdlc";      // Run at Local System
        //    string strFolderPath = ReportPath +  "Summary_Report.rdlc";      // Run at Local System

        //    //string strFolderPath = Consts.Common.ReportFolderLocation +  Report_Name;    // Run at Server

        //    rvViewer.LocalReport.ReportPath = strFolderPath;
        //    ReportDataSource CaptainDataSource = new ReportDataSource("ZipCodeDataset", Summary_table);

        //    rvViewer.LocalReport.DataSources.Add(CaptainDataSource);
        //    rvViewer.LocalReport.Refresh();
        //    boolStatus = false;
        //}

        private void Btn_Bypass_Click(object sender, EventArgs e)
        {
            DataTable Result_Table = new DataTable();
            switch (Scr_Code)
            {
                case "CASB0004": Result_Table = Result_DataSet.Tables[3]; 
                                Report_Name = Main_Rep_Name + "Bypass_RdlC.rdlc";
                                break;
                case "CASB0014": Result_Table = Result_DataSet.Tables[3]; //12
                                Report_Name = Main_Rep_Name + "Detail_RdlC.rdlc";
                                break;
            }

            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Summary_table, Result_Table, Report_Name, "Result Table", ReportPath);
            CASB2012_AdhocRDLCForm RDLC_Form;
            if (Scr_Code == "CASB0004")
                RDLC_Form = new CASB2012_AdhocRDLCForm(Result_Table, Summary_table, Report_Name, "Result Table", ReportPath,UserId,string.Empty);
            else
                RDLC_Form = new CASB2012_AdhocRDLCForm(Summary_table, Result_Table, Report_Name, "Result Table", ReportPath, UserId, string.Empty);
            //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
            RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
            RDLC_Form.ShowDialog();
        }

        DirectoryInfo MyDir;
        private void Delete_Dynamic_RDLC_File(object sender, FormClosedEventArgs e)
        {
            CASB2012_AdhocRDLCForm form = sender as CASB2012_AdhocRDLCForm;
            //MyDir = new DirectoryInfo(Consts.Common.ReportFolderLocation + "\\"); // Run at Server
            //MyDir = new DirectoryInfo(Consts.Common.Tmp_ReportFolderLocation + "\\"); // Run at Server
            MyDir = new DirectoryInfo(ReportPath + "\\"); // Run at Server
            
            FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
            bool MasterRep_Deleted = false, SubReport_Deleted = false;
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    if (Report_Name == MyFile.Name)
                    {
                        MyFile.Delete();
                        MasterRep_Deleted = true;
                    }

                    if (MasterRep_Deleted && SubReport_Deleted)
                        break;
                }
            }
        }

        private void Btn_SNP_Details_Click(object sender, EventArgs e)
        {
            //Report_Name = RemoveBetween(Report_Name, '.', 'c');
            Report_Name = Main_Rep_Name + "SNP_IND_RdlC.rdlc";

            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Result_DataSet.Tables[5], Summary_table, Report_Name, "Result Table");
            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Detail_Rep_Required ? Result_DataSet.Tables[5] : Result_DataSet.Tables[3], Summary_table, Report_Name, "Result Table", ReportPath); //8, 5
            CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Detail_Rep_Required ? Result_DataSet.Tables[5] : Result_DataSet.Tables[3], Summary_table, Report_Name, "Result Table", ReportPath, UserId, string.Empty); //8, 5
            //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
            RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
            RDLC_Form.ShowDialog();

        }

        private void Btn_MST_Details_Click(object sender, EventArgs e)
        {
            Report_Name = Main_Rep_Name + "MST_FAM_RdlC.rdlc";

            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Result_DataSet.Tables[9], Summary_table, Report_Name, "Result Table");
            CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Detail_Rep_Required ? Result_DataSet.Tables[7] : Result_DataSet.Tables[5], Summary_table, Report_Name, "Result Table", ReportPath, UserId, string.Empty); //8, 8
            //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
            RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
            RDLC_Form.ShowDialog();

        }

        string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }



    }
}