#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Report_Get_SaveParams_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;

        #endregion

        public Report_Get_SaveParams_Form(ControlCard_Entity card_entity, string operation_type)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Card_Entity = card_entity;
            Operation_Type = operation_type;
            SetControls_Basedon_OperationType();
        }

        public Report_Get_SaveParams_Form(ControlCard_Entity card_entity, string operation_type, BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm;
            Privileges = privileges;
            btnOverwrite.Visible = false;

            Card_Entity = card_entity;
            Operation_Type = operation_type;
            SetControls_Basedon_OperationType();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ControlCard_Entity Card_Entity { get; set; }

        public string Operation_Type { get; set; }

        #endregion

        string Sql_SP_Result_Message = string.Empty;

        private void SetControls_Basedon_OperationType()
        {
            switch (Operation_Type)
            {
                case "Save":
                    this.Text =/* Card_Entity.Scr_Code + " - */"Save Parameters";
                    Size = new Size(510, 168);//(470, 137);
                    //this.SaveParams_Panel.Location = new System.Drawing.Point(2, 37);
                    SaveParams_Panel.Visible = true;//LblSave_Header.Visible = true;
                    GetParams_Panel.Visible = false;
                    string params1 = CASB2012_AdhocForm._pastselParams;
                    if (params1 != "")
                    {
                        string[] existParams = new string[2];
                        existParams = params1.Split(',');
                        TxtID.Text = existParams[0].ToString().Trim();
                        TxtDesc.Text = existParams[1].ToString().Trim();
                    }

                    break;

                case "Get":
                    this.Text = /*Card_Entity.Scr_Code + " - */"Saved Parameters";
                    GetParams_Panel.Size = new Size(510, 308);
                    Size = new Size(510, 364);//(470, 353);
                    //this.LblGet_Header.Location = new System.Drawing.Point(5, 5);
                    GetParams_Panel.Visible = true;//LblGet_Header.Visible = true;
                    SaveParams_Panel.Visible = false;
                    Fill_Parameters_Grid();

                    break;
            }
        }


        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Card_Entity.Card_ID = TxtID.Text.Trim();

                ControlCard_Entity Tmp_card_entity = new ControlCard_Entity(true);
                List<ControlCard_Entity> Tmp_Card_List = new List<ControlCard_Entity>();
                Tmp_card_entity.Scr_Code = Card_Entity.Scr_Code;
                //Tmp_card_entity.Card_ID = TxtID.Text.Trim();
                Tmp_card_entity.Card_DESC = TxtDesc.Text.Trim();
                Tmp_card_entity.UserID = Card_Entity.UserID;
                Tmp_card_entity.Module = Card_Entity.Module;

                Tmp_Card_List = _model.AdhocData.Browse_CONTROLCARD(Tmp_card_entity, "Browse");

                if (Tmp_Card_List.Count > 0)
                {
                    MessageBox.Show("Do you want to Overwrite the existing Saved Parameters with this ID?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: AddAdditionalAward);
                }
                else
                {
                    Card_Entity.Card_DESC = TxtDesc.Text.Trim();
                    Card_Entity.Rowtype = "U";

                    if (_model.AdhocData.UpdateControl_Card(Card_Entity, out Sql_SP_Result_Message))
                    {
                        this.Close();
                        AlertBox.Show("Saved Successfully");
                    }
                }
            }
        }

        private void AddAdditionalAward(DialogResult dialogresult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogresult == DialogResult.No)
                return;

            Card_Entity.Card_DESC = TxtDesc.Text.Trim();
            Card_Entity.Rowtype = "U";

            if (_model.AdhocData.UpdateControl_Card(Card_Entity, out Sql_SP_Result_Message))
                this.Close();
            AlertBox.Show("Existing Saved Parameters Overwritten Successfully");
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            //if (String.IsNullOrEmpty(TxtID.Text.Trim()))
            //{
            //    _errorProvider.SetError(TxtID, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label1.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(TxtID, null);

            if (String.IsNullOrEmpty(TxtDesc.Text.Trim()))
            {
                _errorProvider.SetError(TxtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label2.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtDesc, null);

            return (isValid);
        }

        List<ControlCard_Entity> Card_List = new List<ControlCard_Entity>();
        private void Fill_Parameters_Grid()
        {
            Card_List = _model.AdhocData.Browse_CONTROLCARD(Card_Entity, "Browse");
            IDs_Grid.Rows.Clear();

            if (Card_List.Count > 0)
            {
                IDs_Grid.Rows.Clear();
                string Tmp_Date = string.Empty;

                foreach (ControlCard_Entity Entity in Card_List)
                {
                    Tmp_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.LSTC_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    IDs_Grid.Rows.Add(Entity.Card_ID, Entity.Card_DESC, Tmp_Date, Entity.ADHASSOC_Count);
                }
                Btn_Delete.Visible = Btn_Select.Visible = true;
            }
            else
                Btn_Delete.Visible = Btn_Select.Visible = false;
        }

        private void Btn_Select_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string Get_Common_Saved_Parameters()
        {
            string Saved_Params = string.Empty;

            if (Card_List.Count > 0)
            {
                foreach (ControlCard_Entity Entity in Card_List)
                {
                    if (Entity.Card_ID == IDs_Grid.CurrentRow.Cells["ID"].Value.ToString())
                    { Saved_Params = Entity.Card_1; break; }
                }
            }

            return Saved_Params;
        }


        public string[] Get_Adhoc_Saved_Parameters()
        {
            string[] Saved_Params = new string[2];
            Saved_Params[0] = Saved_Params[1] = string.Empty;

            if (Card_List.Count > 0)
            {
                foreach (ControlCard_Entity Entity in Card_List)
                {
                    if (Entity.Card_ID == IDs_Grid.CurrentRow.Cells["ID"].Value.ToString())
                    {
                        string pastSelParams = IDs_Grid.CurrentRow.Cells["ID"].Value.ToString() + "," + IDs_Grid.CurrentRow.Cells["Desc"].Value.ToString();
                        CASB2012_AdhocForm._pastselParams = pastSelParams;
                        Saved_Params[0] = Entity.Card_1;
                        Saved_Params[1] = Entity.Card_2;
                        break;
                    }
                }
            }
            return Saved_Params;
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {


            if (Card_List.Count > 0 && IDs_Grid.CurrentRow.Cells["Associations_Count"].Value.ToString() == "0")
            {
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected ID: " + IDs_Grid.CurrentRow.Cells["ID"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);
            }
            else
                AlertBox.Show("You can not delete ID: '" + IDs_Grid.CurrentRow.Cells["ID"].Value.ToString() + "' !!! \n Adhoc associations exists for this ID", MessageBoxIcon.Warning);
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogresult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            if (dialogresult == DialogResult.Yes)
            {
                foreach (ControlCard_Entity Entity in Card_List)
                {
                    if (Entity.Card_ID == IDs_Grid.CurrentRow.Cells["ID"].Value.ToString())
                    {
                        Card_Entity.Rowtype = "D";
                        Card_Entity.Card_ID = Entity.Card_ID;
                        Card_Entity.Card_DESC = Card_Entity.Card_1 = "Delete Selected ID";
                        if (_model.AdhocData.UpdateControl_Card(Card_Entity, out Sql_SP_Result_Message))
                        {
                            Card_Entity.Card_ID = Card_Entity.Card_DESC = null;
                            Fill_Parameters_Grid();
                        }
                        break;
                    }
                }
                AlertBox.Show("Deleted Successfully");
            }
        }

        //private void lnkOverwrite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
        //    Save_Entity.Scr_Code = Privileges.Program;
        //    Save_Entity.UserID = BaseForm.UserID;
        //    Save_Entity.Module = BaseForm.BusinessModuleID;
        //    Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
        //    Save_Form.FormClosed += new Form.FormClosedEventHandler(Get_Saved_Parameters);
        //    Save_Form.ShowDialog();
        //}

        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Controls();

                TxtID.Text = Saved_Parameters[0].ToString().Trim();
                TxtDesc.Text = Saved_Parameters[1].ToString().Trim();
                //RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);


                //RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[1]);

            }
        }

        public string[] Get_Adhoc_Saved_Controls()
        {
            string[] Saved_Params = new string[2];
            Saved_Params[0] = Saved_Params[1] = string.Empty;

            if (Card_List.Count > 0)
            {
                foreach (ControlCard_Entity Entity in Card_List)
                {
                    if (Entity.Card_ID == IDs_Grid.CurrentRow.Cells["ID"].Value.ToString())
                    {
                        Saved_Params[0] = Entity.Card_ID;
                        Saved_Params[1] = Entity.Card_DESC;
                        break;
                    }
                }
            }
            return Saved_Params;
        }

        private void btnOverwrite_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }

        private void IDs_Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}