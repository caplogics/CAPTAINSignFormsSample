using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD99001_Staff_Members : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<CommonEntity> _selectedStaffMem = null;

        public HUD99001_Staff_Members(BaseForm baseForm, PrivilegeEntity privilege/*, string screenCode*/)
        {
            InitializeComponent();

            _baseForm = baseForm;
            _privilegeEntity = privilege;
            _model = new CaptainModel();

            //currerntIndex = currentsel;
            //ScreenCode = screenCode;

            fill_Grid();
        }

        #region Properties

        public BaseForm _baseForm
        {
            get;
            set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get;
            set;
        }

        public string ScreenCode
        {
            get;
            set;
        }

        #endregion

       public int currerntIndex = 0;

        private void fill_Grid()
        {
            dgvStaffMem.Rows.Clear();

            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
            STAFFMST_List = STAFFMST_List.OrderBy(u => u.Active).ToList();

            //if (ScreenCode == "HUD99001")
            //{
                //this.dataGridViewColumn3.Visible = true;
                if (STAFFMST_List.Count > 0)
                {
                    foreach (STAFFMSTEntity staffdetails in STAFFMST_List)
                    {
                        int rowIndex = dgvStaffMem.Rows.Add(false, staffdetails.Staff_Code, LookupDataAccess.GetMemberName(staffdetails.First_Name, staffdetails.Middle_Name, staffdetails.Last_Name, string.Empty), staffdetails.HNo.Trim() + " " + staffdetails.Street.Trim() + " " + staffdetails.Suffix.Trim() + " " + staffdetails.City.Trim() + ", " + staffdetails.State.Trim() + "  " + SetLeadingZeros(staffdetails.Zip.Trim()) + " - " + "0000".Substring(0, 4 - staffdetails.Zip_Plus.Length) + staffdetails.Zip_Plus, staffdetails.First_Name, staffdetails.Last_Name, staffdetails.Staff_Code.Trim());

                        dgvStaffMem.Rows[rowIndex].Tag = staffdetails;

                        if (!staffdetails.Active.Equals("A"))
                            dgvStaffMem.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            //}
            //else if (ScreenCode == "HUD00001")
            //{
            //    this.dataGridViewColumn3.Visible = false;

            //    List<HUDSTAFFEntity> hudCounselorList = _model.HUDCNTLData.GetHUDSTAFF(_baseForm.BaseAgency,string.Empty,"2");

            //    List<STAFFMSTEntity> StaffName = STAFFMST_List.Where(u => u.Staff_Code.ToString() == hudCounselorList[0].Staff_code).ToList();

            //    if (hudCounselorList.Count > 0)
            //    {
            //        foreach(HUDSTAFFEntity entity in hudCounselorList)
            //        {
            //            dgvStaffMem.Rows.Add(false, entity.Staff_code, StaffName[0].First_Name + " " + StaffName[0].Last_Name);
            //        }
            //    }
            //}

            if (dgvStaffMem.Rows.Count > 0)
            {
                dgvStaffMem.Rows[currerntIndex].Selected = true;
                dgvStaffMem.CurrentCell = dgvStaffMem.Rows[currerntIndex].Cells[1];
            }
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seqlen = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seqlen)
            {
                case 4:
                    TmpCode = "0" + TmpCode;
                    break;
                case 3:
                    TmpCode = "00" + TmpCode;
                    break;
                case 2:
                    TmpCode = "000" + TmpCode;
                    break;
                case 1:
                    TmpCode = "0000" + TmpCode;
                    break;
            }

            return (TmpCode);
        }

        public STAFFMSTEntity selStaffMem
        {
            get; set;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvStaffMem.Rows.Count > 0)
            {
                HUD99001_Staff staff = sender as HUD99001_Staff;

                foreach (DataGridViewRow dr in dgvStaffMem.Rows)
                {
                    if (dr.Cells["Column3"].Value.ToString() == "True")
                    {
                        selStaffMem = new STAFFMSTEntity();

                        selStaffMem = dr.Tag as STAFFMSTEntity;

                        currerntIndex = dgvStaffMem.CurrentRow.Index;

                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }
            }
        }

        public string SelectedStaffMem()
        {
            string staff = string.Empty;

            if (dgvStaffMem.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in dgvStaffMem.Rows)
                {
                    if (dr.Cells["Column3"].Value.ToString().ToUpper() == "TRUE")
                    {
                        staff = dr.Cells["Column2"].Value.ToString();
                        break;
                    }
                }
            }
            return staff;
        }

        private void dgvStaffMem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgvstaffGrid = sender as DataGridView;

            if (dgvstaffGrid.CurrentCell.ColumnIndex != 0)
                return;

            string selStaff = dgvstaffGrid.SelectedRows[0].Cells["Column2"].Value.ToString();

            foreach (DataGridViewRow dr in dgvstaffGrid.Rows)
            {
                string rowCode = dr.Cells["Column2"].Value.ToString();
                if (!rowCode.Equals(selStaff))
                {
                    dr.Cells["Column3"].Value = "false";
                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
