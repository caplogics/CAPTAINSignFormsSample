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
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class BrowseApplicantForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;


        #endregion
        public BrowseApplicantForm(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Mode = mode;
            propAgency =BaseForm.BaseAgency;
            propDept = BaseForm.BaseDept;
            propProg = BaseForm.BaseProg;
            propYear = string.Empty;
            if(Mode=="Edit")
            {
                btnSearch.Visible = false;
            }
            
        }

        public BrowseApplicantForm(BaseForm baseForm, string mode, PrivilegeEntity privilegeEntity,string Agency,string Dept,string Prog,string Year)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Mode = mode;
            propAgency =Agency;
            propDept =Dept;
            propProg =Prog;
            propYear = Year;
            fillGrid();
            
            if (Mode == "Edit")
            {
                btnSearch.Visible = false;
            }

        }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public CaseMstEntity MstData { get; set; }
        public string propAgency { get; set; }
        public string propDept { get; set; }
        public string propProg { get; set; }
        public string propYear { get; set; }
       

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (gvwMstDetails.Rows.Count > 0)
            {
                if (gvwMstDetails.SelectedRows[0].Selected)
                {
                    MstData = gvwMstDetails.SelectedRows[0].Tag as  CaseMstEntity;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("Select Applicant");
                }
            }       

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            gvwMstDetails.Rows.Clear();
            fillGrid();
            btnSelect.Visible = true;
            string strssno = string.Empty;
            string strName = string.Empty;
            string strApplNo = string.Empty;
            if (txtFirstName.Text.Trim() != string.Empty)
                strName = txtFirstName.Text;
            if (txtApplicantNo.Text.Trim() != string.Empty)
                strApplNo = txtApplicantNo.Text;
            if (mskSSN.Text.Trim() != string.Empty)
                strssno = mskSSN.Text;

            List<CaseMstEntity> casemstlist = _model.CaseMstData.GetCaseMstAll(propAgency, propDept, propProg, propYear, strApplNo, strssno, strName, string.Empty, string.Empty, "MSTALLSNP");
            if (casemstlist.Count > 0)
            {
                foreach (CaseMstEntity item in casemstlist)
                {
                    int rowIndex = gvwMstDetails.Rows.Add(item.ApplNo, item.LastName + "  " + item.FirstName, item.Hn + item.Street + item.Suffix, item.City, item.State);
                    gvwMstDetails.Rows[rowIndex].Tag = item;
                }
            }
            else
            {
                btnSelect.Visible = false;
                CommonFunctions.MessageBoxDisplay("No records found");
            }
            if (gvwMstDetails.Rows.Count > 0)
                gvwMstDetails.Rows[0].Selected = true;
        }

        void fillGrid()
        {
            string strssno = string.Empty;
            string strName = string.Empty;
            string strApplNo = string.Empty;
            if (txtFirstName.Text.Trim() != string.Empty)
                strName = txtFirstName.Text;
            if (txtApplicantNo.Text.Trim() != string.Empty)
                strApplNo = txtApplicantNo.Text;
            if (mskSSN.Text.Trim() != string.Empty)
                strssno = mskSSN.Text;


            List<CaseMstEntity> casemstlist = _model.CaseMstData.GetCaseMstAll(propAgency, propDept, propProg, propYear, strApplNo, strssno, strName, string.Empty, string.Empty, "MSTALLSNP");
            if (casemstlist.Count > 0)
            {
                foreach (CaseMstEntity item in casemstlist)
                {
                    int rowIndex = gvwMstDetails.Rows.Add(item.ApplNo, item.LastName + "  " + item.FirstName, item.Hn + item.Street + item.Suffix, item.City, item.State);
                    gvwMstDetails.Rows[rowIndex].Tag = item;
                }
            }
            //else
            //{
            //    btnSelect.Visible = false;
            //    CommonFunctions.MessageBoxDisplay("No records found");
            //}
            if (gvwMstDetails.Rows.Count > 0)
                gvwMstDetails.Rows[0].Selected = true;
        }

        

        private void txtApplicantNo_Leave(object sender, EventArgs e)
        {
            if (txtApplicantNo.Text.Trim() != string.Empty)
            {
                txtApplicantNo.Text = SetLeadingZeros(txtApplicantNo.Text);
            }
        }
        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }
            return (TmpCode);
        }

        private void gvwMstDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}