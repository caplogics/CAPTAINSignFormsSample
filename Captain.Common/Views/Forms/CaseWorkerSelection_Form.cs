#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CaseWorkerSelection_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;
        int rowsSel = 0;
        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        public string strTaskCount = string.Empty;
        #endregion

        /// <summary>
        /// This Constractor Using Funding Code
        /// </summary>
        /// <param name="baseform"></param>
        /// <param name="selFundincode"></param>
        /// <param name="privileges"></param>
        /// 
        public CaseWorkerSelection_Form(BaseForm baseform, List<HierarchyEntity> selWorkers, PrivilegeEntity privileges,string Agy,string Dept,string Prog)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            //GchartCode = string.Empty;
            _model = new CaptainModel();
            this.Text = "Assigned Worker Selection";
            BaseForm = baseform;
            Program = Prog; Agency = Agy; Depart = Dept;
            CaseWorkerList = selWorkers;
            FillGridCaseworker();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Code { get; set; }

        //public string ComponentCode { get; set; }

        //public string GchartCode { get; set; }

        public string FormCode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        public List<HierarchyEntity> CaseWorkerList { get; set; }

        //public List<ChldTrckEntity> TasksList { get; set; }

        public bool IsSaveValid { get; set; }

        public string Agency { get; set; }
        public string Depart { get; set; }
        public string Program { get; set; }

        #endregion

        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = Consts.Icons.ico_Tick;

        private void FillGridCaseworker()
        {
            gvwWorker.Rows.Clear();
            hierarchyEntity = _model.CaseMstData.GetCaseWorker(BaseForm.BaseHierarchyCwFormat.ToString(), Agency, Depart, Program);
            string strCaseworker = string.Empty;
            //int rowIndex = 0;
            foreach (HierarchyEntity caseworker in hierarchyEntity)
            {
                if (strCaseworker != caseworker.CaseWorker.ToString())
                {
                    strCaseworker = caseworker.CaseWorker.ToString();
                    bool Sel_Ref = false;
                    int rowIndex = 0;
                    foreach (HierarchyEntity SEntity in CaseWorkerList)
                    {
                        if (SEntity.CaseWorker.Trim() == caseworker.CaseWorker.Trim())
                        {
                            Sel_Ref = true;
                            rowIndex = gvwWorker.Rows.Add(Img_Tick, caseworker.CaseWorker.Trim(), caseworker.HirarchyName.Trim(), "Y",caseworker.InActiveFlag=="Y"?"Inactive":"Active");
                            rowsSel = rowIndex;
                            gvwWorker.Rows[rowIndex].Tag = SEntity;
                            if (caseworker.InActiveFlag == "Y")
                            {
                                gvwWorker.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            }
                        }
                    }

                    if (!Sel_Ref)
                    {
                        rowIndex = gvwWorker.Rows.Add(Img_Blank, caseworker.CaseWorker.Trim(), caseworker.HirarchyName.Trim(), "N", caseworker.InActiveFlag == "Y" ? "Inactive" : "Active");
                        gvwWorker.Rows[rowIndex].Tag = caseworker;
                        if (caseworker.InActiveFlag == "Y")
                        {
                            gvwWorker.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                    //row_Cnt++;

                    //rowIndex = gvwWorker.Rows.Add(Img_Blank, caseworker.CaseWorker.ToString(), caseworker.HirarchyName.ToString(), "N");
                    //gvwWorker.Rows[rowIndex].Tag = caseworker;
                }
            }
            if (gvwWorker.Rows.Count > 0)
            {
                gvwWorker.Rows[rowsSel].Selected = true;
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = true;
            }
            else
            {
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkUnselectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnselectAll.Checked)
            {
                chkSelectAll.Checked = false;
                foreach (DataGridViewRow item in gvwWorker.Rows)
                {
                    item.Cells["Sel_Img"].Value = Img_Blank;
                    item.Cells["Selected"].Value = "N";
                }
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkUnselectAll.Checked = false;
                foreach (DataGridViewRow item in gvwWorker.Rows)
                {
                    item.Cells["Sel_Img"].Value = Img_Tick;
                    item.Cells["Selected"].Value = "Y";
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }

        private void gvsite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwWorker.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gvwWorker.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            gvwWorker.CurrentRow.Cells["Sel_Img"].Value = Img_Blank;
                            gvwWorker.CurrentRow.Cells["Selected"].Value = "N";
                            //Sel_Count--;
                        }
                        else
                        {
                            gvwWorker.CurrentRow.Cells["Sel_Img"].Value = Img_Tick;
                            gvwWorker.CurrentRow.Cells["Selected"].Value = "Y";
                            //Sel_Count++;
                        }
                        //if (Sel_Count > 30 && FormCode == "HSSB2106")
                        //{
                        //    gvwFundSource.CurrentRow.Cells["Sel_Img"].Value = Img_Blank;
                        //    gvwFundSource.CurrentRow.Cells["Selected"].Value = "N";
                        //    Sel_Count--;
                        //    MessageBox.Show("You may not select more than 30 services", "CAP Systems");
                        //}
                    }
                }
            }
        }

        public List<HierarchyEntity> GetSelectedWorkers()
        {
            List<HierarchyEntity> sele_Rooms_List = new List<HierarchyEntity>();
            foreach (DataGridViewRow dr in gvwWorker.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    HierarchyEntity Add_Entity = dr.Tag as HierarchyEntity;
                    sele_Rooms_List.Add(Add_Entity);
                }
            }
            return sele_Rooms_List;
        }

    }
}