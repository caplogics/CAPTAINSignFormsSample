#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class IncrementExceptionForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;


        #endregion
        public IncrementExceptionForm(BaseForm baseform, PrivilegeEntity privileges,string  strStartDt,string strEndDt,string strHierarchy,string strAgency,string strDept,string strProgram,string strFamily1value)
        {
            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = privileges;            
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 0;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.Icon = null;
            propAgency = strAgency;
            propDept = strDept ;
            propProgram = strProgram;
            propHierchy = strHierarchy;
            propStartDt = strStartDt;
            propEndDt = strEndDt;
            propFamily1value = strFamily1value;
            txtFam1.Text = propFamily1value;
            txtHierachy.Text = strHierarchy;
            txtFam1.Validator = txtFam2.Validator = txtFam3.Validator = txtFam4.Validator= txtFam5.Validator= txtFam6.Validator= txtFam7.Validator= txtFam8.Validator= TextBoxValidation.FloatValidator;

            btnOk.Visible = false;
            GetData();
          
        }
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }
        public string propAgency { get; set; }
        public string propDept { get; set; }
        public string propProgram { get; set; }
        public string propHierchy { get; set; }
        public string propStartDt { get; set; }
        public string propEndDt { get; set; }
        public string propFamily1value { get; set; }

        public PovertyException propIncrementdata { get; set; }

        #endregion

        public void GetData()
        {
            propIncrementdata = _model.masterPovertyData.GetPovertyException(propAgency, propDept, propProgram,propStartDt, propEndDt,string.Empty,string.Empty);
            ButtonsPrevilegs();            
            if (propIncrementdata == null)
            {
                txtFam2.Text = txtFam3.Text = txtFam4.Text = txtFam5.Text = txtFam6.Text = txtFam7.Text = txtFam8.Text = string.Empty;
            }
            else
            { 
                txtFam2.Text = propIncrementdata.Exp2Value ;
                txtFam3.Text = propIncrementdata.Exp3Value;
                txtFam4.Text = propIncrementdata.Exp4Value;
                txtFam5.Text = propIncrementdata.Exp5Value;
                txtFam6.Text = propIncrementdata.Exp6Value;
                txtFam7.Text = propIncrementdata.Exp7Value;
                txtFam8.Text = propIncrementdata.Exp8Value;
            }
        }
        private void ButtonsPrevilegs()
        {

            if (Privileges.AddPriv.Equals("false"))
            {
                PbAdd.Visible = false;
            }
            else
            {
                if (propIncrementdata == null)
                {
                    PbAdd.Visible = true;                   
                }
                else
                    PbAdd.Visible = false;
            }

            if (Privileges.ChangePriv.Equals("false"))
            {
                PbEdit.Visible = false;
            }
            else
            {
                if (propIncrementdata == null)
                {
                    PbEdit.Visible = false;                    
                }
                else
                {
                    PbEdit.Visible = true;                   
                }
            }


            if (Privileges.DelPriv.Equals("false"))
            {
                PbDelete.Visible = false;               
            }
            else
            {
                if (propIncrementdata != null)
                {
                    PbDelete.Visible = true;
                }
                else
                {
                    PbDelete.Visible = false;
                }
            }
        }

        private void EnableDisableviewMode(bool booltruefalse)
        {            
             txtFam2.Enabled = txtFam3.Enabled = txtFam4.Enabled = txtFam5.Enabled = txtFam6.Enabled = txtFam7.Enabled = txtFam8.Enabled = booltruefalse;            
        }
      
        private void btnCancel_Click(object sender, EventArgs e)
        {           
                this.Close();           
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (validationForm())
            {
                PovertyException povertyData = new PovertyException();
                povertyData.Mode = string.Empty;
                povertyData.Agency = propAgency;
                povertyData.Dept = propDept;
                povertyData.Program = propProgram;
                povertyData.StartDate = propStartDt;
                povertyData.EndDate = propEndDt;
                povertyData.Exp1Value = txtFam1.Text;
                povertyData.Exp2Value = txtFam2.Text;
                povertyData.Exp3Value = txtFam3.Text;
                povertyData.Exp4Value = txtFam4.Text;
                povertyData.Exp5Value = txtFam5.Text;
                povertyData.Exp6Value = txtFam6.Text;
                povertyData.Exp7Value = txtFam7.Text;
                povertyData.Exp8Value = txtFam8.Text;
                povertyData.ExpAddOperator = BaseForm.UserID;
                povertyData.ExpLstcOperator = BaseForm.UserID;
                if (_model.masterPovertyData.InsertUpdateDelPovertyExp(povertyData))
                {

                    btnOk.Visible = false;
                    GetData();
                    EnableDisableviewMode(false);
                }
            }

        }

        public bool validationForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtFam1.Text.Trim()))
            {
                _errorProvider.SetError(txtFam1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam1.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam1, null);
            }
            if (String.IsNullOrEmpty(txtFam2.Text.Trim()))
            {
                _errorProvider.SetError(txtFam2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam2.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam2, null);
            }
            if (String.IsNullOrEmpty(txtFam3.Text.Trim()))
            {
                _errorProvider.SetError(txtFam3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam3.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam3, null);
            }
            if (String.IsNullOrEmpty(txtFam4.Text.Trim()))
            {
                _errorProvider.SetError(txtFam4, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam4.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam4, null);
            }
            if (String.IsNullOrEmpty(txtFam5.Text.Trim()))
            {
                _errorProvider.SetError(txtFam5, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam5.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam5, null);
            }
            if (String.IsNullOrEmpty(txtFam6.Text.Trim()))
            {
                _errorProvider.SetError(txtFam6, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam6.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam6, null);
            }
            if (String.IsNullOrEmpty(txtFam7.Text.Trim()))
            {
                _errorProvider.SetError(txtFam7, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam7.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam7, null);
            }
            if (String.IsNullOrEmpty(txtFam8.Text.Trim()))
            {
                _errorProvider.SetError(txtFam8, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFam8.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                _errorProvider.SetError(txtFam8, null);
            }
            return isValid;
        }

      

        private void PbEdit_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = true;
            btnOk.Visible = true;
            EnableDisableviewMode(true);
            PbEdit.Visible = false;
            PbDelete.Visible = false;
            pnlAdd.Visible = false;
            Size = new Size(520, 265);
            // btnCancel.Text = "Cancel";
        }

        private void PbDelete_Click(object sender, EventArgs e)
        {
            btnOk.Visible = false;

            if (propIncrementdata != null)
            {
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: OnDeleteMessageBoxMstClicked);
            }


        }

        private void OnDeleteMessageBoxMstClicked(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogResult == DialogResult.Yes)
            {
               
                propIncrementdata.Mode = "Delete";
                if (_model.masterPovertyData.InsertUpdateDelPovertyExp(propIncrementdata))
                {
                    GetData();                   

                }
            }
           
        }


        private void PbAdd_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = true;
            btnOk.Visible = true;
            EnableDisableviewMode(true);
            PbAdd.Visible = false;
            pnlAdd.Visible = false;
            Size = new Size(520,265);
        }

      
    }
}