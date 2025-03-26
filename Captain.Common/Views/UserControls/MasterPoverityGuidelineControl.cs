#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Wisej.Web;

//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Controllers;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.Forms;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.UserControls.Base;
using System.IO;
using NPOI.SS.Formula.Functions;
using DevExpress.Utils;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class MasterPoverityGuidelineControl : BaseUserControl
    {

        public MasterPoverityGuidelineControl(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;

            _model = new CaptainModel();

            propReportPath = _model.lookupDataAccess.GetReportPath();

            PopulateToolbar(oToolbarMnustrip);

            fillcombo();
            IntializeTabs();

        }

        private ErrorProvider _errorProvider = null;
        private CustomQuestionsControl _serviceHierarchy = null;
        private CaptainModel _model = null;
        private PrivilegesControl _screenPrivileges = null;

        private List<HierarchyEntity> _selectedHierarchies = null;
        #region properties

        public BaseForm BaseForm
        {
            get; set;
        }

        public PrivilegeEntity Privileges
        {
            get; set;
        }

        public ToolBarButton ToolBarEdit
        {
            get; set;
        }

        public ToolBarButton ToolBarNew
        {
            get; set;
        }

        public ToolBarButton ToolBarDel
        {
            get; set;
        }
        public ToolBarButton ToolBarExcel
        {
            get; set;
        }
        public ToolBarButton ToolBarHelp
        {
            get; set;
        }

        public List<MasterPovertyEntity> _masterPovertyEntiry
        {
            get; set;
        }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                return _selectedHierarchies;

            }
        }
        public string propReportPath
        {
            get; set;
        }

        #endregion


        private string strPublicStartDate = "";
        private string strPublicEndDate = "";
        private string strPublicAgency = "";
        private string strPublicDept = "";
        private string strPublicProgram = "";
        private string strPublicCode = "";
        private string strOldPublicCode = "";
        private string strOldAgency = "";
        private string strOldDept = "";
        private string strOldProgram = "";
        private bool boolChangeStatus = false;
        private bool boolSubGridStatus = false;
        private int strIndex = 0;


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
                ToolBarNew.ToolTipText = "Add Increment Exception";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";//new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Master Poverty";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";//new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.Enabled = true;
                ToolBarExcel.ImageSource = "captain-excel";
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Master Poverty Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help";//new IconResourceHandle(Consts.Icons16x16.Help);
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }

            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarNew != null)
                    ToolBarNew.Enabled = false;
            }
            else
            {
                if (ToolBarNew != null)
                    ToolBarNew.Enabled = true;
            }

            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarExcel != null)
                    ToolBarExcel.Enabled = false;
            }
            else
            {
                if (ToolBarExcel != null)
                    ToolBarExcel.Enabled = true;
            }

            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
            }
            else
            {
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = true;
            }

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarDel,
                ToolBarExcel,
                ToolBarHelp
            });
        }

        /// <summary>
        /// Handles the toolbar button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();

            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null)
            {
                return;
            }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:
                        if (tabControl1.SelectedIndex == 0)
                        {

                            DataGridView dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];

                            string strSelect = string.Empty;
                            string strexpstartdate = string.Empty;
                            string strexpenddate = string.Empty;
                            string strexpAgency = string.Empty;
                            string strexpDept = string.Empty;
                            string strexpProgram = string.Empty;
                            string strexpHierchy = string.Empty;
                            string strFamily1value = string.Empty;
                            foreach (DataGridViewRow dr in dataGridView.SelectedRows)
                            {
                                if (dr.Selected)
                                {
                                    string strMode = Convert.ToString(dataGridView.SelectedRows[0].Cells["Mode"].Value);
                                    strexpstartdate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_START_DATE"].Value);
                                    strexpenddate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_END_DATE"].Value);
                                    strexpHierchy = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value);
                                    strFamily1value = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_1_VALUE"].Value);
                                    strIndex = dataGridView.SelectedRows[0].Index;
                                    string[] strSplit;
                                    string[] strOldSplit;
                                    if (strMode == "U")
                                    {
                                        strSplit = strexpHierchy.Split('-');
                                        if (strSplit.Length > 1)
                                        {
                                            strexpAgency = strSplit[0];
                                            strexpDept = strSplit[1];
                                            strSelect = "Select";
                                            if (strSplit.Length >= 2)
                                                strexpProgram = strSplit[2];
                                        }
                                    }
                                }
                            }
                            if (strSelect != "")
                            {
                                IncrementExceptionForm exceptionForm = new IncrementExceptionForm(BaseForm, Privileges, strexpstartdate, strexpenddate, strexpHierchy, strexpAgency, strexpDept, strexpProgram, strFamily1value);
                                exceptionForm.FormClosed += new Wisej.Web.FormClosedEventHandler(exceptionForm_FormClosed);
                                exceptionForm.StartPosition = FormStartPosition.CenterScreen;
                                exceptionForm.ShowDialog();
                            }

                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (GridSelect().ToString() != "")
                        {
                            bool booldelete = true;
                            if (tabControl1.SelectedIndex == 0)
                            {
                                DataGridView dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                                ;
                                string strNewStartDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_START_DATE"].Value);
                                string strNewEndDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_END_DATE"].Value);
                                string strHierarchy = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value);
                                string[] strSplit = strHierarchy.Split('-');
                                string Agency = string.Empty;
                                string Dept = string.Empty;
                                string Program = string.Empty;
                                if (strSplit.Length > 1)
                                {
                                    Agency = strSplit[0].ToString();
                                    Dept = strSplit[1].ToString();
                                    Program = strSplit[2].ToString();
                                }
                                CaptainModel model = new CaptainModel();
                                PovertyException propIncrementdata = model.masterPovertyData.GetPovertyException(Agency, Dept, Program, strNewStartDate, strNewEndDate, string.Empty, string.Empty);
                                if (propIncrementdata != null)
                                {
                                    booldelete = false;
                                    AlertBox.Show("Please Delete Increment Exception Data", MessageBoxIcon.Warning);
                                }

                            }
                            if (booldelete)
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + Privileges.PrivilegeName, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                        }
                        break;
                    case Consts.ToolbarActions.Excel:

                        if (tabControl1.SelectedTab == null)
                            return;
                        string code = tabControl1.SelectedTab.Tag as string;

                        if (code.Equals("FED"))
                        {
                            Genrate_FED_OMB_Excel();
                        }
                        else if (code.Equals("HUD"))
                        {
                            Genrate_HUD_Excel();
                        }
                        else if (code.Equals("CMI"))
                        {
                            Genrate_CMI_Excel();
                        }
                        else if (code.Equals("SMI"))
                        {
                            Genrate_SMI_Excel();
                        }

                        break;
                    case Consts.ToolbarActions.Help:
                        // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN00010");
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        void exceptionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid1();
        }

        private string GridSelect()
        {
            DataGridView dataGridView = null;
            DataGridView dataGridSubView = null;
            string strType = "";
            string strCounty = "";
            string Agency = "";
            string Dept = "";
            string Program = "";
            string strSelect = "";
            CaptainModel model = new CaptainModel();

            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                strType = "FED";
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                string strcmbCounty = "";
                strType = "HUD";
                if (cmbCounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                dataGridView = gvwHud;
                strCounty = strcmbCounty;

            }
            else if (tabControl1.SelectedIndex == 2)
            {

                string strcmbCmiCounty = "";
                strType = "CMI";
                if (cmbCMICounty.Items.Count > 0)
                    strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
                dataGridView = gvwCMI;
                strCounty = strcmbCmiCounty;

            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                strType = "SMI";
            }
            foreach (DataGridViewRow dr in dataGridView.SelectedRows)
            {
                if (dr.Selected)
                {


                    //string strMode = Convert.ToString(dataGridView.CurrentRow.Cells["Mode"].Value);
                    //string strNewStartDate = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_START_DATE"].Value);
                    //string strNewEndDate = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_END_DATE"].Value);
                    //string strPovBase = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_1_VALUE"].Value);
                    //string strIncrement = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_2_VALUE"].Value);
                    //string strHierarchy = Convert.ToString(dataGridView.CurrentRow.Cells["Hierarchy"].Value);
                    string strMode = Convert.ToString(dataGridView.SelectedRows[0].Cells["Mode"].Value);
                    string strNewStartDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_START_DATE"].Value);
                    string strNewEndDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_END_DATE"].Value);
                    string strHierarchy = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value);
                    strIndex = dataGridView.SelectedRows[0].Index;
                    string[] strSplit;
                    string[] strOldSplit;
                    if (strMode == "U")
                    {
                        strSplit = strHierarchy.Split('-');
                        if (strSplit.Length > 1)
                        {
                            strSelect = "Select";
                        }
                    }
                }
            }
            return strSelect;

        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            // Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                DataGridView dataGridView = null;
                DataGridView dataGridSubView = null;
                string strType = "";
                string strCounty = "";
                string Agency = "";
                string Dept = "";
                string Program = "";
                CaptainModel model = new CaptainModel();

                if (tabControl1.SelectedIndex == 0)
                {
                    dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                    strType = "FED";
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    string strcmbCounty = "";
                    strType = "HUD";
                    if (cmbCounty.Items.Count > 0)
                        strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                    dataGridView = gvwHud;
                    strCounty = strcmbCounty;

                }
                else if (tabControl1.SelectedIndex == 2)
                {

                    string strcmbCmiCounty = "";
                    strType = "CMI";
                    if (cmbCMICounty.Items.Count > 0)
                        strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
                    dataGridView = gvwCMI;
                    strCounty = strcmbCmiCounty;

                }
                else if (tabControl1.SelectedIndex == 3)
                {
                    dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                    strType = "SMI";
                }
                foreach (DataGridViewRow dr in dataGridView.SelectedRows)
                {
                    if (dr.Selected)
                    {

                        string strMode = Convert.ToString(dataGridView.SelectedRows[0].Cells["Mode"].Value);
                        string strNewStartDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_START_DATE"].Value);
                        string strNewEndDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_END_DATE"].Value);
                        string strHierarchy = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value);
                        strIndex = dataGridView.SelectedRows[0].Index;
                        string[] strSplit;
                        string[] strOldSplit;
                        if (strMode == "U")
                        {
                            strSplit = strHierarchy.Split('-');
                            if (strSplit.Length > 1)
                            {
                                Agency = strSplit[0].ToString();
                                Dept = strSplit[1].ToString();
                                Program = strSplit[2].ToString();
                            }
                        }

                        if (model.masterPovertyData.DeleteCaseGdl(Agency, Dept, Program, strType, strCounty, strNewStartDate, strNewEndDate, string.Empty, string.Empty))
                        {
                            strIndex = dataGridView.SelectedRows[0].Index;
                            if (strIndex != 0)
                                strIndex = strIndex - 1;
                            AlertBox.Show("Record Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                            RefreshGrid1();

                        }
                        else
                        {
                            AlertBox.Show("You can’t delete this " + Privileges.PrivilegeName.Trim() + ", as there are Dependices", MessageBoxIcon.Warning);
                        }
                    }
                }
            }

            // }

        }

        private void MessageBoxHandlerSub(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                DataGridView dataGridView = null;
                DataGridView dataGridSubView = null;
                string strType = "";
                string strCounty = "";
                string Agency = "";
                string Dept = "";
                string Program = "";
                CaptainModel model = new CaptainModel();

                if (tabControl1.SelectedIndex == 0)
                {
                    dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                    strType = "FED";
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    string strcmbCounty = "";
                    strType = "HUD";
                    if (cmbCounty.Items.Count > 0)
                        strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                    dataGridView = gvwHud;
                    strCounty = strcmbCounty;
                    dataGridSubView = (DataGridView)pnlHUDSubGrid/*splitContainer1.Panel2*/.Controls["HudSubGrid"];
                }
                else if (tabControl1.SelectedIndex == 2)
                {

                    string strcmbCmiCounty = "";
                    strType = "CMI";
                    if (cmbCMICounty.Items.Count > 0)
                        strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
                    dataGridView = gvwCMI;
                    strCounty = strcmbCmiCounty;
                    dataGridSubView = (DataGridView)pnlCMISubGrid/*splitContainer2.Panel2*/.Controls["CMISubGrid"];


                }
                else if (tabControl1.SelectedIndex == 3)
                {
                    dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                    strType = "SMI";
                    dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
                }
                foreach (DataGridViewRow dr in dataGridView.SelectedRows)
                {
                    if (dr.Selected)
                    {

                        string strMode = Convert.ToString(dataGridView.SelectedRows[0].Cells["Mode"].Value);
                        string strNewStartDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_START_DATE"].Value);
                        string strNewEndDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_END_DATE"].Value);
                        string strHierarchy = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value);
                        strIndex = dataGridView.SelectedRows[0].Index;
                        string[] strSplit;
                        string[] strOldSplit;
                        if (strMode == "U")
                        {
                            strSplit = strHierarchy.Split('-');
                            if (strSplit.Length > 1)
                            {
                                Agency = strSplit[0].ToString();
                                Dept = strSplit[1].ToString();
                                Program = strSplit[2].ToString();
                            }
                        }
                        string strNoHolds = "";
                        if (dataGridSubView != null)
                        {
                            strNoHolds = Convert.ToString(dataGridSubView.SelectedRows[0].Cells["GDL_NO_HOUSEHOLDS"].Value);
                        }

                        if (model.masterPovertyData.DeleteCaseGdl(Agency, Dept, Program, strType, strCounty, strNewStartDate, strNewEndDate, strNoHolds, "Sub"))
                        {
                            strIndex = dataGridView.SelectedRows[0].Index;
                            AlertBox.Show("Record Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                            RefreshGrid1();

                        }
                        else
                        {
                            AlertBox.Show("You can’t delete this " + Privileges.PrivilegeName.Trim() + ", as there are Dependices", MessageBoxIcon.Warning);
                        }

                    }
                }
            }

            //}

        }
        public void RefreshGrid1()
        {

            OnModulesTabControlSelectedIndexChanged(tabControl1, new EventArgs());
            DataGridView dataGridView = null;
            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridView = gvwHud;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridView = gvwCMI;

            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];

            }
            if (dataGridView != null)
            {
                dataGridView.Rows[strIndex].Selected = true;
                dataGridView.CurrentCell = dataGridView.Rows[strIndex].Cells[1];
            }

        }




        /// <summary>
        /// Adds a grid to a tabpage
        /// </summary>
        /// <param name="tabPage">The tab page to add the grid to.</param>
        /// <param name="gridName">The name of the grid control </param>
        /// <returns>Returns a hierarchical grid control.</returns>
        private DataGridView AddGrid(TabPage tabPage, string gridName)
        {
            DataGridView moduleGrid = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)(moduleGrid)).BeginInit();

            tabPage.Controls.Add(moduleGrid);

            // 
            // moduleGrid
            // 
            moduleGrid.Anchor = Wisej.Web.AnchorStyles.None;
            moduleGrid.Dock = Wisej.Web.DockStyle.Fill;
            moduleGrid.Location = new System.Drawing.Point(0, 0);
            moduleGrid.Name = gridName;
            moduleGrid.Size = new System.Drawing.Size(938, 628);
            moduleGrid.TabIndex = 0;
            moduleGrid.RowHeadersVisible = true;
            moduleGrid.RowHeadersWidth = 15;
            moduleGrid.AllowUserToAddRows = true;
            moduleGrid.RowTemplate.MinimumHeight = 25;
            moduleGrid.AllowUserToDeleteRows = false;
            moduleGrid.BackColor = System.Drawing.Color.White;
            moduleGrid.AllowUserToResizeRows = true;
            moduleGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            moduleGrid.MultiSelect = true;
            moduleGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            moduleGrid.DataError -= new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);
            moduleGrid.DataError += new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);

            ((System.ComponentModel.ISupportInitialize)(moduleGrid)).EndInit();

            return moduleGrid;
        }

        /// <summary>
        /// Adds a grid to a tabpage
        /// </summary>
        /// <param name="tabPage">The tab page to add the grid to.</param>
        /// <param name="gridName">The name of the grid control </param>
        /// <returns>Returns a hierarchical grid control.</returns>
        private DataGridView AddGrid(Panel tabPage, string gridName)
        {
            DataGridView moduleGrid = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)(moduleGrid)).BeginInit();

            tabPage.Controls.Add(moduleGrid);

            // 
            // moduleGrid
            // 
            moduleGrid.Anchor = Wisej.Web.AnchorStyles.None;
            moduleGrid.Dock = Wisej.Web.DockStyle.Fill;
            moduleGrid.Name = gridName;
            moduleGrid.Size = new System.Drawing.Size(900, 650);
            moduleGrid.TabIndex = 0;
            moduleGrid.RowHeadersVisible = true;
            moduleGrid.RowHeadersWidth = 15;
            moduleGrid.AllowUserToAddRows = true;
            moduleGrid.DefaultCellStyle.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            moduleGrid.RowTemplate.MinimumHeight = 22;
            moduleGrid.AllowUserToDeleteRows = false;
            moduleGrid.BackColor = System.Drawing.Color.White;
            moduleGrid.AllowUserToResizeRows = true;
            moduleGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            moduleGrid.MultiSelect = false;
            moduleGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            moduleGrid.DataError -= new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);
            moduleGrid.DataError += new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);


            ((System.ComponentModel.ISupportInitialize)(moduleGrid)).EndInit();

            return moduleGrid;
        }

        /// <summary>
        /// Handles  grid data error event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataGridViewDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddScreenGridColumnsFEBUSDA(DataGridView dataGridView, string strH1, string strC1, string strH2, string strC2, string strH3, string strC3, string strH4, string strC4, string strH5, string strC5)
        {
            DataGridViewTextBoxColumn descColumn = null;
            string columnName = string.Empty;

            dataGridView.Columns.Clear();
            dataGridView.DefaultRowHeight = 25;
            DataGridViewTextBoxColumn moduleColumn = new DataGridViewTextBoxColumn();
            moduleColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleColumn.HeaderText = strH1;
            moduleColumn.Name = strC1;
            moduleColumn.Width = 0;
            moduleColumn.ReadOnly = false;
            moduleColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            moduleColumn.Visible = false;
            moduleColumn.ShowInVisibilityMenu = false;
            dataGridView.Columns.Add(moduleColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn();
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.SortMode = DataGridViewColumnSortMode.NotSet;
            codeColumn.HeaderText = strH2;
            codeColumn.Name = strC2;
            codeColumn.DataPropertyName = strC2;
            codeColumn.Width = 100;
            codeColumn.ReadOnly = false;
            codeColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            codeColumn.MaxInputLength = 10;
            codeColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(codeColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSet;
            descColumn.HeaderText = strH3;
            descColumn.Name = strC3;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.MaxInputLength = 10;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);


            // CalenderColumn objStartDate = new CalenderColumn(); 
            //// objStartDate.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            //// objStartDate.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            //// objStartDate.CellTemplate = new DataGridViewTextBoxCell();
            // objStartDate.HeaderText = strH3;
            // objStartDate.Name = strC3;
            // objStartDate.Width = 100;
            // objStartDate.ReadOnly = false;
            // dataGridView.Columns.Add(objStartDate);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = strH4;
            descColumn.Name = strC4;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = strH5;
            descColumn.Name = strC5;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.MaxInputLength = 12;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = "Hierarchy";
            descColumn.Name = "Hierarchy";
            descColumn.Width = 70;
            descColumn.ReadOnly = true;
            descColumn.Visible = true;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);


            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            btnColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            btnColumn.HeaderText = "Hierarchy";
            btnColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            btnColumn.Name = "Hierarchy1";
            btnColumn.Width = 70;
            btnColumn.MinimumWidth = 60;
            btnColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            btnColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            btnColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            btnColumn.ShowInVisibilityMenu = false;
            btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridView.Columns.Add(btnColumn);

        }

        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddScreenGridColumns(DataGridView dataGridView, string strH1, string strC1, string strH2, string strC2, string strH3, string strC3, string strH4, string strC4, string strH5, string strC5, string strH6, string strC6, string strH7, string strC7, string strH8, string strC8, string strH9, string strC9)
        {
            DataGridViewTextBoxColumn descColumn = null;
            string columnName = string.Empty;
            dataGridView.Columns.Clear();
            dataGridView.DefaultRowHeight = 25;
            DataGridViewTextBoxColumn moduleColumn = new DataGridViewTextBoxColumn();
            moduleColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            moduleColumn.HeaderText = strH1;
            moduleColumn.Name = strC1;
            moduleColumn.Width = 0;
            moduleColumn.ReadOnly = false;
            moduleColumn.Visible = false;
            moduleColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            moduleColumn.ShowInVisibilityMenu = false;
            dataGridView.Columns.Add(moduleColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn();
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.SortMode = DataGridViewColumnSortMode.NotSet;
            codeColumn.HeaderText = strH2;
            codeColumn.Name = strC2;
            codeColumn.DataPropertyName = strC2;
            codeColumn.Width = 100;
            codeColumn.ReadOnly = false;
            codeColumn.MaxInputLength = 10;
            codeColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            codeColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(codeColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSet;
            descColumn.HeaderText = strH3;
            descColumn.Name = strC3;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 10;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);


            // CalenderColumn objStartDate = new CalenderColumn(); 
            //// objStartDate.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            //// objStartDate.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            //// objStartDate.CellTemplate = new DataGridViewTextBoxCell();
            // objStartDate.HeaderText = strH3;
            // objStartDate.Name = strC3;
            // objStartDate.Width = 100;
            // objStartDate.ReadOnly = false;
            // dataGridView.Columns.Add(objStartDate);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = strH4;
            descColumn.Name = strC4;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = strH5;
            descColumn.Name = strC5;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = strH6;
            descColumn.Name = strC6;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = strH7;
            descColumn.Name = strC7;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = strH8;
            descColumn.Name = strC8;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = strH9;
            descColumn.Name = strC9;
            descColumn.Width = 90;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderText = "Hierarchy";
            descColumn.Name = "Hierarchy";
            descColumn.Width = 70;
            descColumn.ReadOnly = true;
            descColumn.Visible = true;
            descColumn.MaxInputLength = 12;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);


            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            btnColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            btnColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            btnColumn.HeaderText = "Hierarchy";
            btnColumn.Name = "Hierarchy1";
            btnColumn.Width = 70;
            btnColumn.MinimumWidth = 60;
            btnColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            btnColumn.ShowInVisibilityMenu = false;
            btnColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridView.Columns.Add(btnColumn);

        }

        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddSubGridColumns(DataGridView dataGridView, string strH1, string strC1, string strH2, string strC2, string strH3, string strC3, string strH4, string strC4, string strH5, string strC5, string strH6, string strC6, string strH7, string strC7, string strH8, string strC8)
        {
            DataGridViewTextBoxColumn descColumn = null;
            string columnName = string.Empty;

            dataGridView.Columns.Clear();
            dataGridView.DefaultRowHeight = 25;
            DataGridViewTextBoxColumn moduleColumn = new DataGridViewTextBoxColumn();
            moduleColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleColumn.HeaderText = strH1;
            moduleColumn.Name = strC1;
            moduleColumn.Width = 0;
            moduleColumn.ReadOnly = false;
            moduleColumn.Visible = false;
            //moduleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            moduleColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            moduleColumn.ShowInVisibilityMenu = false;
            dataGridView.Columns.Add(moduleColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn(); //Fam Size
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.HeaderText = strH2;
            codeColumn.Name = strC2;
            codeColumn.Width = 75;
            codeColumn.ReadOnly = false;
            codeColumn.MaxInputLength = 3;
            // codeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            codeColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            codeColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(codeColumn);

            descColumn = new DataGridViewTextBoxColumn();   //30%
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = strH3;
            descColumn.Name = strC3;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            //descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();   //60%
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = strH4;
            descColumn.Name = strC4;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            // descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();   //80%
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = strH5;
            descColumn.Name = strC5;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.MaxInputLength = 12;
            // descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();   //100%
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = strH6;
            descColumn.Name = strC6;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            //descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();   //185%
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = strH7;
            descColumn.Name = strC7;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            //descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            descColumn = new DataGridViewTextBoxColumn();   //500%
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = strH8;
            descColumn.Name = strC8;
            descColumn.Width = 100;
            descColumn.ReadOnly = false;
            descColumn.MaxInputLength = 12;
            // descColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            descColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            descColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            dataGridView.Columns.Add(descColumn);

            /*commented by Vikash*/
            //DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            //btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            //btnColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            //btnColumn.HeaderText = " Delete ";
            //btnColumn.Name = "Delete";
            //btnColumn.Width = 70;
            //btnColumn.MinimumWidth = 60;
            //btnColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; btnColumn.ShowInVisibilityMenu = false;
            //btnColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            //btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            //dataGridView.Columns.Add(btnColumn);

            DataGridViewImageColumn btnColumn = new DataGridViewImageColumn();   //Delete
            btnColumn.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            btnColumn.CellImageSource = "captain-delete";
            btnColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            btnColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            btnColumn.HeaderText = " Delete ";
            btnColumn.Name = "Delete";
            btnColumn.Width = 70;
            btnColumn.MinimumWidth = 60;
            btnColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //btnColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            btnColumn.ShowInVisibilityMenu = false;
            btnColumn.Resizable = Wisej.Web.DataGridViewTriState.False;
            btnColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridView.Columns.Add(btnColumn);


        }

        /// <summary>
        /// Intialize the Tab Page controls.
        /// </summary>
        private void IntializeTabs()
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                string poveryItem = tabPage.Tag as string;
                DataGridView dataGridView = null;
                DataGridView dataGridSubView = null;
                switch (poveryItem)
                {
                    case "FED":
                        dataGridView = AddGrid(pnlFebOmb, "FedGrid");
                        dataGridView.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView_RowsAdded);
                        AddScreenGridColumnsFEBUSDA(dataGridView, "Mode", "Mode", "Start Date*", "GDL_START_DATE", "End Date*", "GDL_END_DATE", "Poverty Base*", "GDL_1_VALUE", "Increment*", "GDL_2_VALUE");
                        FillGridData(dataGridView, "", "", "", "FED", "");

                        dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
                        dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        dataGridView.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);
                        break;
                    case "HUD":

                        //dataGridView = AddGrid(splitContainer1.Panel1, "HudGrid");
                        gvwHud.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView_RowsAdded);
                        AddScreenGridColumns(gvwHud, "Mode", "Mode", "Start Date*", "GDL_START_DATE", "End Date*", "GDL_END_DATE", "1st % *", "GDL_1_VALUE", "2nd % *", "GDL_2_VALUE", "3rd % ", "GDL_3_VALUE", "4th %", "GDL_4_VALUE", "5th %", "GDL_5_VALUE", "6th %", "GDL_6_VALUE");

                        string strcmbCounty = "";
                        if (cmbCounty.Items.Count > 0)
                            strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                        cmbCounty.SelectedIndexChanged -= new EventHandler(cmbCounty_SelectedIndexChanged);
                        gvwHud.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        gvwHud.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        gvwHud.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        gvwHud.CellClick -= new DataGridViewCellEventHandler(dataGridView_CellClick);
                        gvwHud.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                        gvwHud.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        FillGridData(gvwHud, "", "", "", "HUD", strcmbCounty);

                        cmbCounty.SelectedIndexChanged += new EventHandler(cmbCounty_SelectedIndexChanged);
                        gvwHud.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        gvwHud.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        gvwHud.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        gvwHud.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
                        gvwHud.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                        gvwHud.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        gvwHud.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);
                        dataGridSubView = AddGrid(pnlHUDSubGrid/*splitContainer1.Panel2*/, "HudSubGrid");
                        AddSubGridColumns(dataGridSubView, "Mode", "Mode", " ", "GDL_NO_HOUSEHOLDS", " ", "GDL_1_VALUE", " ", "GDL_2_VALUE", " ", "GDL_3_VALUE", " ", "GDL_4_VALUE", " ", "GDL_5_VALUE", " ", "GDL_6_VALUE");
                        dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                        dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);

                        break;
                    case "CMI":

                        //dataGridView = AddGrid(splitContainer2.Panel1, "CMIGrid");
                        gvwCMI.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView_RowsAdded);
                        AddScreenGridColumns(gvwCMI, "Mode", "Mode", "Start Date*", "GDL_START_DATE", "End Date*", "GDL_END_DATE", "1st % *", "GDL_1_VALUE", "2nd % *", "GDL_2_VALUE", "3rd % ", "GDL_3_VALUE", "4th %", "GDL_4_VALUE", "5th %", "GDL_5_VALUE", "6th %", "GDL_6_VALUE");

                        string strcmbCmiCounty = "";
                        if (cmbCMICounty.Items.Count > 0)
                            strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
                        gvwCMI.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        gvwCMI.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        gvwCMI.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        gvwCMI.CellClick -= new DataGridViewCellEventHandler(dataGridView_CellClick);
                        cmbCMICounty.SelectedIndexChanged -= new EventHandler(cmbCMICounty_SelectedIndexChanged);
                        gvwCMI.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                        gvwCMI.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        FillGridData(gvwCMI, "", "", "", "CMI", strcmbCmiCounty);
                        gvwCMI.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        gvwCMI.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        gvwCMI.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        gvwCMI.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
                        cmbCMICounty.SelectedIndexChanged += new EventHandler(cmbCMICounty_SelectedIndexChanged);
                        gvwCMI.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                        gvwCMI.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        gvwCMI.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);
                        dataGridSubView = AddGrid(pnlCMISubGrid, "CMISubGrid");
                        AddSubGridColumns(dataGridSubView, "Mode", "Mode", " ", "GDL_NO_HOUSEHOLDS", " ", "GDL_1_VALUE", " ", "GDL_2_VALUE", " ", "GDL_3_VALUE", " ", "GDL_4_VALUE", " ", "GDL_5_VALUE", " ", "GDL_6_VALUE");
                        dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                        dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                        break;
                    case "SMI":

                        dataGridView = AddGrid(pnlSMIGrid, "SMIGrid");
                        dataGridView.RowsAdded -= new DataGridViewRowsAddedEventHandler(dataGridView_RowsAdded);
                        dataGridView.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView_RowsAdded);
                        dataGridView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        dataGridView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        dataGridView.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        dataGridView.CellClick -= new DataGridViewCellEventHandler(dataGridView_CellClick);
                        dataGridView.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                        dataGridView.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        AddScreenGridColumns(dataGridView, "Mode", "Mode", "Start Date*", "GDL_START_DATE", "End Date*", "GDL_END_DATE", "1st % *", "GDL_1_VALUE", "2nd % *", "GDL_2_VALUE", "3rd % ", "GDL_3_VALUE", "4th %", "GDL_4_VALUE", "5th %", "GDL_5_VALUE", "6th %", "GDL_6_VALUE");

                        FillGridData(dataGridView, "", "", "", "SMI", "");
                        dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
                        dataGridView.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                        dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        dataGridView.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);
                        dataGridSubView = AddGrid(pnlSMISubGrid, "SMISubGrid");
                        AddSubGridColumns(dataGridSubView, "Mode", "Mode", " ", "GDL_NO_HOUSEHOLDS", " ", "GDL_1_VALUE", " ", "GDL_2_VALUE", " ", "GDL_3_VALUE", " ", "GDL_4_VALUE", " ", "GDL_5_VALUE", " ", "GDL_6_VALUE");
                        dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                        dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                        break;

                }
                //tabControl1.TabPages.Add(tabPage);
            }
            OnModulesTabControlSelectedIndexChanged(tabControl1, new EventArgs());
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = null;
            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridView = gvwHud;
                dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridView = gvwCMI;
                dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            if (dataGridView != null)
            {
                string[] strOldSplit;
                //if (strMode == "U")
                //{
                //    strSplit = strHierarchy.Split('-');

                foreach (DataGridViewRow item in dataGridView.Rows)
                {
                    if (item.Selected)
                    {

                        MasterPovertyEntity row = dataGridView.SelectedRows[0].Tag as MasterPovertyEntity;
                        string strchangeMode = string.Empty;
                        if (row != null)
                        {
                            strOldPublicCode = row.GdlAgency + "-" + row.GdlDept + "-" + row.GdlProgram;
                            strchangeMode = row.Mode;
                        }
                        strOldSplit = strOldPublicCode.Split('-');
                        if (strOldSplit.Length > 1)
                        {
                            strOldAgency = strOldSplit[0].ToString();
                            strOldDept = strOldSplit[1].ToString();
                            strOldProgram = strOldSplit[2].ToString();
                        }
                        if (e.ColumnIndex == dataGridView.ColumnCount - 1 && e.RowIndex != -1)
                        {
                            if (Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["Mode"].Value) != "U")
                            {
                                string strHierarchy = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["Hierarchy"].Value);
                                // HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, "Master", strHierarchy, string.Empty, "Reports");
                                HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, strHierarchy, "Master", "I", "*", "I");
                                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                                hierarchieSelectionForm.ShowDialog();
                            }
                            else
                            {
                                AlertBox.Show("Hierarchy cannot be changed", MessageBoxIcon.Warning);
                            }

                        }

                    }
                }

            }
        }

        private void dataGridSubView_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            DataGridView dataGridSubView = null;
            DataGridView dataGridView = null;
            string strType = "";
            string strCounty = "";

            if (tabControl1.SelectedIndex == 1)
            {
                string strcmbCounty = "";
                if (cmbCounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();

                dataGridView = gvwHud;
                dataGridSubView = (DataGridView)pnlHUDSubGrid/*splitContainer1.Panel2*/.Controls["HudSubGrid"];
                dataGridSubView.EditMode = DataGridViewEditMode.EditOnEnter;
                strCounty = strcmbCounty;
                strType = "HUD";
            }
            else if (tabControl1.SelectedIndex == 2)
            {

                string strcmbCmiCounty = "";
                if (cmbCMICounty.Items.Count > 0)
                    strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();

                dataGridView = gvwCMI;
                dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
                dataGridSubView.EditMode = DataGridViewEditMode.EditOnEnter;
                strCounty = strcmbCmiCounty;
                strType = "CMI";
            }
            else if (tabControl1.SelectedIndex == 3)
            {

                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
                dataGridSubView.EditMode = DataGridViewEditMode.EditOnEnter;
                strType = "SMI";

            }
            if (dataGridView.SelectedRows.Count == 0)
                return;
            if (dataGridSubView != null)
            {
                if (e.ColumnIndex == dataGridView.ColumnCount - 3 && e.RowIndex != -1)
                {
                    if (Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["Mode"].Value) == "U")
                    {
                        MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + Privileges.PrivilegeName, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerSub);
                    }
                }
            }



        }

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView dataGridView = null;
            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridView = gvwHud;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridView = gvwCMI;
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
            }

            if (dataGridView != null)
            {
                if (dataGridView.ColumnCount > 3)
                {
                    try
                    {
                        if (dataGridView.Rows[e.RowIndex].Cells["Mode"].Value == null)  //!dataGridView.Rows[e.RowIndex].Cells["Mode"].Value.Equals("U"))
                        {
                            dataGridView.Rows[e.RowIndex].Cells["Hierarchy1"].Value = "...";//"Hierarchy";
                            dataGridView.Rows[e.RowIndex].Cells[0].Selected = true;
                        }
                    }
                    catch (Exception ex)
                    {


                    }

                }
            }
        }

        private void dataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = null;
            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridView = gvwHud;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridView = gvwCMI;
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
            }
            if (dataGridView != null)
            {
                if (dataGridView.IsCurrentCellInEditMode)
                {
                    string strStartDate = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_START_DATE"].Value);
                    string strEndDate = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_END_DATE"].Value);
                    if (strStartDate != "")
                    {
                        if (strEndDate != "")
                        {
                            //if (Convert.ToDateTime(DateTime.ParseExact(strStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)) > Convert.ToDateTime(DateTime.ParseExact(strEndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)))
                            if (Convert.ToDateTime(strStartDate).Date > Convert.ToDateTime(strEndDate).Date)
                            {
                                AlertBox.Show("End Date should be greater than or equal to Start Date", MessageBoxIcon.Warning);
                                dataGridView.Rows[e.RowIndex].Cells["GDL_END_DATE"].Value = "";
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            DataGridView dataGridView = null;
            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridView = gvwHud;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridView = gvwCMI;
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
            }

            if (dataGridView.IsCurrentCellInEditMode)
            {

                string formatedValue = e.FormattedValue as string;
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_1_VALUE")
                //    && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
                //{
                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                //    e.Cancel = true;
                //}

                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_2_VALUE")
                //   && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
                //{

                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                //    e.Cancel = true;
                //}
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_3_VALUE")
                //   && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
                //{

                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                //    e.Cancel = true;
                //}
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_4_VALUE")
                //   && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
                //{

                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                //    e.Cancel = true;
                //}
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_5_VALUE")
                //   && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
                //{

                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                //    e.Cancel = true;
                //}
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_6_VALUE")
                //   && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
                //{

                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                //    e.Cancel = true;
                //}
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_START_DATE")
                //      && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.DateFormatMMDDYYYY))
                //{

                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyyyDateFormat);
                //    e.Cancel = true;

                //}
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_END_DATE")
                //      && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.DateFormatMMDDYYYY))
                //{
                //    e.Cancel = true;
                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyyyDateFormat);

                //}
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("Hierarchy")
                //      && System.Text.RegularExpressions.Match.Equals(formatedValue, string.Empty))
                //{
                //    e.Cancel = true;
                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.RequiredHierarchy);

                //}
                if (dataGridView != null)
                {
                    if (dataGridView.IsCurrentCellInEditMode)
                    {
                        string strStartDate = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_START_DATE"].Value);
                        string strEndDate = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_END_DATE"].Value);
                        if (strStartDate != "")
                        {
                            if (strEndDate != "")
                            {
                                //if (Convert.ToDateTime(DateTime.ParseExact(strStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)) > Convert.ToDateTime(DateTime.ParseExact(strEndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)))
                                if (Convert.ToDateTime(strStartDate).Date > Convert.ToDateTime(strEndDate).Date)
                                {
                                    AlertBox.Show("End Date should be greater than or equal to Start Date", MessageBoxIcon.Warning);
                                    dataGridView.Rows[e.RowIndex].Cells["GDL_END_DATE"].Value = "";
                                    e.Cancel = false;
                                }
                            }
                        }


                    }


                }
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //  boolChangeStatus = true;

            bool boolcellstatus = true;
            string strType = "";
            DataGridView dataGridView = null;
            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                strType = "FED";
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridView = gvwHud;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridView = gvwCMI;
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
            }
            if (dataGridView != null)
            {
                int intcolindex = dataGridView.CurrentCell.ColumnIndex;
                int introwindex = dataGridView.CurrentCell.RowIndex;
                string strGdl3Value = "";
                string strGdl4Value = "";
                string strGdl5Value = "";
                string strGdl6Value = "";
                //string strCurrectValue1 = dataGridView.CurrentCell.Value.ToString();
                string strCurrectValue = Convert.ToString(dataGridView.Rows[introwindex].Cells[intcolindex].Value);
                string strGdlStartDate = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_START_DATE"].Value);
                string strGdlEndDate = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_END_DATE"].Value);
                string strGdl1Value = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_1_VALUE"].Value);
                string strGdl2Value = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_2_VALUE"].Value);
                if (strType != "FED")
                {
                    strGdl3Value = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_3_VALUE"].Value);
                    strGdl4Value = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_4_VALUE"].Value);
                    strGdl5Value = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_5_VALUE"].Value);
                    strGdl6Value = Convert.ToString(dataGridView.Rows[introwindex].Cells["GDL_6_VALUE"].Value);
                }
                if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_START_DATE"))
                {
                    if (strCurrectValue == string.Empty)
                    {
                        boolcellstatus = false;
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                    {
                        try
                        {

                            if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                            {
                                AlertBox.Show("Dates below '01/01/1800' are not accepted", MessageBoxIcon.Warning);// ("01/01/1800 below date not except");
                                dataGridView.Rows[introwindex].Cells["GDL_START_DATE"].Value = string.Empty;
                                boolcellstatus = false;
                            }
                            else
                            {
                                dataGridView.Rows[introwindex].Cells["GDL_START_DATE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                                boolcellstatus = false;
                            }

                        }
                        catch (Exception)
                        {
                            dataGridView.Rows[introwindex].Cells["GDL_START_DATE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                            boolcellstatus = false;
                        }


                    }
                }
                if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_END_DATE"))
                {
                    if (strGdlStartDate == string.Empty)
                    {
                        boolcellstatus = false;
                        AlertBox.Show(Consts.Messages.RequireddateFields, MessageBoxIcon.Warning);
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                    {
                        try
                        {

                            if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                            {
                                AlertBox.Show("Dates below '01/01/1800' are not accepted", MessageBoxIcon.Warning);//("01/01/1800 below date not except");
                                dataGridView.Rows[introwindex].Cells["GDL_END_DATE"].Value = string.Empty;
                                boolcellstatus = false;
                            }
                            else
                            {
                                dataGridView.Rows[introwindex].Cells["GDL_END_DATE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                                boolcellstatus = false;
                            }
                        }
                        catch (Exception)
                        {
                            dataGridView.Rows[introwindex].Cells["GDL_END_DATE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                            boolcellstatus = false;
                        }
                    }
                }

                if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_1_VALUE"))
                {
                    if (strGdlStartDate == string.Empty || strGdlEndDate == string.Empty)
                    {
                        boolcellstatus = false;
                        AlertBox.Show(Consts.Messages.RequireddateFields, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                        {
                            dataGridView.Rows[introwindex].Cells["GDL_1_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            boolcellstatus = false;
                        }
                    }
                }
                if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_2_VALUE"))
                {
                    if (strGdlStartDate == string.Empty || strGdlEndDate == string.Empty || strGdl1Value == string.Empty)
                    {
                        boolcellstatus = false;
                        AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                    {
                        dataGridView.Rows[introwindex].Cells["GDL_2_VALUE"].Value = string.Empty;
                        AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                        boolcellstatus = false;
                    }
                }
                if (strType != "FED")
                {
                    if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_3_VALUE"))
                    {
                        if (strGdlStartDate == string.Empty || strGdlEndDate == string.Empty || strGdl1Value == string.Empty || strGdl2Value == string.Empty)
                        {
                            boolcellstatus = false;
                            dataGridView.Rows[introwindex].Cells["GDL_3_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {
                                dataGridView.Rows[introwindex].Cells["GDL_3_VALUE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            }
                        }
                    }

                    if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_4_VALUE"))
                    {
                        if (strGdl1Value == string.Empty || strGdl2Value == string.Empty || strGdl3Value == string.Empty)
                        {
                            boolcellstatus = false;
                            dataGridView.Rows[introwindex].Cells["GDL_4_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {
                                dataGridView.Rows[introwindex].Cells["GDL_4_VALUE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_5_VALUE"))
                    {
                        if (strGdl1Value == string.Empty || strGdl2Value == string.Empty || strGdl3Value == string.Empty || strGdl4Value == string.Empty)
                        {
                            boolcellstatus = false;
                            dataGridView.Rows[introwindex].Cells["GDL_5_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {
                                dataGridView.Rows[introwindex].Cells["GDL_5_VALUE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    if (dataGridView.Columns[e.ColumnIndex].Name.Equals("GDL_6_VALUE"))
                    {
                        if (strGdl1Value == string.Empty || strGdl2Value == string.Empty || strGdl3Value == string.Empty || strGdl4Value == string.Empty || strGdl5Value == string.Empty)
                        {
                            boolcellstatus = false;
                            dataGridView.Rows[introwindex].Cells["GDL_6_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {

                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                                dataGridView.Rows[introwindex].Cells["GDL_6_VALUE"].Value = string.Empty;
                            }
                        }
                    }
                }
                //if (dataGridView.Columns[e.ColumnIndex].Name.Equals("Hierarchy")
                //     && System.Text.RegularExpressions.Match.Equals(strCurrectValue, string.Empty))
                //{

                //    CommonFunctions.MessageBoxDisplay(Consts.Messages.RequiredHierarchy);

                //}
                if (dataGridView.Columns[e.ColumnIndex].Name.Equals("Hierarchy1"))
                {
                    boolcellstatus = false;
                }
                if (strGdlStartDate != string.Empty && strGdlEndDate != string.Empty && strGdl1Value != string.Empty && strGdl2Value != string.Empty)
                {
                    if (boolcellstatus)
                    {
                        boolChangeStatus = true;
                    }
                }

            }

        }
        private void dataGridSubView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //   boolSubGridStatus = true;

            bool boolSubcellstatus = true;

            DataGridView dataGridSubView = null;
            if (tabControl1.SelectedIndex == 1)
            {
                dataGridSubView = (DataGridView)pnlHUDSubGrid/*splitContainer1.Panel2*/.Controls["HudSubGrid"];
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
            }

            if (dataGridSubView != null)
            {
                if (dataGridSubView.Rows.Count > 1)
                {
                    int intcolindex = dataGridSubView.CurrentCell.ColumnIndex;
                    int introwindex = dataGridSubView.CurrentCell.RowIndex;
                    //string strCurrectValue1 = dataGridView.CurrentCell.Value.ToString();
                    string strCurrectValue = Convert.ToString(dataGridSubView.Rows[introwindex].Cells[intcolindex].Value);
                    string strGdlNoHouseHolds = Convert.ToString(dataGridSubView.Rows[introwindex].Cells["GDL_NO_HOUSEHOLDS"].Value);
                    string strGdl1Value = Convert.ToString(dataGridSubView.Rows[introwindex].Cells["GDL_1_VALUE"].Value);
                    string strGdl2Value = Convert.ToString(dataGridSubView.Rows[introwindex].Cells["GDL_2_VALUE"].Value);
                    string strGdl3Value = Convert.ToString(dataGridSubView.Rows[introwindex].Cells["GDL_3_VALUE"].Value);
                    string strGdl4Value = Convert.ToString(dataGridSubView.Rows[introwindex].Cells["GDL_4_VALUE"].Value);
                    string strGdl5Value = Convert.ToString(dataGridSubView.Rows[introwindex].Cells["GDL_5_VALUE"].Value);
                    string strGdl6Value = Convert.ToString(dataGridSubView.Rows[introwindex].Cells["GDL_6_VALUE"].Value);

                    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_NO_HOUSEHOLDS"))
                    {
                        if (strCurrectValue == string.Empty)
                        {
                            //CommonFunctions.MessageBoxDisplay(Consts.Messages.RequiredData);
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString))
                        {
                            dataGridSubView.Rows[introwindex].Cells["GDL_NO_HOUSEHOLDS"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            boolSubcellstatus = false;
                        }
                        for (int i = 0; i < dataGridSubView.Rows.Count - 2; i++)
                        {
                            string strMainGdlNoHouseHolds = Convert.ToString(dataGridSubView.Rows[i].Cells["GDL_NO_HOUSEHOLDS"].Value);
                            if (strGdlNoHouseHolds.TrimStart('0') == strMainGdlNoHouseHolds.TrimStart('0'))
                            {
                                dataGridSubView.Rows[introwindex].Cells["GDL_NO_HOUSEHOLDS"].Value = string.Empty;
                                AlertBox.Show("Fam Size value is already existed", MessageBoxIcon.Warning);//("Already exist Fam size value");
                                boolSubcellstatus = false;
                                return;
                            }
                        }
                    }

                    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_1_VALUE"))
                    {
                        if (strGdlNoHouseHolds == string.Empty)
                        {
                            boolSubcellstatus = false;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                        {

                            AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            boolSubcellstatus = false;
                            dataGridSubView.Rows[introwindex].Cells["GDL_1_VALUE"].Value = string.Empty;
                        }
                    }
                    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_2_VALUE"))
                    {
                        if (strGdlNoHouseHolds == string.Empty || strGdl1Value == string.Empty)
                        {
                            boolSubcellstatus = false;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                        {

                            AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            boolSubcellstatus = false;
                            dataGridSubView.Rows[introwindex].Cells["GDL_2_VALUE"].Value = string.Empty;
                        }
                    }
                    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_3_VALUE"))
                    {
                        if (strGdl1Value == string.Empty || strGdl2Value == string.Empty)
                        {
                            boolSubcellstatus = false;
                            dataGridSubView.Rows[introwindex].Cells["GDL_3_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);

                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {
                                dataGridSubView.Rows[introwindex].Cells["GDL_3_VALUE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            }
                        }
                    }

                    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_4_VALUE"))
                    {
                        if (strGdlNoHouseHolds == string.Empty || strGdl1Value == string.Empty || strGdl2Value == string.Empty || strGdl3Value == string.Empty)
                        {
                            boolSubcellstatus = false;
                            dataGridSubView.Rows[introwindex].Cells["GDL_4_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {
                                dataGridSubView.Rows[introwindex].Cells["GDL_4_VALUE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_5_VALUE"))
                    {
                        if (strGdlNoHouseHolds == string.Empty || strGdl1Value == string.Empty || strGdl2Value == string.Empty || strGdl3Value == string.Empty || strGdl4Value == string.Empty)
                        {
                            boolSubcellstatus = false;
                            dataGridSubView.Rows[introwindex].Cells["GDL_5_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {
                                dataGridSubView.Rows[introwindex].Cells["GDL_5_VALUE"].Value = string.Empty;
                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_6_VALUE"))
                    {
                        if (strGdlNoHouseHolds == string.Empty || strGdl1Value == string.Empty || strGdl2Value == string.Empty || strGdl3Value == string.Empty || strGdl4Value == string.Empty || strGdl5Value == string.Empty)
                        {
                            boolSubcellstatus = false;
                            dataGridSubView.Rows[introwindex].Cells["GDL_6_VALUE"].Value = string.Empty;
                            AlertBox.Show(Consts.Messages.RequiredbelowMaspoverty, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                            {

                                AlertBox.Show(Consts.Messages.PleaseEnterDecimals, MessageBoxIcon.Warning);
                                dataGridSubView.Rows[introwindex].Cells["GDL_6_VALUE"].Value = string.Empty;
                            }
                        }
                    }

                    if (strGdlNoHouseHolds != string.Empty && strGdl1Value != string.Empty && strGdl2Value != string.Empty)
                    {
                        if (boolSubcellstatus)
                        {
                            boolSubGridStatus = true;
                        }
                    }
                }
            }
        }

        private void dataGridSubView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (boolSubGridStatus)
            {

                DataGridView dataGridSubView = null;
                DataGridView dataGridView = null;
                string strType = "";
                string strCounty = "";

                if (tabControl1.SelectedIndex == 1)
                {
                    string strcmbCounty = "";
                    if (cmbCounty.Items.Count > 0)
                        strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();

                    dataGridView = gvwHud;
                    dataGridSubView = (DataGridView)pnlHUDSubGrid.Controls["HudSubGrid"];

                    strCounty = strcmbCounty;
                    strType = "HUD";
                }
                else if (tabControl1.SelectedIndex == 2)
                {

                    string strcmbCmiCounty = "";
                    if (cmbCMICounty.Items.Count > 0)
                        strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();

                    dataGridView = gvwCMI;
                    dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
                    strCounty = strcmbCmiCounty;
                    strType = "CMI";
                }
                else if (tabControl1.SelectedIndex == 3)
                {

                    dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                    dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
                    strType = "SMI";

                }
                if (dataGridView.SelectedRows.Count == 0)
                    return;
                string strHierarchy = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value);
                string[] strSplit = strHierarchy.Split('-');
                if (strSplit.Length > 1)
                {
                    strPublicAgency = strSplit[0].ToString();
                    strPublicDept = strSplit[1].ToString();
                    strPublicProgram = strSplit[2].ToString();
                }

                string strMode = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["Mode"].Value);
                string strGdlNoHolds = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["GDL_NO_HOUSEHOLDS"].Value);
                string strGdl1Value = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["GDL_1_VALUE"].Value);
                string strGdl2Value = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["GDL_2_VALUE"].Value);
                string strGdl3Value = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["GDL_3_VALUE"].Value);
                string strGdl4Value = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["GDL_4_VALUE"].Value);
                string strGdl5Value = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["GDL_5_VALUE"].Value);
                string strGdl6Value = Convert.ToString(dataGridSubView.Rows[e.RowIndex].Cells["GDL_6_VALUE"].Value);


                CaptainModel model = new CaptainModel();
                MasterPovertyEntity masterPovertyDetails = new MasterPovertyEntity();
                if (strPublicStartDate != "" && strPublicEndDate != "" && strGdl1Value != "" && strGdl2Value != "")//&& strGdl3Value != ""
                {
                    masterPovertyDetails.GdlAgency = strPublicAgency;
                    masterPovertyDetails.GdlDept = strPublicDept;
                    masterPovertyDetails.GdlProgram = strPublicProgram;
                    masterPovertyDetails.GdlOldAgency = strPublicAgency;
                    masterPovertyDetails.GdlOldDept = strPublicDept;
                    masterPovertyDetails.GdlOldProgram = strPublicProgram;
                    masterPovertyDetails.GdlType = strType;
                    masterPovertyDetails.GdlCounty = strCounty;
                    masterPovertyDetails.GdlStartDate = strPublicStartDate;
                    masterPovertyDetails.GdlEndDate = strPublicEndDate;
                    masterPovertyDetails.GdlNoHouseHolds = strGdlNoHolds;
                    masterPovertyDetails.Gdl1Value = strGdl1Value;
                    masterPovertyDetails.Gdl2Value = strGdl2Value;
                    masterPovertyDetails.Gdl3Value = strGdl3Value;
                    masterPovertyDetails.Gdl4Value = strGdl4Value;
                    masterPovertyDetails.Gdl5Value = strGdl5Value;
                    masterPovertyDetails.Gdl6Value = strGdl6Value;
                    masterPovertyDetails.GdlAddOperator = BaseForm.UserID;
                    masterPovertyDetails.GdlLstcOperator = BaseForm.UserID;
                    masterPovertyDetails.Mode = strMode;
                    List<MasterPovertyEntity> modelPovertyList = model.masterPovertyData.GetCaseGdlSubadptMain(strPublicAgency, strPublicDept, strPublicProgram, strType, strCounty, strPublicStartDate, strPublicEndDate);
                    if (modelPovertyList.Count > 0)
                    {
                        bool boolsucess = model.masterPovertyData.InsertCaseGdl(masterPovertyDetails);
                        if (boolsucess)
                        {
                            if (strMode == "U")
                            {
                                AlertBox.Show("Record Updated Successfully");
                                //CommonFunctions.MessageBoxDisplay("Record updated successfully");
                            }
                            else
                            {
                                AlertBox.Show("Record Saved Successfully");
                                //CommonFunctions.MessageBoxDisplay("Record Saved Successfully");
                            }
                        }
                        dataGridSubView.Rows[e.RowIndex].Cells["Mode"].Value = 'U';
                        // dataGridSubView.Rows[e.RowIndex].Cells["Delete"].Value = "";
                        strPublicAgency = "";
                        strPublicDept = "";
                        strPublicProgram = "";
                        boolSubGridStatus = false;
                    }
                    else
                    {
                        AlertBox.Show("The record in the top grid is not saved, please enter all columns in top grid", MessageBoxIcon.Warning);
                    }
                }

                else
                {
                    // CommonFunctions.MessageBoxDisplay("Date Already Exist..");
                }

            }
        }

        private void dataGridSubView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            //DataGridView dataGridSubView = null;
            //if (tabControl1.SelectedIndex == 1)
            //{
            //    dataGridSubView = (DataGridView)splitContainer1.Panel2.Controls["HudSubGrid"];
            //}
            //else if (tabControl1.SelectedIndex == 2)
            //{
            //    dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
            //}
            //else if (tabControl1.SelectedIndex == 3)
            //{
            //    dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
            //}

            //if (dataGridSubView.IsCurrentCellInEditMode)
            //{

            //    string formatedValue = e.FormattedValue as string;
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_NO_HOUSEHOLDS")
            //   && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);

            //    }

            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_1_VALUE")
            //        && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);

            //    }
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_2_VALUE")
            //       && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);

            //    }
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_3_VALUE")
            //       && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);

            //    }
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_4_VALUE")
            //       && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);

            //    }
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_5_VALUE")
            //       && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);

            //    }
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_6_VALUE")
            //       && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.TwoDecimalString))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);

            //    }
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_START_DATE")
            //          && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.DateFormatMMDDYYYY))
            //    {
            //        dataGridSubView.Rows[e.RowIndex].ErrorText = String.Empty;
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyyyDateFormat);


            //    }
            //    if (dataGridSubView.Columns[e.ColumnIndex].Name.Equals("GDL_END_DATE")
            //          && !System.Text.RegularExpressions.Regex.IsMatch(formatedValue, Consts.StaticVars.DateFormatMMDDYYYY))
            //    {
            //        e.Cancel = true;
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyyyDateFormat);

            //    }


            // }
        }

        private void dataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (boolChangeStatus)
            {
                DataGridView dataGridView = null;
                DataGridView dataGridSubView = null;
                string strType = "";
                string strCounty = "";
                string Agency = "";
                string Dept = "";
                string Program = "";
                CaptainModel model = new CaptainModel();

                if (tabControl1.SelectedIndex == 0)
                {
                    dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                    strType = "FED";
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, null);
                    //if (dataGridView.Rows[e.RowIndex].Tag is MasterPovertyEntity)
                    //{

                    //}
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    string strcmbCounty = "";
                    strType = "HUD";
                    if (cmbCounty.Items.Count > 0)
                        strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();

                    dataGridView = gvwHud;
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, strcmbCounty);
                    dataGridSubView = (DataGridView)pnlHUDSubGrid.Controls["HudSubGrid"];
                    FillGridSubData(dataGridView, dataGridSubView, "HUD", strcmbCounty);
                    strCounty = strcmbCounty;

                }
                else if (tabControl1.SelectedIndex == 2)
                {

                    string strcmbCmiCounty = "";
                    strType = "CMI";
                    if (cmbCMICounty.Items.Count > 0)
                        strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();

                    dataGridView = gvwCMI;
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, strcmbCmiCounty);
                    dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
                    FillGridSubData(dataGridView, dataGridSubView, "CMI", strcmbCmiCounty);
                    strCounty = strcmbCmiCounty;

                }
                else if (tabControl1.SelectedIndex == 3)
                {

                    dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                    // dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
                    //  FillGridSubData(dataGridView, dataGridSubView, "**", "**", "**", "SMI", "");
                    strType = "SMI";
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, null);
                }
                foreach (DataGridViewRow dr in dataGridView.SelectedRows)
                {
                    if (dr.Selected)
                    {

                        string strMode = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["Mode"].Value);
                        string strNewStartDate = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_START_DATE"].Value);
                        string strNewEndDate = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_END_DATE"].Value);
                        string strPovBase = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_1_VALUE"].Value);
                        string strIncrement = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_2_VALUE"].Value);
                        string strHierarchy = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["Hierarchy"].Value);
                        strIndex = dataGridView.Rows[e.RowIndex].Index;
                        string[] strSplit;
                        string[] strOldSplit;
                        if (strMode == "U")
                        {
                            strSplit = strHierarchy.Split('-');

                        }
                        else
                        {

                            if (strPublicCode != "")
                            {
                                strSplit = strPublicCode.Split('-');
                            }
                            else
                            {
                                strHierarchy = Convert.ToString(dataGridView.CurrentRow.Cells["Hierarchy"].Value);
                                if (strHierarchy == "")
                                {
                                    if (strNewStartDate != "" && strNewEndDate != "")
                                    {
                                        AlertBox.Show("Please Select Hierarchy", MessageBoxIcon.Warning);
                                        dataGridView.Rows[strIndex].Selected = true;
                                        boolChangeStatus = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    strPublicCode = strHierarchy;
                                }
                                strSplit = strPublicCode.Split('-');
                            }
                        }

                        if (strSplit.Length > 1)
                        {
                            Agency = strSplit[0].ToString();
                            Dept = strSplit[1].ToString();
                            Program = strSplit[2].ToString();
                            strPublicAgency = strSplit[0].ToString();
                            strPublicDept = strSplit[1].ToString();
                            strPublicProgram = strSplit[2].ToString();
                        }

                        string strGdl3Value = "";
                        string strGdl4Value = "";
                        string strGdl5Value = "";
                        string strGdl6Value = "";
                        strPublicStartDate = strNewStartDate;
                        strPublicEndDate = strNewEndDate;
                        if (strType == "SMI" || strType == "HUD" || strType == "CMI")
                        {
                            strGdl3Value = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_3_VALUE"].Value);
                            strGdl4Value = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_4_VALUE"].Value);
                            strGdl5Value = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_5_VALUE"].Value);
                            strGdl6Value = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells["GDL_6_VALUE"].Value);
                        }
                        string strStartDate = "";
                        string strEndDate = "";
                        bool boolDateCheck = false;
                        int intCount = 0;
                        if (strMode != "U")
                        {
                            if (_masterPovertyEntiry.Count > 0)
                            {
                                if (strNewStartDate != "" && strNewEndDate != "")
                                {
                                    int totalrows = model.masterPovertyData.CaseGdldateExistCheck(strPublicAgency, strPublicDept, strPublicProgram, strType, strNewStartDate, strNewEndDate, strCounty);
                                    if (totalrows == 0)
                                    {
                                        boolDateCheck = true;
                                    }
                                    else
                                    {
                                        AlertBox.Show("Date already exists", MessageBoxIcon.Warning);
                                        dataGridView.Rows[strIndex].Selected = true;
                                        return;
                                    }

                                }
                                else
                                    return;

                            }
                            else
                            {
                                boolDateCheck = true;
                            }
                        }
                        else
                        {
                            boolDateCheck = true;
                        }
                        if (boolDateCheck)
                        {

                            MasterPovertyEntity masterPovertyDetails = new MasterPovertyEntity();
                            if (strType == "SMI" || strType == "HUD" || strType == "CMI")
                            {
                                if (strNewStartDate != "" && strNewEndDate != "" && strPovBase != "" && strIncrement != "")//&& strGdl3Value != ""
                                {

                                    masterPovertyDetails.GdlAgency = Agency;
                                    masterPovertyDetails.GdlDept = Dept;
                                    masterPovertyDetails.GdlProgram = Program;
                                    if (strMode != "U")
                                    {
                                        masterPovertyDetails.GdlOldAgency = strOldAgency;
                                        masterPovertyDetails.GdlOldDept = strOldDept;
                                        masterPovertyDetails.GdlOldProgram = strOldProgram;
                                    }
                                    else
                                    {
                                        masterPovertyDetails.GdlOldAgency = Agency;
                                        masterPovertyDetails.GdlOldDept = Dept;
                                        masterPovertyDetails.GdlOldProgram = Program;
                                    }
                                    masterPovertyDetails.GdlType = strType;
                                    masterPovertyDetails.GdlCounty = strCounty;
                                    masterPovertyDetails.GdlStartDate = strNewStartDate;
                                    masterPovertyDetails.GdlEndDate = strNewEndDate;
                                    masterPovertyDetails.GdlNoHouseHolds = "0";
                                    masterPovertyDetails.Gdl1Value = strPovBase;
                                    masterPovertyDetails.Gdl2Value = strIncrement;
                                    masterPovertyDetails.Gdl3Value = strGdl3Value;
                                    masterPovertyDetails.Gdl4Value = strGdl4Value;
                                    masterPovertyDetails.Gdl5Value = strGdl5Value;
                                    masterPovertyDetails.Gdl6Value = strGdl6Value;
                                    masterPovertyDetails.GdlAddOperator = BaseForm.UserID;
                                    masterPovertyDetails.GdlLstcOperator = BaseForm.UserID;
                                    masterPovertyDetails.Mode = strMode;

                                    bool boolsucess = model.masterPovertyData.InsertCaseGdl(masterPovertyDetails);
                                    if (boolsucess)
                                    {

                                        if (strMode == "U")
                                        {
                                            AlertBox.Show("Record Updated Successfully");
                                            //CommonFunctions.MessageBoxDisplay("Record updated successfully");
                                        }
                                        else
                                        {
                                            //dataGridView.CurrentRow.Cells["Mode"].Value = 'U';
                                            //dataGridView.CurrentRow.Cells["Hierarchy"].Value = strPublicCode;
                                            dataGridView.Rows[e.RowIndex].Cells["Mode"].Value = 'U';
                                            dataGridView.Rows[e.RowIndex].Cells["Hierarchy"].Value = strPublicCode;
                                            AlertBox.Show("Record Saved Successfully");
                                            //CommonFunctions.MessageBoxDisplay("Record Saved Successfully");
                                        }
                                    }
                                    boolChangeStatus = false;
                                    strPublicCode = "";
                                    strOldPublicCode = "";
                                    // RefreshGrid();
                                }
                                else
                                {
                                    boolChangeStatus = false;
                                    strPublicCode = "";
                                    strOldPublicCode = "";
                                }
                            }
                            else
                            {

                                if (strNewStartDate != "" && strNewEndDate != "" && strPovBase != "" && strIncrement != "")
                                {

                                    masterPovertyDetails.GdlAgency = Agency;
                                    masterPovertyDetails.GdlDept = Dept;
                                    masterPovertyDetails.GdlProgram = Program;
                                    if (strMode != "U")
                                    {
                                        masterPovertyDetails.GdlOldAgency = strOldAgency;
                                        masterPovertyDetails.GdlOldDept = strOldDept;
                                        masterPovertyDetails.GdlOldProgram = strOldProgram;
                                    }
                                    else
                                    {
                                        masterPovertyDetails.GdlOldAgency = Agency;
                                        masterPovertyDetails.GdlOldDept = Dept;
                                        masterPovertyDetails.GdlOldProgram = Program;
                                    }
                                    masterPovertyDetails.GdlType = strType;
                                    masterPovertyDetails.GdlCounty = strCounty;
                                    masterPovertyDetails.GdlStartDate = strNewStartDate;
                                    masterPovertyDetails.GdlEndDate = strNewEndDate;
                                    masterPovertyDetails.GdlNoHouseHolds = "0";
                                    masterPovertyDetails.Gdl1Value = strPovBase;
                                    masterPovertyDetails.Gdl2Value = strIncrement;
                                    masterPovertyDetails.Gdl3Value = strGdl3Value;
                                    masterPovertyDetails.Gdl4Value = strGdl4Value;
                                    masterPovertyDetails.Gdl5Value = strGdl5Value;
                                    masterPovertyDetails.Gdl6Value = strGdl6Value;
                                    masterPovertyDetails.GdlAddOperator = BaseForm.UserID;
                                    masterPovertyDetails.GdlLstcOperator = BaseForm.UserID;
                                    masterPovertyDetails.Mode = strMode;

                                    bool boolsucess = model.masterPovertyData.InsertCaseGdl(masterPovertyDetails);
                                    if (boolsucess)
                                    {

                                        if (strMode == "U")
                                        {
                                            AlertBox.Show("Record Updated Successfully");// CommonFunctions.MessageBoxDisplay("Record updated successfully");
                                        }
                                        else
                                        {
                                            //dataGridView.CurrentRow.Cells["Mode"].Value = 'U';
                                            //dataGridView.CurrentRow.Cells["Hierarchy"].Value = strPublicCode;
                                            dataGridView.Rows[e.RowIndex].Cells["Mode"].Value = 'U';
                                            dataGridView.Rows[e.RowIndex].Cells["Hierarchy"].Value = strPublicCode;
                                            AlertBox.Show("Record Saved Successfully");// CommonFunctions.MessageBoxDisplay("Record Saved Successfully");
                                        }
                                    }
                                    boolChangeStatus = false;
                                    strPublicCode = "";
                                    strOldPublicCode = "";
                                    // RefreshGrid();
                                }
                                else
                                {
                                    boolChangeStatus = false;
                                    strPublicCode = "";
                                    strOldPublicCode = "";
                                }

                            }
                        }
                        else
                        {
                            boolChangeStatus = false;
                            strPublicCode = "";
                            strOldPublicCode = "";
                        }
                    }
                }
                //  Exit:
                //  CommonFunctions.MessageBoxDisplay("Date already Exits ");

            }

        }

        private void MasterPovertyaddupdate()
        {
            if (boolChangeStatus)
            {
                DataGridView dataGridView = null;
                DataGridView dataGridSubView = null;
                string strType = "";
                string strCounty = "";
                string Agency = "";
                string Dept = "";
                string Program = "";
                CaptainModel model = new CaptainModel();

                if (tabControl1.SelectedIndex == 0)
                {
                    dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                    strType = "FED";
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, null);
                    //if (dataGridView.Rows[e.RowIndex].Tag is MasterPovertyEntity)
                    //{

                    //}
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    string strcmbCounty = "";
                    strType = "HUD";
                    if (cmbCounty.Items.Count > 0)
                        strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();

                    dataGridView = gvwHud;
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, strcmbCounty);
                    dataGridSubView = (DataGridView)pnlHUDSubGrid.Controls["HudSubGrid"];
                    FillGridSubData(dataGridView, dataGridSubView, "HUD", strcmbCounty);
                    strCounty = strcmbCounty;

                }
                else if (tabControl1.SelectedIndex == 2)
                {

                    string strcmbCmiCounty = "";
                    strType = "CMI";
                    if (cmbCMICounty.Items.Count > 0)
                        strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();

                    dataGridView = gvwCMI;
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, strcmbCmiCounty);
                    dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
                    FillGridSubData(dataGridView, dataGridSubView, "CMI", strcmbCmiCounty);
                    strCounty = strcmbCmiCounty;

                }
                else if (tabControl1.SelectedIndex == 3)
                {

                    dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                    // dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
                    //  FillGridSubData(dataGridView, dataGridSubView, "**", "**", "**", "SMI", "");
                    strType = "SMI";
                    _masterPovertyEntiry = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, strType, null);
                }
                foreach (DataGridViewRow dr in dataGridView.SelectedRows)
                {
                    if (dr.Selected)
                    {


                        string strMode = Convert.ToString(dataGridView.CurrentRow.Cells["Mode"].Value);
                        string strNewStartDate = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_START_DATE"].Value);
                        string strNewEndDate = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_END_DATE"].Value);
                        string strPovBase = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_1_VALUE"].Value);
                        string strIncrement = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_2_VALUE"].Value);
                        string strHierarchy = Convert.ToString(dataGridView.CurrentRow.Cells["Hierarchy"].Value);
                        strIndex = dataGridView.CurrentRow.Index;
                        string[] strSplit;
                        if (strMode == "U")
                        {
                            strSplit = strHierarchy.Split('-');
                        }
                        else
                        {

                            if (strPublicCode != "")
                            {
                                strSplit = strPublicCode.Split('-');
                            }
                            else
                            {
                                strHierarchy = Convert.ToString(dataGridView.CurrentRow.Cells["Hierarchy"].Value);
                                if (strHierarchy == "")
                                {
                                    //if (strNewStartDate != "" && strNewEndDate != "")
                                    //{
                                    //    CommonFunctions.MessageBoxDisplay("Please Select Hierarchy");
                                    //    dataGridView.Rows[strIndex].Selected = true;
                                    //    boolChangeStatus = false;
                                    //    return;
                                    //}
                                }
                                else
                                {
                                    strPublicCode = strHierarchy;
                                }
                                strSplit = strPublicCode.Split('-');
                            }
                        }

                        if (strSplit.Length > 1)
                        {
                            Agency = strSplit[0].ToString();
                            Dept = strSplit[1].ToString();
                            Program = strSplit[2].ToString();
                            strPublicAgency = strSplit[0].ToString();
                            strPublicDept = strSplit[1].ToString();
                            strPublicProgram = strSplit[2].ToString();
                        }

                        string strGdl3Value = "";
                        string strGdl4Value = "";
                        string strGdl5Value = "";
                        string strGdl6Value = "";
                        strPublicStartDate = strNewStartDate;
                        strPublicEndDate = strNewEndDate;
                        if (strType == "SMI" || strType == "HUD" || strType == "CMI")
                        {
                            strGdl3Value = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_3_VALUE"].Value);
                            strGdl4Value = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_4_VALUE"].Value);
                            strGdl5Value = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_5_VALUE"].Value);
                            strGdl6Value = Convert.ToString(dataGridView.CurrentRow.Cells["GDL_6_VALUE"].Value);
                        }
                        string strStartDate = "";
                        string strEndDate = "";
                        bool boolDateCheck = false;
                        int intCount = 0;
                        if (strMode != "U")
                        {
                            if (_masterPovertyEntiry.Count > 0)
                            {
                                if (strNewStartDate != "" && strNewEndDate != "")
                                {
                                    int totalrows = model.masterPovertyData.CaseGdldateExistCheck(strPublicAgency, strPublicDept, strPublicProgram, strType, strNewStartDate, strNewEndDate, strCounty);
                                    if (totalrows == 0)
                                    {
                                        boolDateCheck = true;
                                    }
                                    else
                                    {
                                        AlertBox.Show("Date already exist", MessageBoxIcon.Warning);
                                        dataGridView.Rows[strIndex].Selected = true;
                                        return;
                                    }

                                }
                                else
                                    return;

                            }
                            else
                            {
                                boolDateCheck = true;
                            }
                        }
                        else
                        {
                            boolDateCheck = true;
                        }
                        if (boolDateCheck)
                        {

                            MasterPovertyEntity masterPovertyDetails = new MasterPovertyEntity();
                            if (strType == "SMI" || strType == "HUD" || strType == "CMI")
                            {
                                if (strNewStartDate != "" && strNewEndDate != "" && strPovBase != "" && strIncrement != "")//&& strGdl3Value != ""
                                {

                                    masterPovertyDetails.GdlAgency = Agency;
                                    masterPovertyDetails.GdlDept = Dept;
                                    masterPovertyDetails.GdlProgram = Program;
                                    if (strMode != "U")
                                    {
                                        masterPovertyDetails.GdlOldAgency = strOldAgency;
                                        masterPovertyDetails.GdlOldDept = strOldDept;
                                        masterPovertyDetails.GdlOldProgram = strOldProgram;
                                    }
                                    else
                                    {
                                        masterPovertyDetails.GdlOldAgency = Agency;
                                        masterPovertyDetails.GdlOldDept = Dept;
                                        masterPovertyDetails.GdlOldProgram = Program;
                                    }
                                    masterPovertyDetails.GdlType = strType;
                                    masterPovertyDetails.GdlCounty = strCounty;
                                    masterPovertyDetails.GdlStartDate = strNewStartDate;
                                    masterPovertyDetails.GdlEndDate = strNewEndDate;
                                    masterPovertyDetails.GdlNoHouseHolds = "0";
                                    masterPovertyDetails.Gdl1Value = strPovBase;
                                    masterPovertyDetails.Gdl2Value = strIncrement;
                                    masterPovertyDetails.Gdl3Value = strGdl3Value;
                                    masterPovertyDetails.Gdl4Value = strGdl4Value;
                                    masterPovertyDetails.Gdl5Value = strGdl5Value;
                                    masterPovertyDetails.Gdl6Value = strGdl6Value;
                                    masterPovertyDetails.GdlAddOperator = BaseForm.UserID;
                                    masterPovertyDetails.GdlLstcOperator = BaseForm.UserID;
                                    masterPovertyDetails.Mode = strMode;
                                    bool boolsucess = model.masterPovertyData.InsertCaseGdl(masterPovertyDetails);
                                    if (boolsucess)
                                    {

                                        if (strMode == "U")
                                        {
                                            AlertBox.Show("Record Updated Successfully");
                                            //CommonFunctions.MessageBoxDisplay("Record updated successfully");
                                        }
                                        else
                                        {
                                            dataGridView.CurrentRow.Cells["Mode"].Value = 'U';
                                            dataGridView.CurrentRow.Cells["Hierarchy"].Value = strPublicCode;
                                            AlertBox.Show("Record Saved Successfully");// CommonFunctions.MessageBoxDisplay("Record Saved Successfully");
                                            dataGridView.CurrentRow.Cells["Hierarchy"].ReadOnly = true;
                                            dataGridView.CurrentRow.Cells["GDL_START_DATE"].ReadOnly = true;
                                            dataGridView.CurrentRow.Cells["GDL_END_DATE"].ReadOnly = true;
                                        }
                                    }
                                    boolChangeStatus = false;
                                    strPublicCode = "";

                                    // RefreshGrid();
                                }
                                else
                                {
                                    boolChangeStatus = false;
                                    strPublicCode = "";
                                }

                            }
                            else
                            {
                                if (strNewStartDate != "" && strNewEndDate != "" && strPovBase != "" && strIncrement != "")
                                {

                                    masterPovertyDetails.GdlAgency = Agency;
                                    masterPovertyDetails.GdlDept = Dept;
                                    masterPovertyDetails.GdlProgram = Program;
                                    if (strMode != "U")
                                    {
                                        masterPovertyDetails.GdlOldAgency = strOldAgency;
                                        masterPovertyDetails.GdlOldDept = strOldDept;
                                        masterPovertyDetails.GdlOldProgram = strOldProgram;
                                    }
                                    else
                                    {
                                        masterPovertyDetails.GdlOldAgency = Agency;
                                        masterPovertyDetails.GdlOldDept = Dept;
                                        masterPovertyDetails.GdlOldProgram = Program;
                                    }
                                    masterPovertyDetails.GdlType = strType;
                                    masterPovertyDetails.GdlCounty = strCounty;
                                    masterPovertyDetails.GdlStartDate = strNewStartDate;
                                    masterPovertyDetails.GdlEndDate = strNewEndDate;
                                    masterPovertyDetails.GdlNoHouseHolds = "0";
                                    masterPovertyDetails.Gdl1Value = strPovBase;
                                    masterPovertyDetails.Gdl2Value = strIncrement;
                                    masterPovertyDetails.Gdl3Value = strGdl3Value;
                                    masterPovertyDetails.Gdl4Value = strGdl4Value;
                                    masterPovertyDetails.Gdl5Value = strGdl5Value;
                                    masterPovertyDetails.Gdl6Value = strGdl6Value;
                                    masterPovertyDetails.GdlAddOperator = BaseForm.UserID;
                                    masterPovertyDetails.GdlLstcOperator = BaseForm.UserID;
                                    masterPovertyDetails.Mode = strMode;
                                    bool boolsucess = model.masterPovertyData.InsertCaseGdl(masterPovertyDetails);
                                    if (boolsucess)
                                    {

                                        if (strMode == "U")
                                        {
                                            AlertBox.Show("Record Updated Successfully");
                                            // CommonFunctions.MessageBoxDisplay("Record updated successfully");
                                        }
                                        else
                                        {
                                            dataGridView.CurrentRow.Cells["Mode"].Value = 'U';
                                            dataGridView.CurrentRow.Cells["Hierarchy"].Value = strPublicCode;
                                            AlertBox.Show("Record Saved Successfully");//CommonFunctions.MessageBoxDisplay("Record Saved Successfully");
                                            dataGridView.CurrentRow.Cells["Hierarchy"].ReadOnly = true;
                                            dataGridView.CurrentRow.Cells["GDL_START_DATE"].ReadOnly = true;
                                            dataGridView.CurrentRow.Cells["GDL_END_DATE"].ReadOnly = true;
                                        }
                                    }
                                    boolChangeStatus = false;
                                    strPublicCode = "";

                                    // RefreshGrid();
                                }
                                else
                                {
                                    boolChangeStatus = false;
                                    strPublicCode = "";
                                }
                            }
                        }
                        else
                        {
                            boolChangeStatus = false;
                            strPublicCode = "";
                        }
                    }
                }
                //  Exit:
                //  CommonFunctions.MessageBoxDisplay("Date already Exits ");

            }

        }


        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = null;
            DataGridView dataGridSubView = null;

            //     case "HUD":
            if (tabControl1.SelectedIndex == 0)
            {

            }
            else if (tabControl1.SelectedIndex == 1)
            {
                string strcmbCounty = "";
                if (cmbCounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();

                dataGridView = gvwHud;
                dataGridSubView = (DataGridView)pnlHUDSubGrid.Controls["HudSubGrid"];
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick -= new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
                FillGridSubData(dataGridView, dataGridSubView, "HUD", strcmbCounty);
                dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick += new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged += new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);

            }
            //case "CMI":
            else if (tabControl1.SelectedIndex == 2)
            {
                string strcmbCmiCounty = "";
                if (cmbCMICounty.Items.Count > 0)
                    strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();

                dataGridView = gvwCMI;
                dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick -= new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
                FillGridSubData(dataGridView, dataGridSubView, "CMI", strcmbCmiCounty);
                dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick += new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged += new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
            }
            //"SMI":
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellClick -= new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
                FillGridSubData(dataGridView, dataGridSubView, "SMI", "");
                dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick += new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged += new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
            }

            boolSubGridStatus = false;

        }


        private void FillGridData(DataGridView dataGridview, string Agency, string Dept, string Program, string Type, string County)
        {
            CaptainModel model = new CaptainModel();
            List<MasterPovertyEntity> modelPovertyList = model.masterPovertyData.GetCaseGdladpt(Agency, Dept, Program, Type, County);
            if (Type == "HUD" || Type == "CMI")
            {
                if (modelPovertyList.Count > 0)
                    modelPovertyList = modelPovertyList.FindAll(u => u.GdlCounty == County);
            }
            _masterPovertyEntiry = modelPovertyList;
            dataGridview.Rows.Clear(); //int rowIndex = 0;
            switch (Type)
            {
                case "FED":

                    foreach (MasterPovertyEntity hierarchy in modelPovertyList)
                    {
                        PovertyException propIncrementdata = model.masterPovertyData.GetPovertyException(hierarchy.GdlAgency, hierarchy.GdlDept, hierarchy.GdlProgram, hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, string.Empty, string.Empty);
                        string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                        int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), code, "..." /*"Hierarchy"*/);
                        dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                        dataGridview.Rows[rowIndex].Tag = hierarchy;
                        dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                        dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                        dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                        //dataGridview.ItemsPerPage = 100;
                        CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                        if (Privileges.ChangePriv.Equals("false"))
                        {
                            dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                            dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                        }
                        else
                        {
                            dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                            dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                        }
                        if (propIncrementdata != null)
                        {
                            dataGridview.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                    }
                    if (dataGridview.Rows.Count > 0)
                        dataGridview.Rows[0].Selected = true;
                    break;
                case "HUD":
                case "CMI":
                case "SMI":
                    foreach (MasterPovertyEntity hierarchy in modelPovertyList)
                    {
                        string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                        int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), hierarchy.Gdl3Value.ToString(), hierarchy.Gdl4Value.ToString(), hierarchy.Gdl5Value.ToString(), hierarchy.Gdl6Value.ToString(), code, "..."/*"Hierarchy"*/);
                        dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                        dataGridview.Rows[rowIndex].Tag = hierarchy;
                        dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                        dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                        dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                        // dataGridview.ItemsPerPage = 100;
                        CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                        if (Privileges.ChangePriv.Equals("false"))
                        {
                            dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                            dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                            dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = true;
                            dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = true;
                            dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = true;
                            dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = true;
                        }
                        else
                        {
                            dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                            dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                            dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = false;
                            dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = false;
                            dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = false;
                            dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = false;
                        }
                    }
                    if (dataGridview.Rows.Count > 0)
                        dataGridview.Rows[0].Selected = true;
                    break;
            }
            if (Privileges.AddPriv.Equals("false"))
            {
                dataGridview.AllowUserToAddRows = false;
            }
            else
            {
                dataGridview.AllowUserToAddRows = true;
            }



        }

        private void FillGridSubData(DataGridView dataGridView, DataGridView dataGridSubView, string Type, string County)
        {
            string Agency = "";
            string Dept = "";
            string Program = "";

            switch (Type)
            {
                case "HUD":
                case "CMI":
                case "SMI":
                    if (dataGridView.Rows.Count > 1)
                    {
                        //MasterPovertyEntity row = dataGridView.SelectedRows[0].Tag as MasterPovertyEntity;  
                        string strStartDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_START_DATE"].Value);//DateTime dtStart = DateTime.ParseExact(strStartDate,"mm/dd/yyyy",System.Globalization.CultureInfo.InvariantCulture);
                        string strEndDate = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_END_DATE"].Value); //DateTime dtEnd = DateTime.ParseExact(strEndDate, "mm/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        string strHierarchy = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value);
                        string[] strSplit = strHierarchy.Split('-');
                        if (strSplit.Length > 1)
                        {
                            Agency = strSplit[0].ToString();
                            Dept = strSplit[1].ToString();
                            Program = strSplit[2].ToString();
                        }
                        strPublicStartDate = strStartDate;
                        strPublicEndDate = strEndDate;
                        dataGridSubView.Rows.Clear();
                        dataGridSubView.Columns[1].HeaderText = " ";
                        dataGridSubView.Columns[2].HeaderText = " ";
                        dataGridSubView.Columns[3].HeaderText = " ";
                        dataGridSubView.Columns[4].HeaderText = " ";
                        dataGridSubView.Columns[5].HeaderText = " ";
                        dataGridSubView.Columns[6].HeaderText = " ";
                        dataGridSubView.Columns[7].HeaderText = " ";
                        if (strStartDate != "" && strEndDate != "")
                        {
                            dataGridSubView.Columns[1].HeaderText = "Fam Size";
                            if (Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_1_VALUE"].Value) != "")
                                dataGridSubView.Columns[2].HeaderText = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_1_VALUE"].Value) + "%";
                            if (Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_2_VALUE"].Value) != "")
                                dataGridSubView.Columns[3].HeaderText = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_2_VALUE"].Value) + "%";
                            if (Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_3_VALUE"].Value) != "")
                                dataGridSubView.Columns[4].HeaderText = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_3_VALUE"].Value) + "%";
                            if (Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_4_VALUE"].Value) != "")
                                dataGridSubView.Columns[5].HeaderText = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_4_VALUE"].Value) + "%";
                            if (Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_5_VALUE"].Value) != "")
                                dataGridSubView.Columns[6].HeaderText = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_5_VALUE"].Value) + "%";
                            if (Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_6_VALUE"].Value) != "")
                                dataGridSubView.Columns[7].HeaderText = Convert.ToString(dataGridView.SelectedRows[0].Cells["GDL_6_VALUE"].Value) + "%";
                            CaptainModel model = new CaptainModel();
                            List<MasterPovertyEntity> modelPovertyList = model.masterPovertyData.GetCaseGdlSubadpt(Agency, Dept, Program, Type, County, strStartDate, strEndDate);
                            foreach (MasterPovertyEntity hierarchy in modelPovertyList)
                            {
                                int rowIndex = dataGridSubView.Rows.Add("U", hierarchy.GdlNoHouseHolds, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), hierarchy.Gdl3Value.ToString(), hierarchy.Gdl4Value.ToString(), hierarchy.Gdl5Value.ToString(), hierarchy.Gdl6Value.ToString());
                                dataGridSubView.Rows[rowIndex].Tag = hierarchy;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridSubView);
                                dataGridSubView.Rows[rowIndex].Cells["GDL_NO_HOUSEHOLDS"].ReadOnly = true;
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = true;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = true;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = true;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = true;

                                }
                                else
                                {
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = false;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = false;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = false;
                                    dataGridSubView.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = false;

                                }
                            }
                        }
                    }
                    else
                    {
                        dataGridSubView.Columns[1].HeaderText = " ";
                        dataGridSubView.Columns[2].HeaderText = " ";
                        dataGridSubView.Columns[3].HeaderText = " ";
                        dataGridSubView.Columns[4].HeaderText = " ";
                        dataGridSubView.Columns[5].HeaderText = " ";
                        dataGridSubView.Columns[6].HeaderText = " ";
                        dataGridSubView.Columns[7].HeaderText = " ";
                    }

                    break;

            }
            if (Privileges.AddPriv.Equals("false"))
            {
                dataGridSubView.AllowUserToAddRows = false;
            }
            else
            {
                dataGridSubView.AllowUserToAddRows = true;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                dataGridSubView.Columns["Delete"].Visible = false;
            }
            else
            {
                dataGridSubView.Columns["Delete"].Visible = true;
            }
            if (dataGridSubView.Rows.Count > 0)
                dataGridSubView.Rows[0].Selected = true;
        }

        private void FillSmiGrid()
        {
            DataGridView dataGridSubView = null;
            DataGridView dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
            if (dataGridView != null)
            {
                dataGridView.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                dataGridView.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridView.Rows.Clear();
                FillGridData(dataGridView, "", "", "", "SMI", "");
                dataGridView.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
                dataGridSubView.Rows.Clear();
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick -= new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
                FillGridSubData(dataGridView, dataGridSubView, "SMI", "");
                dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick += new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged += new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);

            }
        }


        private void FillFedGrid()
        {

            DataGridView dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
            if (dataGridView != null)
            {

                dataGridView.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridView.Rows.Clear();
                FillGridData(dataGridView, "", "", "", "FED", "");
                dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);

            }
        }


        /// <summary>
        /// Handles the ModulesControl tabs SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnModulesTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null)
                return;
            string code = tabControl1.SelectedTab.Tag as string;
            boolChangeStatus = false;
            if (ToolBarNew != null)
                ToolBarNew.Visible = false;
            if (code.Equals("FED"))
            {
                FillFedGrid();
                if (ToolBarNew != null)
                    ToolBarNew.Visible = true;

                ToolBarExcel.ToolTipText = "FED OMB Excel";
            }
            else if (code.Equals("HUD"))
            {
                cmbCounty_SelectedIndexChanged(cmbCounty, new EventArgs());

                ToolBarExcel.ToolTipText = "HUD Excel";
            }
            else if (code.Equals("CMI"))
            {
                cmbCMICounty_SelectedIndexChanged(cmbCMICounty, new EventArgs());

                ToolBarExcel.ToolTipText = "CMI Excel";
            }
            else if (code.Equals("SMI"))
            {
                FillSmiGrid();

                ToolBarExcel.ToolTipText = "SMI Excel";
            }
        }

        public void fillcombo()
        {
            DataSet ds = null;
            DataTable dt = null;

            cmbCounty.Items.Clear();
            ds = Captain.DatabaseLayer.ZipCodePlusAgency.GetCounty();
            dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cmbCounty.Items.Add(new ListItem(dr["Agy_7"].ToString(), dr["Agy_3"].ToString()));
            }
            if (dt.Rows.Count != 0)
            {
                this.cmbCounty.SelectedIndexChanged -= new System.EventHandler(this.cmbCounty_SelectedIndexChanged);
                cmbCounty.SelectedIndex = 0;
                this.cmbCounty.SelectedIndexChanged += new System.EventHandler(this.cmbCounty_SelectedIndexChanged);
            }
            cmbCMICounty.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cmbCMICounty.Items.Add(new ListItem(dr["Agy_7"].ToString(), dr["Agy_3"].ToString()));
            }
            if (dt.Rows.Count != 0)
            {
                this.cmbCMICounty.SelectedIndexChanged -= new System.EventHandler(this.cmbCMICounty_SelectedIndexChanged);
                cmbCMICounty.SelectedIndex = 0;
                this.cmbCMICounty.SelectedIndexChanged += new System.EventHandler(this.cmbCMICounty_SelectedIndexChanged);
            }
        }

        private void MasterPoverityGuidelineControl_Load(object sender, EventArgs e)
        {
            //IntializeTabs();
        }

        private void cmbCounty_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataGridView dataGridView = null;
            DataGridView dataGridSubView = null;
            string strcmbCounty = "";
            if (cmbCounty.Items.Count > 0)
                strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
            else
                AlertBox.Show("Counties are not defined in agytable 00525", MessageBoxIcon.Warning);

            dataGridView = gvwHud;
            if (dataGridView != null)
            {
                dataGridView.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                dataGridView.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridView.Rows.Clear();
                FillGridData(dataGridView, "", "", "", "HUD", strcmbCounty);
                dataGridView.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridSubView = (DataGridView)pnlHUDSubGrid.Controls["HudSubGrid"];
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick -= new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
                dataGridSubView.Rows.Clear();
                FillGridSubData(dataGridView, dataGridSubView, "HUD", strcmbCounty);
                dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick += new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged += new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);

            }

        }

        private void cmbCMICounty_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = null;
            DataGridView dataGridSubView = null;
            string strcmbCmiCounty = "";



            if (cmbCMICounty.Items.Count > 0)
                strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
            else
                AlertBox.Show("Counties are not defined in agytable 00525", MessageBoxIcon.Warning);
            dataGridView = gvwCMI;

            if (dataGridView != null)
            {
                dataGridView.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                dataGridView.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridView.Rows.Clear();
                FillGridData(dataGridView, "", "", "", "CMI", strcmbCmiCounty);
                dataGridView.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick -= new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);
                dataGridSubView.Rows.Clear();
                FillGridSubData(dataGridView, dataGridSubView, "CMI", strcmbCmiCounty);
                dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                dataGridSubView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridSubView_CellValidating);
                dataGridSubView.CellClick += new DataGridViewCellEventHandler(dataGridSubView_CellClick);
                dataGridSubView.CellValueChanged += new DataGridViewCellEventHandler(dataGridSubView_CellValueChanged);



            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                DataGridView dataGridView = null;
                string code = tabControl1.SelectedTab.Tag as string;
                if (code.Equals("FED"))
                {
                    dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                }
                else if (code.Equals("HUD"))
                {
                    dataGridView = gvwHud;
                }
                else if (code.Equals("CMI"))
                {
                    dataGridView = gvwCMI;
                }
                else if (code.Equals("SMI"))
                {
                    dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                }

                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    hierarchy += row.Agency + row.Dept + row.Prog;
                    strPublicCode = row.Code;
                    if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
                    {
                        //dataGridView.SelectedRows[0].Cells["Hierarchy1"].Value = strPublicCode;

                        //if(dataGridView.Rows.Count-1 != dataGridView.SelectedRows[0].Index +1 ) 
                        //      strOldPublicCode = Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value.ToString()) == null ? "" : Convert.ToString(dataGridView.SelectedRows[0].Cells["Hierarchy"].Value.ToString());

                        dataGridView.SelectedRows[0].Cells["Hierarchy"].Value = strPublicCode;
                    }
                }
                MasterPovertyaddupdate();

            }
        }

        public void RefreshGrid()
        {
            DataGridView dataGridView = null;
            DataGridView dataGridSubView = null;
            string strcmbCounty = "";
            string strType = "";

            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];

            }

            if (tabControl1.SelectedIndex == 1)
            {
                if (cmbCounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                dataGridView = gvwHud;
                dataGridSubView = (DataGridView)pnlHUDSubGrid.Controls["HudSubGrid"];
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (cmbCMICounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
                dataGridView = gvwCMI;
                dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];

            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
            }
            if (dataGridView != null)
            {
                if (dataGridView != null)
                {
                    if (tabControl1.SelectedIndex == 0)
                    {
                        dataGridView.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                        dataGridView.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        dataGridView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        dataGridView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        dataGridView.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        dataGridView.CellClick -= new DataGridViewCellEventHandler(dataGridView_CellClick);
                        dataGridView.ColumnHeaderMouseClick -= new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);
                        dataGridView.Rows.Clear();
                        FillGridData(dataGridView, "", "", "", strType, strcmbCounty);
                        dataGridView.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                        dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
                        dataGridView.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);


                    }
                    else
                    {

                        dataGridView.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                        dataGridView.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        dataGridView.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        dataGridView.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        dataGridView.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        dataGridView.CellClick -= new DataGridViewCellEventHandler(dataGridView_CellClick);
                        dataGridView.ColumnHeaderMouseClick -= new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);
                        dataGridView.Rows.Clear();
                        FillGridData(dataGridView, "", "", "", strType, strcmbCounty);
                        dataGridView.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                        dataGridView.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                        dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                        dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                        dataGridView.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                        dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
                        dataGridSubView.Rows.Clear();
                        dataGridSubView.RowLeave -= new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                        FillGridSubData(dataGridView, dataGridSubView, strType, strcmbCounty);
                        dataGridSubView.RowLeave += new DataGridViewCellEventHandler(dataGridSubView_RowLeave);
                        dataGridView.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView_ColumnHeaderMouseClick);
                    }
                }
            }

        }

        void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DisplaySortOrder(e.ColumnIndex);
        }

        private void MasterPoverty_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (sender is DataGridView)
            {
                DataGridView dataGridView = sender as DataGridView;
                if (!dataGridView.Rows[e.RowIndex].Cells["Mode"].Value.Equals("U"))
                {
                    dataGridView.Rows[e.RowIndex].Cells["Hierarchy1"].Value = "...";//"Hierarchy";
                    // dataGridView.Rows[e.RowIndex].Cells["Hierarchy"].ReadOnly = false;
                    dataGridView.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                    dataGridView.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                    dataGridView.Rows[e.RowIndex].Cells[0].Selected = true;
                }
            }
        }

        public void MainGrid(out string strType, out string strcmbCounty, out DataGridView dataGridView, out DataGridView dataGridSubView)
        {
            string strTypeNew = string.Empty;
            string strcmbCountyNew = string.Empty;
            DataGridView datagridNew = null;
            DataGridView dataGridSubNew = null;

            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                strTypeNew = "FED";
            }
            else if (tabControl1.SelectedIndex == 1)
            {

                strTypeNew = "HUD";
                if (cmbCounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                dataGridView = gvwHud;
                dataGridSubView = (DataGridView)pnlHUDSubGrid.Controls["HudSubGrid"];
            }
            else if (tabControl1.SelectedIndex == 2)
            {

                string strcmbCmiCounty = "";
                strTypeNew = "CMI";
                if (cmbCMICounty.Items.Count > 0)
                    strcmbCmiCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
                dataGridView = gvwCMI;
                dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];


            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridView = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                strTypeNew = "SMI";
                dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
            }
            strType = strTypeNew;
            strcmbCounty = strcmbCountyNew;
            dataGridView = datagridNew;
            dataGridSubView = dataGridSubNew;
        }

        string strAsending = string.Empty;
        string strEndAsending = string.Empty;

        private void DisplaySortOrder(int ColumnIndex)
        {

            DataGridView dataGridview = null;

            string strcmbCounty = "";
            string strType = "";

            if (tabControl1.SelectedIndex == 0)
            {
                dataGridview = (DataGridView)pnlFebOmb.Controls["FedGrid"];
                strType = "FED";
            }

            if (tabControl1.SelectedIndex == 1)
            {
                if (cmbCounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCounty.SelectedItem).Value.ToString();
                dataGridview = gvwHud;
                strType = "HUD";
                // dataGridSubView = (DataGridView)splitContainer1.Panel2.Controls["HudSubGrid"];
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (cmbCMICounty.Items.Count > 0)
                    strcmbCounty = ((ListItem)cmbCMICounty.SelectedItem).Value.ToString();
                dataGridview = gvwCMI;
                strType = "CMI";
                // dataGridSubView = (DataGridView)pnlCMISubGrid.Controls["CMISubGrid"];

            }
            else if (tabControl1.SelectedIndex == 3)
            {
                dataGridview = (DataGridView)pnlSMIGrid.Controls["SMIGrid"];
                strType = "SMI";
                //  dataGridSubView = (DataGridView)pnlSMISubGrid.Controls["SMISubGrid"];
            }



            if (ColumnIndex == dataGridview.Columns["GDL_START_DATE"].Index)
            {
                dataGridview.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                dataGridview.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridview.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridview.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridview.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridview.CellClick -= new DataGridViewCellEventHandler(dataGridView_CellClick);
                dataGridview.Rows.Clear();

                if (strAsending == string.Empty)
                {

                    var sortedList = from staffmember in _masterPovertyEntiry
                                     orderby Convert.ToDateTime(DateTime.ParseExact(staffmember.GdlStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)) ascending
                                     select staffmember;

                    switch (strType)
                    {
                        case "FED":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), code, "..."/*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                // dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                        case "HUD":
                        case "CMI":
                        case "SMI":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), hierarchy.Gdl3Value.ToString(), hierarchy.Gdl4Value.ToString(), hierarchy.Gdl5Value.ToString(), hierarchy.Gdl6Value.ToString(), code, "..."/*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                // dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                    }

                    //foreach (MasterPovertyEntity hierarchy in sortedList)
                    //{
                    //    string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                    //    int rowIndex = dataGridView.Rows.Add(hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString());

                    //}
                    strAsending = "descending";
                }
                else
                {


                    var sortedList = from staffmember in _masterPovertyEntiry
                                     orderby Convert.ToDateTime(DateTime.ParseExact(staffmember.GdlStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)) descending
                                     select staffmember;

                    switch (strType)
                    {
                        case "FED":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), code, "..." /*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                //  dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                        case "HUD":
                        case "CMI":
                        case "SMI":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), hierarchy.Gdl3Value.ToString(), hierarchy.Gdl4Value.ToString(), hierarchy.Gdl5Value.ToString(), hierarchy.Gdl6Value.ToString(), code, "..."/*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                // dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                    }



                    strAsending = string.Empty;
                }

                dataGridview.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                dataGridview.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridview.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridview.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridview.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridview.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);

            }
            else if (ColumnIndex == dataGridview.Columns["GDL_END_DATE"].Index)
            {
                dataGridview.SelectionChanged -= new EventHandler(dataGridView_SelectionChanged);
                dataGridview.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridview.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridview.CellValidating -= new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridview.CellValidated -= new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridview.CellClick -= new DataGridViewCellEventHandler(dataGridView_CellClick);
                dataGridview.Rows.Clear();

                if (strEndAsending == string.Empty)
                {



                    var sortedList = from staffmember in _masterPovertyEntiry
                                     orderby Convert.ToDateTime(DateTime.ParseExact(staffmember.GdlEndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)) ascending
                                     select staffmember;

                    switch (strType)
                    {
                        case "FED":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), code, "..."/*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                // dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                        case "HUD":
                        case "CMI":
                        case "SMI":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), hierarchy.Gdl3Value.ToString(), hierarchy.Gdl4Value.ToString(), hierarchy.Gdl5Value.ToString(), hierarchy.Gdl6Value.ToString(), code, "..."/*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                // dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                    }

                    strEndAsending = "descending";
                }
                else
                {


                    var sortedList = from staffmember in _masterPovertyEntiry
                                     orderby Convert.ToDateTime(DateTime.ParseExact(staffmember.GdlEndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)) descending
                                     select staffmember;

                    switch (strType)
                    {
                        case "FED":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), code, "..."/*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                // dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                        case "HUD":
                        case "CMI":
                        case "SMI":
                            foreach (MasterPovertyEntity hierarchy in sortedList)
                            {
                                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                                int rowIndex = dataGridview.Rows.Add("U", hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, hierarchy.Gdl1Value.ToString(), hierarchy.Gdl2Value.ToString(), hierarchy.Gdl3Value.ToString(), hierarchy.Gdl4Value.ToString(), hierarchy.Gdl5Value.ToString(), hierarchy.Gdl6Value.ToString(), code, "..." /*"Hierarchy"*/);
                                dataGridview.Rows[rowIndex].Cells["Hierarchy1"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Tag = hierarchy;
                                dataGridview.Rows[rowIndex].Cells["Hierarchy"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_START_DATE"].ReadOnly = true;
                                dataGridview.Rows[rowIndex].Cells["GDL_END_DATE"].ReadOnly = true;
                                // dataGridview.ItemsPerPage = 100;
                                CommonFunctions.setTooltip(rowIndex, hierarchy.GdlAddOperator, hierarchy.GdlDateAdd, hierarchy.GdlLstcOperator, hierarchy.GdlDateLstc, dataGridview);
                                if (Privileges.ChangePriv.Equals("false"))
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = true;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = true;
                                }
                                else
                                {
                                    dataGridview.Rows[rowIndex].Cells["GDL_1_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_2_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_3_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_4_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_5_VALUE"].ReadOnly = false;
                                    dataGridview.Rows[rowIndex].Cells["GDL_6_VALUE"].ReadOnly = false;
                                }
                            }
                            if (dataGridview.Rows.Count > 0)
                                dataGridview.Rows[0].Selected = true;
                            break;
                    }

                    strEndAsending = string.Empty;
                }

                dataGridview.SelectionChanged += new EventHandler(dataGridView_SelectionChanged);
                dataGridview.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                dataGridview.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                dataGridview.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
                dataGridview.CellValidated += new DataGridViewCellEventHandler(dataGridView_CellValidated);
                dataGridview.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
            }



        }

        private void pbFedHelp_Click(object sender, EventArgs e)
        {

            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void pbHUDHelp_Click(object sender, EventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 2, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void pbCMIHelp_Click(object sender, EventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 3, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void pbSMIhelp_Click(object sender, EventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 4, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void btnCopyFrom_Click(object sender, EventArgs e)
        {
            //MasterPovertyCopyFrom copyFrom = new MasterPovertyCopyFrom(BaseForm, Privileges, ((Captain.Common.Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString().Trim(), "HUD");
            //copyFrom.FormClosed += new Wisej.Web.FormClosedEventHandler(county_FormClosed);
            //copyFrom.StartPosition = FormStartPosition.CenterScreen;
            //copyFrom.ShowDialog();
        }

        private void county_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MasterPovertyCopyFrom form = sender as MasterPovertyCopyFrom;

            //if (form.DialogResult == DialogResult.OK)
            //{
            //    RefreshGrid1();
            //}
            //else
            //    strIndex = 0;
        }
        string Random_Filename = null;
        private void btnCMICopyFrom_Click(object sender, EventArgs e)
        {
            //MasterPovertyCopyFrom copyFrom = new MasterPovertyCopyFrom(BaseForm, Privileges, ((Captain.Common.Utilities.ListItem)cmbCMICounty.SelectedItem).Value.ToString().Trim(), "CMI");
            //copyFrom.FormClosed += new Wisej.Web.FormClosedEventHandler(county_FormClosed);
            //copyFrom.StartPosition = FormStartPosition.CenterScreen;
            //copyFrom.ShowDialog();
        }

        private void Genrate_FED_OMB_Excel()
        {
            #region PDF Name

            string PdfName = "Fed OMB" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

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
                string Tmpstr = PdfName + ".xlsx";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xlsx";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xlsx";

            #endregion

            #region Excel Generation

            int _RowIndex = 0, _ColumnIndex = 0;
            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets[0];
                    worksheet.Name = "Fed OMB";
                    worksheet.ActiveView.ShowGridlines = false;

                    _RowIndex = 0;
                    _ColumnIndex = 0;
                    wb.Unit = DevExpress.Office.DocumentUnit.Point;

                    worksheet.Columns[0].Width = 10;
                    worksheet.Columns[1].Width = 70;
                    worksheet.Columns[2].Width = 70;
                    worksheet.Columns[3].Width = 100;
                    worksheet.Columns[4].Width = 70;
                    worksheet.Columns[5].Width = 70;
                    worksheet.Columns[6].Width = 10;

                    _RowIndex = 0;
                    _ColumnIndex = 0;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Fed OMB";
                    oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                    _ColumnIndex = _ColumnIndex + 5;


                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;

                    _RowIndex++;
                    _ColumnIndex = 0;
                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Start Date";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "End Date";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Poverty Base";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Increment";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Hierarchy";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;

                    List<MasterPovertyEntity> FEDEntity = _model.masterPovertyData.GetCaseGdladpt("", "", "", "FED", "");

                    #region Data

                    foreach (MasterPovertyEntity entity in FEDEntity)
                    {
                        _RowIndex++;
                        _ColumnIndex = 0;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = entity.GdlStartDate;
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = entity.GdlEndDate;
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;

                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = entity.Gdl1Value;
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = entity.Gdl2Value;
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = entity.GdlAgency + "-" + entity.GdlDept + "-" + entity.GdlProgram;
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleR;
                        _ColumnIndex++;

                    }
                    _RowIndex++;
                    _ColumnIndex = 0;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;
                    _ColumnIndex++;

                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "©2024 CAPSystems Inc";
                    oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 5, oDevExpress_Excel_Properties.gxlFrameFooterStyle);
                    _ColumnIndex = _ColumnIndex + 5;


                    worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                    worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                    worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;

                    worksheet.IgnoredErrors.Add(worksheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                    wb.SaveDocument(PdfName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);


                    try
                    {
                        string localFilePath = PdfName;
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


                    #endregion
                }
            }
            catch (Exception ex)
            {

            }

            #endregion
        }
        private void Genrate_HUD_Excel()
        {
            #region PDF Name

            string PdfName = "HUD" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

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
                string Tmpstr = PdfName + ".xlsx";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xlsx";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xlsx";

            #endregion

            #region Excel Generation

            int _RowIndex = 0, _ColumnIndex = 0;
            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    DataSet ds = DatabaseLayer.MasterPoverty.GetCASEGDL();
                    DataTable dt = ds.Tables[0];

                    DataView dv = new DataView(dt);
                    dv.RowFilter = "GDL_TYPE = 'HUD'";
                    dt = dv.ToTable();

                    DataTable dtTop = new DataTable();
                    DataTable dtbottom = new DataTable();

                    DataView topData = new DataView(dt);
                    topData.RowFilter = "GDL_NO_HOUSEHOLDS = '0'  AND GDL_COUNTY = '" + ((Captain.Common.Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString().Trim() + "' AND GDL_START_DATE = '" + gvwHud.CurrentRow.Cells["GDL_START_DATE"].Value.ToString() + "' AND GDL_END_DATE = '" + gvwHud.CurrentRow.Cells["GDL_END_DATE"].Value.ToString() + "'";
                    dtTop = topData.ToTable();
                    int i = 0;

                    foreach (DataRow row in dtTop.Rows)
                    {
                        //i++;
                        DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets[0];
                        //if (i == 1)
                        //{
                        //    worksheet = wb.Worksheets[0];
                        //    worksheet.Name = "HUD_" + i;
                        //}
                        //else
                        //    worksheet = wb.Worksheets.Add("HUD_" + i);

                        worksheet.Name = "HUD";

                        worksheet.ActiveView.ShowGridlines = false;

                        _RowIndex = 0;
                        _ColumnIndex = 0;
                        wb.Unit = DevExpress.Office.DocumentUnit.Point;

                        worksheet.Columns[0].Width = 10;
                        worksheet.Columns[1].Width = 70;
                        worksheet.Columns[2].Width = 70;
                        worksheet.Columns[3].Width = 70;
                        worksheet.Columns[4].Width = 70;
                        worksheet.Columns[5].Width = 70;
                        worksheet.Columns[6].Width = 70;
                        worksheet.Columns[7].Width = 70;
                        worksheet.Columns[8].Width = 70;
                        worksheet.Columns[9].Width = 70;
                        worksheet.Columns[10].Width = 10;

                        //if (gvwHud.CurrentRow.Cells["GDL_START_DATE"].Value.ToString() == row["GDL_START_DATE"].ToString() && gvwHud.CurrentRow.Cells["GDL_END_DATE"].Value.ToString() == row["GDL_END_DATE"].ToString())
                        //{
                            _RowIndex = 0;
                            _ColumnIndex = 0;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "HUD";
                            oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 9, oDevExpress_Excel_Properties.gxlFrameStyleL);
                            _ColumnIndex = _ColumnIndex + 9;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;

                            _RowIndex++;
                            _ColumnIndex = 0;
                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Start Date";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "End Date";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "1st%";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "2nd%";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "3rd%";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "4th%";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "5th%";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "6th%";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Hierarchy";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;

                            _RowIndex++;
                            _ColumnIndex = 0;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_START_DATE"].ToString() == "" ? "" : Convert.ToDateTime(row["GDL_START_DATE"].ToString()).ToString("MM/dd/yyyy");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_END_DATE"].ToString() == "" ? "" : Convert.ToDateTime(row["GDL_END_DATE"].ToString()).ToString("MM/dd/yyyy");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_1_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_2_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_3_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_4_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_5_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_6_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_AGENCY"].ToString() + "-" + row["GDL_DEPT"].ToString() + "-" + row["GDL_PROGRAM"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleR;

                            _RowIndex++;
                            _ColumnIndex = 0;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 9, oDevExpress_Excel_Properties.gxlFrameFooterStyle);
                            _ColumnIndex = _ColumnIndex + 9;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;

                            _RowIndex = _RowIndex + 3;
                            _ColumnIndex = 0;

                            //"SNP_APP = '" + Entity.App.Trim() + "' AND INCOME_FAMILY_SEQ = '" + Entity.FamilySeq + "'";

                            DataView bottomData = new DataView(dt);
                            bottomData.RowFilter = "GDL_NO_HOUSEHOLDS <> '0' AND GDL_AGENCY = '" + row["GDL_AGENCY"].ToString() + "' AND GDL_DEPT = '" + row["GDL_DEPT"].ToString() + "' AND GDL_PROGRAM = '" + row["GDL_PROGRAM"].ToString() + "' AND GDL_TYPE = 'HUD' AND GDL_COUNTY = '" + ((Captain.Common.Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString().Trim() + "' AND GDL_START_DATE = '" + row["GDL_START_DATE"].ToString() + "' AND GDL_END_DATE = '" + row["GDL_END_DATE"].ToString() + "'";
                            dtbottom = bottomData.ToTable();

                            #region 2nd Grid Headers

                            _ColumnIndex = 1;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Fam Size";
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_1_VALUE"].ToString() == "" ? "" : (row["GDL_1_VALUE"].ToString() + "%");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_2_VALUE"].ToString() == "" ? "" : (row["GDL_2_VALUE"].ToString() + "%");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_3_VALUE"].ToString() == "" ? "" : (row["GDL_3_VALUE"].ToString() + "%");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_4_VALUE"].ToString() == "" ? "" : (row["GDL_4_VALUE"].ToString() + "%");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_5_VALUE"].ToString() == "" ? "" : (row["GDL_5_VALUE"].ToString() + "%");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_6_VALUE"].ToString() == "" ? "" : (row["GDL_6_VALUE"].ToString() + "%");
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;

                            #endregion

                            foreach (DataRow bottomrow in dtbottom.Rows)
                            {
                                _RowIndex++;
                                _ColumnIndex = 1;

                                worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_NO_HOUSEHOLDS"].ToString();
                                worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                _ColumnIndex++;

                                worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_1_VALUE"].ToString();
                                worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                _ColumnIndex++;

                                worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_2_VALUE"].ToString();
                                worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                _ColumnIndex++;

                                worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_3_VALUE"].ToString();
                                worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                _ColumnIndex++;

                                worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_4_VALUE"].ToString();
                                worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                _ColumnIndex++;

                                worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_5_VALUE"].ToString();
                                worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                _ColumnIndex++;

                                worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_6_VALUE"].ToString();
                                worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                _ColumnIndex++;
                            }
                        //}
                        worksheet.IgnoredErrors.Add(worksheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);
                    }

                    wb.Worksheets.ActiveWorksheet = wb.Worksheets[0];
                    wb.SaveDocument(PdfName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);


                    try
                    {
                        string localFilePath = PdfName;
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


            #endregion


        }
        private void Genrate_CMI_Excel()
        {
            #region PDF Name

            string PdfName = "CMI" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

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
                string Tmpstr = PdfName + ".xlsx";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xlsx";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xlsx";

            #endregion

            #region Excel Generation

            int _RowIndex = 0, _ColumnIndex = 0;
            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    DataSet ds = DatabaseLayer.MasterPoverty.GetCASEGDL();
                    DataTable dt = ds.Tables[0];

                    DataView dv = new DataView(dt);
                    dv.RowFilter = "GDL_TYPE = 'CMI'";
                    dt = dv.ToTable();

                    DataTable dtTop = new DataTable();
                    DataTable dtbottom = new DataTable();

                    DataView topData = new DataView(dt);
                    topData.RowFilter = "GDL_NO_HOUSEHOLDS = '0' AND GDL_COUNTY = '" + ((Captain.Common.Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString().Trim() + "' AND GDL_START_DATE = '" + gvwCMI.CurrentRow.Cells["GDL_START_DATE"].Value.ToString() + "' AND GDL_END_DATE = '" + gvwCMI.CurrentRow.Cells["GDL_END_DATE"].Value.ToString() + "'";

                    dtTop = topData.ToTable();
                    int i = 0;
                    foreach (DataRow row in dtTop.Rows)
                    {
                        //i++;
                        DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets[0];
                        //if (i == 1)
                        //{
                        //    worksheet = wb.Worksheets[0];
                        //    worksheet.Name = "CMI_" + i;
                        //}
                        //else
                        //    worksheet = wb.Worksheets.Add("CMI_" + i);

                        worksheet.Name = "CMI";

                        worksheet.ActiveView.ShowGridlines = false;

                        _RowIndex = 0;
                        _ColumnIndex = 0;
                        wb.Unit = DevExpress.Office.DocumentUnit.Point;

                        worksheet.Columns[0].Width = 10;
                        worksheet.Columns[1].Width = 70;
                        worksheet.Columns[2].Width = 70;
                        worksheet.Columns[3].Width = 70;
                        worksheet.Columns[4].Width = 70;
                        worksheet.Columns[5].Width = 70;
                        worksheet.Columns[6].Width = 70;
                        worksheet.Columns[7].Width = 70;
                        worksheet.Columns[8].Width = 70;
                        worksheet.Columns[9].Width = 70;
                        worksheet.Columns[10].Width = 10;

                        _RowIndex = 0;
                        _ColumnIndex = 0;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "CMI";
                        oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 9, oDevExpress_Excel_Properties.gxlFrameStyleL);
                        _ColumnIndex = _ColumnIndex + 9;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;

                        _RowIndex++;
                        _ColumnIndex = 0;
                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Start Date";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "End Date";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "1st%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "2nd%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "3rd%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "4th%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "5th%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "6th%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Hierarchy";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;

                        _RowIndex++;
                        _ColumnIndex = 0;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_START_DATE"].ToString() == "" ? "" : Convert.ToDateTime(row["GDL_START_DATE"].ToString()).ToString("MM/dd/yyyy");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_END_DATE"].ToString() == "" ? "" : Convert.ToDateTime(row["GDL_END_DATE"].ToString()).ToString("MM/dd/yyyy");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_1_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_2_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_3_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_4_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_5_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_6_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_AGENCY"].ToString() + "-" + row["GDL_DEPT"].ToString() + "-" + row["GDL_PROGRAM"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleR;

                        _RowIndex++;
                        _ColumnIndex = 0;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 9, oDevExpress_Excel_Properties.gxlFrameFooterStyle);
                        _ColumnIndex = _ColumnIndex + 9;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;

                        _RowIndex = _RowIndex + 3;
                        _ColumnIndex = 0;

                        //"SNP_APP = '" + Entity.App.Trim() + "' AND INCOME_FAMILY_SEQ = '" + Entity.FamilySeq + "'";

                        DataView bottomData = new DataView(dt);
                        bottomData.RowFilter = "GDL_NO_HOUSEHOLDS <> '0' AND GDL_AGENCY = '" + row["GDL_AGENCY"].ToString() + "' AND GDL_DEPT = '" + row["GDL_DEPT"].ToString() + "' AND GDL_PROGRAM = '" + row["GDL_PROGRAM"].ToString() + "' AND GDL_TYPE = 'CMI' AND GDL_COUNTY = '" + ((Captain.Common.Utilities.ListItem)cmbCounty.SelectedItem).Value.ToString().Trim() + "' AND GDL_START_DATE = '" + row["GDL_START_DATE"].ToString() + "' AND GDL_END_DATE = '" + row["GDL_END_DATE"].ToString() + "'";
                        dtbottom = bottomData.ToTable();

                        #region 2nd Grid Headers

                        _ColumnIndex = 1;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Fam Size";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_1_VALUE"].ToString() == "" ? "" : (row["GDL_1_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_2_VALUE"].ToString() == "" ? "" : (row["GDL_2_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_3_VALUE"].ToString() == "" ? "" : (row["GDL_3_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_4_VALUE"].ToString() == "" ? "" : (row["GDL_4_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_5_VALUE"].ToString() == "" ? "" : (row["GDL_5_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_6_VALUE"].ToString() == "" ? "" : (row["GDL_6_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;

                        #endregion

                        foreach (DataRow bottomrow in dtbottom.Rows)
                        {
                            _RowIndex++;
                            _ColumnIndex = 1;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_NO_HOUSEHOLDS"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_1_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_2_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_3_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_4_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_5_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_6_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;
                        }

                        worksheet.IgnoredErrors.Add(worksheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);
                    }

                    #region Data


                    wb.Worksheets.ActiveWorksheet = wb.Worksheets[0];
                    wb.SaveDocument(PdfName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);


                    try
                    {
                        string localFilePath = PdfName;
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


                    #endregion
                }
            }
            catch (Exception ex)
            {

            }


            #endregion
        }
        private void Genrate_SMI_Excel()
        {
            #region PDF Name

            string PdfName = "SMI" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

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
                string Tmpstr = PdfName + ".xlsx";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xlsx";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xlsx";

            #endregion

            #region Excel Generation

            int _RowIndex = 0, _ColumnIndex = 0;
            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    DataSet ds = DatabaseLayer.MasterPoverty.GetCASEGDL();
                    DataTable dt = ds.Tables[0];

                    DataView dv = new DataView(dt);
                    dv.RowFilter = "GDL_TYPE = 'SMI'";
                    dt = dv.ToTable();

                    DataTable dtTop = new DataTable();
                    DataTable dtbottom = new DataTable();

                    DataView topData = new DataView(dt);
                    topData.RowFilter = "GDL_NO_HOUSEHOLDS = '0'AND GDL_START_DATE = '" + ((DataGridView)pnlSMIGrid.Controls["SMIGrid"]).CurrentRow.Cells["GDL_START_DATE"].Value.ToString() + "' AND GDL_END_DATE = '" + ((DataGridView)pnlSMIGrid.Controls["SMIGrid"]).CurrentRow.Cells["GDL_END_DATE"].Value.ToString() + "'";

                    dtTop = topData.ToTable();
                    int i = 0;
                    foreach (DataRow row in dtTop.Rows)
                    {
                        i++;
                        DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets[0];
                        //if (i == 1)
                        //{
                        //    worksheet = wb.Worksheets[0];
                        //    worksheet.Name = "SMI_" + i;
                        //}
                        //else
                        //    worksheet = wb.Worksheets.Add("SMI_" + i);
                        worksheet.Name = "SMI";
                        worksheet.ActiveView.ShowGridlines = false;

                        _RowIndex = 0;
                        _ColumnIndex = 0;
                        wb.Unit = DevExpress.Office.DocumentUnit.Point;

                        worksheet.Columns[0].Width = 10;
                        worksheet.Columns[1].Width = 70;
                        worksheet.Columns[2].Width = 70;
                        worksheet.Columns[3].Width = 70;
                        worksheet.Columns[4].Width = 70;
                        worksheet.Columns[5].Width = 70;
                        worksheet.Columns[6].Width = 70;
                        worksheet.Columns[7].Width = 70;
                        worksheet.Columns[8].Width = 70;
                        worksheet.Columns[9].Width = 70;
                        worksheet.Columns[10].Width = 10;

                        _RowIndex = 0;
                        _ColumnIndex = 0;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "SMI";
                        oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 9, oDevExpress_Excel_Properties.gxlFrameStyleL);
                        _ColumnIndex = _ColumnIndex + 9;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 30;

                        _RowIndex++;
                        _ColumnIndex = 0;
                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Start Date";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "End Date";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "1st%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "2nd%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "3rd%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "4th%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "5th%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "6th%";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Hierarchy";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;

                        _RowIndex++;
                        _ColumnIndex = 0;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_START_DATE"].ToString() == "" ? "" : Convert.ToDateTime(row["GDL_START_DATE"].ToString()).ToString("MM/dd/yyyy");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_END_DATE"].ToString() == "" ? "" : Convert.ToDateTime(row["GDL_END_DATE"].ToString()).ToString("MM/dd/yyyy");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_1_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_2_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_3_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_4_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_5_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_6_VALUE"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_AGENCY"].ToString() + "-" + row["GDL_DEPT"].ToString() + "-" + row["GDL_PROGRAM"].ToString();
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleR;

                        _RowIndex++;
                        _ColumnIndex = 0;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        oDevExpress_Excel_Properties.xlRowsMerge(worksheet, _RowIndex, _ColumnIndex, 9, oDevExpress_Excel_Properties.gxlFrameFooterStyle);
                        _ColumnIndex = _ColumnIndex + 9;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 10;

                        _RowIndex = _RowIndex + 3;
                        _ColumnIndex = 0;

                        //"SNP_APP = '" + Entity.App.Trim() + "' AND INCOME_FAMILY_SEQ = '" + Entity.FamilySeq + "'";

                        DataView bottomData = new DataView(dt);
                        bottomData.RowFilter = "GDL_NO_HOUSEHOLDS <> '0' AND GDL_AGENCY = '" + row["GDL_AGENCY"].ToString() + "' AND GDL_DEPT = '" + row["GDL_DEPT"].ToString() + "' AND GDL_PROGRAM = '" + row["GDL_PROGRAM"].ToString() + "' AND GDL_TYPE = 'SMI' AND GDL_START_DATE = '" + row["GDL_START_DATE"].ToString() + "' AND GDL_END_DATE = '" + row["GDL_END_DATE"].ToString() + "'";
                        dtbottom = bottomData.ToTable();

                        #region 2nd Grid Headers

                        _ColumnIndex = 1;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = "Fam Size";
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_1_VALUE"].ToString() == "" ? "" : (row["GDL_1_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_2_VALUE"].ToString() == "" ? "" : (row["GDL_2_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_3_VALUE"].ToString() == "" ? "" : (row["GDL_3_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_4_VALUE"].ToString() == "" ? "" : (row["GDL_4_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_5_VALUE"].ToString() == "" ? "" : (row["GDL_5_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;
                        _ColumnIndex++;

                        worksheet.Rows[_RowIndex][_ColumnIndex].Value = row["GDL_6_VALUE"].ToString() == "" ? "" : (row["GDL_6_VALUE"].ToString() + "%");
                        worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        worksheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 25;

                        #endregion

                        foreach (DataRow bottomrow in dtbottom.Rows)
                        {
                            _RowIndex++;
                            _ColumnIndex = 1;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_NO_HOUSEHOLDS"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_1_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_2_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_3_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_4_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_5_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;

                            worksheet.Rows[_RowIndex][_ColumnIndex].Value = bottomrow["GDL_6_VALUE"].ToString();
                            worksheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlNCC;
                            _ColumnIndex++;
                        }

                        worksheet.IgnoredErrors.Add(worksheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);
                    }



                    #region Data


                    wb.Worksheets.ActiveWorksheet = wb.Worksheets[0];
                    wb.SaveDocument(PdfName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);


                    try
                    {
                        string localFilePath = PdfName;
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


                    #endregion
                }
            }
            catch (Exception ex)
            {

            }


            #endregion
        }
    }

}