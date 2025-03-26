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
    public partial class HSS00137Form : Form
    {
         #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion
       
        public HSS00137Form(BaseForm baseForm,PrivilegeEntity privilegeEntity,string strOther,string strTime,string strBillMeals,string strOtherCode)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();           
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propOtherCode = string.Empty;
            lblOtherReq.Visible = false;
            if (strOtherCode == "4")
            {
                lblOtherReq.Visible = true;
                propOtherCode = strOtherCode;
                txtOther.Text = strOther;
                txtOther.ReadOnly = false;
            }
            this.Text = /*privilegeEntity.Program"Attendance Posting By Site" +*/ "Attendance";
            if (strTime == "F")
                rdoFull.Checked = true;
            if (strTime == "1")
                rdo1.Checked = true;
            if (strTime == "2")
                rdo2.Checked = true;
            if (strTime == "3")
                rdo3.Checked = true;
            if (strTime == string.Empty)
            {
                rdoFull.Enabled = false;
                rdo1.Enabled = false;
                rdo2.Enabled = false;
                rdo3.Enabled = false;                
            }
           // chkBillFormeals.Checked = strBillMeals == "Y" ? true : false;
        }

        public BaseForm BaseForm { get; set; }
        public string propBillForMeals { get; set; }
        public string propTime { get; set; }
        public string propOther { get; set; }
        public string propOtherCode { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                propOther = string.Empty;
                propTime = string.Empty;
                if (rdo1.Checked == true)
                    propTime = "1";
                if (rdo2.Checked == true)
                    propTime = "2";
                if (rdo3.Checked == true)
                    propTime = "3";
                if (rdoFull.Checked == true)
                    propTime = "F";
                propOther = txtOther.Text;
                //  propBillForMeals = chkBillFormeals.Checked == true ? "Y" : "N";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
        
         private bool ValidateForm()
        {
            bool isValid = true;

            if (propOtherCode == "4")
            {
                if (String.IsNullOrEmpty(txtOther.Text))
                {
                    _errorProvider.SetError(txtOther, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblOther.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {

                    _errorProvider.SetError(txtOther, null);

                }
            }
            return isValid;
        }
    }
}