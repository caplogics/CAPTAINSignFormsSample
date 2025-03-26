using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class VendorBrowser_From
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendorBrowser_From));
            this.panel1 = new Wisej.Web.Panel();
            this.btnEdit = new Wisej.Web.PictureBox();
            this.btnAdd = new Wisej.Web.PictureBox();
            this.btnSearch = new Wisej.Web.Button();
            this.cmbSource = new Wisej.Web.ComboBox();
            this.lblSource = new Wisej.Web.Label();
            this.txtName = new Wisej.Web.TextBox();
            this.lblSearch = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.label1 = new Wisej.Web.Label();
            this.label9 = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.panel4 = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.label4 = new Wisej.Web.Label();
            this.label8 = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.maskEIN = new Wisej.Web.MaskedTextBox();
            this.lbl1099 = new Wisej.Web.Label();
            this.Cmb1099 = new Wisej.Web.ComboBox();
            this.cmbEinSSN = new Wisej.Web.ComboBox();
            this.chkbW9 = new Wisej.Web.CheckBox();
            this.lblFirst = new Wisej.Web.Label();
            this.txtFirst = new Wisej.Web.TextBox();
            this.txtLast = new Wisej.Web.TextBox();
            this.lblLast = new Wisej.Web.Label();
            this.lblZip = new Wisej.Web.Label();
            this.lblContPhone = new Wisej.Web.Label();
            this.maskContPhone = new Wisej.Web.MaskedTextBox();
            this.lblContName = new Wisej.Web.Label();
            this.txtContName = new Wisej.Web.TextBox();
            this.lblCode = new Wisej.Web.Label();
            this.txtCode = new Wisej.Web.TextBox();
            this.txtVendName = new Wisej.Web.TextBox();
            this.lblStreet = new Wisej.Web.Label();
            this.txtStreet = new Wisej.Web.TextBox();
            this.txtAddr1 = new Wisej.Web.TextBox();
            this.lblAddress = new Wisej.Web.Label();
            this.txtAddr2 = new Wisej.Web.TextBox();
            this.txtCity = new Wisej.Web.TextBox();
            this.lblState = new Wisej.Web.Label();
            this.txtState = new Wisej.Web.TextBox();
            this.txtZipPlus = new Wisej.Web.TextBox();
            this.txtZip = new Wisej.Web.TextBox();
            this.btnZipSearch = new Wisej.Web.Button();
            this.lblFax = new Wisej.Web.Label();
            this.maskFax = new Wisej.Web.MaskedTextBox();
            this.maskPhone = new Wisej.Web.MaskedTextBox();
            this.lblphone = new Wisej.Web.Label();
            this.CmbVendorType = new Wisej.Web.ComboBox();
            this.lblFuelType = new Wisej.Web.Label();
            this.lblVendorType = new Wisej.Web.Label();
            this.txtFuelType = new Wisej.Web.TextBox();
            this.btnFuelTypes = new Wisej.Web.Button();
            this.label3 = new Wisej.Web.Label();
            this.ChkbActive = new Wisej.Web.CheckBox();
            this.lblName = new Wisej.Web.Label();
            this.lblCity = new Wisej.Web.Label();
            this.pnlselect = new Wisej.Web.Panel();
            this.lblTotNoRec = new Wisej.Web.Label();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnSelect = new Wisej.Web.Button();
            this.lblTotal = new Wisej.Web.Label();
            this.pnllistView_Vendor = new Wisej.Web.Panel();
            this.listView_Vendor = new Wisej.Web.ListView();
            this.Number = new Wisej.Web.ColumnHeader();
            this.Name = new Wisej.Web.ColumnHeader();
            this.Name2 = new Wisej.Web.ColumnHeader();
            this.NameAddress = new Wisej.Web.ColumnHeader();
            this.Active = new Wisej.Web.ColumnHeader();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlselect.SuspendLayout();
            this.pnllistView_Vendor.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.cmbSource);
            this.panel1.Controls.Add(this.lblSource);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.lblSearch);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(867, 64);
            this.panel1.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.btnEdit.ImageSource = "captain-edit";
            this.btnEdit.Location = new System.Drawing.Point(680, 35);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(16, 16);
            this.btnEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.StretchImage;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.btnAdd.ImageSource = "captain-add";
            this.btnAdd.Location = new System.Drawing.Point(662, 35);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(16, 16);
            this.btnAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.StretchImage;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(222, 36);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 21);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Se&arch";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbSource
            // 
            this.cmbSource.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(79, 34);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(131, 25);
            this.cmbSource.TabIndex = 5;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(6, 38);
            this.lblSource.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(43, 18);
            this.lblSource.TabIndex = 4;
            this.lblSource.Text = "Source";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(79, 5);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(621, 25);
            this.txtName.TabIndex = 3;
            this.txtName.Enter += new System.EventHandler(this.txtName_Enter);
            this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
            this.txtName.KeyDown += new Wisej.Web.KeyEventHandler(this.txtName_EnterKeyDown);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(6, 7);
            this.lblSearch.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(60, 18);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search By";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.pnlselect);
            this.panel2.Controls.Add(this.pnllistView_Vendor);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(867, 578);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.maskEIN);
            this.panel3.Controls.Add(this.lbl1099);
            this.panel3.Controls.Add(this.Cmb1099);
            this.panel3.Controls.Add(this.cmbEinSSN);
            this.panel3.Controls.Add(this.chkbW9);
            this.panel3.Controls.Add(this.lblFirst);
            this.panel3.Controls.Add(this.txtFirst);
            this.panel3.Controls.Add(this.txtLast);
            this.panel3.Controls.Add(this.lblLast);
            this.panel3.Controls.Add(this.lblZip);
            this.panel3.Controls.Add(this.lblContPhone);
            this.panel3.Controls.Add(this.maskContPhone);
            this.panel3.Controls.Add(this.lblContName);
            this.panel3.Controls.Add(this.txtContName);
            this.panel3.Controls.Add(this.lblCode);
            this.panel3.Controls.Add(this.txtCode);
            this.panel3.Controls.Add(this.txtVendName);
            this.panel3.Controls.Add(this.lblStreet);
            this.panel3.Controls.Add(this.txtStreet);
            this.panel3.Controls.Add(this.txtAddr1);
            this.panel3.Controls.Add(this.lblAddress);
            this.panel3.Controls.Add(this.txtAddr2);
            this.panel3.Controls.Add(this.txtCity);
            this.panel3.Controls.Add(this.lblState);
            this.panel3.Controls.Add(this.txtState);
            this.panel3.Controls.Add(this.txtZipPlus);
            this.panel3.Controls.Add(this.txtZip);
            this.panel3.Controls.Add(this.btnZipSearch);
            this.panel3.Controls.Add(this.lblFax);
            this.panel3.Controls.Add(this.maskFax);
            this.panel3.Controls.Add(this.maskPhone);
            this.panel3.Controls.Add(this.lblphone);
            this.panel3.Controls.Add(this.CmbVendorType);
            this.panel3.Controls.Add(this.lblFuelType);
            this.panel3.Controls.Add(this.lblVendorType);
            this.panel3.Controls.Add(this.txtFuelType);
            this.panel3.Controls.Add(this.btnFuelTypes);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.ChkbActive);
            this.panel3.Controls.Add(this.lblName);
            this.panel3.Controls.Add(this.lblCity);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Enabled = false;
            this.panel3.Location = new System.Drawing.Point(0, 215);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(867, 363);
            this.panel3.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(85, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 13;
            this.label1.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(81, 262);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(9, 14);
            this.label9.TabIndex = 13;
            this.label9.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(70, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(9, 14);
            this.label6.TabIndex = 13;
            this.label6.Text = "*";
            // 
            // panel4
            // 
            this.panel4.AppearanceKey = "panel-grdo";
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.spacer2);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 328);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel4.Size = new System.Drawing.Size(867, 35);
            this.panel4.TabIndex = 19;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(711, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(779, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(784, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Can&cel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(47, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 14);
            this.label4.TabIndex = 13;
            this.label4.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(29, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(9, 14);
            this.label8.TabIndex = 13;
            this.label8.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(315, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 14);
            this.label7.TabIndex = 13;
            this.label7.Text = "*";
            // 
            // maskEIN
            // 
            this.maskEIN.Enabled = false;
            this.maskEIN.Location = new System.Drawing.Point(619, 244);
            this.maskEIN.Mask = "000-00-0000";
            this.maskEIN.Name = "maskEIN";
            this.maskEIN.Size = new System.Drawing.Size(81, 25);
            this.maskEIN.TabIndex = 9;
            this.maskEIN.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lbl1099
            // 
            this.lbl1099.Location = new System.Drawing.Point(440, 248);
            this.lbl1099.Name = "lbl1099";
            this.lbl1099.Size = new System.Drawing.Size(51, 17);
            this.lbl1099.TabIndex = 1;
            this.lbl1099.Text = "1099 Cd";
            // 
            // Cmb1099
            // 
            this.Cmb1099.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb1099.Enabled = false;
            this.Cmb1099.FormattingEnabled = true;
            this.Cmb1099.Location = new System.Drawing.Point(495, 244);
            this.Cmb1099.Name = "Cmb1099";
            this.Cmb1099.Size = new System.Drawing.Size(59, 25);
            this.Cmb1099.TabIndex = 8;
            // 
            // cmbEinSSN
            // 
            this.cmbEinSSN.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbEinSSN.Enabled = false;
            this.cmbEinSSN.FormattingEnabled = true;
            this.cmbEinSSN.Location = new System.Drawing.Point(560, 244);
            this.cmbEinSSN.Name = "cmbEinSSN";
            this.cmbEinSSN.Size = new System.Drawing.Size(53, 25);
            this.cmbEinSSN.TabIndex = 8;
            // 
            // chkbW9
            // 
            this.chkbW9.Enabled = false;
            this.chkbW9.Location = new System.Drawing.Point(381, 32);
            this.chkbW9.Name = "chkbW9";
            this.chkbW9.Size = new System.Drawing.Size(54, 21);
            this.chkbW9.TabIndex = 2;
            this.chkbW9.Text = "W-9";
            // 
            // lblFirst
            // 
            this.lblFirst.AutoSize = true;
            this.lblFirst.Enabled = false;
            this.lblFirst.Location = new System.Drawing.Point(440, 35);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(28, 14);
            this.lblFirst.TabIndex = 0;
            this.lblFirst.Text = "First";
            this.lblFirst.Visible = false;
            // 
            // txtFirst
            // 
            this.txtFirst.Enabled = false;
            this.txtFirst.Location = new System.Drawing.Point(495, 33);
            this.txtFirst.MaxLength = 20;
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(205, 25);
            this.txtFirst.TabIndex = 3;
            this.txtFirst.Visible = false;
            // 
            // txtLast
            // 
            this.txtLast.Enabled = false;
            this.txtLast.Location = new System.Drawing.Point(495, 61);
            this.txtLast.MaxLength = 20;
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(205, 25);
            this.txtLast.TabIndex = 3;
            this.txtLast.Visible = false;
            // 
            // lblLast
            // 
            this.lblLast.AutoSize = true;
            this.lblLast.Enabled = false;
            this.lblLast.Location = new System.Drawing.Point(440, 63);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(27, 14);
            this.lblLast.TabIndex = 0;
            this.lblLast.Text = "Last";
            this.lblLast.Visible = false;
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(10, 181);
            this.lblZip.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(23, 18);
            this.lblZip.TabIndex = 0;
            this.lblZip.Text = "ZIP";
            // 
            // lblContPhone
            // 
            this.lblContPhone.AutoSize = true;
            this.lblContPhone.Location = new System.Drawing.Point(441, 296);
            this.lblContPhone.Name = "lblContPhone";
            this.lblContPhone.Size = new System.Drawing.Size(39, 14);
            this.lblContPhone.TabIndex = 0;
            this.lblContPhone.Text = "Phone";
            // 
            // maskContPhone
            // 
            this.maskContPhone.Location = new System.Drawing.Point(495, 291);
            this.maskContPhone.Mask = "999- 000-0000";
            this.maskContPhone.Name = "maskContPhone";
            this.maskContPhone.Size = new System.Drawing.Size(80, 25);
            this.maskContPhone.TabIndex = 2;
            this.maskContPhone.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblContName
            // 
            this.lblContName.Location = new System.Drawing.Point(10, 296);
            this.lblContName.Name = "lblContName";
            this.lblContName.Size = new System.Drawing.Size(87, 17);
            this.lblContName.TabIndex = 0;
            this.lblContName.Text = "Contact Name";
            // 
            // txtContName
            // 
            this.txtContName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtContName.Location = new System.Drawing.Point(111, 294);
            this.txtContName.MaxLength = 35;
            this.txtContName.Name = "txtContName";
            this.txtContName.Size = new System.Drawing.Size(248, 25);
            this.txtContName.TabIndex = 1;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(10, 9);
            this.lblCode.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(75, 18);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "Vendor Code";
            // 
            // txtCode
            // 
            this.txtCode.Enabled = false;
            this.txtCode.Location = new System.Drawing.Point(112, 6);
            this.txtCode.MaxLength = 10;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(78, 25);
            this.txtCode.TabIndex = 1;
            // 
            // txtVendName
            // 
            this.txtVendName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtVendName.Location = new System.Drawing.Point(112, 34);
            this.txtVendName.MaxLength = 35;
            this.txtVendName.Name = "txtVendName";
            this.txtVendName.Size = new System.Drawing.Size(249, 25);
            this.txtVendName.TabIndex = 4;
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Location = new System.Drawing.Point(10, 66);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(37, 14);
            this.lblStreet.TabIndex = 0;
            this.lblStreet.Text = "Street";
            // 
            // txtStreet
            // 
            this.txtStreet.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtStreet.Location = new System.Drawing.Point(112, 63);
            this.txtStreet.MaxLength = 35;
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(249, 25);
            this.txtStreet.TabIndex = 6;
            // 
            // txtAddr1
            // 
            this.txtAddr1.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtAddr1.Location = new System.Drawing.Point(112, 92);
            this.txtAddr1.MaxLength = 35;
            this.txtAddr1.Name = "txtAddr1";
            this.txtAddr1.Size = new System.Drawing.Size(249, 25);
            this.txtAddr1.TabIndex = 7;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(10, 95);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(96, 20);
            this.lblAddress.TabIndex = 0;
            this.lblAddress.Text = "Mailing Address";
            // 
            // txtAddr2
            // 
            this.txtAddr2.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtAddr2.Location = new System.Drawing.Point(112, 121);
            this.txtAddr2.MaxLength = 35;
            this.txtAddr2.Name = "txtAddr2";
            this.txtAddr2.Size = new System.Drawing.Size(249, 25);
            this.txtAddr2.TabIndex = 8;
            // 
            // txtCity
            // 
            this.txtCity.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCity.Location = new System.Drawing.Point(112, 150);
            this.txtCity.MaxLength = 20;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(125, 25);
            this.txtCity.TabIndex = 9;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(286, 153);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(33, 14);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "State";
            // 
            // txtState
            // 
            this.txtState.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtState.Location = new System.Drawing.Point(333, 150);
            this.txtState.MaxLength = 2;
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(28, 25);
            this.txtState.TabIndex = 10;
            // 
            // txtZipPlus
            // 
            this.txtZipPlus.Location = new System.Drawing.Point(156, 178);
            this.txtZipPlus.MaxLength = 4;
            this.txtZipPlus.Name = "txtZipPlus";
            this.txtZipPlus.Size = new System.Drawing.Size(33, 25);
            this.txtZipPlus.TabIndex = 12;
            // 
            // txtZip
            // 
            this.txtZip.Location = new System.Drawing.Point(112, 178);
            this.txtZip.MaxLength = 5;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(42, 25);
            this.txtZip.TabIndex = 11;
            // 
            // btnZipSearch
            // 
            this.btnZipSearch.Location = new System.Drawing.Point(194, 179);
            this.btnZipSearch.Name = "btnZipSearch";
            this.btnZipSearch.Size = new System.Drawing.Size(60, 25);
            this.btnZipSearch.TabIndex = 13;
            this.btnZipSearch.Text = "S&earch";
            this.btnZipSearch.Click += new System.EventHandler(this.btnZipSearch_Click);
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.Location = new System.Drawing.Point(251, 210);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(25, 14);
            this.lblFax.TabIndex = 0;
            this.lblFax.Text = "Fax";
            // 
            // maskFax
            // 
            this.maskFax.Location = new System.Drawing.Point(279, 207);
            this.maskFax.Mask = "999- 000-0000";
            this.maskFax.Name = "maskFax";
            this.maskFax.Size = new System.Drawing.Size(80, 25);
            this.maskFax.TabIndex = 15;
            this.maskFax.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // maskPhone
            // 
            this.maskPhone.Location = new System.Drawing.Point(112, 207);
            this.maskPhone.Mask = "999- 000-0000";
            this.maskPhone.Name = "maskPhone";
            this.maskPhone.Size = new System.Drawing.Size(80, 25);
            this.maskPhone.TabIndex = 14;
            this.maskPhone.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblphone
            // 
            this.lblphone.Location = new System.Drawing.Point(10, 211);
            this.lblphone.Name = "lblphone";
            this.lblphone.Size = new System.Drawing.Size(68, 23);
            this.lblphone.TabIndex = 0;
            this.lblphone.Text = "Telephone";
            // 
            // CmbVendorType
            // 
            this.CmbVendorType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbVendorType.FormattingEnabled = true;
            this.CmbVendorType.Location = new System.Drawing.Point(112, 236);
            this.CmbVendorType.Name = "CmbVendorType";
            this.CmbVendorType.Size = new System.Drawing.Size(117, 25);
            this.CmbVendorType.TabIndex = 16;
            // 
            // lblFuelType
            // 
            this.lblFuelType.Location = new System.Drawing.Point(10, 265);
            this.lblFuelType.Name = "lblFuelType";
            this.lblFuelType.Size = new System.Drawing.Size(75, 18);
            this.lblFuelType.TabIndex = 1;
            this.lblFuelType.Text = "Service Type";
            // 
            // lblVendorType
            // 
            this.lblVendorType.Location = new System.Drawing.Point(10, 238);
            this.lblVendorType.Name = "lblVendorType";
            this.lblVendorType.Size = new System.Drawing.Size(76, 17);
            this.lblVendorType.TabIndex = 1;
            this.lblVendorType.Text = "Vendor Type";
            // 
            // txtFuelType
            // 
            this.txtFuelType.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFuelType.Enabled = false;
            this.txtFuelType.Location = new System.Drawing.Point(112, 265);
            this.txtFuelType.MaxLength = 20;
            this.txtFuelType.Name = "txtFuelType";
            this.txtFuelType.Size = new System.Drawing.Size(125, 25);
            this.txtFuelType.TabIndex = 17;
            // 
            // btnFuelTypes
            // 
            this.btnFuelTypes.Location = new System.Drawing.Point(242, 265);
            this.btnFuelTypes.Name = "btnFuelTypes";
            this.btnFuelTypes.Size = new System.Drawing.Size(50, 25);
            this.btnFuelTypes.TabIndex = 18;
            this.btnFuelTypes.Text = "&View";
            this.btnFuelTypes.Click += new System.EventHandler(this.btnFuelTypes_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(91, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "*";
            // 
            // ChkbActive
            // 
            this.ChkbActive.CheckState = Wisej.Web.CheckState.Checked;
            this.ChkbActive.Location = new System.Drawing.Point(281, 8);
            this.ChkbActive.Name = "ChkbActive";
            this.ChkbActive.Size = new System.Drawing.Size(65, 21);
            this.ChkbActive.TabIndex = 2;
            this.ChkbActive.Text = "Active";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(10, 37);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(83, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Vendor Name";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(10, 153);
            this.lblCity.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(58, 18);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "City/Town";
            // 
            // pnlselect
            // 
            this.pnlselect.AppearanceKey = "panel-grdo";
            this.pnlselect.Controls.Add(this.lblTotNoRec);
            this.pnlselect.Controls.Add(this.spacer1);
            this.pnlselect.Controls.Add(this.btnSelect);
            this.pnlselect.Controls.Add(this.lblTotal);
            this.pnlselect.Dock = Wisej.Web.DockStyle.Top;
            this.pnlselect.Location = new System.Drawing.Point(0, 180);
            this.pnlselect.Name = "pnlselect";
            this.pnlselect.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlselect.Size = new System.Drawing.Size(867, 35);
            this.pnlselect.TabIndex = 2;
            // 
            // lblTotNoRec
            // 
            this.lblTotNoRec.AutoSize = true;
            this.lblTotNoRec.Dock = Wisej.Web.DockStyle.Left;
            this.lblTotNoRec.Location = new System.Drawing.Point(114, 5);
            this.lblTotNoRec.Name = "lblTotNoRec";
            this.lblTotNoRec.Size = new System.Drawing.Size(4, 25);
            this.lblTotNoRec.TabIndex = 2;
            this.lblTotNoRec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(94, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(20, 25);
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(780, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(72, 25);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "Se&lect";
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Dock = Wisej.Web.DockStyle.Left;
            this.lblTotal.Location = new System.Drawing.Point(15, 5);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(79, 25);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total Records";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTotal.Visible = false;
            // 
            // pnllistView_Vendor
            // 
            this.pnllistView_Vendor.Controls.Add(this.listView_Vendor);
            this.pnllistView_Vendor.Dock = Wisej.Web.DockStyle.Top;
            this.pnllistView_Vendor.Location = new System.Drawing.Point(0, 0);
            this.pnllistView_Vendor.Name = "pnllistView_Vendor";
            this.pnllistView_Vendor.Size = new System.Drawing.Size(867, 180);
            this.pnllistView_Vendor.TabIndex = 2;
            // 
            // listView_Vendor
            // 
            this.listView_Vendor.Columns.AddRange(new Wisej.Web.ColumnHeader[] {
            this.Number,
            this.Name,
            this.Name2,
            this.NameAddress,
            this.Active});
            this.listView_Vendor.Dock = Wisej.Web.DockStyle.Fill;
            this.listView_Vendor.LabelWrap = true;
            this.listView_Vendor.Location = new System.Drawing.Point(0, 0);
            this.listView_Vendor.Name = "listView_Vendor";
            this.listView_Vendor.Selectable = true;
            this.listView_Vendor.Size = new System.Drawing.Size(867, 180);
            this.listView_Vendor.TabIndex = 0;
            this.listView_Vendor.View = Wisej.Web.View.Details;
            this.listView_Vendor.SelectedIndexChanged += new System.EventHandler(this.listView_Vendor_SelectedIndexChanged);
            // 
            // Number
            // 
            this.Number.Name = "Number";
            this.Number.Text = "Number";
            this.Number.Width = 85;
            // 
            // Name
            // 
            this.Name.Name = "Name";
            this.Name.Text = "Name";
            this.Name.Width = 170;
            // 
            // Name2
            // 
            this.Name2.Name = "Name2";
            this.Name2.Text = "Contact Name";
            this.Name2.Width = 240;
            // 
            // NameAddress
            // 
            this.NameAddress.Name = "NameAddress";
            this.NameAddress.Text = "Address";
            this.NameAddress.Width = 350;
            // 
            // Active
            // 
            this.Active.Name = "Active";
            this.Active.Width = 20;
            // 
            // VendorBrowser_From
            // 
            this.ClientSize = new System.Drawing.Size(867, 642);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.Name = "VendorBrowser_From";
            this.Text = "Test";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.pnlselect.ResumeLayout(false);
            this.pnlselect.PerformLayout();
            this.pnllistView_Vendor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label lblSearch;
        private TextBox txtName;
        private ComboBox cmbSource;
        private Label lblSource;
        private Button btnSearch;
        private Panel panel2;
        private ListView listView_Vendor;
        private ColumnHeader Number;
        private ColumnHeader NameAddress;
        private ColumnHeader Name;
        private Button btnSelect;
        private Label lblTotNoRec;
        private Label lblTotal;
        private ColumnHeader Active;
        private ColumnHeader Name2;
        private PictureBox btnEdit;
        private PictureBox btnAdd;
        private Panel panel3;
        private Label lblCode;
        private TextBox txtCode;
        private TextBox txtVendName;
        private Label lblName;
        private Label lblStreet;
        private TextBox txtStreet;
        private TextBox txtAddr1;
        private Label lblAddress;
        private TextBox txtAddr2;
        private TextBox txtCity;
        private Label lblCity;
        private Label lblState;
        private TextBox txtState;
        private TextBox txtZipPlus;
        private TextBox txtZip;
        private Button btnZipSearch;
        private Label lblFax;
        private MaskedTextBox maskFax;
        private MaskedTextBox maskPhone;
        private Label lblphone;
        private ComboBox CmbVendorType;
        private Label lblFuelType;
        private Label lblVendorType;
        private TextBox txtFuelType;
        private Button btnFuelTypes;
        private Label label3;
        private Label label4;
        private Label label6;
        private Label label7;
        private Label label9;
        private CheckBox ChkbActive;
        private Label label1;
        private Label lblContName;
        private TextBox txtContName;
        private Label lblContPhone;
        private MaskedTextBox maskContPhone;
        private Label lblZip;
        private Label label8;
        private Button btnSave;
        private Button btnCancel;
        private CheckBox chkbW9;
        private Label lblFirst;
        private TextBox txtFirst;
        private TextBox txtLast;
        private Label lblLast;
        private Label lbl1099;
        private ComboBox Cmb1099;
        private ComboBox cmbEinSSN;
        private MaskedTextBox maskEIN;
        private Panel panel4;
        private Spacer spacer2;
        private Panel pnlselect;
        private Spacer spacer1;
        private Panel pnllistView_Vendor;
    }
}