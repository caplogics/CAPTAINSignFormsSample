#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Wisej.Web;
using NPOI.SS.Formula.Functions;
using DevExpress.ExpressApp.SystemModule;
using Captain.Common.Interfaces;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class STFMST10CourseDetails : Form
    {
        private CaptainModel _model = null;
        int currentRowIndex = 0;
        private ErrorProvider _errorProvider = null;
        public STFMST10CourseDetails(BaseForm baseForm, string mode, string strStaffCode, PrivilegeEntity privilegeEntity, List<STAFFPostEntity> staffPostEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Mode = mode;
            SelectStaffCodeId = strStaffCode;
            _model = new CaptainModel();
            this.Text = /*privilegeEntity.Program + " - */"Course Details" ;

            this.Size = new Size(this.Width, this.Height - pnlgvCourses.Height);

            PropCategory = _model.lookupDataAccess.GetCategory();
            propstaffPostEntity = staffPostEntity;
            propProviders = _model.lookupDataAccess.GetAgyTabRecordsByCode("00552");
            //FillGridData();

            txtCredHrs.Validator = TextBoxValidation.CustomDecimalValidation8dot3;
            txtHrs.Validator = TextBoxValidation.CustomDecimalValidation8dot3;

            #region Combo Box Filling

            fillTopic();

            //fillServiceArea();

            ServAreaListBox.Height = 200;

            ServiceArea = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0220", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);

            //this.cmbUserServArea.Items.Add("");

            foreach (CommonEntity entity in ServiceArea)
            {
                this.ServAreaListBox.Items.Add(new Captain.Common.Utilities.ListItem(entity.Desc, entity.Code));
            }


            fillFormat();

            fillLocation();

            fillCreditType();

            fillLevel();

            #endregion

            if (BaseForm.BaseAgyTabsEntity.Count > 0)
            {
                Topics = BaseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "00450");
                ServArea = BaseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "S0220");
                Locations = BaseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "00452");
                Formats = BaseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "00453");
                CREDType = BaseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "00454");
            }

            fillTrainGrid();
            dgvStaffTrain.Enabled = true;
            pnlParams.Enabled = false;
            btnSave.Visible = btnCancel.Visible = false;
        }

        List<CommonEntity> Topics = new List<CommonEntity>();
        List<CommonEntity> ServArea = new List<CommonEntity>();
        List<CommonEntity> Locations = new List<CommonEntity>();
        List<CommonEntity> Formats = new List<CommonEntity>();
        List<CommonEntity> CREDType = new List<CommonEntity>();


        List<CommonEntity> Topic = new List<CommonEntity>();
        private void fillTopic()
        {
            cmbTopic.Items.Clear();
            cmbTopic.ColorMember = "FavoriteColor";

            Topic = _model.lookupDataAccess.GetLookkupFronAGYTAB("00450");

            cmbTopic.Items.Add(new ListItem("", "0"));

            foreach (CommonEntity entity in Topic)
            {
                cmbTopic.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }

            if (cmbTopic.Items.Count > 0)
                cmbTopic.SelectedIndex = 0;
        }

        List<CommonEntity> ServiceArea = new List<CommonEntity>();
        private void fillServiceArea()
        {
            cmbServArea.Items.Clear();
            cmbServArea.ColorMember = "FavoriteColor";
            ServiceArea = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0220", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            cmbServArea.Items.Add(new ListItem("", "00"));
            foreach (CommonEntity entity in ServiceArea)
            {
                cmbServArea.Items.Add(new ListItem(entity.Desc, entity.Code, entity.Active, entity.Active.Equals("Y") ? Color.Black : Color.Red));
            }
            if (cmbServArea.Items.Count > 0)
                cmbServArea.SelectedIndex = 0;
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


        string name = string.Empty;
        private void fillTrainGrid()
        {
            this.dgvStaffTrain.SelectionChanged -= new System.EventHandler(this.dgvStaffTrain_SelectionChanged);
            dgvStaffTrain.Rows.Clear();

            staffTrainEntity = _model.STAFFData.GetSTFTRAIN(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, "");
            staffBulkPostEntity = _model.STAFFData.GetStaffBulkPost(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, SelectStaffCodeId);

            if (staffBulkPostEntity.Count > 0)
            {
                string Topic = string.Empty, ServAre = string.Empty, Format = string.Empty;

                foreach (STAFFBULKPOSTEntity entity in staffBulkPostEntity)
                {
                    name = staffTrainEntity.Find(u => u.ID.Trim().Equals(entity.Train_ID.Trim())).Name.Trim();

                    if (Topics.Count > 0 && entity.Topic != "0")
                        Topic = Topics.Find(u => u.Code == entity.Topic).Desc.Trim();
                    //if (ServArea.Count > 0 && entity.Area != "00")
                    //    ServAre = ServArea.Find(u => u.Code == entity.Area).Desc.Trim();
                    if (Formats.Count > 0 && entity.Format != "0")
                        Format = Formats.Find(u => u.Code == entity.Format).Desc.Trim();

                    string[] ServAreaValues = entity.Area.Split(',');
                    ServAre = "";
                    foreach (string SerArea in ServAreaValues)
                    {
                        CommonEntity serEntrity = ServArea.Find(u => u.Code == SerArea);
                        if (serEntrity != null)
                            ServAre += serEntrity.Desc.Trim() + ", ";

                    }
                    ServAre = ServAre.Trim().TrimEnd(',');

                    dgvStaffTrain.Rows.Add(name, entity.Year, entity.Comp_Date == "" ? "" : Convert.ToDateTime(entity.Comp_Date).ToString("MM/dd/yyyy"), Topic, ServAre, Format, entity.Train_ID, entity.Seq, entity.Staff_Code);
                }
            }
            if (dgvStaffTrain.Rows.Count > 0)
            {
                dgvStaffTrain.Rows[currentRowIndex].Selected = true;
                dgvStaffTrain.CurrentCell = dgvStaffTrain.Rows[currentRowIndex].Cells[0];
            }
            this.dgvStaffTrain.SelectionChanged += new System.EventHandler(this.dgvStaffTrain_SelectionChanged);
            dgvStaffTrain_SelectionChanged(dgvStaffTrain, new EventArgs());
        }

        #region Properties

        public BaseForm BaseForm
        {
            get; set;
        }

        public PrivilegeEntity Privileges
        {
            get; set;
        }

        public List<CommonEntity> propProviders
        {
            get; set;
        }

        public List<CommonEntity> PropCategory
        {
            get; set;
        }

        public List<STAFFPostEntity> propstaffPostEntity
        {
            get; set;
        }

        public string Mode
        {
            get; set;
        }
        public string SelectStaffCodeId
        {
            get; set;
        }

        public List<STAFFTRAINEntity> staffTrainEntity
        {
            get; set;
        }
        public List<STAFFBULKPOSTEntity> staffBulkPostEntity
        {
            get; set;
        }

        #endregion

        private void dgvStaffTrain_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStaffTrain.Rows.Count > 0)
            {
                FillControls();
            }
            else
            {
                Clear_Controls();
            }
        }
        private void Clear_Controls()
        {
            cmbFormat.SelectedIndex = 0;
            cmbCredType.SelectedIndex = 0;
            if (cmbLevel.Items.Count > 0)
                cmbLevel.SelectedIndex = 0;
            cmbLoc.SelectedIndex = 0;
            ServAreaListBox.ClearChecked();
            //cmbServArea.SelectedIndex = 0;
            cmbTopic.SelectedIndex = 0;

            chkbActive.Checked = false;

            txtLocNote.Text = string.Empty;
            txtCredHrs.Text = string.Empty;
            txtHrs.Text = string.Empty;
            txtTraiNotes.Text = string.Empty;

            dtpCompDte.Value = DateTime.Now;
        }
        private void FillControls()
        {
            staffBulkPostEntity = _model.STAFFData.GetStaffBulkPost(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, "");

            List<STAFFBULKPOSTEntity> courseEntity = staffBulkPostEntity.FindAll(x => x.Train_ID == dgvStaffTrain.SelectedRows[0].Cells["Column6"].Value.ToString() && x.Staff_Code == dgvStaffTrain.SelectedRows[0].Cells["Column8"].Value.ToString() && x.Seq == dgvStaffTrain.SelectedRows[0].Cells["Column7"].Value.ToString());

            if (courseEntity.Count > 0)
            {
                CommonFunctions.SetComboBoxValue(cmbTopic, courseEntity[0].Topic);

                string[] ServAreaValues = courseEntity[0].Area.Split(',');
                ServAreaListBox.ClearChecked();
                foreach (string SerArea in ServAreaValues)
                {
                    for (int x = 0; x < ServAreaListBox.Items.Count; x++)
                    {
                        if (((Captain.Common.Utilities.ListItem)ServAreaListBox.Items[x]).Value.ToString() == SerArea)
                            ServAreaListBox.SetItemChecked(x, true);
                    }
                }

                //CommonFunctions.SetComboBoxValue(cmbServArea, courseEntity[0].Area);
                CommonFunctions.SetComboBoxValue(cmbLevel, courseEntity[0].Level);
                CommonFunctions.SetComboBoxValue(cmbFormat, courseEntity[0].Format);
                CommonFunctions.SetComboBoxValue(cmbLoc, courseEntity[0].Loc);
                CommonFunctions.SetComboBoxValue(cmbCredType, courseEntity[0].Cred_type);

                if (courseEntity[0].Active == "Y")
                    chkbActive.Checked = true;
                else
                    chkbActive.Checked = false;

                txtLocNote.Text = courseEntity[0].Loc_Note;
                txtCredHrs.Text = courseEntity[0].Cred_Hrs;
                txtHrs.Text = courseEntity[0].Hrs;
                txtTraiNotes.Text = courseEntity[0].Tr_notes;

                if (!string.IsNullOrEmpty(dgvStaffTrain.CurrentRow.Cells["Column2"].Value.ToString()))
                {
                    dtpCompDte.Value = Convert.ToDateTime(dgvStaffTrain.CurrentRow.Cells["Column2"].Value.ToString());
                }
                else
                {
                    dtpCompDte.Value = DateTime.Now;
                }
            }
        }

        private void FillGridData()
        {

            string strProviderDesc = string.Empty;
            string strCategoryDesc = string.Empty;
            foreach (STAFFPostEntity item in propstaffPostEntity)
            {
                strProviderDesc = string.Empty;
                strCategoryDesc = string.Empty;
                if (propProviders.Count > 0)
                {
                    strProviderDesc = (propProviders.Find(u => u.Code.ToString().Trim().ToUpper().Equals(item.Provider.ToUpper().Trim())).Desc.ToString());
                }
                if (PropCategory.Count > 0)
                {
                    strCategoryDesc = (PropCategory.Find(u => u.Code.ToString().Trim().ToUpper().Equals(item.Category.ToUpper().Trim())).Desc.ToString());
                }
                int index = gvCourses.Rows.Add(strCategoryDesc, item.CourseTitle, strProviderDesc, LookupDataAccess.Getdate(item.DateCompleted), item.CollegeCredits, item.CeuCredits, item.ClockHours, item.Staff_Code, item.Seq);
                gvCourses.Rows[index].Tag = item;
                CommonFunctions.setTooltip(index, item.ADD_Operator, item.Date_ADD, item.LSTC_Operator, item.Date_LSTC, gvCourses);

            }
            gvCourses.Rows[0].Selected = true;
        }

        int currentSeq = 0;
        private void btnSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpCompDte, null);

            if (Validate_Complete_Date())
            {
                _errorProvider.SetError(dtpCompDte, null);

                STAFFBULKPOSTEntity savePostEntity = new STAFFBULKPOSTEntity();

                savePostEntity.Agency = BaseForm.BaseAgency;
                savePostEntity.Dept = BaseForm.BaseDept;
                savePostEntity.Prog = BaseForm.BaseProg;

                if (BaseForm.BaseYear != "")
                    savePostEntity.Year = BaseForm.BaseYear;

                savePostEntity.Staff_Code = SelectStaffCodeId;

                if (dgvStaffTrain.CurrentRow.Cells["Column7"].Value.ToString() != "")
                    currentSeq = Convert.ToInt32(dgvStaffTrain.CurrentRow.Cells["Column7"].Value);

                savePostEntity.Seq = currentSeq.ToString();

                savePostEntity.Train_ID = dgvStaffTrain.SelectedRows[0].Cells["Column6"].Value.ToString();

                //Saving Training Data in Bulk Post Table
                savePostEntity.Active = chkbActive.Checked ? "Y" : "N";
                savePostEntity.Topic = ((Captain.Common.Utilities.ListItem)cmbTopic.SelectedItem).Value.ToString().Trim();
                savePostEntity.Area = ServAreaValues.Trim().TrimEnd(',');//((Captain.Common.Utilities.ListItem)cmbServArea.SelectedItem).Value.ToString().Trim();
                savePostEntity.Level = ((Captain.Common.Utilities.ListItem)cmbLevel.SelectedItem).Value.ToString().Trim();
                savePostEntity.Loc = ((Captain.Common.Utilities.ListItem)cmbLoc.SelectedItem).Value.ToString().Trim();
                savePostEntity.Loc_Note = txtLocNote.Text == "" ? "" : txtLocNote.Text.Trim();
                savePostEntity.Format = ((Captain.Common.Utilities.ListItem)cmbFormat.SelectedItem).Value.ToString().Trim();
                savePostEntity.Cred_type = ((Captain.Common.Utilities.ListItem)cmbCredType.SelectedItem).Value.ToString().Trim();
                savePostEntity.Cred_Hrs = txtCredHrs.Text == "" ? "" : txtCredHrs.Text.Trim();
                savePostEntity.Hrs = txtHrs.Text == "" ? "" : txtHrs.Text.Trim();
                savePostEntity.Tr_notes = txtTraiNotes.Text == "" ? "" : txtTraiNotes.Text.Trim();

                savePostEntity.Comp_Date = dtpCompDte.Text;//dgvStaffTrain.SelectedRows[0].Cells["Column2"].Value.ToString();

                savePostEntity.ADD_Operator = BaseForm.UserID;
                savePostEntity.LSTC_Operator = BaseForm.UserID;

                if (_model.STAFFData.InsertUpdateStaffBulkPost(savePostEntity, "UPDATE"))
                {
                    AlertBox.Show("Course Details Updated Successfully");

                    fillTrainGrid();
                    dgvStaffTrain.Enabled = true;
                    btnSave.Visible = btnCancel.Visible = false;
                    pbEdit.Visible = pbDel.Visible = true;
                    pnlParams.Enabled = false;
                }
            }
            //else
            //{
            //    _errorProvider.SetError(dtpCompDte, "Please Enter a Valid Completed Date");
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpCompDte, null);
            fillTrainGrid();
            dgvStaffTrain.Enabled = true;
            btnSave.Visible = btnCancel.Visible = false;
            pbEdit.Visible = pbDel.Visible = true;
            pnlParams.Enabled = false;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            dgvStaffTrain.Enabled = false;
            pnlParams.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
            pbEdit.Visible = pbDel.Visible = false;

            currentRowIndex = dgvStaffTrain.CurrentRow.Index;

            cmbTopic.Focus();
        }

        private void pbDel_Click(object sender, EventArgs e)
        {
            if (dgvStaffTrain.CurrentRow.Index > 0)
                currentRowIndex = dgvStaffTrain.CurrentRow.Index - 1;
            else
                currentRowIndex = 0;

            MessageBox.Show("Are you sure you want to delete the Course?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
        }

        private void MessageBoxHandler(DialogResult result)
        {
            if (result == DialogResult.Yes)
            {
                STAFFBULKPOSTEntity entity = new STAFFBULKPOSTEntity();

                entity.Agency = BaseForm.BaseAgency;
                entity.Dept = BaseForm.BaseDept;
                entity.Prog = BaseForm.BaseProg;
                if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                    entity.Year = BaseForm.BaseYear;
                entity.Staff_Code = dgvStaffTrain.SelectedRows[0].Cells["Column8"].Value.ToString();
                entity.Seq = dgvStaffTrain.SelectedRows[0].Cells["Column7"].Value.ToString();

                if (_model.STAFFData.InsertUpdateStaffBulkPost(entity, "DELETE"))
                {
                    AlertBox.Show("Deleted Course Successfully");

                    fillTrainGrid();
                    if (dgvStaffTrain.Rows.Count < 1)
                        pbEdit.Visible = pbDel.Visible = false;

                }
            }
        }

        private bool Validate_Complete_Date()
        {
            bool isCorrectDate = true;
            _errorProvider.SetError(dtpCompDte, null);
            if (string.IsNullOrWhiteSpace(dtpCompDte.Text))
            {
                _errorProvider.SetError(dtpCompDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCompDte.Text.Replace(Consts.Common.Colon, string.Empty)));
                isCorrectDate = false;
            }
            else
            {
                _errorProvider.SetError(dtpCompDte, null);

                if (!string.IsNullOrEmpty(dtpCompDte.Text))
                {
                    if (Convert.ToDateTime(dtpCompDte.Text) > DateTime.Now)
                    {
                        _errorProvider.SetError(dtpCompDte, string.Format("Future date is not allowed".Replace(Consts.Common.Colon, string.Empty)));
                        isCorrectDate = false;

                        dtpCompDte.Value = DateTime.Now;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpCompDte, null);
                    }
                }
            }
            return isCorrectDate;
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
    }
}