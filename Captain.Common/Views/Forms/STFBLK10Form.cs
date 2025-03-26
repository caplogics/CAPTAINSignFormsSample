#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class STFBLK10Form : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public STFBLK10Form(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity, List<String> employeecodeList, string strCategory,STAFFPostEntity staffPost)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;            
            //lblHeader.Text = privilegeEntity.PrivilegeName;
            propstaffPostEntity = staffPost;
            propEmployeecode = employeecodeList;
            propCategory = strCategory;
            this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
            _model = new CaptainModel();
            propCategory = strCategory;
            propCode = strCategory;
            DropdownFills();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtCeucreduts.Validator = TextBoxValidation.FloatValidator;
            txtClockHours.Validator = TextBoxValidation.FloatValidator;
            txtCollegeCredites.Validator = TextBoxValidation.FloatValidator;
            txtCost.Validator = TextBoxValidation.FloatValidator;
            if (Mode.Equals("Add"))
            {
                if (propstaffPostEntity.CourseTitle!=string.Empty)
                    btnCopy.Visible = true;
            }
            if (Mode.Equals("Edit"))
            {
                btnCopy.Visible = false;
                // txtEmployNumber.Enabled = false;
                this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                 FillAllControls();
            }
            else if (Mode.Equals("View"))
            {

                //  panel3.Enabled = false;
                btnCopy.Visible = false;
                btnClear.Visible = false;
                btnSave.Visible = false;
                btnCancel.Text = "&Close";
                this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.View;
                //   FillAllControls();
            }
        }

        private void DropdownFills()
        {

            cmbProvider.Items.Clear();
            List<CommonEntity> PrimaryLanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00552",  BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetResident();

            //List<CommonEntity> PrimaryLanguage = _model.lookupDataAccess.GetAgyTabRecordsByCode("00552");
            // List<CommonEntity> Languages = filterByHIE(PrimaryLanguage);
            cmbProvider.Items.Insert(0, new ListItem(" ", "0"));
            cmbProvider.ColorMember = "FavoriteColor";
            cmbProvider.SelectedIndex = 0;
            PrimaryLanguage = PrimaryLanguage.OrderByDescending(u => u.Active).ToList();
            foreach (CommonEntity primarylanguage in PrimaryLanguage)
            {
                ListItem li = new ListItem(primarylanguage.Desc, primarylanguage.Code, primarylanguage.Active, primarylanguage.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbProvider.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && primarylanguage.Default.Equals("Y")) cmbProvider.SelectedItem = li;
            }
        }

        private void FillAllControls()
        {
            if (propstaffPostEntity != null)
            {
                txtCoursetitle.Text = propstaffPostEntity.CourseTitle.ToString();
                txtCeucreduts.Text = propstaffPostEntity.CeuCredits;
                txtClockHours.Text = propstaffPostEntity.ClockHours;
                txtCollegeCredites.Text =propstaffPostEntity.CollegeCredits ;
                CommonFunctions.SetComboBoxValue(cmbProvider,propstaffPostEntity.Provider);
                if (!string.IsNullOrEmpty(propstaffPostEntity.DateCompleted))
                {
                    dtCompleted.Value = Convert.ToDateTime(propstaffPostEntity.DateCompleted);
                    dtCompleted.Checked = true;
                }
                txtLocation.Text = propstaffPostEntity.Location;
                txtSponsor.Text = propstaffPostEntity.Sponsor;
                txtPresenter.Text = propstaffPostEntity.Presenter;
                txtCost.Text = propstaffPostEntity.TotalCost;

            }
        }
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        public string SelectStaffCodeId { get; set; }

        public bool IsSaveValid { get; set; }
        public string propCategory { get; set; }
        public string propCode { get; set; }

        public List<String> propEmployeecode { get; set; }
        public STAFFPostEntity propstaffPostEntity { get; set; }
        private void txtHeader_Leave(object sender, EventArgs e)
        {

        }

        private void OnHelpClick(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "STFBLK10");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int rowIndex = 0;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidate()) return;
            if(Mode.Equals("Add"))
            {
                foreach (String item in propEmployeecode)
                {

                    STAFFPostEntity staffPostEntit = new STAFFPostEntity();                   

                    staffPostEntit.Agency = BaseForm.BaseAgency;
                    staffPostEntit.Dept = BaseForm.BaseDept;
                    staffPostEntit.Prog = BaseForm.BaseProg;
                    staffPostEntit.Year = BaseForm.BaseYear;
                    staffPostEntit.Staff_Code = item.ToString();
                    staffPostEntit.Category = propCategory;
                    staffPostEntit.CeuCredits = txtCeucreduts.Text;
                    staffPostEntit.ClockHours = txtClockHours.Text;
                    staffPostEntit.CollegeCredits = txtCollegeCredites.Text;
                    staffPostEntit.CourseTitle = txtCoursetitle.Text;
                    if (!((ListItem)cmbProvider.SelectedItem).Value.ToString().Equals("0"))
                    {
                        staffPostEntit.Provider = ((ListItem)cmbProvider.SelectedItem).Value.ToString();
                    }
                    if (dtCompleted.Checked == true)
                    {
                        staffPostEntit.DateCompleted = dtCompleted.Value.ToString();
                    }
                    staffPostEntit.Location = txtLocation.Text ;
                    staffPostEntit.Sponsor = txtSponsor.Text ;
                    staffPostEntit.Presenter = txtPresenter.Text ;
                    staffPostEntit.TotalCost = txtCost.Text ;

                    staffPostEntit.ADD_Operator = BaseForm.UserID;
                    staffPostEntit.LSTC_Operator = BaseForm.UserID;
                    staffPostEntit.Mode = Mode;

                    if (_model.STAFFData.InsertUpdateDelStaffPost(staffPostEntit))
                    {
                        STFBLK10Control staffBulkControl = BaseForm.GetBaseUserControl() as STFBLK10Control;
                        if (staffBulkControl != null)
                        {
                            staffBulkControl.RefreshGrid();
                        }
                        this.Close();
                        AlertBox.Show("Saved Successfully");
                    }
                    //rowIndex = int.Parse(staffPostEntit.Seq);
                }
            }
            else
            {
                STAFFPostEntity staffPostEntit = new STAFFPostEntity();

                staffPostEntit.Agency = BaseForm.BaseAgency;
                staffPostEntit.Dept = BaseForm.BaseDept;
                staffPostEntit.Prog = BaseForm.BaseProg;
                staffPostEntit.Staff_Code = propstaffPostEntity.Staff_Code;
                staffPostEntit.Seq = propstaffPostEntity.Seq;
                staffPostEntit.Category = propCategory;
                staffPostEntit.CeuCredits = txtCeucreduts.Text;
                staffPostEntit.ClockHours = txtClockHours.Text;
                staffPostEntit.CollegeCredits = txtCollegeCredites.Text;
                staffPostEntit.CourseTitle = txtCoursetitle.Text;
                if (!((ListItem)cmbProvider.SelectedItem).Value.ToString().Equals("0"))
                {
                    staffPostEntit.Provider = ((ListItem)cmbProvider.SelectedItem).Value.ToString();
                }
                if (dtCompleted.Checked == true)
                {
                    staffPostEntit.DateCompleted = dtCompleted.Value.ToString();
                }
                staffPostEntit.ADD_Operator = BaseForm.UserID;
                staffPostEntit.LSTC_Operator = BaseForm.UserID;
                staffPostEntit.Mode = Mode;
                staffPostEntit.Location = txtLocation.Text;
                staffPostEntit.Sponsor = txtSponsor.Text;
                staffPostEntit.Presenter = txtPresenter.Text;
                staffPostEntit.TotalCost = txtCost.Text;

                if (_model.STAFFData.InsertUpdateDelStaffPost(staffPostEntit))
                {
                    STFBLK10Control staffBulkControl = BaseForm.GetBaseUserControl() as STFBLK10Control;
                    if (staffBulkControl != null)
                    {
                        staffBulkControl.RefreshStaffPostGrid();
                    }
                    this.Close();
                    AlertBox.Show("Updated Successfully");
                }
                rowIndex = int.Parse(staffPostEntit.Seq);
            }
        }

        private bool isValidate()
        {
            bool isValid = true;
            _errorProvider.SetError(dtCompleted, null);
            _errorProvider.SetError(txtCoursetitle, null);
            _errorProvider.SetError(txtLocation, null);
            _errorProvider.SetError(cmbProvider, null);
            _errorProvider.SetError(txtSponsor, null);
            _errorProvider.SetError(txtPresenter, null);

            if (string.IsNullOrEmpty(txtCoursetitle.Text.Trim()))
            {
                _errorProvider.SetError(txtCoursetitle, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCourseTitle.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtCoursetitle, null);
            }

            if (dtCompleted.Checked == false)
            {
                _errorProvider.SetError(dtCompleted, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDateComple.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtCompleted, null);
            }

            if (dtCompleted.Checked == true)
            {
                if (string.IsNullOrWhiteSpace(dtCompleted.Text))
                {
                    _errorProvider.SetError(dtCompleted, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDateComple.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtCompleted, null);
                }
            }

            if ((((ListItem)cmbProvider.SelectedItem).Value.ToString().Equals("0")))
            {
                _errorProvider.SetError(cmbProvider, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblProvider.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbProvider, null);
            }

            if (string.IsNullOrEmpty(txtLocation.Text.Trim()))
            {
                _errorProvider.SetError(txtLocation, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLocation.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtLocation, null);
            }

            if (string.IsNullOrEmpty(txtSponsor.Text.Trim()))
            {
                _errorProvider.SetError(txtSponsor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSponsor.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtSponsor, null);
            }

            if (string.IsNullOrEmpty(txtPresenter.Text.Trim()))
            {
                _errorProvider.SetError(txtPresenter, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPresenter.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtPresenter, null);
            }


            return isValid;
        }

        private void txtCollegeCredites_Leave(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtCollegeCredites.Text))
            {
                if (Convert.ToDouble(txtCollegeCredites.Text) >= 99)
                {
                    txtCollegeCredites.Text = "99.00";
                }
            }
        }

        private void txtCeucreduts_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCeucreduts.Text))
            {
                if (Convert.ToDouble(txtCeucreduts.Text) >= 99)
                {
                    txtCeucreduts.Text = "99.00";
                }
            }
        }

        private void txtClockHours_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtClockHours.Text))
            {
                if (Convert.ToDouble(txtClockHours.Text) >= 99)
                {
                    txtClockHours.Text = "99.00";
                }
            }
        }

        private void dtCompleted_ValueChanged(object sender, EventArgs e)
        {
            if (dtCompleted.Checked == true)
            {
                if (dtCompleted.Value.Year < 1900)
                {
                    dtCompleted.Value = DateTime.Now;
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            FillAllControls();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCeucreduts.Text = string.Empty;
            txtClockHours.Text = string.Empty;
            txtCollegeCredites.Text = string.Empty;
            txtCoursetitle.Text = string.Empty;
            txtLocation.Text = string.Empty;
            txtSponsor.Text = string.Empty;
            txtPresenter.Text = string.Empty;
            txtCost.Text = string.Empty;
            dtCompleted.Checked = false;
            cmbProvider.SelectedIndex = 0;
        }

        private void STFBLK10Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 1, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }
    }
}