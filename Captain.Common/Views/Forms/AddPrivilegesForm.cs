/************************************************************************************
 * Class Name    : AddTeamRolesForm
 * Author        : 
 * Created Date  : 
 * Version       : 1.0
 * Description   : This is License Control, which is used to show the license information.
 ************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Captain.Common.Model.Objects;
using Captain.Common.Resources;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using System.Data;
using Captain.Common.Model.Data;
using System.Data.SqlClient;
using System.Drawing;
using Wisej.Web;
using DevExpress.XtraReports.UI;


#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AddPrivilegesForm : Form
    {
        #region Variables

        private ErrorProvider _errorProvider = null;
        private List<DataGridViewRow> _selectedPrivileges = null;
        private List<string> _selectedModules = new List<string>();
        private string _selectedHierarchy = string.Empty;
        private List<PrivilegeEntity> _changedPrivileges = new List<PrivilegeEntity>();
        private CaptainModel _model = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor where private variables, objects and label texts are initialized/enabled/disabled.
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="menuAction"></param>
        /// <param name="objectPermission"></param>
        /// <param name="selectedPermissionIndex"></param>
        public AddPrivilegesForm(BaseForm baseForm, List<HierarchyEntity> hierarchy, List<PrivilegeEntity> privileges, string privilegesType, string userID, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            SelectedHierarchy = hierarchy;
            MenuAction = privilegesType;
            SelectedUser = userID;
            ListOfSelectedPrivileges = privileges;
            BaseForm = baseForm;
            _model = new CaptainModel();
            PrivilegeType = privilegesType;
            // this.Text = privilegeEntity.Program;
            if (privilegesType.Equals("Screen"))
            {
                this.Text = "Screen Privileges"; //privilegeEntity.PrivilegeName.Replace("&", "&&");
            }
            else
            {
                this.Text = "Report Privileges"; //privilegeEntity.PrivilegeName.Replace("&", "&&");
            }
            IntializeForm();
        }

        #endregion

        #region Properties

        public BaseForm BaseForm { get; set; }

        public TagClass SelectedNodeTagClass { get; set; }

        public List<HierarchyEntity> SelectedHierarchy { get; set; }

        public string MenuAction { get; set; }

        public string SelectedUser { get; set; }

        public string Mode { get; set; }

        public string PrivilegeType { get; set; }

        public int SelectedPermissionIndex { get; set; }

        public List<PrivilegeEntity> ListOfSelectedPrivileges
        {
            get;
            set;
        }

        public List<string> SelectedModules
        {
            get
            {
                return _selectedModules;
            }
        }

        public string ChangeHierarchy
        {
            get
            {
                _selectedHierarchy = ((ListItem)cmbHierarchy.SelectedItem).Value.ToString();
                return _selectedHierarchy;
            }
        }

        public List<PrivilegeEntity> MainPrivileges
        {
            get
            {
                return _changedPrivileges;
            }
        }

        public List<DataGridViewRow> SelectedPrivileges
        {
            get
            {
                _selectedPrivileges = new List<DataGridViewRow>();
                var grids = (from t in tabControl1.TabPages.Cast<TabPage>().ToList()
                             where t.Name.ToLower().IndexOf("module") >= 0
                             from c in t.Controls.Cast<Control>().ToList()
                             where c is DataGridView
                             select c).ToList();

                if (grids != null)
                {
                    foreach (DataGridView moduleGrid in grids)
                    {
                        List<DataGridViewRow> modulePrivileges = (from c in moduleGrid.Rows.Cast<DataGridViewRow>().ToList()
                                                                  where (((DataGridViewCheckBoxCell)c.Cells["View"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                                  select (DataGridViewRow)c).ToList();
                        if (modulePrivileges != null && modulePrivileges.Count > 0)
                        {
                            _selectedPrivileges.AddRange(modulePrivileges.ToList());
                        }

                        var query = (from c in moduleGrid.Rows.Cast<DataGridViewRow>().ToList()
                                     select (PrivilegeEntity)c.Tag).ToList();
                        _changedPrivileges.AddRange(query);
                    }
                    _selectedModules = (from g in _changedPrivileges
                                        orderby g.ModuleCode, g.Hierarchy
                                        select g.ModuleCode + g.Hierarchy).Distinct().ToList();
                }
                return _selectedPrivileges;
            }
        }

        //public DataGridView ModuleGrid1
        //{
        //    get { return tabControl1.TabPages[0].Controls[0] as DataGridView; }
        //}

        public DataGridView ModuleGrid2
        {
            get { return tabControl1.TabPages[1].Controls[0] as DataGridView; }
        }

        public DataGridView ModuleGrid3
        {
            get { return tabControl1.TabPages[2].Controls[0] as DataGridView; }
        }

        public DataGridView ModuleGrid4
        {
            get { return tabControl1.TabPages[3].Controls[0] as DataGridView; }
        }

        public DataGridView ModuleGrid5
        {
            get { return tabControl1.TabPages[4].Controls[0] as DataGridView; }
        }

        public bool IsLoading1 { get; set; }

        public bool IsLoading2 { get; set; }

        public bool IsLoading3 { get; set; }

        public bool IsLoading4 { get; set; }

        public bool IsLoading5 { get; set; }

        public bool IsLoading6 { get; set; }

        public bool IsLoading7 { get; set; }

        public bool IsLoading8 { get; set; }

        public bool IsLoading9 { get; set; }

        public bool IsLoading10 { get; set; }

        public bool IsLoading99 { get; set; }


        #endregion

        #region Private Methods

        /// <summary>
        /// To intialize the form
        /// </summary>
        private void IntializeForm()
        {
            IntializeLists();
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
            // moduleGrid.ItemsPerPage = 100;
            moduleGrid.Tag = gridName.Substring(gridName.Length - 2, 2);
            moduleGrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(2);
            moduleGrid.RowHeadersVisible = false;
            moduleGrid.AllowUserToAddRows = false;
            moduleGrid.AllowUserToDeleteRows = false;
            moduleGrid.BackColor = System.Drawing.Color.White;
            moduleGrid.AllowUserToResizeRows = false;
            moduleGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            moduleGrid.MultiSelect = false;
            moduleGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            moduleGrid.DataError -= new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);
            moduleGrid.DataError += new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);
            //moduleGrid.MenuClick += new Wisej.Web.MenuEventHandler(DataGrid_MenuClick);
            ((System.ComponentModel.ISupportInitialize)(moduleGrid)).EndInit();

            return moduleGrid;
        }

        /// <summary>
        /// Handles  grid data error event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataGridViewDataError(object sender, DataGridViewDataErrorEventArgs e) { }

        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddReportGridColumns(DataGridView dataGridView)
        {
            DataGridViewCheckBoxColumn dataTypeColumn = null;
            string columnName = string.Empty;

            dataGridView.Columns.Clear();
            DataGridViewTextBoxColumn moduleColumn = new DataGridViewTextBoxColumn();
            moduleColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleColumn.HeaderText = "Module";
            moduleColumn.Name = "Module";
            moduleColumn.Width = 0;
            moduleColumn.Visible = false;
            moduleColumn.ReadOnly = true;
            dataGridView.Columns.Add(moduleColumn);

            DataGridViewTextBoxColumn moduleNameColumn = new DataGridViewTextBoxColumn();
            moduleNameColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleNameColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleNameColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleNameColumn.HeaderText = "ModuleName";
            moduleNameColumn.Name = "ModuleName";
            moduleNameColumn.Width = 0;
            moduleNameColumn.ReadOnly = true;
            moduleNameColumn.Visible = false;
            moduleNameColumn.ShowInVisibilityMenu = false;
            dataGridView.Columns.Add(moduleNameColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "View";
            dataTypeColumn.Name = "View";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 75;
            dataGridView.Columns.Add(dataTypeColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn();
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.ReadOnly = true;
            codeColumn.HeaderText = "Report Name";
            codeColumn.Name = "ScreenName";
            codeColumn.HeaderCell.Style.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            codeColumn.Width = 350;
            dataGridView.Columns.Add(codeColumn);
        }

        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddScreenGridColumns(DataGridView dataGridView)
        {
            DataGridViewCheckBoxColumn dataTypeColumn = null;
            string columnName = string.Empty;

            dataGridView.Columns.Clear();

            DataGridViewTextBoxColumn moduleColumn = new DataGridViewTextBoxColumn();
            moduleColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleColumn.HeaderText = "Module";
            moduleColumn.Name = "Module";
            moduleColumn.Width = 0;
            moduleColumn.ReadOnly = true;
            moduleColumn.Visible = false;
            dataGridView.Columns.Add(moduleColumn);


            DataGridViewTextBoxColumn moduleNameColumn = new DataGridViewTextBoxColumn();
            moduleNameColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            moduleNameColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            moduleNameColumn.CellTemplate = new DataGridViewTextBoxCell();
            moduleNameColumn.HeaderText = "ModuleName";
            moduleNameColumn.Name = "ModuleName";
            moduleNameColumn.Width = 0;
            moduleNameColumn.ReadOnly = true;
            moduleNameColumn.Visible = false;
            moduleNameColumn.ShowInVisibilityMenu = false;
            dataGridView.Columns.Add(moduleNameColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn();
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.HeaderText = "Screen Name";
            codeColumn.Name = "ScreenName";
            codeColumn.Width = 350;
            codeColumn.ReadOnly = true;
            codeColumn.HeaderCell.Style.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridView.Columns.Add(codeColumn);

            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = "Hierarchy";
            descColumn.Name = "Hierarchy";
            descColumn.Width = 70;
            descColumn.ReadOnly = true;
            descColumn.HeaderCell.Style.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridView.Columns.Add(descColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "    All";
            dataTypeColumn.Name = "All";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataTypeColumn.ReadOnly = false;
            dataGridView.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "    View";
            dataTypeColumn.Name = "View";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataTypeColumn.ReadOnly = false;
            dataGridView.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "    Add";
            dataTypeColumn.Name = "Add";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataTypeColumn.ReadOnly = false;
            dataGridView.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "   Edit";
            dataTypeColumn.Name = "Change";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataGridView.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "  Delete";
            dataTypeColumn.Name = "Delete";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 50;
            dataGridView.Columns.Add(dataTypeColumn);
            //dataGridView.ContextMenu = contextMenu1;

            dataGridView.ShowColumnVisibilityMenu = false;
        }

        /// <summary>
        /// To select the combo box item.
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            int index = -1;
            if (comboBox.Items.Count > 0)
            {
                index = 0;
                List<ListItem> items = comboBox.Items.Cast<ListItem>().ToList();
                if ((from u in items where u.Text.Equals(value) select u) != null)
                {
                    index = items.FindIndex(item => item.Text.Equals(value));
                }
            }
            comboBox.SelectedIndex = index;
        }

        /// <summary>
        /// Gets the Default access level ID for the selected Team Role
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetDefaultAccessLevelIDForTeamRole(ComboBox comboBox, string value)
        {
            ListItem item = null;
            string defaultAccessLevelID = string.Empty;
            if (comboBox.Items != null)
            {
                List<ListItem> items = comboBox.Items.Cast<ListItem>().ToList();
                if (items != null)
                {
                    item = items.Find(a => a.Text.Equals(value));
                }
            }
            return defaultAccessLevelID;
        }

        /// <summary>
        /// Intialize the ListBox and ComboBox controls.
        /// </summary>
        private void IntializeLists()
        {
            if (MenuAction.Equals("Reports"))
            {
                cmbHierarchy.Visible = false;
                lblHierarchie.Visible = false;
            }
            else
            {

                cmbHierarchy.ColorMember = "FavoriteColor";
                bool boolusedflag = true;
                foreach (HierarchyEntity hierarchyEntity in SelectedHierarchy)
                {
                    ListItem li = new ListItem(hierarchyEntity.Code + "-" + hierarchyEntity.HirarchyName, (hierarchyEntity.Code.Replace("-", string.Empty)), hierarchyEntity.UsedFlag, hierarchyEntity.UsedFlag.Equals("Y") ? Color.Red : Color.Black);

                    if (hierarchyEntity.UsedFlag == "")       // Add Mode     // kranthi 02/01/2023:: to show all hierarchies in ADD mode
                    {
                        cmbHierarchy.Items.Add(li);
                    }
                    else
                    {                      // Edit Mode        // kranthi 02/01/2023:: Show only Usedflag records in Edit mode
                        if (hierarchyEntity.UsedFlag.Equals("N"))
                            cmbHierarchy.Items.Add(li);
                    }
                    
                    
                    if (hierarchyEntity.UsedFlag.Equals("N"))
                    {
                        boolusedflag = false;
                        //cmbHierarchy.Items.Add(li);
                        cmbHierarchy.SelectedItem = li;
                    }


                    //cmbHierarchy.Items.Add(new ListItem(hierarchyEntity.Code + "-" + hierarchyEntity.HirarchyName, hierarchyEntity.Code,string.Empty,hierarchyEntity.UsedFlag.Equals("Y")?Color.Red:Color.Red));
                }
                cmbHierarchy.Update();
                if (cmbHierarchy.Items.Count == 1)
                {
                    cmbHierarchy.SelectedIndex = 0;
                }
                else
                {

                    if (boolusedflag)
                    {
                        cmbHierarchy.SelectedIndex = 0;
                    }
                }
            }
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            List<CommonEntity> CommoncapApplList = _model.lookupDataAccess.GetCapAppl();
            DataTable dt = ds.Tables[0];
            tabControl1.TabPages.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                string moduleCode = dr["APPL_CODE"].ToString();
                string moduleDesc = dr["APPL_DESCRIPTION"].ToString();
                TabPage tabPage = new TabPage();
                tabPage.Tag = moduleCode;
                DataGridView dataGridView = null;
                if (CommoncapApplList.FindAll(u => u.Code == moduleCode).Count > 0)
                {
                    switch (moduleCode)
                    {
                        case "01":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module01";
                            dataGridView = AddGrid(tabPage, "ModuleGrid01");
                            break;
                        case "02":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module02";
                            dataGridView = AddGrid(tabPage, "ModuleGrid02");
                            break;
                        case "03":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module03";
                            dataGridView = AddGrid(tabPage, "ModuleGrid03");
                            break;
                        case "04":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module04";
                            dataGridView = AddGrid(tabPage, "ModuleGrid04");
                            break;
                        case "05":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module05";
                            dataGridView = AddGrid(tabPage, "ModuleGrid05");
                            break;
                        case "06":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module06";
                            dataGridView = AddGrid(tabPage, "ModuleGrid06");
                            break;
                        case "07":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module07";
                            dataGridView = AddGrid(tabPage, "ModuleGrid07");
                            break;
                        case "08":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module08";
                            dataGridView = AddGrid(tabPage, "ModuleGrid08");
                            break;
                        case "09":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module09";
                            dataGridView = AddGrid(tabPage, "ModuleGrid09");
                            break;
                        case "10":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module10";
                            dataGridView = AddGrid(tabPage, "ModuleGrid10");
                            break;
                        case "11":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module11";
                            dataGridView = AddGrid(tabPage, "ModuleGrid11");
                            break;
                        case "99":
                            tabPage.Text = moduleDesc;
                            tabPage.Name = "Module99";
                            dataGridView = AddGrid(tabPage, "ModuleGrid99");
                            break;
                    }
                }
                if (dataGridView != null)
                {
                    if (MenuAction.Equals("Screen"))
                    {
                        AddScreenGridColumns(dataGridView);
                    }
                    else
                    {
                        AddReportGridColumns(dataGridView);
                    }
                    tabControl1.TabPages.Add(tabPage);
                }
            }
            if (dt.Rows.Count > 0)
            {
                OnModulesTabControlSelectedIndexChanged(tabControl1, new EventArgs());
            }
        }

        /// <summary> 
        /// Remove Access Levels less than the current Access Level on the Team Role
        /// </summary>
        /// <param name="accessLevelID"></param>
        private void BindAccessLevels(string accessLevelID, string defaultAccessLevelID)
        {
        }

        /// <summary>
        /// To remove the duplicates from the list of object collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        private List<T> RemoveDuplicateObjects<T>(List<T> items)
        {
            List<T> distinctItems = null;
            T item = default(T);

            if (items.Count > 0)
            {
                distinctItems = new List<T>();
                for (int index = 0; index < items.Count; index++)
                {
                    item = items[index];
                    if (!distinctItems.Contains(item))
                    {
                        distinctItems.Add(item);
                    }
                }
            }
            return distinctItems;
        }

        /// <summary>
        /// Used to update the Permssions property
        /// </summary>
        private bool SaveDetails()
        {
            bool isValid = true;

            if (ValidateForm())
            {
                var grids = (from t in tabControl1.TabPages.Cast<TabPage>().ToList()
                             where t.Name.ToLower().IndexOf("module") >= 0
                             from c in t.Controls.Cast<Control>().ToList()
                             where c is DataGridView
                             select c).ToList();

                if (grids != null)
                {
                    foreach (DataGridView moduleGrid in grids)
                    {
                        saveEMPLFUNC(moduleGrid);
                    }
                }
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private void saveEMPLFUNC(DataGridView gridPrivileges)
        {
            string type = string.Empty;

            if (MenuAction.Equals("Screen"))
            {
                foreach (DataGridViewRow row in gridPrivileges.Rows)
                {
                    PrivilegeEntity privilegeEntity = row.Tag as PrivilegeEntity;
                    string hierarchy = row.Cells["Hierarchy"].Value.ToString();
                    hierarchy = hierarchy.Replace("-", string.Empty);
                    string moduleCode = privilegeEntity.ModuleCode;
                    string progNo = privilegeEntity.Program;
                    string desc = privilegeEntity.PrivilegeName;
                    string viewPriv = "N";
                    string addPriv = "N";
                    string delPriv = "N";
                    string changePriv = "N";
                    if (row.Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        viewPriv = "Y";
                    }
                    if (privilegeEntity.DateAdd.Equals(string.Empty) && viewPriv.Equals("N"))
                    {
                        continue;
                    }
                    if (row.Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        addPriv = "Y";
                    }
                    if (row.Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        delPriv = "Y";
                    }
                    if (row.Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        changePriv = "Y";
                    }

                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@EFR_EMPLOYEE_NO", SelectedUser));
                    sqlParamList.Add(new SqlParameter("@EFR_MODULE_CODE", moduleCode));
                    sqlParamList.Add(new SqlParameter("@EFR_PROGNO", progNo));
                    sqlParamList.Add(new SqlParameter("@EFR_HIERARCHY", hierarchy));
                    sqlParamList.Add(new SqlParameter("@EFR_ADD_PRIV", addPriv));
                    sqlParamList.Add(new SqlParameter("@EFR_CHG_PRIV", changePriv));
                    sqlParamList.Add(new SqlParameter("@EFR_DEL_PRIV", delPriv));
                    sqlParamList.Add(new SqlParameter("@EFR_INQ_PRIV", viewPriv));
                    sqlParamList.Add(new SqlParameter("@EFR_DESCRIPTION", desc));
                    sqlParamList.Add(new SqlParameter("@EFR_ADD_OPERATOR", BaseForm.UserID));
                    Captain.DatabaseLayer.UserAccess.InsertUpdateEMPLFUNC(sqlParamList);
                }
            }
            else
            {
                foreach (DataGridViewRow row in gridPrivileges.Rows)
                {
                    PrivilegeEntity privilegeEntity = row.Tag as PrivilegeEntity;
                    string moduleCode = privilegeEntity.ModuleCode;
                    string progNo = privilegeEntity.Program;
                    string desc = privilegeEntity.PrivilegeName;
                    string viewPriv = "N";
                    if (row.Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        viewPriv = "Y";
                    }
                    if (privilegeEntity.ViewPriv.Equals(string.Empty) && viewPriv.Equals("N"))
                    {
                        continue;
                    }
                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@BAT_EMPLOYEE_NO", SelectedUser));
                    sqlParamList.Add(new SqlParameter("@BAT_MODULE_CODE", moduleCode));
                    sqlParamList.Add(new SqlParameter("@BAT_REPORT_CODE", progNo));
                    sqlParamList.Add(new SqlParameter("@BAT_REPORT_NAME", desc));
                    sqlParamList.Add(new SqlParameter("@BAT_VIEW_PRIV", viewPriv));
                    sqlParamList.Add(new SqlParameter("@BAT_ADD_OPERATOR", BaseForm.UserID));
                    Captain.DatabaseLayer.UserAccess.InsertUpdateBATCNTL(sqlParamList);
                }
            }
        }

        /// <summary>
        /// Validating form before saving the data
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            bool isValid = true;

            return isValid;
        }

        #endregion

        #region Handle Events

        /// <summary>
        /// Handles the ok button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void OKClick(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();

            /*:::  KRANTHI 07/26/2022 ::: Enhance the code to Copy Privilages from one HIE to Another HIE*/
            //List<DataGridViewRow> selectedPrivileges = SelectedPrivileges;

            //if (selectedPrivileges.Count > 0)
            //{
            //    int OtherModulesCnt = SelectedPrivileges.FindAll(x => x.Cells[0].Value.ToString() != "01").Count;
            //    int AdminModulesCnt = SelectedPrivileges.FindAll(x => x.Cells[0].Value.ToString() == "01").Count;

            //    if (OtherModulesCnt > 0)
            //    {
            //        MessageBox.Show("Do you want to copy the same privilages to other Hierarchies?", "CAPTAIN", MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: HIErarchyLoop);
            //    }
            //    else if (AdminModulesCnt > 0 && OtherModulesCnt == 0)
            //    {
            //        this.DialogResult = DialogResult.OK;
            //        this.Close();
            //    }
            //}
            //else
            //    AlertBox.Show("Please add at least one Privilages", MessageBoxIcon.Warning, null, ContentAlignment.BottomRight, 5000, null, null, true);

            //  
        }

        private void HIErarchyLoop(DialogResult dialogResult)
        {

            if (dialogResult == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Handles the Cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelClick(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

            // MessageBox.Show("Do you want to copy the same privilages to other Hierarchies?","CAPTAIN",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
        }

        #endregion

        private void cmbHierarchy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) { return; }
            //string code = tabControl1.SelectedTab.Tag as string;
            //string hierarchy = ((ListItem)cmbHierarchy.SelectedItem).Value.ToString();
            //DataGridView dataGridView = tabControl1.SelectedTab.Controls[0] as DataGridView;
            //if (dataGridView == null) return;
            //foreach (DataGridViewRow dr in dataGridView.Rows)
            //{
            //    dr.Cells["Hierarchy"].Value = hierarchy;
            //}
            IsLoading1 = false;
            IsLoading2 = false;
            IsLoading3 = false;
            IsLoading4 = false;
            IsLoading5 = false;
            IsLoading6 = false;
            IsLoading7 = false;
            IsLoading8 = false;
            IsLoading9 = false;
            IsLoading10 = false;
            IsLoading99 = false;
            OnModulesTabControlSelectedIndexChanged(tabControl1, new EventArgs());
        }

        /// <summary>
        /// Handles the ModulesControl tabs SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        List<string> strAllItems = new List<string>();
        public void OnModulesTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;
            string code = tabControl1.SelectedTab.Tag as string;
            txtHierarchy.SendToBack();
            cmbHierarchy.BringToFront();
            lblHierarchie.Visible = true;
            txtHierarchy.Visible = true;
            cmbHierarchy.Visible = true;
            panel1.Visible = true;
            if (PrivilegeType.Equals("Reports"))
            {
                lblHierarchie.Visible = false;
                txtHierarchy.Visible = false;
                cmbHierarchy.Visible = false;
                panel1.Visible = false;

            }
            if (code.Equals("01"))
            {
                txtHierarchy.BringToFront();
                cmbHierarchy.SendToBack();
                lblHierarchie.Visible = false;
                txtHierarchy.Visible = false;
                cmbHierarchy.Visible = false;
                panel1.Visible = false;
                //pnlCopy.Visible = false;
                if (IsLoading1)
                {
                    return;
                }
                else
                {
                    IsLoading1 = true;
                }
            }
            else if (code.Equals("02"))
            {
                if (IsLoading2)
                {
                    return;
                }
                else
                {
                    IsLoading2 = true;
                }
            }
            else if (code.Equals("03"))
            {
                if (IsLoading3)
                {
                    return;
                }
                else
                {
                    IsLoading3 = true;
                }
            }
            else if (code.Equals("04"))
            {
                if (IsLoading4)
                {
                    return;
                }
                else
                {
                    IsLoading4 = true;
                }
            }
            else if (code.Equals("05"))
            {
                if (IsLoading5)
                {
                    return;
                }
                else
                {
                    IsLoading5 = true;
                }
            }
            else if (code.Equals("06"))
            {
                if (IsLoading6)
                {
                    return;
                }
                else
                {
                    IsLoading6 = true;
                }
            }
            else if (code.Equals("07"))
            {
                if (IsLoading7)
                {
                    return;
                }
                else
                {
                    IsLoading7 = true;
                }
            }
            else if (code.Equals("08"))
            {
                if (IsLoading8)
                {
                    return;
                }
                else
                {
                    IsLoading8 = true;
                }
            }
            else if (code.Equals("09"))
            {
                if (IsLoading9)
                {
                    return;
                }
                else
                {
                    IsLoading9 = true;
                }
            }
            else if (code.Equals("10"))
            {
                if (IsLoading10)
                {
                    return;
                }
                else
                {
                    IsLoading10 = true;
                }
            }
            else if (code.Equals("99"))
            {
                if (IsLoading99)
                {
                    return;
                }
                else
                {
                    IsLoading99 = true;
                }
            }
            DataGridView moduleGrid = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (MenuAction.Equals("Reports"))
            {
                moduleGrid.Rows.Clear();
                List<PrivilegeEntity> reportPrivileges = new List<PrivilegeEntity>();
                reportPrivileges = _model.NavigationData.GetReportsByUserID(code, SelectedUser, "Edit");

                foreach (PrivilegeEntity privilegesEntity in reportPrivileges)
                {
                    string view = "false";
                    if (ListOfSelectedPrivileges != null && ListOfSelectedPrivileges.Count > 0)
                    {
                        if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.ViewPriv.Contains("true")))
                        {
                            view = "true";
                        }
                    }
                    else
                    {
                        view = privilegesEntity.ViewPriv;
                    }
                    int rowIndex = moduleGrid.Rows.Add(code, privilegesEntity.ModuleName, false, privilegesEntity.PrivilegeName);
                    if (view == "true") moduleGrid.Rows[rowIndex].Cells["View"].Value = true; else moduleGrid.Rows[rowIndex].Cells["View"].Value = false;
                    moduleGrid.Rows[rowIndex].Tag = privilegesEntity;
                }
            }
            else
            {
                moduleGrid.Rows.Clear();
                string hierarchy = string.Empty;
                if (cmbHierarchy.Items.Count > 0)
                {
                    if (((ListItem)cmbHierarchy.SelectedItem).Value != null)
                    {
                        hierarchy = ((ListItem)cmbHierarchy.SelectedItem).Value.ToString();
                    }
                }

                //Added by Sudheer on 07/12/2022
                if (SelectedHierarchy.Count > 1 && SelectedUser != null && code != "01")
                {
                    pnlCopy.Visible = true;
                    TofillCopyFromHierarchysCombo(code, SelectedUser, hierarchy);
                }
                else pnlCopy.Visible = false;


                if (code.Equals("01"))
                {
                    hierarchy = "******";
                }
                List<PrivilegeEntity> screenPrivileges = new List<PrivilegeEntity>();
                screenPrivileges = _model.NavigationData.GetScreensByUserID(code, SelectedUser, "Edit", hierarchy);
                moduleGrid.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);

                int i = 0;
                foreach (PrivilegeEntity privilegesEntity in screenPrivileges)
                {
                    if (privilegesEntity.Program.ToString() != "ADMN0012")
                    {
                        i++;
                        if (i == 20)
                        {
                            string x = "0";
                        }
                        string viewPriv = "false";
                        string editPriv = "false";
                        string addPriv = "false";
                        string delPriv = "false";
                        string AllPriv = "false";

                        if (ListOfSelectedPrivileges != null && ListOfSelectedPrivileges.Count > 0)
                        {
                            if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.ViewPriv.Contains("true")))
                            {
                                viewPriv = "true";
                            }
                            if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.ChangePriv.Contains("true")))
                            {
                                editPriv = "true";
                            }
                            if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.AddPriv.Contains("true")))
                            {
                                addPriv = "true";
                            }
                            if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.DelPriv.Contains("true")))
                            {
                                delPriv = "true";
                            }
                        }
                        else
                        {
                            viewPriv = privilegesEntity.ViewPriv;
                            editPriv = privilegesEntity.ChangePriv;
                            addPriv = privilegesEntity.AddPriv;
                            delPriv = privilegesEntity.DelPriv;
                        }



                        string rowHierarchy = privilegesEntity.Hierarchy.Equals(string.Empty) ? hierarchy : privilegesEntity.Hierarchy;
                        int rowIndex = moduleGrid.Rows.Add(code, privilegesEntity.ModuleName, privilegesEntity.PrivilegeName, rowHierarchy, AllPriv, viewPriv, addPriv, editPriv, delPriv);
                        moduleGrid.Rows[rowIndex].Cells["All"].Tag = "All";
                        moduleGrid.Rows[rowIndex].Cells["View"].Tag = "View";
                        moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "Add";
                        moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "Change";
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "Delete";

                        if (viewPriv == "true") { moduleGrid.Rows[rowIndex].Cells["View"].Value = true; } else { moduleGrid.Rows[rowIndex].Cells["View"].Value = false; }
                        if (addPriv == "true") { moduleGrid.Rows[rowIndex].Cells["Add"].Value = true; } else { moduleGrid.Rows[rowIndex].Cells["Add"].Value = false; }
                        if (editPriv == "true") { moduleGrid.Rows[rowIndex].Cells["Change"].Value = true; } else { moduleGrid.Rows[rowIndex].Cells["Change"].Value = false; }
                        if (delPriv == "true") { moduleGrid.Rows[rowIndex].Cells["Delete"].Value = true; } else { moduleGrid.Rows[rowIndex].Cells["Delete"].Value = false; }

                        if (viewPriv == "true" && editPriv == "true" && addPriv == "true" && delPriv == "true")
                            moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                        else
                            moduleGrid.Rows[rowIndex].Cells["All"].Value = false;

                        moduleGrid.Rows[rowIndex].Tag = privilegesEntity;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "Delete";
                        string Img_Blank = "blank";
                        if (code.Equals("01"))
                        {
                            moduleGrid.Columns["Hierarchy"].Visible = false;
                        }
                        if (privilegesEntity.Program.Equals("ADMN0005") && code.Equals("01"))
                        {
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";

                            if (viewPriv == "true" && editPriv == "true" && addPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;

                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",D");

                        }
                        if (privilegesEntity.Program.Equals("SCHAUDIT"))
                        {
                            //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                            moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);

                            moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                            if (viewPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                            else
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = false;


                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",A D C");
                        }
                        if (privilegesEntity.Program.Equals("MAT00001"))
                        {
                            //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                            moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);

                            moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);


                            if (viewPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                            else
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = false;

                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",A D C");
                        }
                        if (privilegesEntity.Program.Equals("TRIGBULK"))
                        {
                            //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                            moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);

                            moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                            if (viewPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                            else
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = false;

                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",A D C");
                        }
                        if (privilegesEntity.Program.Equals("TMSTRIGG"))
                        {
                            //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";


                            moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";


                            moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";

                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
                            moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                            moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                            if (viewPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                            else
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = false;

                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",A D C");
                        }
                        if (privilegesEntity.Program.Equals("CASE9006"))
                        {
                            //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                            //moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                            //moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";

                            moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);


                            if (viewPriv == "true" && addPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                            else
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = false;


                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",D C");
                        }
                        //Added by Sudheer on 09/22/2021
                        if (privilegesEntity.Program.Equals("CASE9016"))
                        {
                            //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";

                            //moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                            //moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";

                            //moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                            //moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";

                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
                            // moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                            // moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);


                            if (viewPriv == "true" && editPriv == "true" && addPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                            else
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = false;

                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",D");
                        }
                        if (privilegesEntity.Program.Equals("TMS00085"))
                        {
                            //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";

                            moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";

                            moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                            moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";

                            moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
                            moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                            moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);


                            if (viewPriv == "true")
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = true;
                            else
                                moduleGrid.Rows[rowIndex].Cells["All"].Value = false;

                            strAllItems.Add(privilegesEntity.PrivilegeName.ToString() + ",A D C");
                        }
                    }
                }
                if (moduleGrid.Rows.Count > 0)
                    moduleGrid.Rows[0].Selected = true;

                moduleGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            }
        }

        /// <summary>
        /// Handles the grid value changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView hierarchicalGrid = sender as DataGridView;

            string _currentchkedcell = hierarchicalGrid.CurrentCell.Tag.ToString();

            string _ScreenName = hierarchicalGrid.CurrentRow.Cells["ScreenName"].Value.ToString();

            bool _notView = true;

            if (_currentchkedcell == "View")
            {
                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase))
                {
                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;
                    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value = false;
                    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value = false;
                    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value = false;
                }
                else
                {
                    //hierarchicalGrid.SelectedRows[0].Cells["Add"].Value = true;
                    //hierarchicalGrid.SelectedRows[0].Cells["Change"].Value = true;
                    //hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value = true;
                }
                _notView = false;
            }
            else if (_currentchkedcell == "All")
            {
                if (hierarchicalGrid.SelectedRows[0].Cells["All"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase))
                {
                    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = false;
                    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value = false;
                    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value = false;
                    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value = false;
                }
                else
                {
                    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value = true;
                    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value = true;
                    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value = true;
                }
            }


            if (_notView)
            {
                if (strAllItems.Count > 0)
                {


                    var _flag = strAllItems.FindAll(x => x.Contains(_ScreenName));
                    if (_flag.Count > 0)
                    {
                        var strarry = _flag[0].Split(',')[1].Split(' ');



                        if (strarry.Length == 1)
                        {

                            if (strarry[0].Trim().ToString() == "A")
                            {
                                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                       hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                       hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                                }
                                else
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;

                            }
                            if (strarry[0].Trim().ToString() == "D")
                            {
                                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                                }
                                else
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;





                                if (hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) ||
                                    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                                }
                            }
                            if (strarry[0].Trim().ToString() == "C")
                            {
                                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                                }
                                else
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;


                            }
                        }
                        if (strarry.Length == 2)
                        {
                            if ((strarry[0].Trim().ToString() == "A" && strarry[1].Trim().ToString() == "D") || (strarry[0].Trim().ToString() == "D" && strarry[1].Trim().ToString() == "A"))
                            {


                                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                                }
                                else
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;


                                if (hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                                }


                            }
                            if ((strarry[0].Trim().ToString() == "A" && strarry[1].Trim().ToString() == "C") || (strarry[0].Trim().ToString() == "C" && strarry[1].Trim().ToString() == "A"))
                            {


                                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                                }
                                else
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;


                                if (hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                                }
                            }
                            if ((strarry[0].Trim().ToString() == "D" && strarry[1].Trim().ToString() == "C") || (strarry[0].Trim().ToString() == "C" && strarry[1].Trim().ToString() == "D"))
                            {




                                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                                    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                                }
                                else
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;


                                if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                                }
                            }
                        }
                        if (strarry.Length == 3)
                        {
                            if (strarry[0].Trim().ToString() == "A" && strarry[1].Trim().ToString() == "D" && strarry[2].Trim().ToString() == "C")
                            {

                                if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;
                                else
                                    hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;
                            }
                        }


                        //if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) ||
                        //           hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) ||
                        //           hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                        //}
                        //else
                        //    hierarchicalGrid.SelectedRows[0].Cells["View"].Value = false;
                    }
                    else
                    {


                        if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase) &&
                        hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase) &&
                        hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase)
                        )
                        {
                            hierarchicalGrid.SelectedRows[0].Cells["View"].Value = false;
                        }
                        else
                        {
                            hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                        }




                        if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                      hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                      hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase)
                      )
                        {
                            hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                        }
                        else
                            hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;

                    }

                }
                else
                {


                    if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase) &&
                    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase) &&
                    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase)
                    )
                    {
                        hierarchicalGrid.SelectedRows[0].Cells["View"].Value = false;
                    }
                    else
                    {
                        hierarchicalGrid.SelectedRows[0].Cells["View"].Value = true;
                    }




                    if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                  hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) &&
                  hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase)
                  )
                    {
                        hierarchicalGrid.SelectedRows[0].Cells["All"].Value = true;

                    }
                    else
                        hierarchicalGrid.SelectedRows[0].Cells["All"].Value = false;

                }
            }

            //int introwindex = hierarchicalGrid.CurrentCell.RowIndex;
            // hierarchicalGrid.Rows[introwindex].Selected = true;




            //hierarchicalGrid.Rows[0].Selected = true;

            //if (hierarchicalGrid.SelectedRows[0].Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    hierarchicalGrid.SelectedRows[0].Cells["Add"].Value = "false";
            //    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value = "false";
            //    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value = "false";
            //}
            //else if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) ||
            //    hierarchicalGrid.SelectedRows[0].Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) ||
            //    hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase)
            //    )
            //{

            //    if (hierarchicalGrid.SelectedRows[0].Cells["Delete"].Tag == "NOTDelete")
            //    {
            //        if (e.ColumnIndex == 7)
            //        {
            //            hierarchicalGrid.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            //            CommonFunctions.MessageBoxDisplay("Delete privileges not available for this screen.");
            //            hierarchicalGrid.SelectedRows[0].Cells["Delete"].Value = "false";
            //            hierarchicalGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            //        }
            //    }
            //    else
            //    {
            //        hierarchicalGrid.SelectedRows[0].Cells["View"].Value = "true";
            //    }

            //    if (hierarchicalGrid.SelectedRows[0].Cells["Change"].Tag == "NOTDelete")
            //    {
            //        if (e.ColumnIndex == 5)
            //        {
            //            hierarchicalGrid.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            //            CommonFunctions.MessageBoxDisplay("Change privileges not available for this screen.");
            //            hierarchicalGrid.SelectedRows[0].Cells["Change"].Value = "false";
            //            hierarchicalGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            //        }
            //    }
            //    if (hierarchicalGrid.SelectedRows[0].Cells["Add"].Tag == "NOTDelete")
            //    {
            //        if (e.ColumnIndex == 4)
            //        {
            //            hierarchicalGrid.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            //            CommonFunctions.MessageBoxDisplay("Add privileges not available for this screen.");
            //            hierarchicalGrid.SelectedRows[0].Cells["Add"].Value = "false";
            //            hierarchicalGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            //        }
            //    }


            //}

            hierarchicalGrid.Update();
            hierarchicalGrid.ResumeLayout();
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            DataGridView moduleGrid = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (moduleGrid.Rows.Count > 0)
            {
                contextMenu1.MenuItems.Clear();
                MenuItem menuLst = new MenuItem();
                menuLst.Text = "Select All";
                menuLst.Tag = "A";
                contextMenu1.MenuItems.Add(menuLst);
                MenuItem menuLst2 = new MenuItem();
                menuLst2.Text = "Unselect All";
                menuLst2.Tag = "U";
                contextMenu1.MenuItems.Add(menuLst2);
            }
        }

        private void DataGrid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            DataGridView moduleGrid = tabControl1.SelectedTab.Controls[0] as DataGridView;
            moduleGrid.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            if (moduleGrid.Rows.Count > 0)
            {
                if (objArgs.MenuItem.Tag == "A")
                {
                    moduleGrid.SelectedRows[0].Cells["View"].Value = "true";
                    moduleGrid.SelectedRows[0].Cells["Add"].Value = "true";
                    moduleGrid.SelectedRows[0].Cells["Change"].Value = "true";
                    if (moduleGrid.SelectedRows[0].Cells["Delete"].Tag == "NOTDelete")
                    {
                        moduleGrid.SelectedRows[0].Cells["Delete"].Value = "false";
                    }
                    else
                    {
                        moduleGrid.SelectedRows[0].Cells["Delete"].Value = "true";
                    }
                    if (moduleGrid.SelectedRows[0].Cells["Change"].Tag == "NOTDelete")
                    {
                        moduleGrid.SelectedRows[0].Cells["Change"].Value = "false";
                    }
                    else
                    {
                        moduleGrid.SelectedRows[0].Cells["Change"].Value = "true";
                    }
                    if (moduleGrid.SelectedRows[0].Cells["Add"].Tag == "NOTDelete")
                    {
                        moduleGrid.SelectedRows[0].Cells["Add"].Value = "false";
                    }
                    else
                    {
                        moduleGrid.SelectedRows[0].Cells["Add"].Value = "true";
                    }


                }
                else
                {
                    moduleGrid.SelectedRows[0].Cells["View"].Value = "false";
                    moduleGrid.SelectedRows[0].Cells["Add"].Value = "false";
                    moduleGrid.SelectedRows[0].Cells["Change"].Value = "false";
                    moduleGrid.SelectedRows[0].Cells["Delete"].Value = "false";

                }
            }
            moduleGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;
            string code = tabControl1.SelectedTab.Tag as string;
            DataGridView moduleGrid = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (MenuAction.Equals("Reports"))
            {
                foreach (DataGridViewRow privilegesrow in moduleGrid.Rows)
                {
                    privilegesrow.Cells[2].Value = true;
                }
            }
            else
            {

                string hierarchy = string.Empty;

                moduleGrid.CurrentCell = moduleGrid[4, 0];

                foreach (DataGridViewRow privilegesrow in moduleGrid.Rows)
                {

                    bool editPriv = true;
                    bool addPriv = true;
                    bool delPriv = true;
                    if (privilegesrow.Cells[1].Value.ToString().Contains("Assign User Account & Privileges") && code.Equals("01"))
                    {
                        delPriv = false;

                    }
                    if (privilegesrow.Cells[1].Value.ToString().Contains("Scheduler Audit"))
                    {
                        addPriv = false;
                        editPriv = false;
                        delPriv = false;


                    }
                    if (privilegesrow.Cells[1].Value.ToString().Contains("Matrix/Scales Definitions"))
                    {
                        addPriv = false;
                        editPriv = false;
                        delPriv = false;

                    }
                    if (privilegesrow.Cells[1].Value.ToString().Contains("Trigger Post"))
                    {
                        addPriv = false;
                        editPriv = false;
                        delPriv = false;

                    }
                    if (privilegesrow.Cells[1].Value.ToString().Contains("Trigger Control Definition"))
                    {
                        addPriv = false;
                        editPriv = false;
                        delPriv = false;

                    }


                    privilegesrow.Cells[4].Value = true;
                    privilegesrow.Cells[5].Value = true;
                    privilegesrow.Cells[6].Value = addPriv;
                    privilegesrow.Cells[7].Value = editPriv;
                    privilegesrow.Cells[8].Value = delPriv;
                }
            }

        }

        private void TofillCopyFromHierarchysCombo(string Module, string User, string Hie)
        {
            List<PrivilegeEntity> screenPrivileges = new List<PrivilegeEntity>();

            cmbCopyHie.Items.Clear();
            cmbCopyHie.SelectedIndexChanged -= new System.EventHandler(this.cmbCopyHie_SelectedIndexChanged);
            cmbCopyHie.ColorMember = "FavoriteColor";
            if (SelectedHierarchy.Count > 0)
            {
                ListItem li1 = new ListItem("", "000000", "N", Color.White);
                cmbCopyHie.Items.Add(li1);
                foreach (HierarchyEntity hierarchyEntity in SelectedHierarchy)
                {
                    if (hierarchyEntity.Code.Replace("-", string.Empty) != Hie)
                    {
                        screenPrivileges = _model.NavigationData.GetScreensByUserID(Module, SelectedUser, "View", hierarchyEntity.Code.Replace("-", string.Empty));
                        if (screenPrivileges.Count > 0)
                        {
                            //screenPrivileges = screenPrivileges.FindAll(u => u.Hierarchy != Hie && u.ViewPriv == "Y");

                            PrivilegeEntity Entity = screenPrivileges.Find(u => u.Hierarchy == hierarchyEntity.Agency + hierarchyEntity.Dept + hierarchyEntity.Prog);
                            if (Entity != null)
                            {
                                ListItem li = new ListItem(hierarchyEntity.Code, (hierarchyEntity.Code.Replace("-", string.Empty)), hierarchyEntity.UsedFlag, hierarchyEntity.UsedFlag.Equals("Y") ? Color.Red : Color.Black);
                                cmbCopyHie.Items.Add(li);
                                if (hierarchyEntity.UsedFlag.Equals("N"))
                                {

                                    cmbCopyHie.SelectedItem = li;
                                }
                            }
                        }
                    }
                }

                if (cmbCopyHie.Items.Count > 1)
                    cmbCopyHie.SelectedIndex = 0;
                else
                {
                    pnlCopy.Visible = false;
                }
                cmbCopyHie.SelectedIndexChanged += new System.EventHandler(this.cmbCopyHie_SelectedIndexChanged);

            }
        }

        private void cmbCopyHie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCopyHie.Items.Count > 1 && chkbCopyFrom.Checked)
            {
                //if(((ListItem)cmbCopyHie.SelectedItem).Value.ToString()!="000000")
                //{
                string code = tabControl1.SelectedTab.Tag as string;
                DataGridView moduleGrid = tabControl1.SelectedTab.Controls[0] as DataGridView;

                string Hierarchy = string.Empty;
                if (((ListItem)cmbCopyHie.SelectedItem).Value.ToString() == "000000") Hierarchy = ((ListItem)cmbHierarchy.SelectedItem).Value.ToString();
                else Hierarchy = ((ListItem)cmbCopyHie.SelectedItem).Value.ToString();

                moduleGrid.Rows.Clear();

                List<PrivilegeEntity> screenPrivileges = new List<PrivilegeEntity>();
                screenPrivileges = _model.NavigationData.GetScreensByUserID(code, SelectedUser, "Edit", Hierarchy);
                moduleGrid.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);

                foreach (PrivilegeEntity privilegesEntity in screenPrivileges)
                {
                    string AllPriv = "false";
                    string viewPriv = "false";
                    string editPriv = "false";
                    string addPriv = "false";
                    string delPriv = "false";
                    if (ListOfSelectedPrivileges != null && ListOfSelectedPrivileges.Count > 0)
                    {
                        if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.ViewPriv.Contains("true")))
                        {
                            viewPriv = "true";
                        }
                        if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.ChangePriv.Contains("true")))
                        {
                            editPriv = "true";
                        }
                        if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.AddPriv.Contains("true")))
                        {
                            addPriv = "true";
                        }
                        if (ListOfSelectedPrivileges.Exists(u => u.Program.Equals(privilegesEntity.Program) && u.ModuleCode.Equals(privilegesEntity.ModuleCode) && u.Hierarchy.Equals(privilegesEntity.Hierarchy) && u.DelPriv.Contains("true")))
                        {
                            delPriv = "true";
                        }

                        if (viewPriv == "true" && editPriv == "true" && addPriv == "true" && delPriv == "true")
                            AllPriv = "true";

                    }
                    else
                    {
                        viewPriv = privilegesEntity.ViewPriv;
                        editPriv = privilegesEntity.ChangePriv;
                        addPriv = privilegesEntity.AddPriv;
                        delPriv = privilegesEntity.DelPriv;
                    }

                    if (!privilegesEntity.Hierarchy.Equals(string.Empty) && !privilegesEntity.Hierarchy.Equals(((ListItem)cmbHierarchy.SelectedItem).Value.ToString()))
                    {
                        privilegesEntity.Hierarchy = ((ListItem)cmbHierarchy.SelectedItem).Value.ToString();

                    }

                    string rowHierarchy = privilegesEntity.Hierarchy.Equals(string.Empty) ? Hierarchy : privilegesEntity.Hierarchy;
                    int rowIndex = moduleGrid.Rows.Add(code, privilegesEntity.ModuleName, privilegesEntity.PrivilegeName, rowHierarchy, AllPriv, viewPriv, addPriv, editPriv, delPriv);
                    moduleGrid.Rows[rowIndex].Tag = privilegesEntity;
                    moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "Delete";
                    moduleGrid.Rows[rowIndex].Cells["All"].Tag = "All";
                    moduleGrid.Rows[rowIndex].Cells["View"].Tag = "View";
                    moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "Add";
                    moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "Change";
                    //moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "Delete";
                    string Img_Blank = "blank";
                    if (code.Equals("01"))
                    {
                        moduleGrid.Columns["Hierarchy"].Visible = false;
                    }
                    if (privilegesEntity.Program.Equals("ADMN0005") && code.Equals("01"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";

                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                        //moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";

                    }
                    if (privilegesEntity.Program.Equals("SCHAUDIT"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                        moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);

                        moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                    }
                    if (privilegesEntity.Program.Equals("MAT00001"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                        moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);

                        moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                    }
                    if (privilegesEntity.Program.Equals("TRIGBULK"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                        moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);

                        moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                    }
                    if (privilegesEntity.Program.Equals("TMSTRIGG"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";


                        moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";


                        moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";

                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
                        moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                        moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                    }
                    if (privilegesEntity.Program.Equals("CASE9006"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);

                        //moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Style.ForeColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";

                        moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";
                        moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                    }
                    //Added by Sudheer on 09/22/2021
                    if (privilegesEntity.Program.Equals("CASE9016"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";

                        //moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Style.ForeColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";

                        //moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";

                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                    }
                    if (privilegesEntity.Program.Equals("TMS00085"))
                    {
                        //  moduleGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Delete"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Delete"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Delete"].Tag = "NOTDelete";

                        moduleGrid.Rows[rowIndex].Cells["Add"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.BackColor = Color.LightGray;
                        // moduleGrid.Rows[rowIndex].Cells["Add"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Add"].Tag = "NOTDelete";

                        moduleGrid.Rows[rowIndex].Cells["Change"].Value = "false";
                        // moduleGrid.Rows[rowIndex].Cells["Change"].Style.BackColor = Color.LightGray;
                        //moduleGrid.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.LightGray;
                        moduleGrid.Rows[rowIndex].Cells["Change"].Tag = "NOTDelete";

                        moduleGrid.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
                        moduleGrid.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                        moduleGrid.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(moduleGrid.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);

                    }
                }
                if (moduleGrid.Rows.Count > 0)
                    moduleGrid.Rows[0].Selected = true;
                moduleGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
                //}


            }
        }

        private void chkbCopyFrom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbCopyFrom.Checked)
                cmbCopyHie.Visible = true;
            else
                cmbCopyHie.Visible = false;
        }
    }
}
