using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using System;
using System.Collections.Generic;
using Wisej.Web;
using static NPOI.HSSF.Util.HSSFColor;

namespace Captain.Common.Views.Forms
{
    public partial class Paid_Bundle_Sel_Form : Form
    {

        private CaptainModel _model = null;

        public Paid_Bundle_Sel_Form(BaseForm baseform, PrivilegeEntity privilegeEntity, List<CABUNDLEENTITY> Paybatch_List, string selBudgets, string Agy, string Dept, string Prog, string Year)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;
            paidBundlesList = Paybatch_List;
            _selBudgets = selBudgets;
            _Agency = Agy;
            _Depart = Dept;
            _Program = Prog;
            _Year = Year;

            fill_Budgets();

            fillSelBundles(_selBudgets);

        }

        public string selStaff = string.Empty;
        private void btnOk_Click(object sender, EventArgs e)
        {
            string selectedCode = string.Empty;

                foreach (DataGridViewRow gvRows in dgvBundles.Rows)
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

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void fill_Budgets()
        {
            if (paidBundlesList.Count > 0)
            {
                dgvBundles.Rows.Clear();

                foreach (CABUNDLEENTITY Entity in paidBundlesList)
                {
                    dgvBundles.Rows.Add(false, "00000".Substring(0, 5 - Entity.CAB_BUNDLE.Length) + Entity.CAB_BUNDLE, Entity.CAB_BUNDLE);
                }

                if (dgvBundles.Rows.Count > 0)
                {
                    dgvBundles.Rows[0].Selected = true;
                    dgvBundles.CurrentCell = dgvBundles.Rows[0].Cells[1];
                }
            }

        }
        public string GetSelectedBundle()
        {
            string sel_bundle_List = string.Empty;
            foreach (DataGridViewRow dr in dgvBundles.Rows)
            {

                if (dr.Cells["gvSelect"].Value != null && Convert.ToBoolean(dr.Cells["gvSelect"].Value) == true)
                {
                    if (!sel_bundle_List.Equals(string.Empty))
                    {
                        sel_bundle_List += dr.Cells["gvCode"].Value + ",";//"'" + dr.Cells["gvCode"].Value + "'" + ",";
                    }
                    else
                    {
                        sel_bundle_List += dr.Cells["gvCode"].Value + ",";//"'" + dr.Cells["gvCode"].Value + "'" + ",";
                    }
                }

            }

            sel_bundle_List = sel_bundle_List.TrimEnd(',');
            return sel_bundle_List;
        }

        public void fillSelBundles(string strCode)
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

            foreach (DataGridViewRow staff in dgvBundles.Rows)
            {
                if (CodesList.Contains(staff.Cells["gvCode"].Value.ToString()))
                    staff.Cells["gvSelect"].Value = true;
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
        public List<CABUNDLEENTITY> paidBundlesList
        {
            get; set;
        }
        public string _selBudgets
        {
            get; set;
        }
        public string _Agency
        {
            get; set;
        }
        public string _Depart
        {
            get; set;
        }
        public string _Program
        {
            get; set;
        }
        public string _Year
        {
            get; set;
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
