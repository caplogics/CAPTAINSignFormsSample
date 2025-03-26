#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PirQuestForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion
        public PirQuestForm(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;           
            //lblHeader.Text = privilegeEntity.PrivilegeName;
            this.Text = /*privilegeEntity.Program*/privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
            _model = new CaptainModel();
             GetPirSectionDetails();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtQuesCode.Validator = TextBoxValidation.IntegerValidator;
            txtQuesNo.Validator = TextBoxValidation.IntegerValidator;
            txtPosition.Validator = TextBoxValidation.IntegerValidator;
            txtSeqCode.Validator = TextBoxValidation.IntegerValidator;
            propPirQuest = _model.PIRData.GetPIRQUEST(BaseForm.BaseYear);
           
        }

        public PirQuestForm(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity, PIRQUESTEntity drpirQues)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            //lblHeader.Text = privilegeEntity.PrivilegeName;
            this.Text = privilegeEntity.Program + " - " + mode;
            _model = new CaptainModel();
            GetPirSectionDetails();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtQuesCode.Validator = TextBoxValidation.IntegerValidator;
            txtQuesNo.Validator = TextBoxValidation.IntegerValidator;
            txtSeqCode.Validator = TextBoxValidation.IntegerValidator;
            txtPosition.Validator = TextBoxValidation.IntegerValidator;
            propPirQuest = null;
            if (drpirQues != null)
            { 
                txtQuesNo.Text = drpirQues.Ques_Unique_ID.ToString();
                txtPosition.Text = drpirQues.Ques_Position.ToString();
                txtQuesCode.Text = drpirQues.Ques_Code.ToString();
                txtQuesSection.Text = drpirQues.Ques_Seq.ToString();
                txtSeqCode.Text = drpirQues.Ques_SCode.ToString();
                txtDesc.Text = drpirQues.Ques_Desc.ToString();
                txtFundType.Text = drpirQues.Fund_Type.ToString();
                CommonFunctions.SetComboBoxValue(cmbQuesType,drpirQues.Ques_Type.ToString());
                CommonFunctions.SetComboBoxValue(cmbSection,drpirQues.Ques_section.ToString());

                if (drpirQues.Active_Status.ToString().ToUpper() == "A")
                    chkStatus.Checked = true;
                else
                    chkStatus.Checked = false;

                if (drpirQues.Ques_A1Day.ToString().ToUpper() == "Y")
                    chkAttnday.Checked = true;
                else
                    chkAttnday.Checked = false;

                if (drpirQues.Ques_Bold.ToString().ToUpper() == "Y")
                    chkBold.Checked = true;
                else
                    chkBold.Checked = false;

                txtQuesNo.Enabled = false;
                txtPosition.Enabled = false;

                txtQuesCode.Enabled = false;
                txtQuesSection.Enabled = false;
                txtSeqCode.Enabled = false;
                txtFundType.Enabled = false;
                cmbQuesType.Enabled = false;
                cmbSection.Enabled = false;
                chkStatus.Enabled = false;
            }

        }


        public void GetPirSectionDetails()
        {
            cmbSection.Items.Insert(0, new ListItem("Section - A", "A"));
            cmbSection.Items.Insert(1, new ListItem("Section - B", "B"));
            cmbSection.Items.Insert(2, new ListItem("Section - C", "C"));
            cmbSection.SelectedIndex = 2;

            //cmbFund.Items.Insert(0, new ListItem("All Funds", "9"));
            //cmbFund.Items.Insert(1, new ListItem("HS   - H", "H"));
            //cmbFund.Items.Insert(2, new ListItem("HS 2 - I", "I"));
            //cmbFund.Items.Insert(3, new ListItem("EHS  - E", "E"));
            //cmbFund.Items.Insert(4, new ListItem("EHSCCP-S", "S"));
            //cmbFund.Items.Insert(5, new ListItem("HS and EHS", "1"));
            //cmbFund.Items.Insert(6, new ListItem("HS and EHSCCP", "2"));
            //cmbFund.Items.Insert(7, new ListItem("EHS and EHSCCP", "3"));
            //cmbFund.SelectedIndex = 0;

            cmbQuesType.Items.Insert(0, new ListItem(" ", "0"));
           // cmbQuesType.Items.Insert(1, new ListItem("A", "A"));
            cmbQuesType.Items.Insert(1, new ListItem("F", "F"));
            cmbQuesType.Items.Insert(2, new ListItem("H", "H"));
            cmbQuesType.Items.Insert(3, new ListItem("I", "I"));
            cmbQuesType.Items.Insert(4, new ListItem("T", "T"));
            cmbQuesType.SelectedIndex = 4;
        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        public List<PIRQUESTEntity> propPirQuest { get; set; }

        public string Mode { get; set; }

        private void OnHelpClick(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PIRQUESTEntity pirQuest = new PIRQUESTEntity();
                pirQuest.Ques_Year = BaseForm.BaseYear;
                pirQuest.Ques_Code = txtQuesCode.Text;
                pirQuest.Ques_Desc = txtDesc.Text;
                pirQuest.Ques_SCode = txtSeqCode.Text;
                pirQuest.Ques_Seq = txtQuesSection.Text;
                pirQuest.Ques_Unique_ID = txtQuesNo.Text;
                pirQuest.Ques_Position = txtPosition.Text;
                pirQuest.Ques_section = ((ListItem)cmbSection.SelectedItem).Value.ToString();
                pirQuest.Ques_Type = ((ListItem)cmbQuesType.SelectedItem).Value.ToString();
                pirQuest.Fund_Type = txtFundType.Text.ToString();// ((ListItem)cmbFund.SelectedItem).Value.ToString();
                pirQuest.Active_Status = chkStatus.Checked == true ? "A" : "I";
                pirQuest.Ques_A1Day = chkAttnday.Checked == true ? "Y" : "N";
                pirQuest.Ques_Bold = chkBold.Checked == true ? "Y" : "N";
                pirQuest.Mode = Mode;
                if (_model.PIRData.InsertUpdatePirQues(pirQuest))
                {
                    PIR20001Control pir20001Control = BaseForm.GetBaseUserControl() as PIR20001Control;
                    if (pir20001Control != null)
                    {
                        pir20001Control.RefreshGridData(txtQuesNo.Text + txtQuesCode.Text + txtSeqCode.Text + txtQuesSection.Text);
                    }

                    this.Close();
                    this.Close();
                    AlertBox.Show("Saved Successfully");
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtQuesNo.Text))
            {
                _errorProvider.SetError(txtQuesNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQNo.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtQuesNo, null);
               
            }

            if (String.IsNullOrEmpty(txtQuesCode.Text.Trim()))
            {
                _errorProvider.SetError(txtQuesCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtQuesCode, null);
            }

            if (String.IsNullOrEmpty(txtPosition.Text.Trim()))
            {
                _errorProvider.SetError(txtPosition, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPosition.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtPosition, null);
            }

            if (String.IsNullOrEmpty(txtFundType.Text.Trim()))
            {
                _errorProvider.SetError(btnFundType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFundType.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(btnFundType, null);
            }

            if (((ListItem)cmbQuesType.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbQuesType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQType.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbQuesType, null);
            }

            if (String.IsNullOrEmpty(txtQuesSection.Text.Trim()))
            {

                _errorProvider.SetError(txtQuesSection, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQSection.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;

            }
            else
            {
              if(Mode.Equals("ADD"))
              {
                    PIRQUESTEntity pirquescheck = propPirQuest.Find(u => u.Ques_Unique_ID == txtQuesNo.Text && u.Ques_Code == txtQuesCode.Text && u.Ques_SCode == txtSeqCode.Text && u.Ques_Seq == txtQuesSection.Text);
                    if (pirquescheck != null)
                    {
                        _errorProvider.SetError(txtQuesSection, "Section already exist");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtQuesSection, null);
                    }
                }
                else
                    _errorProvider.SetError(txtQuesSection, null);
               


            }

            if (String.IsNullOrEmpty(txtSeqCode.Text.Trim()))
            {

                _errorProvider.SetError(txtSeqCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQSCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;

            }
            else
            {

                _errorProvider.SetError(txtSeqCode, null);
               
            }
            if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
            {

                _errorProvider.SetError(txtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;

            }
            else
            {

                _errorProvider.SetError(txtDesc, null);
            }

           

            return (isValid);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnFundType_Click(object sender, EventArgs e)
        {
            AlertCodeForm objform = new AlertCodeForm(BaseForm, Privileges, txtFundType.Text, "FUNDS", "Funds");
            objform.FormClosed += new FormClosedEventHandler(objform_FundFormClosed);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.ShowDialog();

        }

        void objform_FundFormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.propAlertCode.Contains("9"))
            {
                txtFundType.Text = "9";
            }
            else
            {
                txtFundType.Text = form.propAlertCode;
            }
        }
    }
}