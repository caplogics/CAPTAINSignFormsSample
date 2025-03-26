#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using Captain.Common.Views.Forms.Base;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RdlcUserForm : Form
    {
        public RdlcUserForm(string struserids, DataTable dtuser, string strType, string strPrograms, DataTable dtprogram,string strstrdate,string strenddate,string strtabledata,BaseForm BasePage)
        {
            InitializeComponent();
            propType = strType;
            propDatatableuser = dtuser;
            propDatatableProgram = dtprogram;
            strstartdate =strstrdate;
            strEnddate =strenddate;
            strTabledata = strtabledata;
            _BasePage = BasePage;
            this.Text = "Select Report Parameters";
            if (propType == "USER")
            {
                pnlTop.Visible = false;
                pnlTop.Enabled = false;
                pnlBottom.Location = pnlTop.Location;
                this.Size = pnlBottom.Size;

                string response = string.Empty;
                string[] arrResponse = null;
                if (struserids != null)
                {
                    response = struserids;
                    if (response.IndexOf(',') > 0)
                    {
                        arrResponse = response.Split(',');
                    }
                    else if (!response.Equals(string.Empty))
                    {
                        arrResponse = new string[] { response };
                    }
                }
                bool boolexist = false;
                DataTable dt1 = dtuser;
                gvtPCode.Visible = false;
                gvtUserId.Width = 260;
                string strUserId = string.Empty;
                foreach (DataRow dr in dt1.Rows)
                {
                    strUserId = dr["Userid"].ToString().Trim() == "ZZTotalUsers" ? "Program totals" : dr["Userid"].ToString();
                    if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(dr["Userid"].ToString().Trim())))
                    {
                        gvwData.Rows.Add(Img_Tick, string.Empty, strUserId, "Y");
                    }
                    else
                    {
                        gvwData.Rows.Add(Img_Blank, string.Empty, strUserId, "N");
                    }
                }
                if (gvwData.Rows.Count > 0)
                    gvwData.Rows[0].Selected = true;
            }
            else if (propType == "PROGRAM")
            {
                pnlTop.Visible = false;
                pnlTop.Enabled = false;
                pnlBottom.Location = pnlTop.Location;
                this.Size = pnlBottom.Size;
                string response = string.Empty;
                string[] arrResponse = null;
                if (strPrograms != null)
                {
                    response = strPrograms;
                    if (response.IndexOf(',') > 0)
                    {
                        arrResponse = response.Split(',');
                    }
                    else if (!response.Equals(string.Empty))
                    {
                        arrResponse = new string[] { response };
                    }
                }
                bool boolexist = false;
                DataTable dt1 = dtprogram;
                foreach (DataRow dr in dt1.Rows)
                {
                    if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(dr["ProgramCode"].ToString().Trim())))
                    {
                        gvwData.Rows.Add(Img_Tick, dr["ProgramCode"].ToString(), dr["Program"].ToString(), "Y");
                    }
                    else
                    {
                        gvwData.Rows.Add(Img_Blank, dr["ProgramCode"].ToString(), dr["Program"].ToString(), "N");
                    }
                }
                if(gvwData.Rows.Count > 0)
                    gvwData.Rows[0].Selected = true;
            }
            else
            {
                chkSelectAll.Enabled = false;
                chkUnselectAll.Enabled = false;
                pnlBottom.Visible = false;
                fillCombo();
                this.Size = new Size(Width, this.Height - pnlBottom.Height);//pnlTop.Size;
            }
            //DataSet ds1 = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();

            //List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            //listItem.Clear();
            //listItem.Add(new Captain.Common.Utilities.ListItem("All Users", "**"));


            //foreach (DataRow dr in dt1.Rows)
            //{
            //    if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(dr["PWR_EMPLOYEE_NO"].ToString().Trim())))
            //    {
            //        gvwData.Rows.Add(Img_Tick, dr["PWR_EMPLOYEE_NO"].ToString(), "Y");
            //    }
            //    else
            //    {
            //        gvwData.Rows.Add(Img_Blank, dr["PWR_EMPLOYEE_NO"].ToString(), "N");
            //    }
            //}

            //DataSet ds = Captain.DatabaseLayer.UserAccess.UserSearch(null, null, null, null);
            //DataTable dt = ds.Tables[0];
            //foreach (DataRow dr in dt.Rows)
            //{
            //    string status = "Active";
            //    if (dr["PWR_INACTIVE_FLAG"].ToString().Equals("Y"))
            //    {
            //        status = "Inactive";
            //    }
            //    string strDefaultHie = string.Empty;

            //    int rowIndex = gvwData.Rows.Add(Img_Blank, dr["PWR_EMPLOYEE_NO"].ToString());
            //    gvwData.Rows[rowIndex].Tag = dr;  
            //    if (status.Equals("Inactive"))
            //    {
            //        gvwData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //    }
            //}
        }
        public string propUserName { get; set; }
        public string propProgramName { get; set; }
        public string propFilterUserName { get; set; }
        public string propFilterProgramName { get; set; }
        public string propType { get; set; }
        public DataTable propDatatableuser { get; set; }
        public DataTable propDatatableProgram { get; set; }
        public string propReportType { get; set; }

        public BaseForm _BasePage
        { get; set; }
        public string strstartdate { get; set; }
        public string strEnddate { get; set; }
        public string strTabledata { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {

            if (propType == "USER")
            {
                string strUserNames = string.Empty;
                foreach (DataGridViewRow item in gvwData.Rows)
                {

                    if (item.Cells["gvtSelect"].Value.ToString() == "Y")
                    {
                        if (strUserNames.Trim() == string.Empty)
                            strUserNames = item.Cells["gvtUserId"].Value.ToString() == "Program totals" ? "ZZTotalUsers" : item.Cells["gvtUserId"].Value.ToString() + ",";
                        else
                            strUserNames = strUserNames + (item.Cells["gvtUserId"].Value.ToString() == "Program totals" ? "ZZTotalUsers" : item.Cells["gvtUserId"].Value.ToString()) + ",";
                    }
                }
                propUserName = strUserNames;
            }
            else
            {
                string strProgramNames = string.Empty;
                foreach (DataGridViewRow item in gvwData.Rows)
                {

                    if (item.Cells["gvtSelect"].Value.ToString() == "Y")
                    {
                        if (strProgramNames.Trim() == string.Empty)
                            strProgramNames = item.Cells["gvtPCode"].Value.ToString() + ",";
                        else
                            strProgramNames = strProgramNames + item.Cells["gvtPCode"].Value.ToString() + ",";
                    }
                }
                propProgramName = strProgramNames;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
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

                if (gvISelect.Index == ColIdx)
                {
                    if (e.ColumnIndex == 0)//&& (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {
                        if (gvwData.CurrentRow.Cells["gvtSelect"].Value.ToString() == "Y")
                        {
                            gvwData.CurrentRow.Cells["gvISelect"].Value = Img_Blank;
                            gvwData.CurrentRow.Cells["gvtSelect"].Value = "N";

                        }
                        else
                        {
                            gvwData.CurrentRow.Cells["gvISelect"].Value = Img_Tick;
                            gvwData.CurrentRow.Cells["gvtSelect"].Value = "Y";
                        }
                    }
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        void userform_FormClosed(object sender, FormClosedEventArgs e)
        {
            RdlcUserForm rdlcform = sender as RdlcUserForm;
            if (rdlcform.DialogResult == DialogResult.OK)
            {
                if (rdlcform.propType == "USER")
                {
                    propUserName = rdlcform.propUserName;
                }
                else
                {
                    propProgramName = rdlcform.propProgramName;
                }
            }
        }

        private void btnGeneraterpt_Click(object sender, EventArgs e)
        {
            propReportType = ((ListItem)cmbChartType.SelectedItem).Text.ToString();
            DataSet ds = GetReportDynamicRdlcDateset();
            PrintRdlcForm printrdlcform = new PrintRdlcForm(ds, propReportType);
            printrdlcform.StartPosition = FormStartPosition.CenterScreen;
            printrdlcform.ShowDialog();
           // this.DialogResult = DialogResult.OK;
        }


        private void fillCombo()
        {
            cmbChartType.Items.Clear();
            cmbChartType.Visible = true;

            cmbChartType.Items.Add(new ListItem("Column", "1"));
            cmbChartType.Items.Add(new ListItem("Shape", "2"));
            cmbChartType.Items.Add(new ListItem("Bar", "3"));
            // cmbChartType.Items.Add(new Captain.Common.Utilities.ListItem("Area", "5"));
            cmbChartType.Items.Insert(0, new ListItem("Report", "0"));
            cmbChartType.SelectedIndex = 0;


        }

        private void rdoUSelect_Click(object sender, EventArgs e)
        {
            if (rdoUSelect.Checked)
            {
                RdlcUserForm userform = new RdlcUserForm(propUserName, propDatatableuser, "USER", propProgramName, propDatatableProgram,string.Empty,string.Empty,string.Empty, _BasePage);
                userform.FormClosed += userform_FormClosed;
                userform.StartPosition = FormStartPosition.CenterScreen;
                userform.ShowDialog();

            }
            else
            { }
        }

        private void rdoPSelect_Click(object sender, EventArgs e)
        {
            if (rdoPSelect.Checked)
            {
                RdlcUserForm userform = new RdlcUserForm(propUserName, propDatatableuser, "PROGRAM", propProgramName, propDatatableProgram,string.Empty,string.Empty,string.Empty, _BasePage);
                userform.FormClosed += userform_FormClosed;
                userform.StartPosition = FormStartPosition.CenterScreen;
                userform.ShowDialog();
            }
            else
            { }
        }

        private void chkUnselectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnselectAll.Checked)
            {
                chkSelectAll.Checked = false;
                foreach (DataGridViewRow item in gvwData.Rows)
                {
                    item.Cells["gvISelect"].Value = Img_Blank;
                    item.Cells["gvtSelect"].Value = "N";
                }
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkUnselectAll.Checked = false;
                foreach (DataGridViewRow item in gvwData.Rows)
                {
                    item.Cells["gvISelect"].Value = Img_Tick;
                    item.Cells["gvtSelect"].Value = "Y";
                }

            }
            else if (chkSelectAll.Checked == false)
            {
                foreach (DataGridViewRow item in gvwData.Rows)
                {
                    item.Cells["gvISelect"].Value = Img_Blank;
                    item.Cells["gvtSelect"].Value = "N";
                }
            }
        }


        public DataSet GetReportDynamicRdlcDateset()
        {
            DataSet thisDataSetDetails = new DataSet();
            try
            {


                string strUserId = string.Empty;
                string strStartDate = string.Empty;
                string strEndDate = string.Empty;
                string strType = string.Empty;

                if (strstartdate!=string.Empty)
                {
                    strStartDate = Convert.ToDateTime(strstartdate).ToShortDateString();
                    strEndDate = DateTime.Now.ToShortDateString();
                    if (strEndDate!=string.Empty)
                        strEndDate = Convert.ToDateTime(strEndDate).ToShortDateString();
                }

                string strProgrames = string.Empty;
                string strUsers = string.Empty;
                if(rdoUSelect.Checked)
                    strUsers = propUserName;
                if (rdoPSelect.Checked)
                    strProgrames = propProgramName;

                thisDataSetDetails = DashBoardDB.GETRDLCALLDATANew123(strUserId, strStartDate, strEndDate, strTabledata.ToString(), string.Empty, strUsers, strProgrames, _BasePage.BaseAgency);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return thisDataSetDetails;
        }

    }
}