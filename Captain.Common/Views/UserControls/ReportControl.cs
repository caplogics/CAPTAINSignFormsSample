#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

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

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ReportControl : BaseUserControl
    {
        #region private variables

        private CaptainModel _model = null;
        public bool blnUseHostedpageLoad = true;

        #endregion
        public ReportControl(BaseForm baseForm, PrivilegeEntity privileges, string strReportName)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            propReportName = strReportName;
            _model = new CaptainModel();
         
            fillCombo();
            blnUseHostedpageLoad = true;
            
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportName { get; set; }

        #endregion

        private void fillCombo()
        {

            DataSet ds1 = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();
            DataTable dt1 = ds1.Tables[0];
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Clear();
            listItem.Add(new Captain.Common.Utilities.ListItem("All Users", "**"));
            foreach (DataRow dr in dt1.Rows)
            {
                listItem.Add(new Captain.Common.Utilities.ListItem(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_EMPLOYEE_NO"].ToString()));
            }
            cmbRepUserId.Items.AddRange(listItem.ToArray());
            cmbRepUserId.SelectedIndex = 0;


            //if (propReportName == "RPINTAKE")
            //{
                cmbChartType.Visible = true;
                lblChartType.Visible = true;
                cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Column", "1"));
                
                cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Line", "2"));
                cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Shape", "3"));
                cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Bar", "4"));
                cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Area", "5"));
                cmbChartType.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
                cmbChartType.SelectedIndex = 1;
           // }
                if (Privileges.Program == "RPREPORT")
                {
                    cmbChartType.Visible = false;
                    lblChartType.Visible = false;
                }
                if (Privileges.Program == "RPREPOR1")
                {
                    cmbChartType.Visible = false;
                    lblChartType.Visible = false;
                }
        }

        //TODO: Alaa - Removed for now (report phase)

        //private void reportView_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (blnUseHostedpageLoad)
        //    {
        //        //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //        //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report1.rdlc";
        //        GetReportData();
        //        //if (Privileges.Program == "RPMEMBER")
        //        //{
        //        //    //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //        //    //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report1.rdlc";


        //        //    //DataSet ds = _model.CaseMstData.GetCaseSnpadpynDataset("01", "02", "01", "2015", string.Empty);

        //        //    //ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);

        //        //    //reportViewer1.LocalReport.DataSources.Add(datasource);
        //        //    //reportViewer1.LocalReport.Refresh();
        //        //    //blnUseHostedpageLoad = false;


        //        //    reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //        //    reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report1.rdlc";

        //        //    Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
        //        //    parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", "1");
        //        //    this.reportViewer1.LocalReport.SetParameters(parameters);
                
        //        //    string strUserId = string.Empty;
        //        //    string strDate = string.Empty;
        //        //    if (((ListItem)cmbRepUserId.SelectedItem).Value != "**")
        //        //    {
        //        //        strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
        //        //    }
        //        //    if (dtStartDate.Checked)
        //        //        strDate = dtStartDate.Value.ToShortDateString();
        //        //    string strType = "ALLMST";

        //        //    DataSet thisDataSet = DashBoardDB.GET_INTAKE_DATA(strUserId, strDate, strType);

        //        //    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //        //    reportViewer1.LocalReport.DataSources.Add(datasource);
        //        //    reportViewer1.LocalReport.Refresh();
        //        //    reportViewer1.Update();
        //        //    blnUseHostedpageLoad = false;

        //        //}
        //        //else if (Privileges.Program == "RPINTAKE")
        //        //{

        //        //    reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //        //    reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report1.rdlc";

        //        //    Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
        //        //    parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", "1");
        //        //    this.reportViewer1.LocalReport.SetParameters(parameters);

        //        //    //Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
        //        //    //parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", "1");
        //        //    //this.reportViewer1.LocalReport.SetParameters(parameters);

        //        //    //Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
        //        //    //parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", "1");
        //        //    //this.reportViewer1.LocalReport.SetParameters(parameters);

        //        //    //SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
        //        //    //SqlCommand cmd = new SqlCommand("GET_RDLC_DATA", con);
        //        //    //cmd.CommandType = CommandType.StoredProcedure;
        //        //    //SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        //    //DataSet thisDataSet = new DataSet();
        //        //    //da.Fill(thisDataSet);
        //        //    string strUserId = string.Empty;
        //        //    string strDate = string.Empty;
        //        //    if (((ListItem)cmbRepUserId.SelectedItem).Value != "**")
        //        //    {
        //        //        strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
        //        //    }
        //        //    if (dtStartDate.Checked)
        //        //        strDate = dtStartDate.Value.ToShortDateString();
        //        //    string strType = "ALLMST";

        //        //    DataSet thisDataSet = DashBoardDB.GET_INTAKE_DATA1(strUserId, strDate, strType);

        //        //    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //        //    reportViewer1.LocalReport.DataSources.Add(datasource);
        //        //    reportViewer1.LocalReport.Refresh();
        //        //    reportViewer1.Update();
        //        //    blnUseHostedpageLoad = false;
        //        //}
        //        //else
        //        //{
        //        //    reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //        //    reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report4.rdl";


        //        //    Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
        //        //    parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", "1");
        //        //    this.reportViewer1.LocalReport.SetParameters(parameters);

        //        //    SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
        //        //    SqlDataAdapter da = new SqlDataAdapter("SELECT  (SELECT COUNT(*) FROM CASEMST WHERE MST_AGENCY=DEP_AGENCY AND MST_DEPT = DEP_DEPT AND MST_PROGRAM = DEP_PROGRAM) AS MSTCOUNT, DEP_AGENCY+DEP_DEPT + DEP_PROGRAM AS DEPARTMENT FROM CASEDEP", con);

        //        //    DataSet thisDataSet = new DataSet();
        //        //    da.Fill(thisDataSet);

        //        //    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);

        //        //    reportViewer1.LocalReport.DataSources.Add(datasource);
        //        //    reportViewer1.LocalReport.Refresh();
        //        //    reportViewer1.Update();
        //        //    blnUseHostedpageLoad = false;
        //        //}
        //        ////ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //        ////ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/SSRS/Reports/Report1.rdl");
        //        //////List<ZipCodeEntity> Zipcodedata =
        //        ////DataSet ds = _model.CaseMstData.GetCaseSnpadpynDataset("01", "02", "01", string.Empty, string.Empty);//.GetZipCodeSearch(string.Empty,string.Empty,string.Empty,string.Empty);
        //        ////ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
        //        ////ReportViewer1.LocalReport.DataSources.Clear();
        //        ////ReportViewer1.LocalReport.DataSources.Add(datasource);

        //    }
        //}

        private void cmbChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (((ListItem)cmbChartType.SelectedItem).Value != "0")
            //{
            //    blnUseHostedpageLoad = true;
            //    GetReportData();              
            //}
        }

        private void btnGetReport_Click(object sender, EventArgs e)
        {
          
            GetReportData();
        }

        public void GetReportData()
        {
            try
            {
                //blnUseHostedpageLoad = true;
                if (Privileges.Program == "RPMEMBER")
                {

                    //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report1.rdlc";

                    Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
                    parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", ((ListItem)cmbChartType.SelectedItem).Value.ToString());
                    //this.reportViewer1.LocalReport.SetParameters(parameters);

                    string strUserId = string.Empty;
                    string strDate = string.Empty;
                    string strType = "ALLMST";
                    if (((ListItem)cmbRepUserId.SelectedItem).Value != "**")
                    {
                        strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
                        strType = "USERMST";
                    }
                    if (dtStartDate.Checked)
                        strDate = dtStartDate.Value.ToShortDateString();
                  

                    DataSet thisDataSet = DashBoardDB.GET_INTAKE_DATA(strUserId, strDate, strType);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);
                    //reportViewer1.LocalReport.DataSources.Clear();
                    //reportViewer1.LocalReport.DataSources.Add(datasource);
                    //reportViewer1.LocalReport.Refresh();
                    //reportViewer1.Update();
                    blnUseHostedpageLoad = false;

                }
                else if (Privileges.Program == "RPINTAKE")
                {

                    //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report4.rdlc";

                    Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
                    parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", ((ListItem)cmbChartType.SelectedItem).Value.ToString());
                    //this.reportViewer1.LocalReport.SetParameters(parameters);

                    //Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
                    //parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", "1");
                    //this.reportViewer1.LocalReport.SetParameters(parameters);

                    //Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
                    //parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", "1");
                    //this.reportViewer1.LocalReport.SetParameters(parameters);

                    //SqlConnection con = new SqlConnection(BaseForm.DataBaseConnectionString);
                    //SqlCommand cmd = new SqlCommand("GET_RDLC_DATA", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);

                    //DataSet thisDataSet = new DataSet();
                    //da.Fill(thisDataSet);
                    string strUserId = string.Empty;
                    string strDate = string.Empty;
                    string strType = "ALLAGENCY";
                    if (((ListItem)cmbRepUserId.SelectedItem).Value != "**")
                    {
                        strType = "USERAGENCY";
                        strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
                    }
                    if (dtStartDate.Checked)
                        strDate = dtStartDate.Value.ToShortDateString();
                   

                    DataSet thisDataSet = DashBoardDB.GET_INTAKE_DATA1(strUserId, strDate, strType);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);
                    //reportViewer1.LocalReport.DataSources.Clear();
                    //reportViewer1.LocalReport.DataSources.Add(datasource);
                    //reportViewer1.LocalReport.Refresh();
                    //reportViewer1.Update();
                    blnUseHostedpageLoad = false;
                }
                else if (Privileges.Program == "RPREPORT")
                {
                    //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report3.rdlc";

                  
                    string strUserId = string.Empty;
                    string strDate = string.Empty;
                    string strType = "ALLAGENCY";
                    if (((ListItem)cmbRepUserId.SelectedItem).Value != "**")
                    {
                        strType = "USERAGENCY";
                        strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
                    }
                    if (dtStartDate.Checked)
                        strDate = dtStartDate.Value.ToShortDateString();


                    DataSet thisDataSet = DashBoardDB.GET_INTAKE_DATA1(strUserId, strDate, strType);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);
                    //reportViewer1.LocalReport.DataSources.Clear();
                    //reportViewer1.LocalReport.DataSources.Add(datasource);
                    //reportViewer1.LocalReport.Refresh();
                    //reportViewer1.Update();
                    blnUseHostedpageLoad = false;
                }
                else if (Privileges.Program == "RPREPOR1")
                {
                    //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\Report5.rdlc";


                    string strUserId = string.Empty;
                    string strDate = string.Empty;
                    string strType = "ALLAGENCY";
                    if (((ListItem)cmbRepUserId.SelectedItem).Value != "**")
                    {
                        strType = "USERAGENCY";
                        strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
                    }
                    if (dtStartDate.Checked)
                        strDate = dtStartDate.Value.ToShortDateString();


                    DataSet thisDataSet = DashBoardDB.GET_INTAKE_DATA1(strUserId, strDate, strType);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);
                    //reportViewer1.LocalReport.DataSources.Clear();
                    //reportViewer1.LocalReport.DataSources.Add(datasource);
                    //reportViewer1.LocalReport.Refresh();
                    //reportViewer1.Update();
                    blnUseHostedpageLoad = false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}