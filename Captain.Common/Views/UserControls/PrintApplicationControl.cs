#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;

using Wisej.Web;
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
using System.Text.RegularExpressions;
using System.IO;
using DevExpress.Pdf;
using DevExpress.XtraReports.UI;
using DevExpress.Data.Browsing.Design;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.Cms;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class PrintApplicationControl : BaseUserControl
    {
        string _agency = "";
        public PrintApplicationControl(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _agency = BaseForm.BaseAdminAgency;
            Privileges = privileges;
            PopulateToolbar(oToolbarMnustrip);
            FillAppGrid();

            if(tbcAppandPrintLetter.SelectedIndex == 1)
                FillPrintLetterGrid();
        }

        private void FillPrintLetterGrid()
        {
            dgvPrintLetters.Rows.Clear();
            int rowIndex = 0;

            DataSet ds = new DataSet();

            ds = DatabaseLayer.MainMenu.GET_PRINAPPCNTL(string.Empty, string.Empty, string.Empty,"L");

            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                //ds = DatabaseLayer.MainMenu.GET_PRINAPPCNTL("**", string.Empty, string.Empty,"L");
                //dt = ds.Tables[0];
                if (BaseForm.BaseAdminAgency != "**")
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "PAC_AGY='" + BaseForm.BaseAdminAgency + "'";
                    dt = dv.ToTable();
                }
            }

            if (dt.Rows.Count == 0)
                dt = ds.Tables[0];
           

            if (dt.Rows.Count > 0)
            {
                if (BaseForm.BaseAdminAgency == "**")
                {
                    //DataSet ds1 = DatabaseLayer.MainMenu.GET_PRINAPPCNTL(string.Empty, string.Empty, "**");

                    DataView dv2 = new DataView(dt);
                    dv2.RowFilter = "PAC_AGY='" + "**" + "'";
                    DataTable dtAll = dv2.ToTable();

                    if (dtAll.Rows.Count == 0)
                    {
                        DataTable dtApp = CreatePRINTLetters();
                        
                        foreach (DataRow dr in dtApp.Rows)
                        {
                            rowIndex= dgvPrintLetters.Rows.Add(dr["PAC_FORM"], false, dr["PAC_DISP_NAME"], false, BaseForm.BaseAdminAgency, "A", "L");

                            if (BaseForm.BaseAgencyControlDetails.TXAlienSwitch != "Y" && dr["PAC_FORM"].ToString()=="10")
                                dgvPrintLetters.Rows[rowIndex].Visible= false;
                                

                        }
                    }
                }
                foreach (DataRow dr in dt.Rows)
                {
                    bool enableflag = false;
                    bool isSigned = false;

                    if (dr["PAC_ENABLE"].ToString() == "Y")
                        enableflag = true;


                    if (dr["PAC_SIGN"].ToString() == "Y")
                        isSigned = true;

                    rowIndex= dgvPrintLetters.Rows.Add(dr["PAC_FORM"], enableflag, dr["PAC_DISP_NAME"], isSigned, dr["PAC_AGY"], "U","L");

                    if (BaseForm.BaseAgencyControlDetails.TXAlienSwitch != "Y" && dr["PAC_FORM"].ToString() == "10")
                        dgvPrintLetters.Rows[rowIndex].Visible = false;

                }
                //Mode = "UPDATE";
                ToolBarNew.Visible = false;
                ToolBarEdit.Visible = true; ToolBarEdit.Enabled = true;
            }
            else
            {
               
                DataTable dtApp = CreatePRINTLetters();
                foreach (DataRow dr in dtApp.Rows)
                {
                    rowIndex= dgvPrintLetters.Rows.Add(dr["PAC_FORM"], false, dr["PAC_DISP_NAME"],false, BaseForm.BaseAdminAgency, "A");

                    if (BaseForm.BaseAgencyControlDetails.TXAlienSwitch != "Y" && dr["PAC_FORM"].ToString() == "10")
                        dgvPrintLetters.Rows[rowIndex].Visible = false;
                }
                //Mode = "ADD";
                ToolBarNew.Visible = true; ToolBarNew.Enabled = true;
                ToolBarEdit.Visible = false;
            }

        }
        #region Properties

        public DataTable CreatePRINTApp()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PAC_FORM", typeof(string));
            dt.Columns.Add("PAC_DISP_NAME", typeof(string));

            dt.Rows.Add("Case Management Application", "Case Management Application");
            dt.Rows.Add("Head Start Application", "Head Start Application");
            dt.Rows.Add("SIM Referral Letter", "SIM Referral Letter");
            dt.Rows.Add("Old Application for Assistance", "Case Management Application");
            dt.Rows.Add("Application for Assistance", "Application for Assistance");
            dt.Rows.Add("Print Letter", "Print Letter");
            dt.Rows.Add("Print Water & Sewer Letters", "Print Water & Sewer Letters");
            dt.Rows.Add("Energy Assistance Application", "Energy Assistance Application");
            dt.Rows.Add("Pre-Assessment Form", "Pre-Assessment Form");
            dt.Rows.Add("Emergency Sheet", "Emergency Sheet");

            return dt;
        }

        public DataTable CreatePRINTLetters()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PAC_FORM", typeof(string));
            dt.Columns.Add("PAC_DISP_NAME", typeof(string));

            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC")
            {
                dt.Rows.Add("1", "Delay-in-eligibility determination");
                dt.Rows.Add("2", "Eligibility/Denial Notification");
                dt.Rows.Add("4", "Right to Appeal Notice");
                dt.Rows.Add("5", "CEAP Benefit fulfillment form");
                dt.Rows.Add("6", "Client satisfaction survey");
                dt.Rows.Add("7", "Termination Notification");
                dt.Rows.Add("8", "CEAP Priority Rating Form");
                dt.Rows.Add("9", "Declaration of Income Statement");
            }
            else
            {
                dt.Rows.Add("1", "Eligibilty Letter");
                dt.Rows.Add("2", "CEAP Priority Rating Form");

                if (BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC")
                    dt.Rows.Add("3", "Notice of Payment");
            }
            //if (BaseForm.BaseAgencyControlDetails.TXAlienSwitch == "Y")
                dt.Rows.Add("10", "Client Intake & SAVE Form");

            return dt;
        }

        public PrivilegeEntity Privileges { get; set; }
        public ToolBarButton ToolBarEdit { get; set; }
        public ToolBarButton ToolBarNew { get; set; }
        #endregion
        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (toolBar.Controls.Count == 0)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Add";//"Add Print Application Name";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.AddPriv.Equals("false"))
                {
                    ToolBarNew.Enabled = false;
                }

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit";//"Edit Print Application Name";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.ChangePriv.Equals("false"))
                {
                    ToolBarEdit.Enabled = false;
                }

                if (Privileges.AddPriv.Equals("false"))
                {
                    if (ToolBarNew != null) ToolBarNew.Enabled = false;
                }
                else
                {
                    if (ToolBarNew != null) ToolBarNew.Enabled = true;
                }

                if (Privileges.ChangePriv.Equals("false"))
                {
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                }
                else
                {
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
                }

                toolBar.Buttons.AddRange(new ToolBarButton[]
                {
                ToolBarNew,
                ToolBarEdit,
                });
            }
        }

        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();

            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null) { return; }

            try
            {

                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:
                        dgvApplications.Enabled = true; dgvPrintLetters.Enabled = true; btnSave.Visible = btnCancel.Visible = true;
                        disableTabs("D");
                        ToolBarNew.Enabled = false;
                        break;

                    case Consts.ToolbarActions.Edit:
                        ToolBarEdit.Enabled = false;
                        disableTabs("D");
                        dgvApplications.Enabled = true; dgvPrintLetters.Enabled = true; btnSave.Visible = btnCancel.Visible = true;
                        ToolBarEdit.Enabled = false;
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex) { }
        }
        private void TabControl1_Selecting(object sender, Wisej.Web.TabControlCancelEventArgs e)
        {
            // Check if the tab page you want to disable is clicked
            if (_tabindex > -1)
            {
                if (e.TabPageIndex != _tabindex)
                {
                    //Cancel the selection
                    e.Cancel = true;
                }
            }
        }
        int _tabindex = -1;
        void disableTabs(string strType)
        {
            if (strType == "D")
            {
                _tabindex = tbcAppandPrintLetter.SelectedIndex;
                foreach (Wisej.Web.TabPage tab in tbcAppandPrintLetter.TabPages)
                {
                    tab.Enabled = false;
                    tbcAppandPrintLetter.Selecting += TabControl1_Selecting;
                    //tab.AddState("readonly");
                }
            (tbcAppandPrintLetter.TabPages[_tabindex] as Wisej.Web.TabPage).Enabled = true;
            }
            else if (strType == "E")
            {
                foreach (Wisej.Web.TabPage tab in tbcAppandPrintLetter.TabPages)
                {
                    tab.Enabled = true;
                }
                _tabindex = -1;
                tbcAppandPrintLetter.Selecting -= TabControl1_Selecting;
            }
        }
        //bool isExist = false;
        private void FillAppGrid()
        {
            dgvApplications.Rows.Clear();
            int rowIndex = 0;

            DataSet ds = new DataSet();

            ds = DatabaseLayer.MainMenu.GET_PRINAPPCNTL(string.Empty, string.Empty, string.Empty,"A");

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                //ds = DatabaseLayer.MainMenu.GET_PRINAPPCNTL("**", string.Empty, string.Empty,"L");
                //dt = ds.Tables[0];
                if (BaseForm.BaseAdminAgency != "**")
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "PAC_AGY='" + BaseForm.BaseAdminAgency + "'";
                    dt = dv.ToTable();
                }
            }

            //if (dt.Rows.Count == 0)
            //    dt = ds.Tables[0];


            if (dt.Rows.Count > 0)
            {
                if (BaseForm.BaseAdminAgency == "**")
                {
                    //DataSet ds1 = DatabaseLayer.MainMenu.GET_PRINAPPCNTL(string.Empty, string.Empty, "**", "A");

                    DataView dv2 = new DataView(dt);
                    dv2.RowFilter = "PAC_AGY='" + "**" + "'";
                    DataTable dtAll = dv2.ToTable();

                    if (dtAll.Rows.Count == 0)
                    {
                        DataTable dtApp = CreatePRINTApp();

                        foreach (DataRow dr in dtApp.Rows)
                        {
                            dgvApplications.Rows.Add(dr["PAC_FORM"], false, dr["PAC_DISP_NAME"], false, BaseForm.BaseAdminAgency, "A", "A");
                        }
                    }
                }

                foreach (DataRow dr in dt.Rows)
                {
                    bool enableflag = false;
                    bool isSigned = false;

                    if (dr["PAC_ENABLE"].ToString() == "Y")
                        enableflag = true;

                    if (dr["PAC_SIGN"].ToString() == "Y")
                        enableflag = true;

                    dgvApplications.Rows.Add(dr["PAC_FORM"], enableflag, dr["PAC_DISP_NAME"], isSigned, dr["PAC_AGY"], "U", dr["PAC_TYPE"]);
                }
                //Mode = "UPDATE";
                ToolBarNew.Visible = false; ToolBarEdit.Visible = true; ToolBarEdit.Enabled = true;
            }
            else
            {
                DataTable dtApp = CreatePRINTApp();
                foreach (DataRow dr in dtApp.Rows)
                {
                    dgvApplications.Rows.Add(dr["PAC_FORM"], false, dr["PAC_DISP_NAME"],false, BaseForm.BaseAdminAgency,"A");
                }
                //Mode = "ADD";
                ToolBarNew.Visible = true; ToolBarNew.Enabled = true; ToolBarEdit.Visible = false;
            }

        }
        public void RefreshGrid()
        {
            FillAppGrid();
            FillPrintLetterGrid();
        }

        private void dgvApplications_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    DataGridView AppGrid = sender as DataGridView;
                    string selectedHIE = AppGrid.SelectedRows[0].Cells["dgvForm"].Value.ToString();

                    bool isSelect = false;

                    if (AppGrid.SelectedRows[0].Cells["dgvEnable"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isSelect = true;
                    }

                    foreach (DataGridViewRow dr in AppGrid.Rows)
                    {
                        string rowCode = dr.Cells["dgvForm"].Value.ToString();
                        if (!rowCode.Equals(selectedHIE))
                        {
                            dr.Cells["dgvEnable"].Value = false;
                            dr.DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            dr.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
        }
        string Mode = string.Empty;
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            if (tbcAppandPrintLetter.SelectedIndex == 1)
            {
                foreach (DataGridViewRow dr in dgvPrintLetters.Rows)
                {
                    string Enable = "N";
                    string Agency = string.Empty;
                    string isSigned = "N";

                    Agency = dr.Cells["dgvPLAgency"].Value.ToString().Trim();

                    if (dr.Cells["dgvPLMode"].Value.ToString().Equals("A"))
                        Mode = "ADD";
                    else
                        Mode = "UPDATE";

                    if (dr.Cells["dgvPLEnable"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                        Enable = "Y";

                    if (dr.Cells["dgvPLSign"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                            isSigned = "Y";

                        ds = DatabaseLayer.MainMenu.INSUPDEL_PRINAPPCNTL(dr.Cells["dgvPLForm"].Value.ToString().Trim(),Enable, dr.Cells["dgvPLDisplayName"].Value.ToString().Trim(), isSigned, Agency, "JAKE", Mode, "L");
                }
                FillPrintLetterGrid();
            }
            else
            {
                foreach (DataGridViewRow dr in dgvApplications.Rows)
                {
                    string Enable = "N";
                    string Agency = string.Empty;

                    Agency = dr.Cells["dgvAgency"].Value.ToString().Trim(); //BaseForm.BaseAdminAgency;


                    if (dr.Cells["dgvMode"].Value.ToString().Equals("A"))
                        Mode = "ADD";
                    else
                        Mode = "UPDATE";

                    if (dr.Cells["dgvEnable"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                        Enable = "Y";

                    ds = DatabaseLayer.MainMenu.INSUPDEL_PRINAPPCNTL(dr.Cells["dgvForm"].Value.ToString().Trim(), Enable, dr.Cells["dgvDisplayName"].Value.ToString().Trim(),string.Empty,
                                                     Agency, "JAKE", Mode, "A");
                }
                FillAppGrid();
            }

            AlertBox.Show("Saved Successfully");
            dgvApplications.Enabled = false;
            dgvPrintLetters.Enabled = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
            ToolBarNew.Visible = false; ToolBarEdit.Visible = true; 
            ToolBarEdit.Enabled = true; disableTabs("E");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (tbcAppandPrintLetter.SelectedIndex == 0)
                FillAppGrid();
            else if (tbcAppandPrintLetter.SelectedIndex == 1)
                FillPrintLetterGrid();
            dgvPrintLetters.Enabled = false;
            dgvApplications.Enabled = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
            ToolBarEdit.Enabled = true; disableTabs("E");
        }

        private void tbcAppandPrintLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbcAppandPrintLetter.SelectedIndex == 1)
            { FillPrintLetterGrid(); dgvPrintLetters.Enabled = false; }
            else
            { FillAppGrid(); dgvApplications.Enabled = false; }
        }

        private void dgvApplications_Click(object sender, EventArgs e)
        {

        }

        private void pnlSave_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void dgvPrintLetters_Click(object sender, EventArgs e)
        {

        }
    }
}
