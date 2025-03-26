#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;

using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Exceptions;
using System.Diagnostics;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Controls.Compatibility;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using static iTextSharp.text.pdf.XfaForm;
using Spire.Pdf.Grid;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class Case0009Control : BaseUserControl
    {
        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        private string strYear = "    ";
        private string strMode = Consts.Common.View;

        int strIndex = 0;
        int strGroupIndex = 0;
        int strQuestionIndex = 0;
        #endregion
        public Case0009Control(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();


            propReportPath = _model.lookupDataAccess.GetReportPath();
            fillQuestionsHierchys();           
            fillGrid();
            txtHierachy.Validator = TextBoxValidation.IntegerValidator;

            PopulateToolbar(oToolbarMnustrip);
           
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarPrint { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        //public List<CaseELIGQEntity> caseEligQEntity { get; set; }

        //public List<CaseELIGHEntity> caseEligHEntity { get; set; }

        public string propReportPath { get; set; }

        public List<CaseELIGQUESEntity> EligQuestionsData { get; set; }

        public List<CaseELIGEntity> caseEligEntityAll { get; set; }

        public List<CaseELIGEntity> caseEligEntityOnlyGroups { get; set; }
        #endregion

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
                ToolBarNew.ToolTipText = "Add New Eligibility Group";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add"; //new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Eligibility Group";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit"; //new IconResourceHandle(Consts.Icons16x16.EditIcon);
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Eligibility Group";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete"; //new IconResourceHandle(Consts.Icons16x16.Delete);
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarPrint = new ToolBarButton();
                ToolBarPrint.Tag = "Print";
                ToolBarPrint.ToolTipText = "Eligibility Criteria Print";
                ToolBarPrint.Enabled = true;
                ToolBarPrint.ImageSource = "captain-print"; //new IconResourceHandle(Consts.Icons16x16.Print);
                ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Eligibility Criteria Help";
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
                ToolBarPrint,
                ToolBarHelp
            });

            if (gvwHierachy.Rows.Count == 0)
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = false;
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
                        if (GetSelectedRow() != null)
                        {
                            Case0009Form addUserForm = new Case0009Form(BaseForm, "Add", Consts.LiheAllDetails.strMainType, GetSelectedRow(), string.Empty, string.Empty, gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString(), Privileges);
                            addUserForm.FormClosed += new FormClosedEventHandler(OnCase0009FromClosed);
                            addUserForm.StartPosition = FormStartPosition.CenterScreen;
                            addUserForm.ShowDialog();
                        }
                        else
                        {
                            if (gvwHierachy.Rows.Count > 0)
                            {
                                Case0009Form addUserForm = new Case0009Form(BaseForm, "Add", Consts.LiheAllDetails.strMainType, string.Empty, string.Empty, string.Empty, gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString(), Privileges);
                                addUserForm.FormClosed += new FormClosedEventHandler(OnCase0009FromClosed);
                                addUserForm.StartPosition = FormStartPosition.CenterScreen;
                                addUserForm.ShowDialog();
                            }
                            else
                            {
                                Case0009Form addUserForm = new Case0009Form(BaseForm, "Add", Consts.LiheAllDetails.strMainType, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Privileges);
                                addUserForm.FormClosed += new FormClosedEventHandler(OnCase0009FromClosed);
                                addUserForm.StartPosition = FormStartPosition.CenterScreen;
                                addUserForm.ShowDialog();
                            }
                        }
                        break;
                    case Consts.ToolbarActions.Edit:
                        if (GetSelectedRow() != null)
                        {
                            Case0009Form EditForm = new Case0009Form(BaseForm, "Edit", Consts.LiheAllDetails.strMainType, GetSelectedRow(), string.Empty, "0", gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString(), Privileges);
                            EditForm.FormClosed += new FormClosedEventHandler(OnCase0009FromClosed);
                            EditForm.StartPosition = FormStartPosition.CenterScreen;
                            EditForm.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Delete:

                        if (GetSelectedRow() != null)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandler);
                        }
                        break;
                    case Consts.ToolbarActions.Print:
                        //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false);
                        //pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveForm_Closed);
                        //pdfListForm.ShowDialog();
                        PDFModule = "All";
                        On_SaveForm_Closed();


                        break;
                    case Consts.ToolbarActions.Help:
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        //  Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Case2009");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                // ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
           // Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;
            string strMsg = string.Empty;
            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {

                    CaseELIGEntity EligEntity = new CaseELIGEntity();
                    EligEntity.EligAgency = gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString();
                    EligEntity.EligDept = gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString();
                    EligEntity.EligProgram = gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString();
                    EligEntity.EligGroupCode = GetSelectedRow();
                    EligEntity.EligGroupSeq = "0";
                    EligEntity.Mode = "Delete";
                    EligEntity.Type = "Group";
                    if (_model.CaseSumData.InsertUpdateDelCaseElig(EligEntity, out strMsg))
                    {
                    AlertBox.Show("Deleted Successfully", MessageBoxIcon.Information, null, System.Drawing.ContentAlignment.BottomRight);
                    if (gvwGroups.Rows.Count == 1)
                        {
                        if (strIndex != 0)
                                strIndex = strIndex - 1;
                        }
                        RefreshGrid();

                    }
                    else
                    {
                    //MessageBox.Show("You can’t delete this Group, as there are Dependencies", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AlertBox.Show("You can’t delete this group because there are existing questions", MessageBoxIcon.Warning);
                }

                }
           // }
        }

        private void MessageBoxHandlerQuestions(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
         //   Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;
            string strMsg = string.Empty;
          //  if (senderForm != null)
          //  {
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    GetSelectedRow();
                    CaseELIGEntity EligQEntity = new CaseELIGEntity();
                    EligQEntity.EligAgency = gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString();
                    EligQEntity.EligDept = gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString();
                    EligQEntity.EligProgram = gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString();
                    EligQEntity.EligGroupCode = gvwQuestions.SelectedRows[0].Cells["QGroupCode"].Value != null ? gvwQuestions.SelectedRows[0].Cells["QGroupCode"].Value.ToString() : string.Empty;
                    EligQEntity.EligQuesCode = gvwQuestions.SelectedRows[0].Cells["QuestionId"].Value != null ? gvwQuestions.SelectedRows[0].Cells["QuestionId"].Value.ToString() : string.Empty;
                    EligQEntity.EligGroupSeq = gvwQuestions.SelectedRows[0].Cells["QSeq"].Value != null ? gvwQuestions.SelectedRows[0].Cells["QSeq"].Value.ToString() : string.Empty;
                    EligQEntity.Mode = "Delete";
                    EligQEntity.Type = "Question";
                    if (_model.CaseSumData.InsertUpdateDelCaseElig(EligQEntity, out strMsg))
                    {
                    AlertBox.Show("Deleted Successfully", MessageBoxIcon.Information, null, System.Drawing.ContentAlignment.BottomRight);
                    if (strMsg == "Success")
                        {
                        if (strIndex != 0)
                                strIndex = strIndex - 1;
                            RefreshQuestions("1");
                        }
                        else
                            AlertBox.Show("You can’t delete this Question, exception error", MessageBoxIcon.Warning);

                    }
                    else
                    {
                        AlertBox.Show("You can’t delete this Question, as it is already used in Service Integration Matrix", MessageBoxIcon.Warning);
                    }

               // }
            }
        }

        private void fillQuestionsHierchys()
        {
            hierarchyEntity = _model.lookupDataAccess.GetHierarchyByUserID(null, "I", string.Empty);
            EligQuestionsData = _model.CaseSumData.GetELIGQUES();
            caseEligEntityOnlyGroups = _model.CaseSumData.GetCASEELIGadpgs(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "OnlyGroup");
            caseEligEntityAll = _model.CaseSumData.GetCASEELIGadpgs(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
        
        public void RefreshQuestions(string QuestSeq)
        {
            EligQuestionsData = _model.CaseSumData.GetELIGQUES();
            caseEligEntityAll = _model.CaseSumData.GetCASEELIGadpgs(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            fillGridQuestions(QuestSeq);
            if (gvwGroups.Rows.Count > 0)
            {
                gvwGroups.Rows[strGroupIndex].Selected = true;
                //gvwGroups.CurrentCell = gvwGroups.Rows[strGroupIndex].Cells[1];
            }
        }


        public void fillGrid()
        {
            
            List<CaseELIGEntity> CaseEligHierachys = _model.CaseSumData.GetCaseEligHierachys(BaseForm.UserID,BaseForm.BaseAdminAgency);
            gvwHierachy.Rows.Clear();
            gvwGroups.Rows.Clear();
            gvwQuestions.Rows.Clear();
            foreach (var caseElighsub in CaseEligHierachys)
            {
                HierarchyEntity hierachysubEntity = hierarchyEntity.Find(u => u.Agency.Equals(caseElighsub.EligAgency) && u.Dept.Equals(caseElighsub.EligDept) && u.Prog.Equals(caseElighsub.EligProgram));
                if (hierachysubEntity != null)
                {
                    int rowIndex = gvwHierachy.Rows.Add(hierachysubEntity.Code, hierachysubEntity.HirarchyName, hierachysubEntity.Agency, hierachysubEntity.Dept, hierachysubEntity.Prog);
                    // CommonFunctions.setTooltip(rowIndex, hierachysubEntity.AddOperator, hierachysubEntity.DateAdd, hierachysubEntity.LSTCOperator, hierachysubEntity.DateLSTC, gvwHierachy);
                }
             
            }
            if (Privileges.AddPriv.Equals("false"))
            {
                gvwGroups.Columns["Add"].Visible = false;

            }
            else
            {
                gvwGroups.Columns["Add"].Visible = true;
            }
            if (gvwHierachy.Rows.Count == 0)
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
            }
            if (gvwHierachy.Rows.Count > 0)
                gvwHierachy.Rows[0].Selected = true;
        }

        public void RefreshGrid()
        {
            EligQuestionsData = _model.CaseSumData.GetELIGQUES();
            caseEligEntityOnlyGroups = _model.CaseSumData.GetCASEELIGadpgs(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "OnlyGroup");
            caseEligEntityAll = _model.CaseSumData.GetCASEELIGadpgs(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            fillGrid();
            if (gvwHierachy.Rows.Count > 0)
            {
                gvwHierachy.Rows[strIndex].Selected = true;
                gvwHierachy.CurrentCell = gvwHierachy.Rows[strIndex].Cells[0];
                //gvwHierachy.Columns[0].Width = 70;
                //gvwHierachy.Columns[0].Width =250;
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
                ToolBarPrint.Enabled = true;

            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = false;
                if (ToolBarPrint != null)
                    ToolBarPrint.Enabled = false;
            }
        }

        private void gvwGroups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvwGroups.ColumnCount - 2 && e.RowIndex != -1)
            {
                GetSelectedRow();
                Case0009Form AddForm = new Case0009Form(BaseForm, "Add", Consts.LiheAllDetails.strSubType, gvwGroups.SelectedRows[0].Cells["GroupCode"].Value.ToString(), string.Empty, string.Empty, gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString(), Privileges);
                AddForm.FormClosed += new FormClosedEventHandler(OnCase0009QuestionFromClosed);
                AddForm.StartPosition = FormStartPosition.CenterScreen;
                AddForm.ShowDialog();
            }
        }

        string QuestGrpSeq = "1"; public int _gvwQuesRowCnt = 0;
        private void gvwQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(gvwQuestions.ColumnCount.ToString());
            if (e.ColumnIndex == 7 && e.RowIndex != -1)
            {
                GetSelectedRow(); QuestGrpSeq = gvwQuestions.SelectedRows[0].Cells["QSeq"].Value.ToString();
                _gvwQuesRowCnt = gvwQuestions.Rows.Count;
                Case0009Form EditForm = new Case0009Form(BaseForm, "Edit", Consts.LiheAllDetails.strSubType, gvwQuestions.SelectedRows[0].Cells["QGroupCode"].Value.ToString(), gvwQuestions.SelectedRows[0].Cells["QuestionId"].Value.ToString(), gvwQuestions.SelectedRows[0].Cells["QSeq"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["HierarchyDesc"].Value.ToString(), Privileges);
                EditForm.FormClosed += new FormClosedEventHandler(OnCase0009QuestionFromClosed);
                EditForm.StartPosition = FormStartPosition.CenterScreen;
                EditForm.ShowDialog();
            }
            else if (e.ColumnIndex == 8 && e.RowIndex != -1)
            {
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandlerQuestions);
            }
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRow()
        {
            string GroupCode = null;
            if (gvwGroups != null)
            {
                foreach (DataGridViewRow dr in gvwGroups.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        strIndex = gvwHierachy.SelectedRows[0].Index;
                        strGroupIndex = gvwGroups.SelectedRows[0].Index;
                        //strPageIndex = gvwGroups.CurrentPage;
                        GroupCode = gvwGroups.SelectedRows[0].Cells["GroupCode"].Value.ToString();

                    }
                }
            }
            return GroupCode;
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gvwGroups_SelectionChanged(object sender, EventArgs e)
        {
            fillGridQuestions(string.Empty);
        }

        public void fillGridQuestions(string QuestSeq)
        {

            string strResponceValue = string.Empty;
            string strConjunction = string.Empty;
            int rowIndex = 0; int SelIndex = 0;
            foreach (DataGridViewRow dr in gvwGroups.Rows)
            {
                if (dr.Selected)
                {
                    string strHierchycode = gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString();

                    List<CaseELIGEntity> caseEligQuestions = caseEligEntityAll.FindAll(u => ((u.EligAgency + u.EligDept + u.EligProgram).Equals(strHierchycode)) && (!u.EligGroupSeq.Equals("0")) && (u.EligGroupCode.Equals(gvwGroups.SelectedRows[0].Cells["GroupCode"].Value.ToString())));


                    gvwQuestions.Rows.Clear();
                    foreach (var caseEligquestion in caseEligQuestions)
                    {
                        strResponceValue = string.Empty;
                        if (caseEligquestion.EligResponseType.ToString() == "N")
                        {
                            if (caseEligquestion.EligNumEqual != string.Empty)
                            {
                                if (Convert.ToDecimal(caseEligquestion.EligNumEqual) > 0)
                                    strResponceValue = "= " + caseEligquestion.EligNumEqual + ",";
                            }
                            if (caseEligquestion.EligNumLthan != string.Empty)
                            {
                                if (Convert.ToDecimal(caseEligquestion.EligNumLthan) > 0)
                                    strResponceValue = strResponceValue + "< " + caseEligquestion.EligNumLthan + ",";
                            }
                            if (caseEligquestion.EligNumGthan != string.Empty)
                            {
                                if (Convert.ToDecimal(caseEligquestion.EligNumGthan) > 0)
                                    strResponceValue = strResponceValue + "> " + caseEligquestion.EligNumGthan + ",";
                            }
                            if (strResponceValue != string.Empty)
                                strResponceValue = strResponceValue.Remove(strResponceValue.Length - 1);
                        }
                        else if (caseEligquestion.EligResponseType.ToString() == "T")
                        {
                            strResponceValue = caseEligquestion.EligDDTextResp.Trim();
                        }
                        else
                        {
                            if (caseEligquestion.EligQuesCode == "I00025" || caseEligquestion.EligQuesCode == "I00026" || caseEligquestion.EligQuesCode == "I00036")
                            {
                                if (caseEligquestion.EligDDTextResp.Trim() == "Y")
                                {
                                    strResponceValue = "Yes";
                                }
                                else if (caseEligquestion.EligDDTextResp.Trim() == "N")
                                {
                                    strResponceValue = "No";
                                }
                                else if (caseEligquestion.EligDDTextResp.Trim() == "U")
                                {
                                    strResponceValue = "Unavailable";
                                }
                            }
                            else
                            {
                                if (caseEligquestion.EligAgyCode != string.Empty)
                                    if (caseEligquestion.EligAgyCode.StartsWith("C") || caseEligquestion.EligAgyCode.StartsWith("P"))
                                        strResponceValue = CaseSumData.GetCustRespCode(caseEligquestion.EligAgyCode, caseEligquestion.EligDDTextResp.Trim());
                                    else
                                        strResponceValue = LookupDataAccess.GetLookUpCode(caseEligquestion.EligAgyCode, caseEligquestion.EligDDTextResp.Trim());
                            }

                        }
                        strConjunction = string.Empty;
                        if (caseEligquestion.EligConjunction.Trim() != string.Empty)
                            strConjunction = caseEligquestion.EligConjunction == "A" ? "And" : "Or";
                        rowIndex = gvwQuestions.Rows.Add(string.Empty, caseEligquestion.EligQuestionDesc, strResponceValue, strConjunction, caseEligquestion.EligGroupCode, caseEligquestion.EligQuesCode, caseEligquestion.EligGroupSeq);
                        CommonFunctions.setTooltip(rowIndex, caseEligquestion.AddOperator, caseEligquestion.DateAdd, caseEligquestion.LstcOperator, caseEligquestion.DateLstc, gvwQuestions);
                        if (QuestSeq == caseEligquestion.EligGroupSeq)
                            SelIndex = rowIndex;
                    }
                }
                if (gvwQuestions.Rows.Count > 0)
                {
                    gvwQuestions.Rows[SelIndex].Selected = true;
                    gvwQuestions.CurrentCell = gvwQuestions.Rows[SelIndex].Cells[1];
                    //foreach (DataGridViewRow item in gvwQuestions.Rows)
                    //{
                    //    int i = item.Index;
                    //    gvwQuestions.CurrentCell = gvwQuestions.Rows[i].Cells[1];
                    //}
                }
            }


            if (Privileges.ChangePriv.Equals("false"))
            {
                gvwQuestions.Columns["EditQuestions"].Visible = false;

            }
            else
            {
                gvwQuestions.Columns["EditQuestions"].Visible = true;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                gvwQuestions.Columns["DeleteQuestion"].Visible = false;

            }
            else
            {
                gvwQuestions.Columns["DeleteQuestion"].Visible = true;
            }

            //      gvwQuestions.Sort(gvwQuestions.Columns["QSeq"], ListSortDirection.Ascending);

        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //string strCode = ((ListItem)cmbDepartment.SelectedItem).Value.ToString();
            bool isHierarchy = false;
            foreach (DataGridViewRow item in gvwHierachy.Rows)
            {
                if (txtHierachy.Text.Trim() == item.Cells["gAgency"].Value.ToString() + item.Cells["gDept"].Value.ToString() + item.Cells["gProgram"].Value.ToString())
                {
                    //gvwCustomer.Update();
                    int i = item.Index;
                    // int intscroolindex = gvwCustomer.FirstDisplayedScrollingRowIndex;                 

                    //gvwHierachy.FirstDisplayedScrollingRowIndex = i;
                    gvwHierachy.CurrentCell = gvwHierachy.Rows[i].Cells[0];
                    gvwHierachy.Rows[i].Selected = true;
                    isHierarchy = true;
                    break;
                }

            }
            if (!isHierarchy)
                if (txtHierachy.Text != "")
                    AlertBox.Show("This hierarchy record is not found", MessageBoxIcon.Warning);

            //gvwGroups.Rows.Clear();
            //List<CaseELIGGEntity> caseELigEntity = _model.CaseSumData.GETCASEELIGGDETAILS(string.Empty,string.Empty, txtHierachy.Text);
            //foreach (CaseELIGGEntity eligEntity in caseELigEntity)
            //{
            //    string questionCode = eligEntity.EligQuesCode.ToString();
            //    questionCode = "0000".Substring(0, 4 - questionCode.Length) + questionCode;
            //    int rowIndex = gvwGroups.Rows.Add(questionCode, eligEntity.EligQuesDesc, eligEntity.EligPoints.ToString());
            //    gvwGroups.Rows[rowIndex].Tag = eligEntity;
            //    CommonFunctions.setTooltip(rowIndex, eligEntity.AddOperator, eligEntity.DateAdd, eligEntity.DateLstc, eligEntity.LstcOperator, gvwGroups);
            //}
        }
        string PDFModule = null;
        private void gvwHierachy_SelectionChanged(object sender, EventArgs e)
        {
            string strResponceValue = string.Empty;
            string strConjunction = string.Empty;
            foreach (DataGridViewRow dr in gvwHierachy.SelectedRows)
            {
                if (dr.Selected)
                {

                    //strPageIndex = gvwGroups.CurrentPage;
                    string strHierchycode = gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString() + gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString();
                    List<CaseELIGEntity> caseEligGroups = caseEligEntityOnlyGroups.FindAll(u => ((u.EligAgency + u.EligDept + u.EligProgram).Equals(strHierchycode)) && (u.EligGroupSeq.Equals("0")));

                    gvwGroups.Rows.Clear();
                    foreach (var caseEligroupentity in caseEligGroups)
                    {
                        string questionCode = caseEligroupentity.EligGroupCode.ToString();
                        questionCode = "0000".Substring(0, 4 - questionCode.Length) + questionCode;
                        strConjunction = string.Empty;
                        if (caseEligroupentity.EligConjunction.Trim() != string.Empty)
                            strConjunction = caseEligroupentity.EligConjunction == "A" ? "And" : "Or";
                        int rowIndex = gvwGroups.Rows.Add(questionCode, caseEligroupentity.EligGroupDesc, strConjunction, caseEligroupentity.EligPoints.ToString());
                        gvwGroups.Rows[rowIndex].Tag = caseEligroupentity;
                        CommonFunctions.setTooltip(rowIndex, caseEligroupentity.AddOperator, caseEligroupentity.DateAdd, caseEligroupentity.LstcOperator, caseEligroupentity.DateLstc, gvwGroups);
                    }
                    QuestGrpSeq = "1";
                    if (gvwGroups.Rows.Count > 0)
                        gvwGroups.Rows[0].Selected = true;
                    //   gvwGroups.Sort(gvwGroups.Columns["GroupCode"], ListSortDirection.Ascending);
                    gvwGroups_SelectionChanged(sender, e);
                }
            }

        }

        private void gvwHierachy_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 5)
                {
                    PDFModule = "Grid";
                    On_SaveForm_Closed();
                    //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false);
                    //pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveForm_Closed);
                    //pdfListForm.ShowDialog();
                }
            }
        }

        private void OnCase0009FromClosed(object sender, FormClosedEventArgs e)
        {

            Case0009Form form = sender as Case0009Form;
            
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.Mode == "Add")
                {
                    if (form.strgroupcode.ToString().Trim() != "0001")
                    {
                        foreach (DataGridViewRow item in gvwGroups.Rows)
                        {
                            if (item.Cells["GroupCode"].Value.ToString().Trim() == form.strgroupcode.ToString().Trim())
                            {
                                item.Selected = true;
                                gvwGroups.CurrentCell = item.Cells[1];
                            }
                        }
                    }
                    else
                    {
                        foreach (DataGridViewRow item in gvwHierachy.Rows)
                        {
                            if (item.Cells["gAgency"].Value.ToString().Trim() + item.Cells["gDept"].Value.ToString().Trim() + item.Cells["gProgram"].Value.ToString().Trim() == form.HiearchyCode.ToString().Trim())
                            {
                                item.Selected = true;
                                gvwHierachy.CurrentCell = item.Cells[0];
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow item in gvwGroups.Rows)
                    {
                        if (item.Cells["GroupCode"].Value.ToString().Trim() == form.strgroupcode.ToString().Trim())
                        {
                            item.Selected = true;
                            gvwGroups.CurrentCell = item.Cells[1];
                        }
                    }
                }
            }
        }

        private void OnCase0009QuestionFromClosed(object sender, FormClosedEventArgs e)
        {

            Case0009Form form = sender as Case0009Form;
            if (form.DialogResult == DialogResult.OK)
            {
                foreach (DataGridViewRow item in gvwQuestions.Rows)
                {
                    if (item.Cells["QuestionId"].Value.ToString().Trim() == form.strQuestionCode.ToString().Trim() && item.Cells["QSeq"].Value.ToString()==form.strSeqCode.ToString())
                    {
                        item.Selected = true;
                        //gvwQuestions.CurrentCell = item.Cells[0];
                    }
                }
            }
        }

        //Begin Report Section.....................

        string Agency = null;
        string Random_Filename = null; string PdfName = null;
        private void On_SaveForm_Closed()//object sender, FormClosedEventArgs e)
        {

            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "EligbiltyCriteria";
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

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_Calibri, 9, 1, BaseColor.BLACK);

            //BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            //Font fc1 = new iTextSharp.text.Font(bfTimes, 12, 2, BaseColor.BLUE);

            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 500f;
            table.WidthPercentage = 100;
            table.LockedWidth = true;
            float[] widths = new float[] { 10f, 50f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;

            table.SpacingAfter = 05f;


            PdfPTable Groups = new PdfPTable(4);
            Groups.TotalWidth = 500f;
            Groups.WidthPercentage = 100;
            Groups.LockedWidth = true;
            float[] widths2 = new float[] { 80f, 40f, 40f, 10f };
            Groups.SetWidths(widths2);
            Groups.HorizontalAlignment = Element.ALIGN_CENTER;
            //Groups.SpacingBefore = 10f;
            //Groups.SpacingAfter = 10f;
            string PrivAgency = null;
            DataSet dsCaseElig = new DataSet();
            DataTable dtCaseElig = new DataTable();
            if (PDFModule == "Grid")
            {
                dsCaseElig = Captain.DatabaseLayer.CaseSum.Browse_CASEELIG(gvwHierachy.SelectedRows[0].Cells["gAgency"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["gDept"].Value.ToString(), gvwHierachy.SelectedRows[0].Cells["gProgram"].Value.ToString());
                dtCaseElig = dsCaseElig.Tables[0];
                DataView dv = new DataView(dtCaseElig);
                dv.Sort = "CASEELIG_AGENCY,CASEELIG_DEPT,CASEELIG_PROGRAM,CASEELIG_GROUP_CODE,CASEELIG_GROUP_SEQ";
                dtCaseElig = dv.ToTable();
            }
            else
            {
                dsCaseElig = Captain.DatabaseLayer.CaseSum.Browse_CASEELIG(null, null, null);
                dtCaseElig = dsCaseElig.Tables[0];
                DataView dv = new DataView(dtCaseElig);
                dv.Sort = "CASEELIG_AGENCY,CASEELIG_DEPT,CASEELIG_PROGRAM,CASEELIG_GROUP_CODE,CASEELIG_GROUP_SEQ";
                dtCaseElig = dv.ToTable();
            }
            bool First; string MemAccess = null; string Response = null; string Conjuction = null; string GrpConj = null;
            string Code = null;
            List<CaseELIGQUESEntity> EligQueslist;
            EligQueslist = _model.CaseSumData.Browse_ELIGQUES();

            DataSet dsCust = Captain.DatabaseLayer.SPAdminDB.BrowseCustFlds();
            DataTable dtCust = dsCust.Tables[0];
            if (dtCaseElig.Rows.Count > 0)
            {
                First = true;
                foreach (DataRow drEligList in dtCaseElig.Rows)
                {
                    if (drEligList["CASEELIG_AGENCY"].ToString() != "**")
                    {
                        DataSet dsProg = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(drEligList["CASEELIG_AGENCY"].ToString(), drEligList["CASEELIG_DEPT"].ToString(), drEligList["CASEELIG_PROGRAM"].ToString());
                        if (dsProg.Tables[0].Rows.Count > 0)
                            Agency = (dsProg.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        //if (drEligList["CASEELIG_AGENCY"].ToString() == "04" && drEligList["CASEELIG_DEPT"].ToString() == "04")
                        //{
                        //    MessageBox.Show("Check");
                        //}

                        if (Agency != PrivAgency)
                        {
                            PdfPCell Program = new PdfPCell(new Phrase("Program", TblFontBoldColor));
                            Program.HorizontalAlignment = Element.ALIGN_LEFT;
                            Program.Border = Rectangle.NO_BORDER;
                            table.AddCell(Program);

                            PdfPCell ProgDesc = new PdfPCell(new Phrase(Agency + "(" + drEligList["CASEELIG_AGENCY"].ToString() + "-" + drEligList["CASEELIG_DEPT"].ToString() + "-" + drEligList["CASEELIG_PROGRAM"].ToString() + ")", TblFontBoldColor));
                            ProgDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProgDesc.Border = Rectangle.NO_BORDER;
                            table.AddCell(ProgDesc);
                            PrivAgency = Agency;

                            table.SpacingBefore = 30f;
                            document.Add(table);                       //Header row
                            table.DeleteBodyRows();
                            //table.DeleteLastRow(); 
                        }
                        if (drEligList["CASEELIG_GROUP_SEQ"].ToString() == "0")
                        {
                            if (drEligList["CASEELIG_CONJN"].ToString() == "A")
                                GrpConj = "And";
                            else if (drEligList["CASEELIG_CONJN"].ToString() == "O")
                                GrpConj = "Or";
                            else
                                GrpConj = " ";
                            if (!First)
                            {
                                PdfPCell GrpSpace = new PdfPCell(new Phrase(" ", TblFontBold));
                                GrpSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                GrpSpace.Colspan = 4;
                                GrpSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //GrpSpace.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                //GrpSpace.BorderColor = BaseColor.WHITE;
                                Groups.AddCell(GrpSpace);
                            }

                            if (string.IsNullOrEmpty(drEligList["CASEELIG_CONJN"].ToString()))
                            {
                                PdfPCell GrpDesc = new PdfPCell(new Phrase(drEligList["CASEELIG_GROUP_DESC"].ToString(), TblFontBold));
                                GrpDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                GrpDesc.Colspan = 4;
                                //GrpDesc.Border = Rectangle.BOX;
                                GrpDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                GrpDesc.BorderColor = BaseColor.WHITE;
                                Groups.AddCell(GrpDesc);
                            }
                            else
                            {
                                PdfPCell GrpDesc = new PdfPCell(new Phrase(drEligList["CASEELIG_GROUP_DESC"].ToString(), TblFontBold));
                                GrpDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                GrpDesc.Colspan = 3;
                                //GrpDesc.Border = Rectangle.BOX;
                                GrpDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                GrpDesc.BorderColor = BaseColor.WHITE;
                                Groups.AddCell(GrpDesc);
                            }

                            //PdfPCell Space = new PdfPCell(new Phrase(" ", fc1));
                            //Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space.Border = Rectangle.NO_BORDER;
                            //Groups.AddCell(Space);

                            //PdfPCell Space1 = new PdfPCell(new Phrase(" ", fc1));
                            //Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space1.Border = Rectangle.NO_BORDER;
                            //Groups.AddCell(Space1);
                            if (!string.IsNullOrEmpty(drEligList["CASEELIG_CONJN"].ToString()))
                            {
                                PdfPCell Space2 = new PdfPCell(new Phrase(GrpConj, TblFontBold));
                                Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space2.Border = Rectangle.BOX;
                                Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Space2.BorderColor = BaseColor.WHITE;
                                Groups.AddCell(Space2);
                            }
                            First = false;
                            document.Add(Groups);
                            Groups.DeleteBodyRows();
                        }
                        else
                        {

                            Code = drEligList["CASEELIG_QUES_CODE"].ToString();

                            if (Code.Substring(0, 1).ToString() == "C")
                            {
                                foreach (DataRow drCust in dtCust.Rows)
                                {
                                    if (Code.ToString().Trim() == drCust["CUST_CODE"].ToString())
                                    {
                                        string Desc = drCust["CUST_DESC"].ToString();

                                        PdfPCell Group = new PdfPCell(new Phrase(Desc, TableFont));
                                        Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Group.Border = Rectangle.BOX;
                                        Group.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        Group.BorderColor = BaseColor.WHITE;
                                        Groups.AddCell(Group);

                                        if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "*")
                                            MemAccess = "All Members";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")
                                            MemAccess = "Applicant Only";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "1")
                                            MemAccess = "Any One Member";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "H")
                                            MemAccess = "Household";
                                        else
                                            MemAccess = " ";
                                        PdfPCell MemberAccess = new PdfPCell(new Phrase(MemAccess, TableFont));
                                        MemberAccess.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //MemberAccess.Border = Rectangle.BOX;
                                        MemberAccess.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        MemberAccess.BorderColor = BaseColor.WHITE;
                                        Groups.AddCell(MemberAccess);
                                        if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "D")
                                        {
                                            DataSet dsResp = DatabaseLayer.SPAdminDB.BrowseCustResp(drCust["CUST_CODE"].ToString());
                                            DataTable dtResp = dsResp.Tables[0];
                                            foreach (DataRow dr in dtResp.Rows)
                                            {
                                                if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().Trim() == dr["RSP_RESP_CODE"].ToString().Trim())
                                                {
                                                    Response = dr["RSP_DESC"].ToString(); break;
                                                }
                                            }
                                        }
                                        else if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().ToString() == "N")
                                        {
                                            if (drEligList["CASEELIG_NUM_EQUAL"].ToString() != string.Empty && drEligList["CASEELIG_NUM_EQUAL"].ToString() != "0.00")
                                                Response = "= " + drEligList["CASEELIG_NUM_EQUAL"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_LTHAN"].ToString() != string.Empty && drEligList["CASEELIG_NUM_LTHAN"].ToString() != "0.00")
                                                Response = Response + "<" + drEligList["CASEELIG_NUM_LTHAN"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_GTHAN"].ToString() != string.Empty && drEligList["CASEELIG_NUM_GTHAN"].ToString() != "0.00")
                                                Response = Response + "> " + drEligList["CASEELIG_NUM_GTHAN"].ToString() + ",";
                                            if (!string.IsNullOrEmpty(Response))
                                                Response = Response.Remove(Response.Length - 1);
                                            else Response = " ";
                                        }
                                        else
                                            Response = " ";

                                        if (drEligList["CASEELIG_CONJN"].ToString() == "A")
                                            Conjuction = "And";
                                        else if (drEligList["CASEELIG_CONJN"].ToString() == "O")
                                            Conjuction = "Or";
                                        else
                                            Conjuction = "";
                                        if (Conjuction == "")
                                        {
                                            PdfPCell Resp = new PdfPCell(new Phrase(Response, TableFont));
                                            Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Resp.Colspan = 2;
                                            //Resp.Border = Rectangle.BOX;
                                            Resp.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Resp.BorderColor = BaseColor.WHITE;
                                            Groups.AddCell(Resp);
                                        }
                                        else
                                        {
                                            PdfPCell Resp = new PdfPCell(new Phrase(Response, TableFont));
                                            Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //Resp.Border = Rectangle.BOX;
                                            Resp.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Resp.BorderColor = BaseColor.WHITE;
                                            Groups.AddCell(Resp);

                                            PdfPCell Conjuc = new PdfPCell(new Phrase(Conjuction, TableFont));
                                            Conjuc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //Conjuc.Border = Rectangle.BOX;
                                            Conjuc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Conjuc.BorderColor = BaseColor.WHITE;
                                            Groups.AddCell(Conjuc);
                                        }

                                        //if (First)
                                        //{
                                        //    table.SpacingBefore = 30f;
                                        //    document.Add(table);                       //Header row
                                        //    table.DeleteBodyRows();
                                        //    First = false;
                                        //}
                                        //First = false;
                                        document.Add(Groups);
                                        Groups.DeleteBodyRows();
                                        //break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (CaseELIGQUESEntity EligQuees in EligQueslist)
                                {
                                    if (Code.ToString().Trim() == EligQuees.EligQuesCode.ToString())
                                    {
                                        string Desc = EligQuees.EligQuesDesc.ToString();

                                        PdfPCell Group = new PdfPCell(new Phrase(Desc, TableFont));
                                        Group.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Group.Border = Rectangle.BOX;
                                        Group.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        Group.BorderColor = BaseColor.WHITE;
                                        Groups.AddCell(Group);

                                        if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "*")
                                            MemAccess = "All Members";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "A")
                                            MemAccess = "Applicant Only";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "1")
                                            MemAccess = "Any One Member";
                                        else if (drEligList["CASEELIG_MEM_ACCESS"].ToString() == "H")
                                            MemAccess = "Household";
                                        else
                                            MemAccess = " ";
                                        PdfPCell MemberAccess = new PdfPCell(new Phrase(MemAccess, TableFont));
                                        MemberAccess.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //MemberAccess.Border = Rectangle.BOX;
                                        MemberAccess.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        MemberAccess.BorderColor = BaseColor.WHITE;
                                        Groups.AddCell(MemberAccess);
                                        if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "D")
                                        {
                                            DataSet dsResp = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(EligQuees.EligAgyCode);
                                            DataTable dtResp = dsResp.Tables[0];
                                            if (dtResp.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtResp.Rows)
                                                {
                                                    if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().Trim() == dr["Code"].ToString().Trim())
                                                    {
                                                        Response = dr["LookUpDesc"].ToString(); break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "L")
                                        {
                                            DataSet dsResp = DatabaseLayer.CaseSum.GetAGYTABS(EligQuees.EligAgyCode);
                                            DataTable dtResp = dsResp.Tables[0];
                                            if (dtResp.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtResp.Rows)
                                                {
                                                    if (drEligList["CASEELIG_DD_TEXT_RESP"].ToString().Trim() == dr["AGYS_CODE"].ToString().Trim())
                                                    {
                                                        Response = dr["AGYS_DESC"].ToString(); break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (drEligList["CASEELIG_RESP_TYPE"].ToString() == "N")
                                        {
                                            Response = string.Empty;
                                            string Equal = drEligList["CASEELIG_NUM_EQUAL"].ToString();
                                            string Minimum = drEligList["CASEELIG_NUM_LTHAN"].ToString();
                                            string Maximum = drEligList["CASEELIG_NUM_GTHAN"].ToString();
                                            if (drEligList["CASEELIG_NUM_EQUAL"].ToString() != string.Empty && Equal != "0.00")
                                                Response = "= " + drEligList["CASEELIG_NUM_EQUAL"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_LTHAN"].ToString() != string.Empty && Minimum != "0.00")
                                                Response = Response + "<" + drEligList["CASEELIG_NUM_LTHAN"].ToString() + ",";
                                            if (drEligList["CASEELIG_NUM_GTHAN"].ToString() != string.Empty && Maximum != "0.00")
                                                Response = Response + "> " + drEligList["CASEELIG_NUM_GTHAN"].ToString() + ",";
                                            if (!string.IsNullOrEmpty(Response))
                                                Response = Response.Remove(Response.Length - 1);
                                            else Response = " ";
                                        }
                                        else
                                            Response = " ";

                                        if (drEligList["CASEELIG_CONJN"].ToString() == "A")
                                            Conjuction = "And";
                                        else if (drEligList["CASEELIG_CONJN"].ToString() == "O")
                                            Conjuction = "Or";
                                        else
                                            Conjuction = "";
                                        if (Conjuction == "")
                                        {
                                            PdfPCell Resp = new PdfPCell(new Phrase(Response, TableFont));
                                            Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Resp.Colspan = 2;
                                            //Resp.Border = Rectangle.BOX;
                                            Resp.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Resp.BorderColor = BaseColor.WHITE;
                                            Groups.AddCell(Resp);
                                        }
                                        else
                                        {
                                            PdfPCell Resp = new PdfPCell(new Phrase(Response, TableFont));
                                            Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //Resp.Border = Rectangle.BOX;
                                            Resp.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Resp.BorderColor = BaseColor.WHITE;
                                            Groups.AddCell(Resp);

                                            PdfPCell Conjuc = new PdfPCell(new Phrase(Conjuction, TableFont));
                                            Conjuc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //Conjuc.Border = Rectangle.BOX;
                                            Conjuc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Conjuc.BorderColor = BaseColor.WHITE;
                                            Groups.AddCell(Conjuc);
                                        }
                                        //PdfPCell Resp = new PdfPCell(new Phrase(Response, fc));
                                        //Resp.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Resp.Border = Rectangle.BOX;
                                        //Groups.AddCell(Resp);

                                        //PdfPCell Conjuc = new PdfPCell(new Phrase(Conjuction, fc));
                                        //Conjuc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //Conjuc.Border = Rectangle.BOX;
                                        //Groups.AddCell(Conjuc);

                                        //if (First)
                                        //{
                                        //    table.SpacingBefore = 30f;
                                        //    document.Add(table);                       //Header row
                                        //    table.DeleteBodyRows();
                                        //    First = false;
                                        //}

                                        document.Add(Groups);
                                        Groups.DeleteBodyRows();
                                        //break;
                                    }
                                }
                            }
                        }
                        //document.Add(table);

                    }
                }
            }

            document.Close();
            fs.Close();
            fs.Dispose();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            //}
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void Case0009Control_Load(object sender, EventArgs e)
        {
           
        }
        List<PopUp_Menu_L1_Entity> listItem_L1_Menu = new List<PopUp_Menu_L1_Entity>();
        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            listItem_L1_Menu.Clear();
            if (gvwQuestions.Rows.Count>0)
            {
                listItem_L1_Menu.Add(new PopUp_Menu_L1_Entity("C", "Copy"));

                foreach (PopUp_Menu_L1_Entity Ent_1 in listItem_L1_Menu)
                {
                    MenuItem Menu_L1 = new MenuItem();
                    Menu_L1.Text = Ent_1.Cat_Desc;
                    Menu_L1.Tag = Ent_1.Cat_Code;
                    contextMenu1.MenuItems.Add(Menu_L1);
                    
                }
            }
        }

        private void gvwGroups_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is string)
            {
                if (GetSelectedRow() != null)
                {
                    string strgroupcode = GetSelectedRow();
                    List<CaseELIGEntity> caseEligGEntity = _model.CaseSumData.GetCASEELIGadpgsGroup(gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString().Substring(0, 2), gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString().Substring(3, 2), gvwHierachy.SelectedRows[0].Cells["Hierarchy"].Value.ToString().Substring(6, 2), strgroupcode, "0", "Group");
                    if(caseEligGEntity.Count>0)
                    {
                        CaseELIGEntity caseEliggentity = new CaseELIGEntity();
                        string strMsg = string.Empty;

                        caseEliggentity.EligGroupCode = caseEligGEntity[0].EligGroupCode;
                        caseEliggentity.EligGroupDesc = caseEligGEntity[0].EligGroupDesc.Trim() + " Copy";
                        caseEliggentity.EligPoints = caseEligGEntity[0].EligPoints;
                        caseEliggentity.EligConjunction = caseEligGEntity[0].EligConjunction;
                        caseEliggentity.EligAgency = caseEligGEntity[0].EligAgency;
                        caseEliggentity.EligDept = caseEligGEntity[0].EligDept;
                        caseEliggentity.EligProgram = caseEligGEntity[0].EligProgram;
                        caseEliggentity.EligGroupSeq = "0";
                        caseEliggentity.AddOperator = BaseForm.UserID;
                        caseEliggentity.LstcOperator = BaseForm.UserID;
                        caseEliggentity.Mode = "Copy";
                        caseEliggentity.Type = "Group";
                        if (_model.CaseSumData.InsertUpdateDelCaseElig(caseEliggentity, out strMsg))
                        {
                            strgroupcode = strMsg;
                            AlertBox.Show("Copied Successfully");
                            RefreshGrid();

                            foreach(DataGridViewRow item in gvwGroups.Rows)
                            {
                                if (item.Cells["GroupCode"].Value.ToString().Trim()==strgroupcode.Trim())
                                {
                                    item.Selected = true;
                                    gvwGroups.CurrentCell = item.Cells[1];
                                }
                            }
                        }

                    }
                }

            }
        }

        //End Report Section...........................



    }
}