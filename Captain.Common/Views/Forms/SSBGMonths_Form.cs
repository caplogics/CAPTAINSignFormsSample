#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using Captain.Common.Views.UserControls;

#endregion
namespace Captain.Common.Views.Forms
{
    public partial class SSBGMonths_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public SSBGMonths_Form(BaseForm baseForm, string mode, string strType, string strID, string strParamSeq, string strTypeCode, string strHierchyCode, string strHierchydesc, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            strgroupcode = strParamSeq;
            TypeCode = strTypeCode;
            strSeqCode = strID;
            strHierachycode = strHierchyCode;
            FormType = strType;
            //lblHeader.Text = "Monthly Targets";
            this.Text = /*privilegeEntity.Program*/"Monthly Targets" + " - " + Mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            string strCode = strgroupcode;
             caseEligGEntity = _model.CaseSumData.GetSSBGParams(strHierachycode.Substring(0, 2), strHierachycode.Substring(3, 2), strHierachycode.Substring(6, 2));
            List<SSBGPARAMEntity> SelcaseEligGEntity = caseEligGEntity.FindAll(u => u.SSBGSeq.Equals(strSeqCode) && u.SSBGYear.Equals(strCode));
            foreach (var EligQDetails in SelcaseEligGEntity)
            {
                txtGroupDesc.Text = EligQDetails.SSBGDesc;
                txtGroupDesc.Text = EligQDetails.SSBGDesc.Trim();
                //txtYear.Text = EligQDetails.SSBGYear;
                //txtBudget.Text = EligQDetails.SSBGBudget;
                //txtAward.Text = EligQDetails.SSBGAward;
                if (!string.IsNullOrEmpty(EligQDetails.SSBGGPFrom.Trim()))
                {
                    dtpGPFrom.Checked = true;
                    dtpGPFrom.Text = EligQDetails.SSBGGPFrom;
                }
                if (!string.IsNullOrEmpty(EligQDetails.SSBGGPTo.Trim()))
                {
                    dtpGPFrom.Checked = true;
                    dtpGPTo.Text = EligQDetails.SSBGGPTo;
                }
                //dtpGPFrom.Text = EligQDetails.SSBGGPFrom;
                //dtpGPTo.Text = EligQDetails.SSBGGPTo;
                //if (!string.IsNullOrEmpty(EligQDetails.SSBGRPDate.Trim()))
                //{
                //    dtpRepDate.Checked = true;
                //    dtpRepDate.Text = EligQDetails.SSBGRPDate;
                //}
                //dtpRPFrom.Text = EligQDetails.SSBGRPFrom;
                //dtpRPTo.Text = EligQDetails.SSBGRPTo;
                //txtSeq.Text = EligQDetails.SSBGSeq;
            }
            PbHierarchies.Visible = false;
            txtHierarchy.Text = strHierchyCode;
            txtHierachydesc.Text = strHierchydesc;
            List<SSBGTYPESEntity> SSBGTypesall = _model.CaseSumData.GetSSBGTypes();
            SSBGTYPESEntity caseEligGroups = SSBGTypesall.Find(u => u.SBGTID.Equals(strSeqCode) && u.SBGTCode.Equals(TypeCode) && u.SBGTCodeSeq.Equals("0"));
            if (caseEligGroups != null)
                txtTDesc.Text = caseEligGroups.SBGTDesc.Trim();

            fillGrid();

            if (Mode == "View")
            {
                gvMonths.ReadOnly = true;
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }

        }

        #region Properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string Mode { get; set; }
        public string strgroupcode { get; set; }
        public string TypeCode { get; set; }
        public string TypeSeq { get; set; }
        public string strSeqCode { get; set; }
        public string FormType { get; set; }
        public string strHierachycode { get; set; }
        public string HiearchyCode { get; set; }
        public string strQuestionCode { get; set; }
        public List<SSBGPARAMEntity> caseEligGEntity { get; set; }


        #endregion


