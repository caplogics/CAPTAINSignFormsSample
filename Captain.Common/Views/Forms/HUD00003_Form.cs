using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD00003_Form : Form
    {
        #region Private Variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public int selRowIndex = 0;

        #endregion
        public HUD00003_Form(BaseForm baseform, PrivilegeEntity privilegeEntity, string mstSeq, string mode, int currentRowIndex)
        {
            InitializeComponent();
            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 0;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.Icon = null;
            _Mode = mode;
            _mstSeq = mstSeq;
            selRowIndex = currentRowIndex;

            fill_Status_Combo();

            txtMSTSeq.Text = _mstSeq;

            hudmstEntity = _model.HUDCNTLData.GetHUDForm(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, "");

            

            if (_Mode == "INSERT")
            {
                dtpDate.Value = DateTime.Now;
                cmbStatus.SelectedIndex = 0;
            }
            else
            {
                if (hudmstEntity.Count > 0)
                    fill_Form();
            }
            
        }

        private void fill_Form()
        {
            HUDMSTENTITY entity = hudmstEntity.Find(x => x.Seq.ToString() == txtMSTSeq.Text);

            dtpDate.Value = entity.Date == "" ? DateTime.Now : Convert.ToDateTime(entity.Date);
            SetComboBoxValue(cmbStatus, entity.Status);
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
        #region Properties

        public BaseForm _baseForm
        {
            get;
            set;
        }
        public PrivilegeEntity _privilegeEntity
        {
            get; set;
        }
        public string _Mode
        {
            get; set;
        }
        public List<HUDMSTENTITY> hudmstEntity
        {
            get; set;
        }
        public string _mstSeq
        {
            get; set;
        }

        #endregion

        private bool ValidateForm()
        {
            bool IsValid = true;

            _errorProvider.SetError(dtpDate, null);

            #region Validate Date

            if (string.IsNullOrEmpty(dtpDate.Text.Trim()))
            {
                _errorProvider.SetError(dtpDate, lblDate.Text.Trim() + " is required.");
                IsValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpDate, null);

                if (dtpDate.Value > DateTime.Now)
                {
                    _errorProvider.SetError(dtpDate, "Future Date is not allowed.");
                    IsValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpDate, null);
                }
            }

            #endregion

            return IsValid;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpDate, null);
            HUD00003_ClientHUDForms HUD0003_Control = _baseForm.GetBaseUserControl() as HUD00003_ClientHUDForms;
            if (ValidateForm())
            {
                HUDMSTENTITY saveEntity = new HUDMSTENTITY();

                saveEntity.Agency = _baseForm.BaseAgency;
                saveEntity.Dept = _baseForm.BaseDept;
                saveEntity.Prog = _baseForm.BaseProg;
                saveEntity.Year = _baseForm.BaseYear;

                saveEntity.AppNo = _baseForm.BaseApplicationNo;

                string Seq = "1";
                if (_Mode == "INSERT")
                    Seq = "1";
                else if (_Mode == "UPDATE")
                    Seq = txtMSTSeq.Text.Trim();

                saveEntity.Seq = Seq;

                saveEntity.Date = dtpDate.Text;
                saveEntity.Status = ((Captain.Common.Utilities.ListItem)cmbStatus.SelectedItem).Value.ToString().Trim();

                saveEntity.Add_Operator = _baseForm.UserID;
                saveEntity.Lstc_Operator = _baseForm.UserID;

                string strOutMstseq = saveEntity.Seq;

                if (_model.HUDCNTLData.InsertUpdateHUDForm(saveEntity, _Mode, out strOutMstseq))
                {
                    if (_Mode == "INSERT")
                        AlertBox.Show("HUD Form Inserted Successfully");
                    if (_Mode == "UPDATE")
                        AlertBox.Show("HUD Form Updated Successfully");

                    DialogResult = DialogResult.OK;

                    this.Close();

                    if (HUD0003_Control != null)
                    {
                        HUD0003_Control.RefreshGrid(0, selRowIndex, false);
                    }
                }


            }
        }

        List<CommonEntity> StatusList = new List<CommonEntity>();
        private void fill_Status_Combo()
        {
            cmbStatus.Items.Clear();

            StatusList = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0047", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);
            StatusList = StatusList.OrderByDescending(x => x.Code).ToList();
            //cmbStatus.Items.Add(new ListItem("Select One", "0"));

            foreach (CommonEntity entity in StatusList)
            {
                cmbStatus.Items.Add(new ListItem(entity.Desc, entity.Code));
            }

            //cmbStatus.Items.Add(new ListItem("Opened", "O"));
            //cmbStatus.Items.Add((new ListItem("Closed", "C")));

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            HUD00003_ClientHUDForms HUD0003_Control = _baseForm.GetBaseUserControl() as HUD00003_ClientHUDForms;
            if (HUD0003_Control != null)
            {
                HUD0003_Control.RefreshGrid(0, selRowIndex, true);
            }
        }

        private void HUD00003_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
            HUD00003_ClientHUDForms HUD0003_Control = _baseForm.GetBaseUserControl() as HUD00003_ClientHUDForms;
            if (HUD0003_Control != null)
            {
                HUD0003_Control.RefreshGrid(0, selRowIndex, true);
            }
        }
    }
}
