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
using Captain.Common.Views.Controls.Compatibility;
//using Gizmox.WebGUI.Common.Interfaces;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class INKIND20_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        public INKIND20_Form(BaseForm baseform, string mode, string FormName, string Code, PrivilegeEntity privilegeEntity, string Seq)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privilegeEntity;
            Mode = mode;
            IKM_Code = Code;
            FormType = FormName;
            IKD_Seq = Seq;

            FillCombos();
            if (FormType == "Donor")
            {
               // this.Size = new System.Drawing.Size(670, 311);
                panel2.Visible = false; panel1.Visible = true;
                this.Size = new System.Drawing.Size(this.Width, this.Height - panel2.Height);
               // panel1.Location = new System.Drawing.Point(1, 55);
                //pnlHeader.Size = new System.Drawing.Size(667, 55);
                //pictureBox2.Location = new System.Drawing.Point(642, 20);
                //txtCode.Validator = TextBoxValidation.IntegerValidator;
                this.Text = Privileges.PrivilegeName;
                if (Mode == "Add")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
                    txtCode.Text = "New";
                    txtCode.Enabled = false;
                    btnAppSearch.Visible = true;
                
                }
                else if (Mode == "Edit")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                    txtCode.Enabled = false;
                    FillControls();
                }
                else if (Mode == "View")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.View;
                    txtCode.Enabled = false;
                    FillControls();
                    FillEnableDisableFields();
                }
                else if (Mode == "CopyFrom")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
                    FillControls();
                    txtCode.Text = "New";
                    txtCode.Enabled = false;
                    Mode = "Add";

                }
            }
            else
            {

               // this.Size = new System.Drawing.Size(545, 450);
                panel2.Visible = true; panel1.Visible = false;
                this.Size = new System.Drawing.Size(this.Width, this.Height - panel1.Height);
                // panel2.Location = new System.Drawing.Point(1, 55);
                //pnlHeader.Size = new System.Drawing.Size(540, 55);
                //pictureBox2.Location = new System.Drawing.Point(469, 20);
                txtPTime.Validator = TextBoxValidation.FloatValidator;
                txtPSTime.Validator = TextBoxValidation.FloatValidator;
                txtMileage.Validator = TextBoxValidation.FloatValidator;
                txtRTMDriven.Validator = TextBoxValidation.FloatValidator;
                txtTotServices.Validator = TextBoxValidation.FloatValidator;
                txtInkind.Validator = TextBoxValidation.FloatValidator;
                this.Text = "Service Activity";
                if (Mode == "Add")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
                    //txtCode.Text = "New";
                    //txtCode.Enabled = false;
                }
                else if (Mode == "Edit")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                    //txtCode.Enabled = false;
                    FillControls();
                }
                else if (Mode == "View")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.View;
                    txtCaseNotes.ReadOnly = true;
                    //txtCode.Enabled = false;
                    FillControls();
                    FillEnableDisableFields();
                }

            }

        }

        public INKIND20_Form(BaseForm baseform, string mode, string FormName, string Code, PrivilegeEntity privilegeEntity, string Seq, string TypeFrom)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privilegeEntity;
            Mode = mode;
            IKM_Code = Code;
            FormType = FormName;
            IKD_Seq = Seq;

            FillCombos();
            if (FormType == "Donor")
            {
                //this.Size = new System.Drawing.Size(670, 311);
                panel2.Visible = false; panel1.Visible = true;
                this.Size = new System.Drawing.Size(this.Width, this.Height - panel2.Height);
                //panel1.Location = new System.Drawing.Point(1, 55);
                //pnlHeader.Size = new System.Drawing.Size(667, 55);
                //pictureBox2.Location = new System.Drawing.Point(642, 20);
                //txtCode.Validator = TextBoxValidation.IntegerValidator;
                this.Text = Privileges.PrivilegeName;
                if (Mode == "Add")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
                    txtCode.Text = "New";
                    txtCode.Enabled = false;
                }
                else if (Mode == "Edit")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                    txtCode.Enabled = false;
                    FillControls();
                }
                else if (Mode == "View")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.View;
                    txtCode.Enabled = false;
                    FillControls();
                    FillEnableDisableFields();
                }
                else if (Mode == "CopyFrom")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
                    FillControls();
                    txtCode.Text = "New";
                    txtCode.Enabled = false;
                    Mode = "Add";

                }
            }
            else
            {

                //this.Size = new System.Drawing.Size(545, 450);
                panel2.Visible = true; panel1.Visible = false;
                this.Size = new System.Drawing.Size(this.Width, this.Height - panel1.Height);
                //panel2.Location = new System.Drawing.Point(1, 55);
                //pnlHeader.Size = new System.Drawing.Size(540, 55);
                //pictureBox2.Location = new System.Drawing.Point(469, 20);
                txtPTime.Validator = TextBoxValidation.FloatValidator;
                txtPSTime.Validator = TextBoxValidation.FloatValidator;
                txtMileage.Validator = TextBoxValidation.FloatValidator;
                txtRTMDriven.Validator = TextBoxValidation.FloatValidator;
                txtTotServices.Validator = TextBoxValidation.FloatValidator;
                txtInkind.Validator = TextBoxValidation.FloatValidator;
                this.Text = "Service Activity";
                if (Mode == "Add")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
                    //txtCode.Text = "New";
                    //txtCode.Enabled = false;
                }
                else if (Mode == "Edit")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                    //txtCode.Enabled = false;
                    FillControls();
                }
                else if (Mode == "View")
                {
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.View;
                    txtCaseNotes.ReadOnly = true;
                    //txtCode.Enabled = false;
                    FillControls();
                    FillEnableDisableFields();
                }

            }

        }


        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        public string FormType { get; set; }

        public string IKM_Code { get; set; }
        public string IKD_Seq { get; set; }
        public bool IsSaveValid { get; set; }



        List<AGYTABSEntity> AGYTABSProfile = new List<AGYTABSEntity>();
        private void FillCombos()
        {

            if (FormType == "Donor")
            {
                List<CommonEntity> Gender = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00019", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                CmbGender.Items.Insert(0, new ListItem("", "0"));
                CmbGender.ColorMember = "FavoriteColor";
                CmbGender.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Gender)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    CmbGender.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) CmbGender.SelectedItem = li;
                }

                List<CommonEntity> Ethncity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00352", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbEthenCity.Items.Insert(0, new ListItem("", "0"));
                cmbEthenCity.ColorMember = "FavoriteColor";
                cmbEthenCity.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Ethncity)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbEthenCity.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbEthenCity.SelectedItem = li;
                }

                List<CommonEntity> Race = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00003", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbRace.Items.Insert(0, new ListItem("", "0"));
                cmbRace.ColorMember = "FavoriteColor";
                cmbRace.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Race)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbRace.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbRace.SelectedItem = li;
                }


                List<CommonEntity> EduLevel = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00007", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbEduLevel.Items.Insert(0, new ListItem("", "0"));
                cmbEduLevel.ColorMember = "FavoriteColor";
                cmbEduLevel.SelectedIndex = 0;
                foreach (CommonEntity Etncity in EduLevel)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbEduLevel.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbEduLevel.SelectedItem = li;
                }

                List<CommonEntity> Disabled = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00002", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbDisabled.Items.Insert(0, new ListItem("", "0"));
                cmbDisabled.ColorMember = "FavoriteColor";
                cmbDisabled.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Disabled)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbDisabled.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbDisabled.SelectedItem = li;
                }

                List<CommonEntity> Veteran = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00021", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbVeteran.Items.Insert(0, new ListItem("", "0"));
                cmbVeteran.ColorMember = "FavoriteColor";
                cmbVeteran.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Veteran)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbVeteran.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbVeteran.SelectedItem = li;
                }

                List<CommonEntity> Farmer = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00022", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbFarmer.Items.Insert(0, new ListItem("", "0"));
                cmbFarmer.ColorMember = "FavoriteColor";
                cmbFarmer.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Farmer)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbFarmer.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbFarmer.SelectedItem = li;
                }

                List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0001", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbHSParent.Items.Insert(0, new ListItem("", "0"));
                cmbHSParent.ColorMember = "FavoriteColor";
                cmbHSParent.SelectedIndex = 0;
                cmbUSCitizen.Items.Insert(0, new ListItem("", "0"));
                cmbUSCitizen.ColorMember = "FavoriteColor";
                cmbUSCitizen.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Ethnicity)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbHSParent.Items.Add(li); cmbUSCitizen.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) { cmbHSParent.SelectedItem = li; cmbUSCitizen.SelectedItem = li; }
                }

                List<CommonEntity> DonorType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "05555", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbDonorType.Items.Insert(0, new ListItem("", "NA"));
                cmbDonorType.ColorMember = "FavoriteColor";
                cmbDonorType.SelectedIndex = 0;
                foreach (CommonEntity Etncity in DonorType)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbDonorType.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbDonorType.SelectedItem = li;
                }

            }
            else
            {
                List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "05558", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
                cmbService.Items.Insert(0, new ListItem("Select One", "0"));
                cmbService.ColorMember = "FavoriteColor";
                cmbService.SelectedIndex = 0;
                foreach (CommonEntity Etncity in Ethnicity)
                {
                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                    cmbService.Items.Add(li);
                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbService.SelectedItem = li;
                }
                //cmbService.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            }

            //AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            ////searchAgytabs.Tabs_Type = "08004";
            //List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);
            //if (AgyTabs_List.Count > 0)
            //{
            //    foreach (AGYTABSEntity item in AgyTabs_List)
            //    {
            //        if (FormType == "Donor")
            //        {
            //            if (item.Tabs_Type == "00019")
            //            {

            //                CmbGender.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            }
            //            if (item.Tabs_Type == "00352")
            //                cmbEthenCity.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            if (item.Tabs_Type == "00003")
            //                cmbRace.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            if (item.Tabs_Type == "00007")
            //                cmbEduLevel.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            if (item.Tabs_Type == "00002")
            //                cmbDisabled.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            if (item.Tabs_Type == "00021")
            //                cmbVeteran.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            if (item.Tabs_Type == "00022")
            //            {

            //                cmbFarmer.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            }
            //            if (item.Tabs_Type == "S0001")
            //                cmbUSCitizen.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            if (item.Tabs_Type == "S0001")
            //            {

            //                cmbHSParent.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            }
            //            if (item.Tabs_Type == "05555")
            //            {

            //                cmbDonorType.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            }
            //        }
            //        else
            //        {
            //            if (item.Tabs_Type == "05558")
            //            {
            //                List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "05558", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            //                cmbService.Items.Insert(0, new ListItem("Select One", "0"));
            //                cmbService.ColorMember = "FavoriteColor";
            //                cmbService.SelectedIndex = 0;
            //                foreach (CommonEntity Etncity in Ethnicity)
            //                {
            //                    ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
            //                    cmbService.Items.Add(li);
            //                    if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbService.SelectedItem = li;
            //                }
            //                //cmbService.Items.Add(new ListItem(item.Code_Desc, item.Table_Code));
            //            }
            //        }
            //    }

            //    CmbGender.Items.Insert(0, new ListItem("", "0"));
            //    CmbGender.SelectedIndex = 0;
            //    cmbEthenCity.Items.Insert(0, new ListItem("", "0"));
            //    cmbEthenCity.SelectedIndex = 0;
            //    cmbRace.Items.Insert(0, new ListItem("", "0"));
            //    cmbRace.SelectedIndex = 0;
            //    cmbEduLevel.Items.Insert(0, new ListItem("", "0"));
            //    cmbEduLevel.SelectedIndex = 0;
            //    cmbDisabled.Items.Insert(0, new ListItem("", "0"));
            //    cmbDisabled.SelectedIndex = 0;
            //    cmbVeteran.Items.Insert(0, new ListItem("", "0"));
            //    cmbVeteran.SelectedIndex = 0;
            //    cmbFarmer.Items.Insert(0, new ListItem("", "0"));
            //    cmbFarmer.SelectedIndex = 0;

            //    cmbUSCitizen.Items.Insert(0, new ListItem("", "0"));
            //    cmbUSCitizen.SelectedIndex = 0;

            //    cmbHSParent.Items.Insert(0, new ListItem("", "0"));
            //    cmbHSParent.SelectedIndex = 0;

            //    //cmbService.Items.Insert(0, new ListItem("", "0"));
            //    //cmbService.SelectedIndex = 0;

            //    ////DataSet dsDonor = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.DONORTYPE);
            //    ////DataTable dtDonor = dsDonor.Tables[0];
            //    ////List<ListItem> listlang = new List<ListItem>();
            //    //cmbDonorType.Items.Insert(0, new ListItem("", "NA"));
            //    ////foreach (DataRow drDonor in dtDonor.Rows)
            //    ////{
            //    ////    cmbDonorType.Items.Add(new ListItem(drDonor["LookUpDesc"].ToString().Trim(), drDonor["Code"].ToString().Trim()));
            //    ////}
            //    //cmbDonorType.SelectedIndex = 0;
            //}
        }

        private void FillEnableDisableFields()
        {
            if (FormType == "Donor")
            {
                txtCode.Enabled = false;
                cmbDonorType.Enabled = false;
                txtDName.Enabled = false;
                txtFName.Enabled = false;
                txtLName.Enabled = false;
                txtAddress.Enabled = false;
                maskPhone.Enabled = false; dtpDOB.Enabled = false;
                cmbDisabled.Enabled = false;
                cmbDonorType.Enabled = false;
                cmbEduLevel.Enabled = false; cmbEthenCity.Enabled = false;
                cmbFarmer.Enabled = false; CmbGender.Enabled = false;
                cmbHSParent.Enabled = false; cmbRace.Enabled = false;
                cmbUSCitizen.Enabled = false; cmbVeteran.Enabled = false;
                btnSave.Visible = false; btnCancel.Visible = false;
            }
            else
            {
                cmbService.Enabled = false;
                dtpDate.Enabled = false;
                dtpStartTime.Enabled = false; dtpEndtime.Enabled = false;
                txtInkind.Enabled = false; txtMileage.Enabled = false;
                txtPSTime.Enabled = false; txtPTime.Enabled = false;
                txtRTMDriven.Enabled = false; txtTotServices.Enabled = false;
                btnSSave.Visible = false; btnSCancel.Visible = false;
            }
        }


        private void FillControls()
        {
            if (FormType == "Donor")
            {
                INKINDMSTEntity Search_MST = new INKINDMSTEntity(true);
                Search_MST.Agency = BaseForm.BaseAgency; Search_MST.CODE = IKM_Code;
                List<INKINDMSTEntity> MST_List = _model.INKINDData.Browse_INKINDMST(Search_MST, "Browse");
                if (MST_List.Count > 0)
                {
                    txtCode.Text = MST_List[0].CODE.Trim();
                    SetComboBoxValue(cmbDonorType, MST_List[0].TYPE.Trim());
                    txtDName.Text = MST_List[0].DONOR_NAME.Trim();
                    txtFName.Text = MST_List[0].CHILD_FNAME.Trim();
                    txtLName.Text = MST_List[0].CHILD_LNAME.Trim();
                    txtAddress.Text = MST_List[0].ADDRESS.Trim();
                    SetComboBoxValue(CmbGender, MST_List[0].SEX.Trim());
                    maskPhone.Text = MST_List[0].PHONE.Trim();
                    if (!string.IsNullOrEmpty(MST_List[0].DOB.Trim()))
                    {
                        dtpDOB.Checked = true;
                        dtpDOB.Text = MST_List[0].DOB.Trim();
                    }
                    SetComboBoxValue(cmbEthenCity, MST_List[0].ETHNICITY.Trim());
                    SetComboBoxValue(cmbRace, MST_List[0].RACE.Trim());
                    SetComboBoxValue(cmbEduLevel, MST_List[0].EDUCATION.Trim());
                    SetComboBoxValue(cmbDisabled, MST_List[0].DISABLED.Trim());
                    SetComboBoxValue(cmbVeteran, MST_List[0].VETERAN.Trim());
                    SetComboBoxValue(cmbFarmer, MST_List[0].FARMER.Trim());
                    SetComboBoxValue(cmbUSCitizen, MST_List[0].US_CITIZEN.Trim());
                    SetComboBoxValue(cmbHSParent, MST_List[0].HS_PARENT.Trim());
                }
            }
            else
            {
                INKINDDTLEntity Search_MST = new INKINDDTLEntity(true);
                Search_MST.Agency = BaseForm.BaseAgency; Search_MST.CODE = IKM_Code; Search_MST.SEQ = IKD_Seq;
                List<INKINDDTLEntity> MST_List = _model.INKINDData.Browse_INKINDDTL(Search_MST, "Browse");
                if (MST_List.Count > 0)
                {
                    SetComboBoxValue(cmbService, MST_List[0].SERVICE_TYPE.Trim());
                    if (!string.IsNullOrEmpty(MST_List[0].SERVICE_DATE.Trim()))
                    {
                        dtpDate.Checked = true;
                        dtpDate.Text = MST_List[0].SERVICE_DATE.Trim();
                    }
                    if (!string.IsNullOrEmpty(MST_List[0].START_TIME.Trim()))
                    {
                        dtpStartTime.Checked = true;
                        dtpStartTime.Text = MST_List[0].START_TIME.Trim();
                    }
                    if (!string.IsNullOrEmpty(MST_List[0].END_TIME.Trim()))
                    {
                        dtpEndtime.Checked = true;
                        dtpEndtime.Text = MST_List[0].END_TIME.Trim();
                    }
                    txtInkind.Text = MST_List[0].TOTAL_INKIND.Trim();
                    txtMileage.Text = MST_List[0].TOTAL_MILEAGE.Trim();
                    txtPSTime.Text = MST_List[0].SERVICE_TIME.Trim();
                    txtPTime.Text = MST_List[0].TRANSPORT_TIME.Trim();
                    txtRTMDriven.Text = MST_List[0].MILES_DRIVEN.Trim();
                    txtTotServices.Text = MST_List[0].TOTAL_SERVICE.Trim();

                    if (!string.IsNullOrEmpty(MST_List[0].Site.Trim()))
                        txtSite.Text = MST_List[0].Site.Trim();

                    ShowCaseNotesImages(Search_MST);
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
        string strmsgGrp = string.Empty; string SqlMsg = string.Empty;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    INKINDMSTEntity Entity = new INKINDMSTEntity();

                    CaptainModel Model = new CaptainModel();
                    if (Mode == "Edit")
                        Entity.Rec_Type = "U";
                    Entity.CODE = txtCode.Text;
                    if (txtCode.Text == "New")
                    {
                        Entity.Rec_Type = "I";
                        Entity.CODE = "1";
                    }
                    //Entity.Active = ChkbActive.Checked ? "A" : "I";
                    Entity.Agency = BaseForm.BaseAgency;
                    Entity.Dept = "  ";
                    Entity.Prog = "  ";
                    Entity.TYPE = ((ListItem)cmbDonorType.SelectedItem).Value.ToString().Trim();
                    Entity.DONOR_NAME = txtDName.Text.Trim();
                    Entity.CHILD_FNAME = txtFName.Text.Trim();
                    Entity.CHILD_LNAME = txtLName.Text.Trim();
                    Entity.ADDRESS = txtAddress.Text.Trim();
                    if (((ListItem)CmbGender.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.SEX = ((ListItem)CmbGender.SelectedItem).Value.ToString().Trim();
                    Entity.PHONE = maskPhone.Text; string[] phonebreak = Regex.Split(maskPhone.Text, "-");
                    if (dtpDOB.Checked == true)
                        Entity.DOB = dtpDOB.Text.ToString().Trim();
                    if (((ListItem)cmbEthenCity.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.ETHNICITY = ((ListItem)cmbEthenCity.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbRace.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.RACE = ((ListItem)cmbRace.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbEduLevel.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.EDUCATION = ((ListItem)cmbEduLevel.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbDisabled.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.DISABLED = ((ListItem)cmbDisabled.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbVeteran.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.VETERAN = ((ListItem)cmbVeteran.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbFarmer.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.FARMER = ((ListItem)cmbFarmer.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbUSCitizen.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.US_CITIZEN = ((ListItem)cmbUSCitizen.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbHSParent.SelectedItem).Value.ToString().Trim() != "0")
                        Entity.HS_PARENT = ((ListItem)cmbHSParent.SelectedItem).Value.ToString().Trim();
                    Entity.LSTC_OPERATOR = BaseForm.UserID;
                    Entity.ADD_OPERATOR = BaseForm.UserID;

                    if (_model.INKINDData.UpdateINKINDMST(Entity, "Update", out strmsgGrp, out SqlMsg))
                    {
                        AlertBox.Show("Saved Successfully");
                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (FormType == "Donor")
            {
                if (string.IsNullOrEmpty(txtDName.Text) || string.IsNullOrWhiteSpace(txtDName.Text))
                {
                    _errorProvider.SetError(txtDName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDName.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtDName, null);

                if (((ListItem)cmbDonorType.SelectedItem).Value.ToString().Trim() == "NA")
                {
                    _errorProvider.SetError(cmbDonorType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDonorType.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbDonorType, null);
            }
            else
            {
                if (((ListItem)cmbService.SelectedItem).Value.ToString().Trim() == "0")
                {
                    _errorProvider.SetError(cmbService, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblService.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbService, null);

                if (dtpDate.Checked == false)
                {
                    _errorProvider.SetError(dtpDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(dtpDate, null);

                if (dtpStartTime.Checked.Equals(true) && dtpEndtime.Checked.Equals(true))
                {
                    if (!string.IsNullOrEmpty(dtpStartTime.Text) && (!string.IsNullOrEmpty(dtpEndtime.Text)))
                    {
                        if (Convert.ToDateTime(dtpStartTime.Text) > Convert.ToDateTime(dtpEndtime.Text))
                        {
                            _errorProvider.SetError(dtpEndtime, "It should be greater than Start date ".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtpEndtime, null);
                        }
                    }
                }
            }

            IsSaveValid = isValid;
            return (isValid);
        }

        public string[] GetSelected_Inkind_Code()
        {
            string[] Added_Edited_InkindCode = new string[3];

            if (Mode == "Edit")
                Added_Edited_InkindCode[0] = txtCode.Text.Trim();
            else
                Added_Edited_InkindCode[0] = strmsgGrp;
            Added_Edited_InkindCode[1] = Mode;
            //Added_Edited_InkindCode[2] = Type;

            return Added_Edited_InkindCode;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE0012");
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void btnSSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    INKINDDTLEntity Entity = new INKINDDTLEntity();

                    CaptainModel Model = new CaptainModel();
                    if (Mode == "Edit")
                    {
                        Entity.Rec_Type = "U";
                        Entity.SEQ = IKD_Seq;
                    }
                    else
                    {
                        Entity.Rec_Type = "I";
                        Entity.SEQ = "1";
                    }
                    Entity.CODE = IKM_Code;
                    //Entity.Active = ChkbActive.Checked ? "A" : "I";
                    Entity.Agency = BaseForm.BaseAgency;
                    Entity.Dept = "  ";
                    Entity.Prog = "  ";
                    Entity.SERVICE_TYPE = ((ListItem)cmbService.SelectedItem).Value.ToString().Trim();

                    if (dtpDate.Checked == true)
                        Entity.SERVICE_DATE = dtpDate.Text.ToString().Trim();
                    if (dtpStartTime.Checked == true)
                        Entity.START_TIME = dtpStartTime.Value.ToString("HH:mm:ss");
                    if (dtpEndtime.Checked == true)
                        Entity.END_TIME = dtpEndtime.Value.ToString("HH:mm:ss");
                    if (!string.IsNullOrEmpty(txtPSTime.Text.Trim()))
                        Entity.SERVICE_TIME = txtPSTime.Text.Trim();
                    if (!string.IsNullOrEmpty(txtPTime.Text.Trim()))
                        Entity.TRANSPORT_TIME = txtPTime.Text.Trim();
                    if (!string.IsNullOrEmpty(txtRTMDriven.Text.Trim()))
                        Entity.MILES_DRIVEN = txtRTMDriven.Text.Trim();
                    if (!string.IsNullOrEmpty(txtTotServices.Text.Trim()))
                        Entity.TOTAL_SERVICE = txtTotServices.Text.Trim();
                    if (!string.IsNullOrEmpty(txtMileage.Text.Trim()))
                        Entity.TOTAL_MILEAGE = txtMileage.Text.Trim();
                    if (!string.IsNullOrEmpty(txtInkind.Text.Trim()))
                        Entity.TOTAL_INKIND = txtInkind.Text.Trim();
                    Entity.LSTC_Operator = BaseForm.UserID;
                    Entity.ADD_Operator = BaseForm.UserID;

                    if (!string.IsNullOrEmpty(txtSite.Text.Trim()))
                        Entity.Site = txtSite.Text.Trim();

                    if (_model.INKINDData.UpdateINKINDDET(Entity, "Update", out strmsgGrp, out SqlMsg))
                    {
                        string strSeq = string.Empty;
                        if (Mode == "Edit")
                        {
                            strSeq = Entity.SEQ;
                        }
                        else
                        {
                            strSeq = strmsgGrp;
                        }

                        if (!string.IsNullOrEmpty(txtCaseNotes.Text))
                        {


                            InsertDelCaseNotes(BaseForm.BaseAgency + "  " + "  " + IKM_Code + strSeq, BaseForm.BaseAgency + "  " + "  " + IKM_Code, string.Empty);
                        }
                        else
                        {
                            InsertDelCaseNotes(BaseForm.BaseAgency + "  " + "  " + IKM_Code + strSeq, BaseForm.BaseAgency + "  " + "  " + IKM_Code, "Del");
                        }
                        AlertBox.Show("Saved Successfully");
                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void InsertDelCaseNotes(string FiledName, string ApplicationNo, string strMode)
        {

            CaseNotesEntity caseNotesDetails = new CaseNotesEntity();
            caseNotesDetails.ScreenName = Privileges.Program;
            caseNotesDetails.FieldName = FiledName;
            caseNotesDetails.AppliCationNo = ApplicationNo;
            caseNotesDetails.Data = txtCaseNotes.Text.Trim();
            caseNotesDetails.AddOperator = BaseForm.UserID;
            caseNotesDetails.LstcOperation = BaseForm.UserID;
            if (strMode == "Del")
            {
                caseNotesDetails.Mode = "Del";
            }
            if (_model.TmsApcndata.InsertUpdateCaseNotes(caseNotesDetails))
            {
            }


        }

        public string[] GetSelected_Service_Code()
        {
            string[] Added_Edited_InkindSCode = new string[3];

            if (Mode == "Edit")
                Added_Edited_InkindSCode[2] = IKD_Seq;
            else
                Added_Edited_InkindSCode[2] = strmsgGrp;
            Added_Edited_InkindSCode[1] = Mode;
            Added_Edited_InkindSCode[0] = IKM_Code;

            return Added_Edited_InkindSCode;
        }

        private void btnSCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        public static TextBoxValidation CustomDecimalValidation9dot2
        {
            get
            {
                return new TextBoxValidation(@"String(value).match(/^[0-9]\d{0,8}(\.\d{1,2})*(,\d+)?$/ )", "Value must be between 0 - 999999999.99", "0-9\\.");
            }
        }

        private void txtPSTime_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPSTime.Text.Trim()))
            {
                if (Convert.ToDecimal(txtPSTime.Text) > 999999999)
                    txtPSTime.Text = txtPSTime.Text.Substring((txtPSTime.Text.Length - 9), 9).Trim();
            }
        }

        private void txtPTime_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPTime.Text.Trim()))
            {
                if (Convert.ToDecimal(txtPTime.Text) > 999999999)
                    txtPTime.Text = txtPTime.Text.Substring((txtPTime.Text.Length - 9), 9).Trim();
            }
        }

        private void txtRTMDriven_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRTMDriven.Text.Trim()))
            {
                if (Convert.ToDecimal(txtRTMDriven.Text) > 999999999)
                    txtRTMDriven.Text = txtRTMDriven.Text.Substring((txtRTMDriven.Text.Length - 9), 9).Trim();
            }
        }

        private void txtTotServices_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotServices.Text.Trim()))
            {
                if (Convert.ToDecimal(txtTotServices.Text) > 999999999)
                    txtTotServices.Text = txtTotServices.Text.Substring((txtTotServices.Text.Length - 9), 9).Trim();
            }
        }

        private void txtMileage_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMileage.Text.Trim()))
            {
                if (Convert.ToDecimal(txtMileage.Text) > 999999999)
                    txtMileage.Text = txtMileage.Text.Substring((txtMileage.Text.Length - 9), 9).Trim();
            }
        }

        private void txtInkind_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInkind.Text.Trim()))
            {
                if (Convert.ToDecimal(txtInkind.Text) > 999999999)
                    txtInkind.Text = txtInkind.Text.Substring((txtInkind.Text.Length - 9), 9).Trim();
            }
        }
        public List<CaseNotesEntity> caseNotesEntity { get; set; }
        private void ShowCaseNotesImages(INKINDDTLEntity Entity)
        {

            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, Entity.Agency + "  " + "  " + Entity.CODE + Entity.SEQ);
            if (caseNotesEntity.Count > 0)
            {
                // pbCaseNotes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesView);
                txtCaseNotes.Text = caseNotesEntity[0].Data.ToString().Trim();
            }
            else
            {
                txtCaseNotes.Text = string.Empty;
                // pbCaseNotes.Image = new IconResourceHandle(Consts.Icons16x16.CaseNotesNew);
            }


        }

        private void btnSite_Click(object sender, EventArgs e)
        {
            SiteSearchForm siteSearchForm = new SiteSearchForm(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges, "ClientIntake", BaseForm);
            siteSearchForm.FormClosed += new FormClosedEventHandler(OnSiteFormClosed);
            siteSearchForm.StartPosition = FormStartPosition.CenterScreen;
            siteSearchForm.ShowDialog();
        }

        private void OnSiteFormClosed(object sender, FormClosedEventArgs e)
        {
            SiteSearchForm form = sender as SiteSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string selectedSite = form.GetSelectedRowDetails();
                txtSite.Text = selectedSite;
            }
        }

        private void btnAppSearch_Click(object sender, EventArgs e)
        {
            SSNSearchForm SSNSearchForm = new SSNSearchForm(BaseForm, Privileges.Program, "H");
            SSNSearchForm.FormClosed += new FormClosedEventHandler(OnSearchFormClosed);
            SSNSearchForm.StartPosition = FormStartPosition.CenterScreen;
            SSNSearchForm.ShowDialog();
        }

        private void OnSearchFormClosed(object sender, FormClosedEventArgs e)
        {
            SSNSearchForm form = sender as SSNSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                CaseMstSnpEntity selectedSsn = form.GetSelectedRow();
                if (selectedSsn != null)
                {
                    List<CaseMstEntity> mstdetails = _model.CaseMstData.GetCaseMstadpyn(selectedSsn.Agency, selectedSsn.Dept, selectedSsn.Program, selectedSsn.Year, selectedSsn.ApplNo);
                    CaseSnpEntity snpdetails = _model.CaseMstData.GetCaseSnpDetails(selectedSsn.Agency, selectedSsn.Dept, selectedSsn.Program, selectedSsn.Year, selectedSsn.ApplNo, selectedSsn.FamilySeq);
                    txtFName.Text = snpdetails.NameixFi;
                    txtLName.Text = snpdetails.NameixLast;
                    if (snpdetails.AltBdate != string.Empty)
                        dtpDOB.Text = Convert.ToDateTime(snpdetails.AltBdate).ToString();
                    CommonFunctions.SetComboBoxValue(cmbDisabled, snpdetails.Disable.Trim());
                    CommonFunctions.SetComboBoxValue(cmbRace, snpdetails.Race.Trim());
                    CommonFunctions.SetComboBoxValue(CmbGender, snpdetails.Sex.Trim());
                    CommonFunctions.SetComboBoxValue(cmbEthenCity, snpdetails.Ethnic.Trim());
                    CommonFunctions.SetComboBoxValue(cmbEduLevel, snpdetails.Education.Trim());
                    CommonFunctions.SetComboBoxValue(cmbFarmer, snpdetails.Farmer.Trim());
                    CommonFunctions.SetComboBoxValue(cmbVeteran, snpdetails.Vet.Trim());

                    if (mstdetails.Count > 0)
                    {
                        txtAddress.Text = mstdetails[0].Hn + ' ' + mstdetails[0].Street + ' ' + mstdetails[0].City + ' ' + mstdetails[0].State + ' ' + mstdetails[0].Zip;
                        maskPhone.Text = mstdetails[0].Area + mstdetails[0].Phone;
                    }

                }
            }
        }
    }
}