#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Captain.Common.EventArg;
using Captain.Common.Handlers;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Model.Objects;
using Wisej.Web;
using Captain.Common.Model.Data;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class PrivilegesControl : UserControl
    {
        private List<PrivilegeEntity> _selectedPrivileges = null;

        public PrivilegesControl(BaseForm baseForm, string privType, string userID)
        {
            InitializeComponent();
            BaseForm = baseForm;
            PrivType = privType;
            SelectedUser = userID;
            if (PrivType.Equals("Screen"))
            {
                AddScreenGridColumns(gvwControl);
            }
            else
            {
                AddReportGridColumns(gvwControl);
            }

            if (gvwControl.ContextMenu == null)
            {
                // gvwControl.ContextMenu = BaseForm.MenuManager.GenerateHierarchieGridContextMenu(OnContextMenuTeamTabMenuClick);
            }

        }
        public PrivilegesControl(BaseForm baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
        }

        #region Public Properties

        public BaseForm BaseForm { get; set; }

        public string SelectedUser { get; set; }

        public string PrivType { get; set; }

        public PrivilegeEntity PrivelegeEntity { get; set; }

        public List<HierarchyEntity> SelectedHierarchy { get; set; }

        public List<PrivilegeEntity> SelectedPrivileges
        {
            get
            {
                _selectedPrivileges = new List<PrivilegeEntity>();
                foreach (DataGridViewRow row in gvwControl.Rows)
                {
                    _selectedPrivileges.Add(row.Tag as PrivilegeEntity);
                }
                return _selectedPrivileges;
            }
        }

        public bool SetEditable
        {
            set
            {
                picAdd.Visible = !value;
                picEdit.Visible = value;
            }
        }

        public bool SetVisible
        {
            set
            {
                picAdd.Visible = value;
                picEdit.Visible = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView GridViewControl
        {
            get
            {
                return gvwControl;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPrivilegesFormClosed(object sender, FormClosedEventArgs e)
        {
            AddPrivilegesForm form = sender as AddPrivilegesForm;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;

            if (form.DialogResult == DialogResult.Yes)
            {
                List<DataGridViewRow> selectedPrivileges = form.SelectedPrivileges;
                int OtherModulesCnt = selectedPrivileges.FindAll(x => x.Cells[0].Value.ToString() != "01").Count;
                int AdminModulesCnt = selectedPrivileges.FindAll(x => x.Cells[0].Value.ToString() == "01").Count;

                //if (gvwControl.Rows.Count > 0)
                //{
                //    string formType = form.MenuAction;

                //    string changeHierarchy = string.Empty;
                //    if (formType.Equals("Screen")) changeHierarchy = form.ChangeHierarchy;
                //    List<string> selectedModule = form.SelectedModules;
                //    List<DataGridViewRow> listOfSelected = new List<DataGridViewRow>();
                //    foreach (DataGridViewRow row in gvwControl.Rows)
                //    {
                //        PrivilegeEntity privilegeEntity = row.Tag as PrivilegeEntity;
                //        if (formType.Equals("Screen"))
                //        {
                //            string mcode = privilegeEntity.ModuleCode + privilegeEntity.Hierarchy;
                //            if (!(selectedModule.Contains(mcode)))
                //            {
                //                listOfSelected.Add(row);
                //            }
                //        }
                //        else
                //        {
                //            if (!selectedModule.Contains(privilegeEntity.ModuleCode))
                //            {
                //                listOfSelected.Add(row);
                //            }
                //        }
                //    }
                //    gvwControl.Rows.Clear();
                //    foreach (DataGridViewRow row in listOfSelected)
                //    {
                //        //gvwControl.Rows.Add(row);
                //        string _cell1 = row[0].Value.ToString();
                //        string _cell2 = row[1].Value.ToString();
                //        string _cell3 = row[2].Value.ToString();
                //        string _cell4 = row[3].Value.ToString();
                //        string _cell5 = row[4].Value.ToString();
                //        string _cell6 = row[5].Value.ToString();
                //        string _cell7 = row[6].Value.ToString();


                //        int rowIndex = gvwControl.Rows.Add(_cell1, _cell2, _cell3, _cell4, _cell5, _cell6, _cell7);
                //        gvwControl.Rows[rowIndex].Tag = row.Tag;

                //    }
                //}
                gvwControl.Rows.Clear();
                int Acnt = 0; string hierarchy = ""; int rowIndex = 0; List<PrivilegeEntity> prvTempEntity = null;
                
                foreach (HierarchyEntity hierarchyEntity in SelectedHierarchy)
                {
                    foreach (DataGridViewRow row in selectedPrivileges)
                    {


                        bool isView = false;
                        bool isChange = false;
                        bool isDelete = false;
                        bool isAdd = false;
                        string screenName = string.Empty;
                        string moduleCode = string.Empty;
                        if (row.Cells["ScreenName"].Value != null)
                        {
                            screenName = row.Cells["ScreenName"].Value.ToString();
                        }
                        if (row.Cells["Module"].Value != null)
                        {
                            moduleCode = row.Cells["ModuleName"].Value.ToString();
                        }
                        PrivilegeEntity privilegeEntity = row.Tag as PrivilegeEntity;

                        prvTempEntity = new List<PrivilegeEntity>();
                        

                        if (PrivType.Equals("Screen"))
                        {
                            hierarchy = string.Empty;
                            isView = false;
                            isAdd = false;
                            isChange = false;
                            isDelete = false;
                            if (row.Cells["Module"].Value.ToString() == "01")
                            {
                                if (row.Cells["Hierarchy"].Value != null)
                                {
                                    hierarchy = row.Cells["Hierarchy"].Value.ToString();
                                }
                                Acnt++;
                            }
                            else
                            {
                                hierarchy = hierarchyEntity.Code;
                                privilegeEntity.Hierarchy = hierarchy;
                            }
                            DataGridViewCheckBoxCell viewCell = row.Cells["View"] as DataGridViewCheckBoxCell;
                            DataGridViewCheckBoxCell addCell = row.Cells["Add"] as DataGridViewCheckBoxCell;
                            DataGridViewCheckBoxCell changeCell = row.Cells["Change"] as DataGridViewCheckBoxCell;
                            DataGridViewCheckBoxCell deleteCell = row.Cells["Delete"] as DataGridViewCheckBoxCell;
                            if (viewCell.Value != null)
                            {
                                if (viewCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                    isView = true;
                            }
                            if (addCell.Value != null)
                            {
                                if (addCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                    isAdd = true;
                            }
                            if (changeCell.Value != null)
                            {
                                if (changeCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                    isChange = true;
                            }
                            if (deleteCell.Value != null)
                            {
                                if (deleteCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                    isDelete = true;


                            }
                            if (row.Cells["Module"].Value.ToString() == "01" && AdminModulesCnt < Acnt) { }
                            else
                            {
                                rowIndex = gvwControl.Rows.Add(moduleCode, screenName, hierarchy, isView, isAdd, isChange, isDelete);
                            }
                        }
                        else
                        {
                            DataGridViewCheckBoxCell viewCell = row.Cells["View"] as DataGridViewCheckBoxCell;
                            if (viewCell.Value != null)
                            {
                                if (viewCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                    isView = true;
                            }
                            rowIndex = gvwControl.Rows.Add(moduleCode, screenName, isView);
                        }

                        if (row.Cells["Module"].Value.ToString() == "01" && AdminModulesCnt < Acnt) { }
                        else
                        {
                            privilegeEntity.ViewPriv = isView ? "true" : "false";
                            privilegeEntity.AddPriv = isAdd ? "true" : "false";
                            privilegeEntity.DelPriv = isDelete ? "true" : "false";
                            privilegeEntity.ChangePriv = isChange ? "true" : "false";
                            privilegeEntity.Hierarchy = hierarchy;
                            

                            gvwControl.Rows[rowIndex].Tag = privilegeEntity;
                            
                        }
                        /*Hiding the checkboxes for particular screens -- kranthi 07-07-2022 */
                        LookupDataAccess.Hide_ScreenPrivilages_checkboxes(privilegeEntity, gvwControl, rowIndex);

                    }
                }
            }

            if (form.DialogResult == DialogResult.OK)
            {
                List<DataGridViewRow> selectedPrivileges = form.SelectedPrivileges;
                if (gvwControl.Rows.Count > 0)
                {
                    string formType = form.MenuAction;

                    string changeHierarchy = string.Empty;
                    if (formType.Equals("Screen")) changeHierarchy = form.ChangeHierarchy;
                    List<string> selectedModule = form.SelectedModules;
                    List<DataGridViewRow> listOfSelected = new List<DataGridViewRow>();
                    foreach (DataGridViewRow row in gvwControl.Rows)
                    {
                        PrivilegeEntity privilegeEntity = row.Tag as PrivilegeEntity;
                        if (formType.Equals("Screen"))
                        {
                            string mcode = privilegeEntity.ModuleCode + privilegeEntity.Hierarchy;
                            if (!(selectedModule.Contains(mcode)))
                            {
                                listOfSelected.Add(row);
                            }
                        }
                        else
                        {
                            if (!selectedModule.Contains(privilegeEntity.ModuleCode))
                            {
                                listOfSelected.Add(row);
                            }
                        }
                    }
                    gvwControl.Rows.Clear();
                    foreach (DataGridViewRow row in listOfSelected)
                    {
                        string _cell1 = "";
                        string _cell2 = "";
                        string _cell3 = "";
                        string _cell4 = "";
                        string _cell5 = "";
                        string _cell6 = "";
                        string _cell7 = "";

                        //gvwControl.Rows.Add(row);
                        
                        if (formType.Equals("Screen"))
                        {
                            _cell1 = row[0].Value.ToString();
                            _cell2 = row[1].Value.ToString();
                            _cell3 = row[2].Value.ToString();
                            _cell4 = row[3].Value.ToString();
                            _cell5 = row[4].Value.ToString();
                            _cell6 = row[5].Value.ToString();
                            _cell7 = row[6].Value.ToString();
                        }
                        else {

                            _cell1 = row[0].Value.ToString();
                            _cell2 = row[1].Value.ToString();
                            _cell3 = row[2].Value.ToString();
                            _cell4 = "";
                            _cell5 = "";
                            _cell6 = "";
                            _cell7 = "";
                        }

                        int rowIndex = gvwControl.Rows.Add(_cell1, _cell2, _cell3, _cell4, _cell5, _cell6, _cell7);
                        gvwControl.Rows[rowIndex].Tag = row.Tag;

                    }
                }
                //gvwControl.Rows.Clear();
                foreach (DataGridViewRow row in selectedPrivileges)
                {
                    bool isView = false;
                    bool isChange = false;
                    bool isDelete = false;
                    bool isAdd = false;
                    string screenName = string.Empty;
                    string moduleCode = string.Empty;
                    if (row.Cells["ScreenName"].Value != null)
                    {
                        screenName = row.Cells["ScreenName"].Value.ToString();
                    }
                    if (row.Cells["Module"].Value != null)
                    {
                        moduleCode = row.Cells["ModuleName"].Value.ToString();
                    }
                    PrivilegeEntity privilegeEntity = row.Tag as PrivilegeEntity;
                    int rowIndex = 0;
                    if (PrivType.Equals("Screen"))
                    {
                        string hierarchy = string.Empty;
                        isView = false;
                        isAdd = false;
                        isChange = false;
                        isDelete = false;
                        if (row.Cells["Hierarchy"].Value != null)
                        {
                            hierarchy = row.Cells["Hierarchy"].Value.ToString();
                        }
                        DataGridViewCheckBoxCell viewCell = row.Cells["View"] as DataGridViewCheckBoxCell;
                        DataGridViewCheckBoxCell addCell = row.Cells["Add"] as DataGridViewCheckBoxCell;
                        DataGridViewCheckBoxCell changeCell = row.Cells["Change"] as DataGridViewCheckBoxCell;
                        DataGridViewCheckBoxCell deleteCell = row.Cells["Delete"] as DataGridViewCheckBoxCell;
                        if (viewCell.Value != null)
                        {
                            if (viewCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                isView = true;
                        }
                        if (addCell.Value != null)
                        {
                            if (addCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                isAdd = true;
                        }
                        if (changeCell.Value != null)
                        {
                            if (changeCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                isChange = true;
                        }
                        if (deleteCell.Value != null)
                        {
                            if (deleteCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                isDelete = true;


                        }
                        rowIndex = gvwControl.Rows.Add(moduleCode, screenName, hierarchy, isView, isAdd, isChange, isDelete);
                    }
                    else
                    {
                        DataGridViewCheckBoxCell viewCell = row.Cells["View"] as DataGridViewCheckBoxCell;
                        if (viewCell.Value != null)
                        {
                            if (viewCell.Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                isView = true;
                        }
                        rowIndex = gvwControl.Rows.Add(moduleCode, screenName, isView);
                    }
                    privilegeEntity.ViewPriv = isView ? "true" : "false";
                    privilegeEntity.AddPriv = isAdd ? "true" : "false";
                    privilegeEntity.DelPriv = isDelete ? "true" : "false";
                    privilegeEntity.ChangePriv = isChange ? "true" : "false";
                    gvwControl.Rows[rowIndex].Tag = privilegeEntity;

                    /*Hiding the checkboxes for particular screens -- kranthi 07-07-2022 */
                    LookupDataAccess.Hide_ScreenPrivilages_checkboxes(privilegeEntity, gvwControl, rowIndex);
                }

            }
        }



        #endregion



        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddReportGridColumns(DataGridView dataGridView)
        {
            DataGridViewCheckBoxColumn dataTypeColumn = null;
            string columnName = string.Empty;

            gvwControl.Columns.Clear();
            DataGridViewTextBoxColumn moduleColumn = new DataGridViewTextBoxColumn();
            moduleColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleColumn.HeaderText = "Module";
            moduleColumn.Name = "Module";
            moduleColumn.Width = 200;
            dataGridView.Columns.Add(moduleColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn();
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.HeaderText = "Report Name";
            codeColumn.Name = "ScreenName";
            codeColumn.Width = 310;
            dataGridView.Columns.Add(codeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "View";
            dataTypeColumn.Name = "View";
            // dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 40;
            dataGridView.Columns.Add(dataTypeColumn);
        }

        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddScreenGridColumns(DataGridView dataGridView)
        {
            DataGridViewCheckBoxColumn dataTypeColumn = null;
            string columnName = string.Empty;

            gvwControl.Columns.Clear();
            DataGridViewTextBoxColumn moduleColumn = new DataGridViewTextBoxColumn();
            moduleColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleColumn.HeaderText = "Module";
            moduleColumn.Name = "Module";
            moduleColumn.Width = 200;
            dataGridView.Columns.Add(moduleColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn();
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.HeaderText = "Screen Name";
            codeColumn.Name = "ScreenName";
            codeColumn.Width = 310;
            dataGridView.Columns.Add(codeColumn);

            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = "Hierarchy";
            descColumn.Name = "Hierarchy";
            descColumn.Width = 85;
            dataGridView.Columns.Add(descColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "   View";
            dataTypeColumn.Name = "View";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataTypeColumn.HeaderStyle.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            dataGridView.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "Add";
            dataTypeColumn.Name = "Add";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataTypeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataTypeColumn.HeaderStyle.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            dataGridView.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "Edit";
            dataTypeColumn.Name = "Change";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataTypeColumn.HeaderStyle.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            dataGridView.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "  Delete";
            dataTypeColumn.Name = "Delete";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.HeaderStyle.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            dataTypeColumn.Width = 50;
            dataGridView.Columns.Add(dataTypeColumn);
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {

        }

        private void OnAddClick(object sender, EventArgs e)
        {
            try
            {
                AddPrivilegesForm hsForm = new AddPrivilegesForm(BaseForm, SelectedHierarchy, SelectedPrivileges, PrivType, null, PrivelegeEntity);
                hsForm.StartPosition = FormStartPosition.CenterScreen;
                hsForm.FormClosed += new FormClosedEventHandler(OnPrivilegesFormClosed);
                hsForm.StartPosition = FormStartPosition.CenterScreen;
                hsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            try
            {
                AddPrivilegesForm hsForm = new AddPrivilegesForm(BaseForm, SelectedHierarchy, SelectedPrivileges, PrivType, SelectedUser, PrivelegeEntity);
                hsForm.StartPosition = FormStartPosition.CenterScreen;
                hsForm.FormClosed += new FormClosedEventHandler(OnPrivilegesFormClosed);
                hsForm.StartPosition = FormStartPosition.CenterScreen;
                hsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                //
            }
        }

    }
}