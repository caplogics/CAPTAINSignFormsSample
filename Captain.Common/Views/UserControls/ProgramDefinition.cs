#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;


using Wisej.Web;
using Wisej.Design;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;

using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ProgramDefinition : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public int strIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public ProgramDefinition(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            Privileges = privileges;
            List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty,BaseForm.UserID, BaseForm.BaseAdminAgency);
            foreach (ProgramDefinitionEntity programEntity in programEntityList)
            {
                string code = programEntity.Agency + "-" + programEntity.Dept + "-" + programEntity.Prog;

                int rowIndex = gvwProgram.Rows.Add(code, programEntity.DepAGCY, programEntity.DepYear, programEntity.Address1, programEntity.City);
                gvwProgram.Rows[rowIndex].Tag = programEntity;
                setTooltip(rowIndex, programEntity);
            }

            setControlEnabled(false);
            fillDropDowns();
            if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                List<CommonEntity> PAYMENTService = _model.lookupDataAccess.GetPaymentCategoryServicee();

                cmbPayCatservice.Items.Insert(0, new ListItem("Select One", "0"));
                cmbPayCatservice.SelectedIndex = 0;

                foreach (CommonEntity servicedata in PAYMENTService)
                {
                    if (servicedata.Extension == string.Empty)
                        cmbPayCatservice.Items.Add(new ListItem(servicedata.Desc, servicedata.Code));
                }

                cmbPayCatservice.Visible = true;
                lblPayemntserviceposting.Visible = true;
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            if (gvwProgram.Rows.Count > 0)
            {
                this.gvwProgram.SelectionChanged -= new System.EventHandler(this.gvwCustomer_SelectionChanged);
                gvwProgram.CurrentCell = gvwProgram.Rows[0].Cells[1];
                gvwProgram.Rows[0].Selected = true;
                this.gvwProgram.SelectionChanged += new System.EventHandler(this.gvwCustomer_SelectionChanged);
                gvwCustomer_SelectionChanged(gvwProgram, new EventArgs());
            }

            PopulateToolbar(oToolbarMnustrip);
        }

        public void RefreshGrid(ProgramDefinitionEntity programEntity)
        {
            gvwProgram.Rows.Clear();
            string programName = txtProgramName.Text.ToString();
            string hierarchy = txtHierarchy.Text.ToString();
            if (!hierarchy.Equals(string.Empty)) { hierarchy = hierarchy.Replace("**", string.Empty); }

            List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(programName, hierarchy,BaseForm.UserID,BaseForm.BaseAdminAgency);

            //List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty);
            foreach (ProgramDefinitionEntity progEntity in programEntityList)
            {
                string code = progEntity.Agency + "-" + progEntity.Dept + "-" + progEntity.Prog;

                int rowIndex = gvwProgram.Rows.Add(code, progEntity.DepAGCY, progEntity.DepYear, progEntity.Address1, progEntity.City);
                gvwProgram.Rows[rowIndex].Tag = progEntity;
                if (programEntity != null)
                {
                    if (programEntity.Agency.Equals(progEntity.Agency) && programEntity.Dept.Equals(progEntity.Dept) && programEntity.Prog.Equals(progEntity.Prog))
                    {
                        gvwProgram.Rows[rowIndex].Selected = true;
                        strIndex = rowIndex;
                    }
                }
                setTooltip(rowIndex, progEntity);
            }

            if (gvwProgram.Rows.Count != 0)
            {

                
                if (strIndex > 0)
                {
                    gvwProgram.Rows[strIndex].Selected = true;
                    gvwProgram.CurrentCell = gvwProgram.Rows[strIndex].Cells[1];
                    //gvwProgram.CurrentPage = ((strIndex + gvwProgram.ItemsPerPage) / gvwProgram.ItemsPerPage);
                    //int scrollPosition = 0;
                    //scrollPosition = gvwProgram.CurrentCell.RowIndex;
                    //int CurrentPage = (scrollPosition / gvwProgram.ItemsPerPage);
                    //CurrentPage++;
                    //gvwProgram.CurrentPage = CurrentPage;
                    //gvwProgram.FirstDisplayedScrollingRowIndex = scrollPosition;
                }                                                                 // Added By Yeswanth 07142012 Begin


                //gvwProgram.Rows[strIndex].Selected = true;
                //gvwProgram.CurrentPage = strPageIndex;
                if (strIndex == 0)
                {
                    gvwProgram.CurrentCell = gvwProgram.Rows[0].Cells[1];      
                    gvwProgram.Rows[0].Selected = true;
                    gvwCustomer_SelectionChanged(gvwProgram, new EventArgs());
                }
            }

            gvwProgram.Update();
            gvwProgram.ResumeLayout();
        }

        private void setTooltip(int rowIndex, ProgramDefinitionEntity dr)
        {

            string toolTipText = "Added By     : " + dr.AddOperator.Trim() + " on " + dr.DateAdd + "\n";
            toolTipText += "Modified By : " + dr.LSTCOperator.Trim() + " on " + dr.DateLSTC.ToString();

            foreach (DataGridViewCell cell in gvwProgram.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        #region properties

        /// <summary>
        /// 
        /// </summary>
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarDel { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }

        #endregion

        private void OnSearchClick(object sender, EventArgs e)
        {
            gvwProgram.Rows.Clear();

            string programName = txtProgramName.Text.ToString();
            string hierarchy = txtHierarchy.Text.ToString();
            if (!hierarchy.Equals(string.Empty)) { hierarchy = hierarchy.Replace("**", string.Empty); }

            List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(programName, hierarchy, BaseForm.UserID, BaseForm.BaseAdminAgency);
            if (programEntityList.Count > 0)
            {
                foreach (ProgramDefinitionEntity programEntity in programEntityList)
                {
                    string code = programEntity.Agency + "-" + programEntity.Dept + "-" + programEntity.Prog;

                    int rowIndex = gvwProgram.Rows.Add(code, programEntity.DepAGCY, programEntity.DepYear, programEntity.Address1, programEntity.City);
                    gvwProgram.Rows[rowIndex].Tag = programEntity;
                    setTooltip(rowIndex, programEntity);
                }

                if (gvwProgram.Rows.Count > 0)
                {
                    gvwProgram.Rows[0].Selected = true;
                    gvwCustomer_SelectionChanged(gvwProgram, new EventArgs());
                }
            }
            else
            {
                AlertBox.Show("No Record found with this criteria", MessageBoxIcon.Warning, null, ContentAlignment.BottomRight);
                //CommonFunctions.MessageBoxDisplay("Record does not exist");
                clearControls();
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (!txtHierarchy.Equals(string.Empty))
            {
                if (!CommonFunctions.IsNumeric(txtHierarchy.Text))
                {
                    _errorProvider.SetError(txtHierarchy, "Hierarchy should be numeric only");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtHierarchy, Convert.ToString(""));
                }
            }
            return (isValid);
        }

        private void gvwCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwProgram.SelectedRows.Count > 0)
            {
                ProgramDefinitionEntity row = gvwProgram.SelectedRows[0].Tag as ProgramDefinitionEntity;
                clearControls();
                if (row != null)
                {
                    strIndex = gvwProgram.SelectedRows[0].Index;
                    //strPageIndex = gvwProgram.CurrentPage;
                    fillTabs(row);
                    fillMasterPoverty();
                }
                setControlEnabled(false);
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
                ToolBarNew.ToolTipText = "New Program";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Program";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Program";
                ToolBarDel.Enabled = true;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Program Definition Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help";
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
                        ProgramDefinitionEntity selectedRec = GetSelectedRow();
                        if (selectedRec != null)
                        {
                            AddProgramDefinitionForm addProgramForm = new AddProgramDefinitionForm(BaseForm, "Add", selectedRec, Privileges);
                            addProgramForm.StartPosition = FormStartPosition.CenterScreen;
                            addProgramForm.ShowDialog();
                        }
                        else
                        {
                            AddProgramDefinitionForm addProgramForm = new AddProgramDefinitionForm(BaseForm, "Add", null, Privileges);
                            addProgramForm.StartPosition = FormStartPosition.CenterScreen;
                            addProgramForm.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Edit:
                        ProgramDefinitionEntity selectedRow = GetSelectedRow();
                        if (selectedRow != null)
                        {
                            AddProgramDefinitionForm editProgramForm = new AddProgramDefinitionForm(BaseForm, "Edit", selectedRow, Privileges);
                            editProgramForm.StartPosition = FormStartPosition.CenterScreen;
                            editProgramForm.ShowDialog();
                        }
                        break;
                    case Consts.ToolbarActions.Delete:
                        if (gvwProgram.Rows.Count > 0)
                        {
                            selectedRow = GetSelectedRow();
                            if (selectedRow != null)
                            {
                                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nProgram: " + selectedRow.ProgramName, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                            }
                        }
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

        private void MessageBoxHandler(DialogResult dialogresult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogresult == DialogResult.Yes)
                {
                    ProgramDefinitionEntity selectedRowEntity = GetSelectedRow();
                    string strMsg = _model.HierarchyAndPrograms.DeleteCaseDep(selectedRowEntity);
                    if (strMsg == "Success")
                    {
                        RefreshGrid(selectedRowEntity);
                    }
                    else
                    {
                        if (strMsg == "CASESUM")
                        {
                            AlertBox.Show("You can’t delete this program,as there are Dependents CASESUM Ref Hierarchy", MessageBoxIcon.Warning);

                        }
                        else
                        {
                            AlertBox.Show("You can’t delete this program, as there are Dependents", MessageBoxIcon.Warning);
                        }
                    }

                }
            //}
        }

        private void fillIncome(string incomes)
        {
            List<string> incomeList = new List<string>();
            if (incomes != null)
            {
                string[] incomeTypes = incomes.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    incomeList.Add(incomeTypes.GetValue(i).ToString());
                }
            }

            List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
            gvwIncomeTypes.Rows.Clear();
            foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
            {
                string flag = "false";
                if (incomes != null && incomeList.Contains(agyEntity.agycode))
                {
                    flag = "true";
                }
                if(flag != "false")
                {
                    int rowIndex = gvwIncomeTypes.Rows.Add(flag, agyEntity.agydesc);
                    gvwIncomeTypes.Rows[rowIndex].Tag = agyEntity;
                }
                
            }
        }

        private void fillRelation(string relations)
        {
            List<string> relationList = new List<string>();
            if (relations != null)
            {
                string[] relationTypes = relations.Split(' ');
                for (int i = 0; i < relationTypes.Length; i++)
                {
                    relationList.Add(relationTypes.GetValue(i).ToString());
                }
            }
            List<CommonEntity> lookUps = _model.lookupDataAccess.GetRelationship();
            gvwRelationShips.Rows.Clear();
            foreach (CommonEntity agyEntity in lookUps)
            {
                string flag = "false";
                if (relations != null && relationList.Contains(agyEntity.Code))
                {
                    flag = "true";
                }
                if (flag != "false")
                {
                    int rowIndex = gvwRelationShips.Rows.Add(flag, agyEntity.Desc);
                    gvwRelationShips.Rows[rowIndex].Tag = agyEntity;
                }
            }

        }

        private void fillEnrlHieracheis(string Agency, string Dept, string Program)
        {

            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("ALL", string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency=="**"?string.Empty: BaseForm.BaseAdminAgency);
            List<DepEnrollHierachiesEntity> DepEnrollList = _model.HierarchyAndPrograms.GetDepEntollHierachies(Agency, Dept, Program, string.Empty);
            gvwHiearchys.Rows.Clear();
            foreach (HierarchyEntity hieEntity in caseHierarchy)
            {
                foreach (DepEnrollHierachiesEntity item in DepEnrollList)
                {
                    if (item.Hierachies == hieEntity.Agency + hieEntity.Dept + hieEntity.Prog)
                    {
                        int rowIndex = gvwHiearchys.Rows.Add(hieEntity.Code, item.Nofoslots, LookupDataAccess.Getdate(item.StartDate), LookupDataAccess.Getdate(item.Enddate));
                        item.DepEnrollCode = hieEntity.Code;
                        item.DepEnrollDesc = hieEntity.HirarchyName.ToString();
                        item.Mode = "Update";
                        gvwHiearchys.Rows[rowIndex].Tag = item;
                        CommonFunctions.setTooltip(rowIndex, item.DepAddOperator, item.DepDateAdd, item.DepLstcOperator, item.DepDateLstc, gvwHiearchys);
                    }
                }


            }
        }

        private void fillDropDowns()
        {
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetMonths();
            foreach (CommonEntity month in commonEntity)
            {
                cmbStartMonth.Items.Add(new ListItem(month.Desc, month.Code));
            }
            cmbStartMonth.Items.Insert(0, new ListItem("", "0"));
            cmbStartMonth.SelectedIndex = 0;

            foreach (CommonEntity month in commonEntity)
            {
                cmbEndMonth.Items.Add(new ListItem(month.Desc, month.Code));
            }
            cmbEndMonth.Items.Insert(0, new ListItem("", "0"));
            cmbEndMonth.SelectedIndex = 0;

            commonEntity = _model.lookupDataAccess.GetDefaultQuestions();
            foreach (CommonEntity questions in commonEntity)
            {
                cmbDefaultQuestions.Items.Add(new ListItem(questions.Desc, questions.Code));
            }
            //cmbDefaultQuestions.Items.Insert(0, new ListItem("Select One", "0"));
            cmbDefaultQuestions.SelectedIndex = 0;

            List<AgyTabEntity> lookUpCustomerTypes = _model.lookupDataAccess.GetCustomerTypes();
            foreach (AgyTabEntity customerType in lookUpCustomerTypes)
            {
                cmbCustomerType.Items.Add(new ListItem(customerType.agydesc, customerType.agycode));
            }

            cmbCustomerType.Items.Insert(0, new ListItem("", "0"));
            cmbCustomerType.SelectedIndex = 0;

            List<AgyTabEntity> lookUpInvoiceSource = _model.lookupDataAccess.GetInvoiceSource();
            foreach (AgyTabEntity invoiceSource in lookUpInvoiceSource)
            {
                cmbInvoiceSource.Items.Add(new ListItem(invoiceSource.agydesc, invoiceSource.agycode));
            }
            cmbInvoiceSource.Items.Insert(0, new ListItem("", "0"));
            cmbInvoiceSource.SelectedIndex = 0;
        }

        private void fillMasterPoverty()
        {
            ProgramDefinitionEntity programEntity = GetSelectedRow();
            List<MasterPovertyEntity> modelPovertyList = _model.masterPovertyData.GetCaseGdlByHIE(programEntity.Agency, programEntity.Dept, programEntity.Prog);
            gvwMasterPoverty.Rows.Clear();
            foreach (MasterPovertyEntity hierarchy in modelPovertyList)
            {
                string code = hierarchy.GdlAgency + "-" + hierarchy.GdlDept + "-" + hierarchy.GdlProgram;
                int rowIndex = gvwMasterPoverty.Rows.Add(hierarchy.GdlType, hierarchy.GdlStartDate.ToString(), hierarchy.GdlEndDate, code);
            }
        }

        public void RefreshContactGrid()
        {
            ProgramDefinitionEntity programEntity = GetSelectedRow();
            if (programEntity != null)
            {
                DepContactEntity caseDepContactEntity = new DepContactEntity();
                caseDepContactEntity.Agency = programEntity.Agency;
                caseDepContactEntity.Dept = programEntity.Dept;
                caseDepContactEntity.Program = programEntity.Prog;
                List<DepContactEntity> contactEntity = _model.HierarchyAndPrograms.GetCASEDEPContacts(caseDepContactEntity);
                gvwContactInfo.Rows.Clear();
                if (contactEntity.Count > 0)
                    contactEntity = contactEntity.OrderBy(u => Convert.ToInt32(u.SEQ)).ToList();
                foreach (DepContactEntity contEntity in contactEntity)
                {
                    int rowIndex = gvwContactInfo.Rows.Add(contEntity.FirstName.Trim() + " " + contEntity.LastName.Trim(), CommonFunctions.GetPhoneNo(contEntity.Phone1.Trim()));
                    gvwContactInfo.Rows[rowIndex].Tag = contEntity;
                    CommonFunctions.setTooltip(rowIndex, contEntity.DepAddOperator, contEntity.DepDateAdd, contEntity.DepLstcOperator, contEntity.DepDateLstc, gvwContactInfo);
                }
            }
        }

        private void clearControls()
        {
            txtFname.Text = string.Empty;
            txtLname.Text = string.Empty;
            txtMI.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtAddress3.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtZip.Text = string.Empty;
            txtState.Text = string.Empty;

            txtEmail.Text = string.Empty;

            cbGenerateApp.Checked = false;
            txtAppNo.Text = string.Empty;

            cbIncomeVerification.Checked = false;
            cbIncludeOnly.Checked = false;
            cbWeekly.Checked = false;
            cbIntakeDate.Checked = false;
            cbPhohibitDuplicateSSN.Checked = false;
            cbProhibitDuplicateMember.Checked = false;
            rbAppFN.Checked = false;rbAppFNI.Checked = false;rbAppSSN.Checked = false;
            rbMemFN.Checked = false; rbMemFNI.Checked = false; rbMemSSN.Checked = false;
            cbSMI.Checked = false;
            cbCMI.Checked = false;
            cbFedOMB.Checked = false;
            cbHUD.Checked = false;
            cbIncludeIncomeVer.Checked = false;
            cbIntakeDate.Checked = false;
            cbAddress.Checked = false;
            cbAutoInactive.Checked = false;
            cbEditCaseTypes.Checked = false;
            cbSecretProgram.Checked = false;
            cbUom.Checked = false;
            cbZip.Checked = false;
            cbSS.Checked = false;
            cbEnroll.Checked = false;
            dtpFrom.Text = string.Empty;
            dtpFrom.Checked = false;
            dtpTo.Text = string.Empty;
            dtpTo.Checked = false;
            txtJuvAgeFrom.Text = string.Empty;
            txtSAgeFrom.Text = string.Empty;
            txtSAgeTo.Text = string.Empty;
            txtJuvAgeTo.Text = string.Empty;
            txtThreshold.Text = string.Empty;
            rbManual.Checked = false;
            cbPoints.Checked = false;
            chkIntakeProg.Checked = false;
            chkAttendanceTime.Checked = false;

            cbDateValidateIncome.Checked = false;
            cb30DaysDefault.Checked = false;
            cb60DaysDefault.Checked = false;
            cb90DaysDefault.Checked = false;
            cb30DaysUsed.Checked = false;
            cb60DaysUsed.Checked = false;
            cb90DaysUsed.Checked = false;
            txt30DaysFactor.Text = string.Empty;
            txt60DaysFactor.Text = string.Empty;
            txt90DaysFactor.Text = string.Empty;

            maskedTextBoxDefAcct.Text = string.Empty;
            txtFreeBF.Text = string.Empty;
            txtFreeAMSnack.Text = string.Empty;
            txtFreeLunch.Text = string.Empty;
            txtFreePMSnack.Text = string.Empty;
            txtFreeSupper.Text = string.Empty;
            txtFreeOthers.Text = string.Empty;

            txtReducedBF.Text = string.Empty;
            txtReducedAMSnack.Text = string.Empty;
            txtReducedLunch.Text = string.Empty;
            txtReducedPMSnack.Text = string.Empty;
            txtReducedSupper.Text = string.Empty;
            txtReducedOthers.Text = string.Empty;

            txtPaidBF.Text = string.Empty;
            txtPaidAMSnack.Text = string.Empty;
            txtPaidLunch.Text = string.Empty;
            txtPaidPMSnack.Text = string.Empty;
            txtPaidSupper.Text = string.Empty;
            txtPaidOthers.Text = string.Empty;

            gvwHiearchys.Rows.Clear();
            gvwIncomeTypes.Rows.Clear();
            gvwRelationShips.Rows.Clear();
            gvwContactInfo.Rows.Clear();
            gvwMasterPoverty.Rows.Clear();
            SetComboBoxValue(cmbStartMonth, "0");
            SetComboBoxValue(cmbEndMonth, "0");
            SetComboBoxValue(cmbCustomerType, "0");
            SetComboBoxValue(cmbInvoiceSource, "0");
            SetComboBoxValue(cmbPayCatservice, "0");
            // SetComboBoxValue(cmbDefaultQuestions, Consts.Common.SelectOne);

            RefreshContactGrid();
        }

        private void fillTabs(ProgramDefinitionEntity programEntity)
        {
            if (programEntity != null)
            {
                txtFname.Text = programEntity.DepFirstName;
                txtLname.Text = programEntity.DepLastName;
                txtMI.Text = programEntity.DepMI;
                txtPhone.Text = programEntity.Phone.Trim();
                txtFax.Text = programEntity.DepFax.Trim();
                txtAddress1.Text = programEntity.Address1;
                txtAddress2.Text = programEntity.Address2;
                txtAddress3.Text = programEntity.Address3;
                txtCity.Text = programEntity.City;
                txtZip.Text = programEntity.Zip;

                if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                {
                    CommonFunctions.SetComboBoxValue(cmbPayCatservice, programEntity.DepSerpostPAYCAT.ToString());
                }

                if (!programEntity.Zip.Equals(string.Empty))
                    txtZip.Text = "00000".Substring(0, 5 - programEntity.Zip.Length) + programEntity.Zip + "0000".Substring(0, 4 - programEntity.ZipPlus.Length) + programEntity.ZipPlus;

                if (!programEntity.DepAppNo.Equals(string.Empty))
                    txtAppNo.Text = "00000000".Substring(0, 8 - programEntity.DepAppNo.Length) + programEntity.DepAppNo.Trim();

                txtState.Text = programEntity.State;

                txtEmail.Text = programEntity.emailID;

                if (programEntity.DepGenerateApps.Equals("Y"))
                    cbGenerateApp.Checked = true;
                else
                    cbGenerateApp.Checked = false;

                cbIncomeVerification.Checked = programEntity.IncomeVerMsg.Equals("Y") ? true : false;
                cbIncludeOnly.Checked = programEntity.IncomeTypeOnly.Equals("Y") ? true : false;
                cbWeekly.Checked = programEntity.IncomeWeek.Equals("Y") ? true : false;
                cbIntakeDate.Checked = programEntity.IntakeEdit.Equals("1") ? true : false;
                cbPhohibitDuplicateSSN.Checked = programEntity.PRODUPSSN.Equals("Y") ? true : false;
                if(cbPhohibitDuplicateSSN.Checked)
                {
                    switch (programEntity.PRODUPAPP_BY)
                    {
                        case "1": rbAppSSN.Checked = true; break;
                        case "2": rbAppFN.Checked = true; break;
                        case "3": rbAppFNI.Checked = true; break;
                    }
                }
                cbProhibitDuplicateMember.Checked = programEntity.ProDupMEM.Equals("Y") ? true : false;
                if (cbProhibitDuplicateMember.Checked)
                {
                    switch (programEntity.PRODUPMEM_BY)
                    {
                        case "1": rbMemSSN.Checked = true; break;
                        case "2": rbMemFN.Checked = true; break;
                        case "3": rbMemFNI.Checked = true; break;
                    }
                }
                cbSMI.Checked = programEntity.DepSMIUsed.Equals("Y") ? true : false;
                cbCMI.Checked = programEntity.DepCMIUsed.Equals("Y") ? true : false;
                cbFedOMB.Checked = programEntity.DepFEDUsed.Equals("Y") ? true : false;
                cbHUD.Checked = programEntity.DepHUDUsed.Equals("Y") ? true : false;
                cbIncludeIncomeVer.Checked = programEntity.DepIncludeIncVer.Equals("1") ? true : false;
                cbIntakeDate.Checked = programEntity.IntakeEdit.Equals("1") ? true : false;
                cbAddress.Checked = programEntity.DepAddressEdit.Equals("Y") ? true : false;
                cbAutoInactive.Checked = programEntity.AutoInActivation.Equals("1") ? true : false;
                cbEditCaseTypes.Checked = programEntity.CaseTypeEdit.Equals("Y") ? true : false;
                cbSecretProgram.Checked = programEntity.SecretProgram.Equals("Y") ? true : false;
                cbUom.Checked = programEntity.DepUnitCalc.Equals("Y") ? true : false;
                cbZip.Checked = programEntity.ZipSearch.Equals("Y") ? true : false;
                cbSS.Checked = programEntity.SSNReasonFlag.Equals("Y") ? true : false;
                cbEnroll.Checked = programEntity.SelectedClient.Equals("1") ? true : false;
                chkIntakeProg.Checked = programEntity.DepIntakeProg.Equals("Y") ? true : false;
                chkssnMask.Visible = false;
                chkssnauto.Visible = false;
                chkssnMask.Checked = false;
                chkssnauto.Checked = false;
                if (programEntity.DepIntakeProg.Equals("Y"))
                {
                    //chkssnMask.Visible = true;
                    //chkssnauto.Visible = true;

                    //if (programEntity.DepSsnAutoAssign == "1")
                    //    chkssnMask.Checked = true;
                    //if (programEntity.DepSsnAutoAssign == "2")
                    //    chkssnauto.Checked = true;
                }

                chkAttendanceTime.Checked = programEntity.DepAttendanceTimes.Equals("Y") ? true : false;
                if (programEntity.IntakeEdit.Equals("1"))
                {
                    if (!programEntity.IntakeFDate.Equals(string.Empty))
                    { dtpFrom.Text = programEntity.IntakeFDate; dtpFrom.Checked = true; }
                    if (!programEntity.IntakeTDate.Equals(string.Empty))
                    { dtpTo.Text = programEntity.IntakeTDate; dtpTo.Checked = true; }
                }
                txtJuvAgeFrom.Text = programEntity.DepJUVFromAge.ToString().Trim();
                txtSAgeFrom.Text = programEntity.DepSENFromAge.ToString().Trim();
                txtSAgeTo.Text = programEntity.DepSENToAge.ToString().Trim();
                txtJuvAgeTo.Text = programEntity.DepJUVToAge.ToString().Trim();
                txtThreshold.Text = programEntity.DepThreshold.ToString().Trim();
                if (programEntity.DepAutoRefer.Equals("Y"))
                {
                    rbSystem.Checked = true;
                }
                else
                {
                    rbManual.Checked = true;
                }
                cbPoints.Checked = programEntity.SIMPointsMethod.Equals("Y") ? true : false;

                cbDateValidateIncome.Checked = programEntity.IncomeDateValidate.Equals("1") ? true : false;
                cb30DaysDefault.Checked = programEntity.DepIncExcIntDefault1.Equals("1") ? true : false;
                cb60DaysDefault.Checked = programEntity.DepIncExcIntDefault2.Equals("1") ? true : false;
                cb90DaysDefault.Checked = programEntity.DepIncExcIntDefault3.Equals("1") ? true : false;
                cb30DaysUsed.Checked = programEntity.DepIncExcIntUsed1.Equals("1") ? true : false;
                cb60DaysUsed.Checked = programEntity.DepIncExcIntUsed2.Equals("1") ? true : false;
                cb90DaysUsed.Checked = programEntity.DepIncExcIntUsed3.Equals("1") ? true : false;
                txt30DaysFactor.Text = programEntity.DepIncExcIntFactr1.ToString().Trim();
                txt60DaysFactor.Text = programEntity.DepIncExcIntFactr2.ToString().Trim();
                txt90DaysFactor.Text = programEntity.DepIncExcIntFactr3.ToString().Trim();

                if (!programEntity.Account.Equals(string.Empty))
                    maskedTextBoxDefAcct.Text = "00000000000000".Substring(0, 14 - programEntity.Account.Trim().Length) + programEntity.Account.Trim();

                txtFreeBF.Text = programEntity.Free1.ToString();
                txtFreeAMSnack.Text = programEntity.Free2.ToString();
                txtFreeLunch.Text = programEntity.Free3.ToString();
                txtFreePMSnack.Text = programEntity.Free4.ToString();
                txtFreeSupper.Text = programEntity.Free5.ToString();
                txtFreeOthers.Text = programEntity.Free6.ToString();

                txtReducedBF.Text = programEntity.Reduced1.ToString();
                txtReducedAMSnack.Text = programEntity.Reduced2.ToString();
                txtReducedLunch.Text = programEntity.Reduced3.ToString();
                txtReducedPMSnack.Text = programEntity.Reduced4.ToString();
                txtReducedSupper.Text = programEntity.Reduced5.ToString();
                txtReducedOthers.Text = programEntity.Reduced6.ToString();

                if (programEntity.DepHsAgeMethod.Trim() == "S")
                    rdoSchool.Checked = true;
                else
                    rdoZip.Checked = true;

                //if (programEntity.Search_Cntrl == "F")
                //    rdbFNDOBSSN.Checked = true;
                //else if (programEntity.Search_Cntrl == "C")
                //    rdbClientID.Checked = true;
                //else
                //    rdbFNDOBSSN.Checked = true;

                txtPaidBF.Text = programEntity.Paid1.ToString();
                txtPaidAMSnack.Text = programEntity.Paid2.ToString();
                txtPaidLunch.Text = programEntity.Paid3.ToString();
                txtPaidPMSnack.Text = programEntity.Paid4.ToString();
                txtPaidSupper.Text = programEntity.Paid5.ToString();
                txtPaidOthers.Text = programEntity.Paid6.ToString();
                gvwHiearchys.Rows.Clear();

                fillEnrlHieracheis(programEntity.Agency.ToString(), programEntity.Dept.ToString(), programEntity.Prog);
                fillIncome(programEntity.DepIncomeTypes);
                fillRelation(programEntity.DepReleataionTypes);

                SetComboBoxValue(cmbStartMonth, programEntity.StartMonth);
                SetComboBoxValue(cmbEndMonth, programEntity.EndMonth);
                SetComboBoxValue(cmbCustomerType, programEntity.Type);
                SetComboBoxValue(cmbInvoiceSource, programEntity.Source);
                SetComboBoxValue(cmbDefaultQuestions, programEntity.LoadProgramQuestions);


                RefreshContactGrid();

            }
        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public ProgramDefinitionEntity GetSelectedRow()
        {
            ProgramDefinitionEntity programEntity = null;
            if (gvwProgram != null)
            {
                foreach (DataGridViewRow dr in gvwProgram.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        programEntity = dr.Tag as ProgramDefinitionEntity;
                        break;
                    }
                }
            }
            return programEntity;
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
            txtFname.Enabled = flag;
            txtLname.Enabled = flag;

        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        public void RefreshMaindata()
        {
            gvwProgram.SelectionChanged -= new EventHandler(gvwCustomer_SelectionChanged);
            gvwProgram.Rows.Clear();
            gvwProgram.SelectionChanged += new EventHandler(gvwCustomer_SelectionChanged);

            List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency);
            foreach (ProgramDefinitionEntity programEntity in programEntityList)
            {
                string code = programEntity.Agency + "-" + programEntity.Dept + "-" + programEntity.Prog;

                int rowIndex = gvwProgram.Rows.Add(code, programEntity.DepAGCY, programEntity.DepYear, programEntity.Address1, programEntity.City);
                gvwProgram.Rows[rowIndex].Tag = programEntity;
                setTooltip(rowIndex, programEntity);
            }

            setControlEnabled(false);
            if (gvwProgram.Rows.Count > 0)
            {
                //this.gvwProgram.SelectionChanged -= new System.EventHandler(this.gvwCustomer_SelectionChanged);
                gvwProgram.CurrentCell = gvwProgram.Rows[0].Cells[1];
                gvwProgram.Rows[0].Selected = true;
                //int scrollPosition = 0;
                //scrollPosition = gvRdateRange.CurrentCell.RowIndex;
                ////int CurrentPage = (scrollPosition / gvRdateRange.ItemsPerPage);
                ////CurrentPage++;
                ////gvRdateRange.CurrentPage = CurrentPage;
                ////gvRdateRange.FirstDisplayedScrollingRowIndex = scrollPosition;
                //this.gvRdateRange.SelectionChanged += new System.EventHandler(this.gvRdateRange_SelectionChanged);
                gvwCustomer_SelectionChanged(gvwProgram, new EventArgs());
            }
        }


    }
}