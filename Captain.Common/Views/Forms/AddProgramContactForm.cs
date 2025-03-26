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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AddProgramContactForm : Form
    {

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public AddProgramContactForm(BaseForm baseForm, string mode, DepContactEntity contactEntity)
        {
            InitializeComponent();
            _model = new CaptainModel();
             Mode = mode;
            BaseForm = baseForm;
            ContactEntity = contactEntity;

            this.Text =  " Program Contacts - "+ Mode ;
            
            if (contactEntity != null)
            {
                Agency = contactEntity.Agency;
                Dept = contactEntity.Dept;
                Program = contactEntity.Program;
                SEQ = contactEntity.SEQ;
            }
            if (Mode.Equals("Edit"))
            {
                fillContactsForm();
            }
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
        }

        private void setVisible(bool flag)
        {
            btnOk.Location = new System.Drawing.Point(318, 124);
            btnCancel.Location = new System.Drawing.Point(392, 124);
            panel2.Size = new System.Drawing.Size(533, 160);
            this.Size = new System.Drawing.Size(538, 224);
         }
  
        private void fillContactsForm()
        {
            //List<CaseDepContactEntity> contactEntity = _model.HierarchyAndPrograms.GetCASEDEPContacts(ContactEntity);
            if (ContactEntity != null)
            {
                txtCode.Text = ContactEntity.StaffCode.Trim();
                txtFirstName.Text = ContactEntity.FirstName.Trim();
                txtLastName.Text = ContactEntity.LastName.Trim();
                msktxtPhone1.Text = ContactEntity.Phone1.Trim();
                msktxtPhone2.Text = ContactEntity.Phone2.Trim();
                msktxtFax.Text = ContactEntity.Fax.Trim();
                txtEmail.Text = ContactEntity.Email.Trim();
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

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public DepContactEntity ContactEntity { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Program { get; set; }

        public string SEQ { get; set; }

        private bool isCodeExists(string code)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                //HierarchyEntity hierarchyEntity = _model.lookupDataAccess.GetCaseHierarchy(HierarchyType + "CheckDup", Agency, Dept, Program);
                //if (hierarchyEntity != null)
                //{
                //    isExists = true;
                //}
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
                 _errorProvider.SetError(txtCode, null);
            }

            if (String.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                _errorProvider.SetError(txtFirstName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFirstName, null);
            }

            if (!String.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                if(!CommonFunctions.IsValidEmail(txtEmail.Text))
                {
                    _errorProvider.SetError(txtEmail, "Please Enter Valid Email ID");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtEmail, null);
                }
            }

            if (String.IsNullOrEmpty(txtLastName.Text.Trim()))
            {
                _errorProvider.SetError(txtLastName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblShortName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtLastName, null);
            }

            return (isValid);
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            try
            {

                if (ValidateForm())
                {
                    DepContactEntity contactEntity = new DepContactEntity();
                    contactEntity.Agency = Agency;
                    contactEntity.Dept = Dept;
                    contactEntity.Program = Program;
                    contactEntity.SEQ = SEQ;
                    contactEntity.StaffCode = txtCode.Text.Trim();
                    contactEntity.FirstName = txtFirstName.Text.Trim();
                    contactEntity.LastName = txtLastName.Text.Trim();
                    contactEntity.Phone1 = msktxtPhone1.Text.Trim();
                    contactEntity.Phone2 = msktxtPhone2.Text.Trim();
                    contactEntity.Fax = msktxtFax.Text.Trim();
                    contactEntity.Email = txtEmail.Text.Trim();
                    if(Mode.Equals("Edit")) contactEntity.Mode = "U";
                    contactEntity.DepAddOperator = BaseForm.UserID;
                    contactEntity.DepLstcOperator = BaseForm.UserID;

                    //if (_model.HierarchyAndPrograms.InsertCaseDepContact(contactEntity))
                    //{
                        //AddProgramContactForm programControl = BaseForm.GetBaseUserControl() as AddProgramContactForm;
                        //if (programControl != null)
                        //{
                        //    programControl.RefreshContactGrid();
                        //}
                    ContactEntity = contactEntity;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    //}
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

    }
}