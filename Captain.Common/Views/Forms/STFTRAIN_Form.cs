using Aspose.Cells.Drawing;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using DevExpress.PivotGrid.OLAP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class STFTRAIN_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion

        public STFTRAIN_Form(BaseForm baseForm, string mode,string stfId, PrivilegeEntity privilegeEntity, List<STAFFTRAINEntity> trainEntity)
        {
            InitializeComponent();

            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _baseForm = baseForm;
            _mode = mode;
            STFT_ID = stfId;
            _privilegeEntity = privilegeEntity;
            staffTrainEntity = trainEntity;

            if(_mode.ToUpper() == "INSERT")
                this.Text = "Staff Training Table" + " - " + "Add";
            if(_mode.ToUpper() == "UPDATE")
                this.Text = "Staff Training Table" + " - " + "Edit";

            txtCredHrs.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtHrs.Validator = TextBoxValidation.CustomDecimalValidation8dot3;

            ServAreaListBox.Height = 200;

            ServiceArea = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0220", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            //this.cmbUserServArea.Items.Add("");

            foreach (CommonEntity entity in ServiceArea)
            {
                this.ServAreaListBox.Items.Add(new Captain.Common.Utilities.ListItem(entity.Desc, entity.Code));
            }

            #region Combo Box Filling

            fillTopic();

            //fillServiceArea();

            fillLevel();

            fillLocation();

            fillFormat();

            fillCreditType();

            #endregion

            if(_mode.ToUpper()== "UPDATE")
                fillStaffTrainForm();

        }
        private void fillStaffTrainForm()
        {
            staffTrainEntity = _model.STAFFData.GetSTFTRAIN(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, STFT_ID);

            if(staffTrainEntity.Count>0)
            {
                txtName.Text = staffTrainEntity[0].Name;
                txtLocNote.Text = staffTrainEntity[0].Loc_Note;
                txtTraiNotes.Text = staffTrainEntity[0].Tr_Note;
                txtCredHrs.Text = staffTrainEntity[0].Cred_Hrs;
                txtHrs.Text = staffTrainEntity[0].Hrs;

                if (staffTrainEntity[0].Active == "Y")
                    chkbActive.Checked = true; else chkbActive.Checked = false;

                CommonFunctions.SetComboBoxValue(cmbCredType, staffTrainEntity[0].Cred_Type);
                CommonFunctions.SetComboBoxValue(cmbFormat, staffTrainEntity[0].Format);
                CommonFunctions.SetComboBoxValue(cmbLevel, staffTrainEntity[0].Level);
                CommonFunctions.SetComboBoxValue(cmbLoc, staffTrainEntity[0].Loc);

                string[] ServAreaValues = staffTrainEntity[0].Serv_Area.Split(',');

                foreach (string SerArea in ServAreaValues)
                {
                    for(int x=0; x<ServAreaListBox.Items.Count;x++)
                    {
                        if (((Captain.Common.Utilities.ListItem)ServAreaListBox.Items[x]).Value.ToString() == SerArea)
                            ServAreaListBox.SetItemChecked(x, true);
                    }
                }
                //CommonFunctions.SetComboBoxValue(cmbServArea, staffTrainEntity[0].Serv_Area);

                CommonFunctions.SetComboBoxValue(cmbTopic, staffTrainEntity[0].Topic);
            }

        }

        #region Properties

        public BaseForm _baseForm {get; set;}

        public PrivilegeEntity _privilegeEntity{get; set;}

        public string STFT_ID { get; set; }
        public string _mode{get; set;}
        public List<STAFFTRAINEntity> staffTrainEntity
        {
            get; set;
        }

        public STAFFTRAINEntity STFTRAIN_Entity { get; set; }

        #endregion

        List<CommonEntity> CredType = new List<CommonEntity>();
        private void fillCreditType()
        {
            cmbCredType.Items.Clear();
            cmbCredType.ColorMember = "FavoriteColor";

            CredType = _model.lookupDataAccess.GetLookkupFronAGYTAB("00454");

            cmbCredType.Items.Add(new ListItem("", "0"));

            foreach (CommonEntity entity in CredType)
            {
                cmbCredType.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbCredType.Items.Count > 0)
                cmbCredType.SelectedIndex = 0;
        }

        List<CommonEntity> Format = new List<CommonEntity>();
        private void fillFormat()
        {
            cmbFormat.Items.Clear();
            cmbFormat.ColorMember = "FavoriteColor";

            Format = _model.lookupDataAccess.GetLookkupFronAGYTAB("00453");

            cmbFormat.Items.Add(new ListItem("", "0"));

            foreach (CommonEntity entity in Format)
            {
                cmbFormat.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbFormat.Items.Count > 0)
                cmbFormat.SelectedIndex = 0;
        }

        List<CommonEntity> Location = new List<CommonEntity>();
        private void fillLocation()
        {
            cmbLoc.Items.Clear();
            cmbLoc.ColorMember = "FavoriteColor";

            Location = _model.lookupDataAccess.GetLookkupFronAGYTAB("00452");

            cmbLoc.Items.Add(new ListItem("", "00"));

            foreach (CommonEntity entity in Location)
            {
                cmbLoc.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbLoc.Items.Count > 0)
                cmbLoc.SelectedIndex = 0;
        }

        List<CommonEntity> Level = new List<CommonEntity>();
        private void fillLevel()
        {
            cmbLevel.Items.Clear();
            cmbLevel.ColorMember = "FavoriteColor";

            cmbLevel.Items.Add(new ListItem("", "0"));
            //Level = _model.lookupDataAccess.GetLookkupFronAGYTAB("00454");

            //foreach (CommonEntity entity in Level)
            //{
            //    cmbLevel.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            //}

            if (cmbLevel.Items.Count > 0)
                cmbLevel.SelectedIndex = 0;
        }

        /*List<CommonEntity> ServiceArea = new List<CommonEntity>();
        private void fillServiceArea()
        {
            cmbServArea.Items.Clear();
            cmbServArea.ColorMember = "FavoriteColor";

            ServiceArea = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0220", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            cmbServArea.Items.Add(new ListItem("", "00"));

            foreach (CommonEntity entity in ServiceArea)
            {
                cmbServArea.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbServArea.Items.Count > 0)
                cmbServArea.SelectedIndex = 0;
        }*/

        List<CommonEntity> Topic = new List<CommonEntity>();
        private void fillTopic()
        {
            cmbTopic.Items.Clear();
            cmbTopic.ColorMember = "FavoriteColor";

            Topic = _model.lookupDataAccess.GetLookkupFronAGYTAB("00450");

            cmbTopic.Items.Add(new ListItem("", "00"));

            foreach (CommonEntity entity in Topic)
            {
                cmbTopic.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbTopic.Items.Count > 0)
                cmbTopic.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                STAFFTRAINEntity entity = new STAFFTRAINEntity();

                entity.Agency = _baseForm.BaseAgency;
                entity.Dept = _baseForm.BaseDept;
                entity.Prog = _baseForm.BaseProg;

                if(!string.IsNullOrEmpty(_baseForm.BaseYear))
                    entity.Year = _baseForm.BaseYear;

                if (_mode.ToUpper() == "UPDATE")
                    entity.ID = STFT_ID;

                entity.Name = txtName.Text == "" ? "" : txtName.Text.Trim();
                entity.Active = chkbActive.Checked ? "Y" : "N";
                entity.Topic = ((Captain.Common.Utilities.ListItem)cmbTopic.SelectedItem).Value.ToString().Trim();

                entity.Serv_Area = ServAreaValues.Trim().TrimEnd(',');//((Captain.Common.Utilities.ListItem)cmbServArea.SelectedItem).Value.ToString().Trim();

                if (cmbLevel.Items.Count > 0)
                    entity.Level = ((Captain.Common.Utilities.ListItem)cmbLevel.SelectedItem).Value.ToString().Trim();
                entity.Loc = ((Captain.Common.Utilities.ListItem)cmbLoc.SelectedItem).Value.ToString().Trim();
                entity.Loc_Note = txtLocNote.Text == "" ? "" : txtLocNote.Text.Trim();
                entity.Format = ((Captain.Common.Utilities.ListItem)cmbFormat.SelectedItem).Value.ToString().Trim();
                entity.Cred_Type = ((Captain.Common.Utilities.ListItem)cmbCredType.SelectedItem).Value.ToString().Trim();
                entity.Cred_Hrs = txtCredHrs.Text == "" ? "" : txtCredHrs.Text.Trim();
                entity.Hrs = txtHrs.Text == "" ? "" : Convert.ToDecimal(txtHrs.Text.Trim()).ToString();
                entity.Tr_Note = txtTraiNotes.Text == "" ? "" : txtTraiNotes.Text.Trim();

                entity.ADD_Operator = _baseForm.UserID;
                entity.LSTC_Operator = _baseForm.UserID;

                if (_model.STAFFData.InsertUpdateDelStaffTrain(entity, _mode.ToUpper()))
                {
                    STFTRAIN_Control stfTrainControl = _baseForm.GetBaseUserControl() as STFTRAIN_Control;

                    if (stfTrainControl != null)
                    {
                        if (_mode == "INSERT")
                        {
                            AlertBox.Show("Saved Successfully");
                            stfTrainControl.RefreshGrid(entity.ID);
                        }
                        else if (_mode == "UPDATE")
                        {
                            AlertBox.Show("Updated Successfully");
                            stfTrainControl.RefreshGrid();
                        }

                        
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(txtName, null);

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                _errorProvider.SetError(txtName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblName.Text.Replace(Consts.Common.Colon, string.Empty)));

                isValid = false;
            }
            else
                _errorProvider.SetError(txtName, null);

            return isValid;
        }

        private void txtHrs_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtHrs_Leave(object sender, EventArgs e)
        {
            if (txtHrs.Text.StartsWith("."))
            {
                txtHrs.Text = "0" + txtHrs.Text;
            }
            if (txtHrs.Text.EndsWith("."))
            {
                txtHrs.Text = txtHrs.Text + "0";
            }
            if (txtHrs.Text == ".")
            {
                txtHrs.Text = "0";
            }
        }

        private void txtCredHrs_Leave(object sender, EventArgs e)
        {
            if (txtCredHrs.Text.StartsWith("."))
            {
                txtCredHrs.Text = "0" + txtCredHrs.Text;
            }
            if (txtCredHrs.Text.EndsWith("."))
            {
                txtCredHrs.Text = txtCredHrs.Text + "0";
            }
            if (txtCredHrs.Text == ".")
            {
                txtCredHrs.Text = "0";
            }
        }

        string ServAreaValues = "";
        private void cmbServAreaListBox_AfterItemCheck(object sender, Wisej.Web.ItemCheckEventArgs e)
        {
            string ServAreaText = "";
            ServAreaValues = "";
            foreach (Captain.Common.Utilities.ListItem checkedItem in ServAreaListBox.CheckedItems)
            {
                ServAreaValues += $"{checkedItem.Value},";
                ServAreaText += $"{checkedItem.Text}, ";
            }
            this.cmbUserServArea.Text = ServAreaText.Trim().TrimEnd(',');
        }

        List<CommonEntity> ServiceArea = new List<CommonEntity>();
        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);

        //    ServAreaListBox.Height = 200;

        //    ServiceArea = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "S0220", _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

        //    //this.cmbUserServArea.Items.Add("");

        //    foreach (CommonEntity entity in ServiceArea)
        //    {
        //        this.ServAreaListBox.Items.Add(new Captain.Common.Utilities.ListItem(entity.Desc,entity.Code));
        //    }
        //    //foreach (CommonEntity entity in ServiceArea)
        //    //{
        //    //    var serArea = new List<dynamic>()
        //    //    {
        //    //        new { name = entity.Desc, code = entity.Code },
        //    //    };
        //    //}
        //}
    }
}
