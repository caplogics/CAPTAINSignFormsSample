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
using System.IO;
using CarlosAg.ExcelXmlWriter;

#endregion


namespace Captain.Common.Views.UserControls
{
    public partial class ADMN0022Control : BaseUserControl
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private GridControl _serviceHierarchy = null;
        CaptainModel _model = null;
        #endregion
        public string strZipCode { get; set; }
        public int strIndex = 0;
        public int strPageIndex = 1;
        public string strDeletMsg = string.Empty;


        public ADMN0022Control(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();

            propReportPath = _model.lookupDataAccess.GetReportPath();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propsalQlinkEntitylist = _model.SALDEFData.Browse_SALQLNK(string.Empty, string.Empty, string.Empty);
            Getsaldefdata(string.Empty);
            PopulateToolbar(oToolbarMnustrip);
        }


        public int strQuesIndex = 0;
        public int strGroupIndex = 0;
        public string SortColumn = string.Empty;
        public string SortOrder = string.Empty;
        public string strQuesID = string.Empty;
        public void Refresh(string strType, string SALID)
        {
            propsalQlinkEntitylist = _model.SALDEFData.Browse_SALQLNK(string.Empty, string.Empty, string.Empty);
            strQuesID = string.Empty;
            switch (strType)
            {
                case "S":
                    Getsaldefdata(SALID);
                    if(!string.IsNullOrEmpty(SortOrder))
                    {
                        gvwServices.SortedColumn.Name = SortColumn;
                        if(SortOrder== "Ascending")
                            this.gvwServices.Sort(this.gvwServices.Columns[SortColumn], ListSortDirection.Ascending);
                        else
                            this.gvwServices.Sort(this.gvwServices.Columns[SortColumn], ListSortDirection.Descending);
                    }
                    break;
                case "G":
                    strQuesID = SALID;
                    gvwServices_SelectionChanged(gvwServices, new EventArgs());
                    //if (gvwGroup.Rows.Count > 0)
                    //{
                    //    if (gvwGroup.Rows.Count > strGroupIndex)
                    //    {
                    //        gvwGroup.Rows[strGroupIndex].Selected = true;
                    //       // gvwGroup.CurrentCell = gvwGroup.Rows[strGroupIndex].Cells[1];
                    //    }
                    //    else
                    //    {
                    //        gvwGroup.Rows[0].Selected = true;
                    //      //  gvwGroup.CurrentCell = gvwGroup.Rows[0].Cells[1];
                    //    }
                    //}

                    break;
                case "Q":
                    strQuesID = SALID;
                    gvwGroup_SelectionChanged(gvwServices, new EventArgs());
                    //if (gvwQues.Rows.Count > 0)
                    //{
                    //    if (gvwQues.Rows.Count > strQuesIndex)
                    //    {
                    //        gvwQues.Rows[strQuesIndex].Selected = true;
                    //      //   gvwQues.CurrentCell = gvwQues.Rows[strQuesIndex].Cells[1];
                    //    }
                    //    else
                    //    {
                    //        gvwQues.Rows[0].Selected = true;
                    //     //   gvwQues.CurrentCell = gvwQues.Rows[0].Cells[1];
                    //    }
                    //}
                    break;
                default:
                    break;
            }

        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        List<SALQLNKEntity> propsalQlinkEntitylist { get; set; }

        public string propReportPath { get; set; }

        #endregion

        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (toolBar.Controls.Count==0)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Add New SAL/CAL Definition";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";//new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit SAL/CAL Definition";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit"; //new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete SAL/CAL Definition";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete"; // new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.ToolTipText = "Report in Excel";
                ToolBarExcel.Enabled = true;
                ToolBarExcel.ImageSource = "captain-excel";
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "SAL/CAL Definition Help";
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
                        if(gvwServices.SortOrder.ToString()!="None")
                        { SortColumn = gvwServices.SortedColumn.Name.ToString();SortOrder = gvwServices.SortOrder.ToString(); }
                        ADMN0022Form addUserForm = new ADMN0022Form(BaseForm, "Add", null, Privileges, "S", string.Empty, null, string.Empty);
                        addUserForm.StartPosition = FormStartPosition.CenterScreen;
                        addUserForm.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                        string selectedRowid = GetSelectedRow();
                        if (selectedRowid != null)
                        {
                            if (gvwServices.SortOrder.ToString() != "None")
                            { SortColumn = gvwServices.SortedColumn.Name.ToString(); SortOrder = gvwServices.SortOrder.ToString(); }
                            ADMN0022Form editUserForm = new ADMN0022Form(BaseForm, "Edit", selectedRowid, Privileges, "S", string.Empty, null, string.Empty);
                            editUserForm.StartPosition = FormStartPosition.CenterScreen;
                            editUserForm.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Excel:
                        if (gvwServices.Rows.Count > 0)
                            On_SaveExcelForm_Closed();
                        break;
                    case Consts.ToolbarActions.Delete:
                        selectedRowid = GetSelectedRow();
                        if (selectedRowid != null)
                        {
                            if (gvwGroup.Rows.Count == 0)
                            {
                                if (gvwServices.SortOrder.ToString() != "None")
                                { SortColumn = gvwServices.SortedColumn.Name.ToString(); SortOrder = gvwServices.SortOrder.ToString(); }
                                SaldefEntity Saldefdetails = gvwServices.SelectedRows[0].Tag as SaldefEntity;
                                if (Saldefdetails != null)
                                {
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n " + "SAL/CAL Definition: " + Saldefdetails.SALD_NAME, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandler);
                                }
                            }
                            else
                            {
                                CommonFunctions.MessageBoxDisplay("Can’t delete, as there are Groups/Questions exist");
                            }
                        }
                        break;
                    case Consts.ToolbarActions.Help:
                        //  Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN0022");
                        Application.Navigate(CommonFunctions.CreateZenHelps("ADMN0022", 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        string strmsgGrp = string.Empty; string SqlMsg = string.Empty;
        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
          //  Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

          //  if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult.ToString() == "Yes")
                {
                    SaldefEntity Saldefdetails = gvwServices.SelectedRows[0].Tag as SaldefEntity;
                    if (Saldefdetails != null)
                    {

                        Saldefdetails.Mode = "Delete";

                    string SALID = string.Empty;
                    int SelIndex = gvwServices.CurrentRow.Index;
                    if (gvwServices.Rows.Count - 1 >= SelIndex + 1)
                        SALID = gvwServices.Rows[SelIndex + 1].Cells["gvtCode"].Value.ToString();
                    else
                        SALID = gvwServices.Rows[0].Cells["gvtCode"].Value.ToString();

                    if (_model.SALDEFData.CAP_SALDEF_INSUPDEL(Saldefdetails, out strmsgGrp, out SqlMsg))
                        {
                        AlertBox.Show("Hierarchy: " + gvwServices.CurrentRow.Cells["gvtHie"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                        //if (strIndex != 0)
                        //        strIndex = strIndex - 1;
                            Refresh("S",SALID);

                        }
                        else
                        {
                            AlertBox.Show("You can’t delete this SAL/CAL Definition, as there are Dependices", MessageBoxIcon.Warning);
                        }
                    }

                }
           // }
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        /// 

        public string GetSelectedRow()
        {
            string salId = null;
            if (gvwServices != null)
            {
                foreach (DataGridViewRow dr in gvwServices.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        strIndex = gvwServices.SelectedRows[0].Index;
                       // strPageIndex = gvwServices.CurrentPage;
                        SaldefEntity Saldefdetails = dr.Tag as SaldefEntity;
                        if (Saldefdetails != null)
                        {
                            salId = Saldefdetails.SALD_ID;
                            break;
                        }
                    }
                }
            }
            return salId;
        }


        public void Getsaldefdata(string SalID)
        {
            SaldefEntity _sqldefentity = new SaldefEntity(true);
            List<SaldefEntity> sqldeflist = _model.SALDEFData.Browse_SALDEF(_sqldefentity, "Browse",BaseForm.UserID,BaseForm.BaseAdminAgency);
            gvwServices.SelectionChanged -= gvwServices_SelectionChanged;
            gvwGroup.SelectionChanged -= gvwGroup_SelectionChanged;
            gvwServices.Rows.Clear();
            gvwGroup.Rows.Clear();
            gvwQues.Rows.Clear();
            int SelIndex = 0;
            foreach (SaldefEntity saldefitem in sqldeflist)
            {
                int rowIndex = gvwServices.Rows.Add(saldefitem.SALD_ID, saldefitem.SALD_HIE, saldefitem.SALD_NAME, saldefitem.SALD_TYPE == "C" ? "Contact" : "Service");
                if (saldefitem.SALD_ID == SalID)
                    SelIndex = rowIndex;

                gvwServices.Rows[rowIndex].Tag = saldefitem;
                if (saldefitem.SALD_ACTIVE.Trim() != "A")
                    gvwServices.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                CommonFunctions.setTooltip(rowIndex, saldefitem.SALD_ADD_OPERATOR, saldefitem.SALD_DATE_ADD, saldefitem.SALD_LSTC_OPERATOR, saldefitem.SALD_DATE_LSTC, gvwServices);
            }

            if (gvwServices.Rows.Count > 0)
            {
                if(SelIndex>0)
                {
                    gvwServices.Rows[SelIndex].Selected = true;
                    gvwServices.CurrentCell = gvwServices.Rows[SelIndex].Cells[1];
                }
                else if (gvwServices.Rows.Count > strIndex)
                {
                    gvwServices.Rows[strIndex].Selected = true;
                    gvwServices.CurrentCell = gvwServices.Rows[strIndex].Cells[1];
                }
                else
                {
                    gvwServices.Rows[0].Selected = true;
                    gvwServices.CurrentCell = gvwServices.Rows[0].Cells[1];
                }
            }
            gvwServices.SelectionChanged += gvwServices_SelectionChanged;
            gvwServices_SelectionChanged(gvwServices, new EventArgs());

        }

        // string lnk_question = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("link2.png");
        string lnk_question = "captain-gridlink";

        private void gvwServices_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwServices.Rows.Count > 0)
            {
                if (gvwServices.SelectedRows[0].Selected)
                {
                    SaldefEntity Saldefdetails = gvwServices.SelectedRows[0].Tag as SaldefEntity;
                    if (Saldefdetails != null)
                    {
                        SalquesEntity _sqlquesEntity = new SalquesEntity(true);
                        _sqlquesEntity.SALQ_SALD_ID = Saldefdetails.SALD_ID;
                        List<SalquesEntity> salquesdata = _model.SALDEFData.Browse_SALQUES(_sqlquesEntity, "Browse");
                        salquesdata = salquesdata.FindAll(u => u.SALQ_SEQ == "0");
                        salquesdata = salquesdata.OrderBy(u => u.SALQ_DESC).OrderBy(u => Convert.ToInt32(u.SALQ_GRP_SEQ)).ToList();
                        gvwGroup.SelectionChanged -= gvwGroup_SelectionChanged;
                        gvwGroup.Rows.Clear();
                        gvwQues.Rows.Clear();
                        int SelIndex = 0;
                        foreach (SalquesEntity salquesdatatem in salquesdata)
                        {
                            int rowIndex = gvwGroup.Rows.Add(salquesdatatem.SALQ_GRP_SEQ, salquesdatatem.SALQ_DESC, salquesdatatem.SALQ_ID);
                            if (salquesdatatem.SALQ_ID == strQuesID)
                                SelIndex = rowIndex;
                            gvwGroup.Rows[rowIndex].Tag = salquesdatatem;
                            if (propsalQlinkEntitylist.Count > 0)
                            {
                                List<SALQLNKEntity> dr = propsalQlinkEntitylist.FindAll(u => u.SALQL_GROUP == salquesdatatem.SALQ_ID && u.SALQL_LINKQ.Trim() != string.Empty);
                                if (dr.Count > 0)
                                {
                                    gvwGroup.Rows[rowIndex].Cells["gviLinkq"].Value = lnk_question;
                                }
                            }
                            CommonFunctions.setTooltip(rowIndex, salquesdatatem.SALQ_ADD_OPERATOR, salquesdatatem.SALQ_DATE_ADD, salquesdatatem.SALQ_LSTC_OPERATOR, salquesdatatem.SALQ_DATE_LSTC, gvwGroup);
                        }
                        if (gvwGroup.Rows.Count > 0)
                        {
                            if(SelIndex>0)
                            {
                                gvwGroup.Rows[SelIndex].Selected = true;
                                gvwGroup.CurrentCell = gvwGroup.Rows[SelIndex].Cells[1];
                            }
                            //else if (gvwGroup.Rows.Count > strGroupIndex)
                            //{
                            //    gvwGroup.Rows[strGroupIndex].Selected = true;
                            //    gvwGroup.CurrentCell = gvwGroup.Rows[strGroupIndex].Cells[1];
                            //}
                            else
                            {
                                gvwGroup.Rows[0].Selected = true;
                                gvwGroup.CurrentCell = gvwGroup.Rows[0].Cells[1];
                            }
                        }
                        gvwGroup.SelectionChanged += gvwGroup_SelectionChanged;
                        gvwGroup_SelectionChanged(gvwGroup, new EventArgs());
                    }
                }
            }
        }

        private void gvwServices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.AddPriv.Equals("true"))
            {
                if (this.gvIAdd.Index == e.ColumnIndex && e.RowIndex != -1)
                {
                    SaldefEntity Saldefdetails = gvwServices.SelectedRows[0].Tag as SaldefEntity;
                    if (Saldefdetails != null)
                    {
                        if (Saldefdetails.SALD_5QUEST == "Y" && gvwGroup.Rows.Count == 1)
                        {
                            AlertBox.Show("We should not create more than one Group for this " + Saldefdetails.SALD_NAME.Trim(), MessageBoxIcon.Warning);
                        }
                        else
                        {
                            ADMN0022Form addUserForm = new ADMN0022Form(BaseForm, "Add", Saldefdetails.SALD_ID, Privileges, "G", string.Empty, null, (gvwGroup.Rows.Count + 1).ToString());
                            addUserForm.StartPosition = FormStartPosition.CenterScreen;
                            addUserForm.ShowDialog();
                        }
                    }
                }
            }
        }

        private void gvwGroup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (Privileges.AddPriv.Equals("true"))
                {
                    if (this.gvIGAdd.Index == e.ColumnIndex && e.RowIndex != -1)
                    {
                        if (gvwGroup.Rows.Count > 0)
                        {
                            SaldefEntity Saldefdetails = gvwServices.SelectedRows[0].Tag as SaldefEntity;
                            SalquesEntity Salgrpdetails = gvwGroup.CurrentRow.Tag as SalquesEntity;
                            strGroupIndex = gvwGroup.CurrentRow.Index;
                            if (Salgrpdetails != null)
                            {
                                if (Saldefdetails.SALD_5QUEST == "Y" && gvwQues.Rows.Count == 5)
                                {
                                    AlertBox.Show("We should not add more than 5 questions for this " + Saldefdetails.SALD_NAME.Trim(), MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    ADMN0022Form addUserForm = new ADMN0022Form(BaseForm, "Add", Salgrpdetails.SALQ_SALD_ID, Privileges, "Q", Salgrpdetails.SALQ_ID, Salgrpdetails, (gvwQues.Rows.Count + 1).ToString());
                                    addUserForm.StartPosition = FormStartPosition.CenterScreen;
                                    addUserForm.ShowDialog();
                                }
                            }
                        }
                    }
                }

                if (Privileges.ChangePriv.Equals("true"))
                {
                    if (this.gvIGEdit.Index == e.ColumnIndex && e.RowIndex != -1)
                    {
                        if (gvwGroup.Rows.Count > 0)
                        {
                            strGroupIndex = gvwGroup.CurrentRow.Index;
                            SalquesEntity Saldefdetails = gvwGroup.CurrentRow.Tag as SalquesEntity;
                            if (Saldefdetails != null)
                            {
                                ADMN0022Form addUserForm = new ADMN0022Form(BaseForm, "Edit", Saldefdetails.SALQ_SALD_ID, Privileges, "G", Saldefdetails.SALQ_ID, Saldefdetails, (gvwGroup.Rows.Count + 1).ToString());
                                addUserForm.StartPosition = FormStartPosition.CenterScreen;
                                addUserForm.ShowDialog();
                            }
                        }
                    }
                }

                if (Privileges.DelPriv.Equals("true"))
                {
                    if (this.gvIGDelete.Index == e.ColumnIndex && e.RowIndex != -1)
                    {
                        strGroupIndex = 0;
                        if (gvwQues.Rows.Count == 0)
                        {
                            SalquesEntity Saldefdetails = gvwGroup.CurrentRow.Tag as SalquesEntity;
                            if (Saldefdetails != null)
                            {
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n " + "Group Definition: " + Saldefdetails.SALQ_DESC, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: Delete_Groupdata);
                            }
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Can’t Delete, as there are Questions defined in this group");
                        }
                    }
                }
                if (Privileges.ChangePriv.Equals("true") || Privileges.AddPriv.Equals("true"))
                {
                    if (this.gviLinkq.Index == e.ColumnIndex && e.RowIndex != -1)
                    {
                        if (gvwGroup.Rows.Count > 0)
                        {
                            strGroupIndex = gvwGroup.CurrentRow.Index;
                            SalquesEntity Saldefdetails = gvwGroup.CurrentRow.Tag as SalquesEntity;
                            if (Saldefdetails != null)
                            {
                                SaldefEntity Saldefentity = gvwServices.SelectedRows[0].Tag as SaldefEntity;
                                PreassesDimentionForm addUserForm = new PreassesDimentionForm(BaseForm, Saldefentity.SALD_TYPE.Trim(), Saldefdetails);
                                addUserForm.StartPosition = FormStartPosition.CenterScreen;
                                addUserForm.ShowDialog();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void gvwQues_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (Privileges.ChangePriv.Equals("true"))
                {
                    if (this.gvIQEdit.Index == e.ColumnIndex && e.RowIndex != -1)
                    {
                        strQuesIndex = gvwQues.CurrentRow.Index;
                        SalquesEntity Saldefdetails = gvwQues.CurrentRow.Tag as SalquesEntity;
                        if (Saldefdetails != null)
                        {
                            ADMN0022Form addUserForm = new ADMN0022Form(BaseForm, "Edit", Saldefdetails.SALQ_SALD_ID, Privileges, "Q", Saldefdetails.SALQ_ID, Saldefdetails, (gvwGroup.Rows.Count + 1).ToString());
                            addUserForm.StartPosition = FormStartPosition.CenterScreen;
                            addUserForm.ShowDialog();
                        }
                    }
                }

                if (Privileges.DelPriv.Equals("true"))
                {
                    if (this.gvIQDelete.Index == e.ColumnIndex && e.RowIndex != -1)
                    {
                        strQuesIndex = 0;
                        SalquesEntity Saldefdetails = gvwQues.CurrentRow.Tag as SalquesEntity;
                        if (Saldefdetails != null)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n " + "Question Definition: " + Saldefdetails.SALQ_DESC, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: Delete_Questionsdata);
                        }
                    }
                }
            }
            catch (Exception)
            {


            }
        }


        public void Delete_Groupdata(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
            if (dialogResult.ToString() == "Yes")
            {
                SalquesEntity Salquesdetails = gvwGroup.CurrentRow.Tag as SalquesEntity;

                string SALID = string.Empty;
                int SelIndex = gvwGroup.CurrentRow.Index;
                if (gvwGroup.Rows.Count - 1 >= SelIndex + 1)
                    SALID = gvwGroup.Rows[SelIndex + 1].Cells["gvtGroupId"].Value.ToString();
                else
                    SALID = gvwGroup.Rows[0].Cells["gvtGroupId"].Value.ToString();

                strQuesID = SALID;

                AlertBox.Show("Group Definition Seq: " + gvwGroup.CurrentRow.Cells["gvtGSeq"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);

                if (Salquesdetails != null)
                {
                    Salquesdetails.Mode = "Delete";
                    string stroutquesid = string.Empty;
                    if (_model.SALDEFData.CAP_SALQUES_INSUPDEL(Salquesdetails, "", out stroutquesid))
                    {
                        gvwServices_SelectionChanged(gvwServices, new EventArgs());
                    }
                }
                else
                    AlertBox.Show("Faied to Delete", MessageBoxIcon.Warning);

            }
            //}
        }



        public void Delete_Questionsdata(DialogResult dialogResult)
        {
            // Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
            if (dialogResult.ToString() == "Yes")
            {
                SalquesEntity Saldefdetails = gvwQues.CurrentRow.Tag as SalquesEntity;

                string SALID = string.Empty;
                int SelIndex = gvwQues.CurrentRow.Index;
                if (gvwQues.Rows.Count - 1 >= SelIndex + 1)
                    SALID = gvwQues.Rows[SelIndex + 1].Cells["gvq_ID"].Value.ToString();
                else
                    SALID = gvwQues.Rows[0].Cells["gvq_ID"].Value.ToString(); 

                strQuesID = SALID;

                
                if (Saldefdetails != null)
                {
                    Saldefdetails.Mode = "Delete";
                    string stroutquesid = string.Empty;
                    if (_model.SALDEFData.CAP_SALQUES_INSUPDEL(Saldefdetails, "QUES", out stroutquesid))
                    {
                        AlertBox.Show("Question Definition Seq: " + gvwQues.CurrentRow.Cells["gvtQSeq"].Value.ToString() + "\n" + "Deleted Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                        gvwGroup_SelectionChanged(gvwGroup, new EventArgs());
                    }
                    else
                    {
                        if(stroutquesid=="-1")
                            AlertBox.Show("We can't delete this question because it have Service Association", MessageBoxIcon.Warning);
                        if (stroutquesid == "0")
                            AlertBox.Show("We can't delete this question because it have Contact Association", MessageBoxIcon.Warning);
                    }
                }
                else
                    AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);

            }
            //}
        }




        private void gvwGroup_SelectionChanged(object sender, EventArgs e)
        {

            if (gvwGroup.Rows.Count > 0)
            {
                if (gvwGroup.SelectedRows[0].Selected)
                {
                    int SelIndex = 0;
                    strGroupIndex = gvwGroup.SelectedRows[0].Index;
                    SalquesEntity Saldefdetails = gvwGroup.SelectedRows[0].Tag as SalquesEntity;
                    if (Saldefdetails != null)
                    {
                        SalquesEntity _sqlquesEntity = new SalquesEntity(true);
                        _sqlquesEntity.SALQ_SALD_ID = Saldefdetails.SALQ_SALD_ID;
                        _sqlquesEntity.SALQ_GRP_CODE = Saldefdetails.SALQ_GRP_CODE;
                        List<SalquesEntity> salquesdata = _model.SALDEFData.Browse_SALQUES(_sqlquesEntity, "Browse");
                        salquesdata = salquesdata.FindAll(u => u.SALQ_SEQ != "0");
                        salquesdata = salquesdata.OrderBy(u => u.SALQ_DESC).OrderBy(u => Convert.ToInt32(u.SALQ_SEQ)).ToList();
                        gvwQues.Rows.Clear();
                        
                        foreach (SalquesEntity salquesdatatem in salquesdata)
                        {
                            int rowIndex = gvwQues.Rows.Add(salquesdatatem.SALQ_SEQ, salquesdatatem.SALQ_DESC, getTypedesc(salquesdatatem.SALQ_TYPE),salquesdatatem.SALQ_ID);
                            if (salquesdatatem.SALQ_ID == strQuesID)
                                SelIndex = rowIndex;

                            gvwQues.Rows[rowIndex].Tag = salquesdatatem;
                            CommonFunctions.setTooltip(rowIndex, salquesdatatem.SALQ_ADD_OPERATOR, salquesdatatem.SALQ_DATE_ADD, salquesdatatem.SALQ_LSTC_OPERATOR, salquesdatatem.SALQ_DATE_LSTC, gvwQues);

                        }
                    }
                    if (gvwQues.Rows.Count > 0)
                    {
                        if(SelIndex>0)
                        {
                            gvwQues.Rows[SelIndex].Selected = true;
                            gvwQues.CurrentCell = gvwQues.Rows[SelIndex].Cells[1];
                        }
                        //else if (gvwQues.Rows.Count > strQuesIndex)
                        //{
                        //    gvwQues.Rows[strQuesIndex].Selected = true;
                        //    gvwQues.CurrentCell = gvwQues.Rows[strQuesIndex].Cells[1];
                        //}
                        else
                        {
                            gvwQues.Rows[0].Selected = true;
                            gvwQues.CurrentCell = gvwQues.Rows[0].Cells[1];
                        }
                    }
                }
            }
        }
        string getTypedesc(string strType)
        {
            string strdesc = string.Empty;
            switch (strType)
            {
                case "C":
                    strdesc = "Checkbox";
                    break;
                case "T":
                    strdesc = "Date";
                    break;
                case "D":
                    strdesc = "Dropdown";
                    break;
                case "N":
                    strdesc = "Numeric";
                    break;
                case "X":
                    strdesc = "Text";
                    break;
                case "1":
                    strdesc = "Text- 1 line 80 characters";
                    break;
                case "2":
                    strdesc = "Text- 2 line 80 characters per line";
                    break;
                case "3":
                    strdesc = "Text- 3 lines 80 character per line";
                    break;

            }
            return strdesc;
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if(gvwServices.Rows.Count>0)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Text = "Copy";
                menuItem.Tag = "C";
                contextMenu1.MenuItems.Add(menuItem);
            }
        }

        private void gvwServices_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string selectedRowid = GetSelectedRow();
            if (selectedRowid != null)
            {
                ADMN0022Form editUserForm = new ADMN0022Form(BaseForm, "Copy", selectedRowid, Privileges, "S", string.Empty, null, string.Empty);
                editUserForm.StartPosition = FormStartPosition.CenterScreen;
                editUserForm.ShowDialog();
            }
        }

        #region Excel Report
        string Random_Filename = null; string PdfName = null;
        private void On_SaveExcelForm_Closed()
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "SALCUSTQUES_" + "Report";

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



            string SPCode = string.Empty;
            SPCode = gvwServices.CurrentRow.Cells["gvtHie"].Value.ToString().Trim().Substring(0, 2);

            DataSet ds = DatabaseLayer.SPAdminDB.GET_CUSTQUESReport(SPCode);
            //DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];

            Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                sheet = book.Worksheets.Add("sheet1");
                sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(200);
                sheet.Table.Columns.Add(250);
                sheet.Table.Columns.Add(75);
                sheet.Table.Columns.Add(75);
                sheet.Table.Columns.Add(75);
                sheet.Table.Columns.Add(75);
                sheet.Table.Columns.Add(75);
                sheet.Table.Columns.Add(75);


                int CYear = DateTime.Now.Year;

                Row0 = sheet.Table.Rows.Add();
                cell = Row0.Cells.Add("HIE", DataType.String, "s94");
                cell = Row0.Cells.Add("Description", DataType.String, "s94");
                cell = Row0.Cells.Add("Question", DataType.String, "s94");
                cell = Row0.Cells.Add(CYear.ToString(), DataType.String, "s94");
                cell = Row0.Cells.Add((CYear - 1).ToString(), DataType.String, "s94");
                cell = Row0.Cells.Add((CYear - 2).ToString(), DataType.String, "s94");
                cell = Row0.Cells.Add((CYear - 3).ToString(), DataType.String, "s94");
                cell = Row0.Cells.Add((CYear - 4).ToString(), DataType.String, "s94");
                cell = Row0.Cells.Add("<=" + (CYear - 5).ToString(), DataType.String, "s94");

                foreach (DataRow dr in dt.Rows)
                {
                    Row0 = sheet.Table.Rows.Add();
                    cell = Row0.Cells.Add(dr["SALD_HIE"].ToString().Trim(), DataType.String, "s96");
                    cell = Row0.Cells.Add(dr["SALD_NAME"].ToString().Trim(), DataType.String, "s96");
                    cell = Row0.Cells.Add(dr["SALQ_DESC"].ToString().Trim(), DataType.String, "s96");
                    cell = Row0.Cells.Add(dr["CYEAR"].ToString().Trim(), DataType.String, "s96R");
                    cell = Row0.Cells.Add(dr["PYEAR1"].ToString().Trim(), DataType.String, "s96R");
                    cell = Row0.Cells.Add(dr["PYEAR2"].ToString().Trim(), DataType.String, "s96R");
                    cell = Row0.Cells.Add(dr["PYEAR3"].ToString().Trim(), DataType.String, "s96R");
                    cell = Row0.Cells.Add(dr["PYEAR4"].ToString().Trim(), DataType.String, "s96R");
                    cell = Row0.Cells.Add(dr["PYEAR5"].ToString().Trim(), DataType.String, "s96R");
                }

            }


            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

           /* FileDownloadGateway downloadGateway = new FileDownloadGateway();
            downloadGateway.Filename = "SALCUSTQUES_Report.xls";

            // downloadGateway.Version = file.Version;

            downloadGateway.SetContentType(DownloadContentType.OctetStream);

            downloadGateway.StartFileDownload(new ContainerControl(), PdfName);*/

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
            s95R.Font.Color = "#FFFFFF";
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
            //s96.Font.Bold = true;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96R
            // -----------------------------------------------
            WorksheetStyle s96R = styles.Add("s96R");
            s96R.Font.FontName = "Arial";
            s96R.Font.Color = "#000000";
            s96R.Interior.Color = "#FFFFFF";
            //s96.Font.Bold = true;
            s96R.Interior.Pattern = StyleInteriorPattern.Solid;
            s96R.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s96R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96R.Alignment.WrapText = true;
            s96R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

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

        #endregion

        private void gvwServices_LongTap(object sender, EventArgs e)
        {

            contextMenu1_Popup(sender,e);
    
            contextMenu1.Show(this, new System.Drawing.Point(30, 30));
        }
    }
}