/* ********************************************************************** *
 * Conversion On    :   01/02/2023      * Converted By     :   Kranthi
 * Modified On      :   01/02/2023      * Modified By      :   Kranthi
 * ********************************************************************* */
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.DatabaseLayer;
using Wisej.Web;
using DevExpress.XtraRichEdit.Model;
using DevExpress.Charts.Heatmap.Native;
using System.Linq;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class CASE0026Control : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        int selRow = 0;
        CaptainModel _model = null;
        #endregion

        string strFunddesc = string.Empty;
        public int strIndex = 0;
        public int strPageIndex = 1;
        public string strDeletMsg = string.Empty;
        public string SortColumn = string.Empty;
        public string SortOrder = string.Empty;
        public string strQuesID = string.Empty;
        public CASE0026Control(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();

            //propfundingsource = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "03320", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            propfundingsource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
            GetBudgetDetails(string.Empty);
            PopulateToolbar(oToolbarMnustrip);

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        List<CommonEntity> propfundingsource { get; set; }

        #endregion


        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (ToolBarNew == null)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "New Budget Maintenance";
                ToolBarNew.Visible = true;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Budget Maintenance";
                ToolBarEdit.Visible = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Budget Maintenance";
                ToolBarDel.Visible = true;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);


                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Budget Maintenance Help";
                ToolBarHelp.Visible = true;
                ToolBarHelp.ImageSource = "icon-help";
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }

            if (Privileges.AddPriv.Equals("false"))
            {
                if (ToolBarNew != null) ToolBarNew.Visible = false;
            }
            else
            {
                if (ToolBarNew != null) ToolBarNew.Visible = true;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                if (ToolBarEdit != null) ToolBarEdit.Visible = false;
            }
            else
            {
                if (ToolBarEdit != null) ToolBarEdit.Visible = true;
            }

            if (Privileges.DelPriv.Equals("false"))
            {
                if (ToolBarDel != null) ToolBarDel.Visible = false;
            }
            else
            {
                if (ToolBarDel != null) ToolBarDel.Visible = true;
            }

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                ToolBarHelp
            });

            if (gvwBudget.Rows.Count == 0)
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Visible = false;
                if (ToolBarDel != null)
                    ToolBarDel.Visible = false;
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
                            if (gvwBudget.SortOrder.ToString() != "None")
                            { SortColumn = gvwBudget.SortedColumn.Name.ToString(); SortOrder = gvwBudget.SortOrder.ToString(); }
                            CMBDCEntity emsbdcentity = new CMBDCEntity();
                            CASE0026Form addUserForm = new CASE0026Form(BaseForm, "Add", null, Privileges, emsbdcentity);
                            addUserForm.StartPosition = FormStartPosition.CenterScreen;
                            addUserForm.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                            if (gvwBudget.SortOrder.ToString() != "None")
                            { SortColumn = gvwBudget.SortedColumn.Name.ToString(); SortOrder = gvwBudget.SortOrder.ToString(); }
                            string selectedRowCostcenter = GetSelectedRow();
                            if (selectedRowCostcenter != null)
                            {
                                CMBDCEntity emsBdcData = gvwBudget.SelectedRows[0].Tag as CMBDCEntity;
                                if (emsBdcData != null)
                                {
                                    //string strSucess = EMSBDCDB.GETEMSLOCKVALIDDATA(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, emsBdcData.BDC_COST_CENTER, emsBdcData.BDC_GL_ACCOUNT, emsBdcData.BDC_BUDGET_YEAR, emsBdcData.BDC_INT_ORDER, emsBdcData.BDC_ACCOUNT_TYPE, string.Empty, string.Empty, "EMSBDC");
                                    //if (strSucess.ToUpper() == "SUCCESS")
                                    //{
                                    //_model.EMSBDCData.InsertEMSLOCKDATA(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, emsBdcData.BDC_FUND, string.Empty, string.Empty, string.Empty, emsBdcData.BDC_COST_CENTER, emsBdcData.BDC_GL_ACCOUNT, emsBdcData.BDC_BUDGET_YEAR, emsBdcData.BDC_INT_ORDER, emsBdcData.BDC_ACCOUNT_TYPE, emsBdcData.BDC_START, emsBdcData.BDC_END, "LOCKON", "EMSBDC", BaseForm.UserID, Privileges.Program);
                                    CASE0026Form editUserForm = new CASE0026Form(BaseForm, "Edit", selectedRowCostcenter, Privileges, emsBdcData);
                                    editUserForm.StartPosition = FormStartPosition.CenterScreen;
                                    editUserForm.ShowDialog();
                                    //}
                                    //else
                                    //{
                                    //    strSucess = strSucess.Replace("\\n", "\n");
                                    //    if (strSucess.Contains("Locked"))
                                    //    {
                                    //        CommonFunctions.MessageBoxDisplay(strSucess);
                                    //    }
                                    //}
                                }
                            }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (gvwBudget.Rows.Count > 0)
                        {
                            if (gvwBudget.SortOrder.ToString() != "None")
                            { SortColumn = gvwBudget.SortedColumn.Name.ToString(); SortOrder = gvwBudget.SortOrder.ToString(); }
                            selectedRowCostcenter = GetSelectedRow();
                            if (selectedRowCostcenter != null)
                            {
                                CMBDCEntity emsBdcData = gvwBudget.SelectedRows[0].Tag as CMBDCEntity;
                                if (emsBdcData != null)
                                {
                                    //string strSucess = EMSBDCDB.GETEMSLOCKVALIDDATA(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, emsBdcData.BDC_COST_CENTER, emsBdcData.BDC_GL_ACCOUNT, emsBdcData.BDC_BUDGET_YEAR, emsBdcData.BDC_INT_ORDER, emsBdcData.BDC_ACCOUNT_TYPE, string.Empty, string.Empty, "EMSBDC");
                                    //if (strSucess.ToUpper() == "SUCCESS")
                                    //{
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                                    //}
                                    //else
                                    //{
                                    //    strSucess = strSucess.Replace("\\n", "\n");
                                    //    if (strSucess.Contains("Locked"))
                                    //    {
                                    //        CommonFunctions.MessageBoxDisplay(strSucess);
                                    //    }
                                    //}
                                }
                            }
                        }
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
        string SelCode = string.Empty;
        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                if (gvwBudget.Rows.Count > 0)
                {
                    string strstatus = string.Empty;
                    if (gvwBudget.SelectedRows[0].Selected)
                    {
                        CMBDCEntity emsbdcdata = gvwBudget.SelectedRows[0].Tag as CMBDCEntity;
                        CMBDCEntity emsbdcentity = new CMBDCEntity();
                        emsbdcentity.BDC_AGENCY = BaseForm.BaseAgency;
                        emsbdcentity.BDC_DEPT = BaseForm.BaseDept;
                        emsbdcentity.BDC_PROGRAM = BaseForm.BaseProg;
                        emsbdcentity.BDC_YEAR = emsbdcdata.BDC_YEAR;

                        emsbdcentity.BDC_FUND = emsbdcdata.BDC_FUND;
                        emsbdcentity.BDC_ID = emsbdcdata.BDC_ID;
                        emsbdcentity.BDC_START = emsbdcdata.BDC_START;
                        emsbdcentity.BDC_END = emsbdcdata.BDC_END;
                        emsbdcentity.Mode = "Delete";
                        SelCode = string.Empty;

                        int SelIndex = gvwBudget.CurrentRow.Index;
                        if (gvwBudget.Rows.Count - 1 >= SelIndex + 1)
                            SelCode = gvwBudget.Rows[SelIndex + 1].Cells["gvBDC_ID"].Value.ToString();
                        else
                            SelCode = gvwBudget.Rows[0].Cells["gvBDC_ID"].Value.ToString();

                        if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcentity, out strstatus))
                        {
                            if (strIndex != 0)
                                strIndex = strIndex - 1;
                            AlertBox.Show("Fund: " + gvwBudget.CurrentRow.Cells["gvtfcode"].Value.ToString() + " with Description: " + gvwBudget.CurrentRow.Cells[gvDesc].Value.ToString() + "\n" + "Deleted Successfully");
                            GetBudgetDetails(SelCode);
                            //** if (gvwBudget.Rows.Count>0)
                            //** gvwBudget.Rows[strIndex].Selected= true;

                            if (!string.IsNullOrEmpty(SortOrder))
                            {
                                gvwBudget.SortedColumn.Name = SortColumn;
                                if (SortOrder == "Ascending")
                                    this.gvwBudget.Sort(this.gvwBudget.Columns[SortColumn], ListSortDirection.Ascending);
                                else
                                    this.gvwBudget.Sort(this.gvwBudget.Columns[SortColumn], ListSortDirection.Descending);
                            }
                        }
                        else
                        {
                            if (strstatus == "EXISTS")
                            {
                                AlertBox.Show("You can’t delete this budget "+ emsbdcdata.BDC_DESCRIPTION+", as there are Dependices.", MessageBoxIcon.Warning);
                            }
                            else
                            {
                                AlertBox.Show("You can’t delete this budget, as there are Dependices.", MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
        }


        private void GetBudgetDetails(string Sel_Code)
        {
            string strFund = string.Empty;
            string strDesc = string.Empty;
            int RowCnt = 0; int SelIndex = 0;
            if (rdoFund.Checked)
                strFund = txtName.Text.Trim();
            else
                strDesc = txtName.Text.Trim();
            gvwBudget.Rows.Clear();
            List<CMBDCEntity> Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, strDesc, strFund);
            if (Emsbdc_List.Count > 0)
            {
                Emsbdc_List = Emsbdc_List.OrderBy(u => Convert.ToInt32(u.BDC_ID)).ToList();
                foreach (CMBDCEntity budgetdetails in Emsbdc_List)
                {
                    strFunddesc = string.Empty;
                    if (propfundingsource.Count > 0)
                    {
                        CommonEntity commonfunddesc = propfundingsource.Find(u => u.Code == budgetdetails.BDC_FUND);
                        if (commonfunddesc != null)
                            strFunddesc = commonfunddesc.Desc.ToString();
                    }
                    int rowIndex = gvwBudget.Rows.Add(budgetdetails.BDC_FUND, strFunddesc, budgetdetails.BDC_DESCRIPTION.Trim(), LookupDataAccess.Getdate(budgetdetails.BDC_START), LookupDataAccess.Getdate(budgetdetails.BDC_END), budgetdetails.BDC_YEAR,budgetdetails.BDC_ID);
                    gvwBudget.Rows[rowIndex].Tag = budgetdetails;
                    CommonFunctions.setTooltip(rowIndex, budgetdetails.BDC_ADD_OPERATOR, budgetdetails.BDC_DATE_ADD, budgetdetails.BDC_LSTC_OPERATOR, budgetdetails.BDC_DATE_LSTC, gvwBudget);
                    if (Sel_Code.Trim() == budgetdetails.BDC_ID)
                        SelIndex = RowCnt;
                    RowCnt++;
                }
            }
            if (RowCnt > 0)
            {
                if (string.IsNullOrEmpty(Sel_Code))
                {
                    //gvwBudget.Rows[0].Tag = 0;
                    gvwBudget.Rows[0].Selected = true;
                }
                else
                {
                    gvwBudget.CurrentCell = gvwBudget.Rows[SelIndex].Cells[1];
                    int scrollPosition = 0;
                    scrollPosition = gvwBudget.CurrentCell.RowIndex;
                }
            }
            if (gvwBudget.Rows.Count > 0)
            {
                //gvwBudget.Rows[SelIndex].Selected = true;
                //gvwBudget.CurrentCell = gvwBudget.Rows[SelIndex].Cells[1];
                if (Privileges.ChangePriv.Equals("false"))
                {
                    if (ToolBarEdit != null) ToolBarEdit.Visible = false;
                }
                else
                {
                    if (ToolBarEdit != null) ToolBarEdit.Visible = true;
                }

                if (Privileges.DelPriv.Equals("false"))
                {
                    if (ToolBarDel != null) ToolBarDel.Visible = false;
                }
                else
                {
                    if (ToolBarDel != null) ToolBarDel.Visible = true;
                }
            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Visible = false;
                if (ToolBarDel != null)
                    ToolBarDel.Visible = false;
            }
        }

        public void Refresh()
        {

            propfundingsource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
            GetBudgetDetails(string.Empty);

            //strDeletMsg = "Delete";

            //gvwBudget.Rows.Clear();

            //List<EMSBDCEntity> Emsbdc_List = _model.EMSBDCData.GetEmsBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            //if (Emsbdc_List.Count > 0)
            //{
            //    foreach (EMSBDCEntity budgetdetails in Emsbdc_List)
            //    {

            //        strFunddesc = string.Empty;
            //        if (propfundingsource.Count > 0)
            //        {
            //            CommonEntity commonfunddesc = propfundingsource.Find(u => u.Code == budgetdetails.BDC_FUND);
            //            if (commonfunddesc != null)
            //                strFunddesc = commonfunddesc.Desc.ToString();
            //        }
            //        int rowIndex = gvwBudget.Rows.Add(budgetdetails.BDC_FUND, strFunddesc, budgetdetails.BDC_COST_CENTER, budgetdetails.BDC_GL_ACCOUNT.Trim(), budgetdetails.BDC_BUDGET_YEAR, LookupDataAccess.Getdate(budgetdetails.BDC_START), LookupDataAccess.Getdate(budgetdetails.BDC_END));
            //        gvwBudget.Rows[rowIndex].Tag = budgetdetails;
            //        CommonFunctions.setTooltip(rowIndex, budgetdetails.BDC_ADD_OPERATOR, budgetdetails.BDC_DATE_ADD, budgetdetails.BDC_LSTC_OPERATOR, budgetdetails.BDC_DATE_LSTC, gvwBudget);

            //    }
            //}
            //else
            //{
            //    if (strDeletMsg == string.Empty)
            //    {
            //        CommonFunctions.MessageBoxDisplay(Consts.Messages.Recordsornotfound);
            //        GetBudgetDetails();
            //    }
            //}
            //strDeletMsg = string.Empty;


            //if (gvwBudget.Rows.Count != 0)
            //{
            //    gvwBudget.Rows[strIndex].Selected = true;
            //    gvwBudget.CurrentCell = gvwBudget.Rows[strIndex].Cells[1];
            //    gvwBudget.CurrentPage = strPageIndex;
            //    if (Privileges.ChangePriv.Equals("false"))
            //    {
            //        if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
            //    }
            //    else
            //    {
            //        if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
            //    }

            //    if (Privileges.DelPriv.Equals("false"))
            //    {
            //        if (ToolBarDel != null) ToolBarDel.Enabled = false;
            //    }
            //    else
            //    {
            //        if (ToolBarDel != null) ToolBarDel.Enabled = true;
            //    }
            //}
            //else
            //{
            //    if (ToolBarEdit != null)
            //        ToolBarEdit.Enabled = false;
            //    if (ToolBarDel != null)
            //        ToolBarDel.Enabled = false;
            //}
        }


        public void RefreshGrid(string Budgetcode)
        {
            strDeletMsg = "Delete";

            gvwBudget.Rows.Clear();

            string strFund = string.Empty;
            string strDesc = string.Empty;
            if (rdoFund.Checked)
                strFund = txtName.Text.Trim();
            else
                strDesc = txtName.Text.Trim();


            List<CMBDCEntity> Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, strDesc, strFund);

            if (Emsbdc_List.Count > 0)
            {
                Emsbdc_List = Emsbdc_List.OrderBy(u => Convert.ToInt32(u.BDC_ID)).ToList();

                foreach (CMBDCEntity budgetdetails in Emsbdc_List)
                {

                    strFunddesc = string.Empty;
                    if (propfundingsource.Count > 0)
                    {
                        CommonEntity commonfunddesc = propfundingsource.Find(u => u.Code == budgetdetails.BDC_FUND);
                        if (commonfunddesc != null)
                            strFunddesc = commonfunddesc.Desc.ToString();
                    }
                    int rowIndex = gvwBudget.Rows.Add(budgetdetails.BDC_FUND, strFunddesc, budgetdetails.BDC_DESCRIPTION.Trim(), LookupDataAccess.Getdate(budgetdetails.BDC_START), LookupDataAccess.Getdate(budgetdetails.BDC_END), budgetdetails.BDC_YEAR, budgetdetails.BDC_ID);
                    gvwBudget.Rows[rowIndex].Tag = budgetdetails;
                    CommonFunctions.setTooltip(rowIndex, budgetdetails.BDC_ADD_OPERATOR, budgetdetails.BDC_DATE_ADD, budgetdetails.BDC_LSTC_OPERATOR, budgetdetails.BDC_DATE_LSTC, gvwBudget);
                    if (Budgetcode == budgetdetails.BDC_ID)
                    {
                        strIndex = rowIndex;
                        
                    }
                }
            }
            else
            {
                if (strDeletMsg == string.Empty)
                {
                    AlertBox.Show(Consts.Messages.Recordsornotfound,MessageBoxIcon.Error);
                    GetBudgetDetails(string.Empty);
                }
            }
            strDeletMsg = string.Empty;


            if (gvwBudget.Rows.Count != 0)
            {
                gvwBudget.Rows[strIndex].Selected = true;
                gvwBudget.CurrentCell = gvwBudget.Rows[strIndex].Cells[1];
                //gvwBudget.CurrentPage = strPageIndex;
                if (Privileges.ChangePriv.Equals("false"))
                {
                    if (ToolBarEdit != null) ToolBarEdit.Visible = false;
                }
                else
                {
                    if (ToolBarEdit != null) ToolBarEdit.Visible = true;
                }

                if (Privileges.DelPriv.Equals("false"))
                {
                    if (ToolBarDel != null) ToolBarDel.Visible = false;
                }
                else
                {
                    if (ToolBarDel != null) ToolBarDel.Visible = true;
                }
            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Visible = false;
                if (ToolBarDel != null)
                    ToolBarDel.Visible = false;
            }
            if (gvwBudget.Rows.Count > 0)
            {
                gvwBudget.Rows[strIndex].Selected = true;
                //if (ToolBarNew != null)
                //{
                //    ToolBarNew.Visible = ToolBarEdit.Visible = ToolBarDel.Visible = ToolBarHelp.Visible = true;
                //}
            }
            //else
            //{
            //    if (ToolBarNew != null)
            //    {
            //        ToolBarNew.Visible = true;
            //        ToolBarEdit.Visible = ToolBarDel.Visible = ToolBarHelp.Visible = false;
            //    }
            //}
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRow()
        {
            string BudgetCodeID = null;
            if (gvwBudget != null)
            {
                foreach (DataGridViewRow dr in gvwBudget.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        selRow = strIndex = gvwBudget.SelectedRows[0].Index;
                        //strPageIndex = gvwBudget.CurrentPage;
                        CMBDCEntity Budgetdetails = dr.Tag as CMBDCEntity;
                        if (Budgetdetails != null)
                        {
                            BudgetCodeID = Budgetdetails.BDC_AGENCY + Budgetdetails.BDC_DEPT + Budgetdetails.BDC_PROGRAM + Budgetdetails.BDC_YEAR + Budgetdetails.BDC_FUND + Budgetdetails.BDC_ID;
                            break;
                        }
                    }
                }
            }
            return BudgetCodeID;
        }


        private void onSearch_Click(object sender, EventArgs e)
        {
            gvwBudget.Rows.Clear();

            string strFund = string.Empty;
            string strDesc = string.Empty;
            if (rdoFund.Checked)
                strFund = txtName.Text.Trim();
            else
                strDesc = txtName.Text.Trim();

            List<CMBDCEntity> Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, strDesc, strFund);

            if (Emsbdc_List.Count > 0)
            {
                Emsbdc_List = Emsbdc_List.OrderBy(u => Convert.ToInt32(u.BDC_ID)).ToList();
                foreach (CMBDCEntity budgetdetails in Emsbdc_List)
                {

                    strFunddesc = string.Empty;
                    if (propfundingsource.Count > 0)
                    {
                        CommonEntity commonfunddesc = propfundingsource.Find(u => u.Code == budgetdetails.BDC_FUND);
                        if (commonfunddesc != null)
                            strFunddesc = commonfunddesc.Desc.ToString();
                    }
                    int rowIndex = gvwBudget.Rows.Add(budgetdetails.BDC_FUND, strFunddesc, budgetdetails.BDC_DESCRIPTION.Trim(), LookupDataAccess.Getdate(budgetdetails.BDC_START), LookupDataAccess.Getdate(budgetdetails.BDC_END), budgetdetails.BDC_YEAR, budgetdetails.BDC_ID);
                    gvwBudget.Rows[rowIndex].Tag = budgetdetails;
                    CommonFunctions.setTooltip(rowIndex, budgetdetails.BDC_ADD_OPERATOR, budgetdetails.BDC_DATE_ADD, budgetdetails.BDC_LSTC_OPERATOR, budgetdetails.BDC_DATE_LSTC, gvwBudget);

                }
            }
            else
            {
                if (strDeletMsg == string.Empty)
                {
                    AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Error);
                    GetBudgetDetails(string.Empty);
                }
            }
            strDeletMsg = string.Empty;
            if (gvwBudget.Rows.Count != 0)
            {
                if (Privileges.ChangePriv.Equals("false"))
                {
                    if (ToolBarEdit != null) ToolBarEdit.Visible = false;
                }
                else
                {
                    if (ToolBarEdit != null) ToolBarEdit.Visible = true;
                }

                if (Privileges.DelPriv.Equals("false"))
                {
                    if (ToolBarDel != null) ToolBarDel.Visible = false;
                }
                else
                {
                    if (ToolBarDel != null) ToolBarDel.Visible = true;
                }
            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Visible = false;
                if (ToolBarDel != null)
                    ToolBarDel.Visible = false;
            }
            if (gvwBudget.Rows.Count > 0)
            {
                gvwBudget.Rows[0].Selected = true;
                //if (ToolBarNew != null)
                //{
                //    ToolBarNew.Visible = ToolBarEdit.Visible = ToolBarDel.Visible = ToolBarHelp.Visible = true;
                //}
            }
            //else
            //{
            //    if (ToolBarNew != null)
            //    {
            //        ToolBarNew.Visible = true;
            //        ToolBarEdit.Visible = ToolBarDel.Visible = ToolBarHelp.Visible = false;
            //    }
            //}
        }

        private void rdoFund_CheckedChanged(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            if (rdoFund.Checked)
                txtName.MaxLength = 6;
            else
                txtName.MaxLength = 40;
        }

        private void gvwBudget_DoubleClick(object sender, EventArgs e)
        {
            if (gvwBudget.Rows.Count > 0)
            {
                string selectedRowCostcenter = GetSelectedRow();
                if (selectedRowCostcenter != null)
                {
                    //ADMN0023Form editUserForm = new ADMN0023Form(BaseForm, "View", selectedRowCostcenter, Privileges, gvwBudget.SelectedRows[0].Tag as EMSBDCEntity);
                    //editUserForm.ShowDialog();
                }
            }
        }


    }
}