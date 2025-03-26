using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class DiffMailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiffMailForm));
            this.lblFirstReq = new Wisej.Web.Label();
            this.lblDiffHouseNoReq = new Wisej.Web.Label();
            this.lblDiffStateReq = new Wisej.Web.Label();
            this.lblDiffCityReq = new Wisej.Web.Label();
            this.lblLastReq = new Wisej.Web.Label();
            this.lblDiffStreetReq = new Wisej.Web.Label();
            this.lblLast = new Wisej.Web.Label();
            this.lblFirst = new Wisej.Web.Label();
            this.lblPhone = new Wisej.Web.Label();
            this.lblZipCode = new Wisej.Web.Label();
            this.lblCityName = new Wisej.Web.Label();
            this.lblApartment = new Wisej.Web.Label();
            this.lblFloor = new Wisej.Web.Label();
            this.lblHouseNo = new Wisej.Web.Label();
            this.lblState = new Wisej.Web.Label();
            this.lblSuffix = new Wisej.Web.Label();
            this.lblStreet = new Wisej.Web.Label();
            this.lblCounty = new Wisej.Web.Label();
            this.panel6 = new Wisej.Web.Panel();
            this.txtMailPhone = new Wisej.Web.MaskedTextBox();
            this.lblApartmentReq = new Wisej.Web.Label();
            this.lblPhoneReq = new Wisej.Web.Label();
            this.lblSuffixReq = new Wisej.Web.Label();
            this.lblFloorReq = new Wisej.Web.Label();
            this.cmbMailCounty = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblCountyReq = new Wisej.Web.Label();
            this.lblZipCodeReq = new Wisej.Web.Label();
            this.txtLast = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtMailState = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtFirst = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtZipPlus = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.btnZipSearch = new Wisej.Web.Button();
            this.txtZip = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtMailApartment = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtMailFloor = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtCityName = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtMailSuffix = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtHouseNo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtMailStreet = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.lblMessage = new Wisej.Web.Label();
            this.panel6.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFirstReq
            // 
            this.lblFirstReq.AutoSize = true;
            this.lblFirstReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblFirstReq.Location = new System.Drawing.Point(13, 14);
            this.lblFirstReq.Name = "lblFirstReq";
            this.lblFirstReq.Size = new System.Drawing.Size(9, 14);
            this.lblFirstReq.TabIndex = 28;
            this.lblFirstReq.Text = "*";
            this.lblFirstReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblFirstReq.Visible = false;
            // 
            // lblDiffHouseNoReq
            // 
            this.lblDiffHouseNoReq.AutoSize = true;
            this.lblDiffHouseNoReq.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDiffHouseNoReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblDiffHouseNoReq.Location = new System.Drawing.Point(13, 47);
            this.lblDiffHouseNoReq.Name = "lblDiffHouseNoReq";
            this.lblDiffHouseNoReq.Size = new System.Drawing.Size(9, 14);
            this.lblDiffHouseNoReq.TabIndex = 28;
            this.lblDiffHouseNoReq.Text = "*";
            this.lblDiffHouseNoReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDiffHouseNoReq.Visible = false;
            // 
            // lblDiffStateReq
            // 
            this.lblDiffStateReq.AutoSize = true;
            this.lblDiffStateReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblDiffStateReq.Location = new System.Drawing.Point(673, 84);
            this.lblDiffStateReq.Name = "lblDiffStateReq";
            this.lblDiffStateReq.Size = new System.Drawing.Size(9, 14);
            this.lblDiffStateReq.TabIndex = 28;
            this.lblDiffStateReq.Text = "*";
            this.lblDiffStateReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDiffStateReq.Visible = false;
            // 
            // lblDiffCityReq
            // 
            this.lblDiffCityReq.AutoSize = true;
            this.lblDiffCityReq.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDiffCityReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblDiffCityReq.Location = new System.Drawing.Point(13, 80);
            this.lblDiffCityReq.Name = "lblDiffCityReq";
            this.lblDiffCityReq.Size = new System.Drawing.Size(9, 14);
            this.lblDiffCityReq.TabIndex = 28;
            this.lblDiffCityReq.Text = "*";
            this.lblDiffCityReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDiffCityReq.Visible = false;
            // 
            // lblLastReq
            // 
            this.lblLastReq.AutoSize = true;
            this.lblLastReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblLastReq.Location = new System.Drawing.Point(432, 16);
            this.lblLastReq.Name = "lblLastReq";
            this.lblLastReq.Size = new System.Drawing.Size(9, 14);
            this.lblLastReq.TabIndex = 28;
            this.lblLastReq.Text = "*";
            this.lblLastReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblLastReq.Visible = false;
            // 
            // lblDiffStreetReq
            // 
            this.lblDiffStreetReq.AutoSize = true;
            this.lblDiffStreetReq.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDiffStreetReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblDiffStreetReq.Location = new System.Drawing.Point(198, 46);
            this.lblDiffStreetReq.Name = "lblDiffStreetReq";
            this.lblDiffStreetReq.Size = new System.Drawing.Size(9, 14);
            this.lblDiffStreetReq.TabIndex = 28;
            this.lblDiffStreetReq.Text = "*";
            this.lblDiffStreetReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDiffStreetReq.Visible = false;
            // 
            // lblLast
            // 
            this.lblLast.AutoSize = true;
            this.lblLast.Enabled = false;
            this.lblLast.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblLast.Location = new System.Drawing.Point(444, 15);
            this.lblLast.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblLast.MinimumSize = new System.Drawing.Size(90, 18);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(90, 18);
            this.lblLast.TabIndex = 62;
            this.lblLast.Text = "In Care of Last";
            // 
            // lblFirst
            // 
            this.lblFirst.AutoSize = true;
            this.lblFirst.Enabled = false;
            this.lblFirst.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFirst.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblFirst.Location = new System.Drawing.Point(24, 15);
            this.lblFirst.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblFirst.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(83, 18);
            this.lblFirst.TabIndex = 61;
            this.lblFirst.Text = "In Care of First";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Enabled = false;
            this.lblPhone.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblPhone.Location = new System.Drawing.Point(641, 120);
            this.lblPhone.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblPhone.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(39, 18);
            this.lblPhone.TabIndex = 59;
            this.lblPhone.Text = "Phone";
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Enabled = false;
            this.lblZipCode.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblZipCode.Location = new System.Drawing.Point(444, 85);
            this.lblZipCode.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblZipCode.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(23, 18);
            this.lblZipCode.TabIndex = 34;
            this.lblZipCode.Text = "ZIP";
            // 
            // lblCityName
            // 
            this.lblCityName.AutoSize = true;
            this.lblCityName.Enabled = false;
            this.lblCityName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCityName.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblCityName.Location = new System.Drawing.Point(24, 83);
            this.lblCityName.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblCityName.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblCityName.Name = "lblCityName";
            this.lblCityName.Size = new System.Drawing.Size(60, 18);
            this.lblCityName.TabIndex = 35;
            this.lblCityName.Text = "City Name";
            // 
            // lblApartment
            // 
            this.lblApartment.AutoSize = true;
            this.lblApartment.Enabled = false;
            this.lblApartment.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblApartment.Location = new System.Drawing.Point(565, 52);
            this.lblApartment.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblApartment.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblApartment.Name = "lblApartment";
            this.lblApartment.Size = new System.Drawing.Size(23, 18);
            this.lblApartment.TabIndex = 30;
            this.lblApartment.Text = "Apt";
            // 
            // lblFloor
            // 
            this.lblFloor.AutoSize = true;
            this.lblFloor.Enabled = false;
            this.lblFloor.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblFloor.Location = new System.Drawing.Point(684, 52);
            this.lblFloor.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblFloor.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFloor.Name = "lblFloor";
            this.lblFloor.Size = new System.Drawing.Size(19, 18);
            this.lblFloor.TabIndex = 25;
            this.lblFloor.Text = "Flr";
            // 
            // lblHouseNo
            // 
            this.lblHouseNo.AutoSize = true;
            this.lblHouseNo.Enabled = false;
            this.lblHouseNo.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHouseNo.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblHouseNo.Location = new System.Drawing.Point(24, 49);
            this.lblHouseNo.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblHouseNo.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblHouseNo.Name = "lblHouseNo";
            this.lblHouseNo.Size = new System.Drawing.Size(46, 18);
            this.lblHouseNo.TabIndex = 2;
            this.lblHouseNo.Text = "House#";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblState.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblState.Location = new System.Drawing.Point(683, 86);
            this.lblState.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblState.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(33, 18);
            this.lblState.TabIndex = 56;
            this.lblState.Text = "State";
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Enabled = false;
            this.lblSuffix.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblSuffix.Location = new System.Drawing.Point(444, 50);
            this.lblSuffix.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblSuffix.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(35, 18);
            this.lblSuffix.TabIndex = 4;
            this.lblSuffix.Text = "Suffix";
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Enabled = false;
            this.lblStreet.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStreet.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblStreet.Location = new System.Drawing.Point(207, 52);
            this.lblStreet.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblStreet.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(37, 18);
            this.lblStreet.TabIndex = 10;
            this.lblStreet.Text = "Street";
            // 
            // lblCounty
            // 
            this.lblCounty.AutoSize = true;
            this.lblCounty.Enabled = false;
            this.lblCounty.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCounty.ForeColor = System.Drawing.Color.FromArgb(252, 0, 0, 0);
            this.lblCounty.Location = new System.Drawing.Point(24, 117);
            this.lblCounty.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblCounty.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(43, 18);
            this.lblCounty.TabIndex = 17;
            this.lblCounty.Text = "County";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtMailPhone);
            this.panel6.Controls.Add(this.lblApartmentReq);
            this.panel6.Controls.Add(this.lblPhoneReq);
            this.panel6.Controls.Add(this.lblSuffixReq);
            this.panel6.Controls.Add(this.lblFloorReq);
            this.panel6.Controls.Add(this.lblFirstReq);
            this.panel6.Controls.Add(this.lblPhone);
            this.panel6.Controls.Add(this.cmbMailCounty);
            this.panel6.Controls.Add(this.lblCountyReq);
            this.panel6.Controls.Add(this.lblDiffHouseNoReq);
            this.panel6.Controls.Add(this.lblCounty);
            this.panel6.Controls.Add(this.lblLastReq);
            this.panel6.Controls.Add(this.lblDiffStreetReq);
            this.panel6.Controls.Add(this.lblZipCodeReq);
            this.panel6.Controls.Add(this.txtLast);
            this.panel6.Controls.Add(this.txtMailState);
            this.panel6.Controls.Add(this.txtFirst);
            this.panel6.Controls.Add(this.txtZipPlus);
            this.panel6.Controls.Add(this.lblLast);
            this.panel6.Controls.Add(this.lblState);
            this.panel6.Controls.Add(this.lblFirst);
            this.panel6.Controls.Add(this.btnZipSearch);
            this.panel6.Controls.Add(this.txtZip);
            this.panel6.Controls.Add(this.txtMailApartment);
            this.panel6.Controls.Add(this.lblZipCode);
            this.panel6.Controls.Add(this.lblCityName);
            this.panel6.Controls.Add(this.lblApartment);
            this.panel6.Controls.Add(this.lblDiffStateReq);
            this.panel6.Controls.Add(this.txtMailFloor);
            this.panel6.Controls.Add(this.txtCityName);
            this.panel6.Controls.Add(this.txtMailSuffix);
            this.panel6.Controls.Add(this.lblFloor);
            this.panel6.Controls.Add(this.lblHouseNo);
            this.panel6.Controls.Add(this.txtHouseNo);
            this.panel6.Controls.Add(this.lblDiffCityReq);
            this.panel6.Controls.Add(this.lblSuffix);
            this.panel6.Controls.Add(this.txtMailStreet);
            this.panel6.Controls.Add(this.lblStreet);
            this.panel6.Dock = Wisej.Web.DockStyle.Fill;
            this.panel6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.panel6.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.panel6.Location = new System.Drawing.Point(0, 24);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(818, 154);
            this.panel6.TabIndex = 0;
            this.panel6.TabStop = true;
            this.panel6.Tag = "/";
            this.panel6.Text = "*";
            // 
            // txtMailPhone
            // 
            this.txtMailPhone.Enabled = false;
            this.txtMailPhone.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMailPhone.Location = new System.Drawing.Point(695, 117);
            this.txtMailPhone.Mask = "999-000-0000";
            this.txtMailPhone.Name = "txtMailPhone";
            this.txtMailPhone.Size = new System.Drawing.Size(90, 25);
            this.txtMailPhone.TabIndex = 14;
            this.txtMailPhone.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblApartmentReq
            // 
            this.lblApartmentReq.AutoSize = true;
            this.lblApartmentReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblApartmentReq.Location = new System.Drawing.Point(554, 47);
            this.lblApartmentReq.Name = "lblApartmentReq";
            this.lblApartmentReq.Size = new System.Drawing.Size(9, 14);
            this.lblApartmentReq.TabIndex = 28;
            this.lblApartmentReq.Text = "*";
            this.lblApartmentReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblApartmentReq.Visible = false;
            // 
            // lblPhoneReq
            // 
            this.lblPhoneReq.AutoSize = true;
            this.lblPhoneReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblPhoneReq.Location = new System.Drawing.Point(629, 120);
            this.lblPhoneReq.Name = "lblPhoneReq";
            this.lblPhoneReq.Size = new System.Drawing.Size(9, 14);
            this.lblPhoneReq.TabIndex = 28;
            this.lblPhoneReq.Text = "*";
            this.lblPhoneReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblPhoneReq.Visible = false;
            // 
            // lblSuffixReq
            // 
            this.lblSuffixReq.AutoSize = true;
            this.lblSuffixReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblSuffixReq.Location = new System.Drawing.Point(432, 49);
            this.lblSuffixReq.Name = "lblSuffixReq";
            this.lblSuffixReq.Size = new System.Drawing.Size(9, 14);
            this.lblSuffixReq.TabIndex = 28;
            this.lblSuffixReq.Text = "*";
            this.lblSuffixReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSuffixReq.Visible = false;
            // 
            // lblFloorReq
            // 
            this.lblFloorReq.AutoSize = true;
            this.lblFloorReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblFloorReq.Location = new System.Drawing.Point(673, 50);
            this.lblFloorReq.Name = "lblFloorReq";
            this.lblFloorReq.Size = new System.Drawing.Size(9, 14);
            this.lblFloorReq.TabIndex = 28;
            this.lblFloorReq.Text = "*";
            this.lblFloorReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblFloorReq.Visible = false;
            // 
            // cmbMailCounty
            // 
            this.cmbMailCounty.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMailCounty.Enabled = false;
            this.cmbMailCounty.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbMailCounty.FormattingEnabled = true;
            this.cmbMailCounty.Location = new System.Drawing.Point(116, 116);
            this.cmbMailCounty.Name = "cmbMailCounty";
            this.cmbMailCounty.Size = new System.Drawing.Size(295, 25);
            this.cmbMailCounty.TabIndex = 13;
            // 
            // lblCountyReq
            // 
            this.lblCountyReq.AutoSize = true;
            this.lblCountyReq.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCountyReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblCountyReq.Location = new System.Drawing.Point(13, 113);
            this.lblCountyReq.Name = "lblCountyReq";
            this.lblCountyReq.Size = new System.Drawing.Size(9, 14);
            this.lblCountyReq.TabIndex = 28;
            this.lblCountyReq.Text = "*";
            this.lblCountyReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCountyReq.Visible = false;
            // 
            // lblZipCodeReq
            // 
            this.lblZipCodeReq.AutoSize = true;
            this.lblZipCodeReq.ForeColor = System.Drawing.Color.FromArgb(252, 255, 0, 0);
            this.lblZipCodeReq.Location = new System.Drawing.Point(432, 82);
            this.lblZipCodeReq.Name = "lblZipCodeReq";
            this.lblZipCodeReq.Size = new System.Drawing.Size(9, 14);
            this.lblZipCodeReq.TabIndex = 28;
            this.lblZipCodeReq.Text = "*";
            this.lblZipCodeReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblZipCodeReq.Visible = false;
            // 
            // txtLast
            // 
            this.txtLast.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtLast.Enabled = false;
            this.txtLast.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLast.Location = new System.Drawing.Point(546, 13);
            this.txtLast.MaxLength = 30;
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(239, 25);
            this.txtLast.TabIndex = 2;
            // 
            // txtMailState
            // 
            this.txtMailState.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMailState.Enabled = false;
            this.txtMailState.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMailState.Location = new System.Drawing.Point(726, 82);
            this.txtMailState.MaxLength = 2;
            this.txtMailState.Name = "txtMailState";
            this.txtMailState.RightToLeft = Wisej.Web.RightToLeft.Yes;
            this.txtMailState.Size = new System.Drawing.Size(59, 25);
            this.txtMailState.TabIndex = 12;
            // 
            // txtFirst
            // 
            this.txtFirst.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFirst.Enabled = false;
            this.txtFirst.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFirst.Location = new System.Drawing.Point(116, 13);
            this.txtFirst.MaxLength = 30;
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(295, 25);
            this.txtFirst.TabIndex = 1;
            // 
            // txtZipPlus
            // 
            this.txtZipPlus.Enabled = false;
            this.txtZipPlus.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtZipPlus.Location = new System.Drawing.Point(546, 82);
            this.txtZipPlus.MaxLength = 4;
            this.txtZipPlus.Name = "txtZipPlus";
            this.txtZipPlus.Size = new System.Drawing.Size(54, 25);
            this.txtZipPlus.TabIndex = 10;
            // 
            // btnZipSearch
            // 
            this.btnZipSearch.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnZipSearch.Location = new System.Drawing.Point(606, 84);
            this.btnZipSearch.Name = "btnZipSearch";
            this.btnZipSearch.Size = new System.Drawing.Size(46, 22);
            this.btnZipSearch.TabIndex = 11;
            this.btnZipSearch.Text = "Zip";
            this.btnZipSearch.Click += new System.EventHandler(this.btnZipSearch_Click);
            // 
            // txtZip
            // 
            this.txtZip.Enabled = false;
            this.txtZip.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtZip.Location = new System.Drawing.Point(484, 82);
            this.txtZip.MaxLength = 5;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(59, 25);
            this.txtZip.TabIndex = 9;
            // 
            // txtMailApartment
            // 
            this.txtMailApartment.Enabled = false;
            this.txtMailApartment.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMailApartment.Location = new System.Drawing.Point(594, 48);
            this.txtMailApartment.MaxLength = 10;
            this.txtMailApartment.Name = "txtMailApartment";
            this.txtMailApartment.Size = new System.Drawing.Size(58, 25);
            this.txtMailApartment.TabIndex = 6;
            // 
            // txtMailFloor
            // 
            this.txtMailFloor.Enabled = false;
            this.txtMailFloor.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMailFloor.Location = new System.Drawing.Point(726, 48);
            this.txtMailFloor.MaxLength = 3;
            this.txtMailFloor.Name = "txtMailFloor";
            this.txtMailFloor.Size = new System.Drawing.Size(59, 25);
            this.txtMailFloor.TabIndex = 7;
            // 
            // txtCityName
            // 
            this.txtCityName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCityName.Enabled = false;
            this.txtCityName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCityName.Location = new System.Drawing.Point(116, 82);
            this.txtCityName.MaxLength = 30;
            this.txtCityName.Name = "txtCityName";
            this.txtCityName.Size = new System.Drawing.Size(295, 25);
            this.txtCityName.TabIndex = 8;
            // 
            // txtMailSuffix
            // 
            this.txtMailSuffix.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMailSuffix.Enabled = false;
            this.txtMailSuffix.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMailSuffix.Location = new System.Drawing.Point(483, 48);
            this.txtMailSuffix.MaxLength = 4;
            this.txtMailSuffix.Name = "txtMailSuffix";
            this.txtMailSuffix.Size = new System.Drawing.Size(60, 25);
            this.txtMailSuffix.TabIndex = 5;
            // 
            // txtHouseNo
            // 
            this.txtHouseNo.Enabled = false;
            this.txtHouseNo.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtHouseNo.Location = new System.Drawing.Point(116, 48);
            this.txtHouseNo.MaxLength = 8;
            this.txtHouseNo.Name = "txtHouseNo";
            this.txtHouseNo.ScrollBars = Wisej.Web.ScrollBars.Horizontal;
            this.txtHouseNo.Size = new System.Drawing.Size(67, 25);
            this.txtHouseNo.TabIndex = 3;
            this.txtHouseNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // txtMailStreet
            // 
            this.txtMailStreet.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMailStreet.Enabled = false;
            this.txtMailStreet.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMailStreet.Location = new System.Drawing.Point(250, 48);
            this.txtMailStreet.MaxLength = 25;
            this.txtMailStreet.Name = "txtMailStreet";
            this.txtMailStreet.Size = new System.Drawing.Size(161, 25);
            this.txtMailStreet.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromName("@control");
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 178);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(818, 35);
            this.flowLayoutPanel1.TabIndex = 68;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(740, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 25);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(658, 6);
            this.btnSave.MaximumSize = new System.Drawing.Size(0, 25);
            this.btnSave.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 25);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 178);
            this.panel1.TabIndex = 69;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(253, 250, 209);
            this.lblMessage.Dock = Wisej.Web.DockStyle.Top;
            this.lblMessage.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new Wisej.Web.Padding(5);
            this.lblMessage.Size = new System.Drawing.Size(818, 24);
            this.lblMessage.TabIndex = 68;
            this.lblMessage.Text = "Only fill this box if the Mailing Address is different from Client Address:";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DiffMailForm
            // 
            this.ClientSize = new System.Drawing.Size(818, 213);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiffMailForm";
            this.Text = "Mailing Address";
            this.Load += new System.EventHandler(this.DiffMailForm_Load);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblFirstReq;
        private Label lblDiffHouseNoReq;
        private Label lblDiffStateReq;
        private Label lblDiffCityReq;
        private Label lblLastReq;
        private Label lblDiffStreetReq;
        private TextBoxWithValidation txtLast;
        private TextBoxWithValidation txtFirst;
        private Label lblLast;
        private Label lblFirst;
        private Label lblPhone;
        private ComboBoxEx cmbMailCounty;
        private Label lblZipCode;
        private TextBoxWithValidation txtCityName;
        private Label lblCityName;
        private TextBoxWithValidation txtMailApartment;
        private Label lblApartment;
        private TextBoxWithValidation txtMailFloor;
        private TextBoxWithValidation txtMailSuffix;
        private Label lblFloor;
        private Label lblHouseNo;
        private Label lblState;
        private TextBoxWithValidation txtHouseNo;
        private TextBoxWithValidation txtMailState;
        private Label lblSuffix;
        private TextBoxWithValidation txtMailStreet;
        private Label lblStreet;
        private Label lblCounty;
        private Panel panel6;
        private TextBoxWithValidation txtZipPlus;
        private Button btnZipSearch;
        private TextBoxWithValidation txtZip;
        private Label lblPhoneReq;
        private Label lblSuffixReq;
        private Label lblFloorReq;
        private Label lblCountyReq;
        private Label lblZipCodeReq;
        private Label lblApartmentReq;
        private MaskedTextBox txtMailPhone;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnCancel;
        private Button btnSave;
        private Panel panel1;
        private Label lblMessage;
    }
}