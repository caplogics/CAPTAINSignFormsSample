using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using DevExpress.ExpressApp.Validation.AllContextsView;
using DevExpress.Utils.Extensions;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class MasterPovertyCopyFrom : Form
    {
        #region Private Variables

        private CaptainModel _model = null;
        int currentRowIndex = 0;
        #endregion


        public MasterPovertyCopyFrom(BaseForm baseForm, PrivilegeEntity privilege, string County, string Type)
        {
            InitializeComponent();

            _baseForm = baseForm;
            _privilege = privilege;
            _county = County;
            _type = Type;

            _model = new CaptainModel();

            //dsCounty = DatabaseLayer.MasterPoverty.GET_CASEGDL(_baseForm.BaseAgency, "", "","HUD");
            if (_type == "HUD")
                dsCounty = DatabaseLayer.MasterPoverty.GET_CASEGDL("", "", "", "HUD");
            else if (_type == "CMI")
                dsCounty = DatabaseLayer.MasterPoverty.GET_CASEGDL("", "", "", "CMI");

            FillCountyGrid();
        }

        private void FillCountyGrid()
        {
            dgvCounty.Rows.Clear();

            DataTable dtCounty = new DataTable();
            dtCounty = dsCounty.Tables[0];

            DataView dvCounty = new DataView(dtCounty);
            dvCounty.RowFilter = "GDL_COUNTY<>'" + _county + "'";
            dvCounty.Sort = "GDL_COUNTY, GDL_START_DATE DESC";
            dtCounty = dvCounty.ToTable();

            string countDesc = string.Empty;

           // List<CommonEntity> County = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, _baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);

            int rowIndex = 0;
            foreach (DataRow dr in dtCounty.Rows)
            {
                //countDesc = County.Find(x => x.Code == dr["GDL_COUNTY"].ToString()).Desc;

                rowIndex = dgvCounty.Rows.Add(false, dr["AGYS_DESC"].ToString(), dr["GDL_START_DATE"].ToString() == "" ? "" : Convert.ToDateTime(dr["GDL_START_DATE"].ToString()).ToString("MM/dd/yyyy"), dr["GDL_END_DATE"].ToString() == "" ? "" : Convert.ToDateTime(dr["GDL_END_DATE"].ToString()).ToString("MM/dd/yyyy"), _county, dr["GDL_COUNTY"].ToString(), dr["GDL_AGENCY"].ToString(), dr["GDL_DEPT"].ToString(), dr["GDL_PROGRAM"].ToString());

                dgvCounty.Rows[rowIndex].Tag = copyFromEntity;
            }

            if (dgvCounty.Rows.Count > 0)
            {
                dgvCounty.Rows[currentRowIndex].Selected = true;
                dgvCounty.CurrentCell = dgvCounty.SelectedRows[currentRowIndex].Cells[0]; 
            }
        }

        #region Properties
        public BaseForm _baseForm
        {
            get; set;
        }

        public PrivilegeEntity _privilege
        {
            get; set;
        }
        public string _county
        {
            get; set;
        }
        public string _type
        {
            get; set;
        }
        public List<MasterPovertyEntity> copyFromEntity
        {
            get;
            set;
        }
        public DataSet dsCounty
        {
            get;
            set;
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
           this.Close();
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvCounty.CurrentRow.Cells["gvSelect"].Value.ToString() == "True")
            {
                if (ValidateRecord())
                {
                    //copyFromEntity = _model.masterPovertyData.GetCaseGdladpt(_baseForm.BaseAgency, "", "", "HUD", dgvCounty.CurrentRow.Cells["gvSelCounty"].Value.ToString());

                    if (_type == "HUD")
                    {
                        copyFromEntity = _model.masterPovertyData.GetCaseGdladpt("", "", "", "HUD", dgvCounty.CurrentRow.Cells["gvSelCounty"].Value.ToString());

                        List<MasterPovertyEntity> modelPovertyList = _model.masterPovertyData.GetCaseGdlSubadpt(dgvCounty.CurrentRow.Cells["gvAgency"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvDept"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvProg"].Value.ToString(), "HUD", dgvCounty.CurrentRow.Cells["gvSelCounty"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvStartDte"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvEndDte"].Value.ToString());

                        bool isSaved = false;

                        MasterPovertyEntity selRec = copyFromEntity.Find(u => u.GdlStartDate == dgvCounty.CurrentRow.Cells["gvStartDte"].Value.ToString() && u.GdlEndDate == dgvCounty.CurrentRow.Cells["gvEndDte"].Value.ToString());

                        if (selRec != null)
                        {
                            selRec.GdlCounty = _county;

                            if (_model.masterPovertyData.InsertCaseGdl(selRec))
                            {
                                foreach (MasterPovertyEntity saveEntity in modelPovertyList)
                                {
                                    saveEntity.GdlCounty = _county;

                                    if (_model.masterPovertyData.InsertCaseGdl(saveEntity))
                                    {
                                        isSaved = true;
                                    }
                                }
                            }
                        }
                        if (isSaved)
                        {
                            AlertBox.Show("Copied " + dgvCounty.CurrentRow.Cells["gvCounty"].Value.ToString() + " County Details to this County Successfully");
                            DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else if (_type == "CMI")
                    {
                        copyFromEntity = _model.masterPovertyData.GetCaseGdladpt("", "", "", "CMI", dgvCounty.CurrentRow.Cells["gvSelCounty"].Value.ToString());

                        List<MasterPovertyEntity> modelPovertyList = _model.masterPovertyData.GetCaseGdlSubadpt(dgvCounty.CurrentRow.Cells["gvAgency"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvDept"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvProg"].Value.ToString(), "CMI", dgvCounty.CurrentRow.Cells["gvSelCounty"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvStartDte"].Value.ToString(), dgvCounty.CurrentRow.Cells["gvEndDte"].Value.ToString());

                        bool isSaved = false;

                        MasterPovertyEntity selRec = copyFromEntity.Find(u => u.GdlStartDate == dgvCounty.CurrentRow.Cells["gvStartDte"].Value.ToString() && u.GdlEndDate == dgvCounty.CurrentRow.Cells["gvEndDte"].Value.ToString());

                        if (selRec != null)
                        {
                            selRec.GdlCounty = _county;

                            if (_model.masterPovertyData.InsertCaseGdl(selRec))
                            {
                                foreach (MasterPovertyEntity saveEntity in modelPovertyList)
                                {
                                    saveEntity.GdlCounty = _county;

                                    if (_model.masterPovertyData.InsertCaseGdl(saveEntity))
                                    {
                                        isSaved = true;
                                    }
                                }
                            }
                        }
                        if (isSaved)
                        {
                            AlertBox.Show("Copied " + dgvCounty.CurrentRow.Cells["gvCounty"].Value.ToString() + " County Details to this County Successfully");
                            DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                else
                {
                    AlertBox.Show("Record already exists with the Selected Date Range", MessageBoxIcon.Warning);
                }
            }
            else
            {
                AlertBox.Show("Please select any County", MessageBoxIcon.Warning);
            }

        }

        private bool ValidateRecord()
        {
            bool isRecValid = false;


            if (_type == "HUD")
            {
                List<MasterPovertyEntity> selPovertyEntity = _model.masterPovertyData.GetCaseGdladpt("", "", "", "HUD", dgvCounty.CurrentRow.Cells["gvPresentCounty"].Value.ToString());

                selPovertyEntity = selPovertyEntity.FindAll(s => s.GdlStartDate == dgvCounty.CurrentRow.Cells["gvStartDte"].Value.ToString() && s.GdlEndDate == dgvCounty.CurrentRow.Cells["gvEndDte"].Value.ToString());

                if (selPovertyEntity.Count > 0)
                    isRecValid = false;
                else
                    isRecValid = true;
            }
            else if (_type == "CMI")
            {
                List<MasterPovertyEntity> selPovertyEntity = _model.masterPovertyData.GetCaseGdladpt("", "", "", "CMI", dgvCounty.CurrentRow.Cells["gvPresentCounty"].Value.ToString());

                selPovertyEntity = selPovertyEntity.FindAll(s => s.GdlStartDate == dgvCounty.CurrentRow.Cells["gvStartDte"].Value.ToString() && s.GdlEndDate == dgvCounty.CurrentRow.Cells["gvEndDte"].Value.ToString());

                if (selPovertyEntity.Count > 0)
                    isRecValid = false;
                else
                    isRecValid = true;
            }
            return isRecValid;
        }

        private void dgvCounty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCounty.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 0)
                {
                    dgvCounty.Rows.ForEach(x => x.Cells["gvSelect"].Value = false);

                    dgvCounty.Rows[e.RowIndex].Cells["gvSelect"].Value = true;
                }
            }
        }
    }


}
