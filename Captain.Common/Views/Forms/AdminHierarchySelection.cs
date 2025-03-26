using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

namespace Captain.Common.Views.Forms
{
    public partial class AdminHierarchySelection : Form
    {
        public AdminHierarchySelection(BaseForm baseForm)
        {
            InitializeComponent();

            BaseForm = baseForm;
            //PrivilegeEntity = privilegesEntity;

            FillAgencyGrid();

        }

        #region Properties

        public BaseForm BaseForm { get; set; }
        PrivilegeEntity PrivilegeEntity = new PrivilegeEntity();

        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        CaptainModel model = new CaptainModel();
        private void FillAgencyGrid()
        {

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once
            List<HierarchyEntity> userHierarchy = model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
            if (userHierarchy.Count > 0) userHierarchy = userHierarchy.FindAll(u => u.HirarchyType == "I" && u.Agency == "**");
            gvwHierarchie.Rows.Clear();
            //List<ListItem> listItem = new List<ListItem>();
            int TmpRows = 0;
            int AgyIndex = 0;
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (userHierarchy.Count > 0)
                    {
                        if (BaseForm.BaseAdminAgency == "**")
                            TmpRows = gvwHierarchie.Rows.Add(true, "**", "All Agencies");
                        else
                            TmpRows = gvwHierarchie.Rows.Add(false, "**", "All Agencies");
                    }

                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {

                        if (BaseForm.BaseAdminAgency == dr["Agy"].ToString())
                            TmpRows = gvwHierarchie.Rows.Add(true, dr["Agy"].ToString(), dr["Name"].ToString());
                        else
                            TmpRows = gvwHierarchie.Rows.Add(false, dr["Agy"].ToString(), dr["Name"].ToString());

                        if (BaseForm.BaseAdminAgency == dr["Agy"].ToString())
                            AgyIndex = TmpRows;

                        TmpRows++;
                    }
                    if (TmpRows > 0)
                    {

                        //if (DefHieExist)
                        //    CmbAgency.SelectedIndex = AgyIndex;
                        //else
                        //{
                        //    if (CmbAgency.Items.Count == 1)
                        //        CmbAgency.SelectedIndex = 0;
                        //}
                        gvwHierarchie.CurrentCell = gvwHierarchie.Rows[AgyIndex].Cells[1];
                        gvwHierarchie.Rows[AgyIndex].Selected = true;

                    }
                }
                //DefAgy = DefDept = DefProg = DefYear = null;
            }
            catch (Exception ex) { }
        }

        private void gvwHierarchie_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gvwHierarchie = sender as DataGridView;

            if (gvwHierarchie.CurrentCell.ColumnIndex != 0)
                return;

            string selectedHIE = gvwHierarchie.SelectedRows[0].Cells["Code"].Value.ToString();
            //bool isSelect = false;

            foreach (DataGridViewRow dr in gvwHierarchie.Rows)
            {
                string rowCode = dr.Cells["Code"].Value.ToString();
                if (!rowCode.Equals(selectedHIE))
                {
                    dr.Cells["Select"].Value = "false";
                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }
            }

        }

        public string SelectedAgency()
        {
            string Agency = string.Empty;

            if (gvwHierarchie.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwHierarchie.Rows)
                {
                    if (dr.Cells["Select"].Value.ToString().ToUpper() == "TRUE")
                    {
                        Agency = dr.Cells["Code"].Value.ToString();
                        break;
                    }
                }
            }



            return Agency;
        }
    }
}
