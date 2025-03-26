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
using System.Data.SqlClient;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00134LoadCriteria : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion
        public HSS00134LoadCriteria(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            this.Text = privilegeEntity.PrivilegeName + " - Load Criteria";
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            if (BaseForm.UserProfile.FastLoad.Equals("Y"))
            {
                chkFastLoad.Checked = true;
            }
            else
            {
                chkFastLoad.Checked = false;
            }
            chkFastLoad_CheckedChanged(chkFastLoad, new EventArgs());
            if (BaseForm.UserProfile.LoadData.Equals("1"))
            {
                rdoThisYear.Checked = true;

            }
            else if (BaseForm.UserProfile.LoadData.Equals("2"))
            {
                rdoPrivious.Checked = true;
            }
            else
            {
                rdoAllYear.Checked = true;
            }
            if (BaseForm.UserProfile.CalcSBCB.Equals("Y"))
            {
                chkCalculateSBCB.Checked = true;
            }
            else
            {
                chkCalculateSBCB.Checked = false;
            }

            if (BaseForm.UserProfile.TaskSeq.Equals("1"))
                rdoTaskCode.Checked = true;
            else if (BaseForm.UserProfile.TaskSeq.Equals("2"))
                rdoTaksDesc.Checked = true;
            else
                rdoTaskSeq.Checked = true;
        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@PWR_EMPLOYEE_NO", BaseForm.UserID.ToString().TrimStart()));
            sqlParamList.Add(new SqlParameter("@PWRTYPE", "LoadCriteria"));
            string strSeq = string.Empty;
            if (rdoTaskSeq.Checked == true)
                strSeq = "3";
            else if (rdoTaksDesc.Checked == true)
                strSeq = "2";
            else if (rdoTaskCode.Checked == true)
                strSeq = "1";
            string strLoadData = string.Empty;
            if (rdoThisYear.Checked == true)
                strLoadData = "1";
            else if (rdoPrivious.Checked == true)
                strLoadData = "2";
            else if (rdoAllYear.Checked == true)
                strLoadData = "3";
            sqlParamList.Add(new SqlParameter("@PWR_TASK_SEQ", strSeq));
            sqlParamList.Add(new SqlParameter("@PWR_FAST_LOAD", chkFastLoad.Checked == true ? "Y" : "N"));
            sqlParamList.Add(new SqlParameter("@PWR_LOAD_DATA", strLoadData));
            sqlParamList.Add(new SqlParameter("@PWR_CALC_SBCB", chkCalculateSBCB.Checked == true ? "Y" : "N"));
            if (Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORD(sqlParamList))
            {
                BaseForm.UserProfile.TaskSeq = strSeq;
                BaseForm.UserProfile.CalcSBCB = chkCalculateSBCB.Checked == true ? "Y" : "N";
                BaseForm.UserProfile.FastLoad = chkFastLoad.Checked == true ? "Y" : "N";
                if(chkFastLoad.Checked ==false)
                BaseForm.UserProfile.LoadData = strLoadData;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void chkFastLoad_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFastLoad.Checked == true)
            {
                pnlFastload.Enabled = false;
                chkCalculateSBCB.Checked = false;
                chkCalculateSBCB.Enabled = false;

            }
            else
            {
                pnlFastload.Enabled = true;
                rdoAllYear.Checked = true;
                chkCalculateSBCB.Enabled = true;

            }
        }
    }
}