using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class AddUserForm
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

        #region Visual WebGui Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddUserForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlUser = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.tabUser = new Wisej.Web.TabControl();
            this.tabPageIntake = new Wisej.Web.TabPage();
            this.tabPageServiceHierarchy = new Wisej.Web.TabPage();
            this.tabPageService = new Wisej.Web.TabPage();
            this.tabPageScreen = new Wisej.Web.TabPage();
            this.tabPageReport = new Wisej.Web.TabPage();
            this.tabPageAddlPrivileges = new Wisej.Web.TabPage();
            this.panel3 = new Wisej.Web.Panel();
            this.gvwComponents = new Wisej.Web.DataGridView();
            this.picEdit = new Wisej.Web.PictureBox();
            this.pnlSecurity = new Wisej.Web.Panel();
            this.dtpDob = new Wisej.Web.DateTimePicker();
            this.maskPhone = new Wisej.Web.MaskedTextBox();
            this.lblMobile = new Wisej.Web.Label();
            this.label12 = new Wisej.Web.Label();
            this.chkSearchPIP = new Wisej.Web.CheckBox();
            this.label6 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.clstImageTypes = new Wisej.Web.CheckedListBox();
            this.chkbActive = new Wisej.Web.CheckBox();
            this.label8 = new Wisej.Web.Label();
            this.lblEmail = new Wisej.Web.Label();
            this.txtEmail = new Wisej.Web.TextBox();
            this.label7 = new Wisej.Web.Label();
            this.btnResetPass = new Wisej.Web.Button();
            this.PbHierarchies = new Wisej.Web.PictureBox();
            this.txtDefaultHierachy = new Wisej.Web.TextBox();
            this.lblDefaultHie = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.label14 = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.btnCopy = new Wisej.Web.Button();
            this.cbTemplateUser = new Wisej.Web.CheckBox();
            this.cmbSecurity = new Wisej.Web.ComboBox();
            this.cbForcePassword = new Wisej.Web.CheckBox();
            this.cbAccessAll = new Wisej.Web.CheckBox();
            this.label9 = new Wisej.Web.Label();
            this.cmbSite = new Wisej.Web.ComboBox();
            this.cmbEMS = new Wisej.Web.ComboBox();
            this.lblSecurity = new Wisej.Web.Label();
            this.cmbStaff = new Wisej.Web.ComboBox();
            this.lblStaff = new Wisej.Web.Label();
            this.txtCaseWorker = new Wisej.Web.TextBox();
            this.lblCaseWorker = new Wisej.Web.Label();
            this.txtLastName = new Wisej.Web.TextBox();
            this.lbllname = new Wisej.Web.Label();
            this.txtFirstName = new Wisej.Web.TextBox();
            this.lblfname = new Wisej.Web.Label();
            this.txtUserID = new Wisej.Web.TextBox();
            this.lblUserID = new Wisej.Web.Label();
            this.helpUser = new Wisej.Web.HelpTip(this.components);
            this.cellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDescription = new Wisej.Web.DataGridViewTextBoxColumn();
            this.listBoxWithDropDownControl1 = new Captain.Common.Views.UserControls.ListBoxWithDropDownControl();
            this.cmbImageTypes = new Captain.Common.Views.UserControls.ListBoxWithDropDownControl();
            this.pnlUser.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabUser.SuspendLayout();
            this.tabPageAddlPrivileges.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwComponents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).BeginInit();
            this.pnlSecurity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbHierarchies)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUser
            // 
            this.pnlUser.Controls.Add(this.panel1);
            this.pnlUser.Controls.Add(this.tabUser);
            this.pnlUser.Controls.Add(this.pnlSecurity);
            this.pnlUser.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlUser.Location = new System.Drawing.Point(0, 0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(893, 525);
            this.pnlUser.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 490);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(0, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(893, 35);
            this.panel1.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(725, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 21;
            this.btnOk.Text = "&Save";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(800, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(803, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // tabUser
            // 
            this.tabUser.Controls.Add(this.tabPageIntake);
            this.tabUser.Controls.Add(this.tabPageServiceHierarchy);
            this.tabUser.Controls.Add(this.tabPageService);
            this.tabUser.Controls.Add(this.tabPageScreen);
            this.tabUser.Controls.Add(this.tabPageReport);
            this.tabUser.Controls.Add(this.tabPageAddlPrivileges);
            this.tabUser.Dock = Wisej.Web.DockStyle.Top;
            this.tabUser.Location = new System.Drawing.Point(0, 283);
            this.tabUser.Name = "tabUser";
            this.tabUser.PageInsets = new Wisej.Web.Padding(0, 27, 0, 0);
            this.tabUser.SelectedIndex = 0;
            this.tabUser.Size = new System.Drawing.Size(893, 207);
            this.tabUser.TabIndex = 20;
            this.tabUser.Selecting += new Wisej.Web.TabControlCancelEventHandler(this.OnTabControlSelectedIndexChanging);
            this.tabUser.SelectedIndexChanged += new System.EventHandler(this.OnTabControlSelectedIndexChanged);
            // 
            // tabPageIntake
            // 
            this.tabPageIntake.Location = new System.Drawing.Point(0, 27);
            this.tabPageIntake.Name = "tabPageIntake";
            this.tabPageIntake.Size = new System.Drawing.Size(893, 180);
            this.tabPageIntake.Text = "Intake Hierarchies";
            // 
            // tabPageServiceHierarchy
            // 
            this.tabPageServiceHierarchy.Location = new System.Drawing.Point(0, 27);
            this.tabPageServiceHierarchy.Name = "tabPageServiceHierarchy";
            this.tabPageServiceHierarchy.Size = new System.Drawing.Size(893, 180);
            this.tabPageServiceHierarchy.Text = "Service Plan Hierarchies";
            // 
            // tabPageService
            // 
            this.tabPageService.Location = new System.Drawing.Point(0, 27);
            this.tabPageService.Name = "tabPageService";
            this.tabPageService.Size = new System.Drawing.Size(893, 180);
            this.tabPageService.Text = "Client Inquiry";
            // 
            // tabPageScreen
            // 
            this.tabPageScreen.Location = new System.Drawing.Point(0, 27);
            this.tabPageScreen.Name = "tabPageScreen";
            this.tabPageScreen.Size = new System.Drawing.Size(893, 180);
            this.tabPageScreen.Text = "Screen Privileges";
            // 
            // tabPageReport
            // 
            this.tabPageReport.Location = new System.Drawing.Point(0, 27);
            this.tabPageReport.Name = "tabPageReport";
            this.tabPageReport.Size = new System.Drawing.Size(893, 180);
            this.tabPageReport.Text = "Report Privileges";
            // 
            // tabPageAddlPrivileges
            // 
            this.tabPageAddlPrivileges.Controls.Add(this.panel3);
            this.tabPageAddlPrivileges.Location = new System.Drawing.Point(0, 27);
            this.tabPageAddlPrivileges.Name = "tabPageAddlPrivileges";
            this.tabPageAddlPrivileges.Size = new System.Drawing.Size(893, 180);
            this.tabPageAddlPrivileges.Tag = "AdditionalPrivileges";
            this.tabPageAddlPrivileges.Text = "Additional Privileges";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwComponents);
            this.panel3.Controls.Add(this.picEdit);
            this.panel3.Location = new System.Drawing.Point(1, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(693, 177);
            this.panel3.TabIndex = 0;
            this.panel3.Click += new System.EventHandler(this.panel3_Click);
            // 
            // gvwComponents
            // 
            this.gvwComponents.AllowUserToResizeColumns = false;
            this.gvwComponents.AllowUserToResizeRows = false;
            this.gvwComponents.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwComponents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwComponents.ColumnHeadersHeight = 25;
            this.gvwComponents.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwComponents.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.cellCode,
            this.cellDescription});
            this.gvwComponents.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwComponents.Location = new System.Drawing.Point(5, 5);
            this.gvwComponents.MultiSelect = false;
            this.gvwComponents.Name = "gvwComponents";
            this.gvwComponents.ReadOnly = true;
            this.gvwComponents.RowHeadersWidth = 15;
            this.gvwComponents.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwComponents.Size = new System.Drawing.Size(413, 161);
            this.gvwComponents.TabIndex = 0;
            // 
            // picEdit
            // 
            this.picEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.picEdit.ImageSource = "captain-edit";
            this.picEdit.Location = new System.Drawing.Point(424, 8);
            this.picEdit.Name = "picEdit";
            this.picEdit.Size = new System.Drawing.Size(20, 20);
            this.picEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picEdit.Click += new System.EventHandler(this.picEdit_Click);
            // 
            // pnlSecurity
            // 
            this.pnlSecurity.Controls.Add(this.dtpDob);
            this.pnlSecurity.Controls.Add(this.maskPhone);
            this.pnlSecurity.Controls.Add(this.lblMobile);
            this.pnlSecurity.Controls.Add(this.label12);
            this.pnlSecurity.Controls.Add(this.chkSearchPIP);
            this.pnlSecurity.Controls.Add(this.label6);
            this.pnlSecurity.Controls.Add(this.label1);
            this.pnlSecurity.Controls.Add(this.clstImageTypes);
            this.pnlSecurity.Controls.Add(this.chkbActive);
            this.pnlSecurity.Controls.Add(this.label8);
            this.pnlSecurity.Controls.Add(this.lblEmail);
            this.pnlSecurity.Controls.Add(this.txtEmail);
            this.pnlSecurity.Controls.Add(this.label7);
            this.pnlSecurity.Controls.Add(this.btnResetPass);
            this.pnlSecurity.Controls.Add(this.PbHierarchies);
            this.pnlSecurity.Controls.Add(this.txtDefaultHierachy);
            this.pnlSecurity.Controls.Add(this.lblDefaultHie);
            this.pnlSecurity.Controls.Add(this.label2);
            this.pnlSecurity.Controls.Add(this.label3);
            this.pnlSecurity.Controls.Add(this.label4);
            this.pnlSecurity.Controls.Add(this.label14);
            this.pnlSecurity.Controls.Add(this.label5);
            this.pnlSecurity.Controls.Add(this.btnCopy);
            this.pnlSecurity.Controls.Add(this.cbTemplateUser);
            this.pnlSecurity.Controls.Add(this.cmbSecurity);
            this.pnlSecurity.Controls.Add(this.cbForcePassword);
            this.pnlSecurity.Controls.Add(this.cbAccessAll);
            this.pnlSecurity.Controls.Add(this.label9);
            this.pnlSecurity.Controls.Add(this.cmbSite);
            this.pnlSecurity.Controls.Add(this.cmbEMS);
            this.pnlSecurity.Controls.Add(this.lblSecurity);
            this.pnlSecurity.Controls.Add(this.cmbStaff);
            this.pnlSecurity.Controls.Add(this.lblStaff);
            this.pnlSecurity.Controls.Add(this.txtCaseWorker);
            this.pnlSecurity.Controls.Add(this.lblCaseWorker);
            this.pnlSecurity.Controls.Add(this.txtLastName);
            this.pnlSecurity.Controls.Add(this.lbllname);
            this.pnlSecurity.Controls.Add(this.txtFirstName);
            this.pnlSecurity.Controls.Add(this.lblfname);
            this.pnlSecurity.Controls.Add(this.txtUserID);
            this.pnlSecurity.Controls.Add(this.lblUserID);
            this.pnlSecurity.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSecurity.Location = new System.Drawing.Point(0, 0);
            this.pnlSecurity.Name = "pnlSecurity";
            this.pnlSecurity.Size = new System.Drawing.Size(893, 283);
            this.pnlSecurity.TabIndex = 1;
            this.pnlSecurity.PanelCollapsed += new System.EventHandler(this.pnlSecurity_PanelCollapsed);
            // 
            // dtpDob
            // 
            this.dtpDob.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left)));
            this.dtpDob.AutoSize = false;
            this.dtpDob.Checked = false;
            this.dtpDob.CustomFormat = "MM/dd/yyyy";
            this.dtpDob.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpDob.Location = new System.Drawing.Point(300, 98);
            this.dtpDob.Name = "dtpDob";
            this.dtpDob.ShowCheckBox = true;
            this.dtpDob.ShowToolTips = false;
            this.dtpDob.Size = new System.Drawing.Size(116, 25);
            this.dtpDob.TabIndex = 6;
            // 
            // maskPhone
            // 
            this.maskPhone.Location = new System.Drawing.Point(124, 129);
            this.maskPhone.Mask = "999 000-0000";
            this.maskPhone.Name = "maskPhone";
            this.maskPhone.Size = new System.Drawing.Size(87, 25);
            this.maskPhone.TabIndex = 7;
            this.maskPhone.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblMobile
            // 
            this.lblMobile.Location = new System.Drawing.Point(15, 132);
            this.lblMobile.MaximumSize = new System.Drawing.Size(0, 17);
            this.lblMobile.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblMobile.Name = "lblMobile";
            this.lblMobile.Size = new System.Drawing.Size(103, 18);
            this.lblMobile.TabIndex = 36;
            this.lblMobile.Text = "Mobile Number";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(210, 103);
            this.label12.MinimumSize = new System.Drawing.Size(0, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(84, 18);
            this.label12.TabIndex = 33;
            this.label12.Text = "Date of Birth";
            // 
            // chkSearchPIP
            // 
            this.chkSearchPIP.Location = new System.Drawing.Point(572, 119);
            this.chkSearchPIP.Name = "chkSearchPIP";
            this.chkSearchPIP.Size = new System.Drawing.Size(164, 21);
            this.chkSearchPIP.TabIndex = 18;
            this.chkSearchPIP.Text = "Show Search PIP Button";
            this.chkSearchPIP.Visible = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(480, 163);
            this.label6.MaximumSize = new System.Drawing.Size(0, 17);
            this.label6.MinimumSize = new System.Drawing.Size(0, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "Image Types";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(62, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 32;
            this.label1.Text = "*";
            // 
            // clstImageTypes
            // 
            this.clstImageTypes.Location = new System.Drawing.Point(572, 151);
            this.clstImageTypes.Name = "clstImageTypes";
            this.clstImageTypes.Size = new System.Drawing.Size(279, 127);
            this.clstImageTypes.TabIndex = 21;
            // 
            // chkbActive
            // 
            this.chkbActive.Appearance = Wisej.Web.Appearance.Switch;
            this.chkbActive.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbActive.Location = new System.Drawing.Point(374, 9);
            this.chkbActive.MinimumSize = new System.Drawing.Size(0, 20);
            this.chkbActive.Name = "chkbActive";
            this.chkbActive.Size = new System.Drawing.Size(75, 21);
            this.chkbActive.TabIndex = 2;
            this.chkbActive.Text = "Active";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 223);
            this.label8.MaximumSize = new System.Drawing.Size(0, 20);
            this.label8.MinimumSize = new System.Drawing.Size(0, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "EMS/CM Level";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(15, 163);
            this.lblEmail.MaximumSize = new System.Drawing.Size(0, 17);
            this.lblEmail.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(48, 18);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Email Id";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(124, 160);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(292, 25);
            this.txtEmail.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(115, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 14);
            this.label7.TabIndex = 27;
            this.label7.Text = "*";
            // 
            // btnResetPass
            // 
            this.btnResetPass.Location = new System.Drawing.Point(749, 93);
            this.btnResetPass.Name = "btnResetPass";
            this.btnResetPass.Size = new System.Drawing.Size(102, 24);
            this.btnResetPass.TabIndex = 19;
            this.btnResetPass.Text = "&Reset Password";
            this.btnResetPass.Visible = false;
            this.btnResetPass.Click += new System.EventHandler(this.btnResetPass_Click);
            // 
            // PbHierarchies
            // 
            this.PbHierarchies.Cursor = Wisej.Web.Cursors.Hand;
            this.PbHierarchies.ImageSource = "captain-filter?color=#6a6a6a";
            this.PbHierarchies.Location = new System.Drawing.Point(212, 254);
            this.PbHierarchies.Name = "PbHierarchies";
            this.PbHierarchies.Size = new System.Drawing.Size(20, 20);
            this.PbHierarchies.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.PbHierarchies.Tag = "12";
            this.PbHierarchies.Click += new System.EventHandler(this.PbHierarchies_Click);
            // 
            // txtDefaultHierachy
            // 
            this.txtDefaultHierachy.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left)));
            this.txtDefaultHierachy.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtDefaultHierachy.Location = new System.Drawing.Point(124, 251);
            this.txtDefaultHierachy.MaxLength = 20;
            this.txtDefaultHierachy.Name = "txtDefaultHierachy";
            this.txtDefaultHierachy.ReadOnly = true;
            this.txtDefaultHierachy.Size = new System.Drawing.Size(85, 25);
            this.txtDefaultHierachy.TabIndex = 11;
            // 
            // lblDefaultHie
            // 
            this.lblDefaultHie.Location = new System.Drawing.Point(15, 255);
            this.lblDefaultHie.MaximumSize = new System.Drawing.Size(0, 20);
            this.lblDefaultHie.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblDefaultHie.Name = "lblDefaultHie";
            this.lblDefaultHie.Size = new System.Drawing.Size(132, 18);
            this.lblDefaultHie.TabIndex = 20;
            this.lblDefaultHie.Text = "Default Hierarchy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(58, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 14);
            this.label2.TabIndex = 27;
            this.label2.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(77, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 14);
            this.label3.TabIndex = 27;
            this.label3.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(76, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 14);
            this.label4.TabIndex = 27;
            this.label4.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(89, 97);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(9, 14);
            this.label14.TabIndex = 27;
            this.label14.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(64, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(9, 14);
            this.label5.TabIndex = 27;
            this.label5.Text = "*";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(749, 120);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(102, 24);
            this.btnCopy.TabIndex = 20;
            this.btnCopy.Text = "Co&py From";
            this.btnCopy.Click += new System.EventHandler(this.OnCopyFromButtonClick);
            // 
            // cbTemplateUser
            // 
            this.cbTemplateUser.Location = new System.Drawing.Point(692, 68);
            this.cbTemplateUser.Name = "cbTemplateUser";
            this.cbTemplateUser.Size = new System.Drawing.Size(111, 21);
            this.cbTemplateUser.TabIndex = 16;
            this.cbTemplateUser.Text = "Template User";
            // 
            // cmbSecurity
            // 
            this.cmbSecurity.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSecurity.FormattingEnabled = true;
            this.cmbSecurity.Location = new System.Drawing.Point(124, 190);
            this.cmbSecurity.MinimumSize = new System.Drawing.Size(0, 25);
            this.cmbSecurity.Name = "cmbSecurity";
            this.cmbSecurity.Size = new System.Drawing.Size(292, 25);
            this.cmbSecurity.TabIndex = 9;
            this.cmbSecurity.SelectedIndexChanged += new System.EventHandler(this.cmbSecurity_SelectedIndexChanged);
            // 
            // cbForcePassword
            // 
            this.cbForcePassword.Location = new System.Drawing.Point(572, 93);
            this.cbForcePassword.Name = "cbForcePassword";
            this.cbForcePassword.Size = new System.Drawing.Size(164, 21);
            this.cbForcePassword.TabIndex = 17;
            this.cbForcePassword.Text = "Force Password Change";
            // 
            // cbAccessAll
            // 
            this.cbAccessAll.Location = new System.Drawing.Point(572, 67);
            this.cbAccessAll.Name = "cbAccessAll";
            this.cbAccessAll.Size = new System.Drawing.Size(88, 21);
            this.cbAccessAll.TabIndex = 15;
            this.cbAccessAll.Text = "Access All";
            // 
            // label9
            // 
            this.label9.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(480, 42);
            this.label9.MaximumSize = new System.Drawing.Size(0, 20);
            this.label9.MinimumSize = new System.Drawing.Size(0, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 18);
            this.label9.TabIndex = 17;
            this.label9.Text = "Site";
            // 
            // cmbSite
            // 
            this.cmbSite.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSite.FormattingEnabled = true;
            this.cmbSite.Location = new System.Drawing.Point(572, 38);
            this.cmbSite.MinimumSize = new System.Drawing.Size(0, 25);
            this.cmbSite.Name = "cmbSite";
            this.cmbSite.Size = new System.Drawing.Size(279, 25);
            this.cmbSite.TabIndex = 14;
            this.cmbSite.SelectedIndexChanged += new System.EventHandler(this.cmbSite_SelectedIndexChanged);
            // 
            // cmbEMS
            // 
            this.cmbEMS.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbEMS.FormattingEnabled = true;
            this.cmbEMS.Location = new System.Drawing.Point(124, 220);
            this.cmbEMS.MinimumSize = new System.Drawing.Size(0, 25);
            this.cmbEMS.Name = "cmbEMS";
            this.cmbEMS.Size = new System.Drawing.Size(292, 25);
            this.cmbEMS.TabIndex = 10;
            // 
            // lblSecurity
            // 
            this.lblSecurity.AutoSize = true;
            this.lblSecurity.Location = new System.Drawing.Point(15, 193);
            this.lblSecurity.MaximumSize = new System.Drawing.Size(0, 20);
            this.lblSecurity.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSecurity.Name = "lblSecurity";
            this.lblSecurity.Size = new System.Drawing.Size(48, 18);
            this.lblSecurity.TabIndex = 12;
            this.lblSecurity.Text = "Security";
            // 
            // cmbStaff
            // 
            this.cmbStaff.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbStaff.FormattingEnabled = true;
            this.cmbStaff.Location = new System.Drawing.Point(572, 7);
            this.cmbStaff.MinimumSize = new System.Drawing.Size(0, 25);
            this.cmbStaff.Name = "cmbStaff";
            this.cmbStaff.Size = new System.Drawing.Size(279, 25);
            this.cmbStaff.TabIndex = 13;
            // 
            // lblStaff
            // 
            this.lblStaff.Location = new System.Drawing.Point(480, 9);
            this.lblStaff.MaximumSize = new System.Drawing.Size(0, 20);
            this.lblStaff.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblStaff.Name = "lblStaff";
            this.lblStaff.Size = new System.Drawing.Size(97, 18);
            this.lblStaff.TabIndex = 8;
            this.lblStaff.Text = "Staff Member";
            // 
            // txtCaseWorker
            // 
            this.txtCaseWorker.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCaseWorker.Location = new System.Drawing.Point(124, 98);
            this.txtCaseWorker.MaxLength = 4;
            this.txtCaseWorker.Name = "txtCaseWorker";
            this.txtCaseWorker.Size = new System.Drawing.Size(55, 25);
            this.txtCaseWorker.TabIndex = 5;
            this.txtCaseWorker.KeyPress += new Wisej.Web.KeyPressEventHandler(this.txtCaseWorker_KeyPress);
            // 
            // lblCaseWorker
            // 
            this.lblCaseWorker.AutoSize = true;
            this.lblCaseWorker.Location = new System.Drawing.Point(15, 101);
            this.lblCaseWorker.MaximumSize = new System.Drawing.Size(0, 17);
            this.lblCaseWorker.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblCaseWorker.Name = "lblCaseWorker";
            this.lblCaseWorker.Size = new System.Drawing.Size(74, 18);
            this.lblCaseWorker.TabIndex = 6;
            this.lblCaseWorker.Text = "Case Worker";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(124, 68);
            this.txtLastName.MaxLength = 20;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(214, 25);
            this.txtLastName.TabIndex = 4;
            // 
            // lbllname
            // 
            this.lbllname.AutoSize = true;
            this.lbllname.Location = new System.Drawing.Point(15, 71);
            this.lbllname.MaximumSize = new System.Drawing.Size(0, 17);
            this.lbllname.MinimumSize = new System.Drawing.Size(0, 18);
            this.lbllname.Name = "lbllname";
            this.lbllname.Size = new System.Drawing.Size(62, 18);
            this.lbllname.TabIndex = 4;
            this.lbllname.Text = "Last Name";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(124, 38);
            this.txtFirstName.MaxLength = 20;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(214, 25);
            this.txtFirstName.TabIndex = 3;
            // 
            // lblfname
            // 
            this.lblfname.AutoSize = true;
            this.lblfname.Location = new System.Drawing.Point(15, 41);
            this.lblfname.MaximumSize = new System.Drawing.Size(0, 17);
            this.lblfname.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblfname.Name = "lblfname";
            this.lblfname.Size = new System.Drawing.Size(63, 18);
            this.lblfname.TabIndex = 2;
            this.lblfname.Text = "First Name";
            this.lblfname.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtUserID.Location = new System.Drawing.Point(124, 8);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(214, 25);
            this.txtUserID.TabIndex = 1;
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(15, 11);
            this.lblUserID.MaximumSize = new System.Drawing.Size(0, 17);
            this.lblUserID.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(45, 18);
            this.lblUserID.TabIndex = 0;
            this.lblUserID.Text = "User ID";
            this.lblUserID.Click += new System.EventHandler(this.lblUserID_Click);
            // 
            // cellCode
            // 
            this.cellCode.HeaderText = "Code";
            this.cellCode.Name = "cellCode";
            this.cellCode.ReadOnly = true;
            // 
            // cellDescription
            // 
            this.cellDescription.HeaderText = "Description";
            this.cellDescription.Name = "cellDescription";
            this.cellDescription.ReadOnly = true;
            this.cellDescription.Width = 300;
            // 
            // listBoxWithDropDownControl1
            // 
            this.listBoxWithDropDownControl1.AutoValidate = Wisej.Web.AutoValidate.EnablePreventFocusChange;
            this.listBoxWithDropDownControl1.Location = new System.Drawing.Point(271, 106);
            this.listBoxWithDropDownControl1.Name = "listBoxWithDropDownControl1";
            this.listBoxWithDropDownControl1.Size = new System.Drawing.Size(55, 83);
            this.listBoxWithDropDownControl1.TabIndex = 23;
            // 
            // cmbImageTypes
            // 
            this.cmbImageTypes.AutoValidate = Wisej.Web.AutoValidate.EnablePreventFocusChange;
            this.cmbImageTypes.Location = new System.Drawing.Point(103, 112);
            this.cmbImageTypes.Name = "cmbImageTypes";
            this.cmbImageTypes.Size = new System.Drawing.Size(223, 83);
            this.cmbImageTypes.TabIndex = 23;
            // 
            // AddUserForm
            // 
            this.ClientSize = new System.Drawing.Size(893, 525);
            this.Controls.Add(this.pnlUser);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddUserForm";
            this.Text = "AddUserForm";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "TL_HELP";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.AddUserForm_Load);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.AddUserForm_ToolClick);
            this.pnlUser.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            this.tabPageAddlPrivileges.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwComponents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).EndInit();
            this.pnlSecurity.ResumeLayout(false);
            this.pnlSecurity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbHierarchies)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlUser;
        private Panel pnlSecurity;
        private ComboBox cmbSecurity;
        private CheckBox cbForcePassword;
        private CheckBox cbAccessAll;
        private Label label9;
        private ComboBox cmbSite;
        private ComboBox cmbEMS;
        private Label label8;
        private Label lblSecurity;
        private Label label6;
        private ComboBox cmbStaff;
        private Label lblStaff;
        private TextBox txtCaseWorker;
        private Label lblCaseWorker;
        private TextBox txtLastName;
        private Label lbllname;
        private TextBox txtFirstName;
        private Label lblfname;
        private TextBox txtUserID;
        private Label lblUserID;
        private TabControl tabUser;
        private TabPage tabPageIntake;
        private TabPage tabPageService;
        private TabPage tabPageScreen;
        private TabPage tabPageReport;
        private TabPage tabPageAddlPrivileges;
        private CustomQuestionsControl gridControl1;
        private CustomQuestionsControl gridControl2;
        private Panel panel3;
        private Button btnOk;
        private Button btnCancel;
        private ListBoxWithDropDownControl cmbImageTypes;
        private HelpTip helpUser;
        private CheckedListBox clstImageTypes;
        private ListBoxWithDropDownControl listBoxWithDropDownControl1;
        private CheckBox cbTemplateUser;
        private Button btnCopy;
        private Label label5;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label14;
        private TextBox txtDefaultHierachy;
        private Label lblDefaultHie;
        private Button btnResetPass;
        private PictureBox PbHierarchies;
        private Label label7;
        private DataGridView gvwComponents;
        private DataGridViewTextBoxColumn cellCode;
        private DataGridViewTextBoxColumn cellDescription;
        private PictureBox picEdit;
        private Label lblEmail;
        private TextBox txtEmail;
        private CheckBox chkSearchPIP;
        private TabPage tabPageServiceHierarchy;
        private Panel panel1;
        private Label label1;
        private CheckBox chkbActive;
        private Label label12;
        private Spacer spacer1;
        private Label lblMobile;
        private MaskedTextBox maskPhone;
        private DateTimePicker dtpDob;
    }
}