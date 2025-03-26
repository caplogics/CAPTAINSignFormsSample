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
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MAT00002Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion

        public MAT00002Form(BaseForm baseForm, string mode, string matCode, string scrcode, string group, string qcode, string seq,string bmcode,string outrCode, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Mode = mode;
            Matcode = matCode;
            Scrcode = scrcode;
            Group = group;
            Qcode = qcode;
            Seq = seq;
            BMcode = bmcode;
            OutrCode = outrCode;
            //lblHeader.Text = privilegeEntity.PrivilegeName;
            this.Text = "Matrix / Scale Score Sheets" + " - " + Consts.Common.Add;
            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            if (Mode.Equals("Edit"))
            {
                this.Text = privilegeEntity.Program + " - " + Consts.Common.Edit;
            }
            fillcombo();
        }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Matcode { get; set; }

        public string Scrcode { get; set; }

        public string Group { get; set; }

        public string Qcode { get; set; }

        public string Seq { get; set; }

        public string BMcode { get; set; }

        public string OutrCode { get; set; }

        public bool IsSaveValid { get; set; }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (((ListItem)cmbResponse.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbResponse, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResponce.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbResponse, null);
            }

            //if (String.IsNullOrEmpty(txtCity.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCity.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(txtCity, null);
            //}

            //if (String.IsNullOrEmpty(txtState.Text.Trim()))
            //{

            //    _errorProvider.SetError(txtState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;

            //}
            //else
            //{
            //    if (txtState.TextLength < 2)
            //    {
            //        MessageBox.Show(Consts.Messages.PleaseEnterTwoCharacters);
            //        //_errorProvider.SetError(txtState, Consts.Messages.PleaseEnterTwoCharacters.GetMessage());
            //        isValid = false;
            //        //_errorProvider.SetError(txtState, string.Format(Consts.Messages.PleaseEnterTwoCharacters.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        //isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(txtState, null);
            //    }

            //}   

            return (isValid);
        }

        private void fillcombo()
        {
            
            List<MATQUESREntity> matresponceEntity = _model.MatrixScalesData.GETMATQUESR(Matcode, Scrcode,Group, Qcode, string.Empty);

            foreach (MATQUESREntity matresp in matresponceEntity)
            {
                cmbResponse.Items.Add(new ListItem(matresp.RespDesc, matresp.RespCode));
            }
            cmbResponse.Items.Insert(0, new ListItem("    ", "0"));
            cmbResponse.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                MATOUTREntity matoutResponce = new MATOUTREntity();

                matoutResponce.MatCode = Matcode;
                matoutResponce.SclCode = Scrcode;
                matoutResponce.BmCode = BMcode;
                matoutResponce.Code = OutrCode;
                matoutResponce.Question = Qcode;
                matoutResponce.RespCode = ((ListItem)cmbResponse.SelectedItem).Value.ToString();
                matoutResponce.AddOperator = BaseForm.UserID;
                matoutResponce.LstcOperator = BaseForm.UserID;

                bool boolsucess = false;

                if (Mode.Equals("Edit"))
                {
                    matoutResponce.Mode = "Edit";
                   // boolsucess = _model.MatrixScalesData.InsertUpdateDelZIPCODE(matoutResponce);
                    //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                }
                else
                {
                    matoutResponce.Mode = "Add";
                    //boolsucess = _model.MatrixScalesData.InsertUpdateDelZIPCODE(matoutResponce);
                    //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                }
                if (boolsucess)
                {
                    //ADMN0013 zipControl = BaseForm.GetBaseUserControl() as ADMN0013;
                    //if (zipControl != null)
                    //{
                    //    zipControl.RefreshGrid();
                    //}
                    this.Close();
                }
                this.Close();

            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}