        private void fillGrid()
        {
            this.gvMonths.CellValueChanged -= new DataGridViewCellEventHandler(this.gvMonths_CellValueChanged);
            gvMonths.Rows.Clear();

            var start = dtpGPFrom.Value;
            var end = dtpGPTo.Value;

            SSBGMONTHSEntity SelRows = null;
            List<SSBGMONTHSEntity> MonthsEntity = _model.CaseSumData.GetSSBGMonths(strHierachycode.Substring(0, 2), strHierachycode.Substring(3, 2), strHierachycode.Substring(6, 2));
            List<SSBGMONTHSEntity> SelcaseEligGEntity = MonthsEntity.FindAll(u => u.SSBGSeq.Equals(strSeqCode) && u.SSBGYear.Equals(strgroupcode) && u.SSBGCode.Equals(TypeCode));

            // set end-date to end of month
            end = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

            var diff = Enumerable.Range(0, Int32.MaxValue)
                                 .Select(e => start.AddMonths(e))
                                 .TakeWhile(e => e <= end)
                                 .Select(e => e.ToString("MMMM"));
            int i = 0; int strMonth = start.Month; int StrtYr = start.Year;
            foreach (var item in diff)
            {
                strMonth = strMonth + i;
                if (strMonth > 12) { strMonth = 1; StrtYr = StrtYr + 1; }
                SelRows=SelcaseEligGEntity.Find(u=>u.Month.Equals(strMonth.ToString()) && u.SSBGYearValue.Equals(StrtYr.ToString()));
                if(SelRows!=null)
                    gvMonths.Rows.Add(item + " " + StrtYr, SelRows.CntlOblig, strMonth,StrtYr);
                else
                    gvMonths.Rows.Add(item + " " + StrtYr, string.Empty, strMonth, StrtYr);

                i=1;
            }
            this.gvMonths.CellValueChanged += new DataGridViewCellEventHandler(this.gvMonths_CellValueChanged);

            
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (gvMonths.Rows.Count > 0)
            {
                SSBGMONTHSEntity Entity = new SSBGMONTHSEntity();
                string strMsg = string.Empty;
                string strHierarchy = txtHierarchy.Text;
                Entity.SSBGAgency = strHierarchy.Substring(0, 2);
                Entity.SSBGDept = strHierarchy.Substring(3, 2);
                Entity.SSBGProgram = strHierarchy.Substring(6, 2);
                Entity.SSBGYear = strgroupcode;
                Entity.SSBGSeq = strSeqCode;
                Entity.SSBGCode = TypeCode;
                
                //Entity.SSBGRPFrom = dtpRPFrom.Value.ToShortDateString();
                //Entity.SSBGRPTo = dtpRPTo.Value.ToShortDateString();
                //if (dtpGPFrom.Checked)
                //    Entity.SSBGGPFrom = dtpGPFrom.Value.ToShortDateString();
                //if (dtpGPTo.Checked)
                //    Entity.SSBGGPTo = dtpGPTo.Value.ToShortDateString();
                //if (dtpRepDate.Checked)
                //    Entity.SSBGRPDate = dtpRepDate.Value.ToShortDateString();
                //Entity.SSBGBudget = txtBudget.Text.Trim();
                //Entity.SSBGAward = txtAward.Text.Trim();
                //Entity.SSBGDesc = txtGroupDesc.Text.Trim();
                Entity.AddOperator = BaseForm.UserID;
                Entity.LstcOperator = BaseForm.UserID;
                Entity.Mode = "Delete";

                _model.CaseSumData.InsertUpdateDelSSBGMonths(Entity);
                //{
                    //if (_model.CaseSumData.InsertUpdateDelSSBGParams(Entity, out strMsg))
                    foreach (DataGridViewRow dr in gvMonths.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr.Cells["gvTarget"].Value.ToString()))
                        {
                            Entity.Mode = "Add";
                            Entity.CntlOblig = dr.Cells["gvTarget"].Value.ToString();
                            Entity.Month = dr.Cells["gvMonthCode"].Value.ToString();
                            Entity.SSBGYearValue = dr.Cells["gvYear"].Value.ToString();

                            _model.CaseSumData.InsertUpdateDelSSBGMonths(Entity);
                        }
                    }

                    this.Close();
                AlertBox.Show("Saved Successfully");
                //}

               
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void gvMonths_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void gvMonths_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int intcolindex = gvMonths.CurrentCell.ColumnIndex;
                int introwindex = gvMonths.CurrentCell.RowIndex;

                string strCurrectValue = Convert.ToString(gvMonths.Rows[introwindex].Cells[intcolindex].Value);
                string Points = Convert.ToString(gvMonths.Rows[introwindex].Cells["gvTarget"].Value);

                if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                {
                    AlertBox.Show(Consts.Messages.NumericOnly, MessageBoxIcon.Warning);
                    //boolcellstatus = false; IsValid = false;
                    this.gvMonths.CellValueChanged -= new DataGridViewCellEventHandler(this.gvMonths_CellValueChanged);
                    gvMonths.Rows[introwindex].Cells["gvTarget"].Value = string.Empty;
                    this.gvMonths.CellValueChanged += new DataGridViewCellEventHandler(this.gvMonths_CellValueChanged);

                }
                else
                {
                    if(string.IsNullOrEmpty(strCurrectValue.Trim()))
                        gvMonths.Rows[introwindex].Cells["gvTarget"].Value = string.Empty;
                    //BudgetValue = 0;
                    //foreach (DataGridViewRow dr in gvMonths.Rows)
                    //{
                    //    if (!string.IsNullOrEmpty(dr.Cells["Budget"].Value.ToString().Trim()))
                    //        BudgetValue += int.Parse(dr.Cells["Budget"].Value.ToString().Trim());
                    //}

                    //txtExpAch.Text = BudgetValue.ToString();
                }
            }
        }

        private void SSBGMonths_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }
    }
}