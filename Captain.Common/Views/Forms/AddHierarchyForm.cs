#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Wisej.Web;
using Wisej.Design;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AddHierarchyForm : Form
    {

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public AddHierarchyForm(BaseForm baseForm, string mode, string hierarchyType, string agency, string dept, string program, string strShortName)
        {
            InitializeComponent();
            _model = new CaptainModel();
            HierarchyType = hierarchyType;
            Mode = mode;
            BaseForm = baseForm;
            Agency = agency;
            Dept = dept;
            Program = program;
            txtCode.Validator = TextBoxValidation.IntegerValidator;

            if (HierarchyType.Equals("AGENCY"))
            {
                this.Text = "Agency Definition" + " - " + Mode;//Mode + " Agency";
                // lblHeader.Text = "Agency Definition";
                lblName.Text = "Agency Name";
                this.lblReqAgencyName.Location = new Point(90, 41);
                this.pnlFormat.Visible = false;
                txtShortName.Text = strShortName;
            }
            else if (HierarchyType.Equals("DEPT"))
            {
                this.Text = "Department Definition" + " - " + Mode;//Mode + " Department";
                lblName.Text = "Department Name";
                // lblHeader.Text = "Department Definition";
                this.Size = new Size(415,218);
                this.lblReqAgencyName.Location = new Point(119, 41);
                txtCode.Location = new Point(140,8);
                txtName.Location = new Point(140,40);
                txtShortName.Location = new Point(140, 88);//(140,72);
                cbIntake.Location = new Point(300, 90);//(137,105);
                this.pnlFormat.Visible = false;
                setVisible(false);
                txtShortName.Text = "";
            }
            else if (HierarchyType.Equals("PROGRAM"))
            {
                this.Text = "Program Definition" + " - " + Mode;//Mode + " Program";
                //  lblHeader.Text = "Program Definition";
                lblName.Text = "Program Name";
                this.Size = new Size(392, 218);
                this.lblReqAgencyName.Location = new Point(99, 41);
                txtCode.Location = new Point(118, 8);
                txtName.Location = new Point(118, 40);
                txtShortName.Location = new Point(118, 88);//(118, 72);
                cbIntake.Location = new Point(280, 90);//(115,105);
                this.pnlFormat.Visible = false;
                setVisible(false);
                txtShortName.Text = "";
            }

            fillDropdowns();
           
            if (Mode.Equals("Edit"))
            {
                fillHierarchyForm();
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }

        private void setVisible(bool flag)
        {
            lblAgencyRep.Visible = flag;
            lblCNFormat.Visible = flag;
            lblCWFormat.Visible = flag;
            cmbAgencyRep.Visible = flag;
            cmbCaseWorkName.Visible = flag;
            cmbClientName.Visible = flag;
            //btnOk.Location = new System.Drawing.Point(301, 5);
            //btnCancel.Location = new System.Drawing.Point(379, 5);
            //pnlName.Size = new System.Drawing.Size(400, 132);//(453, 132);
            //this.Size = new System.Drawing.Size(400,222);//(453, 222);
        }

        private void fillDropdowns()
        {
            List<HierarchyEntity> hierarchyAgencyRep = _model.lookupDataAccess.GetAgencyRepresentation();
            foreach (HierarchyEntity hierarchyEntity in hierarchyAgencyRep)
            {
                cmbAgencyRep.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));
            }
            cmbAgencyRep.SelectedIndex = 0;

            List<HierarchyEntity> hierarchyClientName = _model.lookupDataAccess.GetClientNameFormat();
            foreach (HierarchyEntity hierarchyEntity in hierarchyClientName)
            {
                cmbClientName.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));
            }
            cmbClientName.SelectedIndex = 0;

            List<HierarchyEntity> hierarchyCaseWorkerName = _model.lookupDataAccess.GetCaseWorkerFormat();
            foreach (HierarchyEntity hierarchyEntity in hierarchyCaseWorkerName)
            {
                cmbCaseWorkName.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));
            }
            cmbCaseWorkName.SelectedIndex = 0;
        }

        private void fillHierarchyForm()
        {
            HierarchyEntity hierarchyDetails = _model.HierarchyAndPrograms.GetCaseHierarchy(HierarchyType, Agency, Dept, Program, string.Empty, string.Empty);
            if (hierarchyDetails != null)
            {
                if (HierarchyType.Equals("AGENCY"))
                {
                    txtCode.Text = hierarchyDetails.Agency;
                    SetComboBoxValue(cmbAgencyRep, hierarchyDetails.HIERepresentation);
                    SetComboBoxValue(cmbCaseWorkName, hierarchyDetails.CaseWorker);
                    SetComboBoxValue(cmbClientName, hierarchyDetails.CNFormat);
                }
                else if (HierarchyType.Equals("DEPT"))
                {
                    txtCode.Text = hierarchyDetails.Dept;
                }
                else if (HierarchyType.Equals("PROGRAM"))
                {
                    txtCode.Text = hierarchyDetails.Prog;
                }
                txtCode.Enabled = false;
                txtName.Text = hierarchyDetails.HirarchyName.Trim();
                txtShortName.Text = hierarchyDetails.ShortName.Trim();
                if (hierarchyDetails.Intake.Equals("Y")) cbIntake.Checked = true;

            }
        }

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


        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string HierarchyType { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Program { get; set; }

        private bool isCodeExists(string code)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                HierarchyEntity hierarchyEntity = _model.lookupDataAccess.GetCaseHierarchy(HierarchyType + "CheckDup", Agency, Dept, Program, string.Empty,string.Empty);
                if (hierarchyEntity != null)
                {
                    isExists = true;
                }
            }
            return isExists;
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                _errorProvider.SetError(txtCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (isCodeExists(txtCode.Text.Trim()))
                {
                    _errorProvider.SetError(txtCode, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtCode, null);
                }
            }

            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                _errorProvider.SetError(txtName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtName, null);
            }

            if (String.IsNullOrEmpty(txtShortName.Text.Trim()))
            {
                _errorProvider.SetError(txtShortName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblShortName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtShortName, null);
            }

            return (isValid);
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            try
            {
                if (HierarchyType.Equals("AGENCY"))
                {
                    Agency = txtCode.Text;
                }
                else if (HierarchyType.Equals("DEPT"))
                {
                    Dept = txtCode.Text;
                }
                else if (HierarchyType.Equals("PROGRAM"))
                {
                    Program = txtCode.Text;
                }

                if (ValidateForm())
                {
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
                    hierarchyEntity.Agency = Agency;
                    hierarchyEntity.Dept = Dept;
                    hierarchyEntity.Prog = Program;
                    hierarchyEntity.HirarchyName = txtName.Text;
                    hierarchyEntity.ShortName = txtShortName.Text;
                    hierarchyEntity.Intake = cbIntake.Checked ? "Y" : "N";
                    if (HierarchyType.Equals("AGENCY"))
                    {
                        hierarchyEntity.Agency = txtCode.Text;
                        hierarchyEntity.HIERepresentation = ((ListItem)cmbAgencyRep.SelectedItem).Value.ToString();
                        hierarchyEntity.CWFormat = ((ListItem)cmbCaseWorkName.SelectedItem).Value.ToString();
                        hierarchyEntity.CNFormat = ((ListItem)cmbClientName.SelectedItem).Value.ToString();
                    }
                    else if (HierarchyType.Equals("DEPT"))
                    {
                        hierarchyEntity.Dept = txtCode.Text;
                    }
                    else if (HierarchyType.Equals("PROGRAM"))
                    {
                        hierarchyEntity.Prog = txtCode.Text;
                    }
                    hierarchyEntity.Mode = Mode;
                    hierarchyEntity.AddOperator = BaseForm.UserID;
                    hierarchyEntity.LSTCOperator = BaseForm.UserID;

                    if (_model.HierarchyAndPrograms.InsertUpdateHierarchy(hierarchyEntity))
                    {
                        if (HierarchyType.Equals("PROGRAM"))
                        {
                            ProgramDefinitionEntity programEntity = new ProgramDefinitionEntity();
                            programEntity.Agency = Agency;
                            programEntity.Dept = Dept;
                            programEntity.Prog = Program;
                            programEntity.DepAGCY = txtName.Text.Trim();
                            programEntity.ShortName = txtShortName.Text.Trim();
                            programEntity.Mode = "Program";
                            _model.HierarchyAndPrograms.InsertCaseDep(programEntity);

                        }

                        HierarchyDefinitionControl hierarchyControl = BaseForm.GetBaseUserControl() as HierarchyDefinitionControl;
                        if (Mode == "Add")
                        {
                            AlertBox.Show("Saved Successfully");
                        }
                        else
                        {
                            AlertBox.Show("Updated Successfully");
                        }
                        if (hierarchyControl != null)
                        {
                            hierarchyControl.RefreshGrid(HierarchyType, txtCode.Text);
                        }
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CommonTextField_LostFocus(object sender, EventArgs e)
        {
            if (sender == txtCode)
            {
                if (txtCode.Text.Length == 1)
                    txtCode.Text = "0" + txtCode.Text;
                if (!(string.IsNullOrEmpty(txtCode.Text)) && txtCode.Text.Length == 2)
                    _errorProvider.SetError(txtCode, null);
            }

            if (sender == txtShortName && !(string.IsNullOrEmpty(txtShortName.Text)))
                _errorProvider.SetError(txtShortName, null);

            if (sender == txtName && !(string.IsNullOrEmpty(txtName.Text)))
                _errorProvider.SetError(txtName, null);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //if (HierarchyType.Equals("DEPT"))
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN00009_department");
            //else if (HierarchyType.Equals("PROGRAM"))
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN00009_Program");
        }




    }
}