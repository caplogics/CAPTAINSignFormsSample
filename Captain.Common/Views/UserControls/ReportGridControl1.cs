#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;

using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using Captain.Common.Views.Controls;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Controls.Compatibility;
using System.IO;
using System.Threading.Tasks;
using DevExpress.DataAccess.Sql;
using CarlosAg.ExcelXmlWriter;
using DevExpress.XtraPrinting;
using DevExpress.Web.Internal.XmlProcessor;
using OfficeOpenXml;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ReportGridControl1 : UserControl
    {
        #region private variables

        private CaptainModel _model = null;
        public bool blnUseHostedpageLoad = true;
        public bool blnUseHostedpageLoadDetails = true;

        #endregion
        public ReportGridControl1(BaseForm BasePage, PrivilegeEntity privileges, string strReportName)
        {
            InitializeComponent();
            _BasePage = BasePage;
            Privileges = privileges;
            propReportName = strReportName;
            _model = new CaptainModel();
            propReportTabcount = 1;
            fillCombo();
            fillData();
            blnUseHostedpageLoad = true;
            propUserNames = string.Empty;
            // tabPage1.Hide();
            // blnUseHostedpageLoadDetails = true;
            tabControl1.SelectedIndex = 0;


            propReportPath = _model.lookupDataAccess.GetReportPath();
        }

        #region properties

        public BaseForm _BasePage { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportName { get; set; }

        public string propUserNames { get; set; }

        public string propProgrames { get; set; }

        public string propReportType { get; set; }

        public int propReportTabcount { get; set; }

        public DataTable propDatatableUser { get; set; }
        public DataTable propDatatableProgram { get; set; }

        public DataTable propDatatableSummary { get; set; }

        public List<ListItem> propApplication { get; set; }

        public DataTable propSummaryDetails
        {
            get; set;
        }

        public string propReportPath
        {
            get; set;
        }

        #endregion


        private void fillCombo()
        {
            //cmbChartType.Visible = true;
            //lblChartType.Visible = true;
            //cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Column", "1"));

            //cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Line", "2"));
            //cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Shape", "3"));
            //cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Bar", "4"));
            //cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Area", "5"));
            //cmbChartType.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            //cmbChartType.SelectedIndex = 3;

            DataSet ds1 = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();
            DataTable dt1 = ds1.Tables[0];
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Clear();
            listItem.Add(new Captain.Common.Utilities.ListItem("All Users", "**"));
            foreach (DataRow dr in dt1.Rows)
            {
                listItem.Add(new Captain.Common.Utilities.ListItem(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_EMPLOYEE_NO"].ToString()));
            }


            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            DataTable dt2 = ds.Tables[0];

            List<ListItem> listApplications = new List<ListItem>();

            foreach (DataRow dr in dt2.Rows)
            {
                listApplications.Add(new Captain.Common.Utilities.ListItem(dr["APPL_DESCRIPTION"].ToString(), dr["APPL_CODE"].ToString()));
            }
            propApplication = listApplications;

            //cmbRepUserId.Items.AddRange(listItem.ToArray());
            //cmbRepUserId.SelectedIndex = 0;

        }

        private void fillData()
        {
            gvwData.CellValueChanged -= new DataGridViewCellEventHandler(gvwData_CellValueChanged);
            DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.gvwData.Columns["gvcDatetypes"];

            List<CommonEntity> listDateTypes = new List<CommonEntity>();
            listDateTypes.Add(new CommonEntity("1", "DATE ADD"));
            listDateTypes.Add(new CommonEntity("2", "LSTC DATE"));
            listDateTypes.Add(new CommonEntity("3", "Intake DATE"));

            cb.DataSource = listDateTypes;
            cb.DisplayMember = "DESC";
            cb.ValueMember = "CODE";
            cb = (DataGridViewComboBoxColumn)this.gvwData.Columns["gvcDatetypes"];


            ListItem listheadstart = propApplication.Find(u => u.Value.ToString() == "02" || u.Value.ToString() == "03");

            if (listheadstart != null)
            {
                gvwData.Rows.Add(Img_Tick, "Client Intakes", "1", "Y");
                gvwData.Rows.Add(null, "Household Members", "1", "N");
                gvwData.Rows.Add(null, "Income Verification", "1", "N");
                gvwData.Rows.Add(null, "Enrollment", "1", "N");
                gvwData.Rows.Add(null, "Service Plans", "1", "N");
                gvwData.Rows.Add(null, "Services Activities (Services)", "1", "N");
                gvwData.Rows.Add(null, "Outcome Indicators (Outcomes)", "1", "N");
                gvwData.Rows.Add(null, "Full Assessment Scales Completed", "1", "N");
            }
            ListItem listEnergyassisteance = propApplication.Find(u => u.Value.ToString() == "08");
            if (listEnergyassisteance != null)
            {
                gvwData.Rows.Add(null, "Fuel Assistance Supplier Data", "1", "N");
                gvwData.Rows.Add(null, "Fuel Assistance Benefit Data", "1", "N");
                gvwData.Rows.Add(null, "Fuel Assistance Payments", "1", "N");
            }
            ListItem listems = propApplication.Find(u => u.Value.ToString() == "05");
            if (listems != null)
            {
                gvwData.Rows.Add(null, "Emergency Services(Resource Records)", "1", "N");
                gvwData.Rows.Add(null, "Emergency Services(Invoices/Authorizations)", "1", "N");
            }
            gvwData.CellValueChanged += new DataGridViewCellEventHandler(gvwData_CellValueChanged);

            if (gvwData.Rows.Count > 0)
                gvwData.Rows[0].Selected = true;

        }

        void gvwData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            gvwData.CellValueChanged -= new DataGridViewCellEventHandler(gvwData_CellValueChanged);
            if (e.ColumnIndex == gvcDatetypes.Index)
            {
                int introwindex = gvwData.CurrentCell.RowIndex;
                string strDateTypeValue = Convert.ToString(gvwData.Rows[introwindex].Cells["gvcDatetypes"].Value);
                string strtableName = Convert.ToString(gvwData.Rows[introwindex].Cells["gvtTableName"].Value);
                if (strDateTypeValue == "3" && strtableName.ToUpper() != "CLIENT INTAKES")
                {
                    CommonFunctions.MessageBoxDisplay("Intake Date Does Not Select this " + strtableName + " table.");
                    gvwData.Rows[introwindex].Cells["gvcDatetypes"].Value = "1";
                }
            }
            gvwData.CellValueChanged += new DataGridViewCellEventHandler(gvwData_CellValueChanged);
        }

        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Blank = "";
        string Img_Tick = Consts.Icons.ico_Tick;//"";
        private void gvwData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwData.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = gvwData.CurrentCell.ColumnIndex;
                int RowIdx = gvwData.CurrentCell.RowIndex;

                if (gviImg.Index == ColIdx)
                {
                    if (e.ColumnIndex == 0)//&& (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {
                        if (gvwData.CurrentRow.Cells["gvtSelect"].Value.ToString() == "Y")
                        {
                            gvwData.CurrentRow.Cells["gviImg"].Value = Img_Blank;
                            gvwData.CurrentRow.Cells["gvtSelect"].Value = "N";

                            //Selected_Col_Count--;

                            //Delete_SelCol_From_List();


                        }
                        else
                        {
                            gvwData.CurrentRow.Cells["gviImg"].Value = Img_Tick;
                            gvwData.CurrentRow.Cells["gvtSelect"].Value = "Y";


                        }
                    }
                }

            }



        }


        public async void GetReportDataDetails()
        {
            try
            {

                //reportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //reportViewer2.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisereport.rdlc";//UserWiseData.rdl";


                string strUserId = string.Empty;
                string strStartDate = string.Empty;
                string strEndDate = string.Empty;
                string strType = string.Empty;

                if (dtStartDate.Checked)
                {
                    strStartDate = dtStartDate.Value.ToShortDateString();
                    strEndDate = DateTime.Now.ToShortDateString();
                    if (dtEndDate.Checked)
                        strEndDate = dtEndDate.Value.ToShortDateString();
                }
                StringBuilder str = new StringBuilder();
                str.Append("<TABLE>");
                string strAddDate, strLstcDate, strIntakeDate, strCertifiedDt = string.Empty;
                foreach (DataGridViewRow item in gvwData.Rows)
                {

                    if (item.Cells["gvtSelect"].Value == "Y")
                    {
                        str.Append("<Details>");

                        str.Append("<TBNAME>" + GetTableName(item.Cells["gvtTableName"].Value.ToString()) + "</TBNAME>");

                        strAddDate = strLstcDate = strIntakeDate = strCertifiedDt = string.Empty;
                        if (dtStartDate.Checked == true)
                        {
                            if (item.Cells["gvcDatetypes"].Value == "1")
                            {
                                strAddDate = dtStartDate.Value.ToShortDateString();
                            }
                            if (item.Cells["gvcDatetypes"].Value == "2")
                            {
                                strLstcDate = dtStartDate.Value.ToShortDateString();
                            }
                            if (item.Cells["gvcDatetypes"].Value == "3")
                            {
                                strIntakeDate = dtStartDate.Value.ToShortDateString();
                            }


                            if (strAddDate != string.Empty)
                            {
                                str.Append("<TBAddStartDate>" + strAddDate + "</TBAddStartDate>");
                                str.Append("<TBAddEndDate>" + strEndDate + "</TBAddEndDate>");
                            }
                            else if (strLstcDate != string.Empty)
                            {
                                str.Append("<TBLstcStartDate>" + strLstcDate + "</TBLstcStartDate>");
                                str.Append("<TBLstcEndDate>" + strEndDate + "</TBLstcEndDate>");
                            }
                            else if (strIntakeDate != string.Empty)
                            {
                                str.Append("<TBIntakeStartDate>" + strIntakeDate + "</TBIntakeStartDate>");
                                str.Append("<TBIntakeEndDate>" + strEndDate + "</TBIntakeEndDate>");
                            }
                            else if (strCertifiedDt != string.Empty)
                            {
                                str.Append("<TBCertifiedStartDate>" + strCertifiedDt + "</TBCertifiedStartDate>");
                                str.Append("<TBCertifiedEndDate>" + strEndDate + "</TBCertifiedEndDate>");
                            }
                            //str.Append("<Details TBNAME = \"" + item.Cells["gvtTableName"].Value.ToString() + "\" TBAddDate = \"" + strAddDate + "\" TBLstcDate =\"" + strLstcDate + "\" TBIntakeDate = \"" + strIntakeDate + "\" TBCertified = \"" + strCertifiedDt + "\" />");
                        }
                        str.Append("</Details>");

                    }
                }
                str.Append("</TABLE>");



                DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty, string.Empty, string.Empty, _BasePage.BaseAgency);

                propDatatableUser = thisDataSetDetails.Tables[1];
                propDatatableProgram = thisDataSetDetails.Tables[2];
                propDatatableSummary = thisDataSetDetails.Tables[3];

                ReportDataSource datasourcedetails = new ReportDataSource("DataSet1", thisDataSetDetails.Tables[0]);

                //await LoadReport(thisDataSetDetails.Tables[0]);

                //**BindingSource bindingSource = new BindingSource(thisDataSetDetails.Tables[0]);

                //Binding datasourcedetail = new Binding("DataSet1", thisDataSetDetails.Tables[0],"");

                //DataBindings datasourcedetails = new DataBindings(thisDataSetDetails.Tables[0]);

                //PdfViewerNewForm objfrm = new PdfViewerNewForm("Report", datasourcedetails);
                //objfrm.StartPosition = FormStartPosition.CenterScreen;
                //objfrm.ShowDialog();

                //pdfUserorProgram.DataBindings.Clear();
                //pdfUserorProgram.DataBindings.Add(datasourcedetail);
                //pdfUserorProgram.Refresh();
                //pdfUserorProgram.Update();
                //pdfUserorProgram.Show();

                //reportViewer2.LocalReport.DataSources.Clear();
                //reportViewer2.LocalReport.DataSources.Add(datasourcedetails);
                //reportViewer2.LocalReport.Refresh();
                //reportViewer2.Update();
                if (tabControl1.SelectedTab == tbpUserProgView)
                    blnUseHostedpageLoadDetails = false;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private object _data;
        private LocalReportHelper _report;

        byte[] bytesglobal;
        //private async System.Threading.Tasks.Task<MemoryStream> GetMemoryStream(DataTable dtTable)
        //{
        //    ShowLoader = false;
        //    string strFolderPath = "C:\\Users\\USER\\Desktop\\Vikash\\" + propReportName;
        //    var reportHelper = LocalReportHelper.CreateWithSummaryReport(strFolderPath, dtTable);
        //    reportHelper.AddDataSource("DataSet1", this._data);
        //    byte[] bytes = reportHelper.Render("PDF");
        //    bytesglobal = bytes;
        //    _report = reportHelper;
        //    return new MemoryStream(bytes);
        //}

        //MemoryStream msGlobal = new MemoryStream();
        //private async Task LoadReport(DataTable dt)
        //{
        //    await Application.StartTask(async () =>
        //    {
        //        this.ShowLoader = true;
        //        MemoryStream ms = await GetMemoryStream(dt);
        //        msGlobal = ms;
        //        this.pdfUserorProgram.PdfStream = ms;

        //    });
        //}

        public void GetReportSummaryDetails()
        {
            try
            {

                //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //if (rdox.Checked)
                //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\summaryreport.rdlc";//UserWiseData.rdl";
                //else
                //    reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisereportProgram.rdlc";//UserWiseData.rdl";


                //string strUserId = string.Empty;
                //string strStartDate = string.Empty;
                //string strEndDate = string.Empty;
                //string strType = string.Empty;

                //if (dtStartDate.Checked)
                //{
                //    strStartDate = dtStartDate.Value.ToShortDateString();
                //    strEndDate = DateTime.Now.ToShortDateString();
                //    if (dtEndDate.Checked)
                //        strEndDate = dtEndDate.Value.ToShortDateString();
                //}
                //StringBuilder str = new StringBuilder();
                //str.Append("<TABLE>");
                //string strAddDate, strLstcDate, strIntakeDate, strCertifiedDt = string.Empty;
                //foreach (DataGridViewRow item in gvwData.Rows)
                //{

                //    if (item.Cells["gvtSelect"].Value == "Y")
                //    {
                //        str.Append("<Details>");

                //        str.Append("<TBNAME>" + GetTableName(item.Cells["gvtTableName"].Value.ToString()) + "</TBNAME>");

                //        strAddDate = strLstcDate = strIntakeDate = strCertifiedDt = string.Empty;
                //        if (dtStartDate.Checked == true)
                //        {
                //            if (item.Cells["gvcDatetypes"].Value == "1")
                //            {
                //                strAddDate = dtStartDate.Value.ToShortDateString();
                //            }
                //            if (item.Cells["gvcDatetypes"].Value == "2")
                //            {
                //                strLstcDate = dtStartDate.Value.ToShortDateString();
                //            }
                //            if (item.Cells["gvcDatetypes"].Value == "3")
                //            {
                //                strIntakeDate = dtStartDate.Value.ToShortDateString();
                //            }


                //            if (strAddDate != string.Empty)
                //            {
                //                str.Append("<TBAddStartDate>" + strAddDate + "</TBAddStartDate>");
                //                str.Append("<TBAddEndDate>" + strEndDate + "</TBAddEndDate>");
                //            }
                //            else if (strLstcDate != string.Empty)
                //            {
                //                str.Append("<TBLstcStartDate>" + strLstcDate + "</TBLstcStartDate>");
                //                str.Append("<TBLstcEndDate>" + strEndDate + "</TBLstcEndDate>");
                //            }
                //            else if (strIntakeDate != string.Empty)
                //            {
                //                str.Append("<TBIntakeStartDate>" + strIntakeDate + "</TBIntakeStartDate>");
                //                str.Append("<TBIntakeEndDate>" + strEndDate + "</TBIntakeEndDate>");
                //            }
                //            else if (strCertifiedDt != string.Empty)
                //            {
                //                str.Append("<TBCertifiedStartDate>" + strCertifiedDt + "</TBCertifiedStartDate>");
                //                str.Append("<TBCertifiedEndDate>" + strEndDate + "</TBCertifiedEndDate>");
                //            }
                //            //str.Append("<Details TBNAME = \"" + item.Cells["gvtTableName"].Value.ToString() + "\" TBAddDate = \"" + strAddDate + "\" TBLstcDate =\"" + strLstcDate + "\" TBIntakeDate = \"" + strIntakeDate + "\" TBCertified = \"" + strCertifiedDt + "\" />");
                //        }
                //        str.Append("</Details>");

                //    }
                //}
                //str.Append("</TABLE>");

                //DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty, propUserNames, propProgrames);


                ReportDataSource datasourcedetails = new ReportDataSource("DataSet1", propDatatableSummary);
                //reportViewer1.LocalReport.DataSources.Clear();
                //reportViewer1.LocalReport.DataSources.Add(datasourcedetails);
                //reportViewer1.LocalReport.Refresh();
                //reportViewer1.Update();
                intsummaryhosted++;
                if (intsummaryhosted > 2)
                { blnUseHostedpageLoad = false; }

                if (tabControl1.SelectedTab == tbpSumChart)
                    blnUseHostedpageLoad = false;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //TODO: Alaa - (Reporting Phase)
        //Gizmox.WebGUI.Reporting.ReportViewer rptdynamicviewer;
        //public void GetReportDynamicRdlcData(Gizmox.WebGUI.Reporting.ReportViewer rptviewerId)
        //{
        //    try
        //    {

        //        rptviewerId.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //        //if (rdox.Checked)
        //        //    reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\ProgramandUsersrpt.rdlc";//UserWiseData.rdl";
        //        //else
        //        if (propReportName == "Report")
        //            rptviewerId.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisereport.rdlc";//UserWiseData.rdl";
        //        if (propReportName == "Column")
        //            rptviewerId.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisecolumrpt.rdlc";
        //        if (propReportName == "Shape")
        //            rptviewerId.LocalReport.ReportPath = @"SSRS\Reports\ProgramWiseshaperpt.rdlc";
        //        if (propReportName == "Bar")
        //            rptviewerId.LocalReport.ReportPath = @"SSRS\Reports\ProgramWisebarrpt.rdlc";


        //        string strUserId = string.Empty;
        //        string strStartDate = string.Empty;
        //        string strEndDate = string.Empty;
        //        string strType = string.Empty;

        //        if (dtStartDate.Checked)
        //        {
        //            strStartDate = dtStartDate.Value.ToShortDateString();
        //            strEndDate = DateTime.Now.ToShortDateString();
        //            if (dtEndDate.Checked)
        //                strEndDate = dtEndDate.Value.ToShortDateString();
        //        }
        //        StringBuilder str = new StringBuilder();
        //        str.Append("<TABLE>");
        //        string strAddDate, strLstcDate, strIntakeDate, strCertifiedDt = string.Empty;
        //        foreach (DataGridViewRow item in gvwData.Rows)
        //        {

        //            if (item.Cells["gvtSelect"].Value == "Y")
        //            {
        //                str.Append("<Details>");

        //                str.Append("<TBNAME>" + GetTableName(item.Cells["gvtTableName"].Value.ToString()) + "</TBNAME>");

        //                strAddDate = strLstcDate = strIntakeDate = strCertifiedDt = string.Empty;
        //                if (dtStartDate.Checked == true)
        //                {
        //                    if (item.Cells["gvcDatetypes"].Value == "1")
        //                    {
        //                        strAddDate = dtStartDate.Value.ToShortDateString();
        //                    }
        //                    if (item.Cells["gvcDatetypes"].Value == "2")
        //                    {
        //                        strLstcDate = dtStartDate.Value.ToShortDateString();
        //                    }
        //                    if (item.Cells["gvcDatetypes"].Value == "3")
        //                    {
        //                        strIntakeDate = dtStartDate.Value.ToShortDateString();
        //                    }


        //                    if (strAddDate != string.Empty)
        //                    {
        //                        str.Append("<TBAddStartDate>" + strAddDate + "</TBAddStartDate>");
        //                        str.Append("<TBAddEndDate>" + strEndDate + "</TBAddEndDate>");
        //                    }
        //                    else if (strLstcDate != string.Empty)
        //                    {
        //                        str.Append("<TBLstcStartDate>" + strLstcDate + "</TBLstcStartDate>");
        //                        str.Append("<TBLstcEndDate>" + strEndDate + "</TBLstcEndDate>");
        //                    }
        //                    else if (strIntakeDate != string.Empty)
        //                    {
        //                        str.Append("<TBIntakeStartDate>" + strIntakeDate + "</TBIntakeStartDate>");
        //                        str.Append("<TBIntakeEndDate>" + strEndDate + "</TBIntakeEndDate>");
        //                    }
        //                    else if (strCertifiedDt != string.Empty)
        //                    {
        //                        str.Append("<TBCertifiedStartDate>" + strCertifiedDt + "</TBCertifiedStartDate>");
        //                        str.Append("<TBCertifiedEndDate>" + strEndDate + "</TBCertifiedEndDate>");
        //                    }
        //                    //str.Append("<Details TBNAME = \"" + item.Cells["gvtTableName"].Value.ToString() + "\" TBAddDate = \"" + strAddDate + "\" TBLstcDate =\"" + strLstcDate + "\" TBIntakeDate = \"" + strIntakeDate + "\" TBCertified = \"" + strCertifiedDt + "\" />");
        //                }
        //                str.Append("</Details>");

        //            }
        //        }
        //        str.Append("</TABLE>");



        //        DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty, propUserNames, propProgrames,BasePage.BaseAgency);



        //        ReportDataSource datasourcedetails = new ReportDataSource("DataSet1", thisDataSetDetails.Tables[0]);
        //        rptviewerId.LocalReport.DataSources.Clear();
        //        rptviewerId.LocalReport.DataSources.Add(datasourcedetails);
        //        rptviewerId.LocalReport.Refresh();
        //        rptviewerId.Update();
        //        if (tabControl1.SelectedTab.Text == propReportName)
        //        {
        //            if (propReportTabcount != 0)
        //            {
        //                if (tabControl1.TabPages.Count == propReportTabcount)
        //                    booldynamichostedData = false;
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //}


        public DataSet GetReportDynamicRdlcDateset()
        {
            DataSet thisDataSetDetails = new DataSet();
            try
            {




                string strUserId = string.Empty;
                string strStartDate = string.Empty;
                string strEndDate = string.Empty;
                string strType = string.Empty;

                if (dtStartDate.Checked)
                {
                    strStartDate = dtStartDate.Value.ToShortDateString();
                    strEndDate = DateTime.Now.ToShortDateString();
                    if (dtEndDate.Checked)
                        strEndDate = dtEndDate.Value.ToShortDateString();
                }
                StringBuilder str = new StringBuilder();
                str.Append("<TABLE>");
                string strAddDate, strLstcDate, strIntakeDate, strCertifiedDt = string.Empty;
                foreach (DataGridViewRow item in gvwData.Rows)
                {

                    if (item.Cells["gvtSelect"].Value == "Y")
                    {
                        str.Append("<Details>");

                        str.Append("<TBNAME>" + GetTableName(item.Cells["gvtTableName"].Value.ToString()) + "</TBNAME>");

                        strAddDate = strLstcDate = strIntakeDate = strCertifiedDt = string.Empty;
                        if (dtStartDate.Checked == true)
                        {
                            if (item.Cells["gvcDatetypes"].Value == "1")
                            {
                                strAddDate = dtStartDate.Value.ToShortDateString();
                            }
                            if (item.Cells["gvcDatetypes"].Value == "2")
                            {
                                strLstcDate = dtStartDate.Value.ToShortDateString();
                            }
                            if (item.Cells["gvcDatetypes"].Value == "3")
                            {
                                strIntakeDate = dtStartDate.Value.ToShortDateString();
                            }


                            if (strAddDate != string.Empty)
                            {
                                str.Append("<TBAddStartDate>" + strAddDate + "</TBAddStartDate>");
                                str.Append("<TBAddEndDate>" + strEndDate + "</TBAddEndDate>");
                            }
                            else if (strLstcDate != string.Empty)
                            {
                                str.Append("<TBLstcStartDate>" + strLstcDate + "</TBLstcStartDate>");
                                str.Append("<TBLstcEndDate>" + strEndDate + "</TBLstcEndDate>");
                            }
                            else if (strIntakeDate != string.Empty)
                            {
                                str.Append("<TBIntakeStartDate>" + strIntakeDate + "</TBIntakeStartDate>");
                                str.Append("<TBIntakeEndDate>" + strEndDate + "</TBIntakeEndDate>");
                            }
                            else if (strCertifiedDt != string.Empty)
                            {
                                str.Append("<TBCertifiedStartDate>" + strCertifiedDt + "</TBCertifiedStartDate>");
                                str.Append("<TBCertifiedEndDate>" + strEndDate + "</TBCertifiedEndDate>");
                            }
                            //str.Append("<Details TBNAME = \"" + item.Cells["gvtTableName"].Value.ToString() + "\" TBAddDate = \"" + strAddDate + "\" TBLstcDate =\"" + strLstcDate + "\" TBIntakeDate = \"" + strIntakeDate + "\" TBCertified = \"" + strCertifiedDt + "\" />");
                        }
                        str.Append("</Details>");

                    }
                }
                str.Append("</TABLE>");


                thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty, propUserNames, propProgrames, _BasePage.BaseAgency);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return thisDataSetDetails;
        }

        public string GetReportDynamicRdlcTable()
        {
            string strtabledata = string.Empty;
            try
            {




                string strUserId = string.Empty;
                string strStartDate = string.Empty;
                string strEndDate = string.Empty;
                string strType = string.Empty;

                if (dtStartDate.Checked)
                {
                    strStartDate = dtStartDate.Value.ToShortDateString();
                    strEndDate = DateTime.Now.ToShortDateString();
                    if (dtEndDate.Checked)
                        strEndDate = dtEndDate.Value.ToShortDateString();
                }
                StringBuilder str = new StringBuilder();
                str.Append("<TABLE>");
                string strAddDate, strLstcDate, strIntakeDate, strCertifiedDt = string.Empty;
                foreach (DataGridViewRow item in gvwData.Rows)
                {

                    if (item.Cells["gvtSelect"].Value == "Y")
                    {
                        str.Append("<Details>");

                        str.Append("<TBNAME>" + GetTableName(item.Cells["gvtTableName"].Value.ToString()) + "</TBNAME>");

                        strAddDate = strLstcDate = strIntakeDate = strCertifiedDt = string.Empty;
                        if (dtStartDate.Checked == true)
                        {
                            if (item.Cells["gvcDatetypes"].Value == "1")
                            {
                                strAddDate = dtStartDate.Value.ToShortDateString();
                            }
                            if (item.Cells["gvcDatetypes"].Value == "2")
                            {
                                strLstcDate = dtStartDate.Value.ToShortDateString();
                            }
                            if (item.Cells["gvcDatetypes"].Value == "3")
                            {
                                strIntakeDate = dtStartDate.Value.ToShortDateString();
                            }


                            if (strAddDate != string.Empty)
                            {
                                str.Append("<TBAddStartDate>" + strAddDate + "</TBAddStartDate>");
                                str.Append("<TBAddEndDate>" + strEndDate + "</TBAddEndDate>");
                            }
                            else if (strLstcDate != string.Empty)
                            {
                                str.Append("<TBLstcStartDate>" + strLstcDate + "</TBLstcStartDate>");
                                str.Append("<TBLstcEndDate>" + strEndDate + "</TBLstcEndDate>");
                            }
                            else if (strIntakeDate != string.Empty)
                            {
                                str.Append("<TBIntakeStartDate>" + strIntakeDate + "</TBIntakeStartDate>");
                                str.Append("<TBIntakeEndDate>" + strEndDate + "</TBIntakeEndDate>");
                            }
                            else if (strCertifiedDt != string.Empty)
                            {
                                str.Append("<TBCertifiedStartDate>" + strCertifiedDt + "</TBCertifiedStartDate>");
                                str.Append("<TBCertifiedEndDate>" + strEndDate + "</TBCertifiedEndDate>");
                            }
                            //str.Append("<Details TBNAME = \"" + item.Cells["gvtTableName"].Value.ToString() + "\" TBAddDate = \"" + strAddDate + "\" TBLstcDate =\"" + strLstcDate + "\" TBIntakeDate = \"" + strIntakeDate + "\" TBCertified = \"" + strCertifiedDt + "\" />");
                        }
                        str.Append("</Details>");

                    }
                }
                str.Append("</TABLE>");

                strtabledata = str.ToString();
                //thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty, propUserNames, propProgrames);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return strtabledata;
        }


        private string GetTableName(string strDesc)
        {
            string strTableName = string.Empty;
            switch (strDesc)
            {
                case "Client Intakes":
                    strTableName = "CASEMST";
                    break;
                case "Household Members":
                    strTableName = "CASESNP";
                    break;
                case "Income Details":
                    strTableName = "CASEINCOME";
                    break;
                case "Income Verification":
                    strTableName = "CASEVER";
                    break;
                case "Services Activities (Services)":
                    strTableName = "CASEACT";
                    break;
                case "Outcome Indicators (Outcomes)":
                    strTableName = "CASEMS";
                    break;
                case "Service Plans":
                    strTableName = "CASESPM";
                    break;
                case "Fuel Assistance Supplier Data":
                    strTableName = "LIHEAPV";
                    break;
                case "Fuel Assistance Benefit Data":
                    strTableName = "LIHEAPB";
                    break;
                case "Fuel Assistance Payments":
                    strTableName = "PAYMENT";
                    break;
                case "Enrollment":
                    strTableName = "CASEENRL";
                    break;
                case "Full Assessment Scales Completed":
                    strTableName = "MATASMT";
                    break;
                case "Emergency Services(Resource Records)":
                    strTableName = "EMSRES";
                    break;
                case "Emergency Services(Invoices/Authorizations)":
                    strTableName = "EMSCLCPMC";
                    break;


            }

            return strTableName;

        }

        private void btnGetReport_Click(object sender, EventArgs e)
        {
            propUserNames = string.Empty;

            tabControl1.SelectedIndex = 0;
            intsummaryhosted = 0;
            //tabPage1.Hide();
            GetReportDataDetails();
            tabControl1.SelectedIndex = 0;
            tabControl1.SelectedIndex = 0;

            blnUseHostedpageLoad = true;
            if (propDatatableProgram.Rows.Count > 0 || propDatatableUser.Rows.Count > 0)
            {
                //**btnUserId.Visible = true;
                //**btnUserId.Enabled = true;
            }
            else
            {
                btnUserId.Enabled = btnUserId.Visible = false;
            }

            GetReportDataDetails_Excel();
        }

        void UserDataFill(DataTable dt)
        {
            gvwUserData.Rows.Clear();
            //List<CommonEntity> listsubdetails;
            //var varuserid = listrdlcdata.Select(u => u.Code).Distinct();
            //if (listrdlcdata.Count > 0)
            //{
            //    foreach (var useriddata in varuserid)
            //    {
            //        int rowindex = gvwUserData.Rows.Add(useriddata);
            //        gvwUserData.Rows[rowindex].Cells["gvtUserId"].Style.BackColor = Color.AntiqueWhite;
            //        gvwUserData.Rows[rowindex].Cells["gvtUserId"].Style.ForeColor = Color.Green;
            //        listsubdetails = listrdlcdata.FindAll(u => u.Code.Trim() == useriddata.Trim());
            //        foreach (CommonEntity listofcount in listsubdetails)
            //        {
            //            gvwUserData.Rows.Add(listofcount.Desc, listofcount.Extension);
            //        }
            //    }
            //}

        }
        void fillProgramDefinationData(DataTable dt)
        {
            //List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty);
            //foreach (ProgramDefinitionEntity programEntity in programEntityList)
            //{
            //    string code = programEntity.Agency +  programEntity.Dept +  programEntity.Prog;

            //    int rowIndex = gvwUserData.Rows.Add(Img_Blank, code, programEntity.DepAGCY,"N");
            //}

            gvwUserData.Rows.Clear();
            foreach (DataRow dtrows in dt.Rows)
            {
                //string code = programEntity.Agency + programEntity.Dept + programEntity.Prog;

                int rowIndex = gvwUserData.Rows.Add(Img_Blank, dtrows["Programcode"].ToString(), dtrows["Program"].ToString(), "N");
            }
        }


        //Alaa - reporting phases
        //private void reportViewer2_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (blnUseHostedpageLoadDetails)
        //    {
        //        GetReportDataDetails();
        //    }
        //}

        bool booldynamichostedData = true;
        //private void reportViewerdyanamic_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (booldynamichostedData)
        //        GetReportDynamicRdlcData(rptdynamicviewer);

        //}


        private void btnUserId_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count < 6)
            {
                string strUserId = string.Empty;
                string strStartDate = string.Empty;
                string strEndDate = string.Empty;
                string strType = string.Empty;

                if (dtStartDate.Checked)
                {
                    strStartDate = dtStartDate.Value.ToShortDateString();
                    strEndDate = DateTime.Now.ToShortDateString();
                    if (dtEndDate.Checked)
                        strEndDate = dtEndDate.Value.ToShortDateString();
                }
                string strTableData = GetReportDynamicRdlcTable();
                RdlcUserForm userform = new RdlcUserForm(propUserNames, propDatatableUser, string.Empty, string.Empty, propDatatableProgram, strStartDate, strEndDate, strTableData, _BasePage);
                // userform.FormClosed += new Wisej.Web.Form.FormClosedEventHandler(userform_FormClosed);
                userform.StartPosition = FormStartPosition.CenterScreen;
                userform.ShowDialog();
            }
        }

        void userform_FormClosed(object sender, FormClosedEventArgs e)
        {
            RdlcUserForm rdlcform = sender as RdlcUserForm;
            if (rdlcform.DialogResult == DialogResult.OK)
            {

                ////booldynamichostedData = true;
                propUserNames = rdlcform.propUserName;
                propProgrames = rdlcform.propProgramName;
                propReportName = rdlcform.propReportType;


                DataSet ds = GetReportDynamicRdlcDateset();
                PrintRdlcForm printrdlcform = new PrintRdlcForm(ds, propReportName);
                printrdlcform.StartPosition = FormStartPosition.CenterScreen;
                printrdlcform.ShowDialog();

                //propReportTabcount = 0;
                //TabPage tappage1 = new TabPage();
                //Gizmox.WebGUI.Reporting.ReportViewer rptviewer = new Gizmox.WebGUI.Reporting.ReportViewer();
                //rptviewer.ID = "Rpt" + tabControl1.TabPages.Count;
                //tappage1.Text = propReportName;
                //rptviewer.Dock = Wisej.Web.DockStyle.Fill;
                //rptviewer.AsyncRendering = false;
                //tappage1.Controls.Add(rptviewer);
                //rptviewer.HostedPageLoad += new Wisej.Web.Hosts.AspPageEventHandler(reportViewerdyanamic_HostedPageLoad);
                //rptdynamicviewer = rptviewer;
                //GetReportDynamicRdlcData(rptdynamicviewer);
                //tabControl1.TabPages.Add(tappage1);
                //tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                //propReportTabcount = tabControl1.TabPages.Count;               

            }
        }


        private void gvwUserData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwUserData.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = gvwUserData.CurrentCell.ColumnIndex;
                int RowIdx = gvwUserData.CurrentCell.RowIndex;

                if (gvIData.Index == ColIdx)
                {
                    if (e.ColumnIndex == 0)//&& (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {
                        if (gvwUserData.CurrentRow.Cells["gvtSelectuser"].Value.ToString() == "Y")
                        {
                            gvwUserData.CurrentRow.Cells["gvIData"].Value = Img_Blank;
                            gvwUserData.CurrentRow.Cells["gvtSelectuser"].Value = "N";

                        }
                        else
                        {
                            gvwUserData.CurrentRow.Cells["gvIData"].Value = Img_Tick;
                            gvwUserData.CurrentRow.Cells["gvtSelectuser"].Value = "Y";
                        }
                    }
                }
            }

        }

        int intsummaryhosted = 0;

        //Alaa - reporting phase
        //private void reportViewer1_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (blnUseHostedpageLoad)
        //    {
        //        GetReportSummaryDetails();
        //    }
        //}

        private void btnSummary_Click(object sender, EventArgs e)
        {
            TabPage tappage1 = new TabPage();
            tappage1.Text = "Report" + tabControl1.TabPages.Count + 1;
            tabControl1.TabPages.Add(tappage1);
            //string strprogrames = string.Empty;
            //foreach (DataGridViewRow item in gvwUserData.Rows)
            //{

            //    if (item.Cells["gvtSelectuser"].Value == "Y")
            //    {
            //        if (strprogrames.Trim() == string.Empty)
            //            strprogrames = item.Cells["gvtprogramCode"].Value.ToString() + ",";
            //        else
            //            strprogrames = strprogrames + item.Cells["gvtprogramCode"].Value.ToString() + ",";
            //    }
            //}
            //if (strprogrames != string.Empty && propUserNames != string.Empty)
            //{
            //    propProgrames = strprogrames;
            //    GetReportSummaryDetails();
            //    tabControl1.SelectedIndex = 0;
            //}
            //else
            //{
            //    CommonFunctions.MessageBoxDisplay("Please select users or programes");
            //}
        }

        private void tabControl1_CloseClick(object sender, EventArgs e)
        {
            //string strtabname = tabControl1.SelectedItem.Text;
            //if (strtabname != "User/Program View")
            //{
            //    tabControl1.TabPages.Remove(tabControl1.SelectedTab);             
            //    tabControl1.SelectedIndex = 1;
            //    reportViewer2.Update();
            //}

        }

        //Added by Vikash on 02/12/2024 as a part of request in Priority 02122024

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s15
            // -----------------------------------------------
            WorksheetStyle s15 = styles.Add("s15");
            s15.Font.FontName = "Calibri";
            s15.Font.Size = 11;
            // -----------------------------------------------
            //  s15B
            // -----------------------------------------------
            WorksheetStyle s15B = styles.Add("s15B");
            s15B.Font.FontName = "Calibri";
            s15B.Font.Bold = true;
            s15B.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s15B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s15B.Font.Size = 11;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            s16.Font.Bold = true;
            s16.Font.FontName = "Arial";
            s16.Font.Color = "#000000";
            s16.Interior.Color = "#DDEBF7";
            s16.Interior.Pattern = StyleInteriorPattern.Solid;
            s16.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16.Alignment.WrapText = true;
            s16.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s16.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s16.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s16.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s16.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.Font.Bold = true;
            s17.Font.FontName = "Arial";
            s17.Font.Color = "#000000";
            s17.Interior.Color = "#E2EFDA";
            s17.Interior.Pattern = StyleInteriorPattern.Solid;
            s17.Alignment.Vertical = StyleVerticalAlignment.Top;
            s17.Alignment.WrapText = true;
            s17.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s17.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s17.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s17.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s17.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            s18.Font.Bold = true;
            s18.Font.FontName = "Arial";
            s18.Font.Color = "#000000";
            s18.Interior.Color = "#E7E6E6";
            s18.Interior.Pattern = StyleInteriorPattern.Solid;
            s18.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s18.Alignment.Rotate = -90;
            s18.Alignment.Vertical = StyleVerticalAlignment.Top;
            s18.Alignment.WrapText = true;
            s18.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s18.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s18.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            s19.Font.Color = "#000000";
            s19.Interior.Color = "#DDEBF7";
            s19.Interior.Pattern = StyleInteriorPattern.Solid;
            s19.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19.Alignment.WrapText = true;
            s19.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s19.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s19.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s19.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s19.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s19B
            // -----------------------------------------------
            WorksheetStyle s19B = styles.Add("s19B");
            s19B.Font.FontName = "Arial";
            s19B.Font.Color = "#000000";
            s19B.Interior.Color = "#DDEBF7";
            s19B.Interior.Pattern = StyleInteriorPattern.Solid;
            s19B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19B.Alignment.WrapText = true;
            s19B.Font.Bold = true;
            s19B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s19B.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s19B.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s19B.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s19B.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Font.FontName = "Arial";
            s20.Font.Color = "#000000";
            s20.Interior.Color = "#E2EFDA";
            s20.Interior.Pattern = StyleInteriorPattern.Solid;
            s20.Alignment.Vertical = StyleVerticalAlignment.Top;
            s20.Alignment.WrapText = true;
            s20.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s20.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s20.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s20.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s20.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s20B
            // -----------------------------------------------
            WorksheetStyle s20B = styles.Add("s20B");
            s20B.Font.FontName = "Arial";
            s20B.Font.Color = "#000000";
            s20B.Font.Bold = true;
            s20B.Interior.Color = "#E2EFDA";
            s20B.Interior.Pattern = StyleInteriorPattern.Solid;
            s20B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s20B.Alignment.WrapText = true;
            s20B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s20B.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s20B.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s20B.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s20B.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.FontName = "Arial";
            s21.Font.Color = "#000000";
            s21.Interior.Color = "#F2F2F2";
            s21.Interior.Pattern = StyleInteriorPattern.Solid;
            s21.Alignment.Vertical = StyleVerticalAlignment.Top;
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s21.Alignment.WrapText = true;
            s21.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s21.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s21.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s21.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s21.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s21B
            // -----------------------------------------------
            WorksheetStyle s21B = styles.Add("s21B");
            s21B.Font.FontName = "Arial";
            s21B.Font.Color = "#000000";
            s21B.Font.Bold = true;
            s21B.Interior.Color = "#F2F2F2";
            s21B.Interior.Pattern = StyleInteriorPattern.Solid;
            s21B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s21B.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s21B.Alignment.WrapText = true;
            s21B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s21B.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s21B.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s21B.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s21B.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
            // -----------------------------------------------
            //  s22
            // -----------------------------------------------
            WorksheetStyle s22 = styles.Add("s22");
            s22.Font.Bold = true;
            s22.Font.FontName = "Arial";
            s22.Font.Color = "#2E8B57";
            s22.Alignment.Vertical = StyleVerticalAlignment.Top;
            s22.Alignment.WrapText = true;
            s22.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s22.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#D3D3D3");
            s22.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#D3D3D3");
            s22.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#D3D3D3");
            s22.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#D3D3D3");
        }

        string Random_Filename = string.Empty;
        string PdfName = string.Empty;
        private void GetReportDataDetails_Excel()
        {

            PdfName = "Productivity_Report";

            PdfName = propReportPath + _BasePage.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + _BasePage.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _BasePage.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
                ex.ToString();
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

            string strPdfHeader = PdfName;
            if (!string.IsNullOrEmpty(Random_Filename))
            {
                PdfName = Random_Filename;
                strPdfHeader = Random_Filename;
            }
            else
                PdfName += ".xls";

            string strUserId = string.Empty;
            string strStartDate = string.Empty;
            string strEndDate = string.Empty;
            string strType = string.Empty;

            if (dtStartDate.Checked)
            {
                strStartDate = dtStartDate.Value.ToShortDateString();
                strEndDate = DateTime.Now.ToShortDateString();
                if (dtEndDate.Checked)
                    strEndDate = dtEndDate.Value.ToShortDateString();
            }

            StringBuilder str = new StringBuilder();
            str.Append("<TABLE>");
            string strAddDate, strLstcDate, strIntakeDate, strCertifiedDt = string.Empty;

            foreach (DataGridViewRow item in gvwData.Rows)
            {
                if (item.Cells["gvtSelect"].Value == "Y")
                {
                    str.Append("<Details>");

                    str.Append("<TBNAME>" + GetTableName(item.Cells["gvtTableName"].Value.ToString()) + "</TBNAME>");

                    strAddDate = strLstcDate = strIntakeDate = strCertifiedDt = string.Empty;
                    if (dtStartDate.Checked == true)
                    {
                        if (item.Cells["gvcDatetypes"].Value == "1")
                        {
                            strAddDate = dtStartDate.Value.ToShortDateString();
                        }
                        if (item.Cells["gvcDatetypes"].Value == "2")
                        {
                            strLstcDate = dtStartDate.Value.ToShortDateString();
                        }
                        if (item.Cells["gvcDatetypes"].Value == "3")
                        {
                            strIntakeDate = dtStartDate.Value.ToShortDateString();
                        }


                        if (strAddDate != string.Empty)
                        {
                            str.Append("<TBAddStartDate>" + strAddDate + "</TBAddStartDate>");
                            str.Append("<TBAddEndDate>" + strEndDate + "</TBAddEndDate>");
                        }
                        else if (strLstcDate != string.Empty)
                        {
                            str.Append("<TBLstcStartDate>" + strLstcDate + "</TBLstcStartDate>");
                            str.Append("<TBLstcEndDate>" + strEndDate + "</TBLstcEndDate>");
                        }
                        else if (strIntakeDate != string.Empty)
                        {
                            str.Append("<TBIntakeStartDate>" + strIntakeDate + "</TBIntakeStartDate>");
                            str.Append("<TBIntakeEndDate>" + strEndDate + "</TBIntakeEndDate>");
                        }
                        else if (strCertifiedDt != string.Empty)
                        {
                            str.Append("<TBCertifiedStartDate>" + strCertifiedDt + "</TBCertifiedStartDate>");
                            str.Append("<TBCertifiedEndDate>" + strEndDate + "</TBCertifiedEndDate>");
                        }
                        //str.Append("<Details TBNAME = \"" + item.Cells["gvtTableName"].Value.ToString() + "\" TBAddDate = \"" + strAddDate + "\" TBLstcDate =\"" + strLstcDate + "\" TBIntakeDate = \"" + strIntakeDate + "\" TBCertified = \"" + strCertifiedDt + "\" />");
                    }
                    str.Append("</Details>");

                }
            }
            str.Append("</TABLE>");

            DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty, string.Empty, string.Empty, _BasePage.BaseAgency);

            propDatatableUser = thisDataSetDetails.Tables[1];
            propDatatableProgram = thisDataSetDetails.Tables[2];
            propDatatableSummary = thisDataSetDetails.Tables[3];

            propSummaryDetails = thisDataSetDetails.Tables[0];

            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);

            Worksheet sheet = book.Worksheets.Add("Program Wise Report");
            sheet.Table.DefaultRowHeight = 15F;
            //sheet.Table.ExpandedColumnCount = 37;
            //sheet.Table.ExpandedRowCount = 56;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "s15";

            try
            {

                WorksheetColumn column = sheet.Table.Columns.Add();
                column.Width = 23;
                column.StyleID = "s15";
                WorksheetColumn column01 = sheet.Table.Columns.Add();
                column01.Width = 103;
                column01.StyleID = "s15";
                WorksheetColumn column02 = sheet.Table.Columns.Add();
                column02.Width = 170;
                column02.StyleID = "s15";

                WorksheetColumn column0;
                foreach (DataRow drProgram in propDatatableProgram.Rows)
                {
                    column0 = sheet.Table.Columns.Add();
                    column0.Width = 35;
                    column0.Span = 1;
                    column0.StyleID = "s15";
                }

                WorksheetRow Row0 = sheet.Table.Rows.Add();
                Row0.Height = 23;
                WorksheetCell cell;
                cell = Row0.Cells.Add();
                //cell.MergeAcross = 35;
                Row0.AutoFitHeight = false;

                WorksheetRow Row1 = sheet.Table.Rows.Add();
                Row1.Height = 146;
                cell = Row1.Cells.Add();
                cell.StyleID = "s16";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "USER ID";
                cell.Index = 2;
                Row1.Cells.Add("Type", DataType.String, "s17");

                string presentProgram = string.Empty;
                foreach (DataRow drProgram in propDatatableProgram.Rows)
                {
                    Row1.Cells.Add(drProgram["Program"].ToString().Trim(), DataType.String, "s18");
                }
                Row1.Cells.Add("Total Count", DataType.String, "s18");

                int UserTotal = 0;

                WorksheetRow rowData; //= sheet.Table.Rows.Add();
                //rowData.Height = 20;

                DataView distData = new DataView(propSummaryDetails);
                DataTable distinctData = distData.ToTable(true, "UserId", "TBType");

                DataView dv = new DataView(distinctData);
                dv.Sort = "UserId,TBType";

                distinctData = dv.ToTable();

                if (distinctData.Rows.Count > 0)
                {
                    string _PrvUserID = "";
                    foreach (DataRow drUser in distinctData.Rows)
                    {
                        rowData = sheet.Table.Rows.Add();
                        rowData.Height = 20;

                        rowData.Cells.Add("", DataType.String, "s15");

                        if (drUser["UserId"].ToString().Trim() == "ZZTotalUsers")
                        {
                            rowData.Cells.Add("Program Totals", DataType.String, "s19B");

                            string strSevType = drUser["TBType"].ToString().Trim();
                            if (strSevType == "Services Activities (CA)")
                                strSevType = "Services Activities (Services)";
                            if (strSevType == "Outcome Indicators (MS)")
                                strSevType = "Outcome Indicators (Outcomes)";

                            rowData.Cells.Add(strSevType, DataType.String, "s20B");

                            DataRow[] drPrograms = propSummaryDetails.Select("UserId='" + drUser["UserId"].ToString() + "'");

                            DataTable dtProg = new DataTable();
                            if (drPrograms.Length > 0)
                            {
                                dtProg = drPrograms.CopyToDataTable();
                            }

                            foreach (DataRow drProgram in propDatatableProgram.Rows)
                            {
                                DataRow[] drProg = dtProg.Select("Program='" + drProgram["Program"].ToString() + "' AND TBType = '" + drUser["TBType"].ToString() + "'");

                                if (drProg.Length > 0)
                                {
                                    rowData.Cells.Add(drProg[0]["TotalCount"].ToString().Trim(), DataType.String, "s21B");

                                    UserTotal = UserTotal + Convert.ToInt32(drProg[0]["TotalCount"].ToString().Trim());
                                }
                                else
                                    rowData.Cells.Add("", DataType.String, "s21B");
                            }
                            rowData.Cells.Add(UserTotal.ToString(), DataType.String, "s15B");
                            UserTotal = 0;
                        }
                        else
                        {
                            string strUserID = "";
                            string _currUserID = drUser["UserId"].ToString().Trim();
                            if (_PrvUserID != _currUserID)
                                strUserID = _currUserID;

                            rowData.Cells.Add(strUserID, DataType.String, "s19");
                            _PrvUserID = _currUserID;

                            string strSevType = drUser["TBType"].ToString().Trim();
                            if (strSevType == "Services Activities (CA)")
                                strSevType = "Services Activities (Services)";
                            if (strSevType == "Outcome Indicators (MS)")
                                strSevType = "Outcome Indicators (Outcomes)";

                            rowData.Cells.Add(strSevType, DataType.String, "s20");

                            DataRow[] drPrograms = propSummaryDetails.Select("UserId='" + drUser["UserId"].ToString() /*+ "' AND  TBType = '" + drUser["TBType"].ToString()*/ + "'");

                            DataTable dtProg = new DataTable();
                            if (drPrograms.Length > 0)
                            {
                                dtProg = drPrograms.CopyToDataTable();
                            }

                            foreach (DataRow drProgram in propDatatableProgram.Rows)
                            {
                                //[] drProg = dtProg.Select("Program='" + drProgram["Program"].ToString() + "'");
                                DataRow[] drProg = dtProg.Select("Program='" + drProgram["Program"].ToString() + "' AND TBType = '" + drUser["TBType"].ToString() + "'");
                                if (drProg.Length > 0)
                                {
                                    rowData.Cells.Add(drProg[0]["TotalCount"].ToString().Trim(), DataType.String, "s21");

                                    UserTotal = UserTotal + Convert.ToInt32(drProg[0]["TotalCount"].ToString().Trim());
                                }
                                else
                                    rowData.Cells.Add("", DataType.String, "s21");
                            }
                            rowData.Cells.Add(UserTotal.ToString(), DataType.String, "s15B");
                            UserTotal = 0;
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
                    AlertBox.Show("Report Generated Successfully");
                }
                else
                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

    }
}