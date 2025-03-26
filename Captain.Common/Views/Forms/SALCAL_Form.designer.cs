using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class SALCAL_Form
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
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SALCAL_Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool2 = new Wisej.Web.ComponentTool();
            this.panel1 = new Wisej.Web.Panel();
            this.DT_Duration = new Wisej.Web.DateTimePicker();
            this.DT_Dur_To = new Wisej.Web.DateTimePicker();
            this.DT_Dur_From = new Wisej.Web.DateTimePicker();
            this.cmbAttn = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.cmbReceipent = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.cmbLocation = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.cmbStatus = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.CmbWorker = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.CmbFunding = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.Act_Date = new Wisej.Web.DateTimePicker();
            this.lblAttn = new Wisej.Web.Label();
            this.lblRecipient = new Wisej.Web.Label();
            this.lblLocation = new Wisej.Web.Label();
            this.lblStatus = new Wisej.Web.Label();
            this.lblStaff = new Wisej.Web.Label();
            this.lblFund = new Wisej.Web.Label();
            this.lblTimeSpent = new Wisej.Web.Label();
            this.lblTimeOut = new Wisej.Web.Label();
            this.lblTimeIn = new Wisej.Web.Label();
            this.lblSerDate = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.panel7 = new Wisej.Web.Panel();
            this.gvSALNames = new Wisej.Web.DataGridView();
            this.Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SAL_ID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvSign = new Wisej.Web.DataGridViewTextBoxColumn();
            this.label1 = new Wisej.Web.Label();
            this.panel6 = new Wisej.Web.Panel();
            this.txtRecName = new Wisej.Web.TextBox();
            this.lblRName = new Wisej.Web.Label();
            this.panel3 = new Wisej.Web.Panel();
            this.pnlResp = new Wisej.Web.Panel();
            this.pnlSPNotes = new Wisej.Web.Panel();
            this.txtNotes = new Wisej.Web.TextBox();
            this.lblSPName = new Wisej.Web.Label();
            this.pnlQues = new Wisej.Web.Panel();
            this.btnQuesCanel = new Wisej.Web.Button();
            this.btnQuesOk = new Wisej.Web.Button();
            this.txtQues3 = new Wisej.Web.TextBox();
            this.txtQues2 = new Wisej.Web.TextBox();
            this.txtQues1 = new Wisej.Web.TextBox();
            this.LblQues = new Wisej.Web.Label();
            this.gvQuestions = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.label2 = new Wisej.Web.Label();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.btnSign = new Wisej.Web.Button();
            this.panel4 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.btnSave = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.lbldesc = new Wisej.Web.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSALNames)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlResp.SuspendLayout();
            this.pnlSPNotes.SuspendLayout();
            this.pnlQues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuestions)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(247, 247, 247);
            this.panel1.Controls.Add(this.DT_Duration);
            this.panel1.Controls.Add(this.DT_Dur_To);
            this.panel1.Controls.Add(this.DT_Dur_From);
            this.panel1.Controls.Add(this.cmbAttn);
            this.panel1.Controls.Add(this.cmbReceipent);
            this.panel1.Controls.Add(this.cmbLocation);
            this.panel1.Controls.Add(this.cmbStatus);
            this.panel1.Controls.Add(this.CmbWorker);
            this.panel1.Controls.Add(this.CmbFunding);
            this.panel1.Controls.Add(this.Act_Date);
            this.panel1.Controls.Add(this.lblAttn);
            this.panel1.Controls.Add(this.lblRecipient);
            this.panel1.Controls.Add(this.lblLocation);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.lblStaff);
            this.panel1.Controls.Add(this.lblFund);
            this.panel1.Controls.Add(this.lblTimeSpent);
            this.panel1.Controls.Add(this.lblTimeOut);
            this.panel1.Controls.Add(this.lblTimeIn);
            this.panel1.Controls.Add(this.lblSerDate);
            this.panel1.CssStyle = "border-bottom:1px dashed #CCC; border-top:1px dashed #CCC;";
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 154);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 115);
            this.panel1.TabIndex = 2;
            // 
            // DT_Duration
            // 
            this.DT_Duration.Checked = false;
            this.DT_Duration.CustomFormat = "HH:mm";
            this.DT_Duration.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.DT_Duration.Location = new System.Drawing.Point(765, 16);
            this.DT_Duration.MinimumSize = new System.Drawing.Size(0, 25);
            this.DT_Duration.Name = "DT_Duration";
            this.DT_Duration.ShowCheckBox = true;
            this.DT_Duration.ShowToolTips = false;
            this.DT_Duration.ShowUpDown = true;
            this.DT_Duration.Size = new System.Drawing.Size(100, 25);
            this.DT_Duration.TabIndex = 4;
            this.DT_Duration.Value = new System.DateTime(2023, 1, 20, 0, 0, 0, 0);
            this.DT_Duration.Leave += new System.EventHandler(this.DT_Duration_Leave);
            // 
            // DT_Dur_To
            // 
            this.DT_Dur_To.Checked = false;
            this.DT_Dur_To.CustomFormat = "hh:mm:ss tt";
            this.DT_Dur_To.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.DT_Dur_To.Location = new System.Drawing.Point(534, 16);
            this.DT_Dur_To.MinimumSize = new System.Drawing.Size(0, 25);
            this.DT_Dur_To.Name = "DT_Dur_To";
            this.DT_Dur_To.ShowCheckBox = true;
            this.DT_Dur_To.ShowToolTips = false;
            this.DT_Dur_To.ShowUpDown = true;
            this.DT_Dur_To.Size = new System.Drawing.Size(116, 25);
            this.DT_Dur_To.TabIndex = 3;
            this.DT_Dur_To.Value = new System.DateTime(2023, 1, 20, 0, 0, 0, 0);
            this.DT_Dur_To.Leave += new System.EventHandler(this.DT_Duration_Leave);
            // 
            // DT_Dur_From
            // 
            this.DT_Dur_From.Checked = false;
            this.DT_Dur_From.CustomFormat = "hh:mm:ss tt";
            this.DT_Dur_From.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.DT_Dur_From.Location = new System.Drawing.Point(298, 17);
            this.DT_Dur_From.MinimumSize = new System.Drawing.Size(0, 25);
            this.DT_Dur_From.Name = "DT_Dur_From";
            this.DT_Dur_From.ShowCheckBox = true;
            this.DT_Dur_From.ShowToolTips = false;
            this.DT_Dur_From.ShowUpDown = true;
            this.DT_Dur_From.Size = new System.Drawing.Size(116, 25);
            this.DT_Dur_From.TabIndex = 2;
            this.DT_Dur_From.Value = new System.DateTime(2023, 1, 20, 0, 0, 0, 0);
            this.DT_Dur_From.Leave += new System.EventHandler(this.DT_Duration_Leave);
            // 
            // cmbAttn
            // 
            this.cmbAttn.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAttn.FormattingEnabled = true;
            this.cmbAttn.Location = new System.Drawing.Point(534, 74);
            this.cmbAttn.Name = "cmbAttn";
            this.cmbAttn.Size = new System.Drawing.Size(459, 25);
            this.cmbAttn.TabIndex = 10;
            this.cmbAttn.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            // 
            // cmbReceipent
            // 
            this.cmbReceipent.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReceipent.FormattingEnabled = true;
            this.cmbReceipent.Location = new System.Drawing.Point(104, 73);
            this.cmbReceipent.Name = "cmbReceipent";
            this.cmbReceipent.Size = new System.Drawing.Size(329, 25);
            this.cmbReceipent.TabIndex = 9;
            this.cmbReceipent.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(845, 45);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(147, 25);
            this.cmbLocation.TabIndex = 8;
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(534, 45);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(235, 25);
            this.cmbStatus.TabIndex = 7;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbAttn_SelectedIndexChanged);
            // 
            // CmbWorker
            // 
            this.CmbWorker.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbWorker.Enabled = false;
            this.CmbWorker.FormattingEnabled = true;
            this.CmbWorker.Location = new System.Drawing.Point(298, 45);
            this.CmbWorker.Name = "CmbWorker";
            this.CmbWorker.Size = new System.Drawing.Size(135, 25);
            this.CmbWorker.TabIndex = 6;
            // 
            // CmbFunding
            // 
            this.CmbFunding.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbFunding.Enabled = false;
            this.CmbFunding.FormattingEnabled = true;
            this.CmbFunding.Location = new System.Drawing.Point(104, 45);
            this.CmbFunding.Name = "CmbFunding";
            this.CmbFunding.Size = new System.Drawing.Size(113, 25);
            this.CmbFunding.TabIndex = 5;
            // 
            // Act_Date
            // 
            this.Act_Date.Checked = false;
            this.Act_Date.CustomFormat = "MM/dd/yyyy";
            this.Act_Date.Enabled = false;
            this.Act_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Act_Date.Location = new System.Drawing.Point(104, 16);
            this.Act_Date.MinimumSize = new System.Drawing.Size(0, 25);
            this.Act_Date.Name = "Act_Date";
            this.Act_Date.ShowToolTips = false;
            this.Act_Date.Size = new System.Drawing.Size(113, 25);
            this.Act_Date.TabIndex = 1;
            // 
            // lblAttn
            // 
            this.lblAttn.AutoSize = true;
            this.lblAttn.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAttn.Location = new System.Drawing.Point(448, 77);
            this.lblAttn.Name = "lblAttn";
            this.lblAttn.Size = new System.Drawing.Size(89, 14);
            this.lblAttn.TabIndex = 10;
            this.lblAttn.Text = "ATTENDANCE:";
            // 
            // lblRecipient
            // 
            this.lblRecipient.AutoSize = true;
            this.lblRecipient.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRecipient.Location = new System.Drawing.Point(15, 76);
            this.lblRecipient.Name = "lblRecipient";
            this.lblRecipient.Size = new System.Drawing.Size(72, 14);
            this.lblRecipient.TabIndex = 9;
            this.lblRecipient.Text = "RECIPIENT:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLocation.Location = new System.Drawing.Point(776, 50);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(69, 14);
            this.lblLocation.TabIndex = 8;
            this.lblLocation.Text = "LOCATION:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStatus.Location = new System.Drawing.Point(448, 50);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(55, 14);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "STATUS:";
            // 
            // lblStaff
            // 
            this.lblStaff.AutoSize = true;
            this.lblStaff.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStaff.Location = new System.Drawing.Point(242, 48);
            this.lblStaff.Name = "lblStaff";
            this.lblStaff.Size = new System.Drawing.Size(46, 14);
            this.lblStaff.TabIndex = 6;
            this.lblStaff.Text = "STAFF:";
            // 
            // lblFund
            // 
            this.lblFund.AutoSize = true;
            this.lblFund.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFund.Location = new System.Drawing.Point(15, 48);
            this.lblFund.Name = "lblFund";
            this.lblFund.Size = new System.Drawing.Size(62, 14);
            this.lblFund.TabIndex = 5;
            this.lblFund.Text = "FUNDING:";
            // 
            // lblTimeSpent
            // 
            this.lblTimeSpent.AutoSize = true;
            this.lblTimeSpent.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTimeSpent.Location = new System.Drawing.Point(679, 21);
            this.lblTimeSpent.Name = "lblTimeSpent";
            this.lblTimeSpent.Size = new System.Drawing.Size(80, 14);
            this.lblTimeSpent.TabIndex = 4;
            this.lblTimeSpent.Text = "TIME SPENT:";
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.AutoSize = true;
            this.lblTimeOut.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTimeOut.Location = new System.Drawing.Point(448, 21);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(65, 14);
            this.lblTimeOut.TabIndex = 3;
            this.lblTimeOut.Text = "TIME OUT:";
            // 
            // lblTimeIn
            // 
            this.lblTimeIn.AutoSize = true;
            this.lblTimeIn.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTimeIn.Location = new System.Drawing.Point(242, 21);
            this.lblTimeIn.Name = "lblTimeIn";
            this.lblTimeIn.Size = new System.Drawing.Size(52, 14);
            this.lblTimeIn.TabIndex = 2;
            this.lblTimeIn.Text = "TIME IN:";
            // 
            // lblSerDate
            // 
            this.lblSerDate.AutoSize = true;
            this.lblSerDate.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSerDate.Location = new System.Drawing.Point(13, 21);
            this.lblSerDate.Name = "lblSerDate";
            this.lblSerDate.Size = new System.Drawing.Size(96, 14);
            this.lblSerDate.TabIndex = 1;
            this.lblSerDate.Text = "SERVICE DATE:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1016, 154);
            this.panel2.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gvSALNames);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Dock = Wisej.Web.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 35);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1016, 117);
            this.panel7.TabIndex = 0;
            // 
            // gvSALNames
            // 
            this.gvSALNames.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvSALNames.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.gvSALNames.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Name,
            this.SAL_ID,
            this.gvSign});
            this.gvSALNames.Dock = Wisej.Web.DockStyle.Fill;
            this.gvSALNames.Location = new System.Drawing.Point(0, 22);
            this.gvSALNames.Name = "gvSALNames";
            this.gvSALNames.RowHeadersWidth = 22;
            this.gvSALNames.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvSALNames.ShowColumnVisibilityMenu = false;
            this.gvSALNames.Size = new System.Drawing.Size(1016, 95);
            this.gvSALNames.TabIndex = 0;
            this.gvSALNames.TabStop = false;
            this.gvSALNames.SelectionChanged += new System.EventHandler(this.gvSALNames_SelectionChanged);
            // 
            // Name
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Name.HeaderStyle = dataGridViewCellStyle1;
            this.Name.HeaderText = "Name";
            this.Name.Name = "Name";
            this.Name.ReadOnly = true;
            this.Name.Width = 750;
            // 
            // SAL_ID
            // 
            this.SAL_ID.HeaderText = "SAL_ID";
            this.SAL_ID.Name = "SAL_ID";
            this.SAL_ID.ShowInVisibilityMenu = false;
            this.SAL_ID.Visible = false;
            // 
            // gvSign
            // 
            this.gvSign.HeaderText = "gvSign";
            this.gvSign.Name = "gvSign";
            this.gvSign.ShowInVisibilityMenu = false;
            this.gvSign.Visible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(214, 214, 214);
            this.label1.Dock = Wisej.Web.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new Wisej.Web.Padding(15, 4, 4, 4);
            this.label1.Size = new System.Drawing.Size(1016, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "Activity Log";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtRecName);
            this.panel6.Controls.Add(this.lblRName);
            this.panel6.Dock = Wisej.Web.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new Wisej.Web.Padding(15, 5, 5, 5);
            this.panel6.Size = new System.Drawing.Size(1016, 35);
            this.panel6.TabIndex = 1;
            // 
            // txtRecName
            // 
            this.txtRecName.Dock = Wisej.Web.DockStyle.Left;
            this.txtRecName.Enabled = false;
            this.txtRecName.Location = new System.Drawing.Point(114, 5);
            this.txtRecName.Name = "txtRecName";
            this.txtRecName.Size = new System.Drawing.Size(425, 25);
            this.txtRecName.TabIndex = 1;
            // 
            // lblRName
            // 
            this.lblRName.Dock = Wisej.Web.DockStyle.Left;
            this.lblRName.Location = new System.Drawing.Point(15, 5);
            this.lblRName.Name = "lblRName";
            this.lblRName.Size = new System.Drawing.Size(99, 25);
            this.lblRName.TabIndex = 0;
            this.lblRName.Text = "Recipient Name:";
            this.lblRName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pnlResp);
            this.panel3.Controls.Add(this.gvQuestions);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 269);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1016, 315);
            this.panel3.TabIndex = 0;
            // 
            // pnlResp
            // 
            this.pnlResp.Controls.Add(this.pnlSPNotes);
            this.pnlResp.Controls.Add(this.pnlQues);
            this.pnlResp.Dock = Wisej.Web.DockStyle.Top;
            this.pnlResp.Location = new System.Drawing.Point(0, 187);
            this.pnlResp.Name = "pnlResp";
            this.pnlResp.Size = new System.Drawing.Size(1016, 128);
            this.pnlResp.TabIndex = 3;
            // 
            // pnlSPNotes
            // 
            this.pnlSPNotes.Controls.Add(this.txtNotes);
            this.pnlSPNotes.Controls.Add(this.lblSPName);
            this.pnlSPNotes.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSPNotes.Location = new System.Drawing.Point(571, 0);
            this.pnlSPNotes.Name = "pnlSPNotes";
            this.pnlSPNotes.Size = new System.Drawing.Size(445, 128);
            this.pnlSPNotes.TabIndex = 2;
            this.pnlSPNotes.Visible = false;
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(7, 23);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(387, 78);
            this.txtNotes.TabIndex = 1;
            // 
            // lblSPName
            // 
            this.lblSPName.AutoSize = true;
            this.lblSPName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSPName.Location = new System.Drawing.Point(12, 6);
            this.lblSPName.Name = "lblSPName";
            this.lblSPName.Size = new System.Drawing.Size(64, 15);
            this.lblSPName.TabIndex = 1;
            this.lblSPName.Text = "ISP Notes";
            // 
            // pnlQues
            // 
            this.pnlQues.Controls.Add(this.btnQuesCanel);
            this.pnlQues.Controls.Add(this.btnQuesOk);
            this.pnlQues.Controls.Add(this.txtQues3);
            this.pnlQues.Controls.Add(this.txtQues2);
            this.pnlQues.Controls.Add(this.txtQues1);
            this.pnlQues.Controls.Add(this.LblQues);
            this.pnlQues.Dock = Wisej.Web.DockStyle.Left;
            this.pnlQues.Location = new System.Drawing.Point(0, 0);
            this.pnlQues.Name = "pnlQues";
            this.pnlQues.Size = new System.Drawing.Size(571, 128);
            this.pnlQues.TabIndex = 1;
            this.pnlQues.Visible = false;
            // 
            // btnQuesCanel
            // 
            this.btnQuesCanel.Location = new System.Drawing.Point(488, 97);
            this.btnQuesCanel.Name = "btnQuesCanel";
            this.btnQuesCanel.Size = new System.Drawing.Size(60, 24);
            this.btnQuesCanel.TabIndex = 5;
            this.btnQuesCanel.Text = "Cance&l";
            this.btnQuesCanel.Click += new System.EventHandler(this.btnQuesCanel_Click);
            // 
            // btnQuesOk
            // 
            this.btnQuesOk.Location = new System.Drawing.Point(424, 97);
            this.btnQuesOk.Name = "btnQuesOk";
            this.btnQuesOk.Size = new System.Drawing.Size(60, 24);
            this.btnQuesOk.TabIndex = 4;
            this.btnQuesOk.Text = "O&K";
            this.btnQuesOk.Click += new System.EventHandler(this.btnQuesOk_Click);
            // 
            // txtQues3
            // 
            this.txtQues3.Location = new System.Drawing.Point(80, 66);
            this.txtQues3.MaxLength = 80;
            this.txtQues3.Name = "txtQues3";
            this.txtQues3.Size = new System.Drawing.Size(466, 25);
            this.txtQues3.TabIndex = 3;
            // 
            // txtQues2
            // 
            this.txtQues2.Location = new System.Drawing.Point(80, 37);
            this.txtQues2.MaxLength = 80;
            this.txtQues2.Name = "txtQues2";
            this.txtQues2.Size = new System.Drawing.Size(466, 25);
            this.txtQues2.TabIndex = 2;
            // 
            // txtQues1
            // 
            this.txtQues1.Location = new System.Drawing.Point(80, 8);
            this.txtQues1.MaxLength = 80;
            this.txtQues1.Name = "txtQues1";
            this.txtQues1.Size = new System.Drawing.Size(466, 25);
            this.txtQues1.TabIndex = 1;
            // 
            // LblQues
            // 
            this.LblQues.Location = new System.Drawing.Point(15, 10);
            this.LblQues.Name = "LblQues";
            this.LblQues.Size = new System.Drawing.Size(59, 20);
            this.LblQues.TabIndex = 0;
            this.LblQues.Text = "Response";
            // 
            // gvQuestions
            // 
            this.gvQuestions.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvQuestions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvQuestions.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvQuestions.Dock = Wisej.Web.DockStyle.Top;
            this.gvQuestions.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.gvQuestions.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvQuestions.Location = new System.Drawing.Point(0, 24);
            this.gvQuestions.Name = "gvQuestions";
            this.gvQuestions.RowHeadersWidth = 22;
            this.gvQuestions.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvQuestions.ShowColumnVisibilityMenu = false;
            this.gvQuestions.Size = new System.Drawing.Size(1016, 163);
            this.gvQuestions.TabIndex = 4;
            this.gvQuestions.TabStop = false;
            this.gvQuestions.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvQuestions_CellClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(214, 214, 214);
            this.label2.Dock = Wisej.Web.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new Wisej.Web.Padding(15, 5, 5, 5);
            this.label2.Size = new System.Drawing.Size(1016, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Areas addressed during visit";
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            this.contextMenu1.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.gvQuestions_MenuClick);
            // 
            // btnSign
            // 
            this.btnSign.Dock = Wisej.Web.DockStyle.Right;
            this.btnSign.Location = new System.Drawing.Point(911, 4);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(90, 27);
            this.btnSign.TabIndex = 4;
            this.btnSign.Text = "S&ignature";
            this.btnSign.Visible = false;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // panel4
            // 
            this.panel4.AppearanceKey = "panel-grdo";
            this.panel4.Controls.Add(this.btnOk);
            this.panel4.Controls.Add(this.spacer3);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.spacer2);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.spacer1);
            this.panel4.Controls.Add(this.btnSign);
            this.panel4.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 584);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(4, 4, 15, 4);
            this.panel4.Size = new System.Drawing.Size(1016, 35);
            this.panel4.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(508, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 27);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&OK";
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(576, 4);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(182, 27);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(758, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 27);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(826, 4);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 27);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(831, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(906, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // lbldesc
            // 
            this.lbldesc.AutoSize = true;
            this.lbldesc.Location = new System.Drawing.Point(9, 81);
            this.lbldesc.Name = "lbldesc";
            this.lbldesc.Size = new System.Drawing.Size(35, 13);
            this.lbldesc.TabIndex = 12;
            this.lbldesc.Text = "AREAS ADDRESSED DURING VISIT AS OUTLINED BY THE AUTHORIZATION OR ISP:";
            // 
            // SALCAL_Form
            // 
            this.ClientSize = new System.Drawing.Size(1016, 619);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.Name = "SALCAL_Form";
            this.Text = "SALCAL";
            componentTool1.ImageSource = "captain-pdf";
            componentTool1.Name = "PbPdf";
            componentTool1.ToolTipText = "PDF";
            componentTool2.ImageSource = "captain-casenotes";
            componentTool2.Name = "picSPMNotes";
            componentTool2.ToolTipText = "SPM Notes";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1,
            componentTool2});
            this.FormClosing += new Wisej.Web.FormClosingEventHandler(this.SALCAL_Form_FormClosing);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.SALCAL_Form_ToolClick);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvSALNames)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnlResp.ResumeLayout(false);
            this.pnlSPNotes.ResumeLayout(false);
            this.pnlSPNotes.PerformLayout();
            this.pnlQues.ResumeLayout(false);
            this.pnlQues.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuestions)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Panel panel1;
        private Label lblAttn;
        private Label lblRecipient;
        private Label lblLocation;
        private Label lblStatus;
        private Label lblStaff;
        private Label lblFund;
        private Label lblTimeSpent;
        private Label lblTimeOut;
        private Label lblTimeIn;
        private Label lblSerDate;
        private Panel panel2;
        private DataGridView gvSALNames;
        private DataGridViewTextBoxColumn Name;
        private Panel panel3;

        private DataGridViewTextBoxColumn SAL_ID;
        private Label label1;
        private Label label2;
        
        private Panel panel4;
        private Button btnCancel;
        private Button btnOk;
        private DateTimePicker Act_Date;
        private ComboBoxEx CmbFunding;
        private ComboBoxEx CmbWorker;
        private ComboBoxEx cmbLocation;
        private ComboBoxEx cmbStatus;
        private ComboBoxEx cmbAttn;
        private ComboBoxEx cmbReceipent;
        private DateTimePicker DT_Dur_From;
        private DateTimePicker DT_Dur_To;
        private DateTimePicker DT_Duration;
        private ContextMenu contextMenu1;
      
        private Panel pnlQues;
        private TextBox txtQues3;
        private TextBox txtQues2;
        private TextBox txtQues1;
        private Label LblQues;
        private Button btnQuesCanel;
        private Button btnQuesOk;
        
        private Button btnSign;
        private DataGridViewTextBoxColumn gvSign;
        private Label lblRName;
        private TextBox txtRecName;
        private Button btnSave;
        private Label lbldesc;
        private Panel pnlSPNotes;
        private TextBox txtNotes;
        private Label lblSPName;
        private Spacer spacer3;
        private Spacer spacer2;
        private Spacer spacer1;
        private Panel panel7;
        private Panel panel6;
        private Panel pnlResp;
        private Captain.Common.Views.Controls.Compatibility.DataGridViewEx gvQuestions;
    }
}