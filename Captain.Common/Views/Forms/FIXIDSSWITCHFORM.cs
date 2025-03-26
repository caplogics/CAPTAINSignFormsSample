#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class FIXIDSSWITCHFORM : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;      
        private CaptainModel _model = null;


        #endregion
        public FIXIDSSWITCHFORM(string strScreenType, CaseSnpEntity snpentity, BaseForm baseForm, PrivilegeEntity privileges,string strSwitchType)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propScreenType = strScreenType;
            propSnpEntity = snpentity;
            BaseForm = baseForm;
            Privileges = privileges;
            propSwitchType = strSwitchType;

        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        public string propScreenType { get; set; }

        public string propSwitchType { get; set; }


        public CaseSnpEntity propSnpEntity { get; set; }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string strFamilyReason = string.Empty;
            string strClientReason = string.Empty;
            string strSSNReason = string.Empty;
            if (propScreenType=="F")
            {
                strFamilyReason = txtReason.Text;
            }
            if (propScreenType == "C")
            {
                strClientReason = txtReason.Text;
            }
            if (propScreenType == "S")
            {
                strSSNReason = txtReason.Text;
            }
            if (_model.CaseMstData.INSERTUPDATEFIXSNPAUDIT(propSnpEntity.Agency, propSnpEntity.Dept, propSnpEntity.Program, propSnpEntity.Year, propSnpEntity.App, propSnpEntity.FamilySeq, propScreenType, propSnpEntity.Ssno, propSnpEntity.ClaimSsno,  string.Empty, string.Empty, string.Empty, propSnpEntity.ClientId, strFamilyReason, strClientReason,strSSNReason,BaseForm.UserID,propSwitchType))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}