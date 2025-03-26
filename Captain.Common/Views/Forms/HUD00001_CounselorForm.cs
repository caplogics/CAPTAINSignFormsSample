using Captain.Common.Exceptions;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using DevExpress.DataAccess.Sql;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD00001_CounselorForm : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public HUD00001_CounselorForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, DataTable dtcouns, List<string> selCouns, string seq)
        {
            InitializeComponent();

            _baseForm = baseForm;
            _privilegeEntity = privilegeEntity;
            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            dtCounselors = dtcouns;
            _selCouns = selCouns;
            Couns_Seq = seq;

            fillCouns_Grid();

     
            this.Column2.Visible = true;
            this.Column2.ShowInVisibilityMenu = true;
            this.Column1.Width = 150;

        }

        public HUD00001_CounselorForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, DataTable dtcouns,string selCouns)
        {
            InitializeComponent();

            _baseForm = baseForm;
            _privilegeEntity = privilegeEntity;
            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            dtCounselors = dtcouns;
            selcounselors = selCouns;

            fillGropuCouns_Grid();

            
            this.Column2.Visible = false;
            this.Column2.ShowInVisibilityMenu = false;
            this.Column1.Width = 350;
        }

        private void fillGropuCouns_Grid()
        {
            dgvCounselors.Rows.Clear();

            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
            STAFFMST_List = STAFFMST_List.OrderBy(u => u.Active).ToList();

            int rowIndex = 0;
            if (dtCounselors.Rows.Count > 0)
            {
                dtCounselors = dtCounselors.AsEnumerable().GroupBy(row => row.Field<string>("HUDS_STF_CODE")).Select(group => group.First()).CopyToDataTable();

                foreach (DataRow dr in dtCounselors.Rows)
                {
                    counsList = STAFFMST_List.Where(x => x.Staff_Code == dr["HUDS_STF_CODE"].ToString()).ToList();

                    if (selcounselors != "")
                    {
                        if (dr["HUDS_STF_CODE"].ToString() == selcounselors)
                            rowIndex = dgvCounselors.Rows.Add(true, counsList[0].First_Name + " " + counsList[0].Last_Name, "", counsList[0].Staff_Code);
                        else
                            rowIndex = dgvCounselors.Rows.Add(false, counsList[0].First_Name + " " + counsList[0].Last_Name, "", counsList[0].Staff_Code);
                    }
                    else
                        rowIndex = dgvCounselors.Rows.Add(false, counsList[0].First_Name + " " + counsList[0].Last_Name, "", counsList[0].Staff_Code);
                }
            }

            if (dgvCounselors.Rows.Count > 0)
            {
                dgvCounselors.Rows[0].Selected = true;
                dgvCounselors.CurrentCell = dgvCounselors.Rows[0].Cells[1];
            }
        }

        List<STAFFMSTEntity> counsList = new List<STAFFMSTEntity>();
        List<HUDHTCATTNEntity> certList = new List<HUDHTCATTNEntity>();
        private void fillCouns_Grid()
        {
            dgvCounselors.Rows.Clear();

            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
            STAFFMST_List = STAFFMST_List.OrderBy(u => u.Active).ToList();

            List<HUDHTCATTNEntity> certi = _model.HUDCNTLData.GetHTCATTN(_baseForm.BaseAgency, Couns_Seq, "");

            int rowIndex = 0;
            if (dtCounselors.Rows.Count > 0)
            {
                dtCounselors = dtCounselors.AsEnumerable().GroupBy(row => row.Field<string>("HUDS_STF_CODE")).Select(group => group.First()).CopyToDataTable();

                bool IsCert=false;
                foreach (DataRow dr in dtCounselors.Rows)
                {
                    counsList = STAFFMST_List.Where(x => x.Staff_Code == dr["HUDS_STF_CODE"].ToString()).ToList();

                    certList = certi.Where(x => x.Staff_Code == dr["HUDS_STF_CODE"].ToString()).ToList();

                    if (certList.Count > 0)
                    {
                        IsCert = true;
                    }
                    else
                    {
                        IsCert = false;
                    }

                    if (IsCert)
                        rowIndex = dgvCounselors.Rows.Add(true, counsList[0].First_Name + " " + counsList[0].Last_Name, certList[0].Certificate, counsList[0].Staff_Code);
                    else
                        rowIndex = dgvCounselors.Rows.Add(false, counsList[0].First_Name + " " + counsList[0].Last_Name, "", counsList[0].Staff_Code);
                }
            }
            if (dgvCounselors.Rows.Count > 0)
            {
                dgvCounselors.Rows[0].Selected = true;
                dgvCounselors.CurrentCell = dgvCounselors.Rows[0].Cells[1];
            }
        }

        #region Properties

        public BaseForm _baseForm
        {
            get; set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get; set;
        }

        public DataTable dtCounselors
        {
            get; set;
        }

        public List<string> _selCouns
        {
            get; set;
        }
        public string selcounselors
        {
            get; set;
        }
        public string _Mode
        {
            get; set; 
        }

        public string Couns_Seq
        {
            get; set;
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string selectedCounselor = string.Empty;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvCounselors.Rows.Count > 0)
            {
                if (_privilegeEntity.Program == "HUD00001")
                {
                    HUDHTCATTNEntity delEntity = new HUDHTCATTNEntity();

                    delEntity.Agency = _baseForm.BaseAgency;

                    delEntity.Seq = Couns_Seq;

                    _model.HUDCNTLData.InsertUpdateHTCATTN(delEntity, "DELETE");

                    foreach (DataGridViewRow couns in dgvCounselors.Rows)
                    {
                        if (couns.Cells["Column0"].Value.ToString() == "True")
                        {
                            HUDHTCATTNEntity hUDHTCATTNEntity = new HUDHTCATTNEntity();

                            hUDHTCATTNEntity.Agency = _baseForm.BaseAgency;

                            hUDHTCATTNEntity.Seq = Couns_Seq;

                            hUDHTCATTNEntity.Staff_Code = couns["Column3"].Value.ToString();

                            hUDHTCATTNEntity.Certificate = couns["Column2"].Value.ToString();

                            hUDHTCATTNEntity.Add_Operator = _baseForm.UserID;
                            hUDHTCATTNEntity.Lstc_Operator = _baseForm.UserID;

                            if (_model.HUDCNTLData.InsertUpdateHTCATTN(hUDHTCATTNEntity, "INSERT"))
                            {
                                //this.DialogResult = DialogResult.OK;
                                //this.Close();
                            }
                        }
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (_privilegeEntity.Program == "HUD00002")
                {
                    //bool anyCheckboxChecked = dgvCounselors.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["Column0"].Value) == true);

                    bool isSuccess = false;

                    foreach (DataGridViewRow couns in dgvCounselors.Rows)
                    {
                        if(couns.Cells["Column0"].Value.ToString() == "True")
                        {
                            selectedCounselor = couns.Cells["Column1"].Value.ToString();
                            isSuccess = true;
                            break;
                        }
                    }
                    if (isSuccess)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }

        string selCouns = string.Empty;
        public string GetSelected_Couns()
        {
            if (dgvCounselors.Rows.Count > 0)
            {
                selCouns = string.Empty;

                foreach (DataGridViewRow couns in dgvCounselors.Rows)
                {
                    if (couns.Cells["Column0"].Value.ToString() == "True")
                    {
                        selCouns = selCouns + couns.Cells["Column3"].Value.ToString().Trim() + ",";
                    }
                    //else
                    //    selCouns = string.Empty;
                }
            }

            return selCouns.TrimEnd(',');
        }

        private void dgvCounselors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCounselors.Rows.Count > 0)
            {
                if (_privilegeEntity.Program == "HUD00001")
                {
                    if (e.RowIndex != -1 && e.ColumnIndex == 0)
                    {
                        if (dgvCounselors.SelectedRows[0].Cells["Column0"].Value.ToString() == "False")
                        {
                            dgvCounselors.SelectedRows[0].Cells["Column2"].Value = "";
                        }
                    }
                    if (e.RowIndex != -1 && e.ColumnIndex == 2)
                    {
                        if (dgvCounselors.SelectedRows[0].Cells["Column0"].Value.ToString() == "True")
                        {
                            dgvCounselors.SelectedRows[0].Cells["Column2"].ReadOnly = false;
                        }
                        else
                        {
                            dgvCounselors.SelectedRows[0].Cells["Column2"].ReadOnly = true;
                        }
                    }
                }
                else if (_privilegeEntity.Program == "HUD00002")
                {
                    DataGridView dgvcounsGrid = sender as DataGridView;

                    if (dgvCounselors.CurrentCell.ColumnIndex != 0)
                        return;

                    string selTraining = dgvCounselors.SelectedRows[0].Cells["Column3"].Value.ToString();

                    foreach (DataGridViewRow dr in dgvCounselors.Rows)
                    {
                        string rowCode = dr.Cells["Column3"].Value.ToString();
                        if (!rowCode.Equals(selTraining))
                        {
                            dr.Cells["Column0"].Value = "False";
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
    }
}
