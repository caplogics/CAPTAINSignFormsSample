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
//using Wisej.Web.Design;
using Wisej.Web;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.Text.RegularExpressions;
using CarlosAg.ExcelXmlWriter;
using System.IO;


#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class AgencyTableControl : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public AgencyTableControl(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            //if (BaseForm.UserID.ToUpper() == "JAKE")
            //{
            //    btnConverstion.Visible = true;
            //}
            setControlEnabled(false);
            PopulateDropdowns();
            TabControls();
            propAgencyCode = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            PopulateToolbar(oToolbarMnustrip);

        }

        private void TabControls()
        {
            _intakeHierarchy = new GridControl(BaseForm, "Service", null);
            _intakeHierarchy.Dock = DockStyle.Fill;

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }

        public string propReportPath { get; set; }

        private string propAgencyCode { get; set; }
        private string propAgencyseq { get; set; }


        public DataGridViewContentAlignment Alignment { get; set; }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                return _selectedHierarchies = (from c in gvwHierarchy.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();

            }
        }

        #endregion

        private void PopulateDropdowns()
        {
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    cmbModule.Items.Add(new ListItem(dr["APPL_DESCRIPTION"].ToString(), dr["APPL_CODE"].ToString()));
                }
                cmbModule.Items.Insert(0, new ListItem("All Applications", "**"));
                cmbModule.SelectedIndex = 0;
            }
        }

        int Current_Row_Cnt = 0;
        int Previous_Row_Cnt = 0;
        bool Loading_Complete = false;
        string[] RequiredCols = new string[2];
        char RecordMode;
        DataGridViewRow PrivRow;
        int Next_Child_Code = 0;
        string UpdateHierarchiesToTable = null;


        private void gvwAgencyTable_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwAgencyTable.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvwAgencyTable.SelectedRows[0];
                if (row != null)
                {
                    if (Loading_Complete && boolChangeStatus)
                    {
                        //Current_Row_Cnt = int.Parse(gvwAgencyTable.CurrentRow.Index.ToString());
                        Current_Row_Cnt = row.Index;
                        if (Previous_Row_Cnt != Current_Row_Cnt && PrivRow.Index != Current_Row_Cnt)
                        {
                            try
                            {
                                boolChangeStatus = true;
                                if (ValReqBlankItems)
                                {
                                    boolChangeStatus = false;
                                    if (string.IsNullOrEmpty(PrivRow.Cells[CodeColSubscript].EditedFormattedValue.ToString().Trim()) &&
                                        string.IsNullOrEmpty(PrivRow.Cells[DescColSubscript].EditedFormattedValue.ToString().Trim()))
                                        AlertBox.Show("'" + CodeColSubscript + "' and '" + DescColSubscript + "'" + " should not be blank", MessageBoxIcon.Warning);
                                    else
                                        if (string.IsNullOrEmpty(PrivRow.Cells[DescColSubscript].EditedFormattedValue.ToString().Trim()))
                                        AlertBox.Show("'" + DescColSubscript + "'" + " should not be blank", MessageBoxIcon.Warning);
                                    else
                                            if (string.IsNullOrEmpty(PrivRow.Cells[CodeColSubscript].EditedFormattedValue.ToString().Trim()))
                                        AlertBox.Show("'" + CodeColSubscript + "'" + " should not be blank", MessageBoxIcon.Warning);
                                    else
                                    {
                                        boolChangeStatus = true;
                                    }
                                }
                            }
                            catch (Exception ex) { boolChangeStatus = false; }


                            if (boolChangeStatus)
                            {
                                UpdateSelRowRec();
                                /********************* Kranthi :: 09/23/2022*******************************************/
                                int rCurrent_Row_Cnt = Current_Row_Cnt - 1;
                                if (rCurrent_Row_Cnt > -1)
                                {
                                    if (gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells[CodeColSubscript].Value != null && gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells[DescColSubscript].Value != null)
                                    {
                                        if (gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells[CodeColSubscript].Value.ToString() != "" && gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells[DescColSubscript].Value.ToString() != "")
                                        {
                                            if (gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A1"].Value == null)
                                                gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A1"].Value = "0.00000";
                                            if (gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A2"].Value == null)
                                                gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A2"].Value = "0.00000";
                                            if (gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A3"].Value == null)
                                                gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A3"].Value = "0.00000";
                                            if (gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A4"].Value == null)
                                                gvwAgencyTable.Rows[rCurrent_Row_Cnt].Cells["Cell A4"].Value = "0.00000";
                                        }
                                    }
                                }
                                /*****************************************************************************************/
                            }
                            else
                            {
                                int Tmp11 = PrivRow.Index;
                                gvwAgencyTable.Rows[Tmp11].Tag = PrivRow.Index;
                            }
                        }
                        else
                            boolChangeStatus = false;
                    }
                    PrivRow = row;



                    FillHierarchyGrid(row.Cells["SelectedHierarchy"].Value + string.Empty.Trim());
                    UpdateHierarchiesToTable = null;
                }
                setControlEnabled(false);
            }
        }

        private void gvwAgencyTable_SelIndexChanged()
        {
            if (gvwAgencyTable.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvwAgencyTable.SelectedRows[0];
                if (row != null)
                {
                    string hie = row.Cells["SelectedHierarchy"].Value + string.Empty;
                    FillHierarchyGrid(hie.Trim());
                }
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
                ToolBarNew.ToolTipText = "Create Table";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add"; // new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.AddPriv.Equals("false"))
                {
                    ToolBarNew.Enabled = false;
                }

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Table Record";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit"; // new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.ChangePriv.Equals("false"))
                {
                    ToolBarEdit.Enabled = false;
                }

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Table Record";
                ToolBarDel.Enabled = true;
                ToolBarDel.Visible = false;
                ToolBarDel.ImageSource = "captain-delete"; // new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.DelPriv.Equals("false"))
                {
                    ToolBarDel.Enabled = false;
                }

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.ToolTipText = "Agency Table Report in Excel";
                ToolBarExcel.Enabled = true;
                ToolBarExcel.ImageSource = "captain-excel"; // new IconResourceHandle(Consts.Icons16x16.MSExcel);
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Agency Table Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help"; //new IconResourceHandle(Consts.Icons16x16.Help);
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

            if (BaseForm.UserID.ToUpper() == "JAKE")
            {
                if (ToolBarDel != null) ToolBarDel.Visible = true;
                if (ToolBarNew != null) ToolBarNew.Visible = true;
            }
            else
            {
                if (ToolBarDel != null) ToolBarDel.Visible = false;
                if (ToolBarNew != null) ToolBarNew.Visible = false;
            }
        }

        public void Refresh(string typeCode, string mode)
        {
            if (!mode.Equals("Edit"))
            {
                cmbModule.SelectedIndex = 0;
            }
            gvwHierarchy.Rows.Clear();
            cmbModule_SelectedIndexChanged(cmbModule, new EventArgs());
            SetComboBoxValue(cmbAgencyTable, typeCode);
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
                        AgencyTableCreateForm CreateTableChild = new AgencyTableCreateForm(BaseForm, "Add", DynamicAgytype, Privileges.Program, Privileges.PrivilegeName);
                        CreateTableChild.StartPosition = FormStartPosition.CenterScreen;
                        CreateTableChild.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                        if (cmbModule.Items.Count > 0 && cmbAgencyTable.Items.Count > 0)
                        {
                            AgencyTableCreateForm EditTableChild = new AgencyTableCreateForm(BaseForm, "Edit", DynamicAgytype, Privileges.Program, Privileges.PrivilegeName);
                            EditTableChild.StartPosition = FormStartPosition.CenterScreen;
                            EditTableChild.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (cmbModule.Items.Count > 0 && cmbAgencyTable.Items.Count > 0)
                        {
                            //if (CanDeleteHeaderCode())
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected Table - '" + ((ListItem)cmbAgencyTable.SelectedItem).Value.ToString() + "'", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);
                            //else
                            //    MessageBox.Show("You can not Delete Table 'with Active Items' ", "CAP Systems", MessageBoxButtons.OK);
                        }
                        break;
                    case Consts.ToolbarActions.Excel:
                        ExcelAgencyTable(((ListItem)cmbModule.SelectedItem).Value.ToString());
                        break;
                    case Consts.ToolbarActions.Help:
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

        /// <summary>
        /// Handles the message box click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
                if (cmbAgencyTable.Items.Count > 0)
                {
                    string code = ((ListItem)cmbAgencyTable.SelectedItem).Value.ToString();
                    if (_model.Agytab.DeleteAGYTAB(code, "00000"))
                    {
                        cmbModule_SelectedIndexChanged(cmbModule, new EventArgs());
                    }
                }
                AlertBox.Show("Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
            }
        }


        /// <summary>
        /// Handles the message box click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSingleDeleteMessageBoxClicked(DialogResult dialogResult)
        {
            // MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
                if (gvwAgencyTable.Rows.Count > 0)
                {
                    string code = ((ListItem)cmbAgencyTable.SelectedItem).Value.ToString();
                    if (_model.Agytab.DeleteAGYTABSingle(code, propAgencyCode, propAgencyseq))
                    {
                        CommonFunctions.SetComboBoxValue(cmbAgencyTable, code);
                        OnAgencyTableSelectedIndexChanged(gvwAgencyTable, EventArgs.Empty);//(sender, e;

                    }
                }
                AlertBox.Show("Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
            }
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRow()
        {
            string AgyTabCols = null;
            if (gvwAgencyTable != null)
            {
                foreach (DataGridViewRow dr in gvwAgencyTable.SelectedRows)
                {
                    string s = null;
                    if (dr.Selected)
                    {
                        //if (dr.Tag is AgyTabEntity)
                        //{
                        //    AgyTabEntity agyTableEntity = dr.Tag as AgyTabEntity;
                        //    AgyTabCols = agyTableEntity.agycode.ToString();
                        //}
                        //break;

                        if (dr.Selected)
                        {
                            DataRow srow = dr.Tag as DataRow;
                            //AgyTabCols = srow["Agy_Code"].ToString();
                            AgyTabCols = PrivRow.Cells["Agy_Code"].Value.ToString();
                            break;
                        }

                    }
                }
            }
            return AgyTabCols;
        }

        private void FillHierarchyGrid(string AgyHierarchy)
        {
            //string selectedRowUser = GetSelectedRow();
            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(null, "I", string.Empty);
            HierarchyEntity hierarchyEntity = new HierarchyEntity();
            hierarchyEntity.Code = "**-**-**";
            hierarchyEntity.HirarchyName = "All Hierarchies";
            gvwHierarchy.Rows.Clear();

            if (AgyHierarchy.Length > 0)
            {
                try
                {
                    int NextIndex = 0;
                    {
                        NextIndex = AgyHierarchy.Length / 6;
                        string[] HierarchyArray = new string[NextIndex];
                        NextIndex = 0;
                        int sub = 0;
                        AgyHierarchy = AgyHierarchy.Trim();
                        if (AgyHierarchy.Length == 6 && AgyHierarchy.Substring(0, 6) == "******")
                        {
                            int rowIndex = 0;
                            rowIndex = gvwHierarchy.Rows.Add("**-**-**", "All Hierarchies");
                            gvwHierarchy.Rows[rowIndex].Tag = hierarchyEntity;
                        }
                        else
                        {
                            string Tmp = null;
                            for (int count = 0; AgyHierarchy.Length > count;)
                            {
                                HierarchyArray[sub] = AgyHierarchy.Substring(NextIndex, 6);
                                Tmp = HierarchyArray[sub];
                                count = count + 6;
                                NextIndex = count;
                                //FillHierarchyDescription(Tmp, sub);
                                HierarchyEntity hierarchy = caseHierarchy.Find(u => u.Code.Replace("-", string.Empty).Equals(Tmp));
                                if (hierarchy != null)
                                {
                                    int rowIndex = gvwHierarchy.Rows.Add(hierarchy.Code, hierarchy.HirarchyName);
                                    gvwHierarchy.Rows[rowIndex].Tag = hierarchy;
                                }
                                sub++;
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
            if (gvwHierarchy.Rows.Count > 0)
                gvwHierarchy.Rows[0].Selected = true;
            //else
            //{
            //    gvwHierarchy.Rows.Add("**-**-**", "All Hierarchies");
            //}
        }


        private void FillHierarchyDescription(string str, int Row)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(str.Substring(0, 2), str.Substring(2, 2), str.Substring(4, 2));
            string Hiename = (ds.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
            int rowIndex = 0;
            if (Hiename != null)
            {
                rowIndex = gvwHierarchy.Rows.Add(str.Substring(0, 2) + '-' + str.Substring(2, 2) + '-' + str.Substring(4, 2), Hiename);
                gvwHierarchy.Rows[rowIndex].Tag = (Row + 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void setControlEnabled(bool flag)
        {
        }


        private void OnAgencyTableSelectedIndexChanged(object sender, EventArgs e)
        {
            Loading_Complete = boolChangeStatus = ValReqBlankItems = gvwAgencyTable.Visible = Lbl_Req_Controls.Visible = false;
            gvwHierarchy.Rows.Clear();
            try
            {
                //Vikash commented
                //if (!(string.IsNullOrEmpty(_errorProvider.GetError(Lbl_Req_Controls))))
                //    _errorProvider.SetError(Lbl_Req_Controls, null);
            }
            catch (Exception ex) { }

            Previous_Row_Cnt = 0;
            gvwHierarchy.Rows.Clear();
            if (cmbAgencyTable.Items.Count > 0)
            {
                gvwAgencyTable.RowValidating -= new DataGridViewCellCancelEventHandler(OnRowValidating);
                gvwAgencyTable.RowLeave -= new DataGridViewCellEventHandler(dataGridView_RowLeave);
                gvwAgencyTable.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                string code = ((ListItem)cmbAgencyTable.SelectedItem).Value.ToString();
                DynamicAgytype = code;
                AddAgencyTableColumn(code);

                gvwAgencyTable.RowValidating += new DataGridViewCellCancelEventHandler(OnRowValidating);
                gvwAgencyTable.RowLeave += new DataGridViewCellEventHandler(dataGridView_RowLeave);
                gvwAgencyTable.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);

                gvwAgencyTable.Visible = true;
                //gvwAgencyTable.Rows[0].Selected = true;
            }
        }

        string[] DynamicAgyColsNames;
        string CodeColSubscript = null;
        string DescColSubscript = null;
        string DynamicAgyColsseq;
        public string DynamicAgytype;
        bool ValReqBlankItems = false;

        private void AddAgencyTableColumn(string code)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetAgyTab(code);
            string strMainData = ds.Tables[0].Rows[0]["Agy_9"].ToString();
            CodeColSubscript = ds.Tables[0].Rows[0]["AGY_ACTIVE"].ToString().Trim();
            DescColSubscript = ds.Tables[0].Rows[0]["AGY_DEFAULT"].ToString().Trim();

            try
            {
                int TmpInt_1 = 0, TmpInt_2 = 0;
                if (!(string.IsNullOrEmpty(CodeColSubscript)))
                    TmpInt_1 = int.Parse(CodeColSubscript);
                if (!(string.IsNullOrEmpty(DescColSubscript)))
                    TmpInt_2 = int.Parse(DescColSubscript);
                if (TmpInt_1 != 0)
                    ValReqBlankItems = Object.ReferenceEquals(TmpInt_1.GetType(), 1.GetType());
                if (ValReqBlankItems && TmpInt_2 != 0)
                    ValReqBlankItems = Object.ReferenceEquals(1.GetType(), TmpInt_2.GetType());
                if (ValReqBlankItems)
                {
                    CodeColSubscript = "Cell " + CodeColSubscript;
                    DescColSubscript = "Cell " + DescColSubscript;
                    Lbl_Req_Controls.Text = "  ''" + CodeColSubscript + "''  and  ''" + DescColSubscript + "''   are Mandatory";
                    Lbl_Req_Controls.Visible = true;
                }
                else
                {
                    Lbl_Req_Controls.Visible = false;
                    CodeColSubscript = DescColSubscript = null;
                    ValReqBlankItems = false;
                }
            }
            catch (Exception ex) { ValReqBlankItems = false; Lbl_Req_Controls.Visible = false; }



            strMainData = strMainData.Trim();
            DynamicAgyColsseq = strMainData;
            int count = strMainData.Trim().Length;
            char[] ch = strMainData.ToCharArray();

            // Logic to Get Dynamic Columns Name And Number of Columns
            int DymanicColCount = 0;
            string ColSubScript = null;
            List<string> ce = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                if (strMainData != string.Empty)
                {
                    if (ch[i].ToString() == "1")
                    {
                        string xyz = "Cell " + (i + 1);
                        if (ValReqBlankItems)
                        {
                            if (xyz == CodeColSubscript.Substring(0, 6) || xyz == DescColSubscript.Substring(0, 6))
                                xyz = xyz + "*";
                        }
                        ce.Add(xyz);
                        ColSubScript = ColSubScript + (i + 1).ToString();
                        DymanicColCount++;
                    }
                }
            }
            string[] DynamicAgyCols = new string[DymanicColCount];
            for (int i = 0; i < (DymanicColCount); i++)
            {
                DynamicAgyCols[i] = "AGY_" + ColSubScript[i];
            }
            DynamicAgyColsNames = new string[DymanicColCount];
            DynamicAgyColsNames = DynamicAgyCols;
            int TotalGridCol = 7 + DymanicColCount;
            //End of Logic to Get Dynamic Columns Name And Number of Columns

            ce.Add("Cell A1");
            ce.Add("Cell A2");
            ce.Add("Cell A3");
            ce.Add("Cell A4");
            DataGridViewTextBoxColumn dataTypeColumn = null;

            gvwAgencyTable.Columns.Clear();
            this.gvwAgencyTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int TempCount = 1;
            foreach (string strColumnName in ce)
            {
                dataTypeColumn = new DataGridViewTextBoxColumn();
                dataTypeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataTypeColumn.DefaultHeaderCellType = typeof(DataGridViewColumnHeaderCell);
                dataTypeColumn.HeaderText = strColumnName;
                if (TempCount <= DymanicColCount)
                    dataTypeColumn.Name = strColumnName.Substring(0, 6);
                else
                    dataTypeColumn.Name = strColumnName.Substring(0, 7);

                dataTypeColumn.Resizable = DataGridViewTriState.True;
                dataTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                if (TempCount <= DymanicColCount)
                {
                    if (strColumnName[5] == '7' || strColumnName[5] == '8' || strColumnName[5] == '9')
                    {
                        if (DymanicColCount < 3)
                        {
                            dataTypeColumn.Width = 250; dataTypeColumn.MinimumWidth = 250;
                        }
                        else
                        {
                            dataTypeColumn.Width = 210; dataTypeColumn.MinimumWidth = 210;
                        }
                    }
                    else
                    {
                        dataTypeColumn.Width = 70; /* 50; */dataTypeColumn.MinimumWidth = 70;/*50;*/
                        if (strColumnName[5] == '6')
                            dataTypeColumn.Width = dataTypeColumn.MinimumWidth = 83;/*65;*/

                        dataTypeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    SetColumnMaxLength(dataTypeColumn, strColumnName[5]);
                    //dataTypeColumn.gvwAgencyTableCellStyle2.Format = "N5";



                }
                else
                {
                    dataTypeColumn.Width = 90;/*55;*/
                    dataTypeColumn.MinimumWidth = 90;/*55;*/
                    dataTypeColumn.MaxInputLength = 7;  //09052012
                    dataTypeColumn.DefaultCellStyle.Format = "N5";
                    dataTypeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dataTypeColumn.HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    //Kranthi:: hiding A1, A2, A3, A4 columns by default on 11/24/2022
                    //Kranthi:: Again visible A1, A2, A3, A4 columns by default on 05/25/2023
                    dataTypeColumn.Visible = true;

                }
                gvwAgencyTable.Columns.Add(dataTypeColumn);
                TempCount++;
            }


            DataGridViewCheckBoxColumn cbColumn = new DataGridViewCheckBoxColumn();
            cbColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            cbColumn.DefaultHeaderCellType = typeof(DataGridViewColumnHeaderCell);
            cbColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            cbColumn.HeaderText = "Active";
            cbColumn.Name = "Active";
            cbColumn.Width = 60;
            cbColumn.MinimumWidth = 60;
            gvwAgencyTable.Columns.Add(cbColumn);

            cbColumn = new DataGridViewCheckBoxColumn();
            cbColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            cbColumn.DefaultHeaderCellType = typeof(DataGridViewColumnHeaderCell);
            cbColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            cbColumn.HeaderText = "Default";
            cbColumn.Name = "Default";
            cbColumn.Width = 70;
            cbColumn.MinimumWidth = 70;
            gvwAgencyTable.Columns.Add(cbColumn);

            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            btnColumn.DefaultHeaderCellType = typeof(DataGridViewColumnHeaderCell);
            btnColumn.Text = "Select Hierarchy";
            btnColumn.Name = "Hierarchy";
            btnColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            btnColumn.Width = 70;
            btnColumn.MinimumWidth = 70;
            btnColumn.DefaultCellStyle.Alignment= DataGridViewContentAlignment.TopCenter;
            //btnColumn.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(20);
            gvwAgencyTable.Columns.Add(btnColumn);

            dataTypeColumn = new DataGridViewTextBoxColumn();
            dataTypeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "Agy Code";
            dataTypeColumn.Name = "Agy_Code";
            dataTypeColumn.Resizable = DataGridViewTriState.True;
            dataTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Visible = false;
            dataTypeColumn.ShowInVisibilityMenu = false;
            gvwAgencyTable.Columns.Add(dataTypeColumn);

            dataTypeColumn = new DataGridViewTextBoxColumn();
            dataTypeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataTypeColumn.DefaultHeaderCellType = typeof(DataGridViewColumnHeaderCell);
            dataTypeColumn.HeaderText = "Selected Hierarchy";
            dataTypeColumn.Name = "SelectedHierarchy";
            dataTypeColumn.Resizable = DataGridViewTriState.True;
            dataTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataTypeColumn.Visible = false;
            dataTypeColumn.ShowInVisibilityMenu = false;
            gvwAgencyTable.Columns.Add(dataTypeColumn);

            if (BaseForm.UserID.ToUpper() == "JAKE")
            {
                DataGridViewImageColumn dataDeleteColumn = new DataGridViewImageColumn();
                dataDeleteColumn.HeaderText = "Del";
                dataDeleteColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataDeleteColumn.DefaultHeaderCellType = typeof(DataGridViewColumnHeaderCell);
                dataDeleteColumn.CellImageSource = "captain-delete";//new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("DeleteItem.gif"); ;
                dataDeleteColumn.CellImageLayout = DataGridViewImageCellLayout.NotSet;
                dataDeleteColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dataDeleteColumn.Resizable = DataGridViewTriState.False;
                dataDeleteColumn.Name = "gviDelCode";
                dataDeleteColumn.ReadOnly = true;
                dataDeleteColumn.Width = 30;
                dataDeleteColumn.Visible = true;
                dataDeleteColumn.ShowInVisibilityMenu = false;
                gvwAgencyTable.Columns.Add(dataDeleteColumn);
            }



            //Kranthi:: hiding A1, A2, A3, A4 columns by default on 11/24/2022
            //gvwAgencyTable.Columns["Cell A1"].Visible = false;
            //gvwAgencyTable.Columns["Cell A2"].Visible = false;
            //gvwAgencyTable.Columns["Cell A3"].Visible = false;
            //gvwAgencyTable.Columns["Cell A4"].Visible = false;

            //if (code == "65453")
            //    code = "65453"
            selRowindex = 0;
            FillDetailsinGrid(code, DynamicAgyCols);
            gvwAgencyTable_SelIndexChanged();
        }

        private void SetColumnMaxLength(DataGridViewTextBoxColumn COlName, char ColSize)
        {
            switch (ColSize)
            {
                case '1': COlName.MaxInputLength = 1; break;
                case '2': COlName.MaxInputLength = 2; break;
                case '3': COlName.MaxInputLength = 3; break;
                case '4': COlName.MaxInputLength = 4; break;
                case '5': COlName.MaxInputLength = 5; break;
                case '6': COlName.MaxInputLength = 10; break;
                case '7': COlName.MaxInputLength = 20; break;
                case '8': COlName.MaxInputLength = 50; break;
                case '9': COlName.MaxInputLength = 80; break;
            }
        }

        int selRowindex = 0;
        private void gvwAgencyTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selRowindex = e.RowIndex;
            int intcolumcount = gvwAgencyTable.ColumnCount;
            if (BaseForm.UserID.ToUpper() == "JAKE")
            {
                intcolumcount = intcolumcount - 1;
            }
            // To Populate Hierarchy Selection Form and to Select the Hierarchyes
            if ((e.ColumnIndex == intcolumcount - 3) && e.RowIndex != -1)
            {
                //HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, SelectedHierarchies, "Hierarchy Selection");
                //hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
                //hierarchieSelectionForm.ShowDialog();

                HierarchieSelectionForm addForm = new HierarchieSelectionForm(BaseForm, SelectedHierarchies, "Service", null, "Add");
                addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                addForm.StartPosition = FormStartPosition.CenterScreen;
                addForm.ShowDialog();

            }

            if ((e.ColumnIndex == intcolumcount) && e.RowIndex != -1)
            {
                DataRow dr = gvwAgencyTable.Rows[e.RowIndex].Tag as DataRow;
                if (dr != null)
                {
                    propAgencyseq = dr["AGY_CODE"].ToString();
                    propAgencyCode = gvwAgencyTable.Rows[e.RowIndex].Cells[0].Value.ToString();

                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnSingleDeleteMessageBoxClicked);
                }

            }

            // To Validate the if there are Multiple Default Child-Codes and to Uncheck to selected 
            if (e.ColumnIndex == intcolumcount - 4)
            {
                string Tmp;

                Tmp = gvwAgencyTable.CurrentRow.Cells["Default"].Value ==null ? "": gvwAgencyTable.CurrentRow.Cells["Default"].Value.ToString();
                DataGridViewRow Seldr = null;
                foreach (DataGridViewRow dr in gvwAgencyTable.SelectedRows)
                    Seldr = dr;
                try
                {
                    if (Tmp == "True")
                    {
                        foreach (DataGridViewRow dr in gvwAgencyTable.Rows)
                        {
                            if (dr.Index >= Child_Rec_Count)
                                break;

                            if (Tmp == Seldr.Cells["Default"].Value.ToString())
                            {
                                if ((Seldr != dr) && (Tmp == dr.Cells["Default"].Value.ToString()))
                                {
                                    AlertBox.Show("You cannot select Multiple as Default", MessageBoxIcon.Warning);
                                    gvwAgencyTable.CurrentRow.Cells["Default"].Value = false;
                                    //boolChangeStatus = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        gvwAgencyTable.CurrentRow.Cells["Default"].Value = false;
                    }
                }
                catch (Exception ex) { boolChangeStatus = false; }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            //TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedItem.Tag as TagClass;


            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                gvwHierarchy.Rows.Clear();
                string hierarchy = string.Empty;
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    int rowIndex = gvwHierarchy.Rows.Add(row.Code, row.HirarchyName.ToString());
                    gvwHierarchy.Rows[rowIndex].Tag = row;
                    //hierarchy += row.Agency + row.Dept + row.Prog;
                    hierarchy += row.Code.Substring(0, 2) + row.Code.Substring(3, 2) + row.Code.Substring(6, 2);
                }
                if (gvwAgencyTable.SelectedRows[0] != null)
                {
                    gvwAgencyTable.SelectedRows[0].Cells["SelectedHierarchy"].Value = hierarchy;
                }
                UpdateHierarchiesToTable = hierarchy;
            }
            if (gvwHierarchy.Rows.Count > 0)
                gvwHierarchy.Rows[0].Selected = true;
        }

        /// <summary>
        /// Handles  grid data error event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataGridViewDataError(object sender, DataGridViewDataErrorEventArgs e) { }


        int Child_Rec_Count = 0;
        public void FillDetailsinGrid(string AgyType, string[] AgyCode)
        {
            List<AgyTabEntity> agencyTableEntity = _model.Agytab.GetAgencyTableCodes(AgyType);
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetAgyTabDetails(AgyType);
            DataTable dt = ds.Tables[0];
            gvwAgencyTable.Rows.Clear();
            Next_Child_Code = 0;
            int Rows = Child_Rec_Count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string TmpCompareChar = dr["AGY_ACTIVE"].ToString();
                bool AgyActive = false, AgyDefault = false;
                if (TmpCompareChar == 'Y'.ToString() || TmpCompareChar == 'y'.ToString())
                    AgyActive = true;
                TmpCompareChar = dr["AGY_DEFAULT"].ToString();
                if (TmpCompareChar == 'Y'.ToString() || TmpCompareChar == 'y'.ToString())
                    AgyDefault = true;

                string[] a = new string[AgyCode.Length];
                int i = 0;
                foreach (string Tmp in AgyCode)
                {
                    a[i] = dr[Tmp].ToString();
                    i++;
                    if (i > 9)
                        break;
                }

                string Tmp_Child_Code = null;
                Tmp_Child_Code = dr["AGY_CODE"].ToString().Trim();

                if (int.Parse(Tmp_Child_Code) > Next_Child_Code)
                    Next_Child_Code = int.Parse(Tmp_Child_Code);

                int rowIndex = 0;
                switch (AgyCode.Length)
                {
                    case 0:
                        rowIndex = gvwAgencyTable.Rows.Add(dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 1:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0].Trim(), dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 2:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0].Trim(), a[1].Trim(), dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 3:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0].Trim(), a[1].Trim(), a[2].Trim(), dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 4:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0].Trim(), a[1].Trim(), a[2].Trim(), a[3].Trim(), dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 5:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0].Trim(), a[1].Trim(), a[2].Trim(), a[3].Trim(), a[4].Trim(), dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 6:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0].Trim(), a[1].Trim(), a[2].Trim(), a[3].Trim(), a[4].Trim(), a[5].Trim(), dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 7:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0].Trim(), a[1].Trim(), a[2].Trim(), a[3].Trim(), a[4].Trim(), a[5].Trim(), a[6].Trim(), dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 8:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                    case 9:
                        rowIndex = gvwAgencyTable.Rows.Add(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], dr["AGY_A1"], dr["AGY_A2"], dr["AGY_A3"], dr["AGY_A4"], AgyActive, AgyDefault, "...", dr["AGY_CODE"], dr["AGY_HIERARCHY"]);
                        gvwAgencyTable.Rows[rowIndex].Tag = dr;
                        break;
                }
                setTooltip(rowIndex, dr);
                Rows++;
            }
            if (dt.Rows.Count == 0)
            {
                //gvwAgencyTable.Rows[0].Cells["Hierarchy"].Value = "Hierarchies";
                //Loading_Complete = true;
                Rows = Child_Rec_Count = 0;
            }
            Child_Rec_Count = Rows;
            if (Rows > 0 && AgyCode.Length > 0)
            {
                //gvwAgencyTable.Rows[1].Tag = 1;
                gvwAgencyTable.CurrentCell = gvwAgencyTable.Rows[0].Cells[0];
                // Vikash commented
                //PrivRow = gvwAgencyTable.SelectedRows[0];

                //Loading_Complete = true;
                if (selRowindex < 0)
                {
                    selRowindex = 0;
                }

                gvwAgencyTable.Rows[selRowindex].Selected = true;

            }
            Loading_Complete = true;
        }

        private void setTooltip(int rowIndex, DataRow dr)
        {
            string toolTipText = "Added By     : " + dr["AGY_ADD_OPERATOR"].ToString().Trim() + " on " + dr["AGY_DATE_ADD"].ToString() + "\n" +
                                 "Modified By  : " + dr["AGY_LSTC_OPERATOR"].ToString().Trim() + " on " + dr["AGY_DATE_LSTC"].ToString();

            foreach (DataGridViewCell cell in gvwAgencyTable.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        public string UpdateSelRowRec()
        {
            string AgyTabCols = null;
            try
            {
                if (gvwAgencyTable != null)
                {
                    foreach (DataGridViewRow dr in gvwAgencyTable.SelectedRows)
                    {
                        if (dr.Selected)
                        {
                            if (PrivRow.Index < Child_Rec_Count)
                            {
                                AgyTabCols = PrivRow.Cells["Agy_Code"].Value.ToString();

                                if (AgyTabCols.Length < 5)
                                    AgyTabCols = PrepareAgyCode(AgyTabCols);

                                RecordMode = 'U';
                            }
                            else
                            {
                                Next_Child_Code++;
                                AgyTabCols = PrepareAgyCode(Next_Child_Code.ToString());
                                gvwAgencyTable.CurrentRow.Cells["Agy_Code"].Value = AgyTabCols;
                                RecordMode = 'I';
                            }
                            Previous_Row_Cnt = PrivRow.Index;

                            AgyTabEntity AgyDetails = new AgyTabEntity();
                            List<AgyTabEntity> AgyList = new List<AgyTabEntity>();
                            if (RecordMode == 'U')
                            {
                                AgyList = _model.Agytab.GetSelAgyChildDetails(DynamicAgytype, AgyTabCols);
                                AgyDetails = AgyList[0];
                            }
                            else
                            {
                                AgyDetails.agytype = DynamicAgytype;
                                AgyDetails.agycode = AgyTabCols;
                            }

                            foreach (string str in DynamicAgyColsNames)
                            {
                                switch (str)
                                {
                                    case "AGY_1":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 1"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy1 = PrivRow.Cells["Cell 1"].Value.ToString();
                                        else
                                            AgyDetails.agy1 = null;
                                        break;
                                    case "AGY_2":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 2"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy2 = PrivRow.Cells["Cell 2"].Value.ToString();
                                        else
                                            AgyDetails.agy2 = null;
                                        break;
                                    case "AGY_3":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 3"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy3 = PrivRow.Cells["Cell 3"].Value.ToString();
                                        else
                                            AgyDetails.agy3 = null;
                                        break;
                                    case "AGY_4":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 4"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy4 = PrivRow.Cells["Cell 4"].Value.ToString();
                                        else
                                            AgyDetails.agy4 = null;
                                        break;
                                    case "AGY_5":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 5"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy5 = PrivRow.Cells["Cell 5"].Value.ToString();
                                        else
                                            AgyDetails.agy5 = null;
                                        break;

                                    case "AGY_6":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 6"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy6 = PrivRow.Cells["Cell 6"].Value.ToString();
                                        else
                                            AgyDetails.agy6 = null;
                                        break;
                                    case "AGY_7":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 7"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy7 = PrivRow.Cells["Cell 7"].Value.ToString();
                                        else
                                            AgyDetails.agy7 = null;
                                        break;
                                    case "AGY_8":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 8"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy8 = PrivRow.Cells["Cell 8"].Value.ToString();
                                        else
                                            AgyDetails.agy8 = null;
                                        break;
                                    case "AGY_9":
                                        if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell 9"].EditedFormattedValue.ToString())))
                                            AgyDetails.agy9 = PrivRow.Cells["Cell 9"].Value.ToString();
                                        else
                                            AgyDetails.agy9 = null;
                                        break;
                                        //case "AGY_2": AgyDetails.agy2 = PrivRow.Cells["Cell 2"].Value.ToString(); break;
                                }
                            }

                            if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell A1"].EditedFormattedValue.ToString())))
                                AgyDetails.agya1 = PrivRow.Cells["Cell A1"].Value.ToString();
                            else
                                AgyDetails.agya1 = "0.00000";

                            if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell A2"].EditedFormattedValue.ToString())))
                                AgyDetails.agya2 = PrivRow.Cells["Cell A2"].Value.ToString();
                            else
                                AgyDetails.agya2 = "0.00000";

                            if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell A3"].EditedFormattedValue.ToString())))
                                AgyDetails.agya3 = PrivRow.Cells["Cell A3"].Value.ToString();
                            else
                                AgyDetails.agya3 = "0.00000";

                            if (!(string.IsNullOrEmpty(PrivRow.Cells["Cell A4"].EditedFormattedValue.ToString())))
                                AgyDetails.agya4 = PrivRow.Cells["Cell A4"].Value.ToString();
                            else
                                AgyDetails.agya4 = "0.00000";
                            //AgyDetails.agya1 = PrivRow.Cells["Cell A1"].Value.ToString();


                            //AgyDetails.agya1 = Get_Decimal_Equivalent(AgyDetails.agya1);
                            //AgyDetails.agya2 = Get_Decimal_Equivalent(AgyDetails.agya2);
                            //AgyDetails.agya3 = Get_Decimal_Equivalent(AgyDetails.agya3);
                            //AgyDetails.agya4 = Get_Decimal_Equivalent(AgyDetails.agya4);

                            AgyDetails.agya1 = (AgyDetails.agya1.Contains(".") ? AgyDetails.agya1 : (AgyDetails.agya1.Length > 6 ? AgyDetails.agya1.Substring(AgyDetails.agya1.Length - 6, 6) : AgyDetails.agya1));
                            AgyDetails.agya2 = (AgyDetails.agya2.Contains(".") ? AgyDetails.agya2 : (AgyDetails.agya2.Length > 6 ? AgyDetails.agya2.Substring(AgyDetails.agya2.Length - 6, 6) : AgyDetails.agya2));
                            AgyDetails.agya3 = (AgyDetails.agya3.Contains(".") ? AgyDetails.agya3 : (AgyDetails.agya3.Length > 6 ? AgyDetails.agya3.Substring(AgyDetails.agya3.Length - 6, 6) : AgyDetails.agya3));
                            AgyDetails.agya4 = (AgyDetails.agya4.Contains(".") ? AgyDetails.agya4 : (AgyDetails.agya4.Length > 6 ? AgyDetails.agya4.Substring(AgyDetails.agya4.Length - 6, 6) : AgyDetails.agya4));


                            AgyDetails.agyactive = AgyDetails.agydefault = 'N'.ToString();  // 
                            bool Tmp = true;
                            if (RecordMode == 'U')
                            {
                                if (PrivRow.Cells["Active"].Value.ToString() == Tmp.ToString())
                                    AgyDetails.agyactive = 'Y'.ToString();
                                if (PrivRow.Cells["Default"].Value.ToString() == Tmp.ToString())
                                    AgyDetails.agydefault = 'Y'.ToString();
                            }
                            else
                            {
                                if (PrivRow.Cells["Active"].FormattedValue.ToString() == Tmp.ToString())
                                    AgyDetails.agyactive = 'Y'.ToString();
                                if (PrivRow.Cells["Default"].FormattedValue.ToString() == Tmp.ToString())
                                    AgyDetails.agydefault = 'Y'.ToString();
                            }
                            if (UpdateHierarchiesToTable != null)
                                AgyDetails.agyhierarchy = UpdateHierarchiesToTable;
                            if (string.IsNullOrEmpty(AgyDetails.agyhierarchy.Trim()))
                            {
                                if (RecordMode == 'U')
                                    AgyDetails.agyhierarchy = null;
                                else
                                    AgyDetails.agyhierarchy = "******";
                            }

                            bool DefaultCheckStatus = false;
                            if (AgyDetails.agydefault == 'Y'.ToString())
                                DefaultCheckStatus = true;

                            bool ActiveCheckStatus = false;
                            if (AgyDetails.agyactive == 'Y'.ToString())
                                ActiveCheckStatus = true;

                            AgyDetails = FillNullinEmptyCols(AgyDetails);

                            if (DynamicAgytype == "00501")
                            {
                                if (AgyDetails.agy6.Length > 6)
                                {
                                    PrivRow.Cells["Cell 6"].Value = null;
                                    AlertBox.Show("Fund Code should not exceed 6 characters", MessageBoxIcon.Warning);
                                    break;
                                }
                            }

                            bool DupCodeStatus = true;
                            if (ValReqBlankItems)
                                DupCodeStatus = CheckForDupCode();

                            if (DupCodeStatus)
                            {
                                CaptainModel model = new CaptainModel();

                                AgyDetails.agyRowtype = RecordMode.ToString();
                                AgyDetails.agytype = DynamicAgytype;
                                AgyDetails.agycode = AgyTabCols;
                                AgyDetails.agyaddoperator = BaseForm.UserID;
                                AgyDetails.agylstcoperator = BaseForm.UserID;


                                bool boolsucess = model.Agytab.UpdateAGYTAB(AgyDetails);
                                if (boolsucess)
                                {
                                    BaseForm.BaseAgyTabsEntity = model.lookupDataAccess.GetAgyTabs(string.Empty, string.Empty, string.Empty);
                                    if (RecordMode == 'U')
                                        AlertBox.Show("Record Updated Successfully");
                                    //MessageBox.Show("Record Updated Successfully", "CAP Systems", MessageBoxButtons.OK);
                                    else
                                    {
                                        AlertBox.Show("Record Inserted Successfully");
                                        //MessageBox.Show("Record Inserted Successfully", "CAP Systems", MessageBoxButtons.OK);
                                        Child_Rec_Count++;
                                        PrivRow.Cells["Agy_Code"].Value = PrepareAgyCode(Next_Child_Code.ToString());
                                        PrivRow.Cells["Default"].Value = DefaultCheckStatus;
                                        PrivRow.Cells["Active"].Value = ActiveCheckStatus;
                                        PrivRow.Cells["SelectedHierarchy"].Value = AgyDetails.agyhierarchy;
                                    }
                                    //_errorProvider.SetError(Lbl_Req_Controls, null);
                                }
                                else
                                    Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            boolChangeStatus = false;
            return AgyTabCols;
        }

        //private string Get_Decimal_Equivalent(string value)
        //{
        //    double Value = double.Parse(value);

        //    string Retutn_Decimal = "0.00000";
        //    int Tmp = int.Parse(Value.ToString());
        //    string Integra_part = Tmp.ToString();
        //    string Decimal_part_Str = string.Empty;
        //    double Decimal_part = 0.0;


        //    if (Value.ToString().Contains("."))
        //    {
        //        Decimal_part =  Value - int.Parse(Integra_part);

        //        Integra_part = (Integra_part.Length > 6 ? Integra_part.Substring(Integra_part.Length - 6, 6) : Integra_part);
        //        Decimal_part_Str = (Decimal_part.ToString().Length > 5 ? Decimal_part.ToString().Substring(Decimal_part.ToString().Length - 5, 6) : Decimal_part.ToString());
        //    }
        //    else
        //        Integra_part = (Value.ToString().Length > 6 ? Value.ToString().Substring(Value.ToString().Length - 6, 6) : Value.ToString());

        //    Retutn_Decimal = Integra_part + "." + Decimal_part;

        //    return Retutn_Decimal;
        //}

        private AgyTabEntity FillNullinEmptyCols(AgyTabEntity AgyDetails)
        {
            AgyTabEntity TmpAgyDetails = new AgyTabEntity();
            TmpAgyDetails = AgyDetails;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy1))
                TmpAgyDetails.agy1 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy2))
                TmpAgyDetails.agy2 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy3))
                TmpAgyDetails.agy3 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy4))
                TmpAgyDetails.agy4 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy5))
                TmpAgyDetails.agy5 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy6))
                TmpAgyDetails.agy6 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy7))
                TmpAgyDetails.agy7 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy8))
                TmpAgyDetails.agy8 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agy9))
                TmpAgyDetails.agy9 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agya1))
                TmpAgyDetails.agya1 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agya2))
                TmpAgyDetails.agya2 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agya3))
                TmpAgyDetails.agya3 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agya4))
                TmpAgyDetails.agya4 = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agydesc))
                TmpAgyDetails.agydesc = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agyactive))
                TmpAgyDetails.agyactive = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agydefault))
                TmpAgyDetails.agydefault = null;
            if (string.IsNullOrEmpty(TmpAgyDetails.agyhierarchy))
                TmpAgyDetails.agyhierarchy = null;

            return (TmpAgyDetails);
        }

        private bool CheckForDupCode()
        {
            bool ReturnVal = true;
            bool Tmp = true;
            string SelCode = null;
            string SelDesc = null;
            char SelDefault = 'N';

            string TestCode = null;
            string TestDesc = null;
            char TestDefault = 'N';
            string Errorin = null;

            DataGridViewRow Seldr = null;
            foreach (DataGridViewRow dr in gvwAgencyTable.SelectedRows)
            {
                Seldr = dr; Seldr = PrivRow;
                if (dr.Selected)
                {
                    DataRow srow = dr.Tag as DataRow;
                    if (!(string.IsNullOrEmpty(PrivRow.Cells[CodeColSubscript].FormattedValue.ToString())))
                        SelCode = PrivRow.Cells[CodeColSubscript].Value.ToString().Trim();
                    if (!(string.IsNullOrEmpty(PrivRow.Cells[DescColSubscript].FormattedValue.ToString())))
                        SelDesc = PrivRow.Cells[DescColSubscript].Value.ToString().Trim();

                    if (RecordMode == 'U' && PrivRow.Cells["Active"].Value.ToString() == Tmp.ToString())
                        SelDefault = 'Y';

                    if (RecordMode == 'I' && PrivRow.Cells["Active"].FormattedValue.ToString() == "True")
                    {
                        SelDefault = 'Y';
                    }
                }
            }
            foreach (DataGridViewRow dr in gvwAgencyTable.Rows)
            {
                if (dr.Index >= Child_Rec_Count)
                    break;
                TestCode = TestDesc = null;
                if (!(string.IsNullOrEmpty(dr.Cells[CodeColSubscript].FormattedValue.ToString())))
                    TestCode = dr.Cells[CodeColSubscript].Value.ToString().Trim();
                if (!(string.IsNullOrEmpty(dr.Cells[DescColSubscript].FormattedValue.ToString())))
                    TestDesc = dr.Cells[DescColSubscript].Value.ToString().Trim();
                if (dr.Cells["Active"].Value.ToString() == Tmp.ToString())
                    TestDefault = 'Y';
                if (((ListItem)cmbAgencyTable.SelectedItem).Value.ToString() != "08550")
                {
                    if (SelCode == TestCode && Seldr != dr)
                    {
                        ReturnVal = false;
                        Errorin = "Code";
                        AlertBox.Show("Code already exists", MessageBoxIcon.Warning);
                        break;
                    }
                }
                if (SelDesc == TestDesc && Seldr != dr)
                {
                    ReturnVal = false;
                    Errorin = "Desc";
                    AlertBox.Show("Code Description already exists", MessageBoxIcon.Warning);
                    break;
                }
            }
            if (ReturnVal == false)
            {
                if (RecordMode == 'U')
                {
                    string selectedRowUser = GetSelectedRow();
                    DataSet ds = Captain.DatabaseLayer.AgyTab.GetSelAgyChildDetails(DynamicAgytype, selectedRowUser);
                    string TmpValue = null;
                    switch (Errorin)
                    {
                        case "Code":
                            TmpValue = "AGY_" + CodeColSubscript.Substring(5, 1);
                            TmpValue = (ds.Tables[0].Rows[0][TmpValue].ToString()).Trim();
                            Seldr.Cells[CodeColSubscript].Value = TmpValue;
                            break;
                        case "Desc":
                            TmpValue = "AGY_" + DescColSubscript.Substring(5, 1);
                            TmpValue = (ds.Tables[0].Rows[0][TmpValue].ToString()).Trim();
                            Seldr.Cells[DescColSubscript].Value = TmpValue;
                            break;
                    }
                }
                else
                {
                    switch (Errorin)
                    {
                        case "Code": Seldr.Cells[CodeColSubscript].Value = null; break;
                        case "Desc": Seldr.Cells[DescColSubscript].Value = null; break;
                    }

                }
            }
            return ReturnVal;
        }

        private bool isCodeExists(string agencyType, string agencyCode)
        {
            bool isExists = false;
            List<AgyTabEntity> agyTabEntity = _model.Agytab.GetSelAgyChildDetails(agencyType, agencyCode);
            if (agyTabEntity.Count > 0)
            {
                isExists = true;
            }

            return isExists;
        }

        private bool ValidateRow(DataGridViewRow row)
        {
            bool isValid = true;
            //foreach (string str in DynamicAgyColsNames)
            //{
            //    switch (str)
            //    {
            //        case "AGY_1":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 1"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 1"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_2":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 2"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 2"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_3":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 3"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 3"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_4":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 4"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 4"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_5":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 5"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 5"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_6":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 6"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 6"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_7":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 7"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 7"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_8":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 8"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 8"));
            //                isValid = false;
            //            }
            //            break;
            //        case "AGY_9":
            //            if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells["Cell 9"].Value + string.Empty.ToString()))
            //            {
            //                _errorProvider.SetError(gvwAgencyTable, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Cell 9"));
            //                isValid = false;
            //            }
            //            break;
            //    }

            //}

            if (!string.IsNullOrEmpty(CodeColSubscript) && !string.IsNullOrEmpty(CodeColSubscript))
            {
                isValid = false;
                if (string.IsNullOrEmpty(PrivRow.Cells[CodeColSubscript].EditedFormattedValue.ToString()) &&
                    string.IsNullOrEmpty(PrivRow.Cells[DescColSubscript].EditedFormattedValue.ToString()))
                    _errorProvider.SetError(Lbl_Req_Controls, string.Format(CodeColSubscript + ", " + DescColSubscript + " are Required", Lbl_Req_Controls.Text.Replace(Consts.Common.Colon, string.Empty)));
                else
                    if (string.IsNullOrEmpty(PrivRow.Cells[DescColSubscript].EditedFormattedValue.ToString()))
                    _errorProvider.SetError(Lbl_Req_Controls, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), DescColSubscript));
                else
                        if (string.IsNullOrEmpty(PrivRow.Cells[CodeColSubscript].EditedFormattedValue.ToString()))
                    _errorProvider.SetError(Lbl_Req_Controls, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), CodeColSubscript));
                else
                    isValid = true;


                if (isValid)
                {
                    _errorProvider.SetError(Lbl_Req_Controls, null);
                }
            }

            return (isValid);

        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            boolChangeStatus = true;
            decimal number;
            int intgrdColumncount = gvwAgencyTable.ColumnCount;
            if (BaseForm.UserID.ToUpper() == "JAKE")
                intgrdColumncount = intgrdColumncount - 1;
            if ((e.ColumnIndex >= intgrdColumncount - 9) && (e.ColumnIndex <= intgrdColumncount - 6))
            {
                if ((!(Decimal.TryParse(gvwAgencyTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out number))) &&
                !(string.IsNullOrEmpty(gvwAgencyTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())))
                {
                    AlertBox.Show("Please enter Decimal Values in ''Cell A1'' to ''Cell A4''", MessageBoxIcon.Warning);
                    //gvwAgencyTable.Columns[e.ColumnIndex].Selected = false;
                    gvwAgencyTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    boolChangeStatus = false;
                }
            }

            //try
            //{
            //    boolChangeStatus = true;
            //    if (ValReqBlankItems)
            //    {
            //        if (int.Parse(CodeColSubscript.Substring(5, 1)) == (e.ColumnIndex + 1) ||
            //            int.Parse(DescColSubscript.Substring(5, 1)) == (e.ColumnIndex + 1))
            //        {
            //            boolChangeStatus = false;
            //            if (string.IsNullOrEmpty(gvwAgencyTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) )//)&&
            //                //string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells[DescColSubscript].Value.ToString()))
            //                MessageBox.Show("'" + CodeColSubscript + "' and '" + DescColSubscript + "'" + " Should not be blank", "CAP Systems", MessageBoxButtons.OK);
            //            //else
            //            //    if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells[DescColSubscript].Value.ToString()))
            //            //        MessageBox.Show("'" + DescColSubscript + "'" + " Should not be blank", "CAP Systems", MessageBoxButtons.OK);
            //            //    else
            //            //        if (string.IsNullOrEmpty(gvwAgencyTable.CurrentRow.Cells[CodeColSubscript].Value.ToString()))
            //            //            MessageBox.Show("'" + CodeColSubscript + "'" + " Should not be blank", "CAP Systems", MessageBoxButtons.OK);
            //            //        else
            //            //        {
            //            //            boolChangeStatus = true;
            //            //        }
            //        }
            //    }
            //}
            //catch (Exception ex) { boolChangeStatus = false; }

        }

        private void dataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (Child_Rec_Count <= 0)
            {
                if (boolChangeStatus && Loading_Complete)
                {
                    DataGridViewRow row = gvwAgencyTable.Rows[e.RowIndex];
                    if (ValidateRow(row))
                    {
                        string agyCode = string.Empty;
                        if (row.Cells["Agy_Code"].Value != null)
                        {
                            agyCode = row.Cells["Agy_Code"].Value.ToString();
                        }
                        AgyTabEntity AgyDetails = new AgyTabEntity();

                        AgyDetails.agya1 = AgyDetails.agya2 = "0.00000";
                        AgyDetails.agya3 = AgyDetails.agya4 = "0.00000";

                        foreach (string str in DynamicAgyColsNames)
                        {
                            switch (str)
                            {
                                case "AGY_1":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 1"].Value != null)
                                        AgyDetails.agy1 = gvwAgencyTable.CurrentRow.Cells["Cell 1"].Value.ToString();
                                    break;
                                case "AGY_2":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 2"].Value != null)
                                        AgyDetails.agy2 = gvwAgencyTable.CurrentRow.Cells["Cell 2"].Value.ToString();
                                    break;
                                case "AGY_3":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 3"].Value != null)
                                        AgyDetails.agy3 = gvwAgencyTable.CurrentRow.Cells["Cell 3"].Value.ToString();
                                    break;
                                case "AGY_4":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 4"].Value != null)
                                        AgyDetails.agy4 = gvwAgencyTable.CurrentRow.Cells["Cell 4"].Value.ToString();
                                    break;
                                case "AGY_5":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 5"].Value != null)
                                        AgyDetails.agy5 = gvwAgencyTable.CurrentRow.Cells["Cell 5"].Value.ToString();
                                    break;
                                case "AGY_6":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 6"].Value != null)
                                        AgyDetails.agy6 = gvwAgencyTable.CurrentRow.Cells["Cell 6"].Value.ToString();
                                    break;
                                case "AGY_7":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 7"].Value != null)
                                        AgyDetails.agy7 = gvwAgencyTable.CurrentRow.Cells["Cell 7"].Value.ToString();
                                    break;
                                case "AGY_8":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 8"].Value != null)
                                        AgyDetails.agy8 = gvwAgencyTable.CurrentRow.Cells["Cell 8"].Value.ToString();
                                    break;
                                case "AGY_9":
                                    if (gvwAgencyTable.CurrentRow.Cells["Cell 9"].Value != null)
                                        AgyDetails.agy9 = gvwAgencyTable.CurrentRow.Cells["Cell 9"].Value.ToString();
                                    break;
                            }
                        }
                        if (gvwAgencyTable.CurrentRow.Cells["Cell A1"].Value != null)
                            AgyDetails.agya1 = gvwAgencyTable.CurrentRow.Cells["Cell A1"].Value.ToString();
                        if (gvwAgencyTable.CurrentRow.Cells["Cell A2"].Value != null)
                            AgyDetails.agya2 = gvwAgencyTable.CurrentRow.Cells["Cell A2"].Value.ToString();
                        if (gvwAgencyTable.CurrentRow.Cells["Cell A3"].Value != null)
                            AgyDetails.agya3 = gvwAgencyTable.CurrentRow.Cells["Cell A3"].Value.ToString();
                        if (gvwAgencyTable.CurrentRow.Cells["Cell A4"].Value != null)
                            AgyDetails.agya4 = gvwAgencyTable.CurrentRow.Cells["Cell A4"].Value.ToString();

                        AgyDetails.agyactive = AgyDetails.agydefault = 'N'.ToString();

                        bool ActiveCheckStatus = false;
                        bool Tmp = true;
                        if (gvwAgencyTable.CurrentRow.Cells["Active"].Value + string.Empty.ToString() == Tmp.ToString())
                        {
                            AgyDetails.agyactive = 'Y'.ToString();
                            ActiveCheckStatus = true;
                        }

                        bool DefaultCheckStatus = false;
                        if (gvwAgencyTable.CurrentRow.Cells["Default"].Value + string.Empty.ToString() == Tmp.ToString())
                        {
                            AgyDetails.agydefault = 'Y'.ToString();
                            DefaultCheckStatus = true;
                        }

                        AgyDetails.agytype = DynamicAgytype;
                        AgyDetails.agycode = "00001";
                        AgyDetails.agyRowtype = 'I'.ToString();
                        AgyDetails.agyaddoperator = BaseForm.UserID;
                        AgyDetails.agylstcoperator = BaseForm.UserID;


                        AgyDetails.agyhierarchy = gvwAgencyTable.CurrentRow.Cells["SelectedHierarchy"].Value + string.Empty.ToString();
                        if (AgyDetails.agyhierarchy.Equals(string.Empty))
                        {
                            AgyDetails.agyhierarchy = "******";
                        }

                        bool ValReqCol = true;
                        if (ValReqCol)
                        {
                            bool boolsucess = _model.Agytab.UpdateAGYTAB(AgyDetails);
                            if (boolsucess)
                            {
                                BaseForm.BaseAgyTabsEntity = _model.lookupDataAccess.GetAgyTabs(string.Empty, string.Empty, string.Empty);
                                //MessageBox.Show("Record Inserted Successfully", "CAP Systems", MessageBoxButtons.OK);
                                //PrivRow.Cells["Default"].Value = DefaultCheckStatus;
                                gvwAgencyTable.CurrentRow.Cells["Default"].Value = DefaultCheckStatus;
                                gvwAgencyTable.CurrentRow.Cells["Active"].Value = ActiveCheckStatus;

                                Child_Rec_Count++;
                                Next_Child_Code++;
                                Loading_Complete = true;
                                PrivRow.Tag = 0;
                                gvwAgencyTable.CurrentRow.Cells["Agy_Code"].Value = PrepareAgyCode(Next_Child_Code.ToString());
                                //_errorProvider.SetError(Lbl_Req_Controls, null);
                            }
                            else
                                Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                            //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                        }
                        boolChangeStatus = false;
                    }
                }
            }
        }

        private string PrepareAgyCode(string TmpCode)
        {
            switch (TmpCode.ToString().Length)
            {

                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
            }
            return (TmpCode);
        }


        private void OnRowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = gvwAgencyTable.Rows[e.RowIndex];

            if (row != null && boolChangeStatus && !ValidateRow(row))
            {
                e.Cancel = true;
            }
        }

        DataTable dt;
        private void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = ((ListItem)cmbModule.SelectedItem).Value.ToString();
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetAgencyTableByApp(module);
            cmbAgencyTable.Items.Clear();
            gvwAgencyTable.Visible = Lbl_Req_Controls.Visible = false;
            gvwHierarchy.Rows.Clear();
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                Fill_AgyType_Combo();
            }
        }

        private void Fill_AgyType_Combo()
        {
            if (dt.Rows.Count > 0)
            {
                DataRow[] foundRows;

                if (RbCode.Checked)
                    foundRows = dt.Select(null, "AGY_TYPE ASC");
                else
                    foundRows = dt.Select(null, "AGY_DESC ASC");

                Loading_Complete = boolChangeStatus = ValReqBlankItems = false;
                Previous_Row_Cnt = 0;
                gvwHierarchy.Rows.Clear();
                cmbAgencyTable.Items.Clear();
                string Tmp_Desc = string.Empty;

                foreach (DataRow dr in foundRows)
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-50}", dr["AGY_DESC"].ToString().Trim()) + String.Format("{0,8}", " - " + dr["AGY_TYPE"].ToString());

                    cmbAgencyTable.Items.Add(new ListItem(Tmp_Desc, dr["AGY_TYPE"].ToString()));
                }
                if (cmbAgencyTable.Items.Count > 0)
                    cmbAgencyTable.SelectedIndex = 0;
                //OnAgencyTableSelectedIndexChanged(cmbAgencyTable, new EventArgs());
            }
        }


        private void menuItemDel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteCodeMessageBoxClicked);
        }

        /// <summary>
        /// Handles the message box click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteCodeMessageBoxClicked(DialogResult dialogResult)
        {
            // MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
                if (gvwAgencyTable.SelectedRows.Count > 0)
                {
                    if (gvwAgencyTable.SelectedRows[0].Tag is AgyTabEntity)
                    {
                        AgyTabEntity agyTabEntity = gvwAgencyTable.SelectedRows[0].Tag as AgyTabEntity;
                        if (_model.Agytab.DeleteAGYTAB(agyTabEntity.agytype, agyTabEntity.agycode))
                        {
                            OnAgencyTableSelectedIndexChanged(cmbAgencyTable, new EventArgs());
                        }
                    }
                }
                AlertBox.Show("Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
            }
        }

        private void gvwAgencyTable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int intcolumcount = gvwAgencyTable.ColumnCount;
            if (BaseForm.UserID.ToUpper() == "JAKE")
            {
                intcolumcount = intcolumcount - 1;
            }

            if (intcolumcount > 9)
            {
                gvwAgencyTable.Rows[e.RowIndex].Cells["Hierarchy"].Value = "...";
                //gvwAgencyTable.Rows[e.RowIndex].Cells[0].Selected = true;
            }
        }


        private bool CanDeleteHeaderCode()
        {
            bool Tmp = true;
            bool CanDelete = true;
            //Tmp = true;
            foreach (DataGridViewRow dr in gvwAgencyTable.Rows)
            {
                if (dr.Index >= Child_Rec_Count)
                    break;
                if (dr.Cells["Active"].Value.ToString() == Tmp.ToString())
                    CanDelete = false;
            }
            return (CanDelete);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //if (splitContainer1.SplitterDistance < 175)
            //    this.splitContainer1.SplitterDistance = 175;
        }

        private void RbDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (RbDesc.Checked)
                Fill_AgyType_Combo();
        }

        private void RbCode_CheckedChanged(object sender, EventArgs e)
        {
            if (RbCode.Checked)
                Fill_AgyType_Combo();
        }

        //private void btnConverstion_Click(object sender, EventArgs e)
        //{
        //    ConverstionForm converstionform = new ConverstionForm(BaseForm);
        //    converstionform.ShowDialog();
        //}


        string Agency = null;
        string Random_Filename = null; string PdfName = null;
        private void ExcelAgencyTable(string FileName)
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "AgencyTable_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm")+ "Report";

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
                string Tmpstr = PdfName + ".xls";
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

            Worksheet sheet = book.Worksheets.Add("Sheet1");
            sheet.Table.DefaultRowHeight = 14.25F;

            string module = ((ListItem)cmbModule.SelectedItem).Value.ToString();
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetAgencyTableByApp(module);

            //DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetaAgyTabList(FileName);
            DataTable dt = ds.Tables[0];

            sheet.Table.Columns.Add(120);
            sheet.Table.Columns.Add(250);
            //sheet.Table.Columns.Add(250);
            //sheet.Table.Columns.Add(52);
            //sheet.Table.Columns.Add(75);
            //sheet.Table.Columns.Add(75);

            string Agy_Code_Cell = string.Empty;
            string Agy_Desc_Cell = string.Empty;
            string PrivAgyType = string.Empty;
            string HeaderDesc = string.Empty;

            string Code_cell = string.Empty;
            string Desc_Cell = string.Empty;

            bool First = true;

            if (dt.Rows.Count > 0)
            {
                DataTable dtCodes = new DataTable();

                dtCodes = dt;
                DataView dvAgyCodes = new DataView(dtCodes);
                if (((ListItem)cmbModule.SelectedItem).Value.ToString() != "*****")
                {
                    dvAgyCodes.RowFilter = "AGY_TYPE= '" + ((ListItem)cmbAgencyTable.SelectedItem).Value.ToString() + "'";
                }

                //dvAgyCodes.RowFilter = "AGY_CODE='00000'";
                if (RbDesc.Checked)
                    dvAgyCodes.Sort = "AGY_DESC";
                else if (!RbDesc.Checked) dvAgyCodes.Sort = "AGY_TYPE";
                dtCodes = dvAgyCodes.ToTable();

                WorksheetRow Row0 = sheet.Table.Rows.Add();

                WorksheetCell cell;

                foreach (DataRow dr in dtCodes.Rows)
                {

                    int number;
                    if (dr["AGY_CODE"].ToString() == "00000")
                    {

                        if (string.IsNullOrEmpty(dr["AGY_ACTIVE"].ToString()) || string.IsNullOrEmpty(dr["AGY_DEFAULT"].ToString()) ||
                          (!(int.TryParse(dr["AGY_ACTIVE"].ToString(), out number))) || (!(int.TryParse(dr["AGY_DEFAULT"].ToString(), out number))))
                        {
                            PrivAgyType = dr["AGY_TYPE"].ToString(); goto NextAgyType;
                        }
                    }

                    if (dr["AGY_TYPE"].ToString() != PrivAgyType)
                    {
                        HeaderDesc = dr["AGY_DESC"].ToString().Trim();

                        Agy_Code_Cell = "AGY_" + dr["AGY_ACTIVE"].ToString();
                        Agy_Desc_Cell = "AGY_" + dr["AGY_DEFAULT"].ToString();

                        Code_cell = dr["AGY_ACTIVE"].ToString();
                        Desc_Cell = dr["AGY_DEFAULT"].ToString();

                        if (!First)
                        {
                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                        }


                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add(HeaderDesc.Trim(), DataType.String, "s96");
                        cell.MergeAcross = 1;

                        First = false; PrivAgyType = dr["AGY_TYPE"].ToString();
                    }
                    DataSet dsdetails = Captain.DatabaseLayer.AgyTab.GetAgyTabDetails(PrivAgyType);
                    //DataView dv = new DataView(dt);
                    //dv.RowFilter = "AGY_TYPE=" + "'" + PrivAgyType + "'";
                    DataTable dtDetail = dsdetails.Tables[0];



                    if (dtDetail.Rows.Count > 0)
                    {
                        Row0 = sheet.Table.Rows.Add();

                        cell = Row0.Cells.Add("Code (Cell " + Code_cell + ")", DataType.String, "s96");
                        cell = Row0.Cells.Add("Description (Cell " + Desc_Cell + ")", DataType.String, "s96");

                        foreach (DataRow dr1 in dtDetail.Rows)
                        {
                            Row0 = sheet.Table.Rows.Add();

                            if (dr1["AGY_TYPE"].ToString() == PrivAgyType && dr1["AGY_CODE"].ToString() != "00000")
                            {
                                if (!string.IsNullOrEmpty(Agy_Code_Cell.Trim()))
                                    cell = Row0.Cells.Add(dr1[Agy_Code_Cell].ToString().Trim(), DataType.String, "s95C");
                                else
                                    cell = Row0.Cells.Add("", DataType.String, "s95C");

                                if (!string.IsNullOrEmpty(Agy_Desc_Cell.Trim()))
                                    cell = Row0.Cells.Add(dr1[Agy_Desc_Cell].ToString().Trim(), DataType.String, "s95");
                                else
                                    cell = Row0.Cells.Add("", DataType.String, "s95");


                            }
                        }

                    }
                NextAgyType: continue;
                }
            }

            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            //FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //downloadGateway.Filename = "AgencyTable_Report.xls";

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
            s95.Alignment.Vertical = StyleVerticalAlignment.Center;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");

            // -----------------------------------------------
            //  s95C
            // -----------------------------------------------
            WorksheetStyle s95C = styles.Add("s95C");
            s95C.Font.FontName = "Arial";
            s95C.Font.Color = "#000000";
            s95C.Interior.Color = "#FFFFFF";
            s95C.Interior.Pattern = StyleInteriorPattern.Solid;
            s95C.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s95C.Alignment.Vertical = StyleVerticalAlignment.Center;
            s95C.Alignment.WrapText = true;
            s95C.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95C.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95C.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95C.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95C.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
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
            s96.Font.Bold = true;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s96.Alignment.Vertical = StyleVerticalAlignment.Center;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s96.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");


            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.Bold = true;
            s97.Font.FontName = "Arial";
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.NumberFormat = "0%";
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