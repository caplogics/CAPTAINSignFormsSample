#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using DevExpress.XtraRichEdit.Import.Doc;
using System.Web.UI;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SERHIESEL_FORM : Form
    {
        private ErrorProvider _errorProvider = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        private List<ListItem> _selectedListItem = null;
        private CaptainModel _model = null;
        private bool boolhierchy = true;
        string SerstrFilter = string.Empty;
        public SERHIESEL_FORM(BaseForm baseForm, List<HierarchyEntity> hierarchy, string mode, string strType, string strFilter, string strFormType, UserEntity userProfile, List<HierarchyEntity> Servicehierarchy)
        {
            try
            {
                InitializeComponent();
                AddGridColumns(HierarchieGrid);
                BaseForm = baseForm;
                ListOfSelectedHierarchies = hierarchy;
                Mode = mode;
                SerstrFilter = strFilter;
                string inTake = "S";
                this.Text = "Service Plan Hierarchies Selection";
                HierarchyType = inTake;
                string strFormTypefilter = "S";//strFormType;
                _model = new CaptainModel();
                UserProfile = userProfile;
                ListOfSelectedServiceHierarchies = new List<HierarchyEntity>();
                //HierarchyEntity hierarchyAll = new HierarchyEntity();
                //hierarchyAll.Code = "**-**-**";
                //hierarchyAll.HirarchyName = "All Hierarchies";
                if (strFilter == "*")
                {
                    // List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(SelectedUser);
                    //propPasswordSerEntity=
                    propcaseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(UserProfile != null ? UserProfile.UserID : string.Empty, HierarchyType, strFormTypefilter);
                    //if (strType == "I")
                    //    caseHierarchy.Insert(0, hierarchyAll);
                    foreach (HierarchyEntity hierarchyEntity in ListOfSelectedHierarchies)
                    {
                        if (hierarchyEntity.Agency == "**")
                        {
                            HierarchyEntity hierarchyAll = new HierarchyEntity();
                            hierarchyAll.Code = "**-**-**";
                            hierarchyAll.HirarchyName = "All Hierarchies";
                            propcaseHierarchy.Insert(0, hierarchyAll);
                        }
                    }
                    int rowIndex = 0;
                    if (Servicehierarchy != null && Servicehierarchy.Count > 0)
                    {
                        ListOfSelectedServiceHierarchies = Servicehierarchy;
                    }
                    //if (Servicehierarchy != null && Servicehierarchy.Count > 0)
                    //{
                    //    ListOfSelectedServiceHierarchies = Servicehierarchy;
                    //    propcaseHierarchy.ForEach(item => item.InActiveFlag = (Servicehierarchy.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)))) ? "true" : "false");

                    //    List<HierarchyEntity> SelSerHies = propcaseHierarchy.FindAll(u => u.InActiveFlag == "true");
                    //    if (SelSerHies.Count > 0)
                    //    {
                    //        foreach (HierarchyEntity entity in SelSerHies)
                    //        {
                    //            entity.SerAgency = entity.SerAgency == string.Empty ? "**" : entity.SerAgency;
                    //            entity.SerDept = entity.SerDept == string.Empty ? "**" : entity.SerDept;
                    //            entity.SerProg = entity.SerProg == string.Empty ? "**" : entity.SerProg;
                    //            entity.Agency = entity.Agency == string.Empty ? "**" : entity.Agency;
                    //            entity.Dept = entity.Dept == string.Empty ? "**" : entity.Dept;
                    //            entity.Prog = entity.Prog == string.Empty ? "**" : entity.Prog;

                    //            HierarchyEntity SerEntity = Servicehierarchy.Find(u => u.Agency == entity.Agency && u.Dept == entity.Dept && u.Prog == entity.Prog && u.SerAgency == entity.SerAgency && u.SerDept == entity.SerDept && u.SerProg == entity.SerProg);
                    //            if (SerEntity != null)
                    //            {
                    //                entity.SerAgency = SerEntity.SerAgency; entity.SerDept = SerEntity.SerDept; entity.SerProg = SerEntity.SerProg;
                    //                propcaseHierarchy.Remove(entity);
                    //                propcaseHierarchy.Add(entity);

                    //            }

                    //        }
                    //    }

                    //    propcaseHierarchy = propcaseHierarchy.OrderBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();
                    //}
                    //else
                    //{
                    //    //ListOfSelectedServiceHierarchies = new List<Model.Objects.HierarchyEntity>();
                    //}
                    DataGridViewRow dataGridViewRow = new DataGridViewRow();



                    foreach (HierarchyEntity hierarchyEntity in ListOfSelectedHierarchies)
                    {
                        string strAgency = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency;
                        string strDept = hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept;
                        string strProgram = hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                        //string code = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency + "-" + hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept + "-" + hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                        string code = strAgency + "-" + strDept + "-" + strProgram;
                        if (hierarchyEntity.Code == "**-**-**")
                        {
                            code = "**_**_**"; string Name = "All Hierarchies";
                            rowIndex = gvwSelectedHierarachies.Rows.Add(code, Name);
                        }
                        else
                        {
                            rowIndex = gvwSelectedHierarachies.Rows.Add(code, hierarchyEntity.HirarchyName);


                        }
                        if (hierarchyEntity.UsedFlag.Equals("Y"))
                            gvwSelectedHierarachies.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        gvwSelectedHierarachies.Rows[rowIndex].Tag = hierarchyEntity;

                    }
                    if (gvwSelectedHierarachies.Rows.Count > 0)
                    {
                        gvwSelectedHierarachies.Rows[0].Selected = true;







                        fillGriddata(propcaseHierarchy);
                    }
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

        #region Properties


        public TagClass SelectedNodeTagClass { get; set; }

        public BaseForm BaseForm { get; set; }
        PrivilegeEntity PrivilegeEntity = new PrivilegeEntity();
        List<HierarchyEntity> propcaseHierarchy { get; set; }

        List<PASSWORDSERENTITY> propPasswordSerEntity { get; set; }
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

        public List<HierarchyEntity> ListOfSelectedServiceHierarchies { get; set; }



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
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 40;
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
            descColumn.Width = 300;
            dataGridView.Columns.Add(descColumn);
        }


        /// <summary>
        /// Adds the event handlers to the grid
        /// </summary>
        private void AddGridEventHandles()
        {
            gvwHierarchie.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(DataGridViewDataError);
            //gvwHierarchie.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            // Commented By Yeswanth on 11/25/2014 Because of this an issue rised only in CAPOK Site and good in rest the of site
            // Added Below logic to avoid the issue
            gvwHierarchie.CellClick += new DataGridViewCellEventHandler(DataGridView_CellClick);
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
        //private void DataGridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridView hierarchicalGrid = sender as DataGridView;
        //    string selectedHIE = hierarchicalGrid.SelectedRows[0].Cells["Code"].Value.ToString();
        //    bool isSelect = false;
        //    if (hierarchicalGrid.SelectedRows[0].Cells["Select"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        isSelect = true;
        //    }                         // Commented By Yeswanth on 11/25/2014 Because of this an issue rised only in CAPOK Site and good in rest the of site

        //    if (Mode.Equals("Program"))
        //    {
        //        if (Prog_Multiple_sel != "Hie_multiple_sel")
        //        {
        //            foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
        //            {
        //                string rowCode = dr.Cells["Code"].Value.ToString();
        //                if (!rowCode.Equals(selectedHIE))
        //                {
        //                    dr.Cells["Select"].Value = "false";
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //                else
        //                {
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //            }
        //        }
        //    }
        //    if (Mode.Equals("Components"))
        //    {

        //        foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
        //        {
        //            string rowCode = dr.Cells["Code"].Value.ToString();
        //            if (selectedHIE.Equals("****") && !rowCode.Equals(selectedHIE))
        //            {
        //                if (isSelect)
        //                {
        //                    dr.Cells["Select"].Value = "false";
        //                    dr.Cells["Select"].ReadOnly = true;
        //                    dr.DefaultCellStyle.ForeColor = Color.LightGray;
        //                }
        //                else
        //                {
        //                    dr.Cells["Select"].ReadOnly = false;
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //            }
        //            else if (selectedHIE.Equals("None") && !rowCode.Equals(selectedHIE))
        //            {
        //                if (isSelect)
        //                {
        //                    dr.Cells["Select"].Value = "false";
        //                    dr.Cells["Select"].ReadOnly = true;
        //                    dr.DefaultCellStyle.ForeColor = Color.LightGray;
        //                }
        //                else
        //                {
        //                    dr.Cells["Select"].ReadOnly = false;
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //            }
        //        }

        //    }
        //    else if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
        //    {
        //        string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
        //        foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
        //        {
        //            string rowCode = dr.Cells["Code"].Value.ToString();
        //            if (selectedHIE.Equals("**-**-**") && !rowCode.Equals(selectedHIE))
        //            {
        //                if (isSelect)
        //                {
        //                    dr.Cells["Select"].Value = "false";
        //                    dr.Cells["Select"].ReadOnly = true;
        //                    dr.DefaultCellStyle.ForeColor = Color.LightGray;
        //                }
        //                else
        //                {
        //                    dr.Cells["Select"].ReadOnly = false;
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //            }
        //            else if (rowCode.StartsWith(selectedHierarchy) && !rowCode.Equals(selectedHIE))
        //            {
        //                if (isSelect)
        //                {
        //                    dr.Cells["Select"].Value = "false";
        //                    dr.Cells["Select"].ReadOnly = true;
        //                    dr.DefaultCellStyle.ForeColor = Color.LightGray;
        //                }
        //                else
        //                {
        //                    dr.Cells["Select"].ReadOnly = false;
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //            }
        //        }
        //        hierarchicalGrid.Update();
        //        hierarchicalGrid.ResumeLayout();
        //    }
        //}
        private void DataGridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView hierarchicalGrid = sender as DataGridView;
            string selectedHIE = hierarchicalGrid.SelectedRows[0].Cells["Code"].Value.ToString();
            bool isSelect = false;
            if (hierarchicalGrid.SelectedRows[0].Cells["Select"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
            {
                isSelect = true;
            }                         // Commented By Yeswanth on 11/25/2014 Because of this an issue rised only in CAPOK Site and good in rest the of site

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
            if (Mode.Equals("Components"))
            {

                foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
                {
                    string rowCode = dr.Cells["Code"].Value.ToString();
                    if (selectedHIE.Equals("****") && !rowCode.Equals(selectedHIE))
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
                    else if (selectedHIE.Equals("None") && !rowCode.Equals(selectedHIE))
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
            }
        }

        /// <summary>
        /// Handles the grid Cell Click event and keeps track of the changes made on the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        int Hie_Sel_Limit = 0, Sel_Hie_Cnt = 0;
        //private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridView hierarchicalGrid = sender as DataGridView;

        //    if (hierarchicalGrid.CurrentCell.ColumnIndex != 0)
        //        return;
        //    //Hie_Sel_Limit = 0, Sel_Hie_Cnt = 0
        //    if (Mode.Equals("Service") && Hie_Sel_Limit > 0)
        //    {
        //        if (Sel_Hie_Cnt >= Hie_Sel_Limit && hierarchicalGrid.CurrentRow.Cells["Select"].Value.ToString() == true.ToString())
        //        {
        //            MessageBox.Show("Maximum Selection Limit(" + Hie_Sel_Limit.ToString() + ") exceeds");
        //            hierarchicalGrid.CurrentRow.Cells["Select"].Value = "false";
        //            return;
        //        }
        //    }

        //    string selectedHIE = hierarchicalGrid.SelectedRows[0].Cells["Code"].Value.ToString();
        //    bool isSelect = false;
        //    if (hierarchicalGrid.SelectedRows[0].Cells["Select"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        isSelect = true;
        //    }                         // Commented By Yeswanth on 11/25/2014 Because of this an issue rised only in CAPOK Site and good in rest the of site

        //    else if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
        //    {
        //        string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
        //        foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
        //        {
        //            string rowCode = dr.Cells["Code"].Value.ToString();
        //            if (selectedHIE.Equals("**-**-**") && !rowCode.Equals(selectedHIE))
        //            {
        //                if (isSelect)
        //                {
        //                    dr.Cells["Select"].Value = "false";
        //                    dr.Cells["Select"].ReadOnly = true;
        //                    dr.DefaultCellStyle.ForeColor = Color.LightGray;
        //                }
        //                else
        //                {
        //                    dr.Cells["Select"].ReadOnly = false;
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //            }
        //            else if (rowCode.StartsWith(selectedHierarchy) && !rowCode.Equals(selectedHIE))
        //            {
        //                if (isSelect)
        //                {
        //                    dr.Cells["Select"].Value = "false";
        //                    dr.Cells["Select"].ReadOnly = true;
        //                    dr.DefaultCellStyle.ForeColor = Color.LightGray;
        //                }
        //                else
        //                {
        //                    dr.Cells["Select"].ReadOnly = false;
        //                    dr.DefaultCellStyle.ForeColor = Color.Black;
        //                }
        //            }
        //        }
        //        hierarchicalGrid.Update();
        //        hierarchicalGrid.ResumeLayout();
        //    }

        //    if (Mode.Equals("Service"))
        //    {
        //        if (hierarchicalGrid.CurrentRow.Cells["Select"].Value.ToString() == false.ToString())
        //            Sel_Hie_Cnt--;
        //        else
        //            Sel_Hie_Cnt++;

        //        if (Sel_Hie_Cnt <= 0)
        //            Sel_Hie_Cnt = 0;
        //    }

        //}
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView hierarchicalGrid = sender as DataGridView;

            if (hierarchicalGrid.CurrentCell == null)
                return;
            if (hierarchicalGrid.CurrentCell.ColumnIndex != 0)
                return;

            if (hierarchicalGrid.SelectedRows[0].DefaultCellStyle.ForeColor != Color.LightGray)
            {
                string selectedHIE = hierarchicalGrid.SelectedRows[0].Cells["Code"].Value.ToString();
                bool isSelect = false;
                if (hierarchicalGrid.SelectedRows[0].Cells["Select"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                {
                    isSelect = true;
                }

                if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
                {
                    string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
                    foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
                    {
                        string rowCode = dr.Cells["Code"].Value.ToString();
                        if (selectedHIE.Equals("**-**-**") && !rowCode.Equals(selectedHIE))
                        {
                            if (isSelect)
                            {
                                dr.Cells["Select"].Value = false;//"false";
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
                                dr.Cells["Select"].Value = false;//"false";
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
                    HierarchyEntity hieselectdata = hierarchicalGrid.SelectedRows[0].Tag as HierarchyEntity;
                    if (hieselectdata != null)
                    {
                        if (isSelect)
                        {
                            string strAgency = hieselectdata.Agency == string.Empty ? "**" : hieselectdata.Agency;
                            string strDept = hieselectdata.Dept == string.Empty ? "**" : hieselectdata.Dept;
                            string strProgram = hieselectdata.Prog == string.Empty ? "**" : hieselectdata.Prog;

                            HierarchyEntity hierarchydata = gvwSelectedHierarachies.SelectedRows[0].Tag as HierarchyEntity;
                            if (hierarchydata != null)
                            {
                                hieselectdata.SerAgency = hierarchydata.Agency == string.Empty ? "**" : hierarchydata.Agency;
                                hieselectdata.SerDept = hierarchydata.Dept == string.Empty ? "**" : hierarchydata.Dept;
                                hieselectdata.SerProg = hierarchydata.Prog == string.Empty ? "**" : hierarchydata.Prog;
                            }



                            if (strAgency == "**")
                            {
                                ListOfSelectedServiceHierarchies = new List<HierarchyEntity>();
                                ListOfSelectedServiceHierarchies.Add(hieselectdata);
                            }
                            if (strAgency != "**" && strDept == "**")
                            {
                                List<HierarchyEntity> listHie = ListOfSelectedServiceHierarchies.FindAll(u => u.Agency == strAgency && u.Dept != strDept).ToList();

                                foreach (HierarchyEntity item in listHie)
                                {
                                    ListOfSelectedServiceHierarchies.Remove(item);
                                }

                                ListOfSelectedServiceHierarchies.Add(hieselectdata);
                            }
                            if (strAgency != "**" && strDept != "**" && strProgram == "**")
                            {
                                List<HierarchyEntity> listHie = ListOfSelectedServiceHierarchies.FindAll(u => u.Agency == strAgency && u.Dept == strDept && u.Prog != strProgram).ToList();

                                foreach (HierarchyEntity item in listHie)
                                {
                                    ListOfSelectedServiceHierarchies.Remove(item);
                                }

                                ListOfSelectedServiceHierarchies.Add(hieselectdata);
                            }
                        }
                        else
                        {
                            List<HierarchyEntity> Hiearchyselecteddata = ListOfSelectedServiceHierarchies;
                            foreach (HierarchyEntity item in Hiearchyselecteddata)
                            {
                                string strCode = (item.Agency == string.Empty ? "**" : item.Agency) + "-" + (item.Dept == string.Empty ? "**" : item.Dept) + "-" + (item.Prog == string.Empty ? "**" : item.Prog);
                                if (hieselectdata.Code.ToUpper() == strCode.ToUpper())
                                {
                                    Hiearchyselecteddata.Remove(item);
                                    break;
                                }
                            }
                            ListOfSelectedServiceHierarchies = Hiearchyselecteddata;
                        }
                    }
                    hierarchicalGrid.Update();
                    hierarchicalGrid.ResumeLayout();
                }
                else
                {
                    if (isSelect)
                    {
                        HierarchyEntity Intakehierarchydata = gvwSelectedHierarachies.SelectedRows[0].Tag as HierarchyEntity;
                        HierarchyEntity hieselectdata = hierarchicalGrid.SelectedRows[0].Tag as HierarchyEntity;
                        if (hieselectdata != null)
                        {
                            hieselectdata.SerAgency = Intakehierarchydata.Agency;
                            hieselectdata.SerDept = Intakehierarchydata.Dept;
                            hieselectdata.SerProg = Intakehierarchydata.Prog;

                        }
                        if (Intakehierarchydata.UsedFlag != "Y")
                        {
                            hieselectdata.UsedFlag = "N";
                        }
                        else
                            hieselectdata.UsedFlag = "Y";

                        if (ListOfSelectedServiceHierarchies != null)
                        {
                            if (!ListOfSelectedServiceHierarchies.Exists(item => (item.Agency == hieselectdata.Agency && item.Dept == hieselectdata.Dept && item.Prog == hieselectdata.Prog)
                                && (item.SerAgency == Intakehierarchydata.Agency && item.SerDept == Intakehierarchydata.Dept && item.SerProg == Intakehierarchydata.Prog)
                                )) //&& item.SerAgency == Intakehierarchydata.Agency && item.SerDept == Intakehierarchydata.Dept && item.SerProg == Intakehierarchydata.Prog
                                ListOfSelectedServiceHierarchies.Add(hieselectdata);
                        }
                        else
                        {
                            ListOfSelectedServiceHierarchies.Add(hieselectdata);
                        }
                    }
                    else
                    {
                        HierarchyEntity hieselectdata = hierarchicalGrid.SelectedRows[0].Tag as HierarchyEntity;
                        List<HierarchyEntity> Hiearchyselecteddata = ListOfSelectedServiceHierarchies;
                        foreach (HierarchyEntity item in Hiearchyselecteddata)
                        {
                            string strCode = (item.Agency == string.Empty ? "**" : item.Agency) + "-" + (item.Dept == string.Empty ? "**" : item.Dept) + "-" + (item.Prog == string.Empty ? "**" : item.Prog);
                            if (hieselectdata.Code.ToUpper() == strCode.ToUpper())
                            {
                                item.UsedFlag = "Y";
                                //Hiearchyselecteddata.Remove(item);
                                break;
                            }
                        }
                        ListOfSelectedServiceHierarchies = Hiearchyselecteddata;
                    }
                }
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


        private void GetHierarchies(List<HierarchyEntity> Servicehierarchy)
        {
            HierarchyEntity hierarchydata = gvwSelectedHierarachies.SelectedRows[0].Tag as HierarchyEntity;
            if (hierarchydata != null)
            {
                propcaseHierarchy = _model.lookupDataAccess.GetserHierarchyByUserID(UserProfile != null ? UserProfile.UserID : string.Empty, HierarchyType, "S", hierarchydata.Agency, hierarchydata.Dept, hierarchydata.Prog);

                if (SerstrFilter == "*")
                {
                    foreach (HierarchyEntity hierarchyEntity in ListOfSelectedHierarchies)
                    {
                        if (hierarchyEntity.Agency == "**")
                        {
                            HierarchyEntity hierarchyAll = new HierarchyEntity();
                            hierarchyAll.Code = "**-**-**";
                            hierarchyAll.HirarchyName = "All Hierarchies";
                            propcaseHierarchy.Insert(0, hierarchyAll);
                        }
                    }
                }

                if (Servicehierarchy != null && Servicehierarchy.Count > 0)
                {
                    ListOfSelectedServiceHierarchies = Servicehierarchy;
                    propcaseHierarchy.ForEach(item => item.InActiveFlag = (Servicehierarchy.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)) && u.SerAgency == hierarchydata.Agency && u.SerDept == hierarchydata.Dept && u.SerProg == hierarchydata.Prog)) ? "true" : "false");



                    List<HierarchyEntity> SelSerHies = propcaseHierarchy.FindAll(u => u.InActiveFlag == "true");
                    if (SelSerHies.Count > 0)
                    {
                        foreach (HierarchyEntity entity in SelSerHies)
                        {
                            entity.Agency = entity.Agency == string.Empty ? "**" : entity.Agency;
                            entity.Dept = entity.Dept == string.Empty ? "**" : entity.Dept;
                            entity.Prog = entity.Prog == string.Empty ? "**" : entity.Prog;

                            HierarchyEntity SelEntity = Servicehierarchy.Find(u => u.Agency == entity.Agency && u.Dept == entity.Dept && u.Prog == entity.Prog);
                            if (SelEntity != null)
                            {
                                entity.SerAgency = SelEntity.SerAgency;
                                entity.SerDept = SelEntity.SerDept;
                                entity.SerProg = SelEntity.SerProg;
                                //entity.Agency = entity.Agency == string.Empty ? "**" : entity.Agency;
                                //entity.Dept = entity.Dept == string.Empty ? "**" : entity.Dept;
                                //entity.Prog = entity.Prog == string.Empty ? "**" : entity.Prog;
                                ////entity.SerAgency = entity.SerAgency == string.Empty ? "**" : entity.SerAgency;
                                ////entity.SerDept = entity.SerDept == string.Empty ? "**" : entity.SerDept;
                                ////entity.SerProg = entity.SerProg == string.Empty ? "**" : entity.SerProg;
                                ////entity.Agency = entity.Agency == string.Empty ? "**" : entity.Agency;
                                ////entity.Dept = entity.Dept == string.Empty ? "**" : entity.Dept;
                                ////entity.Prog = entity.Prog == string.Empty ? "**" : entity.Prog;
                            }

                            HierarchyEntity SerEntity = Servicehierarchy.Find(u => u.Agency == entity.Agency && u.Dept == entity.Dept && u.Prog == entity.Prog && u.SerAgency == entity.SerAgency && u.SerDept == entity.SerDept && u.SerProg == entity.SerProg);
                            if (SerEntity != null)
                            {
                                entity.SerAgency = SerEntity.SerAgency; entity.SerDept = SerEntity.SerDept; entity.SerProg = SerEntity.SerProg;
                                //SerEntity.InActiveFlag=
                                propcaseHierarchy.Remove(entity);
                                propcaseHierarchy.Add(entity);

                            }

                        }
                    }

                    propcaseHierarchy = propcaseHierarchy.OrderBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();
                }
                else
                {
                    //ListOfSelectedServiceHierarchies = new List<Model.Objects.HierarchyEntity>();
                }
            }
        }

        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {

            if (chkShowAll.Checked)
            {
                List<HierarchyEntity> _listHierchydata = ListOfSelectedServiceHierarchies;
                gvwHierarchie.Rows.Clear();
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(UserProfile != null ? UserProfile.UserID : string.Empty, HierarchyType, "I");
                //if (strType == "I")
                //    caseHierarchy.Insert(0, hierarchyAll);
                foreach (HierarchyEntity hierarchyEntity in ListOfSelectedHierarchies)
                {
                    if (hierarchyEntity.Agency == "**")
                    {
                        HierarchyEntity hierarchyAll = new HierarchyEntity();
                        hierarchyAll.Code = "**-**-**";
                        hierarchyAll.HirarchyName = "All Hierarchies";
                        caseHierarchy.Insert(0, hierarchyAll);
                    }
                }
                int rowIndex = 0;
                if (_listHierchydata != null && _listHierchydata.Count > 0)
                {
                    caseHierarchy.ForEach(item => item.InActiveFlag = (_listHierchydata.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)))) ? "true" : "false");
                }
                fillGriddata(caseHierarchy);

            }
            else
            {
                List<HierarchyEntity> _listHierchydata = SelectedHierarchies;
                List<HierarchyEntity> hierdata = _listHierchydata;
                gvwHierarchie.Rows.Clear();
                foreach (HierarchyEntity hierarchyserviceEntity in _listHierchydata)
                {
                    int rowIndex = gvwHierarchie.Rows.Add(true, hierarchyserviceEntity.Code, hierarchyserviceEntity.HirarchyName);
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyserviceEntity;
                }

            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void gvwSelectedHierarachies_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwSelectedHierarachies.Rows.Count > 0)
            {
                GetHierarchies(ListOfSelectedServiceHierarchies);
                //if (ListOfSelectedServiceHierarchies != null && ListOfSelectedServiceHierarchies.Count > 0)
                //{
                //    propcaseHierarchy.ForEach(item => item.InActiveFlag = (ListOfSelectedServiceHierarchies.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)) && u.SerAgency == gvwSelectedHierarachies.CurrentRow.Cells["CellCode"].ToString().Substring(0,2) && u.SerDept == gvwSelectedHierarachies.CurrentRow.Cells["CellCode"].ToString().Substring(3, 2) && u.SerProg == gvwSelectedHierarachies.CurrentRow.Cells["CellCode"].ToString().Substring(6, 2))) ? "true" : "false");
                //}
                fillGriddata(propcaseHierarchy);
            }
        }
        private void fillGriddata(List<HierarchyEntity> caseHierarchy)
        {
            List<HierarchyEntity> Hierchyservicedata = new List<HierarchyEntity>();
            int rowIndex;
            gvwHierarchie.Rows.Clear();
            HierarchyEntity hierarchydata = gvwSelectedHierarachies.SelectedRows[0].Tag as HierarchyEntity;
            if (hierarchydata != null)
            {
                if (BaseForm.BaseAgencyControlDetails.SerPlanAllow == "A")
                {

                    string strAgency = hierarchydata.Agency == string.Empty ? "**" : hierarchydata.Agency;
                    string strDept = hierarchydata.Dept == string.Empty ? "**" : hierarchydata.Dept;
                    if (strAgency == "**")
                    {
                        //**Hierchyservicedata = caseHierarchy.FindAll(u => u.SerAgency == hierarchydata.Agency && u.SerDept == hierarchydata.Dept && u.SerProg == hierarchydata.Prog);
                        Hierchyservicedata = caseHierarchy.FindAll(u => (u.SerAgency == hierarchydata.Agency || u.SerAgency.Trim() == "") && (u.SerDept == hierarchydata.Dept || u.SerDept.Trim() == "") && (u.SerProg == hierarchydata.Prog || u.SerProg.Trim() == ""));
                    }
                    else
                    {
                        //**Hierchyservicedata = caseHierarchy.FindAll(u => u.Agency == hierarchydata.Agency && u.SerAgency == hierarchydata.Agency && u.SerDept == hierarchydata.Dept && u.SerProg == hierarchydata.Prog);
                        Hierchyservicedata = caseHierarchy.FindAll(u => u.Agency == hierarchydata.Agency);
                    }
                    foreach (HierarchyEntity hierarchyserviceEntity in Hierchyservicedata)
                    {
                        bool inactFlag = false;
                        //**if (hierarchyserviceEntity.InActiveFlag == "true")
                        if (hierarchyserviceEntity.InActiveFlag == "true" && hierarchyserviceEntity.SerAgency == hierarchydata.Agency && hierarchyserviceEntity.SerDept == hierarchydata.Dept && hierarchyserviceEntity.SerProg == hierarchydata.Prog)
                            inactFlag = true;

                        //if (!hierarchyserviceEntity.Code.Contains("**"))
                        //{
                        rowIndex = gvwHierarchie.Rows.Add(inactFlag, hierarchyserviceEntity.Code, hierarchyserviceEntity.HirarchyName);
                        gvwHierarchie.Rows[rowIndex].Tag = hierarchyserviceEntity;
                        // }
                    }

                }
                else
                {
                    string strAgency = hierarchydata.Agency == string.Empty ? "**" : hierarchydata.Agency;
                    string strDept = hierarchydata.Dept == string.Empty ? "**" : hierarchydata.Dept;
                    if (strAgency == "**")
                    {
                        Hierchyservicedata = caseHierarchy.FindAll(u => (u.SerAgency == hierarchydata.Agency || u.SerAgency.Trim() == "") && (u.SerDept == hierarchydata.Dept || u.SerDept.Trim() == "") && (u.SerProg == hierarchydata.Prog || u.SerProg.Trim() == ""));
                    }
                    else if (strAgency != "**" && strDept == "**")
                    {
                        Hierchyservicedata = caseHierarchy.FindAll(u => u.Agency == hierarchydata.Agency); //&& u.SerAgency == hierarchydata.Agency && u.SerDept == hierarchydata.Dept && u.SerProg == hierarchydata.Prog);
                    }
                    else
                    {
                        Hierchyservicedata = caseHierarchy.FindAll(u => u.Agency == hierarchydata.Agency && u.Dept == hierarchydata.Dept); // && u.SerAgency == hierarchydata.Agency && u.SerDept == hierarchydata.Dept && u.SerProg == hierarchydata.Prog);
                    }
                    foreach (HierarchyEntity hierarchyserviceEntity in Hierchyservicedata)
                    {
                        //if (!hierarchyserviceEntity.Code.Contains("**"))
                        //{
                        bool inactFlag = false;
                        if ((hierarchyserviceEntity.InActiveFlag == "true" && hierarchyserviceEntity.SerAgency == hierarchydata.Agency && hierarchyserviceEntity.SerDept == hierarchydata.Dept && hierarchyserviceEntity.SerProg == hierarchydata.Prog) 
                            //||(hierarchyserviceEntity.Code == (hierarchydata.Agency + "-" + hierarchydata.Dept + "-" + hierarchydata.Prog))
                            )
                            inactFlag = true;
                        rowIndex = gvwHierarchie.Rows.Add(inactFlag, hierarchyserviceEntity.Code, hierarchyserviceEntity.HirarchyName);
                        gvwHierarchie.Rows[rowIndex].Tag = hierarchyserviceEntity;
                        //}
                    }

                }

                if (hierarchydata.UsedFlag == "Y" || hierarchydata.InActiveFlag == "Y")
                    gvwHierarchie.ReadOnly = true;
                else
                    gvwHierarchie.ReadOnly = false;

                disablegridrows();
            }
        }

        private void disablegridrows()
        {
            string selectedHIE = string.Empty;
            foreach (DataGridViewRow item in gvwHierarchie.Rows)
            {


                selectedHIE = item.Cells["Code"].Value.ToString();
                bool isSelect = false;
                if (item.Cells["Select"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                {
                    isSelect = true;
                }
                if (isSelect)
                {
                    if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
                    {
                        string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
                        foreach (DataGridViewRow dr in gvwHierarchie.Rows)
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
                        gvwHierarchie.Update();
                        gvwHierarchie.ResumeLayout();
                    }
                }
            }
        }
    }
}