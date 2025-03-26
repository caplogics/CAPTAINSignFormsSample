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
using Captain.Common.Views.Controls.Compatibility;
using NPOI.OpenXmlFormats.Dml.Chart;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class VendorMaintenance_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        public VendorMaintenance_Form(BaseForm baseform, string mode, string code, int count, PrivilegeEntity priviliges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Mode = mode;
            Code = code;
            Vendor_Count = count;
            Privileges = priviliges;
            DataSet ds = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB("08004");
            lblserviceTypeReq.Visible = false;
            btnFuelTypes.Visible = false;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblserviceTypeReq.Visible = true;
                    btnFuelTypes.Visible = true;
                }
            }

            Fillcombos();

            FillAgencyCombo();

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
            {
                cmbAgency.Visible = true; lblAgency.Visible = true;
            }
            else
            {
                cmbAgency.Visible = false;lblAgency.Visible=false;
            }

         //   LblHeader.Text = priviliges.PrivilegeName;
            if (Mode == "Add")
            {
                this.Text = "Vendor Maintenance" /*priviliges.Program*/ + " - Add";
                //lblOrganization.Location = new Point(15, 366);
                //txtOrginization.Location = new Point(106, 362);
                //this.Size = new Size(810, 490);

                if (BaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                {
                    txtCode.Text = "New Vendor";
                    txtCode.Enabled = false;
                }


                //txtCode.Text = "New";
                //txtCode.Enabled = false;
            }
            else
            {
                this.Text = "Vendor Maintenance" /*priviliges.Program*/ + " - Edit";
                //lblOrganization.Location = new Point(15, 366);
                //txtOrginization.Location = new Point(106, 362);
                //this.Size = new Size(810, 490);
                txtCode.Enabled = false;
                FillControls();
            }
            txtCode.Validator = TextBoxValidation.IntegerValidator;
            txtZip.Validator = TextBoxValidation.IntegerValidator;
            txtZipPlus.Validator = TextBoxValidation.IntegerValidator;
            txtSecCode.Validator = TextBoxValidation.IntegerValidator;
            txtAccFormat.Validator = TextBoxValidation.IntegerValidator;
            txtParVendor.Validator = TextBoxValidation.IntegerValidator;
            if (Vendor_Count > 0)
                btnParVendor.Visible = true;
            else
            {
                btnParVendor.Visible = false;
                txtParVendor.Enabled = false;
            }

        }

        public VendorMaintenance_Form(BaseForm baseform, string mode, string code, int count, PrivilegeEntity priviliges, string strSource, string LLRNAME)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Mode = mode;
            Code = code;
            Vendor_Count = count;
            Privileges = priviliges;
            Fillcombos();
          //  LblHeader.Text = priviliges.PrivilegeName;
            if (Mode == "Add")
            {
                this.Text = "Vendor Maintenance" + " - Add";
                //txtCode.Text = "New";

                List<CASEVDDEntity> CaseVddlist;
                CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
                CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

                string RepAddCode = "1";
                if (CaseVddlist.Count > 0)
                {
                    RepAddCode = CaseVddlist.Max(u => u.Code);
                    RepAddCode = (int.Parse(RepAddCode) + 1).ToString();
                }
                txtCode.Text = "0000000000".Substring(0, 10 - RepAddCode.Length) + RepAddCode;

                //string vendorcode = System.Guid.NewGuid().ToString();
                //txtCode.Text = "L000"+ vendorcode.Substring(0, 6).ToUpper();
                txtCode.Enabled = false;



                if (!string.IsNullOrEmpty(LLRNAME.Trim()))
                {
                    CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetLandlordadpya(BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(), BaseForm.BaseYear.ToString(), BaseForm.BaseApplicationNo.ToString(), "Landlord");
                    if (caseDiffDetails != null)
                    {

                        ChkbActive.Checked = true;
                        txtName.Text = caseDiffDetails.IncareFirst + " " + caseDiffDetails.IncareLast;

                        txtCity.Text = caseDiffDetails.City;

                        string Address1 = string.Empty;
                        if (!string.IsNullOrEmpty(caseDiffDetails.Hn.Trim())) Address1 = "H.No: " + caseDiffDetails.Hn.Trim();
                        if (!string.IsNullOrEmpty(caseDiffDetails.Apt.Trim())) { Address1 = Address1.Trim() == "" ? "Apt: " + caseDiffDetails.Apt.Trim() : Address1 + ", Apt: " + caseDiffDetails.Apt.Trim(); }
                        if (!string.IsNullOrEmpty(caseDiffDetails.Flr.Trim())) { Address1 = Address1.Trim() == "" ? "Flr: " + caseDiffDetails.Flr.Trim() : Address1 + ", Flr: " + caseDiffDetails.Flr.Trim(); }

                        txtAddr1.Text = Address1.Trim(); //caseDiffDetails.Hn + " " + caseDiffDetails.Street +" " + caseDiffDetails.Suffix + " " + caseDiffDetails.Apt + " " + caseDiffDetails.Flr;

                        if (caseDiffDetails.Phone != string.Empty)
                        {
                            maskPhone.Text = caseDiffDetails.Phone;
                        }
                        //maskFax.Text = caseDiffDetails.Fax.Trim();
                        txtState.Text = caseDiffDetails.State;
                        txtStreet.Text = caseDiffDetails.Street + " " + caseDiffDetails.Suffix;
                        txtZip.Text = caseDiffDetails.Zip;
                        txtZipPlus.Text = caseDiffDetails.ZipPlus;
                        txtZip.Text = "00000".Substring(0, 5 - caseDiffDetails.Zip.Length) + caseDiffDetails.Zip;
                        txtZipPlus.Text = "0000".Substring(0, 4 - caseDiffDetails.ZipPlus.Length) + caseDiffDetails.ZipPlus;

                    }
                }

                CommonFunctions.SetComboBoxValue(CmbVendorType, "02");
                txtFuelType.Text = strSource;


            }
            DisalbleFields();

            txtCode.Validator = TextBoxValidation.IntegerValidator;
            txtZip.Validator = TextBoxValidation.IntegerValidator;
            txtZipPlus.Validator = TextBoxValidation.IntegerValidator;
            txtSecCode.Validator = TextBoxValidation.IntegerValidator;
            txtAccFormat.Validator = TextBoxValidation.IntegerValidator;
            txtParVendor.Validator = TextBoxValidation.IntegerValidator;
            //if (Vendor_Count > 0)
            //    btnParVendor.Visible = true;
            //else
            //{
            //    btnParVendor.Visible = false;
            //    txtParVendor.Enabled = false;
            //}

        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Code { get; set; }

        public int Vendor_Count { get; set; }

        //public string button_Mode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        private void Fillcombos()
        {
            CmbCycle.Items.Clear(); cmbFixedMargin.Items.Clear();
            Cmb1099.Items.Clear(); cmbElecTrans.Items.Clear();
            CmbVendorType.Items.Clear();
            cmbElecTrans.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("No", "N"));
            listItem.Add(new ListItem("Yes", "Y"));
            CmbCycle.Items.AddRange(listItem.ToArray());
            CmbCycle.SelectedIndex = 0;

            List<ListItem> listItem1 = new List<ListItem>();
            listItem1.Add(new ListItem("", "0"));
            listItem1.Add(new ListItem("SS#", "S"));
            listItem1.Add(new ListItem("EIN", "E"));
            cmbEinSSN.Items.AddRange(listItem1.ToArray());
            cmbEinSSN.SelectedIndex = 0;

            Cmb1099.Items.AddRange(listItem.ToArray());
            Cmb1099.SelectedIndex = 0;

            cmbElecTrans.Items.AddRange(listItem.ToArray());
            cmbElecTrans.SelectedIndex = 0;

           

            List<CommonEntity> commonEntity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.VendorType, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);

            CmbVendorType.Items.Insert(0, new ListItem("Select One", "0"));
            CmbVendorType.SelectedIndex = 0;
            foreach (CommonEntity Resident in commonEntity)
            {
                ListItem li = new ListItem(Resident.Desc, Resident.Code);
                CmbVendorType.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Resident.Default.Equals("Y")) CmbVendorType.SelectedItem = li;
            }

            //listItem = new List<ListItem>();
            //listItem.Add(new ListItem("Fuel Assistance", "01"));
            //listItem.Add(new ListItem("Accounts Payable", "02"));
            //listItem.Add(new ListItem("Housing", "05"));
            //CmbVendorType.Items.AddRange(listItem.ToArray());
            //CmbVendorType.SelectedIndex = 0;

            listItem = new List<ListItem>();
            listItem.Add(new ListItem("N", "N"));
            listItem.Add(new ListItem("D", "D"));
            listItem.Add(new ListItem("1", "1"));
            listItem.Add(new ListItem("2", "2"));
            listItem.Add(new ListItem("3", "3"));
            listItem.Add(new ListItem("4", "4"));
            listItem.Add(new ListItem("5", "5"));
            listItem.Add(new ListItem("6", "6"));
            listItem.Add(new ListItem("7", "7"));
            listItem.Add(new ListItem("8", "8"));
            listItem.Add(new ListItem("9", "9"));
            cmbFixedMargin.Items.AddRange(listItem.ToArray());
            cmbFixedMargin.SelectedIndex = 0;
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "TMS00009");
        }

        private void FillControls()
        {
            List<CASEVDDEntity> CaseVddlist; List<CaseVDD1Entity> vdd1_List;
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVDD1Entity Vdd1_Entity = new CaseVDD1Entity(true);
            Search_Entity.Code = Code.Trim(); Vdd1_Entity.Code = Code.Trim();
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");
            vdd1_List = _model.SPAdminData.Browse_CASEVDD1(Vdd1_Entity, "Browse");
            CASEVDDEntity drVdd = CaseVddlist[0]; CaseVDD1Entity drVdd1 = vdd1_List[0];
            txtCode.Text = drVdd.Code.Trim();
            if (drVdd.Active.Equals("A")) ChkbActive.Checked = true;
            else ChkbActive.Checked = false;
            txtName.Text = drVdd.Name.Trim();
            txtSecCode.Text = drVdd1.SVENDOR_CODE.Trim();
            txtIndexBy.Text = drVdd1.IndexBy.Trim();
            txtStreet.Text = drVdd.Addr1.Trim();
            txtAddr1.Text = drVdd.Addr2.Trim();
            txtAddr2.Text = drVdd.Addr3.Trim();
            txtCity.Text = drVdd.City.Trim();
            txtState.Text = drVdd.State.Trim();
            if (!string.IsNullOrEmpty(drVdd.Zip.Trim()))
            {
                txtZip.Text = drVdd.Zip.Substring(0, 5).Trim();
                txtZipPlus.Text = drVdd.Zip.Substring(5, 4).Trim();
            }
            maskPhone.Text = drVdd.Phone.Trim();
            maskFax.Text = drVdd.Fax.Trim();
            txtContName.Text = drVdd.Cont_Name.Trim();
            maskContPhone.Text = drVdd.Cont_Phone.Trim();
            txtEmail.Text = drVdd.Email.Trim();
            txtParVendor.Text = drVdd1.USE_VENDOR_CODE.Trim();
            txtParVenName.Text = drVdd1.Name2.Trim();
            txtAccFormat.Text = drVdd1.ACCT_FORMAT.Trim();
            if (drVdd1.AR.Equals("Y")) chkbContAgree.Checked = true;
            else chkbContAgree.Checked = false;
            SetComboBoxValue(cmbElecTrans, drVdd1.ELEC_TRANSFER.Trim());
            SetComboBoxValue(cmbFixedMargin, drVdd1.BULK_CODE.Trim());
            SetComboBoxValue(CmbVendorType, drVdd1.Type.Trim());
            SetComboBoxValue(Cmb1099, drVdd.Vdd1099.Trim());
            SetComboBoxValue(cmbEinSSN, drVdd1.EINSSN_TYPE.Trim());
            txtFuelType.Text = drVdd1.FUEL_TYPE1.Trim() + drVdd1.FUEL_TYPE2.Trim() + drVdd1.FUEL_TYPE3.Trim() + drVdd1.FUEL_TYPE4.Trim() + drVdd1.FUEL_TYPE5.Trim() +
                drVdd1.FUEL_TYPE6.Trim() + drVdd1.FUEL_TYPE7.Trim() + drVdd1.FUEL_TYPE8.Trim() + drVdd1.FUEL_TYPE9.Trim() + drVdd1.FUEL_TYPE10.Trim();
            maskEIN.Text = drVdd1.SSNO.Trim();
            SetComboBoxValue(CmbCycle, drVdd1.CYCLE.Trim());

            SetComboBoxValue(cmbAgency, drVdd.VDD_Agency.Trim());

            if (drVdd.W9.Trim() == "Y") chkbW9.Checked = true; else chkbW9.Checked = false;
            txtFirst.Text = drVdd.FName.Trim();
            txtLast.Text = drVdd.LName.Trim();
            txtOrginization.Text = drVdd.Orginization.Trim();

        }

        //Added by Sudheer on 05/28/2021 for disabiling fields for the Monroe Vendor Adding
        private void DisalbleFields()
        {
            lblParentVendor.Visible = false;
            txtParVendor.Visible = false;
            btnParVendor.Visible = false;
            txtParVenName.Visible = false;

            lblFixedMargin.Visible = false;
            cmbFixedMargin.Visible = false;
            lblEleTrans.Visible = false;
            cmbElecTrans.Visible = false;
            lbl1099.Visible = false;
            Cmb1099.Visible = false;
            cmbEinSSN.Visible = false;
            lblReqEIN.Visible = false;
            maskEIN.Visible = false;
            lblCycleCode.Visible = false;
            CmbCycle.Visible = false;
            lblAccFormat.Visible = false;
            lblReqAccFormat.Visible = false;
            txtAccFormat.Visible = false;
            chkbContAgree.Visible = false;
            lblEmail.Visible = false;
            txtEmail.Visible = false;

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string strmsgGrp = string.Empty; string SqlMsg = string.Empty; string Type = string.Empty;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CASEVDDEntity Entity = new CASEVDDEntity();
                    CaseVDD1Entity Vdd1Entity = new CaseVDD1Entity();

                    CaptainModel Model = new CaptainModel();
                    if (Mode == "Edit")
                    {
                        Entity.Rec_Type = "U";
                        Vdd1Entity.Rec_Type = "U";
                    }
                    else
                    {
                        Entity.Rec_Type = "I";
                    }
                    Entity.Code = txtCode.Text;
                    //if (txtCode.Text == "New")
                    //{
                    //    Entity.Rec_Type = "I";
                    //    Entity.Code = "1";
                    //}
                    ////Entity.Active = ChkbActive.Checked ? "A" : "I";
                    if (ChkbActive.Checked)
                        Entity.Active = "A";
                    else
                        Entity.Active = "I";
                    Entity.Name = txtName.Text.Trim();
                    Entity.Addr1 = txtStreet.Text.Trim();
                    Entity.Addr2 = txtAddr1.Text.Trim();
                    Entity.Addr3 = txtAddr2.Text.Trim();
                    Entity.City = txtCity.Text.Trim();
                    Entity.State = txtState.Text.Trim();
                    Entity.Zip = txtZip.Text.Trim() + txtZipPlus.Text.Trim();
                    string[] phonebreak = Regex.Split(maskPhone.Text, "-");
                    Entity.Phone = maskPhone.Text.Replace("-","");//phonebreak[0].Trim() + phonebreak[1].Trim()+phonebreak[2].Trim();
                    string[] FaxBreak = Regex.Split(maskFax.Text, "-");
                    Entity.Fax = maskFax.Text.Replace("-","");//FaxBreak[0].Trim() + FaxBreak[1].Trim()+FaxBreak[2].Trim();
                    Entity.Cont_Name = txtContName.Text.Trim().Replace("-","");
                    string[] ContTelPhone = Regex.Split(maskContPhone.Text, "-");
                    Entity.Cont_Phone = maskContPhone.Text.Replace("-","");//ContTelPhone[0].Trim() + ContTelPhone[1].Trim() + ContTelPhone[2].Trim();
                    Entity.Vdd1099 = ((ListItem)Cmb1099.SelectedItem).Value.ToString().Trim();

                    Entity.Email = txtEmail.Text.Trim();

                    Entity.W9 = chkbW9.Checked ? "Y" : "N";
                    Entity.FName = txtFirst.Text.Trim();
                    Entity.LName = txtLast.Text.Trim();
                    Entity.Orginization = txtOrginization.Text.Trim();
                    Entity.VDD_Agency = ((ListItem)cmbAgency.SelectedItem).Value.ToString().Trim();

                    if (_model.SPAdminData.UpdateCASEVDD(Entity, "Update", out strmsgGrp, out SqlMsg))
                    {
                        if (Mode == "Add")
                        {
                            string msgSql = string.Empty;
                            if (Entity.Rec_Type == "I")
                                Vdd1Entity.Rec_Type = "I";
                            Vdd1Entity.Code = strmsgGrp.Trim();
                            Vdd1Entity.IndexBy = txtIndexBy.Text.Trim();
                            if (((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim() != "0")
                            {
                                Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                                Vdd1Entity.Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                            }
                            Vdd1Entity.SVENDOR_CODE = txtSecCode.Text.Trim();
                            Vdd1Entity.BULK_CODE = ((ListItem)cmbFixedMargin.SelectedItem).Value.ToString().Trim();
                            //if (Sel_Vdd1_List.Count > 0)
                            //{
                            //    for (int i = 1; i <= Sel_Vdd1_List.Count; )
                            //    {
                            //        foreach (CaseVDD1Entity dr in Sel_Vdd1_List)
                            //        {
                            //            if (i == 1)
                            //                Vdd1Entity.FUEL_TYPE1 = dr.FuelType.Trim();
                            //            else if (i == 2)
                            //                Vdd1Entity.FUEL_TYPE2 = dr.FuelType.Trim();
                            //            else if (i == 3)
                            //                Vdd1Entity.FUEL_TYPE3 = dr.FuelType.Trim();
                            //            else if (i == 4)
                            //                Vdd1Entity.FUEL_TYPE4 = dr.FuelType.Trim();
                            //            else if (i == 5)
                            //                Vdd1Entity.FUEL_TYPE5 = dr.FuelType.Trim();
                            //            else if (i == 6)
                            //                Vdd1Entity.FUEL_TYPE6 = dr.FuelType.Trim();
                            //            else if (i == 7)
                            //                Vdd1Entity.FUEL_TYPE7 = dr.FuelType.Trim();
                            //            else if (i == 8)
                            //                Vdd1Entity.FUEL_TYPE8 = dr.FuelType.Trim();
                            //            else if (i == 9)
                            //                Vdd1Entity.FUEL_TYPE9 = dr.FuelType.Trim();
                            //            else
                            //                Vdd1Entity.FUEL_TYPE10 = dr.FuelType.Trim();
                            //            i++;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            if (!string.IsNullOrEmpty(txtFuelType.Text))
                            {
                                int i = 1;
                                for (int j = 0; j < txtFuelType.Text.Trim().Length;)
                                {
                                    string Temp_Fuel = txtFuelType.Text.Substring(j, 2).Trim();
                                    if (i == 1)
                                        Vdd1Entity.FUEL_TYPE1 = Temp_Fuel.Trim();
                                    else if (i == 2)
                                        Vdd1Entity.FUEL_TYPE2 = Temp_Fuel.Trim();
                                    else if (i == 3)
                                        Vdd1Entity.FUEL_TYPE3 = Temp_Fuel.Trim();
                                    else if (i == 4)
                                        Vdd1Entity.FUEL_TYPE4 = Temp_Fuel.Trim();
                                    else if (i == 5)
                                        Vdd1Entity.FUEL_TYPE5 = Temp_Fuel.Trim();
                                    else if (i == 6)
                                        Vdd1Entity.FUEL_TYPE6 = Temp_Fuel.Trim();
                                    else if (i == 7)
                                        Vdd1Entity.FUEL_TYPE7 = Temp_Fuel.Trim();
                                    else if (i == 8)
                                        Vdd1Entity.FUEL_TYPE8 = Temp_Fuel.Trim();
                                    else if (i == 9)
                                        Vdd1Entity.FUEL_TYPE9 = Temp_Fuel.Trim();
                                    else
                                        Vdd1Entity.FUEL_TYPE10 = Temp_Fuel.Trim();
                                    i++;
                                    j += 2;
                                }
                            }
                            //}
                            Vdd1Entity.CYCLE = ((ListItem)CmbCycle.SelectedItem).Value.ToString().Trim();
                            Vdd1Entity.ELEC_TRANSFER = ((ListItem)cmbElecTrans.SelectedItem).Value.ToString().Trim();
                            if (((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim() != "0")
                                Vdd1Entity.EINSSN_TYPE = ((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim();
                            Vdd1Entity.SSNO = maskEIN.Text.Trim().Replace("-","");
                            Vdd1Entity.ACCT_FORMAT = txtAccFormat.Text.Trim();
                            Vdd1Entity.AR = chkbContAgree.Checked ? "Y" : "N";
                            Vdd1Entity.USE_VENDOR_CODE = txtParVendor.Text.Trim();
                            Vdd1Entity.Name2 = txtParVenName.Text.Trim();
                            Vdd1Entity.Lsct_Operator = BaseForm.UserID;
                            Vdd1Entity.Add_Operator = BaseForm.UserID;
                            _model.SPAdminData.UpdateCASEVDD1(Vdd1Entity, "Update", out msgSql);

                            Vendor_Maintainance VendorControl = BaseForm.GetBaseUserControl() as Vendor_Maintainance;
                            if (VendorControl != null)
                            {
                                VendorControl.FillVendorGrid(strmsgGrp);
                            } 
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            string msgSql = string.Empty;
                            if (Entity.Rec_Type == "U")
                                Vdd1Entity.Rec_Type = "U";
                            Vdd1Entity.Code = txtCode.Text.Trim();
                            Vdd1Entity.IndexBy = txtIndexBy.Text.Trim();
                            if (((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim() != "0")
                            {
                                Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                                Vdd1Entity.Type = ((ListItem)CmbVendorType.SelectedItem).Value.ToString().Trim();
                            }
                            Vdd1Entity.SVENDOR_CODE = txtSecCode.Text.Trim();
                            Vdd1Entity.BULK_CODE = ((ListItem)cmbFixedMargin.SelectedItem).Value.ToString().Trim();
                            //if (Sel_Vdd1_List.Count > 0)
                            //{
                            //    for (int i = 1; i <= Sel_Vdd1_List.Count; )
                            //    {
                            //        foreach (CaseVDD1Entity dr in Sel_Vdd1_List)
                            //        {

                            //            if (i == 1)
                            //                Vdd1Entity.FUEL_TYPE1 = dr.FuelType.Trim();
                            //            else if (i == 2)
                            //                Vdd1Entity.FUEL_TYPE2 = dr.FuelType.Trim();
                            //            else if (i == 3)
                            //                Vdd1Entity.FUEL_TYPE3 = dr.FuelType.Trim();
                            //            else if (i == 4)
                            //                Vdd1Entity.FUEL_TYPE4 = dr.FuelType.Trim();
                            //            else if (i == 5)
                            //                Vdd1Entity.FUEL_TYPE5 = dr.FuelType.Trim();
                            //            else if (i == 6)
                            //                Vdd1Entity.FUEL_TYPE6 = dr.FuelType.Trim();
                            //            else if (i == 7)
                            //                Vdd1Entity.FUEL_TYPE7 = dr.FuelType.Trim();
                            //            else if (i == 8)
                            //                Vdd1Entity.FUEL_TYPE8 = dr.FuelType.Trim();
                            //            else if (i == 9)
                            //                Vdd1Entity.FUEL_TYPE9 = dr.FuelType.Trim();
                            //            else
                            //                Vdd1Entity.FUEL_TYPE10 = dr.FuelType.Trim();
                            //            i++;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            if (!string.IsNullOrEmpty(txtFuelType.Text))
                            {
                                int i = 1;
                                for (int j = 0; j < txtFuelType.Text.Trim().Length;)
                                {
                                    string Temp_Fuel = txtFuelType.Text.Substring(j, 2).Trim();
                                    if (i == 1)
                                        Vdd1Entity.FUEL_TYPE1 = Temp_Fuel.Trim();
                                    else if (i == 2)
                                        Vdd1Entity.FUEL_TYPE2 = Temp_Fuel.Trim();
                                    else if (i == 3)
                                        Vdd1Entity.FUEL_TYPE3 = Temp_Fuel.Trim();
                                    else if (i == 4)
                                        Vdd1Entity.FUEL_TYPE4 = Temp_Fuel.Trim();
                                    else if (i == 5)
                                        Vdd1Entity.FUEL_TYPE5 = Temp_Fuel.Trim();
                                    else if (i == 6)
                                        Vdd1Entity.FUEL_TYPE6 = Temp_Fuel.Trim();
                                    else if (i == 7)
                                        Vdd1Entity.FUEL_TYPE7 = Temp_Fuel.Trim();
                                    else if (i == 8)
                                        Vdd1Entity.FUEL_TYPE8 = Temp_Fuel.Trim();
                                    else if (i == 9)
                                        Vdd1Entity.FUEL_TYPE9 = Temp_Fuel.Trim();
                                    else
                                        Vdd1Entity.FUEL_TYPE10 = Temp_Fuel.Trim();
                                    i++;
                                    j += 2;
                                }
                            }
                            //}
                            Vdd1Entity.CYCLE = ((ListItem)CmbCycle.SelectedItem).Value.ToString().Trim();
                            Vdd1Entity.ELEC_TRANSFER = ((ListItem)cmbElecTrans.SelectedItem).Value.ToString().Trim();
                            if (((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim() != "0")
                                Vdd1Entity.EINSSN_TYPE = ((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim();
                            Vdd1Entity.SSNO = maskEIN.Text.Trim().Replace("-","");
                            Vdd1Entity.ACCT_FORMAT = txtAccFormat.Text.Trim();
                            Vdd1Entity.AR = chkbContAgree.Checked ? "Y" : "N";
                            Vdd1Entity.USE_VENDOR_CODE = txtParVendor.Text.Trim();
                            Vdd1Entity.Name2 = txtParVenName.Text.Trim();
                            Vdd1Entity.Lsct_Operator = BaseForm.UserID;
                            Vdd1Entity.Add_Operator = BaseForm.UserID;

                            _model.SPAdminData.UpdateCASEVDD1(Vdd1Entity, "Update", out msgSql);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
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
            _errorProvider.SetError(maskEIN, null);
            _errorProvider.SetError(cmbEinSSN, null);
            if (string.IsNullOrEmpty(txtCode.Text.Trim()) || string.IsNullOrWhiteSpace(txtCode.Text.Trim()))
            {
                _errorProvider.SetError(txtCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                if (Mode == "Add")
                {
                    List<CASEVDDEntity> CaseVddlist;
                    CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
                    CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");
                    if (CaseVddlist.Count > 0)
                    {
                        CASEVDDEntity ExistVendor = CaseVddlist.Find(u => u.Code.Equals(txtCode.Text.Trim()));
                        if (ExistVendor != null)
                        {
                            _errorProvider.SetError(txtCode, string.Format("Vendor Code already exists".Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                        }
                        else
                            _errorProvider.SetError(txtCode, null);
                    }
                }
                else
                    _errorProvider.SetError(txtCode, null);
            }
            else
            {
                _errorProvider.SetError(txtCode, null);
            }

            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                _errorProvider.SetError(txtName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtName, null);

            if (string.IsNullOrEmpty(txtStreet.Text) || string.IsNullOrWhiteSpace(txtStreet.Text))
            {
                _errorProvider.SetError(txtStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStreet.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtStreet, null);

            if (string.IsNullOrEmpty(txtCity.Text) || string.IsNullOrWhiteSpace(txtCity.Text))
            {
                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCity, null);

            if (string.IsNullOrEmpty(txtState.Text) || string.IsNullOrWhiteSpace(txtState.Text))
            {
                _errorProvider.SetError(txtState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtState, null);

            if (string.IsNullOrEmpty(txtZip.Text) || string.IsNullOrWhiteSpace(txtZip.Text))
            {
                _errorProvider.SetError(btnZipSearch, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZip.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(btnZipSearch, null);

            if (lblserviceTypeReq.Visible)
            {
                if (string.IsNullOrEmpty(txtFuelType.Text) || string.IsNullOrWhiteSpace(txtFuelType.Text))
                {
                    _errorProvider.SetError(btnFuelTypes, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFuelType.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(btnFuelTypes, null);
            }
            if (((ListItem)cmbElecTrans.SelectedItem).Value.ToString().Trim() == "Y")
            {
                if (string.IsNullOrEmpty(txtAccFormat.Text) || string.IsNullOrWhiteSpace(txtAccFormat.Text))
                {
                    _errorProvider.SetError(txtAccFormat, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAccFormat.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtAccFormat, null);
            }
            if (((ListItem)Cmb1099.SelectedItem).Value.ToString().Trim() == "Y")
            {
                if (((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim() == "0")
                {
                    _errorProvider.SetError(cmbEinSSN, "EIN or SS# is required");
                    isValid = false;
                }
                else
                {
                    //_errorProvider.SetError(cmbEinSSN, null);
                    if (((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim() == "S")
                    {
                        if (maskEIN.Text != "" && maskEIN.Text != "   -   -")
                        {
                            if (maskEIN.Text.Trim().Replace("-", "").Replace(" ", "").Length < 9)
                            {
                                _errorProvider.SetError(maskEIN, "Please enter valid SSN Number");
                                isValid = false;
                            }
                            else
                            {
                                _errorProvider.SetError(maskEIN, null);
                            }
                        }
                    }
                    if (((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim() == "E")
                    {
                        if (maskEIN.Text != "" && maskEIN.Text != "   -   -")
                        {
                            if (maskEIN.Text.Trim().Replace("-", "").Replace(" ", "").Length < 9)
                            {
                                _errorProvider.SetError(maskEIN, "Please enter valid EIN Number");
                                isValid = false;
                            }
                            else
                            {
                                _errorProvider.SetError(maskEIN, null);
                            }
                        }
                    }
                }


                //if (string.IsNullOrEmpty(maskEIN.Text) || string.IsNullOrWhiteSpace(maskEIN.Text))
                //{
                //    _errorProvider.SetError(maskEIN, "EIN or SS# is required");
                //    isValid = false;
                //}
                //else
                //    _errorProvider.SetError(maskEIN, null);
            }

            if (txtEmail.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, Consts.StaticVars.EmailValidatingString))
                {
                    _errorProvider.SetError(txtEmail, Consts.Messages.PleaseEnterEmail);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtEmail, null);
                }
            }



            if (maskPhone.Text != "" && maskPhone.Text != "   -   -")
            {
                if (maskPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskPhone, "Please enter valid Telephone Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskPhone, null);
                }
            }
            if (maskFax.Text != "" && maskFax.Text != "   -   -")
            {
                if (maskFax.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskFax, "Please enter valid Fax Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskFax, null);
                }
            }
            if (maskContPhone.Text != "" && maskContPhone.Text != "   -   -")
            {
                if (maskContPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskContPhone, "Please enter valid Phone Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskContPhone, null);
                }
            }


            IsSaveValid = isValid;
            return (isValid);
        }

        public string[] GetSelected_Vendor_Code()
        {
            string[] Added_Edited_VendorCode = new string[3];

            Added_Edited_VendorCode[0] = strmsgGrp;
            Added_Edited_VendorCode[1] = Mode;
            Added_Edited_VendorCode[2] = Type;

            return Added_Edited_VendorCode;
        }


        private void btnZipSearch_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
            zipCodeSearchForm.ShowDialog();
        }

        private void OnZipCodeFormClosed(object sender, FormClosedEventArgs e)
        {
            btnZipSearch.Enabled = true;
            //btnCitySearch.Enabled = true;
            ZipCodeSearchForm form = sender as ZipCodeSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                ZipCodeEntity zipcodedetais = form.GetSelectedZipCodedetails();
                if (zipcodedetais != null)
                {
                    string zipPlus = zipcodedetais.Zcrplus4;
                    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    txtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                    txtState.Text = zipcodedetais.Zcrstate;
                    txtCity.Text = zipcodedetais.Zcrcity;
                    //SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                    //SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);

                }
            }
            // btnCitySearch.Focus();
        }

        private void txtZip_Leave(object sender, EventArgs e)
        {
            //btnZipSearch.Enabled = false;
            if (string.IsNullOrEmpty(txtZip.Text) || string.IsNullOrWhiteSpace(txtZip.Text))
                _errorProvider.SetError(btnZipSearch, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZip.Text.Replace(Consts.Common.Colon, string.Empty)));
            else
                _errorProvider.SetError(btnZipSearch, null);


            if (txtZip.Text.Length == 5)
            {
                List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(txtZip.Text, string.Empty, string.Empty, string.Empty);
                if (zipcodeEntity.Count > 0)
                {
                    if (zipcodeEntity.Count == 1)
                    {
                        btnZipSearch.Enabled = true;
                    }
                    foreach (ZipCodeEntity zipcodedetais in zipcodeEntity)
                    {
                        if (zipcodedetais != null)
                        {
                            string zipPlus = zipcodedetais.Zcrplus4;
                            txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                            txtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                            txtState.Text = zipcodedetais.Zcrstate;
                            txtCity.Text = zipcodedetais.Zcrcity;
                            //SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                            //SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);
                        }

                    }
                }
                else
                {
                    string Zip = txtZip.Text.Trim();
                    txtZip.Text = "00000".Substring(0, 5 - Zip.Length) + Zip.Trim();
                    //ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
                    //zipCodeSearchForm.FormClosed += new Form.FormClosedEventHandler(OnZipCodeFormClosed);
                    //zipCodeSearchForm.ShowDialog();
                }
            }
            else
            {
                string Zip = txtZip.Text.Trim();
                txtZip.Text = "00000".Substring(0, 5 - Zip.Length) + Zip.Trim();
                //ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
                //zipCodeSearchForm.FormClosed += new Form.FormClosedEventHandler(OnZipCodeFormClosed);
                //zipCodeSearchForm.ShowDialog();
            }
        }
        List<CaseVDD1Entity> Sel_Vdd1_List = new List<CaseVDD1Entity>();
        private void btnFuelTypes_Click(object sender, EventArgs e)
        {
            List<CaseVDD1Entity> sel_CASEVdd1_entity = new List<CaseVDD1Entity>();
            FuelTypes_Form fuel_form = new FuelTypes_Form("FuelType", txtFuelType.Text);
            fuel_form.FormClosed += new FormClosedEventHandler(On_Form_Select_Closed);
            fuel_form.StartPosition = FormStartPosition.CenterScreen;
            fuel_form.ShowDialog();
        }

        private void On_Form_Select_Closed(object sender, FormClosedEventArgs e)
        {

            string Sql_MSg = string.Empty;
            FuelTypes_Form form = sender as FuelTypes_Form;
            string Fuel_List = string.Empty;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_Vdd1_List = form.GetSelected_FuelTypes();
                Fuel_List = form.GetSelected_Fuels();
                txtFuelType.Text = Fuel_List.Trim();
            }
        }

        private void btnParVendor_Click(object sender, EventArgs e)
        {

            VendBrowseForm Vendor_Browser = new VendBrowseForm(BaseForm, Privileges, "**");
            Vendor_Browser.FormClosed += new FormClosedEventHandler(On_Vendor_Browse_Close);
            Vendor_Browser.StartPosition = FormStartPosition.CenterScreen;
            Vendor_Browser.ShowDialog();

        }

        private void On_Vendor_Browse_Close(object sender, FormClosedEventArgs e)
        {

            VendBrowseForm Vendor_Form = sender as VendBrowseForm;

            if (Vendor_Form.DialogResult == DialogResult.OK)

            {
                string[] From_Results = new string[2];
                From_Results = Vendor_Form.Get_Selected_Vendor();
                txtParVendor.Text = From_Results[0];
                txtParVenName.Text = From_Results[1];
            }
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

        private void cmbElecTrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbElecTrans.SelectedItem).Value.ToString().Trim() == "Y")
            {
                txtAccFormat.Enabled = true;
                lblReqAccFormat.Visible = true;
            }
            else
            {
                txtAccFormat.Enabled = false;
                txtAccFormat.Text = string.Empty;
                lblReqAccFormat.Visible = false;
                _errorProvider.SetError(txtAccFormat, null);
            }
        }

        private void Cmb1099_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)Cmb1099.SelectedItem).Value.ToString().Trim() == "Y")
            {
                maskEIN.Enabled = true;
                lblReqEIN.Visible = true;
                cmbEinSSN.Enabled = true;
                if (Mode == "Add")
                {
                    cmbEinSSN.SelectedIndex = 1;
                }
            }
            else
            {
                maskEIN.Enabled = false;
                maskEIN.Text = string.Empty;
                cmbEinSSN.Enabled = false;
                cmbEinSSN.SelectedIndex = 0;
                lblReqEIN.Visible = false;
                _errorProvider.SetError(maskEIN, null);
                _errorProvider.SetError(cmbEinSSN, null);
            }
        }

        private void txtSecCode_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSecCode.Text) || (!string.IsNullOrWhiteSpace(txtSecCode.Text)))
            {
                string SecVenCode = txtSecCode.Text.Trim();
                txtSecCode.Text = "0000000000".Substring(0, 10 - SecVenCode.Length) + SecVenCode.Trim();
            }
        }

        private void txtZipPlus_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtZipPlus.Text) || (!string.IsNullOrWhiteSpace(txtZipPlus.Text)))
            {
                string ZipPlus = txtZipPlus.Text.Trim();
                txtZipPlus.Text = "0000".Substring(0, 4 - ZipPlus.Length) + ZipPlus.Trim();
            }
            else
                txtZipPlus.Text = "0000";
        }

        private void txtParVendor_Leave(object sender, EventArgs e)
        {
            List<CASEVDDEntity> CaseVddlist;
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            string Parentvendor = txtParVendor.Text.Trim();
            if (!string.IsNullOrEmpty(txtParVendor.Text) || !string.IsNullOrWhiteSpace(txtParVendor.Text))
            {
                Search_Entity.Code = "0000000000".Substring(0, 10 - Parentvendor.Length) + Parentvendor.Trim();
                CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");
                if (CaseVddlist.Count > 0)
                {
                    txtParVendor.Text = CaseVddlist[0].Code;
                    txtParVenName.Text = CaseVddlist[0].Name;
                }
                else
                {
                    txtParVenName.Text = string.Empty; txtParVendor.Text = string.Empty;
                    AlertBox.Show("Parent Vendor code does not Exists in Vendor File", MessageBoxIcon.Warning);
                }
            }
            //cmbFixedMargin.Focus();
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtCode.Text)))
            {
                string VenCode = txtCode.Text.Trim();
                txtCode.Text = "0000000000".Substring(0, 10 - VenCode.Length) + VenCode.Trim();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkbW9_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbW9.Checked)
            {
                lblFirst.Visible = lblLast.Visible = txtFirst.Visible = txtLast.Visible = true;
                //lblOrganization.Location = new Point(15, 398);
                //txtOrginization.Location = new Point(106, 394);
                //this.Size = new Size(810, 523);
            }
            else
            {
                lblFirst.Visible = lblLast.Visible = txtFirst.Visible = txtLast.Visible = false;
                //lblOrganization.Location = new Point(15, 366);
                //txtOrginization.Location = new Point(106, 362);
                //this.Size = new Size(810, 490);
            }
        }

        private void cmbEinSSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEinSSN.Items.Count > 0)
            {
                if (((ListItem)cmbEinSSN.SelectedItem).Value.ToString().Trim() == "S")
                {
                    maskEIN.Mask = "999-00-0000";
                    maskEIN.MaxLength = 11;

                }
                else
                {
                    maskEIN.Mask = "000000000";
                    maskEIN.MaxLength = 9;

                }
            }

        }

        private void FillAgencyCombo()
        {

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once

            cmbAgency.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            int TmpRows = 0;
            int AgyIndex = 0;
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (BaseForm.BusinessModuleID == "01")
                    {
                        if (BaseForm.BaseAdminAgency != "**")
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataView dv = new DataView(dt);
                                dv.RowFilter = "AGY= '" + BaseForm.BaseAdminAgency + "'";
                                dt = dv.ToTable();
                            }

                        }
                    }

                    listItem.Add(new ListItem("**" + " - " + "All Agencies", "**"));
                    foreach (DataRow dr in dt.Rows)
                    {
                        listItem.Add(new ListItem(dr["Agy"] + " - " + dr["Name"], dr["Agy"]));
                        //if (DefAgy == dr["Agy"].ToString())
                        //    AgyIndex = TmpRows;

                        TmpRows++;
                    }
                    if (TmpRows > 0)
                    {
                        cmbAgency.Items.AddRange(listItem.ToArray());
                        //if (DefHieExist)
                        //    CmbAgency.SelectedIndex = AgyIndex;
                        //else
                        //{
                        //    if (CmbAgency.Items.Count == 1)
                        //        CmbAgency.SelectedIndex = 0;
                        //}
                        cmbAgency.SelectedIndex = 0;

                        if (BaseForm.BaseAdminAgency != "**")
                        {
                            CommonFunctions.SetComboBoxValue(cmbAgency, BaseForm.BaseAdminAgency);
                            cmbAgency.Enabled = false;
                        }
                        else
                        {
                            CommonFunctions.SetComboBoxValue(cmbAgency, BaseForm.BaseAdminAgency);
                            cmbAgency.Enabled = true;
                        }
                    }
                }
                //DefAgy = DefDept = DefProg = DefYear = null;
            }
            catch (Exception ex) { }
        }

        private void VendorMaintenance_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }
    }
}