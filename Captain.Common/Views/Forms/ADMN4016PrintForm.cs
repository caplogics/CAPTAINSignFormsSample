#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
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
using Captain.Common.Views.UserControls;
using Microsoft.Reporting.WebForms;
using Wisej.Web;
using DevExpress.Utils;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using iTextSharp.text.pdf;
using DevExpress.XtraRichEdit.Model;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMN4016PrintForm : Form
    {
        public bool blnUseHostedpageLoad = true; string Rep = null;
        private CaptainModel _model = null;
        public ADMN4016PrintForm(BaseForm baseform, string types, string TabType, PrivilegeEntity privilages)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileage = privilages;
            _model = new CaptainModel();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            type = types;
            tab_type = TabType;
            if (tab_type == "CA")
            {
                this.Text = "Services Print Form";//privilages.Program + " - CA Definition";
            }
            else if (tab_type == "MS")
            {
                this.Text = "Outcomes Print Form";//privilages.Program + " - Milestone Definition";
                //chkSel.Visible = true;
            }
            else
            {
            }
        }

        public ADMN4016PrintForm(BaseForm baseform, string types, string TabType, PrivilegeEntity privilages, string strCode)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileage = privilages;
            blnUseHostedpageLoad = true;
            this.Text = "Triggers Report"; //privilages.Program + " - Triggers";
           // LblHeader.Text = "Triggers Report";
            type = types;
            lblSortBy.Visible = false;
            pnlParams.Visible = false;
            tab_type = TabType;
            propCode = strCode;
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileage { get; set; }

        public string type { get; set; }

        public string tab_type { get; set; }

        public string propCode { get; set; }

        public string propReportPath
        {
            get; set;
        }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void rblCode_CheckedChanged(object sender, EventArgs e)
        {
            //rv.Reset();
            //blnUseHostedpageLoad = true;
            //rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //rv.LocalReport.ReportPath = @"Reports\camast.rdlc";
            ////rv.LocalReport.ReportPath=Context.Server.MapPath("~\\Reports\\camast.rdlc");
            //rv.Update();

            //Added by Vikash on 10/20/2023 for generating report for Ser/Out as per Proaction document

            
            
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            Serv_Out_Excel();
            this.Close();
        }
        
        string PdfName = "Pdf File"; string Random_Filename = null;
        private void Serv_Out_Excel()
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            if(tab_type == "CA")
                PdfName = "Services_" + "Report";
            else if(tab_type == "MS")
                PdfName = "Outcomes_" + "Report";
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
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

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xls";
            try
            {
                if (tab_type == "CA")
                {
                    List<CAMASTEntity> CAMASTList;
                    CAMASTList = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
                    if (rblCode.Checked)
                    {
                        if (CAMASTList.Count > 0)
                        {
                            CAMASTList = CAMASTList.OrderByDescending(u => u.Active).ThenBy(u => u.Code).ToList();
                        }

                    }
                    else if (rblDesc.Checked)
                    {
                        if (CAMASTList.Count > 0)
                        {
                            CAMASTList = CAMASTList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
                        }
                    }

                    Workbook book = new Workbook();

                    this.GenerateStyles(book.Styles, book);

                    Worksheet sheet;
                    WorksheetCell cell;

                    WorksheetRow excelrowHeader;
                    WorksheetRow excelrowSpace;
                    WorksheetRow excelrowSpace1;
                    WorksheetRow excelrow;
                    WorksheetRow excelrow1;

                    sheet = book.Worksheets.Add("Services");
                    sheet.Table.DefaultRowHeight = 14.25F;

                    sheet.Table.DefaultColumnWidth = 220.5F;
                    sheet.Table.Columns.Add(120);
                    sheet.Table.Columns.Add(500);

                    if (CAMASTList.Count > 0)
                    {

                        excelrowSpace = sheet.Table.Rows.Add();
                        cell = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                        cell.MergeAcross = 1;

                        excelrowHeader = sheet.Table.Rows.Add();
                        cell = excelrowHeader.Cells.Add("List of Services", DataType.String, "MainHeaderStyles");
                        cell.MergeAcross = 1;

                        excelrowSpace1 = sheet.Table.Rows.Add();
                        cell = excelrowSpace1.Cells.Add("", DataType.String, "NormalLeft");
                        cell.MergeAcross = 1;

                        excelrow1 = sheet.Table.Rows.Add();
                        cell = excelrow1.Cells.Add("Code", DataType.String, "NormalLeftBold");
                        cell = excelrow1.Cells.Add("Services Description", DataType.String, "NormalLeftBold");

                        foreach (CAMASTEntity Entity in CAMASTList)
                        {
                            excelrow = sheet.Table.Rows.Add();
                            if (Entity.Active.ToString() == "False")
                            {
                                cell = excelrow.Cells.Add(Entity.Code.ToString().Trim(), DataType.String, "NormalLeftRed");
                                cell = excelrow.Cells.Add(Entity.Desc.ToString().Trim(), DataType.String, "NormalLeftRed");
                            }
                            else
                            {
                                cell = excelrow.Cells.Add(Entity.Code.ToString().Trim(), DataType.String, "NormalLeft");
                                cell = excelrow.Cells.Add(Entity.Desc.ToString().Trim(), DataType.String, "NormalLeft");
                            }

                        }
                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();

                    FileInfo fiDownload = new FileInfo(PdfName);
                    /// Need to check for file exists, is local file, is allow to read, etc...
                    string name = fiDownload.Name;
                    using (FileStream fileStream = fiDownload.OpenRead())
                    {
                        Application.Download(fileStream, name);
                    }

                    AlertBox.Show("Report Generated Successfully");
                }
                else if (tab_type == "MS")
                {
                    List<MSMASTEntity> MSMASTlist;
                    MSMASTlist = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
                    if (rblCode.Checked)
                    {
                        if (MSMASTlist.Count > 0)
                        {
                            MSMASTlist = MSMASTlist.OrderByDescending(u => u.Active).ThenBy(u => u.Code).ToList();
                        }

                    }
                    else if (rblDesc.Checked)
                    {
                        if (MSMASTlist.Count > 0)
                        {
                            MSMASTlist = MSMASTlist.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
                        }
                    }

                    Workbook book = new Workbook();

                    this.GenerateStyles(book.Styles, book);

                    Worksheet sheet;
                    WorksheetCell cell;

                    WorksheetRow excelrowHeader;
                    WorksheetRow excelrowSpace;
                    WorksheetRow excelrowSpace1;
                    WorksheetRow excelrow;
                    WorksheetRow excelrow1;

                    sheet = book.Worksheets.Add("Outcomes");
                    sheet.Table.DefaultRowHeight = 14.25F;

                    sheet.Table.DefaultColumnWidth = 220.5F;
                    sheet.Table.Columns.Add(120);
                    sheet.Table.Columns.Add(500);

                    if (MSMASTlist.Count > 0)
                    {

                        excelrowSpace = sheet.Table.Rows.Add();
                        cell = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                        cell.MergeAcross = 1;

                        excelrowHeader = sheet.Table.Rows.Add();
                        cell = excelrowHeader.Cells.Add("List of Outcomes", DataType.String, "MainHeaderStyles");
                        cell.MergeAcross = 1;

                        excelrowSpace1 = sheet.Table.Rows.Add();
                        cell = excelrowSpace1.Cells.Add("", DataType.String, "NormalLeft");
                        cell.MergeAcross = 1;

                        excelrow1 = sheet.Table.Rows.Add();
                        cell = excelrow1.Cells.Add("Code", DataType.String, "NormalLeftBold");
                        cell = excelrow1.Cells.Add("Outcomes Description", DataType.String, "NormalLeftBold");

                        foreach (MSMASTEntity Entity in MSMASTlist)
                        {
                            excelrow = sheet.Table.Rows.Add();
                            if (Entity.Active.ToString() == "False")
                            {
                                cell = excelrow.Cells.Add(Entity.Code.ToString().Trim(), DataType.String, "NormalLeftRed");
                                cell = excelrow.Cells.Add(Entity.Desc.ToString().Trim(), DataType.String, "NormalLeftRed");
                            }
                            else
                            {
                                cell = excelrow.Cells.Add(Entity.Code.ToString().Trim(), DataType.String, "NormalLeft");
                                cell = excelrow.Cells.Add(Entity.Desc.ToString().Trim(), DataType.String, "NormalLeft");
                            }
                        }
                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();

                    FileInfo fiDownload = new FileInfo(PdfName);
                    /// Need to check for file exists, is local file, is allow to read, etc...
                    string name = fiDownload.Name;
                    using (FileStream fileStream = fiDownload.OpenRead())
                    {
                        Application.Download(fileStream, name);
                    }

                    AlertBox.Show("Report Generated Successfully");
                }
            }
            catch(Exception ex)
            {
            }
        }

        private void GenerateStyles(WorksheetStyleCollection styles, Workbook book)
        {
            #region Styles
            WorksheetStyle mainstyle = book.Styles.Add("MainHeaderStyles");
            mainstyle.Font.FontName = "Tahoma";
            mainstyle.Font.Size = 12;
            mainstyle.Font.Bold = true;
            mainstyle.Font.Color = "#FFFFFF";
            mainstyle.Interior.Color = "#0070c0";
            mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;


            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Tahoma";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style1.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle stylecenter = book.Styles.Add("Normalcenter");
            stylecenter.Font.FontName = "Tahoma";
            stylecenter.Font.Bold = true;
            stylecenter.Font.Size = 11;
            stylecenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            stylecenter.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style3 = book.Styles.Add("NormalLeft");
            style3.Font.FontName = "Tahoma";
            style3.Font.Size = 10;
            style3.Interior.Color = "#f2f2f2";
            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style3.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style4 = book.Styles.Add("NormalLeftRed");
            style4.Font.FontName = "Tahoma";
            style4.Font.Size = 10;
            style4.Interior.Color = "#f2f2f2";
            style4.Font.Color = "#f00808";
            style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style4.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style15 = book.Styles.Add("NormalLeftBold");
            style15.Font.FontName = "Tahoma";
            style15.Font.Size = 11;
            style15.Font.Bold = true;
            style15.Font.Color = "#FFFFFF";
            style15.Interior.Color = "#0070c0";
            style15.Interior.Pattern = StyleInteriorPattern.Solid;
            style15.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style15.Alignment.Vertical = StyleVerticalAlignment.Center;
            style15.Alignment.WrapText = true;

            #endregion
        }

        //private void rv_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (tab_type == "CA")
        //    {
        //        if (rblCode.Checked == true)
        //        {
        //            if (blnUseHostedpageLoad)
        //            {
        //                rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                rv.LocalReport.ReportPath = @"Reports\Camast.rdlc";

        //                //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                //this.reportViewer1.LocalReport.ReportPath = Context.Server.MapPath(@"~\Resources\Report1.rdlc");
        //                //btnbool = false;
        //                //SqlConnection con = new SqlConnection(@"Data Source=SYS4\SQLEXPRESS;Initial Catalog=Capsystems;Integrated Security=True");
        //                SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
        //                SqlDataAdapter da = new SqlDataAdapter("Select * from CAMAST order by CA_CODE ", con);

        //                DataSet thisDataSet = new DataSet();
        //                da.Fill(thisDataSet);

        //                ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //                rv.LocalReport.DataSources.Add(datasource);
        //                rv.LocalReport.Refresh();
        //                blnUseHostedpageLoad = false;

        //            }
        //        }
        //        else
        //        {
        //            if (blnUseHostedpageLoad)
        //            {
        //                rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                rv.LocalReport.ReportPath = @"Reports\Camast.rdlc";

        //                //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                //this.reportViewer1.LocalReport.ReportPath = Context.Server.MapPath(@"~\Resources\Report1.rdlc");
        //                //btnbool = false;
        //                //SqlConnection con = new SqlConnection(@"Data Source=SYS4\SQLEXPRESS;Initial Catalog=Capsystems;Integrated Security=True");
        //                SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
        //                SqlDataAdapter da = new SqlDataAdapter("Select * from CAMAST order by CA_DESC ", con);

        //                DataSet thisDataSet = new DataSet();
        //                da.Fill(thisDataSet);
        //                ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //                rv.LocalReport.DataSources.Add(datasource);
        //                rv.LocalReport.Refresh();
        //                blnUseHostedpageLoad = false;
        //            }
        //        }
        //    }
        //    else if (tab_type == "TRIGGER")
        //    {
        //        if (blnUseHostedpageLoad)
        //        {
        //            rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //            rv.LocalReport.ReportPath = @"Reports\CtTriggerReport.rdlc";

        //            CaptainModel _model = new CaptainModel();
        //            DataSet thisDataSet = _model.SPAdminData.Get_CT_Trigger_Report(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, null, null, null, null, propCode.ToString());

        //            ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //            rv.LocalReport.DataSources.Add(datasource);
        //            rv.LocalReport.Refresh();
        //            blnUseHostedpageLoad = false;
        //        }
        //    }
        //    else
        //        if (rblCode.Checked == true)
        //        {
        //            if (chkSel.Checked)
        //            {
        //                if (blnUseHostedpageLoad)
        //                {
        //                    rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                    rv.LocalReport.ReportPath = @"Reports\CAMAST_Report.rdlc";

        //                    string Date = string.Empty;
        //                    if (dtpFrmDate.Checked == true) Date = dtpFrmDate.Value.ToShortDateString();

        //                    CaptainModel _model = new CaptainModel();
        //                    DataSet thisDataSet = _model.SPAdminData.Get_MS_Report("Code", "Y", Date);

        //                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //                    rv.LocalReport.DataSources.Add(datasource);
        //                    rv.LocalReport.Refresh();
        //                    blnUseHostedpageLoad = false;
        //                }
        //            }
        //            else
        //            {
        //                if (blnUseHostedpageLoad)
        //                {

        //                    rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                    rv.LocalReport.ReportPath = @"Reports\Msmast.rdlc";

        //                    //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                    //this.reportViewer1.LocalReport.ReportPath = Context.Server.MapPath(@"~\Resources\Report1.rdlc");
        //                    //btnbool = false;
        //                    //SqlConnection con = new SqlConnection(@"Data Source=SYS4\SQLEXPRESS;Initial Catalog=Capsystems;Integrated Security=True");
        //                    SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
        //                    SqlDataAdapter da = new SqlDataAdapter("Select * from MSMAST order by MS_CODE", con);

        //                    DataSet thisDataSet = new DataSet();
        //                    da.Fill(thisDataSet);
        //                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //                    rv.LocalReport.DataSources.Add(datasource);
        //                    rv.LocalReport.Refresh();
        //                    blnUseHostedpageLoad = false;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (chkSel.Checked)
        //            {
        //                if (blnUseHostedpageLoad)
        //                {
        //                    rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                    rv.LocalReport.ReportPath = @"Reports\CAMAST_Report.rdlc";

        //                    string Date = string.Empty;
        //                    if (dtpFrmDate.Checked == true) Date = dtpFrmDate.Value.ToShortDateString();

        //                    CaptainModel _model = new CaptainModel();
        //                    DataSet thisDataSet = _model.SPAdminData.Get_MS_Report("Description", "Y", Date);

        //                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //                    rv.LocalReport.DataSources.Add(datasource);
        //                    rv.LocalReport.Refresh();
        //                    blnUseHostedpageLoad = false;
        //                }
        //            }
        //            else
        //            {
        //                if (blnUseHostedpageLoad)
        //                {
        //                    rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                    rv.LocalReport.ReportPath = @"Reports\Msmast.rdlc";

        //                    //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //                    //this.reportViewer1.LocalReport.ReportPath = Context.Server.MapPath(@"~\Resources\Report1.rdlc");
        //                    //btnbool = false;
        //                    //SqlConnection con = new SqlConnection(@"Data Source=SYS4\SQLEXPRESS;Initial Catalog=Capsystems;Integrated Security=True");
        //                    SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
        //                    SqlDataAdapter da = new SqlDataAdapter("Select * from MSMAST order by MS_DESC ", con);

        //                    DataSet thisDataSet = new DataSet();
        //                    da.Fill(thisDataSet);
        //                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //                    rv.LocalReport.DataSources.Add(datasource);
        //                    rv.LocalReport.Refresh();
        //                    blnUseHostedpageLoad = false;
        //                }
        //            }
        //        }
        //}

        private void ADMN4016PrintForm_Resize(object sender, EventArgs e)
        {
            int height = this.Size.Height;
            int width = this.Size.Width;
            //if (height==441 && width==679)
            //{
            //this.rv.Size = new System.Drawing.Size(width - 10, height - 10);
            //    //this.rv.ZoomPercent = 50;

            //}
            //else
            //{
            //    this.rv.Size = new System.Drawing.Size(width - 10, height - 10);
            //    //this.rv.ZoomPercent = 150;
            //}
        }

        private void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkSel.Checked == true)
            //{
            //    dtpFrmDate.Visible = true;
            //        rv.Reset();
            //        blnUseHostedpageLoad = true;
            //        rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //        rv.LocalReport.ReportPath = @"Reports\CAMAST_Report.rdlc";
            //        //rv.LocalReport.ReportPath=Context.Server.MapPath("~\\Reports\\camast.rdlc");
            //        rv.Update();
                
            //}
            //else
            //{
            //    dtpFrmDate.Visible = false;

            //    rv.Reset();
            //    blnUseHostedpageLoad = true;
            //    rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //    rv.LocalReport.ReportPath = @"Reports\camast.rdlc";
            //    //rv.LocalReport.ReportPath=Context.Server.MapPath("~\\Reports\\camast.rdlc");
            //    rv.Update();
            //}

        }

        private void dtpFrmDate_Leave(object sender, EventArgs e)
        {
            //rv.Reset();
            //blnUseHostedpageLoad = true;
            //rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //rv.LocalReport.ReportPath = @"Reports\CAMAST_Report.rdlc";
            ////rv.LocalReport.ReportPath=Context.Server.MapPath("~\\Reports\\camast.rdlc");
            //rv.Update();
        }


    }
}