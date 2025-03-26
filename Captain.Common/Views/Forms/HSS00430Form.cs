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
using Captain.Common.Views.UserControls;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00430Form : Form
    {
        private AlertCodes alertCodesUserControl = null;
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        public HSS00430Form(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm;
            Privileges = privileges;
            //this.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
            this.Text = BaseForm.BaseApplicationNo + "    " + BaseForm.BaseApplicationName;

            txtMZip.Validator = TextBoxValidation.IntegerValidator;
            txtMZipPlus.Validator = TextBoxValidation.IntegerValidator;
            txtFZip.Validator = TextBoxValidation.IntegerValidator;
            txtFZipplus.Validator = TextBoxValidation.IntegerValidator;
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            
            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);

            alertCodesUserControl = new AlertCodes(baseForm, privileges, programEntity);
            alertCodesUserControl.Dock = DockStyle.Fill;
            pnlAlertCode.Controls.Add(alertCodesUserControl);
           
            GetData();           
            ButtonsPrevilegs();            
            EnableDiableControls(false);
        }

        private void ButtonsPrevilegs()
        {

            if (Privileges.AddPriv.Equals("false"))
            {
                PbAdd.Visible = false;
            }
            else
            {
                if (propCASEBIODetails == null)
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
                if (propCASEBIODetails == null)
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
                if (propCASEBIODetails != null)
                {
                    PbDelete.Visible = true;
                }
                else
                {
                    PbDelete.Visible = false;
                }
            }
        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public CASEBIOEntitty propCASEBIODetails { get; set; }


        public void GetData()
        {
            
            CASEBIOEntitty caseBioEntity = _model.ChldMstData.GetCaseBioDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            propCASEBIODetails = caseBioEntity;
            if (caseBioEntity != null)
            {
                txtFirst.Text = caseBioEntity.BIO_MOT_FNAME;
                txtMLast.Text = caseBioEntity.BIO_MOT_LNAME;
                txtMStreet.Text = caseBioEntity.BIO_MOT_STREET;
                txtMCity.Text = caseBioEntity.BIO_MOT_CITY;
                txtMZip.Text = caseBioEntity.BIO_MOT_ZIP;
                txtMZipPlus.Text = caseBioEntity.BIO_MOT_ZIPPLUS;
                txtMState.Text = caseBioEntity.BIO_MOT_STATE;
                mskMPhone.Text = caseBioEntity.BIO_MOT_PHONE;
                mskMCell.Text = caseBioEntity.BIO_MOT_CELL;
                txtMRelation.Text = caseBioEntity.BIO_MOT_RELATION; 

                txtFFirst.Text = caseBioEntity.BIO_FAT_FNAME;
                txtFLast.Text = caseBioEntity.BIO_FAT_LNAME;
                txtFstreet.Text = caseBioEntity.BIO_FAT_STREET;
                txtFCity.Text = caseBioEntity.BIO_FAT_CITY;
                txtFZip.Text = caseBioEntity.BIO_FAT_ZIP;
                txtFZipplus.Text =  caseBioEntity.BIO_FAT_ZIPPLUS;
                txtFState.Text = caseBioEntity.BIO_FAT_STATE;
                mskFPhone.Text = caseBioEntity.BIO_FAT_PHONE;
                mskFCell.Text = caseBioEntity.BIO_FAT_CELL;
                txtFRelation.Text = caseBioEntity.BIO_FAT_RELATION;

            }
            else
            {
                txtFirst.Text = string.Empty;
                txtMLast.Text = string.Empty;
                txtMStreet.Text = string.Empty;
                txtMCity.Text = string.Empty;
                txtMZip.Text = string.Empty;
                txtMZipPlus.Text = string.Empty;
                txtMState.Text = string.Empty;
                mskMPhone.Text = string.Empty;
                mskMCell.Text = string.Empty;
                txtMRelation.Text = string.Empty;

                txtFFirst.Text = string.Empty;
                txtFLast.Text = string.Empty;
                txtFstreet.Text = string.Empty;
                txtFCity.Text = string.Empty;
                txtFZip.Text = string.Empty;
                txtFZipplus.Text = string.Empty;
                txtFState.Text = string.Empty;
                mskFPhone.Text = string.Empty;
                mskFCell.Text = string.Empty;
                txtFRelation.Text = string.Empty;

            }


        }

        private void PbEdit_Click(object sender, EventArgs e)
        {
            EnableDiableControls(true);
            txtFirst.Focus();
            PbEdit.Visible = false;
            PbDelete.Visible = false;
            btnCancel.Text = "Cancel";
        }

        private void PbDelete_Click(object sender, EventArgs e)
        {
            btnSave.Visible = false;
            
            if (propCASEBIODetails != null)
            {

                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxMstClicked);
            }
        }

        private void OnDeleteMessageBoxMstClicked(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (DialogResult.Yes == dialogResult)
            {

                CASEBIOEntitty caseBioEntity = new CASEBIOEntitty();
                caseBioEntity.BIO_AGENCY = BaseForm.BaseAgency;
                caseBioEntity.BIO_DEPT = BaseForm.BaseDept;
                caseBioEntity.BIO_PROGRAM = BaseForm.BaseProg;
                caseBioEntity.BIO_YEAR = BaseForm.BaseYear;
                caseBioEntity.BIO_APP_NO = BaseForm.BaseApplicationNo;
                caseBioEntity.BIO_MOT_FNAME = txtFirst.Text;
                caseBioEntity.BIO_MOT_LNAME = txtMLast.Text;
                caseBioEntity.BIO_MOT_STREET = txtMStreet.Text;
                caseBioEntity.BIO_MOT_CITY = txtMCity.Text;
                caseBioEntity.BIO_MOT_ZIP = txtMZip.Text;
                caseBioEntity.BIO_MOT_ZIPPLUS =  txtMZipPlus.Text;
                caseBioEntity.BIO_MOT_STATE = txtMState.Text;
                caseBioEntity.BIO_MOT_RELATION = txtMRelation.Text;

                caseBioEntity.BIO_FAT_FNAME = txtFFirst.Text;
                caseBioEntity.BIO_FAT_LNAME = txtFLast.Text;
                caseBioEntity.BIO_FAT_STREET = txtFstreet.Text;
                caseBioEntity.BIO_FAT_CITY = txtFCity.Text;
                caseBioEntity.BIO_FAT_ZIP = txtFZip.Text;
                caseBioEntity.BIO_FAT_ZIPPLUS =  txtFZipplus.Text;
                caseBioEntity.BIO_FAT_STATE = txtFState.Text;
                caseBioEntity.BIO_FAT_RELATION = txtFRelation.Text;

                caseBioEntity.BIO_LSTC_OPERATOR = BaseForm.UserID;
                caseBioEntity.BIO_ADD_OPERATOR = BaseForm.UserID;

                caseBioEntity.Mode = "Delete";
               if( _model.ChldMstData.InsertUpdateDelCASEBIO(caseBioEntity))
               {
                    GetData();                  
                    EnableDiableControls(false);
                    ButtonsPrevilegs();
                }
            }
           
        }


        private void PbAdd_Click(object sender, EventArgs e)
        {
            EnableDiableControls(true);
            PbAdd.Visible = false;
            txtFirst.Focus();
        }

        public void EnableDiableControls(bool boolvalue)
        {
            btnSave.Visible = boolvalue;
            btnSave.Enabled = boolvalue;
            txtFirst.Enabled = boolvalue;
            txtMLast.Enabled = boolvalue;
            txtMStreet.Enabled = boolvalue;
            txtMCity.Enabled = boolvalue;
            txtMZip.Enabled = boolvalue;
            txtMZipPlus.Enabled = boolvalue;
            txtMState.Enabled = boolvalue;
            mskMPhone.Enabled = boolvalue;
            mskMCell.Enabled = boolvalue;
            txtMRelation.Enabled = boolvalue;
            
            txtFFirst.Enabled = boolvalue;
            txtFLast.Enabled = boolvalue;
            txtFstreet.Enabled = boolvalue;
            txtFCity.Enabled = boolvalue;
            txtFZip.Enabled = boolvalue;
            txtFZipplus.Enabled = boolvalue;
            txtFState.Enabled = boolvalue;
            mskFPhone.Enabled = boolvalue;
            mskFCell.Enabled = boolvalue;
            txtFRelation.Enabled = boolvalue;
        }
       

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSS00430");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CASEBIOEntitty caseBioEntity = new CASEBIOEntitty();
            caseBioEntity.BIO_AGENCY = BaseForm.BaseAgency;
            caseBioEntity.BIO_DEPT = BaseForm.BaseDept;
            caseBioEntity.BIO_PROGRAM = BaseForm.BaseProg;
            caseBioEntity.BIO_YEAR = BaseForm.BaseYear;
            caseBioEntity.BIO_APP_NO = BaseForm.BaseApplicationNo;
            caseBioEntity.BIO_MOT_FNAME = txtFirst.Text ;
            caseBioEntity.BIO_MOT_LNAME =txtMLast.Text;
            caseBioEntity.BIO_MOT_STREET = txtMStreet.Text;
            caseBioEntity.BIO_MOT_CITY = txtMCity.Text;
            caseBioEntity.BIO_MOT_ZIP = txtMZip.Text;
            caseBioEntity.BIO_MOT_ZIPPLUS = txtMZipPlus.Text;
            caseBioEntity.BIO_MOT_STATE = txtMState.Text ;

            if (mskMPhone.Text.Length > 0)
            {
                //mskMPhone.Text = mskMPhone.Text.Replace(' ', '0') + SsnZeros(mskMPhone.TextLength, string.Empty);
                caseBioEntity.BIO_MOT_PHONE = mskMPhone.Text.Trim();
            }

            if (mskFPhone.Text.Length > 0)
            {
                //mskFPhone.Text = mskFPhone.Text.Replace(' ', '0') + SsnZeros(mskFPhone.TextLength, string.Empty);
                caseBioEntity.BIO_FAT_PHONE = mskFPhone.Text.Trim();
            }


            //caseBioEntity.BIO_FAT_PHONE=

            caseBioEntity.BIO_FAT_FNAME = txtFFirst.Text;
            caseBioEntity.BIO_FAT_LNAME = txtFLast.Text;
            caseBioEntity.BIO_FAT_STREET = txtFstreet.Text;
            caseBioEntity.BIO_FAT_CITY = txtFCity.Text;
            caseBioEntity.BIO_FAT_ZIP = txtFZip.Text;
            caseBioEntity.BIO_FAT_ZIPPLUS = txtFZipplus.Text;
            caseBioEntity.BIO_FAT_STATE = txtFState.Text;

            if (mskMCell.Text.Length > 0)
            {
                mskMCell.Text = mskMCell.Text.Replace(' ', '0') + SsnZeros(mskMCell.TextLength, string.Empty);
                caseBioEntity.BIO_MOT_CELL = mskMCell.Text;
            }

            if (mskFCell.Text.Length > 0)
            {
                mskFCell.Text = mskFCell.Text.Replace(' ', '0') + SsnZeros(mskFCell.TextLength, string.Empty);
                caseBioEntity.BIO_FAT_CELL = mskFCell.Text;
            }

            caseBioEntity.BIO_MOT_RELATION = txtMRelation.Text.Trim();
            caseBioEntity.BIO_FAT_RELATION = txtFRelation.Text.Trim();

            caseBioEntity.BIO_LSTC_OPERATOR = BaseForm.UserID;
            caseBioEntity.BIO_ADD_OPERATOR = BaseForm.UserID;
            caseBioEntity.Mode = string.Empty;
            if (_model.ChldMstData.InsertUpdateDelCASEBIO(caseBioEntity))
            {
                AlertBox.Show("Saved Succesfully");
                GetData();
                EnableDiableControls(false);
                ButtonsPrevilegs();
              
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (propCASEBIODetails == null)
                this.Close();
            else
            {
                if (btnCancel.Text == "Cancel")
                {
                    btnSave.Visible = false;
                 
                    GetData();
                    EnableDiableControls(false);
                    ButtonsPrevilegs();
                    btnCancel.Text = "Close";

                }
                else
                    this.Close();
            }
        }

        private string SsnZeros(int intLength, string strType)
        {
            string strValue = string.Empty;
            if (strType == "SSN")
            {
                switch (intLength)
                {
                    case 1:
                        strValue = "00000000";
                        break;
                    case 2:
                        strValue = "0000000";
                        break;
                    case 3:
                        strValue = "000000";
                        break;
                    case 4:
                        strValue = "00000";
                        break;
                    case 5:
                        strValue = "0000";
                        break;
                    case 6:
                        strValue = "000";
                        break;
                    case 7:
                        strValue = "00";
                        break;
                    case 8:
                        strValue = "0";
                        break;
                }
            }
            else
            {
                switch (intLength)
                {
                    case 1:
                        strValue = "000000000";
                        break;
                    case 2:
                        strValue = "00000000";
                        break;
                    case 3:
                        strValue = "0000000";
                        break;
                    case 4:
                        strValue = "000000";
                        break;
                    case 5:
                        strValue = "00000";
                        break;
                    case 6:
                        strValue = "0000";
                        break;
                    case 7:
                        strValue = "000";
                        break;
                    case 8:
                        strValue = "00";
                        break;
                    case 9:
                        strValue = "0";
                        break;
                }
            }
            return strValue;
        }

        private void HSS00430Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tlHelp") 
            {
                Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }
    }
}