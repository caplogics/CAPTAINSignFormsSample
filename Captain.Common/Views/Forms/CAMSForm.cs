#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
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
    public partial class CAMSForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        //string Selmode = "View";
        public CAMSForm(BaseForm baseForm, string mode, string CAMS_code, string Tab_page, string BtnMode, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm;
            Privileges = privileges;
            button_Mode = BtnMode;
            Mode = mode;
            Tab_Page = Tab_page;
            if (Tab_page == "MS")
            {
                rbMS.Visible = false;
                rbOutcm.Visible = false;
                lblType.Visible = false;
                this.txtService.MaxLength = 10;
                lblSeviceCode.Text = "Outcome Code";
                //label2.Location = new Point(66, 11);
                Size = new Size(655,200);//(622, 200);
                pnlUnit.Visible = false;
            }
            else
            {
                rbMS.Visible = false;
                rbOutcm.Visible = false;
                lblType.Visible = false;
                this.txtService.MaxLength = 10;
                Size = new Size(655, 200);//(622, 200);
                pnlUnit.Visible = false;
            }
            _model = new CaptainModel();
            CaMs_Code = CAMS_code;
            //txtService.Validator = TextBoxValidation.IntegerValidator;
            this.Text = Tab_page + " " + "Master Maintaince";//privileges.PrivilegeName;

            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
                Member_Activity = dsAgency.Tables[0].Rows[0]["ACR_MEM_ACTIVTY"].ToString().Trim();

            Lbl_UOM.Visible = false;
            Cmb_UOM.Visible = false;
            lblTrans.Visible = false; txtTrans.Visible = false;
            if (Member_Activity == "Y" && Tab_Page == "CA")
            {
                Lbl_UOM.Visible = true;
                Cmb_UOM.Visible = true;
                pnlUnit.Visible = true;
                lblTrans.Visible = true; txtTrans.Visible = true;
                lblVendorpay.Visible = cmbVendor.Visible = false;
                //this.Lbl_UOM.Location = new System.Drawing.Point(282, 13);
                //this.Cmb_UOM.Location = new System.Drawing.Point(316, 12);

                Size = new Size(655, 240);

                this.Lbl_UOM.Location = new System.Drawing.Point(300, 15);//(350, 14);
                this.Cmb_UOM.Location = new System.Drawing.Point(350, 11);//(387, 9);
                this.Cmb_UOM.Size = new System.Drawing.Size(188, 25);

                Fill_UOM();

            }
            if (Tab_Page == "CA" && Member_Activity != "Y")
            {
                if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                {
                    FillVendorpaycategory();
                    lblVendorpay.Visible = true;
                    cmbVendor.Visible = true;
                    lblVendorpay.Location = new Point(300,15);
                    cmbVendor.Location = new Point(450,11);

                }
            }

            switch (Mode)
            {
                case "Edit":
                    //this.Text =  + " - Edit";//privileges.Program
                    if (Tab_Page == "CA")
                    {
                        if (button_Mode == "Copy")
                        {
                            this.Text = "Service Table" + " -  Add";
                            txtService.Enabled = true;
                            Mode = "Add";
                        }
                        else
                        {
                            this.Text = "Service Table" + " -  Edit";
                            txtService.Enabled = false;

                            if (BaseForm.BaseAgencyControlDetails.MemberActivity == "Y")
                                btnUnitPrice.Visible = true;
                        }

                        FillCriticalActivitesControls(CAMS_code);
                    }
                    else
                    {
                        if (button_Mode == "Copy")
                        {
                            this.Text = "Outcome Table"/*privileges.Program*/ + " - Add";
                            txtService.Enabled = true;
                            Mode = "Add";
                        }
                        else
                        {
                            this.Text = "Outcome Table"/*privileges.Program*/ + " - Edit";
                            txtService.Enabled = false;
                        }
                        FillMileStoneControls(CAMS_code);
                    }
                   
                    break;
                case "Add":
                    if (Tab_Page == "CA")
                    {
                        this.Text = "Service Table"/*privileges.Program*/ + " - Add";
                    }
                    else
                    {
                        this.Text = "Outcome Table"/*privileges.Program*/ + " - Add";
                    }
                 
                    break;
                case "Delete":
                    this.Text = privileges.Program + " - Delete";
                    break;
            }
            //txtcode.Text = SP_Code;
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Tab_Page { get; set; }

        public string CaMs_Code { get; set; }

        public string button_Mode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public bool IsSaveValid { get; set; }

        public string Member_Activity { get; set; }

        #endregion

        private void Fill_UOM()
        {

            Cmb_UOM.Items.Clear(); Cmb_UOM.ColorMember = "FavoriteColor";


            List<SPCommonEntity> UOMList = new List<SPCommonEntity>();
            UOMList = _model.SPAdminData.Get_AgyRecs_WithFilter("UOM", "A");

            if (UOMList.Count > 0)
            {
              

                UOMList = UOMList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            int SelIndx = 0; int RowInd = 1;
            foreach (SPCommonEntity Entity in UOMList)
            {
                if (Mode == "Edit" || (Mode == "Add" && Entity.Active.Equals("Y")))
                {

                    Cmb_UOM.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Black : Color.Red), Entity.Ext));
                   
                    if (Entity.Default == "Y")
                        SelIndx = RowInd;

                    RowInd++;
                }
            }
            Cmb_UOM.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));

            Cmb_UOM.SelectedIndex = SelIndx;

            List<CommonEntity> Gender = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.UOMTABLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetGender();

        }

        private void FillVendorpaycategory()
        {
            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "08004", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetJobTitle();
            cmbVendor.Items.Insert(0, new ListItem("Select One", "0"));            
            cmbVendor.SelectedIndex = 0;
            foreach (CommonEntity vendordata in commonEntity)
            {
                ListItem li = new ListItem(vendordata.Desc, vendordata.Code);
                cmbVendor.Items.Add(li);
                // if (Mode.Equals(Consts.Common.Add) && vendordata.Default.Equals("Y")) cmbVendor.SelectedItem = li;
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //if (Tab_Page == "CA")
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CAMS_Form");
            //else
            //    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "MSForm");
        }
        private void FillCriticalActivitesControls(string CAMS_code)
        {
            //List<CriticalActivityEntity> CAData = _model.CAMSTData.GetCADetails();
            List<CAMASTEntity> CAMASTList;
            CAMASTList = _model.SPAdminData.Browse_CAMAST("Code", CAMS_code, null, null);
            if (CAMASTList.Count > 0)
            {
                CAMASTEntity drca = CAMASTList[0];
                if (button_Mode == "Copy")
                    txtService.Text = string.Empty;
                else
                    txtService.Text = drca.Code.ToString();
                txtDesc.Text = drca.Desc.ToString();
                if (drca.Active.ToString() == "True")
                    chkbActive.Checked = true;
                else
                    chkbActive.Checked = false;

                if (drca.AutoPost.ToString() == "Y")
                    chkbAutoPost.Checked = true;
                else
                    chkbAutoPost.Checked = false;

                if (Member_Activity == "Y")
                {
                    CommonFunctions.SetComboBoxValue(Cmb_UOM, drca.UOM);
                    txtTrans.Text = drca.TransactionAlert.Trim();
                }

                if(BaseForm.BaseAgencyControlDetails.PaymentCategorieService=="Y")
                {
                    CommonFunctions.SetComboBoxValue(cmbVendor, drca.VendPaycat);
                }

            }
        }

        private void FillMileStoneControls(string CAMS_code)
        {
            List<MSMASTEntity> MSMASTlist;
            MSMASTlist = _model.SPAdminData.Browse_MSMAST("Code", CAMS_code, null, null, null);
            if (MSMASTlist.Count > 0)
            {
                MSMASTEntity drMs = MSMASTlist[0];
                if (button_Mode == "Copy")
                    txtService.Text = string.Empty;
                else
                    txtService.Text = drMs.Code.ToString();
                txtDesc.Text = drMs.Desc.ToString();
                if (drMs.Active.ToString() == "True")
                    chkbActive.Checked = true;
                else
                    chkbActive.Checked = false;

                if (drMs.AutoPost.ToString() == "Y")
                    chkbAutoPost.Checked = true;
                else
                    chkbAutoPost.Checked = false;

                if (drMs.Type1.ToString() == "M")
                {
                    rbMS.Checked = true;
                    rbOutcm.Checked = false;
                }
                else
                {
                    rbOutcm.Checked = true;
                    rbMS.Checked = false;
                }
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Tab_Page == "CA")
            {
                try
                {
                    if (ValidateForm())
                    {
                        CaptainModel model = new CaptainModel();
                        CAMASTEntity CAEntity = new CAMASTEntity();

                        CAEntity.Code = txtService.Text;
                        CAEntity.Desc = txtDesc.Text;
                        CAEntity.Active = chkbActive.Checked ? "1" : "0";
                        CAEntity.AutoPost = chkbAutoPost.Checked ? "Y" : "N";
                        if (Member_Activity == "Y")
                        {
                            CAEntity.UOM = ((ListItem)Cmb_UOM.SelectedItem).Value.ToString();
                            CAEntity.TransactionAlert = txtTrans.Text.Trim();
                        }
                        CAEntity.addoperator = BaseForm.UserID;
                        CAEntity.lstcOperator = BaseForm.UserID;
                        if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                        {
                            if (((ListItem)cmbVendor.SelectedItem).Value.ToString() != "0")
                                CAEntity.VendPaycat = ((ListItem)cmbVendor.SelectedItem).Value.ToString();
                        }

                        CAEntity.Mode = Mode;
                        if (_model.SPAdminData.InsertCaMAST(CAEntity))
                        {
                           
                        }
                       
                        if (Mode == "Add")
                        {
                           
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                           
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }

                        Mode = "View";

                       

                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                try
                {
                    if (ValidateForm())
                    {
                        CaptainModel model = new CaptainModel();
                        MSMASTEntity MSEntity = new MSMASTEntity();

                        MSEntity.Code = txtService.Text;
                        MSEntity.Desc = txtDesc.Text;
                        MSEntity.Active = chkbActive.Checked ? "1" : "0";
                        MSEntity.AutoPost = chkbAutoPost.Checked ? "Y" : "N";
                        //if (rbMS.Checked == true)
                        //    MSEntity.Type1 = "M";
                        //else
                            MSEntity.Type1 = "O";
                        MSEntity.addoperator = BaseForm.UserID;
                        MSEntity.lstcOperator = BaseForm.UserID;
                        MSEntity.Mode = Mode;

                        if (_model.SPAdminData.InsertMsMast(MSEntity))
                        {

                        }
                      
                        if (Mode == "Add")
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }

                        Mode = "View";
                      

                    }
                }
                catch (Exception ex)
                {
                }
            }

        }

        public bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtService.Text))
            {
                _errorProvider.SetError(txtService, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSeviceCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (isCodeExists(txtService.Text))
                {
                    _errorProvider.SetError(txtService, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblSeviceCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
               
                else
                {
                    _errorProvider.SetError(txtService, null);
                }
            }
            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
            {
                _errorProvider.SetError(txtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtDesc, null);
            }

            IsSaveValid = isValid;
            return (isValid);
        }

        public string[] GetSelected_CA_Code()
        {
            string[] Added_Edited_CACode = new string[2];

            Added_Edited_CACode[0] = txtService.Text;
            Added_Edited_CACode[1] = Mode;

            return Added_Edited_CACode;
        }

        public string[] GetSelected_MS_Code()
        {
            string[] Added_Edited_MSCode = new string[2];

            Added_Edited_MSCode[0] = txtService.Text;
            Added_Edited_MSCode[1] = Mode;

            return Added_Edited_MSCode;
        }

        private bool isCodeExists(string Code)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                if (Tab_Page == "CA")
                {
                    List<CAMASTEntity> CAMASTList;
                    CAMASTList = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
                    foreach (CAMASTEntity Entity in CAMASTList)
                    {
                        if (Entity.Code.Trim().TrimStart('0') == Code.Trim().TrimStart('0'))
                        {
                            isExists = true;
                        }
                    }
                   
                }
                else
                {
                    List<MSMASTEntity> MSMASTList;
                    MSMASTList = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
                    foreach (MSMASTEntity Entity in MSMASTList)
                    {
                        if (Entity.Code.Trim().TrimStart('0') == Code.Trim().TrimStart('0'))
                            isExists = true;
                    }
                }
            }
            return isExists;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtService_LostFocus(object sender, EventArgs e)
        {
           
        }

        private void InvokeFocusCommand(Control objControl)
        {
           
        }

        private void txtService_Leave(object sender, EventArgs e)
        {
           
        }

        private void btnUnitPrice_Click(object sender, EventArgs e)
        {
            CAPrices_Form Form = new CAPrices_Form(BaseForm, Privileges, txtService.Text, txtDesc.Text.Trim());
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.ShowDialog();
        }
    }
}