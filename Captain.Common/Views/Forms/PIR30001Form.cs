using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using Captain.DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class PIR30001Form : Form
    {
        #region Private Variables

        ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public PIR30001Form(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity)
        {

            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();

            _baseForm = baseForm;
            _privileges = privilegeEntity;
            Mode = mode;

            fillSectionCombo();
            fillQuesTypeCombo();
            fillColumn1Combo();
            fillColumn2Combo();


            //fillFormControls();

        }

        public PIR30001Form(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity, PIRQUEST2Entity drpirQues)
        {
            InitializeComponent();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();

            _baseForm = baseForm;
            _privileges = privilegeEntity;
            Mode = mode;
            pirQUEST2Entity = drpirQues;

            fillSectionCombo();
            fillQuesTypeCombo();
            fillColumn1Combo();
            fillColumn2Combo();

            fillFormControls();

            if (pirQUEST2Entity != null)
            {
                txtUniqueID.Enabled = false;
                txtQuesCode.Enabled = false;
                txtSubQuesNo.Enabled = false;
                txtQuesSec.Enabled = false;
                cmbSection.Enabled = false;
                cmbQuesType.Enabled = false;
                txtPosition.Enabled = false;
            }
        }

        #region Properties

        public BaseForm _baseForm
        {
            get; set;
        }
        public PrivilegeEntity _privileges
        {
            get; set;
        }
        public string Mode
        {
            get; set;
        }

        public PIRQUEST2Entity pirQUEST2Entity
        {
        get; set; 
        }

        #endregion

        private void chkbCol1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbCol1.Checked)
            {
                chkbPrint.Visible = true;
                cmbCol1.Enabled = true;
            }
            else
            {
                _errorProvider.SetError(cmbCol1, null);
                if (chkbCol2.Checked == false)
                {
                    chkbPrint.Checked = false;
                    chkbPrint.Visible = false;
                }
                cmbCol1.SelectedIndex = 0;
                cmbCol1.Enabled = false;

                if (chkbCol2.Checked)
                {
                    chkbCol2.Checked = false;

                    if(cmbCol2.SelectedIndex != 0)
                        cmbCol2.SelectedIndex = 0;
                }
            }
        }

        private void chkbCol2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbCol2.Checked)
            {
                if (chkbCol1.Checked && cmbCol1.SelectedIndex != 0)
                {
                    chkbPrint.Visible = true;
                    cmbCol2.Enabled = true;
                }
                else
                {
                    AlertBox.Show("Please Select an Option from Column 1 Combo box first", MessageBoxIcon.Warning);
                }
            }
            else
            {
                _errorProvider.SetError(cmbCol2, null);
                if (chkbCol1.Checked == false)
                {
                    chkbPrint.Checked = false;
                    chkbPrint.Visible = false;
                }
                cmbCol2.SelectedIndex = 0;
                cmbCol2.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate_Form())
            {
                PIRQUEST2Entity pir2Quest = new PIRQUEST2Entity();
                pir2Quest.PIR_Ques_Year = _baseForm.BaseYear;
                pir2Quest.PIR_Ques_Unique_ID = txtUniqueID.Text;
                pir2Quest.PIR_Section = ((ListItem)cmbSection.SelectedItem).Value.ToString();
                pir2Quest.PIR_Fund_Type = txtFundType.Text.ToString();
                pir2Quest.PIR_Ques_Position = txtPosition.Text;
                pir2Quest.PIR_Ques_Code = txtQuesCode.Text;
                pir2Quest.PIR_Ques_SCode = txtSubQuesNo.Text;
                pir2Quest.PIR_Ques_Section = txtQuesSec.Text.Trim();
                pir2Quest.PIR_Ques_Type = ((ListItem)cmbQuesType.SelectedItem).Value.ToString();
                pir2Quest.PIR_Ques_Desc = txtDesc.Text;
                pir2Quest.PIR_Ques_Col1 = ((ListItem)cmbCol1.SelectedItem).Value.ToString();
                pir2Quest.PIR_Ques_Col2 = ((ListItem)cmbCol2.SelectedItem).Value.ToString();
                pir2Quest.PIR_Ques_Col_Head_Top = chkbPrint.Checked == true ? "Y" : "N";
                pir2Quest.PIR_Active = chkbActive.Checked == true ? "A" : "I";
                pir2Quest.PIR_Ques_A1Day = chkbAttn.Checked == true ? "Y" : "N";
                pir2Quest.PIR_Ques_Bold = chkbBold.Checked == true ? "Y" : "N";


                pir2Quest.Mode = Mode;

                if (_model.PIRData.InsertUpdatePirQues2(pir2Quest))
                {
                    PIR30001Control pir30001Control = _baseForm.GetBaseUserControl() as PIR30001Control;
                    if (pir30001Control != null)
                    {
                        pir30001Control.RefreshGridData(txtUniqueID.Text + txtQuesCode.Text + txtSubQuesNo.Text + txtQuesSec.Text, ((ListItem)cmbCol1.SelectedItem).Text.ToString(), ((ListItem)cmbCol2.SelectedItem).Text.ToString());
                    }

                    this.Close();
                    this.Close();
                    AlertBox.Show("Saved Successfully");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Validate_Form()
        {
            bool isValid = true;
            _errorProvider.SetError(txtQuesCode, null);
            _errorProvider.SetError(txtSubQuesNo, null);
            _errorProvider.SetError(txtQuesSec, null);
            _errorProvider.SetError(btnFundSelect, null);
            _errorProvider.SetError(txtPosition, null);
            _errorProvider.SetError(txtDesc, null);
            //_errorProvider.SetError(cmbSection, null);
            _errorProvider.SetError(cmbQuesType, null);
            _errorProvider.SetError(cmbCol1, null);
            _errorProvider.SetError(cmbCol2, null);

            if (string.IsNullOrEmpty(txtQuesCode.Text.Trim()))
            {
                _errorProvider.SetError(txtQuesCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQuesCode.Text.Trim()));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtQuesCode, null);

            if (string.IsNullOrEmpty(txtSubQuesNo.Text.Trim()))
            {
                _errorProvider.SetError(txtSubQuesNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSubQuesNo.Text.Trim()));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtSubQuesNo, null);

            if (string.IsNullOrEmpty(txtQuesSec.Text.Trim()))
            {
                _errorProvider.SetError(txtQuesSec, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQuesSec.Text.Trim()));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtQuesSec, null);

            if (string.IsNullOrEmpty(txtFundType.Text.Trim()))
            {
                _errorProvider.SetError(btnFundSelect, "Please select atleast One Fund");
                isValid = false;
            }
            else
                _errorProvider.SetError(btnFundSelect, null);

            if (string.IsNullOrEmpty(txtPosition.Text.Trim()))
            {
                _errorProvider.SetError(txtPosition, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPosition.Text.Trim()));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtPosition, null);

            if (string.IsNullOrEmpty(txtDesc.Text.Trim()))
            {
                _errorProvider.SetError(txtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDesc.Text.Trim()));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtDesc, null);

            //if (cmbSection.SelectedIndex == 0)
            //{
            //    _errorProvider.SetError(cmbSection, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSection.Text.Trim()));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(cmbSection, null);

            if (cmbQuesType.SelectedIndex == 0)
            {
                _errorProvider.SetError(cmbQuesType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblQuesType.Text.Trim()));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbQuesType, null);

            if (chkbCol1.Checked && cmbCol1.SelectedIndex == 0)
            {
                _errorProvider.SetError(cmbCol1, "Please Select an Option");
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbCol1, null);

            if (chkbCol1.Checked && cmbCol1.SelectedIndex != 0)
            {
                if (chkbCol2.Checked && cmbCol2.SelectedIndex == 0)
                {
                    _errorProvider.SetError(cmbCol2, "Please Select an Option");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbCol2, null);
            }

            //if ((chkbCol1.Checked && cmbCol1.SelectedIndex != 0) && (chkbCol2.Checked && cmbCol2.SelectedIndex != 0))
            //{
            //    if (pirQUEST2Entity.PIR_Ques_Unique_ID == txtUniqueID.Text && ((ListItem)cmbCol1.SelectedItem).Value.ToString() == pirQUEST2Entity.Col1_Desc && ((ListItem)cmbCol2.SelectedItem).Value.ToString() == pirQUEST2Entity.Col2_Desc)
            //        AlertBox.Show("The Selected Column Headers are already used before", MessageBoxIcon.Warning);
            //    isValid = false;
            //}
            //else
            //    isValid = true;

            return isValid;

        }

        private void btnFundSelect_Click(object sender, EventArgs e)
        {
            AlertCodeForm objform = new AlertCodeForm(_baseForm, _privileges, txtFundType.Text, "FUNDS", "Funds");
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

        private void fillSectionCombo()
        {
            cmbSection.Items.Clear();
            cmbSection.Items.Insert(0, new ListItem("Section - A", "A"));
            cmbSection.Items.Insert(1, new ListItem("Section - B", "B"));
            cmbSection.Items.Insert(2, new ListItem("Section - C", "C"));
            cmbSection.SelectedIndex = 0;
        }

        private void fillQuesTypeCombo()
        {
            cmbQuesType.Items.Clear();
            cmbQuesType.Items.Insert(0, new ListItem(" ", "0"));
            cmbQuesType.Items.Insert(1, new ListItem("Staff Master", "F"));
            cmbQuesType.Items.Insert(2, new ListItem("HardCoded", "H"));
            cmbQuesType.Items.Insert(3, new ListItem("Client Intake", "I"));
            cmbQuesType.Items.Insert(4, new ListItem("Child Tracking", "T"));
            cmbQuesType.SelectedIndex = 0;
        }

        private void fillColumn1Combo()
        {
            cmbCol1.Items.Clear();
            
            //DataSet ds = DatabaseLayer.SPAdminDB.FillColumHeaders_PIR_Logic_Ass_2(pirQUEST2Entity.PIR_Ques_ID.ToString().Trim(), pirQUEST2Entity.PIR_Section.ToString().Trim(), pirQUEST2Entity.PIR_Ques_Desc.ToString().Trim());
            DataSet ds = DatabaseLayer.SPAdminDB.FillColumHeaders_PIR_Logic_Ass_2(null, ((ListItem)cmbSection.SelectedItem).Value.ToString(), null);
            DataTable dt = ds.Tables[0];
            List<ListItem> listItem = new List<ListItem>();

            cmbCol1.Items.Add(new ListItem("Select One", "0"));

            foreach (DataRow drCol1 in dt.Rows)
            {
                cmbCol1.Items.Add(new ListItem(drCol1["PIRQCOLH_Description"].ToString().Trim(), drCol1["PIRQCOLH_ID"].ToString().Trim()));
            }
            cmbCol1.SelectedIndex = 0;
        }

        private void fillColumn2Combo()
        {
            cmbCol2.Items.Clear();
            DataSet ds = DatabaseLayer.SPAdminDB.FillColumHeaders_PIR_Logic_Ass_2(null, ((ListItem)cmbSection.SelectedItem).Value.ToString(), null);
            DataTable dt = ds.Tables[0];
            List<ListItem> listItem = new List<ListItem>();

            cmbCol2.Items.Add(new ListItem("Select One", "0"));

            foreach (DataRow drCol1 in dt.Rows)
            {
                cmbCol2.Items.Add(new ListItem(drCol1["PIRQCOLH_Description"].ToString().Trim(), drCol1["PIRQCOLH_ID"].ToString().Trim()));
            }
            cmbCol2.SelectedIndex = 0;
        }

        private void fillFormControls()
        {
            PIRQUEST2Entity pir2Quest = new PIRQUEST2Entity();


            txtUniqueID.Text = pirQUEST2Entity.PIR_Ques_Unique_ID.ToString().Trim();
            txtQuesCode.Text = pirQUEST2Entity.PIR_Ques_Code.ToString().Trim();
            txtSubQuesNo.Text = pirQUEST2Entity.PIR_Ques_SCode.ToString().Trim();
            txtQuesSec.Text = pirQUEST2Entity.PIR_Ques_Section.ToString().Trim();

            CommonFunctions.SetComboBoxValue(cmbSection, pirQUEST2Entity.PIR_Section.ToString());
            CommonFunctions.SetComboBoxValue(cmbQuesType, pirQUEST2Entity.PIR_Ques_Type.ToString());

            if (pirQUEST2Entity.PIR_Ques_Col1 != "0" && !string.IsNullOrEmpty(pirQUEST2Entity.PIR_Ques_Col1))
            {
                chkbCol1.Checked = true;
                cmbCol1.Enabled = true;
            }
            else
            {
                chkbCol1.Checked = false;
                cmbCol1 .Enabled = false;
            }
            CommonFunctions.SetComboBoxValue(cmbCol1, pirQUEST2Entity.PIR_Ques_Col1.ToString());

            if (pirQUEST2Entity.PIR_Ques_Col2 != "0" && !string.IsNullOrEmpty(pirQUEST2Entity.PIR_Ques_Col2))
            {
                chkbCol2.Checked = true;
                cmbCol2.Enabled = true;
            }
            else
            {
                chkbCol2.Checked = false;
                cmbCol2 .Enabled = false;
            }
            CommonFunctions.SetComboBoxValue(cmbCol2, pirQUEST2Entity.PIR_Ques_Col2.ToString());

            txtFundType.Text = pirQUEST2Entity.PIR_Fund_Type.ToString().Trim();

            txtPosition.Text = pirQUEST2Entity.PIR_Ques_Position.ToString().Trim();
            txtDesc.Text = pirQUEST2Entity.PIR_Ques_Desc.ToString().Trim();

            if (pirQUEST2Entity.PIR_Active == "A")
                chkbActive.Checked = true;
            else
                chkbActive.Checked = false;

            if (pirQUEST2Entity.PIR_Ques_A1Day == "Y")
                chkbAttn.Checked = true;
            else
                chkbAttn.Checked = false;

            if (pirQUEST2Entity.PIR_Ques_Bold == "Y")
                chkbBold.Checked = true;
            else
                chkbBold.Checked = false;

            if (pirQUEST2Entity.PIR_Ques_Col_Head_Top == "Y")
                chkbPrint.Checked = true;
            else
                chkbPrint.Checked = false;
        }

        private void cmbCol1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkbCol2.Checked && cmbCol1.SelectedIndex != 0)
            {
                cmbCol2.Enabled = true;
            }
            else
                cmbCol2.Enabled = false;
        }

        private void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbSection.Items.Count > 0) 
            {
                fillColumn1Combo();
                fillColumn2Combo();
            }
        }
    }
}
