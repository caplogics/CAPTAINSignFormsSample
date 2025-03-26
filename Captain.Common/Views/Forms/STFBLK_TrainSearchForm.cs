using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using DevExpress.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Drawing;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class STFBLK_TrainSearchForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        public int strIndex = 0;

        #endregion

        public STFBLK_TrainSearchForm(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            _baseForm = baseForm;
            _privilegeEntity = privilegeEntity;
            
            _model = new CaptainModel();

            if (_baseForm.BaseAgyTabsEntity.Count > 0)
            {
                Topics = _baseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "00450");
                ServArea = _baseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "S0220");
                Formats = _baseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "00453");
            }

            staffTrainEntity = _model.STAFFData.GetSTFTRAIN(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, "");

            fillStaffTrain_Grid();
        }

        #region Properties

        public BaseForm _baseForm
        {
            get; set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get;set;
        }

        public List<STAFFTRAINEntity> staffTrainEntity
        {
            get; set;
        }

        #endregion

        List<CommonEntity> Topics = new List<CommonEntity>();
        List<CommonEntity> ServArea = new List<CommonEntity>();
        List<CommonEntity> Formats = new List<CommonEntity>();

        private void fillStaffTrain_Grid()
        {
            dgvStaffTrain.Rows.Clear();

            foreach (STAFFTRAINEntity entity in staffTrainEntity)
            {
                string Topic = string.Empty, ServAre = string.Empty, Format = string.Empty;
                if (Topics.Count > 0 && entity.Topic != "0")
                    Topic = Topics.Find(u => u.Code == entity.Topic).Desc.Trim();
                if (ServArea.Count > 0 && entity.Serv_Area != "00")
                    ServAre = ServArea.Find(u => u.Code == entity.Serv_Area).Desc.Trim();
                if (Formats.Count > 0 && entity.Format != "0")
                    Format = Formats.Find(u => u.Code == entity.Format).Desc.Trim();


                int rowIndex = dgvStaffTrain.Rows.Add(false, entity.Name, Topic, ServAre, Format, entity.Cred_Hrs, entity.Hrs, entity.ID);

                dgvStaffTrain.Rows[rowIndex].Tag = entity;
            }

            if (dgvStaffTrain.Rows.Count > 0)
            {
                dgvStaffTrain.Rows[0].Selected = true;
                dgvStaffTrain.CurrentCell = dgvStaffTrain.Rows[0].Cells[1];
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public STAFFTRAINEntity selTraining { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvStaffTrain.Rows.Count > 0)
            {
                STAFFBULKPOSTEntity postEntity = new STAFFBULKPOSTEntity();

                foreach (DataGridViewRow dr in dgvStaffTrain.Rows)
                {
                    if (dr.Cells["Column0"].Value.ToString() == "True")
                    {
                        selTraining = new STAFFTRAINEntity();

                        selTraining = dr.Tag as STAFFTRAINEntity;

                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }
            }
        }

        private void dgvStaffTrain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgvstaffGrid = sender as DataGridView;

            if (dgvstaffGrid.CurrentCell.ColumnIndex != 0)
                return;

            string selTraining = dgvstaffGrid.SelectedRows[0].Cells["Column7"].Value.ToString();

            foreach (DataGridViewRow dr in dgvstaffGrid.Rows)
            {
                string rowCode = dr.Cells["Column7"].Value.ToString();
                if (!rowCode.Equals(selTraining))
                {
                    dr.Cells["Column0"].Value = "false";
                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
    }
}
