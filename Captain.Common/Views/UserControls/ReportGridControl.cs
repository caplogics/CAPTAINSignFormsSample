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

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ReportGridControl : UserControl
    {
        #region private variables

        private CaptainModel _model = null;
        public bool blnUseHostedpageLoad = true;
        public bool blnUseHostedpageLoadDetails = true;

        #endregion
        public ReportGridControl(BaseForm baseForm, PrivilegeEntity privileges, string strReportName)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            propReportName = strReportName;
            _model = new CaptainModel();

            fillCombo();

            fillData();
            blnUseHostedpageLoad = true;
            // blnUseHostedpageLoadDetails = true;

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportName { get; set; }

        #endregion


        private void fillCombo()
        {
            cmbChartType.Visible = true;
            lblChartType.Visible = true;
            cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Column", "1"));

            cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Line", "2"));
            cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Shape", "3"));
            cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Bar", "4"));
            cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Area", "5"));
            cmbChartType.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
            cmbChartType.SelectedIndex = 3;

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

            gvwData.Rows.Add(Img_Tick, "Client Intakes", "1", "Y");
            gvwData.Rows.Add(null, "Household Members", "1", "N");
            gvwData.Rows.Add(null, "Income Verification", "1", "N");
            gvwData.Rows.Add(null, "Services Activities (CA)", "1", "N");
            gvwData.Rows.Add(null, "Outcome Indicators (MS)", "1", "N");
            gvwData.Rows.Add(null, "Service Plans", "1", "N");
            gvwData.Rows.Add(null, "Fuel Assistance Supplier Data", "1", "N");
            gvwData.Rows.Add(null, "Fuel Assistance Benefit Data", "1", "N");
            gvwData.Rows.Add(null, "Fuel Assistance Payments", "1", "N");
            gvwData.Rows.Add(null, "Full Assessment Scales Completed", "1", "N"); 
            gvwData.Rows.Add(null, "Head Start Enrollment", "1", "N");

            gvwData.CellValueChanged += new DataGridViewCellEventHandler(gvwData_CellValueChanged);
        }

        void gvwData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            gvwData.CellValueChanged -= new DataGridViewCellEventHandler(gvwData_CellValueChanged);
            if (e.ColumnIndex == gvcDatetypes.Index)
            {
                int introwindex = gvwData.CurrentCell.RowIndex;
                string strDateTypeValue = Convert.ToString(gvwData.Rows[introwindex].Cells["gvcDatetypes"].Value);
                string strtableName = Convert.ToString(gvwData.Rows[introwindex].Cells["gvtTableName"].Value);
                if (strDateTypeValue == "3" && strtableName.ToUpper() != "CASEMST")
                {
                    CommonFunctions.MessageBoxDisplay("Intake Date Does Not Select this " + strtableName + " table.");
                    gvwData.Rows[introwindex].Cells["gvcDatetypes"].Value = "1";
                }
            }
            gvwData.CellValueChanged += new DataGridViewCellEventHandler(gvwData_CellValueChanged);
        }

        //private void reportViewer1_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (blnUseHostedpageLoad)
        //    {
        //        GetReportData();


        //    }
        //}

        //TODO: Alaa - Fix Resources (Report phase)

        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Tick = String.Empty;
        string Img_Blank = String.Empty;
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


        public void GetReportData()
        {
            try
            {


                //reportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\rptAlltableData.rdlc";
                // reportViewer1.LocalReport.ReportPath = @"SSRS\Reports\rptNewAllData.rdlc";
                Microsoft.Reporting.WebForms.ReportParameter[] parameters = new Microsoft.Reporting.WebForms.ReportParameter[1];
                parameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("rptChartType", ((ListItem)cmbChartType.SelectedItem).Value.ToString());
                //this.reportViewer1.LocalReport.SetParameters(parameters);



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

                if (((ListItem)cmbRepUserId.SelectedItem).Value.ToString() != "**")
                {
                    strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
                }

                DataSet thisDataSet = DashBoardDB.GETRDLCALLDATA(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty);



                // List<CommonEntity> listrdlcdata =  _model.lookupDataAccess.GETRDLCALLDATA(strUserId, strStartDate, strEndDate, str.ToString(), "USERTYPE");

                //  UserDataFill(listrdlcdata);
                ReportDataSource datasource = new ReportDataSource("DataSet1", thisDataSet.Tables[0]);
                //reportViewer1.LocalReport.DataSources.Clear();
                //reportViewer1.LocalReport.DataSources.Add(datasource);
                //reportViewer1.LocalReport.Refresh();
                //reportViewer1.Update();

                //reportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //reportViewer2.LocalReport.ReportPath = @"SSRS\Reports\ProgramWiseReport.rdlc";//UserWiseData.rdl";

                //DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty);


                //ReportDataSource datasourcedetails = new ReportDataSource("DataSet1", thisDataSetDetails.Tables[0]);
                //reportViewer2.LocalReport.DataSources.Clear();
                //reportViewer2.LocalReport.DataSources.Add(datasourcedetails);
                //reportViewer2.LocalReport.Refresh();
                //reportViewer2.Update();

                blnUseHostedpageLoad = false;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void GetReportDataDetails()
        {
            try
            {



                //reportViewer2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //reportViewer2.LocalReport.ReportPath = @"SSRS\Reports\ProgramWiseReport.rdl";//UserWiseData.rdl";


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

                if (((ListItem)cmbRepUserId.SelectedItem).Value.ToString() != "**")
                {
                    strUserId = ((ListItem)cmbRepUserId.SelectedItem).Value.ToString();
                }



                DataSet thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, str.ToString(), string.Empty, string.Empty, string.Empty,BaseForm.BaseAgency);


                ReportDataSource datasourcedetails = new ReportDataSource("DataSet1", thisDataSetDetails.Tables[0]);
                //reportViewer2.LocalReport.DataSources.Clear();
                //reportViewer2.LocalReport.DataSources.Add(datasourcedetails);
                //reportViewer2.LocalReport.Refresh();
                //reportViewer2.Update();
                if (tabControl1.SelectedTab == tabPage2)
                    blnUseHostedpageLoadDetails = false;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
                case "Services Activities (CA)":
                    strTableName = "CASEACT";
                    break;
                case "Outcome Indicators (MS)":
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
                case "Head Start Enrollment":
                    strTableName = "CASEENRL";
                    break;
                case "Full Assessment Scales Completed":
                    strTableName = "MATASMT";
                    break;
                  
            }

            return strTableName;

        }

        private void btnGetReport_Click(object sender, EventArgs e)
        {
            GetReportData();
            GetReportDataDetails();
        }

        void UserDataFill(List<CommonEntity> listrdlcdata)
        {
            gvwUserData.Rows.Clear();
            List<CommonEntity> listsubdetails;
            var varuserid = listrdlcdata.Select(u => u.Code).Distinct();
            if (listrdlcdata.Count > 0)
            {
                foreach (var useriddata in varuserid)
                {
                    int rowindex = gvwUserData.Rows.Add(useriddata);
                    gvwUserData.Rows[rowindex].Cells["gvtUserId"].Style.BackColor = Color.AntiqueWhite;
                    gvwUserData.Rows[rowindex].Cells["gvtUserId"].Style.ForeColor = Color.Green;
                    listsubdetails = listrdlcdata.FindAll(u => u.Code.Trim() == useriddata.Trim());
                    foreach (CommonEntity listofcount in listsubdetails)
                    {
                        gvwUserData.Rows.Add(listofcount.Desc, listofcount.Extension);
                    }
                }
            }
        }

        //private void reportViewer2_HostedPageLoad(object sender, Wisej.Web.Hosts.AspPageEventArgs e)
        //{
        //    if (blnUseHostedpageLoadDetails)
        //    {
        //        GetReportDataDetails();
        //    }
        //}        

    }
}