#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

//using Gizmox.WebGUI.Common;
//using Wisej.Web;
using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MatScaleGroup : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion

        public MatScaleGroup(BaseForm baseForm, PrivilegeEntity privilieges, string strMatCode)
        {
            InitializeComponent();           
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            txtSeq.Validator = TextBoxValidation.IntegerValidator;
            this.Text = "Scale Groups"; //Privileges.Program;            
            Matcode = strMatCode;
            Get_Reasons_List();
        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }       

        public string Matcode { get; set; }
        string strMode { get; set; }
        private void picAddreason_Click(object sender, EventArgs e)
        {
            strMode = "I";
            picAddreason.Enabled = txtGroupDesc2.Enabled = true;
            txtSeq.Enabled = true;
            txtShortName.Enabled = true; 
            txtGroupDesc2.Clear();
            txtSeq.Clear();
            txtShortName.Clear();
            Reasons_Grid.Enabled = false;           
            Pb_Save_Reason.Visible = Pb_Cancel_Reason.Visible = true;
            txtShortName.Focus();
        }

        private void Pb_Save_Reason_Click(object sender, EventArgs e)
        {

            if (ReasonValidateForm(strMode))
            {
                Reasons_Grid.Enabled = true;
                picAddreason.Enabled = true;
                MATGroupEntity Search_Entity = new MATGroupEntity(true);
                Search_Entity.Type = strMode;
                Search_Entity.Desc = txtGroupDesc2.Text;
                Search_Entity.ShortName = txtShortName.Text;
                Search_Entity.Seq = txtSeq.Text;
                if(strMode=="U")
                    Search_Entity.Code = Reasons_Grid.CurrentRow.Cells["gvtCode"].Value.ToString();

                Search_Entity.MatCode = Matcode;              
               
                Search_Entity.Add_Operator = BaseForm.UserID;
                Search_Entity.Lstc_Operator = BaseForm.UserID;                
                string strmsg = string.Empty;
                if(_model.MatrixScalesData.INSERTUPDATEDELSCLGRPS(Search_Entity))
                {
                    ClearControls();
                    Get_Reasons_List();                    
                }

               
            }
        }

        public bool ReasonValidateForm(string Mode)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtGroupDesc2.Text.Trim()))
            {
                _errorProvider.SetError(txtGroupDesc2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResnDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtGroupDesc2, null);
            if (string.IsNullOrEmpty(txtShortName.Text.Trim()))
            {
                _errorProvider.SetError(txtShortName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblShortName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtShortName, null);
            if (string.IsNullOrEmpty(txtSeq.Text.Trim()))
            {
                _errorProvider.SetError(txtSeq, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSeq.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtSeq, null);
            if (Mode.ToUpper() == "I")
            {
                foreach (DataGridViewRow gvitem in Reasons_Grid.Rows)
                {
                    if (gvitem.Cells["gvtShortName"].Value.ToString().ToUpper() == txtShortName.Text.ToUpper())
                    {
                        isValid = false;
                        AlertBox.Show("Short Name already exists", MessageBoxIcon.Warning);
                        break;
                    }
                }
            }
            if (Mode.ToUpper() == "U")
            {
                int intcount = 0;
                foreach (DataGridViewRow gvitem in Reasons_Grid.Rows)
                {
                    if (gvitem.Cells["gvtShortName"].Value.ToString().ToUpper() == txtShortName.Text.ToUpper() && Reasons_Grid.CurrentRow.Cells["gvtCode"].Value.ToString() !=gvitem.Cells["gvtCode"].Value.ToString())
                    {
                        isValid = false;
                        AlertBox.Show("Short Name already exists", MessageBoxIcon.Warning);
                        break;
                    }
                }
            }
            return isValid;
        }

        private void Pb_Cancel_Reason_Click(object sender, EventArgs e)
        {
            strMode = string.Empty;
            picAddreason.Enabled = true;
            Reasons_Grid.Enabled = true;
            txtGroupDesc2.Enabled = false;
            txtSeq.Enabled = false;
            txtShortName.Enabled = false; 
            Pb_Save_Reason.Visible = Pb_Cancel_Reason.Visible = false;
            _errorProvider.SetError(txtGroupDesc2, null);
            if (Reasons_Grid.Rows.Count > 0)
            {
                txtGroupDesc2.Text = Reasons_Grid.CurrentRow.Cells["gvtDesc"].Value.ToString();               
                //if(string.IsNullOrEmpty(txtReasonDesc.Text))
                _errorProvider.SetError(txtGroupDesc2, null);
            }
        }

        private void Reasons_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvtEdit.Index && e.RowIndex != -1)
            {
                strMode = "U";
                txtGroupDesc2.Enabled = true;
                txtSeq.Enabled = true;
                txtShortName.Enabled = true; 
                Pb_Save_Reason.Visible = Pb_Cancel_Reason.Visible = true;
                Reasons_Grid.Enabled = false;
                txtGroupDesc2.Focus();
                picAddreason.Enabled = false;
            }
            else if (e.ColumnIndex == gvtDelete.Index && e.RowIndex != -1)
            {
               // Added_Edited_ReasonCode = string.Empty;
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: Delete_Selected_Reason);

            }
        }
        private void Delete_Selected_Reason(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                if (dialogResult == DialogResult.Yes)
                {
                    txtGroupDesc2.Text = string.Empty;
                  
                    MATGroupEntity Search_Entity = new MATGroupEntity();
                    Search_Entity.Type = "D";
                    Search_Entity.MatCode = Matcode.ToString();                   
                    Search_Entity.Code = Reasons_Grid.CurrentRow.Cells["gvtCode"].Value.ToString();                    
                    string strmsg = string.Empty;
                    if (_model.MatrixScalesData.INSERTUPDATEDELSCLGRPS(Search_Entity))
                    {                       
                        Get_Reasons_List();                       
                    }
                    ClearControls();
                }
           // }
        }

        private void Get_Reasons_List()
        {
            Reasons_Grid.SelectionChanged -=new EventHandler(Reasons_Grid_SelectionChanged);
            Reasons_Grid.Rows.Clear();
            List<MATGroupEntity> Group_List = _model.MatrixScalesData.GetSCLGRPS(Matcode, string.Empty);
            foreach (MATGroupEntity groupitem in Group_List)
            {
                Reasons_Grid.Rows.Add(groupitem.Code,groupitem.ShortName, groupitem.Desc,groupitem.Seq);
            }
            Reasons_Grid.SelectionChanged += new EventHandler(Reasons_Grid_SelectionChanged);
            Reasons_Grid_SelectionChanged(Reasons_Grid, new EventArgs());
        }
        public void ClearControls()
        {
            txtGroupDesc2.Enabled = false;
            txtSeq.Enabled = false;
            txtShortName.Enabled = false; 
            txtGroupDesc2.Clear();
            txtSeq.Clear();
            txtShortName.Clear();
            Pb_Save_Reason.Visible = false; Pb_Cancel_Reason.Visible = false;
        }
        private void Reasons_Grid_SelectionChanged(object sender, EventArgs e)
        {
            ReasonsControlsfill();
        }

        void ReasonsControlsfill()
        {
            if (Reasons_Grid.Rows.Count > 0)
            {
                txtGroupDesc2.Text = Reasons_Grid.CurrentRow.Cells["gvtDesc"].Value.ToString();
                txtShortName.Text = Reasons_Grid.CurrentRow.Cells["gvtShortName"].Value.ToString();
                txtSeq.Text = Reasons_Grid.CurrentRow.Cells["gvtSeq"].Value.ToString();    
            }
        }
    }
}