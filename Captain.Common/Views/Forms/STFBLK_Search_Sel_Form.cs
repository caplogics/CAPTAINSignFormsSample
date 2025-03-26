using Aspose.Cells.Drawing;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class STFBLK_Search_Sel_Form : Form
    {
        private List<STAFFMSTEntity> _selectedStaff = null;

        private List<STAFFTRAINEntity> _selectedTrain = null;

        private List<CommonEntity> _selectedcodes = null;

        public STFBLK_Search_Sel_Form(BaseForm baseform, PrivilegeEntity privilegeEntity, List<STAFFMSTEntity> staffEntity, string formtype, string selStaffCode)
        {
            InitializeComponent();

            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;

            staffMSTEntity = staffEntity;
            formType = formtype;

            this.Text = "Select Staff Member(s)";

            fill_Grid();

            fillSelCode(selStaffCode);
        }

        public STFBLK_Search_Sel_Form(BaseForm baseform, PrivilegeEntity privilegeEntity, List<STAFFTRAINEntity> trainEntity, string formtype, string selTrainCode)
        {
            InitializeComponent();

            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;

            trainingEntity = trainEntity;
            formType = formtype;

            this.Text = "Select Training Name(s)";

            fill_Grid();

            fillSelCode(selTrainCode);
        }

        public STFBLK_Search_Sel_Form(BaseForm baseform, PrivilegeEntity privilegeEntity, List<CommonEntity> multipleEtity, string formtype, string selmultiCode)
        {
            InitializeComponent();

            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;

            multiTrainEntity = multipleEtity;
            formType = formtype;

            if (formType == "Topic")
                this.Text = "Select Topic(s)";
            else if (formType == "Format")
                this.Text = "Select Format(s)";
            else if (formType == "Loc")
                this.Text = "Select Location(s)";
            else if (formType == "CredType")
                this.Text = "Select Credit Type(s)";
            else
                this.Text = "Staff Bulk Post Search Form";

            fill_Grid();

            fillSelCode(selmultiCode);
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
        public List<STAFFMSTEntity> staffMSTEntity
        {
            get; set;
        }
        public List<STAFFTRAINEntity> trainingEntity
        {
            get; set;
        }
        public List<CommonEntity> multiTrainEntity
        {
            get; set;
        }

        public string formType
        {
            get; set;
        }
        #endregion

        private void fill_Grid()
        {
            dgvStaffBulkPost.Rows.Clear();

            int rowIndex = 0;

            if (formType == "Staff")
            {
                foreach (STAFFMSTEntity staffEntity in staffMSTEntity)
                {
                    rowIndex = dgvStaffBulkPost.Rows.Add(false, (staffEntity.First_Name + " " + staffEntity.Last_Name), staffEntity.Staff_Code);
                }
            }
            else if (formType == "Training")
            {
                foreach (STAFFTRAINEntity trainEntity in trainingEntity)
                {
                    rowIndex = dgvStaffBulkPost.Rows.Add(false, trainEntity.Name, trainEntity.ID);
                }
            }
            else if (formType == "Topic")
            {
                foreach (CommonEntity topicEntity in multiTrainEntity)
                {
                    rowIndex = dgvStaffBulkPost.Rows.Add(false, topicEntity.Desc, topicEntity.Code);
                }
            }
            else if (formType == "Format")
            {
                foreach (CommonEntity formatEntity in multiTrainEntity)
                {
                    rowIndex = dgvStaffBulkPost.Rows.Add(false, formatEntity.Desc, formatEntity.Code);
                }
            }
            else if (formType == "Loc")
            {
                foreach (CommonEntity locEntity in multiTrainEntity)
                {
                    rowIndex = dgvStaffBulkPost.Rows.Add(false, locEntity.Desc, locEntity.Code);
                }
            }
            else if (formType == "CredType")
            {
                foreach (CommonEntity credTypeEntity in multiTrainEntity)
                {
                    rowIndex = dgvStaffBulkPost.Rows.Add(false, credTypeEntity.Desc, credTypeEntity.Code);
                }
            }

            if (dgvStaffBulkPost.Rows.Count > 0)
            {
                dgvStaffBulkPost.Rows[0].Selected = true;
                dgvStaffBulkPost.CurrentCell = dgvStaffBulkPost.Rows[0].Cells[1];
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvStaffBulkPost_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStaffBulkPost.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 0)
                {
                    bool anyCheckboxChecked = dgvStaffBulkPost.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["gvSelect"].Value) == true);

                    if (anyCheckboxChecked)
                    {
                        btnOk.Visible = true;
                    }
                    else
                        btnOk.Visible = false;
                }
            }
        }

        public string selStaff = string.Empty;
        public string selTrain = string.Empty;
        public string selTopic = string.Empty;
        public string selFormat = string.Empty;
        public string selLoc = string.Empty;
        public string selCredType = string.Empty;
        private void btnOk_Click(object sender, EventArgs e)
        {
            string selectedCode = string.Empty;

            if (formType == "Staff")
            {
                foreach (DataGridViewRow gvRows in dgvStaffBulkPost.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedCode.Equals(string.Empty))
                        {
                            selectedCode += ",";
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }
                        else
                        {
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }
                    }
                }

                selStaff = selectedCode;
            }
            else if (formType == "Training")
            {
                foreach (DataGridViewRow gvRows in dgvStaffBulkPost.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedCode.Equals(string.Empty))
                        {
                            selectedCode += ",";
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }
                        else
                        {
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }

                    }
                }

                selTrain = selectedCode;
            }
            else if (formType == "Topic")
            {
                foreach (DataGridViewRow gvRows in dgvStaffBulkPost.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedCode.Equals(string.Empty))
                        {
                            selectedCode += ",";
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }
                        else
                        {
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }

                    }
                }

                selTopic = selectedCode;
            }
            else if (formType == "Format")
            {
                foreach (DataGridViewRow gvRows in dgvStaffBulkPost.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedCode.Equals(string.Empty))
                        {
                            selectedCode += ",";
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }
                        else
                        {
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }

                    }
                }

                selFormat = selectedCode;
            }
            else if (formType == "Loc")
            {
                foreach (DataGridViewRow gvRows in dgvStaffBulkPost.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedCode.Equals(string.Empty))
                        {
                            selectedCode += ",";
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }
                        else
                        {
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }

                    }
                }

                selLoc = selectedCode;
            }
            else if (formType == "CredType")
            {
                foreach (DataGridViewRow gvRows in dgvStaffBulkPost.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedCode.Equals(string.Empty))
                        {
                            selectedCode += ",";
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }
                        else
                        {
                            selectedCode += gvRows.Cells["gvCode"].Value;
                        }

                    }
                }

                selCredType = selectedCode;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        public string fillCodesforReport()
        {
            string selectedCode = string.Empty;
            if (_privilegeEntity.ViewPriv.Equals("true"))
            {
                foreach (DataGridViewRow gvRows in dgvStaffBulkPost.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedCode.Equals(string.Empty))
                        {
                            selectedCode += "'" + gvRows.Cells["gvCode"].Value + "'" + ",";
                        }
                        else
                        {
                            selectedCode += "'" + gvRows.Cells["gvCode"].Value + "'" + ",";
                        }
                    }
                }
            }

            selectedCode = selectedCode.TrimEnd(',');
            return selectedCode;
        }

        public void fillSelCode(string strCode)
        {
            List<string> CodesList = new List<string>();
            string selectedCodes = strCode;
            if (!string.IsNullOrEmpty(selectedCodes))
            {
                selectedCodes = selectedCodes.Replace("'", "");
                string[] Codes = selectedCodes.Split(',');

                for (int i = 0; i < Codes.Length; i++)
                {
                    CodesList.Add(Codes.GetValue(i).ToString());
                }
            }
         
            int rowIndex = 0;

            //List<STAFFMSTEntity> selstaff = staffMSTEntity.FindAll(x => CodesList.Contains(x.Staff_Code)).ToList();

            foreach (DataGridViewRow staff in dgvStaffBulkPost.Rows)
            {
                if (formType == "Staff")
                {
                    if (CodesList.Contains(staff.Cells["gvCode"].Value.ToString()))
                        staff.Cells["gvSelect"].Value = true;
                }

                if (formType == "Training")
                {
                    if (CodesList.Contains(staff.Cells["gvCode"].Value.ToString()))
                        staff.Cells["gvSelect"].Value = true;
                }
                if (formType == "Topic")
                {
                    if (CodesList.Contains(staff.Cells["gvCode"].Value.ToString()))
                        staff.Cells["gvSelect"].Value = true;
                }
                if (formType == "Format")
                {
                    if (CodesList.Contains(staff.Cells["gvCode"].Value.ToString()))
                        staff.Cells["gvSelect"].Value = true;
                }
                if (formType == "Loc")
                {
                    if (CodesList.Contains(staff.Cells["gvCode"].Value.ToString()))
                        staff.Cells["gvSelect"].Value = true;
                }
                if (formType == "CredType")
                {
                    if (CodesList.Contains(staff.Cells["gvCode"].Value.ToString()))
                        staff.Cells["gvSelect"].Value = true;
                }

            }
            
        }

        //public List<STAFFMSTEntity> SelectedStaffEntity
        //{
        //    get
        //    {
        //        return _selectedStaff = (from c in dgvStaffBulkPost.Rows.Cast<DataGridViewRow>().ToList()
        //                                  where (((DataGridViewCheckBoxCell)c.Cells["gvSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
        //                                  select ((DataGridViewRow)c).Tag as STAFFMSTEntity).ToList();

        //    }
        //}

        //public List<STAFFTRAINEntity> SelectedTrainingEntity
        //{
        //    get
        //    {
        //        return _selectedTrain = (from c in dgvStaffBulkPost.Rows.Cast<DataGridViewRow>().ToList()
        //                                  where (((DataGridViewCheckBoxCell)c.Cells["gvSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
        //                                  select ((DataGridViewRow)c).Tag as STAFFTRAINEntity).ToList();

        //    }
        //}

        //public List<CommonEntity> SelectedEntity
        //{
        //    get
        //    {
        //        return _selectedcodes = (from c in dgvStaffBulkPost.Rows.Cast<DataGridViewRow>().ToList()
        //                                  where (((DataGridViewCheckBoxCell)c.Cells["gvSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
        //                                  select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

        //    }
        //}
    }
}
