using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class VendorMaintenance_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Wisej Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendorMaintenance_Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlVendorCode = new Wisej.Web.Panel();
            this.cmbAgency = new Wisej.Web.ComboBox();
            this.lblAgency = new Wisej.Web.Label();
            this.txtState = new Wisej.Web.TextBox();
            this.lblReqVendorCode = new Wisej.Web.Label();
            this.ChkbActive = new Wisej.Web.CheckBox();
            this.lblserviceTypeReq = new Wisej.Web.Label();
            this.lblReqZIP = new Wisej.Web.Label();
            this.lblReqState = new Wisej.Web.Label();
            this.lblReqStreet = new Wisej.Web.Label();
            this.lblReqVendorName = new Wisej.Web.Label();
            this.btnFuelTypes = new Wisej.Web.Button();
            this.txtFuelType = new Wisej.Web.TextBox();
            this.lblSource = new Wisej.Web.Label();
            this.lblFuelType = new Wisej.Web.Label();
            this.CmbVendorType = new Wisej.Web.ComboBox();
            this.lblphone = new Wisej.Web.Label();
            this.maskPhone = new Wisej.Web.MaskedTextBox();
            this.maskFax = new Wisej.Web.MaskedTextBox();
            this.lblFax = new Wisej.Web.Label();
            this.btnZipSearch = new Wisej.Web.Button();
            this.lblZip = new Wisej.Web.Label();
            this.txtZip = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtZipPlus = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblState = new Wisej.Web.Label();
            this.txtCity = new Wisej.Web.TextBox();
            this.txtAddr2 = new Wisej.Web.TextBox();
            this.lblAddress = new Wisej.Web.Label();
            this.txtAddr1 = new Wisej.Web.TextBox();
            this.txtStreet = new Wisej.Web.TextBox();
            this.lblStreet = new Wisej.Web.Label();
            this.lblIndexby = new Wisej.Web.Label();
            this.txtIndexBy = new Wisej.Web.TextBox();
            this.lblName = new Wisej.Web.Label();
            this.txtName = new Wisej.Web.TextBox();
            this.lblSecCode = new Wisej.Web.Label();
            this.txtSecCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblCode = new Wisej.Web.Label();
            this.lblReqCity = new Wisej.Web.Label();
            this.lblCity = new Wisej.Web.Label();
            this.pnlContactDetails = new Wisej.Web.Panel();
            this.maskEIN = new Wisej.Web.MaskedTextBox();
            this.txtOrginization = new Wisej.Web.TextBox();
            this.lblOrganization = new Wisej.Web.Label();
            this.cmbEinSSN = new Wisej.Web.ComboBox();
            this.lblLast = new Wisej.Web.Label();
            this.txtLast = new Wisej.Web.TextBox();
            this.txtFirst = new Wisej.Web.TextBox();
            this.lblFirst = new Wisej.Web.Label();
            this.chkbW9 = new Wisej.Web.CheckBox();
            this.lblEmail = new Wisej.Web.Label();
            this.txtEmail = new Wisej.Web.TextBox();
            this.chkbContAgree = new Wisej.Web.CheckBox();
            this.lblReqAccFormat = new Wisej.Web.Label();
            this.lblReqEIN = new Wisej.Web.Label();
            this.lblAccFormat = new Wisej.Web.Label();
            this.txtAccFormat = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblCycleCode = new Wisej.Web.Label();
            this.CmbCycle = new Wisej.Web.ComboBox();
            this.Cmb1099 = new Wisej.Web.ComboBox();
            this.lbl1099 = new Wisej.Web.Label();
            this.lblEleTrans = new Wisej.Web.Label();
            this.cmbElecTrans = new Wisej.Web.ComboBox();
            this.cmbFixedMargin = new Wisej.Web.ComboBox();
            this.lblFixedMargin = new Wisej.Web.Label();
            this.btnParVendor = new Wisej.Web.Button();
            this.txtParVendor = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblParentVendor = new Wisej.Web.Label();
            this.txtParVenName = new Wisej.Web.TextBox();
            this.txtContName = new Wisej.Web.TextBox();
            this.lblContName = new Wisej.Web.Label();
            this.maskContPhone = new Wisej.Web.MaskedTextBox();
            this.lblContPhone = new Wisej.Web.Label();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlVendorMaintainance = new Wisej.Web.Panel();
            this.pnlVendorCode.SuspendLayout();
            this.pnlContactDetails.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlVendorMaintainance.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVendorCode
            // 
            this.pnlVendorCode.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlVendorCode.Controls.Add(this.cmbAgency);
            this.pnlVendorCode.Controls.Add(this.lblAgency);
            this.pnlVendorCode.Controls.Add(this.txtState);
            this.pnlVendorCode.Controls.Add(this.lblReqVendorCode);
            this.pnlVendorCode.Controls.Add(this.ChkbActive);
            this.pnlVendorCode.Controls.Add(this.lblserviceTypeReq);
            this.pnlVendorCode.Controls.Add(this.lblReqZIP);
            this.pnlVendorCode.Controls.Add(this.lblReqState);
            this.pnlVendorCode.Controls.Add(this.lblReqStreet);
            this.pnlVendorCode.Controls.Add(this.lblReqVendorName);
            this.pnlVendorCode.Controls.Add(this.btnFuelTypes);
            this.pnlVendorCode.Controls.Add(this.txtFuelType);
            this.pnlVendorCode.Controls.Add(this.lblSource);
            this.pnlVendorCode.Controls.Add(this.lblFuelType);
            this.pnlVendorCode.Controls.Add(this.CmbVendorType);
            this.pnlVendorCode.Controls.Add(this.lblphone);
            this.pnlVendorCode.Controls.Add(this.maskPhone);
            this.pnlVendorCode.Controls.Add(this.maskFax);
            this.pnlVendorCode.Controls.Add(this.lblFax);
            this.pnlVendorCode.Controls.Add(this.btnZipSearch);
            this.pnlVendorCode.Controls.Add(this.lblZip);
            this.pnlVendorCode.Controls.Add(this.txtZip);
            this.pnlVendorCode.Controls.Add(this.txtZipPlus);
            this.pnlVendorCode.Controls.Add(this.lblState);
            this.pnlVendorCode.Controls.Add(this.txtCity);
            this.pnlVendorCode.Controls.Add(this.txtAddr2);
            this.pnlVendorCode.Controls.Add(this.lblAddress);
            this.pnlVendorCode.Controls.Add(this.txtAddr1);
            this.pnlVendorCode.Controls.Add(this.txtStreet);
            this.pnlVendorCode.Controls.Add(this.lblStreet);
            this.pnlVendorCode.Controls.Add(this.lblIndexby);
            this.pnlVendorCode.Controls.Add(this.txtIndexBy);
            this.pnlVendorCode.Controls.Add(this.lblName);
            this.pnlVendorCode.Controls.Add(this.txtName);
            this.pnlVendorCode.Controls.Add(this.lblSecCode);
            this.pnlVendorCode.Controls.Add(this.txtSecCode);
            this.pnlVendorCode.Controls.Add(this.txtCode);
            this.pnlVendorCode.Controls.Add(this.lblCode);
            this.pnlVendorCode.Controls.Add(this.lblReqCity);
            this.pnlVendorCode.Controls.Add(this.lblCity);
            this.pnlVendorCode.Dock = Wisej.Web.DockStyle.Left;
            this.pnlVendorCode.Location = new System.Drawing.Point(0, 0);
            this.pnlVendorCode.Name = "pnlVendorCode";
            this.pnlVendorCode.Size = new System.Drawing.Size(450, 428);
            this.pnlVendorCode.TabIndex = 1;
            // 
            // cmbAgency
            // 
            this.cmbAgency.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAgency.Location = new System.Drawing.Point(159, 393);
            this.cmbAgency.Name = "cmbAgency";
            this.cmbAgency.Size = new System.Drawing.Size(232, 25);
            this.cmbAgency.TabIndex = 20;
            // 
            // lblAgency
            // 
            this.lblAgency.AutoSize = true;
            this.lblAgency.Location = new System.Drawing.Point(15, 397);
            this.lblAgency.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblAgency.Name = "lblAgency";
            this.lblAgency.Size = new System.Drawing.Size(45, 18);
            this.lblAgency.TabIndex = 19;
            this.lblAgency.Text = "Agency";
            // 
            // txtState
            // 
            this.txtState.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtState.Location = new System.Drawing.Point(390, 234);
            this.txtState.MaxLength = 2;
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(33, 25);
            this.txtState.TabIndex = 10;
            // 
            // lblReqVendorCode
            // 
            this.lblReqVendorCode.ForeColor = System.Drawing.Color.Red;
            this.lblReqVendorCode.Location = new System.Drawing.Point(90, 11);
            this.lblReqVendorCode.Name = "lblReqVendorCode";
            this.lblReqVendorCode.Size = new System.Drawing.Size(10, 10);
            this.lblReqVendorCode.TabIndex = 13;
            this.lblReqVendorCode.Text = "*";
            // 
            // ChkbActive
            // 
            this.ChkbActive.Appearance = Wisej.Web.Appearance.Switch;
            this.ChkbActive.AutoSize = false;
            this.ChkbActive.CheckState = Wisej.Web.CheckState.Checked;
            this.ChkbActive.Location = new System.Drawing.Point(264, 12);
            this.ChkbActive.Name = "ChkbActive";
            this.ChkbActive.Size = new System.Drawing.Size(72, 20);
            this.ChkbActive.TabIndex = 2;
            this.ChkbActive.Text = "Active";
            // 
            // lblserviceTypeReq
            // 
            this.lblserviceTypeReq.ForeColor = System.Drawing.Color.Red;
            this.lblserviceTypeReq.Location = new System.Drawing.Point(86, 362);
            this.lblserviceTypeReq.Name = "lblserviceTypeReq";
            this.lblserviceTypeReq.Size = new System.Drawing.Size(10, 10);
            this.lblserviceTypeReq.TabIndex = 13;
            this.lblserviceTypeReq.Text = "*";
            this.lblserviceTypeReq.Visible = false;
            // 
            // lblReqZIP
            // 
            this.lblReqZIP.ForeColor = System.Drawing.Color.Red;
            this.lblReqZIP.Location = new System.Drawing.Point(34, 266);
            this.lblReqZIP.Name = "lblReqZIP";
            this.lblReqZIP.Size = new System.Drawing.Size(13, 12);
            this.lblReqZIP.TabIndex = 13;
            this.lblReqZIP.Text = "*";
            // 
            // lblReqState
            // 
            this.lblReqState.ForeColor = System.Drawing.Color.Red;
            this.lblReqState.Location = new System.Drawing.Point(378, 234);
            this.lblReqState.Name = "lblReqState";
            this.lblReqState.Size = new System.Drawing.Size(10, 10);
            this.lblReqState.TabIndex = 13;
            this.lblReqState.Text = "*";
            // 
            // lblReqStreet
            // 
            this.lblReqStreet.ForeColor = System.Drawing.Color.Red;
            this.lblReqStreet.Location = new System.Drawing.Point(49, 139);
            this.lblReqStreet.Name = "lblReqStreet";
            this.lblReqStreet.Size = new System.Drawing.Size(10, 10);
            this.lblReqStreet.TabIndex = 13;
            this.lblReqStreet.Text = "*";
            // 
            // lblReqVendorName
            // 
            this.lblReqVendorName.ForeColor = System.Drawing.Color.Red;
            this.lblReqVendorName.Location = new System.Drawing.Point(95, 74);
            this.lblReqVendorName.Name = "lblReqVendorName";
            this.lblReqVendorName.Size = new System.Drawing.Size(11, 10);
            this.lblReqVendorName.TabIndex = 13;
            this.lblReqVendorName.Text = "*";
            // 
            // btnFuelTypes
            // 
            this.btnFuelTypes.Location = new System.Drawing.Point(363, 362);
            this.btnFuelTypes.Name = "btnFuelTypes";
            this.btnFuelTypes.Size = new System.Drawing.Size(60, 25);
            this.btnFuelTypes.TabIndex = 18;
            this.btnFuelTypes.Text = "&View";
            this.btnFuelTypes.Visible = false;
            this.btnFuelTypes.Click += new System.EventHandler(this.btnFuelTypes_Click);
            // 
            // txtFuelType
            // 
            this.txtFuelType.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFuelType.Enabled = false;
            this.txtFuelType.Location = new System.Drawing.Point(159, 362);
            this.txtFuelType.MaxLength = 20;
            this.txtFuelType.Name = "txtFuelType";
            this.txtFuelType.Size = new System.Drawing.Size(166, 25);
            this.txtFuelType.TabIndex = 17;
            // 
            // lblSource
            // 
            this.lblSource.Location = new System.Drawing.Point(15, 334);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(72, 16);
            this.lblSource.TabIndex = 1;
            this.lblSource.Text = "Vendor Type";
            // 
            // lblFuelType
            // 
            this.lblFuelType.Location = new System.Drawing.Point(15, 366);
            this.lblFuelType.Name = "lblFuelType";
            this.lblFuelType.Size = new System.Drawing.Size(72, 16);
            this.lblFuelType.TabIndex = 1;
            this.lblFuelType.Text = "Service Type";
            // 
            // CmbVendorType
            // 
            this.CmbVendorType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbVendorType.FormattingEnabled = true;
            this.CmbVendorType.Location = new System.Drawing.Point(159, 330);
            this.CmbVendorType.Name = "CmbVendorType";
            this.CmbVendorType.Size = new System.Drawing.Size(166, 25);
            this.CmbVendorType.TabIndex = 16;
            // 
            // lblphone
            // 
            this.lblphone.Location = new System.Drawing.Point(15, 303);
            this.lblphone.Name = "lblphone";
            this.lblphone.Size = new System.Drawing.Size(61, 16);
            this.lblphone.TabIndex = 0;
            this.lblphone.Text = "Telephone";
            // 
            // maskPhone
            // 
            this.maskPhone.Location = new System.Drawing.Point(159, 298);
            this.maskPhone.Mask = "999-000-0000";
            this.maskPhone.Name = "maskPhone";
            this.maskPhone.Size = new System.Drawing.Size(88, 25);
            this.maskPhone.TabIndex = 14;
            // 
            // maskFax
            // 
            this.maskFax.Location = new System.Drawing.Point(335, 298);
            this.maskFax.Mask = "999-000-0000";
            this.maskFax.Name = "maskFax";
            this.maskFax.Size = new System.Drawing.Size(88, 25);
            this.maskFax.TabIndex = 15;
            // 
            // lblFax
            // 
            this.lblFax.Location = new System.Drawing.Point(307, 303);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(20, 14);
            this.lblFax.TabIndex = 0;
            this.lblFax.Text = "Fax";
            // 
            // btnZipSearch
            // 
            this.btnZipSearch.Location = new System.Drawing.Point(252, 267);
            this.btnZipSearch.Name = "btnZipSearch";
            this.btnZipSearch.Size = new System.Drawing.Size(74, 25);
            this.btnZipSearch.TabIndex = 13;
            this.btnZipSearch.Text = "S&earch";
            this.btnZipSearch.Click += new System.EventHandler(this.btnZipSearch_Click);
            // 
            // lblZip
            // 
            this.lblZip.Location = new System.Drawing.Point(15, 271);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(19, 14);
            this.lblZip.TabIndex = 0;
            this.lblZip.Text = "ZIP";
            // 
            // txtZip
            // 
            this.txtZip.Location = new System.Drawing.Point(159, 266);
            this.txtZip.MaxLength = 5;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(45, 25);
            this.txtZip.TabIndex = 11;
            this.txtZip.Leave += new System.EventHandler(this.txtZip_Leave);
            // 
            // txtZipPlus
            // 
            this.txtZipPlus.Location = new System.Drawing.Point(211, 266);
            this.txtZipPlus.MaxLength = 4;
            this.txtZipPlus.Name = "txtZipPlus";
            this.txtZipPlus.Size = new System.Drawing.Size(37, 25);
            this.txtZipPlus.TabIndex = 12;
            this.txtZipPlus.Leave += new System.EventHandler(this.txtZipPlus_Leave);
            // 
            // lblState
            // 
            this.lblState.Location = new System.Drawing.Point(349, 238);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(30, 14);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "State";
            // 
            // txtCity
            // 
            this.txtCity.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCity.Location = new System.Drawing.Point(159, 234);
            this.txtCity.MaxLength = 20;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(166, 25);
            this.txtCity.TabIndex = 9;
            // 
            // txtAddr2
            // 
            this.txtAddr2.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtAddr2.Location = new System.Drawing.Point(159, 202);
            this.txtAddr2.MaxLength = 35;
            this.txtAddr2.Name = "txtAddr2";
            this.txtAddr2.Size = new System.Drawing.Size(263, 25);
            this.txtAddr2.TabIndex = 8;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(15, 174);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(91, 16);
            this.lblAddress.TabIndex = 0;
            this.lblAddress.Text = "Mailing Address";
            // 
            // txtAddr1
            // 
            this.txtAddr1.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtAddr1.Location = new System.Drawing.Point(159, 170);
            this.txtAddr1.MaxLength = 35;
            this.txtAddr1.Name = "txtAddr1";
            this.txtAddr1.Size = new System.Drawing.Size(263, 25);
            this.txtAddr1.TabIndex = 7;
            // 
            // txtStreet
            // 
            this.txtStreet.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtStreet.Location = new System.Drawing.Point(159, 138);
            this.txtStreet.MaxLength = 35;
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(263, 25);
            this.txtStreet.TabIndex = 6;
            // 
            // lblStreet
            // 
            this.lblStreet.Location = new System.Drawing.Point(15, 142);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(36, 14);
            this.lblStreet.TabIndex = 0;
            this.lblStreet.Text = "Street";
            // 
            // lblIndexby
            // 
            this.lblIndexby.Location = new System.Drawing.Point(15, 110);
            this.lblIndexby.Name = "lblIndexby";
            this.lblIndexby.Size = new System.Drawing.Size(50, 16);
            this.lblIndexby.TabIndex = 0;
            this.lblIndexby.Text = "Index By";
            // 
            // txtIndexBy
            // 
            this.txtIndexBy.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtIndexBy.Location = new System.Drawing.Point(159, 106);
            this.txtIndexBy.MaxLength = 25;
            this.txtIndexBy.Name = "txtIndexBy";
            this.txtIndexBy.Size = new System.Drawing.Size(264, 25);
            this.txtIndexBy.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(15, 78);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(79, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Vendor Name";
            // 
            // txtName
            // 
            this.txtName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtName.Location = new System.Drawing.Point(159, 74);
            this.txtName.MaxLength = 35;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(264, 25);
            this.txtName.TabIndex = 4;
            // 
            // lblSecCode
            // 
            this.lblSecCode.Location = new System.Drawing.Point(15, 47);
            this.lblSecCode.Name = "lblSecCode";
            this.lblSecCode.Size = new System.Drawing.Size(137, 16);
            this.lblSecCode.TabIndex = 0;
            this.lblSecCode.Text = "Secondary Vendor Code";
            // 
            // txtSecCode
            // 
            this.txtSecCode.Location = new System.Drawing.Point(159, 42);
            this.txtSecCode.MaxLength = 10;
            this.txtSecCode.Name = "txtSecCode";
            this.txtSecCode.Size = new System.Drawing.Size(78, 25);
            this.txtSecCode.TabIndex = 3;
            this.txtSecCode.Leave += new System.EventHandler(this.txtSecCode_Leave);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(159, 10);
            this.txtCode.MaxLength = 10;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(78, 25);
            this.txtCode.TabIndex = 1;
            this.txtCode.Leave += new System.EventHandler(this.txtCode_Leave);
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(15, 14);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(75, 14);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "Vendor Code";
            // 
            // lblReqCity
            // 
            this.lblReqCity.ForeColor = System.Drawing.Color.Red;
            this.lblReqCity.Location = new System.Drawing.Point(73, 235);
            this.lblReqCity.Name = "lblReqCity";
            this.lblReqCity.Size = new System.Drawing.Size(11, 10);
            this.lblReqCity.TabIndex = 13;
            this.lblReqCity.Text = "*";
            // 
            // lblCity
            // 
            this.lblCity.Location = new System.Drawing.Point(15, 238);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(59, 16);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "City/Town";
            // 
            // pnlContactDetails
            // 
            this.pnlContactDetails.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlContactDetails.Controls.Add(this.maskEIN);
            this.pnlContactDetails.Controls.Add(this.txtOrginization);
            this.pnlContactDetails.Controls.Add(this.lblOrganization);
            this.pnlContactDetails.Controls.Add(this.cmbEinSSN);
            this.pnlContactDetails.Controls.Add(this.lblLast);
            this.pnlContactDetails.Controls.Add(this.txtLast);
            this.pnlContactDetails.Controls.Add(this.txtFirst);
            this.pnlContactDetails.Controls.Add(this.lblFirst);
            this.pnlContactDetails.Controls.Add(this.chkbW9);
            this.pnlContactDetails.Controls.Add(this.lblEmail);
            this.pnlContactDetails.Controls.Add(this.txtEmail);
            this.pnlContactDetails.Controls.Add(this.chkbContAgree);
            this.pnlContactDetails.Controls.Add(this.lblReqAccFormat);
            this.pnlContactDetails.Controls.Add(this.lblReqEIN);
            this.pnlContactDetails.Controls.Add(this.lblAccFormat);
            this.pnlContactDetails.Controls.Add(this.txtAccFormat);
            this.pnlContactDetails.Controls.Add(this.lblCycleCode);
            this.pnlContactDetails.Controls.Add(this.CmbCycle);
            this.pnlContactDetails.Controls.Add(this.Cmb1099);
            this.pnlContactDetails.Controls.Add(this.lbl1099);
            this.pnlContactDetails.Controls.Add(this.lblEleTrans);
            this.pnlContactDetails.Controls.Add(this.cmbElecTrans);
            this.pnlContactDetails.Controls.Add(this.cmbFixedMargin);
            this.pnlContactDetails.Controls.Add(this.lblFixedMargin);
            this.pnlContactDetails.Controls.Add(this.btnParVendor);
            this.pnlContactDetails.Controls.Add(this.txtParVendor);
            this.pnlContactDetails.Controls.Add(this.lblParentVendor);
            this.pnlContactDetails.Controls.Add(this.txtParVenName);
            this.pnlContactDetails.Controls.Add(this.txtContName);
            this.pnlContactDetails.Controls.Add(this.lblContName);
            this.pnlContactDetails.Controls.Add(this.maskContPhone);
            this.pnlContactDetails.Controls.Add(this.lblContPhone);
            this.pnlContactDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlContactDetails.Location = new System.Drawing.Point(450, 0);
            this.pnlContactDetails.Name = "pnlContactDetails";
            this.pnlContactDetails.Size = new System.Drawing.Size(359, 428);
            this.pnlContactDetails.TabIndex = 2;
            // 
            // maskEIN
            // 
            this.maskEIN.Location = new System.Drawing.Point(249, 202);
            this.maskEIN.Mask = "999-00-0000";
            this.maskEIN.Name = "maskEIN";
            this.maskEIN.Size = new System.Drawing.Size(82, 25);
            this.maskEIN.TabIndex = 10;
            // 
            // txtOrginization
            // 
            this.txtOrginization.CharacterCasing = Wisej.Web.CharacterCasing.Lower;
            this.txtOrginization.Location = new System.Drawing.Point(106, 394);
            this.txtOrginization.MaxLength = 15;
            this.txtOrginization.Name = "txtOrginization";
            this.txtOrginization.Size = new System.Drawing.Size(225, 25);
            this.txtOrginization.TabIndex = 18;
            // 
            // lblOrganization
            // 
            this.lblOrganization.Location = new System.Drawing.Point(15, 398);
            this.lblOrganization.Name = "lblOrganization";
            this.lblOrganization.Size = new System.Drawing.Size(74, 16);
            this.lblOrganization.TabIndex = 0;
            this.lblOrganization.Text = "Organization ";
            // 
            // cmbEinSSN
            // 
            this.cmbEinSSN.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbEinSSN.Enabled = false;
            this.cmbEinSSN.FormattingEnabled = true;
            this.cmbEinSSN.Location = new System.Drawing.Point(169, 202);
            this.cmbEinSSN.Name = "cmbEinSSN";
            this.cmbEinSSN.Size = new System.Drawing.Size(51, 25);
            this.cmbEinSSN.TabIndex = 9;
            this.cmbEinSSN.SelectedIndexChanged += new System.EventHandler(this.cmbEinSSN_SelectedIndexChanged);
            // 
            // lblLast
            // 
            this.lblLast.Location = new System.Drawing.Point(72, 366);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(25, 14);
            this.lblLast.TabIndex = 0;
            this.lblLast.Text = "Last";
            this.lblLast.Visible = false;
            // 
            // txtLast
            // 
            this.txtLast.Location = new System.Drawing.Point(106, 362);
            this.txtLast.MaxLength = 20;
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(225, 25);
            this.txtLast.TabIndex = 17;
            this.txtLast.Visible = false;
            this.txtLast.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txtFirst
            // 
            this.txtFirst.Location = new System.Drawing.Point(106, 330);
            this.txtFirst.MaxLength = 20;
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(225, 25);
            this.txtFirst.TabIndex = 16;
            this.txtFirst.Visible = false;
            // 
            // lblFirst
            // 
            this.lblFirst.Location = new System.Drawing.Point(72, 334);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(28, 14);
            this.lblFirst.TabIndex = 0;
            this.lblFirst.Text = "First";
            this.lblFirst.Visible = false;
            // 
            // chkbW9
            // 
            this.chkbW9.AutoSize = false;
            this.chkbW9.Location = new System.Drawing.Point(12, 331);
            this.chkbW9.Name = "chkbW9";
            this.chkbW9.Size = new System.Drawing.Size(54, 20);
            this.chkbW9.TabIndex = 15;
            this.chkbW9.Text = "W-9";
            this.chkbW9.CheckedChanged += new System.EventHandler(this.chkbW9_CheckedChanged);
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(15, 303);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 14);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.CharacterCasing = Wisej.Web.CharacterCasing.Lower;
            this.txtEmail.Location = new System.Drawing.Point(106, 298);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(225, 25);
            this.txtEmail.TabIndex = 14;
            // 
            // chkbContAgree
            // 
            this.chkbContAgree.AutoSize = false;
            this.chkbContAgree.Location = new System.Drawing.Point(102, 268);
            this.chkbContAgree.Name = "chkbContAgree";
            this.chkbContAgree.Size = new System.Drawing.Size(194, 20);
            this.chkbContAgree.TabIndex = 13;
            this.chkbContAgree.Text = "Contact/Agreement Returned";
            // 
            // lblReqAccFormat
            // 
            this.lblReqAccFormat.ForeColor = System.Drawing.Color.Red;
            this.lblReqAccFormat.Location = new System.Drawing.Point(288, 234);
            this.lblReqAccFormat.Name = "lblReqAccFormat";
            this.lblReqAccFormat.Size = new System.Drawing.Size(10, 10);
            this.lblReqAccFormat.TabIndex = 13;
            this.lblReqAccFormat.Text = "*";
            this.lblReqAccFormat.Visible = false;
            // 
            // lblReqEIN
            // 
            this.lblReqEIN.ForeColor = System.Drawing.Color.Red;
            this.lblReqEIN.Location = new System.Drawing.Point(221, 197);
            this.lblReqEIN.Name = "lblReqEIN";
            this.lblReqEIN.Size = new System.Drawing.Size(10, 10);
            this.lblReqEIN.TabIndex = 13;
            this.lblReqEIN.Text = "*";
            this.lblReqEIN.Visible = false;
            // 
            // lblAccFormat
            // 
            this.lblAccFormat.Location = new System.Drawing.Point(196, 238);
            this.lblAccFormat.Name = "lblAccFormat";
            this.lblAccFormat.Size = new System.Drawing.Size(91, 14);
            this.lblAccFormat.TabIndex = 0;
            this.lblAccFormat.Text = "Account Format";
            // 
            // txtAccFormat
            // 
            this.txtAccFormat.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtAccFormat.Enabled = false;
            this.txtAccFormat.Location = new System.Drawing.Point(300, 234);
            this.txtAccFormat.MaxLength = 2;
            this.txtAccFormat.Name = "txtAccFormat";
            this.txtAccFormat.Size = new System.Drawing.Size(31, 25);
            this.txtAccFormat.TabIndex = 12;
            // 
            // lblCycleCode
            // 
            this.lblCycleCode.Location = new System.Drawing.Point(15, 238);
            this.lblCycleCode.Name = "lblCycleCode";
            this.lblCycleCode.Size = new System.Drawing.Size(63, 16);
            this.lblCycleCode.TabIndex = 1;
            this.lblCycleCode.Text = "Cycle Code";
            // 
            // CmbCycle
            // 
            this.CmbCycle.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbCycle.FormattingEnabled = true;
            this.CmbCycle.Location = new System.Drawing.Point(106, 234);
            this.CmbCycle.Name = "CmbCycle";
            this.CmbCycle.Size = new System.Drawing.Size(48, 25);
            this.CmbCycle.TabIndex = 11;
            // 
            // Cmb1099
            // 
            this.Cmb1099.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb1099.FormattingEnabled = true;
            this.Cmb1099.Location = new System.Drawing.Point(106, 202);
            this.Cmb1099.Name = "Cmb1099";
            this.Cmb1099.Size = new System.Drawing.Size(48, 25);
            this.Cmb1099.TabIndex = 8;
            this.Cmb1099.SelectedIndexChanged += new System.EventHandler(this.Cmb1099_SelectedIndexChanged);
            // 
            // lbl1099
            // 
            this.lbl1099.Location = new System.Drawing.Point(15, 207);
            this.lbl1099.Name = "lbl1099";
            this.lbl1099.Size = new System.Drawing.Size(51, 14);
            this.lbl1099.TabIndex = 1;
            this.lbl1099.Text = "1099 Cd";
            // 
            // lblEleTrans
            // 
            this.lblEleTrans.Location = new System.Drawing.Point(170, 174);
            this.lblEleTrans.Name = "lblEleTrans";
            this.lblEleTrans.Size = new System.Drawing.Size(108, 14);
            this.lblEleTrans.TabIndex = 1;
            this.lblEleTrans.Text = "Electronic Transfer";
            // 
            // cmbElecTrans
            // 
            this.cmbElecTrans.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbElecTrans.FormattingEnabled = true;
            this.cmbElecTrans.Location = new System.Drawing.Point(283, 170);
            this.cmbElecTrans.Name = "cmbElecTrans";
            this.cmbElecTrans.Size = new System.Drawing.Size(48, 25);
            this.cmbElecTrans.TabIndex = 7;
            this.cmbElecTrans.SelectedIndexChanged += new System.EventHandler(this.cmbElecTrans_SelectedIndexChanged);
            // 
            // cmbFixedMargin
            // 
            this.cmbFixedMargin.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbFixedMargin.FormattingEnabled = true;
            this.cmbFixedMargin.Location = new System.Drawing.Point(106, 170);
            this.cmbFixedMargin.Name = "cmbFixedMargin";
            this.cmbFixedMargin.Size = new System.Drawing.Size(38, 25);
            this.cmbFixedMargin.TabIndex = 6;
            // 
            // lblFixedMargin
            // 
            this.lblFixedMargin.Location = new System.Drawing.Point(15, 174);
            this.lblFixedMargin.Name = "lblFixedMargin";
            this.lblFixedMargin.Size = new System.Drawing.Size(74, 16);
            this.lblFixedMargin.TabIndex = 1;
            this.lblFixedMargin.Text = "Fixed Margin";
            // 
            // btnParVendor
            // 
            this.btnParVendor.Location = new System.Drawing.Point(200, 74);
            this.btnParVendor.Name = "btnParVendor";
            this.btnParVendor.Size = new System.Drawing.Size(75, 25);
            this.btnParVendor.TabIndex = 4;
            this.btnParVendor.Text = "Se&arch";
            this.btnParVendor.Click += new System.EventHandler(this.btnParVendor_Click);
            // 
            // txtParVendor
            // 
            this.txtParVendor.Location = new System.Drawing.Point(106, 74);
            this.txtParVendor.MaxLength = 10;
            this.txtParVendor.Name = "txtParVendor";
            this.txtParVendor.Size = new System.Drawing.Size(87, 25);
            this.txtParVendor.TabIndex = 3;
            this.txtParVendor.Leave += new System.EventHandler(this.txtParVendor_Leave);
            // 
            // lblParentVendor
            // 
            this.lblParentVendor.Location = new System.Drawing.Point(15, 78);
            this.lblParentVendor.Name = "lblParentVendor";
            this.lblParentVendor.Size = new System.Drawing.Size(83, 14);
            this.lblParentVendor.TabIndex = 0;
            this.lblParentVendor.Text = "Parent Vendor";
            // 
            // txtParVenName
            // 
            this.txtParVenName.Enabled = false;
            this.txtParVenName.Location = new System.Drawing.Point(106, 106);
            this.txtParVenName.MaxLength = 10;
            this.txtParVenName.Name = "txtParVenName";
            this.txtParVenName.Size = new System.Drawing.Size(225, 25);
            this.txtParVenName.TabIndex = 5;
            // 
            // txtContName
            // 
            this.txtContName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtContName.Location = new System.Drawing.Point(106, 10);
            this.txtContName.MaxLength = 35;
            this.txtContName.Name = "txtContName";
            this.txtContName.Size = new System.Drawing.Size(225, 25);
            this.txtContName.TabIndex = 1;
            // 
            // lblContName
            // 
            this.lblContName.Location = new System.Drawing.Point(15, 14);
            this.lblContName.Name = "lblContName";
            this.lblContName.Size = new System.Drawing.Size(82, 14);
            this.lblContName.TabIndex = 0;
            this.lblContName.Text = "Contact Name";
            // 
            // maskContPhone
            // 
            this.maskContPhone.Location = new System.Drawing.Point(106, 42);
            this.maskContPhone.Mask = "999-000-0000";
            this.maskContPhone.Name = "maskContPhone";
            this.maskContPhone.Size = new System.Drawing.Size(88, 25);
            this.maskContPhone.TabIndex = 2;
            // 
            // lblContPhone
            // 
            this.lblContPhone.Location = new System.Drawing.Point(15, 47);
            this.lblContPhone.Name = "lblContPhone";
            this.lblContPhone.Size = new System.Drawing.Size(39, 14);
            this.lblContPhone.TabIndex = 0;
            this.lblContPhone.Text = "Phone";
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(719, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(641, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 428);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(809, 35);
            this.pnlSave.TabIndex = 3;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(716, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlVendorMaintainance);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(809, 463);
            this.pnlCompleteForm.TabIndex = 21;
            // 
            // pnlVendorMaintainance
            // 
            this.pnlVendorMaintainance.Controls.Add(this.pnlContactDetails);
            this.pnlVendorMaintainance.Controls.Add(this.pnlVendorCode);
            this.pnlVendorMaintainance.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlVendorMaintainance.Location = new System.Drawing.Point(0, 0);
            this.pnlVendorMaintainance.Name = "pnlVendorMaintainance";
            this.pnlVendorMaintainance.Size = new System.Drawing.Size(809, 428);
            this.pnlVendorMaintainance.TabIndex = 0;
            // 
            // VendorMaintenance_Form
            // 
            this.ClientSize = new System.Drawing.Size(809, 463);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VendorMaintenance_Form";
            this.Text = "Vendor Maintenance";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.VendorMaintenance_Form_ToolClick);
            this.pnlVendorCode.ResumeLayout(false);
            this.pnlVendorCode.PerformLayout();
            this.pnlContactDetails.ResumeLayout(false);
            this.pnlContactDetails.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlVendorMaintainance.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlVendorCode;
        private TextBoxWithValidation txtCode;
        private Label lblCode;
        private Label lblSecCode;
        private TextBoxWithValidation txtSecCode;
        private Label lblName;
        private TextBox txtName;
        private Label lblIndexby;
        private TextBox txtIndexBy;
        private TextBox txtAddr2;
        private Label lblAddress;
        private TextBox txtAddr1;
        private TextBox txtStreet;
        private Label lblStreet;
        private Label lblCity;
        private TextBox txtCity;
        private TextBox txtState;
        private Label lblState;
        private Label lblZip;
        private TextBoxWithValidation txtZip;
        private TextBoxWithValidation txtZipPlus;
        private Button btnZipSearch;
        private Label lblphone;
        private MaskedTextBox maskPhone;
        private MaskedTextBox maskFax;
        private Label lblFax;
        private Label lblSource;
        private Label lblFuelType;
        private ComboBox CmbVendorType;
        private Panel pnlContactDetails;
        private TextBoxWithValidation txtParVendor;
        private Label lblParentVendor;
        private TextBox txtParVenName;
        private TextBox txtContName;
        private Label lblContName;
        private MaskedTextBox maskContPhone;
        private Label lblContPhone;
        private Button btnParVendor;
        private ComboBox cmbFixedMargin;
        private Label lblFixedMargin;
        private Label lblEleTrans;
        private ComboBox cmbElecTrans;
        private Label lblCycleCode;
        private ComboBox CmbCycle;
        private ComboBox Cmb1099;
        private Label lbl1099;
        private Label lblAccFormat;
        private TextBoxWithValidation txtAccFormat;
        private Button btnCancel;
        private Button btnSave;
        private Panel pnlSave;
        private TextBox txtFuelType;
        private Button btnFuelTypes;
        private Label lblserviceTypeReq;
        private Label lblReqZIP;
        private Label lblReqState;
        private Label lblReqCity;
        private Label lblReqStreet;
        private Label lblReqVendorName;
        private Label lblReqAccFormat;
        private Label lblReqEIN;
        private CheckBox chkbContAgree;
        private CheckBox ChkbActive;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblReqVendorCode;
        private Label lblLast;
        private TextBox txtLast;
        private TextBox txtFirst;
        private Label lblFirst;
        private CheckBox chkbW9;
        private ComboBox cmbEinSSN;
        private TextBox txtOrginization;
        private Label lblOrganization;
        private Panel pnlCompleteForm;
        private Panel pnlVendorMaintainance;
        private Spacer spacer1;
        private MaskedTextBox maskEIN;
        private ComboBox cmbAgency;
        private Label lblAgency;
    }
}