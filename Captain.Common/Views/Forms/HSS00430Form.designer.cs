using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00430Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00430Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.PbEdit = new Wisej.Web.PictureBox();
            this.PbDelete = new Wisej.Web.PictureBox();
            this.PbAdd = new Wisej.Web.PictureBox();
            this.lblHeader = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlAlertCode = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.txtMRelation = new Wisej.Web.TextBox();
            this.lblMRelation = new Wisej.Web.Label();
            this.lblFRelation = new Wisej.Web.Label();
            this.txtFRelation = new Wisej.Web.TextBox();
            this.mskFPhone = new Wisej.Web.MaskedTextBox();
            this.lblFPhone = new Wisej.Web.Label();
            this.lblFCell = new Wisej.Web.Label();
            this.mskFCell = new Wisej.Web.MaskedTextBox();
            this.mskMCell = new Wisej.Web.MaskedTextBox();
            this.lblMCell = new Wisej.Web.Label();
            this.lblMPhone = new Wisej.Web.Label();
            this.mskMPhone = new Wisej.Web.MaskedTextBox();
            this.txtFFirst = new Wisej.Web.TextBox();
            this.label8 = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.txtFCity = new Wisej.Web.TextBox();
            this.txtFLast = new Wisej.Web.TextBox();
            this.label6 = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.txtFstreet = new Wisej.Web.TextBox();
            this.txtFState = new Wisej.Web.TextBox();
            this.txtFZipplus = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label4 = new Wisej.Web.Label();
            this.txtFZip = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label3 = new Wisej.Web.Label();
            this.lblZipCode = new Wisej.Web.Label();
            this.txtMZip = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblState = new Wisej.Web.Label();
            this.txtMZipPlus = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtMState = new Wisej.Web.TextBox();
            this.txtMStreet = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.txtMLast = new Wisej.Web.TextBox();
            this.lblHouseNo = new Wisej.Web.Label();
            this.txtMCity = new Wisej.Web.TextBox();
            this.lblCityName = new Wisej.Web.Label();
            this.lblFirst = new Wisej.Web.Label();
            this.txtFirst = new Wisej.Web.TextBox();
            this.panel3 = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.lblStreet = new Wisej.Web.Label();
            this.lblSuffixReq = new Wisej.Web.Label();
            this.panel4 = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.PbEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbAdd)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // PbEdit
            // 
            this.PbEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.PbEdit.Dock = Wisej.Web.DockStyle.Left;
            this.PbEdit.ImageSource = "captain-edit";
            this.PbEdit.Location = new System.Drawing.Point(26, 5);
            this.PbEdit.Name = "PbEdit";
            this.PbEdit.Size = new System.Drawing.Size(18, 18);
            this.PbEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.StretchImage;
            this.PbEdit.Visible = false;
            this.PbEdit.Click += new System.EventHandler(this.PbEdit_Click);
            // 
            // PbDelete
            // 
            this.PbDelete.Cursor = Wisej.Web.Cursors.Hand;
            this.PbDelete.Dock = Wisej.Web.DockStyle.Left;
            this.PbDelete.ImageSource = "icon-deleteall";
            this.PbDelete.Location = new System.Drawing.Point(47, 5);
            this.PbDelete.Name = "PbDelete";
            this.PbDelete.Size = new System.Drawing.Size(18, 18);
            this.PbDelete.SizeMode = Wisej.Web.PictureBoxSizeMode.StretchImage;
            this.PbDelete.Visible = false;
            this.PbDelete.Click += new System.EventHandler(this.PbDelete_Click);
            // 
            // PbAdd
            // 
            this.PbAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.PbAdd.Dock = Wisej.Web.DockStyle.Left;
            this.PbAdd.ImageSource = "captain-add";
            this.PbAdd.Location = new System.Drawing.Point(5, 5);
            this.PbAdd.Name = "PbAdd";
            this.PbAdd.Size = new System.Drawing.Size(18, 18);
            this.PbAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.StretchImage;
            this.PbAdd.Visible = false;
            this.PbAdd.Click += new System.EventHandler(this.PbAdd_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(320, 10);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(4, 20);
            this.lblHeader.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(237, 244, 250);
            this.panel1.Controls.Add(this.PbDelete);
            this.panel1.Controls.Add(this.spacer3);
            this.panel1.Controls.Add(this.PbEdit);
            this.panel1.Controls.Add(this.spacer2);
            this.panel1.Controls.Add(this.PbAdd);
            this.panel1.Controls.Add(this.lblHeader);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5);
            this.panel1.Size = new System.Drawing.Size(742, 28);
            this.panel1.TabIndex = 0;
            // 
            // pnlAlertCode
            // 
            this.pnlAlertCode.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAlertCode.Location = new System.Drawing.Point(0, 28);
            this.pnlAlertCode.Name = "pnlAlertCode";
            this.pnlAlertCode.Padding = new Wisej.Web.Padding(0, 3, 0, 0);
            this.pnlAlertCode.Size = new System.Drawing.Size(742, 31);
            this.pnlAlertCode.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtMRelation);
            this.panel2.Controls.Add(this.lblMRelation);
            this.panel2.Controls.Add(this.lblFRelation);
            this.panel2.Controls.Add(this.txtFRelation);
            this.panel2.Controls.Add(this.mskFPhone);
            this.panel2.Controls.Add(this.lblFPhone);
            this.panel2.Controls.Add(this.lblFCell);
            this.panel2.Controls.Add(this.mskFCell);
            this.panel2.Controls.Add(this.mskMCell);
            this.panel2.Controls.Add(this.lblMCell);
            this.panel2.Controls.Add(this.lblMPhone);
            this.panel2.Controls.Add(this.mskMPhone);
            this.panel2.Controls.Add(this.txtFFirst);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.txtFCity);
            this.panel2.Controls.Add(this.txtFLast);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtFstreet);
            this.panel2.Controls.Add(this.txtFState);
            this.panel2.Controls.Add(this.txtFZipplus);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtFZip);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lblZipCode);
            this.panel2.Controls.Add(this.txtMZip);
            this.panel2.Controls.Add(this.lblState);
            this.panel2.Controls.Add(this.txtMZipPlus);
            this.panel2.Controls.Add(this.txtMState);
            this.panel2.Controls.Add(this.txtMStreet);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtMLast);
            this.panel2.Controls.Add(this.lblHouseNo);
            this.panel2.Controls.Add(this.txtMCity);
            this.panel2.Controls.Add(this.lblCityName);
            this.panel2.Controls.Add(this.lblFirst);
            this.panel2.Controls.Add(this.txtFirst);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(742, 261);
            this.panel2.TabIndex = 1;
            // 
            // txtMRelation
            // 
            this.txtMRelation.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMRelation.Enabled = false;
            this.txtMRelation.Location = new System.Drawing.Point(86, 179);
            this.txtMRelation.MaxLength = 20;
            this.txtMRelation.Name = "txtMRelation";
            this.txtMRelation.Size = new System.Drawing.Size(220, 25);
            this.txtMRelation.TabIndex = 10;
            // 
            // lblMRelation
            // 
            this.lblMRelation.AutoSize = true;
            this.lblMRelation.Location = new System.Drawing.Point(11, 180);
            this.lblMRelation.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblMRelation.Name = "lblMRelation";
            this.lblMRelation.Size = new System.Drawing.Size(71, 18);
            this.lblMRelation.TabIndex = 61;
            this.lblMRelation.Text = "Relationship";
            // 
            // lblFRelation
            // 
            this.lblFRelation.AutoSize = true;
            this.lblFRelation.Location = new System.Drawing.Point(389, 184);
            this.lblFRelation.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFRelation.Name = "lblFRelation";
            this.lblFRelation.Size = new System.Drawing.Size(71, 18);
            this.lblFRelation.TabIndex = 61;
            this.lblFRelation.Text = "Relationship";
            // 
            // txtFRelation
            // 
            this.txtFRelation.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFRelation.Enabled = false;
            this.txtFRelation.Location = new System.Drawing.Point(471, 184);
            this.txtFRelation.MaxLength = 20;
            this.txtFRelation.Name = "txtFRelation";
            this.txtFRelation.Size = new System.Drawing.Size(226, 25);
            this.txtFRelation.TabIndex = 20;
            // 
            // mskFPhone
            // 
            this.mskFPhone.Enabled = false;
            this.mskFPhone.Location = new System.Drawing.Point(471, 155);
            this.mskFPhone.Mask = "999-000-0000";
            this.mskFPhone.Name = "mskFPhone";
            this.mskFPhone.Size = new System.Drawing.Size(79, 25);
            this.mskFPhone.TabIndex = 18;
            this.mskFPhone.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblFPhone
            // 
            this.lblFPhone.AutoSize = true;
            this.lblFPhone.Location = new System.Drawing.Point(389, 153);
            this.lblFPhone.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFPhone.Name = "lblFPhone";
            this.lblFPhone.Size = new System.Drawing.Size(43, 18);
            this.lblFPhone.TabIndex = 0;
            this.lblFPhone.Text = "Home#";
            // 
            // lblFCell
            // 
            this.lblFCell.AutoSize = true;
            this.lblFCell.Location = new System.Drawing.Point(565, 158);
            this.lblFCell.Name = "lblFCell";
            this.lblFCell.Size = new System.Drawing.Size(32, 14);
            this.lblFCell.TabIndex = 0;
            this.lblFCell.Text = "Cell#";
            // 
            // mskFCell
            // 
            this.mskFCell.Enabled = false;
            this.mskFCell.Location = new System.Drawing.Point(600, 155);
            this.mskFCell.Mask = "999-000-0000";
            this.mskFCell.Name = "mskFCell";
            this.mskFCell.Size = new System.Drawing.Size(97, 25);
            this.mskFCell.TabIndex = 19;
            this.mskFCell.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // mskMCell
            // 
            this.mskMCell.Enabled = false;
            this.mskMCell.Location = new System.Drawing.Point(218, 150);
            this.mskMCell.Mask = "999-000-0000";
            this.mskMCell.Name = "mskMCell";
            this.mskMCell.Size = new System.Drawing.Size(88, 25);
            this.mskMCell.TabIndex = 9;
            this.mskMCell.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblMCell
            // 
            this.lblMCell.AutoSize = true;
            this.lblMCell.Location = new System.Drawing.Point(180, 153);
            this.lblMCell.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblMCell.Name = "lblMCell";
            this.lblMCell.Size = new System.Drawing.Size(32, 18);
            this.lblMCell.TabIndex = 0;
            this.lblMCell.Text = "Cell#";
            // 
            // lblMPhone
            // 
            this.lblMPhone.AutoSize = true;
            this.lblMPhone.Location = new System.Drawing.Point(11, 153);
            this.lblMPhone.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblMPhone.Name = "lblMPhone";
            this.lblMPhone.Size = new System.Drawing.Size(43, 18);
            this.lblMPhone.TabIndex = 0;
            this.lblMPhone.Text = "Home#";
            // 
            // mskMPhone
            // 
            this.mskMPhone.Enabled = false;
            this.mskMPhone.Location = new System.Drawing.Point(86, 149);
            this.mskMPhone.Mask = "999-000-0000";
            this.mskMPhone.Name = "mskMPhone";
            this.mskMPhone.Size = new System.Drawing.Size(87, 25);
            this.mskMPhone.TabIndex = 8;
            this.mskMPhone.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // txtFFirst
            // 
            this.txtFFirst.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFFirst.Enabled = false;
            this.txtFFirst.Location = new System.Drawing.Point(471, 9);
            this.txtFFirst.MaxLength = 20;
            this.txtFFirst.Name = "txtFFirst";
            this.txtFFirst.Size = new System.Drawing.Size(165, 25);
            this.txtFFirst.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(389, 14);
            this.label8.MinimumSize = new System.Drawing.Size(0, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 18);
            this.label8.TabIndex = 61;
            this.label8.Text = "First";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(388, 97);
            this.label7.MinimumSize = new System.Drawing.Size(0, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 18);
            this.label7.TabIndex = 35;
            this.label7.Text = "City";
            // 
            // txtFCity
            // 
            this.txtFCity.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFCity.Enabled = false;
            this.txtFCity.Location = new System.Drawing.Point(471, 97);
            this.txtFCity.MaxLength = 30;
            this.txtFCity.Name = "txtFCity";
            this.txtFCity.Size = new System.Drawing.Size(226, 25);
            this.txtFCity.TabIndex = 14;
            // 
            // txtFLast
            // 
            this.txtFLast.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFLast.Enabled = false;
            this.txtFLast.Location = new System.Drawing.Point(471, 37);
            this.txtFLast.MaxLength = 20;
            this.txtFLast.Name = "txtFLast";
            this.txtFLast.Size = new System.Drawing.Size(165, 25);
            this.txtFLast.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(388, 41);
            this.label6.MinimumSize = new System.Drawing.Size(0, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 18);
            this.label6.TabIndex = 61;
            this.label6.Text = "Last";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(388, 69);
            this.label5.MinimumSize = new System.Drawing.Size(0, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 18);
            this.label5.TabIndex = 61;
            this.label5.Text = "Street";
            // 
            // txtFstreet
            // 
            this.txtFstreet.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFstreet.Enabled = false;
            this.txtFstreet.Location = new System.Drawing.Point(471, 67);
            this.txtFstreet.MaxLength = 28;
            this.txtFstreet.Name = "txtFstreet";
            this.txtFstreet.Size = new System.Drawing.Size(226, 25);
            this.txtFstreet.TabIndex = 13;
            // 
            // txtFState
            // 
            this.txtFState.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFState.Enabled = false;
            this.txtFState.Location = new System.Drawing.Point(471, 127);
            this.txtFState.MaxLength = 2;
            this.txtFState.Name = "txtFState";
            this.txtFState.RightToLeft = Wisej.Web.RightToLeft.Yes;
            this.txtFState.Size = new System.Drawing.Size(36, 25);
            this.txtFState.TabIndex = 15;
            // 
            // txtFZipplus
            // 
            this.txtFZipplus.Enabled = false;
            this.txtFZipplus.Location = new System.Drawing.Point(642, 126);
            this.txtFZipplus.MaxLength = 4;
            this.txtFZipplus.Name = "txtFZipplus";
            this.txtFZipplus.Size = new System.Drawing.Size(53, 25);
            this.txtFZipplus.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(388, 125);
            this.label4.MinimumSize = new System.Drawing.Size(0, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 18);
            this.label4.TabIndex = 56;
            this.label4.Text = "State";
            // 
            // txtFZip
            // 
            this.txtFZip.Enabled = false;
            this.txtFZip.Location = new System.Drawing.Point(600, 127);
            this.txtFZip.MaxLength = 5;
            this.txtFZip.Name = "txtFZip";
            this.txtFZip.Size = new System.Drawing.Size(38, 25);
            this.txtFZip.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(565, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 14);
            this.label3.TabIndex = 34;
            this.label3.Text = "Zip";
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(180, 123);
            this.lblZipCode.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(21, 18);
            this.lblZipCode.TabIndex = 34;
            this.lblZipCode.Text = "Zip";
            // 
            // txtMZip
            // 
            this.txtMZip.Enabled = false;
            this.txtMZip.Location = new System.Drawing.Point(215, 121);
            this.txtMZip.MaxLength = 5;
            this.txtMZip.Name = "txtMZip";
            this.txtMZip.Size = new System.Drawing.Size(36, 25);
            this.txtMZip.TabIndex = 6;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(11, 126);
            this.lblState.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(33, 18);
            this.lblState.TabIndex = 56;
            this.lblState.Text = "State";
            // 
            // txtMZipPlus
            // 
            this.txtMZipPlus.Enabled = false;
            this.txtMZipPlus.Location = new System.Drawing.Point(255, 121);
            this.txtMZipPlus.MaxLength = 4;
            this.txtMZipPlus.Name = "txtMZipPlus";
            this.txtMZipPlus.Size = new System.Drawing.Size(51, 25);
            this.txtMZipPlus.TabIndex = 7;
            // 
            // txtMState
            // 
            this.txtMState.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMState.Enabled = false;
            this.txtMState.Location = new System.Drawing.Point(86, 121);
            this.txtMState.MaxLength = 2;
            this.txtMState.Name = "txtMState";
            this.txtMState.RightToLeft = Wisej.Web.RightToLeft.Yes;
            this.txtMState.Size = new System.Drawing.Size(36, 25);
            this.txtMState.TabIndex = 5;
            // 
            // txtMStreet
            // 
            this.txtMStreet.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMStreet.Enabled = false;
            this.txtMStreet.Location = new System.Drawing.Point(86, 65);
            this.txtMStreet.MaxLength = 28;
            this.txtMStreet.Name = "txtMStreet";
            this.txtMStreet.Size = new System.Drawing.Size(220, 25);
            this.txtMStreet.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 72);
            this.label2.MinimumSize = new System.Drawing.Size(0, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 18);
            this.label2.TabIndex = 61;
            this.label2.Text = "Street";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 45);
            this.label1.MinimumSize = new System.Drawing.Size(0, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 18);
            this.label1.TabIndex = 61;
            this.label1.Text = "Last";
            // 
            // txtMLast
            // 
            this.txtMLast.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMLast.Enabled = false;
            this.txtMLast.Location = new System.Drawing.Point(86, 37);
            this.txtMLast.MaxLength = 20;
            this.txtMLast.Name = "txtMLast";
            this.txtMLast.Size = new System.Drawing.Size(165, 25);
            this.txtMLast.TabIndex = 2;
            // 
            // lblHouseNo
            // 
            this.lblHouseNo.AutoSize = true;
            this.lblHouseNo.Enabled = false;
            this.lblHouseNo.Location = new System.Drawing.Point(-24, 87);
            this.lblHouseNo.Name = "lblHouseNo";
            this.lblHouseNo.Size = new System.Drawing.Size(22, 14);
            this.lblHouseNo.TabIndex = 2;
            this.lblHouseNo.Text = "HN";
            // 
            // txtMCity
            // 
            this.txtMCity.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtMCity.Enabled = false;
            this.txtMCity.Location = new System.Drawing.Point(86, 93);
            this.txtMCity.MaxLength = 30;
            this.txtMCity.Name = "txtMCity";
            this.txtMCity.Size = new System.Drawing.Size(220, 25);
            this.txtMCity.TabIndex = 4;
            // 
            // lblCityName
            // 
            this.lblCityName.AutoSize = true;
            this.lblCityName.Location = new System.Drawing.Point(11, 99);
            this.lblCityName.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblCityName.Name = "lblCityName";
            this.lblCityName.Size = new System.Drawing.Size(25, 18);
            this.lblCityName.TabIndex = 35;
            this.lblCityName.Text = "City";
            // 
            // lblFirst
            // 
            this.lblFirst.AutoSize = true;
            this.lblFirst.Location = new System.Drawing.Point(11, 18);
            this.lblFirst.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(28, 18);
            this.lblFirst.TabIndex = 61;
            this.lblFirst.Text = "First";
            // 
            // txtFirst
            // 
            this.txtFirst.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtFirst.Enabled = false;
            this.txtFirst.Location = new System.Drawing.Point(86, 9);
            this.txtFirst.MaxLength = 20;
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(165, 25);
            this.txtFirst.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.spacer1);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 226);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(4);
            this.panel3.Size = new System.Drawing.Size(742, 35);
            this.panel3.TabIndex = 19;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(583, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "&Save";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(658, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(663, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Enabled = false;
            this.lblStreet.Location = new System.Drawing.Point(76, 87);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(41, 13);
            this.lblStreet.TabIndex = 10;
            this.lblStreet.Text = "Street";
            // 
            // lblSuffixReq
            // 
            this.lblSuffixReq.AutoSize = true;
            this.lblSuffixReq.ForeColor = System.Drawing.Color.Red;
            this.lblSuffixReq.Location = new System.Drawing.Point(310, 86);
            this.lblSuffixReq.Name = "lblSuffixReq";
            this.lblSuffixReq.Size = new System.Drawing.Size(13, 13);
            this.lblSuffixReq.TabIndex = 28;
            this.lblSuffixReq.Text = "*";
            this.lblSuffixReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSuffixReq.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.pnlAlertCode);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(742, 320);
            this.panel4.TabIndex = 2;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(23, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 18);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(44, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 18);
            // 
            // HSS00430Form
            // 
            this.ClientSize = new System.Drawing.Size(742, 320);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00430Form";
            this.Text = "HSS00430Form";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSS00430Form_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.PbEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbAdd)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private PictureBox PbEdit;
        private PictureBox PbDelete;
        private PictureBox PbAdd;
        private Label lblHeader;
        private Panel panel1;
        private Panel pnlAlertCode;
        private Panel panel2;
        private Panel panel3;
        private TextBox txtMStreet;
        private Label label2;
        private Label label1;
        private TextBox txtMLast;
        private Label lblHouseNo;
        private TextBox txtMCity;
        private Label lblCityName;
        private Label lblFirst;
        private TextBox txtFirst;
        private Label lblStreet;
        private Label lblSuffixReq;
        private Label lblZipCode;
        private TextBoxWithValidation txtMZip;
        private Label lblState;
        private TextBoxWithValidation txtMZipPlus;
        private TextBox txtMState;
        private TextBox txtFFirst;
        private Label label8;
        private Label label7;
        private TextBox txtFCity;
        private TextBox txtFLast;
        private Label label6;
        private Label label5;
        private TextBox txtFstreet;
        private TextBox txtFState;
        private TextBoxWithValidation txtFZipplus;
        private Label label4;
        private TextBoxWithValidation txtFZip;
        private Label label3;
        private Button btnSave;
        private Button btnCancel;
        private MaskedTextBox mskFPhone;
        private Label lblFPhone;
        private Label lblFCell;
        private MaskedTextBox mskFCell;
        private MaskedTextBox mskMCell;
        private Label lblMCell;
        private Label lblMPhone;
        private MaskedTextBox mskMPhone;
        private TextBox txtMRelation;
        private Label lblMRelation;
        private Label lblFRelation;
        private TextBox txtFRelation;
        private Spacer spacer1;
        private Panel panel4;
        private Spacer spacer3;
        private Spacer spacer2;
    }
}