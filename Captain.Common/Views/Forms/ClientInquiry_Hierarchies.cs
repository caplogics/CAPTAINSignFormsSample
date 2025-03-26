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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ClientInquiry_Hierarchies : Form
    {
        private ErrorProvider _errorProvider = null;
        private List<CLINQHIEEntity> _selectedHierarchies = new List<CLINQHIEEntity>();
        private List<ListItem> _selectedListItem = null;
        private CaptainModel _model = null;
        private bool boolhierchy = true;
        public ClientInquiry_Hierarchies(BaseForm baseForm, List<CLINQHIEEntity> SelHie, string mode, string strType, string strFilter, string strFormType)
        {
            InitializeComponent();
            AddGridColumns(HierarchieGrid);
            BaseForm = baseForm;
            Mode = mode;

            lblChoose.Text = "Choose Hierarchy Here";

            _model = new CaptainModel();
            HierarchyEntity hierarchyAll = new HierarchyEntity();
            hierarchyAll.Code = "**-**-**";
            hierarchyAll.HirarchyName = "All Hierarchies";
            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(UserProfile != null ? UserProfile.UserID : string.Empty, HierarchyType, "I");
            if (strType == "I")
                caseHierarchy.Insert(0, hierarchyAll);
            int rowIndex = 0;
            if (caseHierarchy.Count > 0)
            {
                caseHierarchy.ForEach(item => item.InActiveFlag = "false");
                caseHierarchy.ForEach(item => item.Intake = "false");
            }
            if (SelHie != null && SelHie.Count > 0)
            {
                caseHierarchy.ForEach(item => item.InActiveFlag = (SelHie.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)) && u.CLINQPdf.Equals("Y"))) ? "true" : "false");
                caseHierarchy.ForEach(item => item.Intake = (SelHie.Exists(u => u.Code.Replace("-", string.Empty).Equals(item.Code.Replace("-", string.Empty)) && u.CLINQCNotes.Equals("Y"))) ? "true" : "false");
            }
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
            {
                boolhierchy = true;
                if (strFormType == "I")
                {

                    if (hierarchyEntity.Agency != string.Empty && hierarchyEntity.Dept == string.Empty && hierarchyEntity.Prog == string.Empty)
                    {
                        List<HierarchyEntity> caseHierarchyAgency = caseHierarchy.FindAll(u => u.Agency.Equals(hierarchyEntity.Agency));
                        if (caseHierarchyAgency.Count <= 2)
                        {
                            boolhierchy = false;
                        }

                    }
                    if (hierarchyEntity.Agency != string.Empty && hierarchyEntity.Dept != string.Empty && hierarchyEntity.Prog == string.Empty)
                    {
                        List<HierarchyEntity> caseHierarchyDept = caseHierarchy.FindAll(u => u.Agency.Equals(hierarchyEntity.Agency) && u.Dept.Equals(hierarchyEntity.Dept));
                        if (caseHierarchyDept.Count <= 1)
                        {
                            boolhierchy = false;
                        }
                    }

                }
                if (boolhierchy)
                {
                    
                    rowIndex = gvwHierarchie.Rows.Add(hierarchyEntity.Code, hierarchyEntity.HirarchyName,hierarchyEntity.InActiveFlag,hierarchyEntity.Intake);
                    gvwHierarchie.Rows[rowIndex].Tag = hierarchyEntity;
                }
            }
            foreach (CLINQHIEEntity hierarchyEntity in SelHie)
            {
                string strAgency = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency;
                string strDept = hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept;
                string strProgram = hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                //string code = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency + "-" + hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept + "-" + hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                string code = strAgency + "-" + strDept + "-" + strProgram;
                if (hierarchyEntity.Code == "**-**-**")
                {
                    code = "**_**_**"; string Name = "All Hierarchies";
                    rowIndex = gvwSelectedHierarachies.Rows.Add(code, Name, hierarchyEntity.CLINQPdf == "Y" ? true : false, hierarchyEntity.CLINQCNotes == "Y" ? true : false);
                }
                else
                    rowIndex = gvwSelectedHierarachies.Rows.Add(code, hierarchyEntity.HirarchyName, hierarchyEntity.CLINQPdf == "Y" ? true : false, hierarchyEntity.CLINQCNotes == "Y" ? true : false);
                gvwSelectedHierarachies.Rows[rowIndex].Tag = hierarchyEntity;
            }

            EnableDisableCheckBox();

            AddGridEventHandles();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

        }

        #region Properties

        public TagClass SelectedNodeTagClass { get; set; }

        public BaseForm BaseForm { get; set; }

        public string HierarchyType { get; set; }

        public UserEntity UserProfile { get; set; }

        public string Mode { get; set; }

        public string Prog_Multiple_sel { get; set; }

        public DataGridView HierarchieGrid
        {
            get { return gvwHierarchie; }
        }

        public DataGridViewRow HierarchieGridRow
        {
            get;
            set;
        }

        public List<CLINQHIEEntity> SelectedHierarchies { get; set; }
        //{
        //    get
        //    {
        //        //return _selectedHierarchies = (from c in gvwHierarchie.Rows.Cast<DataGridViewRow>().ToList()
        //        //                               where (((DataGridViewCheckBoxCell)c.Cells["PdfChk"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase) || ((DataGridViewCheckBoxCell)c.Cells["casenotesChk"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
        //        //                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();

        //    }
        //}

        #endregion

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

            dataTypeColumn = new DataGridViewCheckBoxColumn();
            dataTypeColumn.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "Pdf";
            dataTypeColumn.Name = "PdfChk";
            dataTypeColumn.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Width = 40;
            dataGridView.Columns.Add(dataTypeColumn);

            DataGridViewCheckBoxColumn dataTypeColumn1 = new DataGridViewCheckBoxColumn();
            dataTypeColumn1.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn1.DefaultHeaderCellType = typeof(Wisej.Web.DataGridViewColumnHeaderCell);
            dataTypeColumn1.HeaderText = "Case Notes";
            dataTypeColumn1.Name = "casenotesChk";
            dataTypeColumn1.Resizable = Wisej.Web.DataGridViewTriState.True;
            dataTypeColumn1.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn1.Width = 77;
            dataGridView.Columns.Add(dataTypeColumn1);

            
        }


        private bool ValidateForm()
        {
            bool isValid = true;

            if (gvwHierarchie.Rows.Count > 0)
            {
                int invalidRecords = 0;
                foreach (DataGridViewRow item in gvwHierarchie.Rows)
                {
                    if (string.IsNullOrEmpty(item.Cells[0].Value.ToString().Trim()))
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

        private void btnOk_Click(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        
        private void EnableDisableCheckBox()
        {
            SelectedHierarchies = SelectedHierar();
            if (SelectedHierarchies.Count == 0) { return; }
            foreach (CLINQHIEEntity hierarchyEntity in SelectedHierarchies)
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
                            dr.Cells["PdfChk"].ReadOnly = true; dr.Cells["casenotesChk"].ReadOnly = true;
                            dr.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                        else if (rowCode.StartsWith(selectedHierarchy) && !rowCode.Equals(selectedHIE))
                        {
                            dr.Cells["PdfChk"].ReadOnly = true; dr.Cells["casenotesChk"].ReadOnly = true;
                            dr.DefaultCellStyle.ForeColor = Color.LightGray;
                        }
                    }
                }
                gvwHierarchie.Update();
                gvwHierarchie.ResumeLayout();
            }
        }

        private void DataGridViewBeforeClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)sender;

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

        private void AddGridEventHandles()
        {
            gvwHierarchie.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(DataGridViewDataError);
            //gvwHierarchie.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
            // Commented By Yeswanth on 11/25/2014 Because of this an issue rised only in CAPOK Site and good in rest the of site
            // Added Below logic to avoid the issue
            gvwHierarchie.CellClick += new DataGridViewCellEventHandler(DataGridView_CellClick);
        }

        /// <summary>
        /// Handles the grid Cell Click event and keeps track of the changes made on the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView hierarchicalGrid = sender as DataGridView;
            if (e.RowIndex > -1)
            {
                if (hierarchicalGrid.SelectedRows[0].DefaultCellStyle.ForeColor != Color.LightGray)
                {
                    if (hierarchicalGrid.CurrentCell.ColumnIndex == 2 || hierarchicalGrid.CurrentCell.ColumnIndex == 3)
                    {
                        // return;

                        string selectedHIE = hierarchicalGrid.SelectedRows[0].Cells["Code"].Value.ToString();
                        bool isSelect = false;
                        bool IsNotes = false;
                        if (hierarchicalGrid.SelectedRows[0].Cells["PdfChk"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                        {
                            isSelect = true;
                        }                         // Commented By Yeswanth on 11/25/2014 Because of this an issue rised only in CAPOK Site and good in rest the of site

                        if (hierarchicalGrid.SelectedRows[0].Cells["casenotesChk"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                            IsNotes = true;

                        if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
                        {
                            string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
                            foreach (DataGridViewRow dr in hierarchicalGrid.Rows)
                            {
                                string rowCode = dr.Cells["Code"].Value.ToString();
                                if (selectedHIE.Equals("**-**-**") && !rowCode.Equals(selectedHIE))
                                {
                                    if (isSelect || IsNotes)
                                    {
                                        dr.Cells["PdfChk"].Value = "false";
                                        dr.Cells["casenotesChk"].Value = "false";
                                        dr.Cells["PdfChk"].ReadOnly = true;
                                        dr.Cells["casenotesChk"].ReadOnly = true;
                                        dr.DefaultCellStyle.ForeColor = Color.LightGray;
                                    }
                                    else
                                    {
                                        dr.Cells["PdfChk"].ReadOnly = false;
                                        dr.Cells["casenotesChk"].ReadOnly = false;
                                        dr.DefaultCellStyle.ForeColor = Color.Black;
                                    }
                                }
                                else if (rowCode.StartsWith(selectedHierarchy) && !rowCode.Equals(selectedHIE))
                                {
                                    if (isSelect || IsNotes)
                                    {
                                        //if (isSelect)
                                        //{
                                        dr.Cells["PdfChk"].Value = "false";
                                        dr.Cells["PdfChk"].ReadOnly = true;
                                        dr.Cells["casenotesChk"].Value = "false";
                                        dr.Cells["casenotesChk"].ReadOnly = true;
                                        dr.DefaultCellStyle.ForeColor = Color.LightGray;
                                        //}
                                        //if (IsNotes)
                                        //{
                                        //    dr.Cells["casenotesChk"].Value = "false";
                                        //    dr.Cells["casenotesChk"].ReadOnly = true;
                                        //    dr.DefaultCellStyle.ForeColor = Color.LightGray;
                                        //}
                                    }
                                    else
                                    {
                                        dr.Cells["PdfChk"].ReadOnly = false;
                                        dr.Cells["casenotesChk"].ReadOnly = false;
                                        dr.DefaultCellStyle.ForeColor = Color.Black;
                                    }
                                }
                            }
                            hierarchicalGrid.Update();
                            hierarchicalGrid.ResumeLayout();
                        }
                    }
                }
            }
        }

        public List<CLINQHIEEntity> SelectedHierar()
        {
            if (gvwHierarchie.Rows.Count > 0)
            {
                _selectedHierarchies = new List<CLINQHIEEntity>();
                foreach (DataGridViewRow dr in gvwHierarchie.Rows)
                {
                    CLINQHIEEntity SearchEntity = new CLINQHIEEntity();

                    if ((dr.Cells["PdfChk"].Value.ToString().ToUpper() == "TRUE") || (dr.Cells["casenotesChk"].Value.ToString().ToUpper() == "TRUE"))
                    {
                        HierarchyEntity hr = dr.Tag as HierarchyEntity;
                        if (hr != null)
                        {
                            if (hr.Code.Contains("**")) { SearchEntity.Agency = (hr.Agency == "" ? "**" : hr.Agency); SearchEntity.Dept = (hr.Dept == "" ? "**" : hr.Dept); SearchEntity.Prog = (hr.Prog == "" ? "**" : hr.Prog); }
                            else
                            { SearchEntity.Agency = hr.Agency; SearchEntity.Dept = hr.Dept; SearchEntity.Prog = hr.Prog; }
                            SearchEntity.UsedFlag = hr.UsedFlag; SearchEntity.Code = hr.Code; SearchEntity.HirarchyName = hr.HirarchyName;
                            if (dr.Cells["PdfChk"].Value.ToString().ToUpper() == "TRUE") SearchEntity.CLINQPdf = "Y"; else SearchEntity.CLINQPdf = "N";
                            if (dr.Cells["casenotesChk"].Value.ToString().ToUpper() == "TRUE") SearchEntity.CLINQCNotes = "Y"; else SearchEntity.CLINQCNotes = "N";

                            _selectedHierarchies.Add(SearchEntity);
                        }
                    }
                }
            }

            return _selectedHierarchies;
        }


    }
}