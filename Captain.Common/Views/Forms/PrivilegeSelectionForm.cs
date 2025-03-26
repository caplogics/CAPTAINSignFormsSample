#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Captain.Common.Utilities;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.CaptainFaults;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Model.Data;
using System.Data.SqlClient;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PrivilegeSelectionForm : Form
    {
        private ErrorProvider _errorProvider = null;
        private List<UserEntity> _selectedUser = null;
        private CaptainModel _model = null;

        public PrivilegeSelectionForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="baseForm"></param>
        public PrivilegeSelectionForm(BaseForm baseForm,PrivilegeEntity privilege)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;

                privilegeEntity = privilege;
                _model = new CaptainModel();
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(privilegeEntity.UserID);
                string Agency = string.Empty;
                if(userHierarchy.Count> 0)
                {
                    userHierarchy = userHierarchy.FindAll(u => u.InActiveFlag == "N");
                    HierarchyEntity UserEntity = userHierarchy.Find(u => u.Agency == "**");
                    if (UserEntity != null) { Agency = string.Empty; } else Agency = BaseForm.BaseAdminAgency;
                }

                
                List<UserEntity> userEntityList = _model.UserProfileAccess.GetTemplateUsers(Agency);

                //if(userEntityList.Count>0)
                //{
                //    if(BaseForm.BaseAdminAgency!="**")
                //        userEntityList= userEntityList.FindAll(u=>u.Agency==BaseForm.BaseAdminAgency);
                //}

                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                foreach (UserEntity userEntity in userEntityList)
                {
                    int rowIndex = gvwPrivileges.Rows.Add("false", userEntity.UserID);
                    gvwPrivileges.Rows[rowIndex].Tag = userEntity;
                }
 
                AddGridEventHandles();
                _errorProvider = new ErrorProvider(this);
                _errorProvider.BlinkRate = 3;
                _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
                _errorProvider.Icon = null;
            }
            catch (Exception)
            {
            }
        }

        #region Properties

        public TagClass SelectedNodeTagClass { get; set; }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity privilegeEntity { get; set; }

        public string HierarchyType { get; set; }

        public DataGridView HierarchieGrid
        {
            get { return gvwPrivileges; }
        }

        public DataGridViewRow HierarchieGridRow
        {
            get;
            set;
        }

        public List<UserEntity> SelectedUser
        {
            get
            {
                return _selectedUser = (from c in gvwPrivileges.Rows.Cast<DataGridViewRow>().ToList()
                                                   where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                               select ((DataGridViewRow)c).Tag as UserEntity).ToList();

            }
        }

        public string SelectedItem
        {
            get;
            set;
        }

        public List<RowState> ExpandedRows { get; set; }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private bool ValidateForm()
        {
            bool isValid = true;

            if (gvwPrivileges.Rows.Count == 0)
            {
                   _errorProvider.SetError(gvwPrivileges, Consts.Messages.PleaseSelectAtLeastOneItem.GetMessage());
                    isValid = false;
            }
            else
            {
                _errorProvider.SetError(gvwPrivileges, Convert.ToString(""));
            }
            return (isValid);
        }

        /// <summary>
        /// Set the display value of a comboBox cell.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="comboBoxCell"></param>
        private void SetComboBoxDisplayValue(TagClass node, DataGridViewComboBoxCell comboBoxCell)
        {
            if (comboBoxCell.Value != null && !string.IsNullOrEmpty(comboBoxCell.Value.ToString()))
            {

                comboBoxCell.ToolTipText = comboBoxCell.Value.ToString();
            }
        }

        /// <summary>
        /// Add the columns to the grid.
        /// </summary>
        /// <param name="dataGridView"></param>
        private void AddGridColumns(DataGridView dataGridView)
        {
            DataGridViewCheckBoxColumn dataTypeColumn = null;
            string columnName = string.Empty;

            gvwPrivileges.Columns.Clear();
            // 
            // DatatypeColumn

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "Select";
            dataTypeColumn.Name = "Select";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 40;
            dataGridView.Columns.Add(dataTypeColumn);

            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = "User ID";
            descColumn.Name = "UserID";
            descColumn.ReadOnly = true;
            descColumn.Width = 350;
            dataGridView.Columns.Add(descColumn);
        }

        /// <summary>
        /// Loops through cells in a row and set options like readonly, values and other.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="row"></param>
        private void SetCellOptions(TagClass node, DataGridViewRow row)
        {
            var cells = (from c in row.Cells.Cast<DataGridViewCell>().ToList()
                         select c).ToList();

            for (int cellIndex = 0; cellIndex < cells.Count; cellIndex++)
            {
                DataGridViewCell cell = cells[cellIndex];
                if (cell.GetType().Name.Equals(Consts.CellTypes.DataGridViewComboBoxCell))
                {
                    DataGridViewComboBoxCell comboBoxCell = cell as DataGridViewComboBoxCell;
                    //comboBoxCell.BeforeClick -= DataGridViewBeforeClick;
                    //comboBoxCell.BeforeClick += DataGridViewBeforeClick;
                    break;
                }
            }
        }

        /// <summary>
        /// Get list of onodes
        /// </summary>
        /// <param name="tagClass"></param>
        /// <param name="useCachedValues"></param>
        /// <returns></returns>
        private List<TagClass> GetChildrenNodes(TagClass tagClass, bool useCachedValues)
        {
            List<string> attributes = new List<string>();
            List<TagClass> nodeTypeList = new List<TagClass>();
            return nodeTypeList;
        }

        /// <summary>
        /// Adds the event handlers to the grid
        /// </summary>
        private void AddGridEventHandles()
        {
            gvwPrivileges.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(DataGridViewDataError);
            gvwPrivileges.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
        }

        /// <summary>
        /// Removes the event handlers from the grid.
        /// </summary>
        private void RemoveGridEventHandles()
        {
            gvwPrivileges.DataError -= new DataGridViewDataErrorEventHandler(DataGridViewDataError);
            gvwPrivileges.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
        }
  
        #endregion

        #region Handled Events

        /// <summary>
        /// Handles the grid value changed event and keeps track of the changes made on the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView hierarchicalGrid = sender as DataGridView;
            string selectedUser = hierarchicalGrid.SelectedRows[0].Cells["UserID"].Value.ToString();
 
            if (!selectedUser.Equals(string.Empty))
            {
                foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
                {
                    string rowCode = dr.Cells["UserID"].Value.ToString();
                    if (!rowCode.Equals(selectedUser))
                    {
                            dr.Cells["Select"].Value = "false";
                    }      
                }
                hierarchicalGrid.Update();
                hierarchicalGrid.ResumeLayout();
            }
        }

        /// <summary>
        /// Handles the grid DataError event to prevent error messages in the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewDataError(object sender, Wisej.Web.DataGridViewDataErrorEventArgs e)
        {
            //DO NOTHING HERE
        }

        /// <summary>
        /// Handles grid before click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewBeforeClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)sender;
            
        }

        /// <summary>
        /// Handles OK click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClick(object sender, EventArgs e)
        {
            if (ValidateForm())
            {  
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Handles Cancel click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN00005_temp");
        }

        private void PrivilegeSelectionForm_Load(object sender, EventArgs e)
        {

        }

        private void PrivilegeSelectionForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "TL_HELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps("ADMN0005", 3, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }
    }
}