/************************************************************************
 * Conversion On    :   12/14/2022      * Converted By     :   Kranthi
 * Modified On      :   12/14/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MAT00003DateChangeForm : Form
    {
        CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        public MAT00003DateChangeForm(BaseForm baseForm, PrivilegeEntity privileges, string strAssementDate, string Matcode)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            this.Text = Privileges.PrivilegeName.Trim() +" Change Dates";
            lblAssessmentdt1.Text = LookupDataAccess.Getdate(strAssementDate);
            propAssessmentDate = strAssementDate;
            propMatcode = Matcode;
            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propMatapdtslist = _model.MatrixScalesData.GETMatapdts(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, Matcode, string.Empty, string.Empty);

        }

        public BaseForm BaseForm { get; set; }
        public string propAssessmentDate { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<MATAPDTSEntity> propMatapdtslist { get; set; }
        public string propMatcode { get; set; }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (propMatapdtslist.Count > 0)
                {
                    bool boolsave = true;
                    foreach (MATAPDTSEntity matapdts in propMatapdtslist)
                    {
                        string strAltDate = LookupDataAccess.Getdate(matapdts.SSDate);
                        if (strAltDate == LookupDataAccess.Getdate(dtAssessmentDate.Value.ToShortDateString()))
                        {
                            CommonFunctions.MessageBoxDisplay("Date already exist choose another date");
                            boolsave = false;
                            break;
                        }
                    }
                    if (boolsave)
                    {
                        if (Captain.DatabaseLayer.MatrixDB.Updatechangematdates(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, propMatcode, propAssessmentDate, "A", LookupDataAccess.Getdate(dtAssessmentDate.Value.ToShortDateString()), BaseForm.UserID))
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
        }
        private bool ValidateForm()
        {
            bool isValid = true;
                if (string.IsNullOrWhiteSpace(dtAssessmentDate.Text))
                {
                    _errorProvider.SetError(dtAssessmentDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtAssessmentDate, null);
                }
            return isValid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}