#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Exceptions;
using System.Diagnostics;

using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using Captain.DatabaseLayer;
using Spire.Pdf.Grid;
using DevExpress.XtraRichEdit.Model;
using System.Linq;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class HierarchyDefinitionControl : BaseUserControl
    {
        private CaptainModel _model = null;
        private PrivilegesControl _screenPrivileges = null;

        public HierarchyDefinitionControl(BaseForm baseForm, PrivilegeEntity privileges) : base(baseForm)
        {
            InitializeComponent();
            _model = new CaptainModel();
            Privileges = privileges;
            BaseForm = baseForm;

            propReportPath = _model.lookupDataAccess.GetReportPath();
            SetGridColsOnPrivileges();
            populateGridData(string.Empty);

            gvwAgency.SelectionChanged += new EventHandler(gvwAgency_SelectionChanged);
            gvwDept.SelectionChanged += new EventHandler(gvwDept_SelectionChanged);

            PopulateToolbar(oToolbarMnustrip);
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }

        #endregion

        public void populateGridData(string selectedAgency)
        {
            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("AGENCY", string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency);
            List<HierarchyEntity> hierarchyAgencyRep = _model.lookupDataAccess.GetAgencyRepresentation();
            List<HierarchyEntity> hierarchyClientName = _model.lookupDataAccess.GetClientNameFormat();
            List<HierarchyEntity> hierarchyCaseWorkerName = _model.lookupDataAccess.GetCaseWorkerFormat();

            gvwAgency.SelectionChanged -= new EventHandler(gvwAgency_SelectionChanged);
            gvwAgency.Rows.Clear();
            gvwAgency.Update();
            foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
            {
                string isIntake = hierarchyEntity.Intake.Equals("Y") ? "true" : "false";
                string agencyRep = string.Empty;
                if (!hierarchyEntity.HIERepresentation.Equals(string.Empty))
                    agencyRep = hierarchyAgencyRep.Find(u => u.Code.Equals(hierarchyEntity.HIERepresentation)).ShortName + string.Empty;
                string clientName = string.Empty;
                if (!hierarchyEntity.CNFormat.Equals(string.Empty))
                    clientName = hierarchyClientName.Find(u => u.Code.Equals(hierarchyEntity.CNFormat)).ShortName + string.Empty;
                string caseworkName = string.Empty;
                if (!hierarchyEntity.CWFormat.Equals(string.Empty))
                    caseworkName = hierarchyCaseWorkerName.Find(u => u.Code.Equals(hierarchyEntity.CWFormat)).ShortName + string.Empty;
                int rowIndex = gvwAgency.Rows.Add(hierarchyEntity.Agency, hierarchyEntity.HirarchyName, hierarchyEntity.ShortName, isIntake, agencyRep, clientName, caseworkName);

                if (Privileges.AddPriv.Equals("false"))
                    gvwAgency.Columns[7].Visible = false;

                gvwAgency.Rows[rowIndex].Tag = hierarchyEntity;
                if (selectedAgency.Equals(hierarchyEntity.Agency))
                {
                    gvwAgency.Rows[rowIndex].Selected = true;
                    gvwAgency.CurrentCell = gvwAgency.Rows[rowIndex].Cells[0];
                }
                setTooltip(rowIndex, hierarchyEntity, gvwAgency);
                if (string.IsNullOrEmpty(selectedAgency.Trim()))
                {
                    gvwAgency.CurrentCell = gvwAgency.Rows[0].Cells[1];
                    gvwAgency.Rows[0].Selected = true;
                }
            }
            gvwAgency.SelectionChanged += new EventHandler(gvwAgency_SelectionChanged);
            if (gvwAgency.Rows.Count > 0)
            {
                gvwAgency_SelectionChanged(gvwAgency, new EventArgs());
            }
        }

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
                ToolBarNew.ToolTipText = "Add New Agency";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";//@"icon-new";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);


                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Agency";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";//@"table-row-editing";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Agency";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";//@"icon-close?color=Red";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.ToolTipText = "Hierarchy Report in Excel";
                ToolBarExcel.Enabled = true;
                ToolBarExcel.ImageSource = "captain-excel";// @"icon-excel?color=Green";
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Hierarchy Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help";
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
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

            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = false;
            }
            else
            {
                if (ToolBarDel != null) ToolBarDel.Enabled = true;
            }

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                ToolBarExcel,
                ToolBarHelp
            });
        }

        public int strQuesIndex = 0;
        public int strGroupIndex = 0;
        public string SortColumn = string.Empty;
        public string SortOrder = string.Empty;
        public string strQuesID = string.Empty;
        public void RefreshGrid(string hieType)
        {
            if (hieType.Equals("DEPT"))
            {
                gvwAgency_SelectionChanged(gvwAgency, new EventArgs());
                if (!string.IsNullOrEmpty(SortOrder))
                {
                    gvwDept.SortedColumn.Name = SortColumn;
                    if (SortOrder == "Ascending")
                        this.gvwDept.Sort(this.gvwDept.Columns[SortColumn], ListSortDirection.Ascending);
                    else
                        this.gvwDept.Sort(this.gvwDept.Columns[SortColumn], ListSortDirection.Descending);
                }
            }
            else if (hieType.Equals("PROGRAM"))
            {
                gvwDept_SelectionChanged(gvwDept, new EventArgs());
                if (!string.IsNullOrEmpty(SortOrder))
                {
                    gvwProgram.SortedColumn.Name = SortColumn;
                    if (SortOrder == "Ascending")
                        this.gvwProgram.Sort(this.gvwProgram.Columns[SortColumn], ListSortDirection.Ascending);
                    else
                        this.gvwProgram.Sort(this.gvwProgram.Columns[SortColumn], ListSortDirection.Descending);
                }
            }
            else
            {
                populateGridData(string.Empty);
                if (!string.IsNullOrEmpty(SortOrder))
                {
                    gvwAgency.SortedColumn.Name = SortColumn;
                    if (SortOrder == "Ascending")
                        this.gvwAgency.Sort(this.gvwAgency.Columns[SortColumn], ListSortDirection.Ascending);
                    else
                        this.gvwAgency.Sort(this.gvwAgency.Columns[SortColumn], ListSortDirection.Descending);
                }
            }


        }

        public void RefreshGrid(string hieType, string selectedHIE)
        {
            if (hieType.Equals("DEPT"))
            {
                populateDept(selectedHIE);
                if (!string.IsNullOrEmpty(SortOrder))
                {
                    gvwDept.SortedColumn.Name = SortColumn;
                    if (SortOrder == "Ascending")
                        this.gvwDept.Sort(this.gvwDept.Columns[SortColumn], ListSortDirection.Ascending);
                    else
                        this.gvwDept.Sort(this.gvwDept.Columns[SortColumn], ListSortDirection.Descending);
                }
            }
            else if (hieType.Equals("PROGRAM"))
            {
                populateProgram(selectedHIE);
                if (!string.IsNullOrEmpty(SortOrder))
                {
                    gvwProgram.SortedColumn.Name = SortColumn;
                    if (SortOrder == "Ascending")
                        this.gvwProgram.Sort(this.gvwProgram.Columns[SortColumn], ListSortDirection.Ascending);
                    else
                        this.gvwProgram.Sort(this.gvwProgram.Columns[SortColumn], ListSortDirection.Descending);
                }
            }
            else
            {
                populateGridData(selectedHIE);
                if (!string.IsNullOrEmpty(SortOrder))
                {
                    gvwAgency.SortedColumn.Name = SortColumn;
                    if (SortOrder == "Ascending")
                        this.gvwAgency.Sort(this.gvwAgency.Columns[SortColumn], ListSortDirection.Ascending);
                    else
                        this.gvwAgency.Sort(this.gvwAgency.Columns[SortColumn], ListSortDirection.Descending);
                }
            }
        }

        private void populateDept(string selectedDept)
        {
            if (gvwAgency.SelectedRows.Count > 0)
            {
                HierarchyEntity row = gvwAgency.SelectedRows[0].Tag as HierarchyEntity;
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("DEPT", row.Agency, string.Empty, string.Empty, string.Empty);
                gvwDept.Rows.Clear();
                gvwProgram.Rows.Clear();
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    string isIntake = hierarchyEntity.Intake.Equals("Y") ? "true" : "false";
                    int rowIndex = gvwDept.Rows.Add(hierarchyEntity.Dept, hierarchyEntity.HirarchyName, hierarchyEntity.ShortName, isIntake);
                    gvwDept.Rows[rowIndex].Tag = hierarchyEntity;
                    if (selectedDept.Equals(hierarchyEntity.Dept))
                    {
                        gvwDept.Rows[rowIndex].Selected = true;
                        gvwDept.CurrentCell = gvwDept.Rows[rowIndex].Cells[1];
                    }
                    setTooltip(rowIndex, hierarchyEntity, gvwDept);
                }

                if (string.IsNullOrEmpty(selectedDept.Trim()))
                    gvwDept.Rows[0].Selected = true;
                gvwDept.Update();
                gvwDept.ResumeLayout();

                if (gvwDept.Rows.Count > 0)
                {
                    gvwDept_SelectionChanged(gvwDept, new EventArgs());
                }
            }
        }


        private void populateProgram(string selectedProgram)
        {
            if (gvwDept.SelectedRows.Count > 0)
            {
                if (gvwDept.SelectedRows[0].Tag is HierarchyEntity)
                {
                    HierarchyEntity row = gvwDept.SelectedRows[0].Tag as HierarchyEntity;
                    List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("PROGRAM", row.Agency, row.Dept, string.Empty, string.Empty);
                    gvwProgram.Rows.Clear();
                    foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                    {
                        string isIntake = hierarchyEntity.Intake.Equals("Y") ? "true" : "false";
                        int rowIndex = gvwProgram.Rows.Add(hierarchyEntity.Prog, hierarchyEntity.HirarchyName, hierarchyEntity.ShortName, isIntake);
                        gvwProgram.Rows[rowIndex].Tag = hierarchyEntity;
                        if (selectedProgram.Equals(hierarchyEntity.Prog))
                        {
                            gvwProgram.Rows[rowIndex].Selected = true;
                            gvwProgram.CurrentCell = gvwProgram.Rows[rowIndex].Cells[1];
                        }
                        setTooltip(rowIndex, hierarchyEntity, gvwProgram);
                    }
                    if (string.IsNullOrEmpty(selectedProgram.Trim()))
                        gvwProgram.Rows[0].Selected = true;
                    gvwProgram.Update();
                    gvwProgram.ResumeLayout();
                }
            }
        }

        private void gvwAgency_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwAgency.SelectedRows.Count > 0)
            {
                HierarchyEntity row = gvwAgency.SelectedRows[0].Tag as HierarchyEntity;
                if (row != null)
                {
                    List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("DEPT", row.Agency, string.Empty, string.Empty, string.Empty);
                    gvwDept.Rows.Clear();
                    gvwProgram.Rows.Clear();
                    foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                    {
                        string isIntake = hierarchyEntity.Intake.Equals("Y") ? "true" : "false";
                        int rowIndex = gvwDept.Rows.Add(hierarchyEntity.Dept, hierarchyEntity.HirarchyName, hierarchyEntity.ShortName, isIntake);
                        gvwDept.Rows[rowIndex].Tag = hierarchyEntity;
                        setTooltip(rowIndex, hierarchyEntity, gvwDept);
                    }

                    gvwDept.Update();
                    gvwDept.ResumeLayout();
                    if (gvwDept.Rows.Count > 0)
                    {
                        gvwDept.Rows[0].Selected = true;
                        gvwDept_SelectionChanged(gvwDept, new EventArgs());
                    }
                }
            }
        }

        private void gvwDept_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwDept.SelectedRows.Count > 0)
            {
                if (gvwDept.SelectedRows[0].Tag is HierarchyEntity)
                {
                    HierarchyEntity row = gvwDept.SelectedRows[0].Tag as HierarchyEntity;
                    List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("PROGRAM", row.Agency, row.Dept, string.Empty, string.Empty);
                    gvwProgram.Rows.Clear();
                    foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                    {
                        string isIntake = hierarchyEntity.Intake.Equals("Y") ? "true" : "false";
                        int rowIndex = gvwProgram.Rows.Add(hierarchyEntity.Prog, hierarchyEntity.HirarchyName, hierarchyEntity.ShortName, isIntake);
                        gvwProgram.Rows[rowIndex].Tag = hierarchyEntity;
                        setTooltip(rowIndex, hierarchyEntity, gvwProgram);
                    }
                    if (gvwProgram.Rows.Count > 0)
                    {
                        gvwProgram.Rows[0].Selected = true;
                    }
                    gvwProgram.Update();
                    gvwProgram.ResumeLayout();
                }
            }
        }

        private void setTooltip(int rowIndex, HierarchyEntity hierarchyEntity, DataGridView gvwControl)
        {

            string toolTipText = "Added By     : " + hierarchyEntity.AddOperator.Trim() + " on " + hierarchyEntity.DateAdd + "\n";
            string modifiedBy = string.Empty;
            if (!hierarchyEntity.LSTCOperator.Trim().Equals(string.Empty))
                modifiedBy = hierarchyEntity.LSTCOperator.Trim() + " on " + hierarchyEntity.DateLSTC;
            toolTipText += "Modified By  : " + modifiedBy;

            foreach (DataGridViewCell cell in gvwControl.Rows[rowIndex].Cells)
            {
                if (!(cell is DataGridViewImageCell))
                {
                    cell.ToolTipText = toolTipText;
                }
            }
        }

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAgencyDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvwAgency.ColumnCount - 1 && e.RowIndex != -1)
            {
                if (gvwAgency.Rows[e.RowIndex].Tag is HierarchyEntity)
                {
                    if (gvwDept.SortOrder.ToString() != "None")
                    { SortColumn = gvwDept.SortedColumn.Name.ToString(); SortOrder = gvwDept.SortOrder.ToString(); }

                    HierarchyEntity hierarchyEntity = gvwAgency.Rows[e.RowIndex].Tag as HierarchyEntity;
                    AddHierarchyForm addHierarchyForm = new AddHierarchyForm(BaseForm, "Add", "DEPT", hierarchyEntity.Agency, string.Empty, string.Empty, hierarchyEntity.ShortName);
                    addHierarchyForm.StartPosition = FormStartPosition.CenterScreen;
                    addHierarchyForm.ShowDialog();
                }
            }
        }

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeptDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvwDept.ColumnCount - 3 && e.RowIndex != -1)
            {
                if (gvwDept.Rows[e.RowIndex].Tag is HierarchyEntity)
                {
                    if (gvwProgram.SortOrder.ToString() != "None")
                    { SortColumn = gvwProgram.SortedColumn.Name.ToString(); SortOrder = gvwProgram.SortOrder.ToString(); }

                    HierarchyEntity hierarchyEntity = gvwDept.Rows[e.RowIndex].Tag as HierarchyEntity;
                    AddHierarchyForm addHierarchyForm = new AddHierarchyForm(BaseForm, "Add", "PROGRAM", hierarchyEntity.Agency, hierarchyEntity.Dept, string.Empty, hierarchyEntity.ShortName);
                    addHierarchyForm.StartPosition = FormStartPosition.CenterScreen;
                    addHierarchyForm.ShowDialog();
                }
            }
            else if (e.ColumnIndex == gvwDept.ColumnCount - 2 && e.RowIndex != -1)
            {
                if (gvwDept.SortOrder.ToString() != "None")
                { SortColumn = gvwDept.SortedColumn.Name.ToString(); SortOrder = gvwDept.SortOrder.ToString(); }

                HierarchyEntity selectedRow = GetSelectedDeptRow();
                AddHierarchyForm editHierarchyForm = new AddHierarchyForm(BaseForm, "Edit", "DEPT", selectedRow.Agency, selectedRow.Dept, string.Empty, string.Empty);
                editHierarchyForm.StartPosition = FormStartPosition.CenterScreen;
                editHierarchyForm.ShowDialog();
            }
            else if (e.ColumnIndex == gvwDept.ColumnCount - 1 && e.RowIndex != -1)
            {
                if (gvwDept.SortOrder.ToString() != "None")
                { SortColumn = gvwDept.SortedColumn.Name.ToString(); SortOrder = gvwDept.SortOrder.ToString(); }

                HierarchyEntity hierarchyEntity = GetSelectedDeptRow();
                if (hierarchyEntity == null) return;
                string strMsg = _model.HierarchyAndPrograms.DeleteHierarchy(hierarchyEntity);
                if (strMsg != "Success")
                    AlertBox.Show("You can’t delete this Department, as there are Program(s)", MessageBoxIcon.Warning);
                else
                {
                    HierarchyEntity selectedRow = GetSelectedDeptRow();
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nDepartment: " + selectedRow.HirarchyName, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxDeptHandler);
                }
            }
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
            if (btn.Tag == null) { return; }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:
                        if (gvwAgency.SortOrder.ToString() != "None")
                        { SortColumn = gvwAgency.SortedColumn.Name.ToString(); SortOrder = gvwAgency.SortOrder.ToString(); }

                        AgencyHierarchyForm addHierarchyForm = new AgencyHierarchyForm(BaseForm, "Add", "AGENCY", string.Empty, Privileges);
                        addHierarchyForm.StartPosition = FormStartPosition.CenterScreen;
                        addHierarchyForm.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                        HierarchyEntity selectedRow = GetSelectedAgencyRow();
                        if (gvwAgency.SortOrder.ToString() != "None")
                        { SortColumn = gvwAgency.SortedColumn.Name.ToString(); SortOrder = gvwAgency.SortOrder.ToString(); }
                        AgencyHierarchyForm editHierarchyForm = new AgencyHierarchyForm(BaseForm, "Edit", "AGENCY", selectedRow.Agency, Privileges);
                        editHierarchyForm.StartPosition = FormStartPosition.CenterScreen;
                        editHierarchyForm.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Delete:
                        selectedRow = GetSelectedAgencyRow();
                        if (gvwAgency.SortOrder.ToString() != "None")
                        { SortColumn = gvwAgency.SortedColumn.Name.ToString(); SortOrder = gvwAgency.SortOrder.ToString(); }

                        HierarchyEntity hierarchyEntity = new HierarchyEntity();
                        hierarchyEntity.Agency = selectedRow.Agency;
                        string strMsg = _model.HierarchyAndPrograms.DeleteHierarchy(hierarchyEntity);

                        if (strMsg != "Success")
                        {
                            AlertBox.Show("You can’t delete this Agency, as there are Department(s)", MessageBoxIcon.Warning);
                        }
                        else
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nAgency: " + selectedRow.HirarchyName, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                        break;
                    case Consts.ToolbarActions.Excel:
                        if (gvwAgency.Rows.Count > 0)
                            gvwAgency.Rows[0].Selected = true;
                        On_SaveExcelForm_Closed();
                        break;
                    case Consts.ToolbarActions.Help:
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
                        break;
                }

                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        private void MessageBoxDeptHandler(DialogResult dialogResult)
        {

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {

                string DepID = string.Empty;
                int SelIndex = gvwDept.CurrentRow.Index;
                if (gvwDept.Rows.Count - 1 >= SelIndex + 1)
                    DepID = gvwDept.Rows[SelIndex + 1].Cells["cellDeptCode"].Value.ToString();
                else
                    DepID = gvwDept.Rows[0].Cells["cellDeptCode"].Value.ToString();

                //Vikash Commented - 11/09/2022 - for delete popup issue
                //HierarchyEntity hierarchyEntity = GetSelectedDeptRow();
                //if (hierarchyEntity == null) return;
                //string strMsg = _model.HierarchyAndPrograms.DeleteHierarchy(hierarchyEntity);


                //if (strMsg == "Success")
                //{ 
                AlertBox.Show("Deapartment Code: " + gvwDept.CurrentRow.Cells["cellDeptCode"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                HierarchyDefinitionControl hierarchyControl = BaseForm.GetBaseUserControl() as HierarchyDefinitionControl;
                if (hierarchyControl != null)
                {
                    hierarchyControl.RefreshGrid("DEPT", DepID);
                }
                //}
                //else
                //{
                //    MessageBox.Show("You can’t delete this Department, as there are Program(s)", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}

            }

        }

        private void MessageBoxProgramHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                //HierarchyEntity hierarchyEntity = GetSelectedProgramRow();
                string ProgID = string.Empty;
                int SelIndex = gvwProgram.CurrentRow.Index;
                if (gvwProgram.Rows.Count - 1 >= SelIndex + 1)
                    ProgID = gvwProgram.Rows[SelIndex + 1].Cells["cellProgCode"].Value.ToString();
                else
                    ProgID = gvwProgram.Rows[0].Cells["cellProgCode"].Value.ToString();

                //Vikash Commented - 11/09/2022 - for delete popup issue
                //if (hierarchyEntity == null) return;
                //string strMsg = _model.HierarchyAndPrograms.DeleteHierarchy(hierarchyEntity);


                //if (strMsg == "Success")
                //{
                AlertBox.Show("Program Code: " + gvwProgram.CurrentRow.Cells["cellProgCode"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                HierarchyDefinitionControl hierarchyControl = BaseForm.GetBaseUserControl() as HierarchyDefinitionControl;
                if (hierarchyControl != null)
                {
                    hierarchyControl.RefreshGrid("PROGRAM", ProgID);
                }
                //}
                //else
                //{
                //    MessageBox.Show("You can’t delete this Program, as there are Dependent(s)", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}

            }

        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {

                //HierarchyEntity selectedRowCode = GetSelectedAgencyRow();

                string AgyID = string.Empty;
                int SelIndex = gvwAgency.CurrentRow.Index;
                if (gvwAgency.Rows.Count - 1 >= SelIndex + 1)
                    AgyID = gvwAgency.Rows[SelIndex + 1].Cells["cellCode"].Value.ToString();
                else
                    AgyID = gvwAgency.Rows[0].Cells["cellCode"].Value.ToString();

                //Vikash Commented - 11/09/2022 - for delete popup issue
                //HierarchyEntity hierarchyEntity = new HierarchyEntity();
                //hierarchyEntity.Agency = selectedRowCode.Agency;
                //string strMsg = _model.HierarchyAndPrograms.DeleteHierarchy(hierarchyEntity);

                //if (strMsg == "Success")
                //{
                AlertBox.Show("Agency Code: " + gvwAgency.CurrentRow.Cells["cellCode"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                HierarchyDefinitionControl hierarchyControl = BaseForm.GetBaseUserControl() as HierarchyDefinitionControl;
                if (hierarchyControl != null)
                {
                    hierarchyControl.RefreshGrid("AGENCY", AgyID);
                }
                //}
                //else
                //{
                //    MessageBox.Show("You can’t delete this Agency, as there are Department(s)", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}

            }

        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public HierarchyEntity GetSelectedAgencyRow()
        {
            HierarchyEntity hierarchyEntity = null;
            if (gvwAgency != null)
            {
                foreach (DataGridViewRow dr in gvwAgency.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        hierarchyEntity = dr.Tag as HierarchyEntity;
                        break;
                    }
                }
            }
            return hierarchyEntity;
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public HierarchyEntity GetSelectedDeptRow()
        {
            HierarchyEntity hierarchyEntity = null;
            if (gvwDept != null)
            {
                foreach (DataGridViewRow dr in gvwDept.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        hierarchyEntity = dr.Tag as HierarchyEntity;
                        break;
                    }
                }
            }
            return hierarchyEntity;
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public HierarchyEntity GetSelectedProgramRow()
        {
            HierarchyEntity hierarchyEntity = null;
            if (gvwProgram != null)
            {
                foreach (DataGridViewRow dr in gvwProgram.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        hierarchyEntity = dr.Tag as HierarchyEntity;
                        break;
                    }
                }
            }
            return hierarchyEntity;
        }

        private void OnProgramDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvwProgram.ColumnCount - 2 && e.RowIndex != -1)
            {
                HierarchyEntity selectedRow = GetSelectedProgramRow();
                if (selectedRow != null)
                {
                    if (gvwProgram.SortOrder.ToString() != "None")
                    { SortColumn = gvwProgram.SortedColumn.Name.ToString(); SortOrder = gvwProgram.SortOrder.ToString(); }


                    AddHierarchyForm editHierarchyForm = new AddHierarchyForm(BaseForm, "Edit", "PROGRAM", selectedRow.Agency, selectedRow.Dept, selectedRow.Prog, string.Empty);
                    editHierarchyForm.StartPosition = FormStartPosition.CenterScreen;
                    editHierarchyForm.ShowDialog();
                }
            }
            else if (e.ColumnIndex == gvwProgram.ColumnCount - 1 && e.RowIndex != -1)
            {
                bool booldelete = true;
                HierarchyEntity selectedRow = GetSelectedProgramRow();
                if (gvwProgram.SortOrder.ToString() != "None")
                { SortColumn = gvwProgram.SortedColumn.Name.ToString(); SortOrder = gvwProgram.SortOrder.ToString(); }

                ProgramDefinitionEntity programdef = _model.HierarchyAndPrograms.GetCaseDepadp(selectedRow.Agency, selectedRow.Dept, selectedRow.Prog);
                if (programdef != null)
                {
                    booldelete = false;
                    AlertBox.Show("You can't delete this Program, \n as this Program is already used in Program Definition", MessageBoxIcon.Warning);
                }
                if (booldelete)
                {
                    DataSet ds = SPAdminDB.Browse_CASESP1(string.Empty, selectedRow.Agency, selectedRow.Dept, selectedRow.Prog, "SP1ONLY");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        booldelete = false;
                        AlertBox.Show ("You can't delete this Program, \n as this Program is already used in SP Administration Screen", MessageBoxIcon.Warning);
                    }
                }
                if (selectedRow == null) return;
                string strMsg = _model.HierarchyAndPrograms.DeleteHierarchy(selectedRow);
                if (strMsg != "Success")
                {
                    AlertBox.Show("You can’t delete this Program, as there are Dependent(s)", MessageBoxIcon.Warning);
                }
                else//(booldelete)
                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nProgram: " + selectedRow.HirarchyName, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxProgramHandler);
            }
        }

        private void SetGridColsOnPrivileges()
        {
            int TmpCount = 3;
            if (Privileges.AddPriv.Equals("false"))
            {
                gvwDept.Columns[4].Visible = false; TmpCount--;
            }
            if (Privileges.ChangePriv.Equals("false"))
            {
                gvwDept.Columns[5].Visible = false;
                gvwProgram.Columns[4].Visible = false; TmpCount--;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvwDept.Columns[6].Visible = false;
                gvwProgram.Columns[5].Visible = false; TmpCount--;
            }
            /* switch (TmpCount)
             {
                 case 1: if (Privileges.AddPriv.Equals("true"))
                            this.splitContainer2.SplitterDistance = 405;
                     else
                         this.splitContainer2.SplitterDistance = 380;
                     break;
                 case 2: if (Privileges.AddPriv.Equals("true"))
                         this.splitContainer2.SplitterDistance = 435;
                     else
                         this.splitContainer2.SplitterDistance = 415;
                     break;
                 default: this.splitContainer2.SplitterDistance = 441; break;
             }*/
        }

        string Agency = null;
        string Random_Filename = null; string PdfName = null;
        private void On_SaveExcelForm_Closed()
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "Hierarchy_" + "Report";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
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

            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);

            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHIERPT("ALL", string.Empty, string.Empty, string.Empty, string.Empty, "frmHIEDEFSCRN");
            Worksheet sheet; WorksheetCell cell; WorksheetRow Row0;
            string ReportName = "Sheet1";
            if (caseHierarchy.Count > 0)
            {
                caseHierarchy = caseHierarchy.OrderBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();

                sheet = book.Worksheets.Add(ReportName);
                sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(300);

                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(150);

                Row0 = sheet.Table.Rows.Add();

                cell = Row0.Cells.Add("Agency Code", DataType.String, "s94");
                cell = Row0.Cells.Add("Department Code", DataType.String, "s94");
                cell = Row0.Cells.Add("Program Code", DataType.String, "s94");
                cell = Row0.Cells.Add("Program Name", DataType.String, "s94");
                
                cell = Row0.Cells.Add("# of Intakes", DataType.String, "s94");
                cell = Row0.Cells.Add("# of Activity Programs", DataType.String, "s94");
                cell = Row0.Cells.Add("# of Outcome Programs", DataType.String, "s94");

                bool First = true; string PrivAgy = string.Empty, PrivDept = string.Empty;
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    Row0 = sheet.Table.Rows.Add();
                    if (hierarchyEntity.Agency.Trim() != PrivAgy)
                    {
                        if (!First)
                        {
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");

                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");

                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");

                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");

                            Row0 = sheet.Table.Rows.Add();

                        }
                        First = false; PrivAgy = hierarchyEntity.Agency.Trim(); PrivDept = hierarchyEntity.Dept.Trim();
                    }
                    else if (hierarchyEntity.Dept.Trim() != PrivDept)
                    {
                        if (!First)
                        {
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");

                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");
                            cell = Row0.Cells.Add("", DataType.String, "s96");

                            Row0 = sheet.Table.Rows.Add();

                        }
                        First = false; PrivAgy = hierarchyEntity.Agency.Trim(); PrivDept = hierarchyEntity.Dept.Trim();
                    }

                    cell = Row0.Cells.Add(hierarchyEntity.Agency.Trim(), DataType.String, "s95");
                    cell = Row0.Cells.Add(hierarchyEntity.Dept.Trim(), DataType.String, "s95");
                    cell = Row0.Cells.Add(hierarchyEntity.Prog.Trim(), DataType.String, "s95");
                    cell = Row0.Cells.Add(hierarchyEntity.HirarchyName.Trim(), DataType.String, "s95");

                    cell = Row0.Cells.Add(hierarchyEntity.IntakeCNT.Trim(), DataType.String, "s97");
                    cell = Row0.Cells.Add(hierarchyEntity.ACTProgCNT.Trim(), DataType.String, "s97");
                    cell = Row0.Cells.Add(hierarchyEntity.MSProgCNT.Trim(), DataType.String, "s97");
                }


            }


            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            //FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //downloadGateway.Filename = "Hierarchy_Report.xls";

            //// downloadGateway.Version = file.Version;

            //downloadGateway.SetContentType(DownloadContentType.OctetStream);

            //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

            FileInfo fiDownload = new FileInfo(PdfName);
            /// Need to check for file exists, is local file, is allow to read, etc...
            string name = fiDownload.Name;
            using (FileStream fileStream = fiDownload.OpenRead())
            {
                Application.Download(fileStream, name);
            }

            //On_Delete_PDF_File();

            //if (LookupDataAccess.FriendlyName().Contains("2012"))
            //{
            //    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            //    objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}
            //else
            //{
            //    FrmViewer objfrm = new FrmViewer(PdfName);
            //    objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.NumberFormat = "0%";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Arial";
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s21.NumberFormat = "0%";
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Interior.Color = "#D8D8D8";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Interior.Color = "#D8D8D8";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Interior.Color = "#D8D8D8";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.NumberFormat = "0%";
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Interior.Color = "#D8D8D8";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Interior.Color = "#D8D8D8";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s28.NumberFormat = "0%";
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.NumberFormat = "0%";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s69.NumberFormat = "0%";
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.NumberFormat = "0%";
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s72.NumberFormat = "0%";
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.NumberFormat = "0%";
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.NumberFormat = "0%";
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.NumberFormat = "0%";
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.NumberFormat = "0%";
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.NumberFormat = "0%";
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Arial";
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.Bold = true;
            s82.Font.FontName = "Arial";
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.NumberFormat = "0%";
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Arial";
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s84.NumberFormat = "0%";
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Arial";
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.NumberFormat = "0%";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Arial";
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s87.NumberFormat = "0%";
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s90.NumberFormat = "0%";
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Font.Bold = true;
            s92.Font.Italic = true;
            s92.Font.FontName = "Arial";
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s92.NumberFormat = "0%";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.Italic = true;
            s93.Font.FontName = "Arial";
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Arial";
            s94.Font.Color = "#000000";
            s94.Interior.Color = "#B0C4DE";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Top;
            s94.Alignment.WrapText = true;
            s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95B
            // -----------------------------------------------
            WorksheetStyle s95B = styles.Add("s95B");
            s95B.Font.FontName = "Arial";
            s95B.Font.Bold = true;
            s95B.Font.Color = "#0000FF";
            s95B.Interior.Color = "#FFFFFF";
            s95B.Interior.Pattern = StyleInteriorPattern.Solid;
            s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95B.Alignment.WrapText = true;
            s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //  s95R
            // -----------------------------------------------
            WorksheetStyle s95R = styles.Add("s95R");
            s95R.Font.FontName = "Arial";
            //s95R.Font.Bold = true;
            s95R.Font.Color = "#FF0000";
            s95R.Interior.Color = "#FFFFFF";
            s95R.Interior.Pattern = StyleInteriorPattern.Solid;
            s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95R.Alignment.WrapText = true;
            s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Font.Bold = true;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //s96.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");

            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.FontName = "Arial";
            s97.Font.Color = "#000000";
            s97.Interior.Color = "#FFFFFF";
            s97.Interior.Pattern = StyleInteriorPattern.Solid;
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s97.Alignment.Vertical = StyleVerticalAlignment.Top;
            s97.Alignment.WrapText = true;
            s97.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s97.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s97.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s97.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s97.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Arial";
            s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Arial";
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.NumberFormat = "0%";
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.NumberFormat = "0%";
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.NumberFormat = "0%";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Arial";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.NumberFormat = "0%";
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.NumberFormat = "0%";
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.NumberFormat = "0%";
        }



    }
}