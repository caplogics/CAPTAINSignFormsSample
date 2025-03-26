#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;

//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.CaptainFaults;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Model.Data;
using System.Data.SqlClient;
using Wisej.Web;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HierarchieSelectionForm : Form
    {
        private ErrorProvider _errorProvider = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        private CaptainModel _model = null;
        public HierarchieSelectionForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with parameters. Hierachys display with agency and ** records two grids.
        /// Using this form AddUserForm 
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm, List<HierarchyEntity> hierarchy, string selectedType, UserEntity userProfile, string mode)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                UserProfile = userProfile;
                BaseForm = baseForm;
                ListOfSelectedHierarchies = hierarchy;
                Mode = mode;
                string inTake = "I";
                this.Text = "Hierarchy Selection";
                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(UserProfile != null ? UserProfile.UserID : null, HierarchyType,string.Empty);

                HierarchyEntity hierarchyAll = new HierarchyEntity();
                hierarchyAll.Code = "**-**-**";
                hierarchyAll.HirarchyName = "All Hierarchies";
                caseHierarchy.Insert(0, hierarchyAll);
                int rowIndex = 0; //gvwHierarchie.Rows.Add("false", hierarchyAll.Code, hierarchyAll.HirarchyName);
                //gvwHierarchie.Rows[rowIndex].Tag = hierarchyAll;
                if (ListOfSelectedHierarchies != null && ListOfSelectedHierarchies.Count > 0)
                {
                    caseHierarchy.ForEach(item => item.InActiveFlag = (ListOfSelectedHierarchies.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)))) ? "true" : "false");
                }
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    //string isInActive = "false";
                    //if (ListOfSelectedHierarchies != null && ListOfSelectedHierarchies.Count > 0)
                    //{
                    //    if (ListOfSelectedHierarchies.Exists(u => u.Code.Replace("-", string.Empty).Equals(hierarchyEntity.Code.Replace("-", string.Empty))))
                    //    {
                    //        isInActive = "true";
                    //    }
                    //}
                    //else
                    //{
                    //    isInActive = hierarchyEntity.InActiveFlag.Equals("N") ? "true" : "false";
                    //}


                    rowIndex = gvwHierarchie.Rows.Add(hierarchyEntity.InActiveFlag, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                }
                foreach (HierarchyEntity hierarchyEntity in ListOfSelectedHierarchies)
                {
                    string strAgency = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency;
                    string strDept = hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept;
                    string strProgram = hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                    //string code = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency + "-" + hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept + "-" + hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                    string code = strAgency + "-" + strDept + "-" + strProgram;
                    if (hierarchyEntity.Code == "**-**-**")
                    {
                        code = "**-**-**"; string Name = "All Hierarchies";
                        rowIndex = gvwSelectedHierarachies.Rows.Add(code, Name);
                    }
                    else
                        rowIndex = gvwSelectedHierarachies.Rows.Add(code, hierarchyEntity.HirarchyName);
                    gvwSelectedHierarachies.Rows[rowIndex].Tag = hierarchyEntity;
                }
                EnableDisableCheckBox();

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

        /// <summary>
        /// Hierachys display with agency with out **  records two grids.selected two grids records
        /// Using this form Add programdefinition 
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm, List<HierarchyEntity> hierarchy, string selectedType, UserEntity userProfile, string mode,string filterType)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                UserProfile = userProfile;
                BaseForm = baseForm;
                ListOfSelectedHierarchies = hierarchy;
                Mode = mode;
                string inTake = "I";
                this.Text = "Hierarchy Selection";
                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(UserProfile != null ? UserProfile.UserID : null, HierarchyType, string.Empty);
                                
                int rowIndex = 0; 
                if (ListOfSelectedHierarchies != null && ListOfSelectedHierarchies.Count > 0)
                {
                    caseHierarchy.ForEach(item => item.InActiveFlag = (ListOfSelectedHierarchies.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)))) ? "true" : "false");
                }
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    
                    if (!hierarchyEntity.Code.Contains('*'))
                    {
                        rowIndex = gvwHierarchie.Rows.Add(hierarchyEntity.InActiveFlag, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                        gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                    }
                }
                foreach (HierarchyEntity hierarchyEntity in ListOfSelectedHierarchies)
                {
                    string strAgency = hierarchyEntity.Agency;
                    string strDept = hierarchyEntity.Dept;
                    string strProgram =  hierarchyEntity.Prog;                   
                    string code = strAgency + "-" + strDept + "-" + strProgram;
                    rowIndex = gvwSelectedHierarachies.Rows.Add(code, hierarchyEntity.HirarchyName);
                    gvwSelectedHierarachies.Rows[rowIndex].Tag = hierarchyEntity;
                }
                EnableDisableCheckBox();

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

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm, List<HierarchyEntity> hierarchy, string selectedType)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                ListOfSelectedHierarchies = hierarchy;
                Mode = "View";
                string inTake = "I";
                this.Text = "Hierarchy Selection";
                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(UserProfile != null ? UserProfile.UserID : null, HierarchyType,string.Empty);
                HierarchyEntity hierarchyAll = new HierarchyEntity();
                hierarchyAll.Code = "**-**-**";
                hierarchyAll.HirarchyName = "All Hierarchies";
                int rowIndex = gvwHierarchie.Rows.Add("false", hierarchyAll.Code, hierarchyAll.HirarchyName);
                gvwHierarchie.Rows[rowIndex].Tag = hierarchyAll;
                foreach (HierarchyEntity hierarchyEntity in ListOfSelectedHierarchies)
                {
                    rowIndex = gvwSelectedHierarachies.Rows.Add(hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    gvwSelectedHierarachies.Rows[rowIndex].Tag = hierarchyEntity;
                }
                gvwSelectedHierarachies.Update();
                gvwSelectedHierarachies.ResumeLayout();
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    string isInActive = hierarchyEntity.InActiveFlag.Equals("N") ? "true" : "false";
                    rowIndex = gvwHierarchie.Rows.Add(isInActive, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                }
                gvwHierarchie.Update();
                gvwHierarchie.ResumeLayout();


                EnableDisableCheckBox();

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


        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm, string hierarchy, string strCode)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                Mode = "Program";
                string inTake = "I";
                this.Text = "Hierarchy Selection";

                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(null, HierarchyType,string.Empty);
                HierarchyEntity hierarchyAll = new HierarchyEntity();
                hierarchyAll.Code = "**-**-**";
                hierarchyAll.HirarchyName = "All Hierarchies";
                int rowIndex = gvwHierarchie.Rows.Add("false", hierarchyAll.Code, hierarchyAll.HirarchyName);
                gvwHierarchie.Rows[rowIndex].Tag = hierarchyAll;

                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    if (hierarchyEntity.Code == strCode)
                    {
                        rowIndex = gvwHierarchie.Rows.Add(true, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    }
                    // string isInActive = hierarchyEntity.InActiveFlag.Equals("N") ? "true" : "false";
                    else
                    {
                        rowIndex = gvwHierarchie.Rows.Add(false, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    }
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                }
                EnableDisableCheckBox();
                gvwSelectedHierarachies.Visible = false;
                lblSelected.Visible = false;
                lblChoose.Location = new Point(12, 6);
                gvwHierarchie.Location = new Point(9, 25);
                gvwHierarchie.Size = new System.Drawing.Size(413, 350);
                gvwHierarchie.DataError += new DataGridViewDataErrorEventHandler(DataGridViewDataError);
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


        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm, string hierarchy, string strCode, string strType)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                Mode = "Program";
                string inTake = "I";
                Prog_Multiple_sel = strType;
                this.Text = "Hierarchy Selection";
                lblChoose.Text = "Choose Hierarchy Here";
                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(null, HierarchyType,string.Empty);
                int rowIndex;
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    if (!hierarchyEntity.Code.Contains('*'))
                    {
                        if (hierarchyEntity.Code == strCode)
                        {
                            rowIndex = gvwHierarchie.Rows.Add(true, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                        }
                        // string isInActive = hierarchyEntity.InActiveFlag.Equals("N") ? "true" : "false";
                        else
                        {
                            rowIndex = gvwHierarchie.Rows.Add(false, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                        }
                        gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                    }
                }
                EnableDisableCheckBox();
                gvwSelectedHierarachies.Visible = false;
                lblSelected.Visible = false;
                lblChoose.Location = new Point(12, 6);
                gvwHierarchie.Location = new Point(9, 25);
                gvwHierarchie.Size = new System.Drawing.Size(413, 350);
               gvwHierarchie.DataError += new DataGridViewDataErrorEventHandler(DataGridViewDataError);
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

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="baseForm"></param>
        ///For Displaying the hierarachies only in Program Definitions 

        public HierarchieSelectionForm(BaseForm baseForm, string hierarchy, string strCode, string strType,string strdep,string strnull)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                Mode = "Program";
                string inTake = "I";
                Prog_Multiple_sel = strType;
                this.Text = "Hierarchy Selection";
                lblChoose.Text = "Choose Hierarchy Here";
                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(null, HierarchyType, string.Empty);
                int rowIndex=0;
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    if (!hierarchyEntity.Code.Contains('*'))
                    {
                        ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(hierarchyEntity.Agency, hierarchyEntity.Dept, hierarchyEntity.Prog);
                        if (programEntity != null)
                        {
                            if (hierarchyEntity.Code == strCode)
                            {
                                rowIndex = gvwHierarchie.Rows.Add(true, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                                    //Get_ClientIntake_Priv();
                            }
                            else
                            {
                                    rowIndex = gvwHierarchie.Rows.Add(false, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                                    //Get_ClientIntake_Priv();
                            }
                            gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                        }
                    }
                }
                EnableDisableCheckBox();
                gvwSelectedHierarachies.Visible = false;
                lblSelected.Visible = false;
                lblChoose.Location = new Point(12, 6);
                gvwHierarchie.Location = new Point(9, 25);
                gvwHierarchie.Size = new System.Drawing.Size(413, 350);
                gvwHierarchie.DataError += new DataGridViewDataErrorEventHandler(DataGridViewDataError);
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




        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm, string hierarchy)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                Mode = "Program";
                string inTake = "I";
                this.Text = "Hierarchy Selection";

                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(null, HierarchyType,string.Empty);
                HierarchyEntity hierarchyAll = new HierarchyEntity();
                hierarchyAll.Code = "**-**-**";
                hierarchyAll.HirarchyName = "All Hierarchies";
                int rowIndex = gvwHierarchie.Rows.Add("false", hierarchyAll.Code, hierarchyAll.HirarchyName);
                gvwHierarchie.Rows[rowIndex].Tag = hierarchyAll;
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    string isInActive = hierarchyEntity.InActiveFlag.Equals("N") ? "true" : "false";
                    rowIndex = gvwHierarchie.Rows.Add(isInActive, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                }
                gvwSelectedHierarachies.Visible = false;
                lblSelected.Visible = false;
                lblChoose.Location = new Point(12, 6);
                gvwHierarchie.Location = new Point(9, 25);
                gvwHierarchie.Size = new System.Drawing.Size(413, 350);
                gvwHierarchie.DataError += new DataGridViewDataErrorEventHandler(DataGridViewDataError);
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

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                Mode = "Program";
                this.Text = "Program Hierarchy Selection";

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy(string.Empty);

                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    //string isInActive = hierarchyEntity.InActiveFlag.Equals("N") ? "true" : "false";
                    int rowIndex = gvwHierarchie.Rows.Add("false", hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                }
                gvwSelectedHierarachies.Visible = false;
                lblSelected.Visible = false;
                lblChoose.Location = new Point(12, 6);
                gvwHierarchie.Location = new Point(9, 25);
                gvwHierarchie.Size = new System.Drawing.Size(413, 350);
                gvwHierarchie.DataError += new DataGridViewDataErrorEventHandler(DataGridViewDataError);
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

        /// <summary>
        /// Constructor with parameters. All Hierachys display single grid only single  hierachy selected.
        /// Using this form MasterPoverityGuidelinecontrol
        /// </summary>
        /// <param name="baseForm"></param>
        public HierarchieSelectionForm(BaseForm baseForm, string hierarchy, string strCode, string strType,string FormType)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                Mode = "Program";
                string inTake = "I";
                this.Text = "Hierarchy Selection";

                HierarchyType = inTake;

                _model = new CaptainModel();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, HierarchyType, FormType);
                //HierarchyEntity hierarchyAll = new HierarchyEntity();
                //hierarchyAll.Code = "**-**-**";
                //hierarchyAll.HirarchyName = "All Hierarchies";
                //int rowIndex = gvwHierarchie.Rows.Add("false", hierarchyAll.Code, hierarchyAll.HirarchyName);
                //gvwHierarchie.Rows[rowIndex].Tag = hierarchyAll;
                int rowIndex = 0;
                foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                {
                    if (hierarchyEntity.Code == strCode)
                    {
                        rowIndex = gvwHierarchie.Rows.Add(true, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    }
                    // string isInActive = hierarchyEntity.InActiveFlag.Equals("N") ? "true" : "false";
                    else
                    {
                        rowIndex = gvwHierarchie.Rows.Add(false, hierarchyEntity.Code, hierarchyEntity.HirarchyName);
                    }
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                }
                //EnableDisableCheckBox();
                gvwSelectedHierarachies.Visible = false;
                lblSelected.Visible = false;
                lblChoose.Location = new Point(12, 6);
                gvwHierarchie.Location = new Point(9, 25);
                gvwHierarchie.Size = new System.Drawing.Size(413, 350);
                gvwHierarchie.DataError += new DataGridViewDataErrorEventHandler(DataGridViewDataError);
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

        public string HierarchyType { get; set; }

        public DataGridView HierarchieGrid
        {
            get { return gvwHierarchie; }
        }

        public List<HierarchyEntity> ListOfSelectedHierarchies
        {
            get;
            set;
        }

        public UserEntity UserProfile { get; set; }

        public string Mode { get; set; }

        public string Prog_Multiple_sel { get; set; }

        public DataGridViewRow HierarchieGridRow
        {
            get;
            set;
        }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                return _selectedHierarchies = (from c in gvwHierarchie.Rows.Cast<DataGridViewRow>().ToList()
                                               where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();

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

            if (gvwHierarchie.Rows.Count > 0)
            {
                int invalidRecords = 0;
                foreach (DataGridViewRow item in gvwHierarchie.Rows)
                {
                    if (string.IsNullOrEmpty(item.Cells[1].Value.ToString().Trim()))
                    {
                        invalidRecords++;
                        break;
                    }
                }
                if (invalidRecords > 0)
                {
                    //_errorProvider.SetError(gvwConvertObject, Consts.Messages.AllObjectTypeSelectionRequired.GetMessage());
                    isValid = false;
                }
                else
                {
                    //_errorProvider.SetError(gvwHierarchie, Convert.ToString(""));
                }
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

            gvwHierarchie.Columns.Clear();
            // 
            // DatatypeColumn

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "Select";
            dataTypeColumn.Name = "Select";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.Automatic;
            dataTypeColumn.Width = 70;
            dataTypeColumn.ShowInVisibilityMenu = false;
            dataGridView.Columns.Add(dataTypeColumn);

            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn();
            codeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            codeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            codeColumn.CellTemplate = new DataGridViewTextBoxCell();
            codeColumn.HeaderText = "Code";
            codeColumn.Name = "Code";
            codeColumn.ReadOnly = true;
            codeColumn.Width = 70;
            dataGridView.Columns.Add(codeColumn);

            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn();
            descColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            descColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            descColumn.CellTemplate = new DataGridViewTextBoxCell();
            descColumn.HeaderText = "Description";
            descColumn.Name = "Description";
            descColumn.ReadOnly = true;
            descColumn.Width = 280;
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
            gvwHierarchie.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(DataGridViewDataError);
            gvwHierarchie.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
        }

        /// <summary>
        /// Removes the event handlers from the grid.
        /// </summary>
        private void RemoveGridEventHandles()
        {
            gvwHierarchie.DataError -= new DataGridViewDataErrorEventHandler(DataGridViewDataError);
            gvwHierarchie.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
        }

        private void EnableDisableCheckBox()
        {
            if (SelectedHierarchies.Count == 0) { return; }
            foreach (HierarchyEntity hierarchyEntity in SelectedHierarchies)
            {
                string selectedHIE = hierarchyEntity.Code;

                if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
                {
                    string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
                    foreach (DataGridViewRow dr in gvwHierarchie.Rows)
                    {
                        string rowCode = dr.Cells["Code"].Value.ToString();
                        if (selectedHIE.Equals("**-**-**") && !rowCode.Equals(selectedHIE))
                        {
                            dr.Cells["Select"].ReadOnly = true;
                            dr.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                        else if (rowCode.StartsWith(selectedHierarchy) && !rowCode.Equals(selectedHIE))
                        {
                            dr.Cells["Select"].ReadOnly = true;
                            dr.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                    }
                }
                gvwHierarchie.Update();
                gvwHierarchie.ResumeLayout();
            }
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
            string selectedHIE = hierarchicalGrid.SelectedRows[0].Cells["Code"].Value.ToString();
            bool isSelect = false;
            hierarchicalGrid.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            if (hierarchicalGrid.SelectedRows[0].Cells["Select"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
            {
                isSelect = true;
            }
            if (Mode.Equals("Program"))
            {
                if (Prog_Multiple_sel != "Hie_multiple_sel")
                {
                    foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
                    {
                        string rowCode = dr.Cells["Code"].Value.ToString();
                        if (!rowCode.Equals(selectedHIE))
                        {
                            dr.Cells["Select"].Value = "false";
                            dr.DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            dr.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
            else if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
            {
                string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
                foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
                {
                    string rowCode = dr.Cells["Code"].Value.ToString();
                    if (selectedHIE.Equals("**-**-**") && !rowCode.Equals(selectedHIE))
                    {
                        if (isSelect)
                        {
                            dr.Cells["Select"].Value = "false";
                            dr.Cells["Select"].ReadOnly = true;
                            dr.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                        else
                        {
                            dr.Cells["Select"].ReadOnly = false;
                            dr.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                    else if (rowCode.StartsWith(selectedHierarchy) && !rowCode.Equals(selectedHIE))
                    {
                        if (isSelect)
                        {
                            dr.Cells["Select"].Value = "false";
                            dr.Cells["Select"].ReadOnly = true;
                            dr.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                        else
                        {
                            dr.Cells["Select"].ReadOnly = false;
                            dr.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
                hierarchicalGrid.Update();
                hierarchicalGrid.ResumeLayout();
                hierarchicalGrid.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
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
                if (Mode.Equals("Edit"))
                {
                    //savePasswordHIE();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void savePasswordHIE()
        {
            //Add PASSWORDHIE
            string userID = UserProfile.UserID; ;
            string fname = UserProfile.FirstName;
            string lname = UserProfile.LastName;
            string caseWorker = UserProfile.CaseWorker;
            string Security = UserProfile.Security;

            foreach (DataGridViewRow row in gvwHierarchie.Rows)
            {
                HierarchyEntity hierarchyEntity = row.Tag as HierarchyEntity;
                string agency = hierarchyEntity.Agency.Equals(string.Empty) ? "**" : hierarchyEntity.Agency.ToString();
                string dept = hierarchyEntity.Dept.Equals(string.Empty) ? "**" : hierarchyEntity.Dept.ToString();
                string prog = hierarchyEntity.Prog.ToString().Equals(string.Empty) ? "**" : hierarchyEntity.Prog.ToString();
                string inActive = "Y";
                if (row.Cells["Select"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                {
                    inActive = "N";
                }
                if (hierarchyEntity.DateAdd.Equals(string.Empty) && inActive.Equals("Y"))
                {
                    continue;
                }
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PWH_EMPLOYEE_NO", userID));
                sqlParamList.Add(new SqlParameter("@PWH_TYPE", HierarchyType));
                sqlParamList.Add(new SqlParameter("@PWH_AGENCY", agency));
                sqlParamList.Add(new SqlParameter("@PWH_DEPT", dept));
                sqlParamList.Add(new SqlParameter("@PWH_PROG", prog));
                sqlParamList.Add(new SqlParameter("@PWH_LAST_NAME", fname));
                sqlParamList.Add(new SqlParameter("@PWH_FIRST_NAME", lname));
                sqlParamList.Add(new SqlParameter("@PWH_CASEWORKER", caseWorker));
                sqlParamList.Add(new SqlParameter("@PWH_SECURITY", Security));
                sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", "N"));
                sqlParamList.Add(new SqlParameter("@PWH_INACTIVE", "N"));
                sqlParamList.Add(new SqlParameter("@PWH_LSTC_OPERATOR", BaseForm.UserID));
                sqlParamList.Add(new SqlParameter("@PWH_ADD_OPERATOR", BaseForm.UserID));
                Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORDHIE(sqlParamList);
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

    }
}