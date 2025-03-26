using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class RdlcUserForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdlcUserForm));
            this.gvwData = new Wisej.Web.DataGridView();
            this.gvISelect = new Wisej.Web.DataGridViewImageColumn();
            this.gvtPCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtUserId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSelect = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnOk = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlBottom = new Wisej.Web.Panel();
            this.pnlgvwData = new Wisej.Web.Panel();
            this.pnlOK = new Wisej.Web.Panel();
            this.chkUnselectAll = new Wisej.Web.CheckBox();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            this.chkSelectAll = new Wisej.Web.CheckBox();
            this.pnlTop = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.cmbChartType = new Wisej.Web.ComboBox();
            this.pnlProcess = new Wisej.Web.Panel();
            this.btnGeneraterpt = new Wisej.Web.Button();
            this.pnlProgram = new Wisej.Web.Panel();
            this.rdoPSelect = new Wisej.Web.RadioButton();
            this.rdoPAll = new Wisej.Web.RadioButton();
            this.lblProgram = new Wisej.Web.Label();
            this.pnlUser = new Wisej.Web.Panel();
            this.rdoUSelect = new Wisej.Web.RadioButton();
            this.rdoUAll = new Wisej.Web.RadioButton();
            this.lblUsers = new Wisej.Web.Label();
            this.pnlMain = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlgvwData.SuspendLayout();
            this.pnlOK.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlProcess.SuspendLayout();
            this.pnlProgram.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwData
            // 
            this.gvwData.AllowUserToResizeColumns = false;
            this.gvwData.AllowUserToResizeRows = false;
            this.gvwData.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwData.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvISelect,
            this.gvtPCode,
            this.gvtUserId,
            this.gvtSelect});
            this.gvwData.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwData.Location = new System.Drawing.Point(0, 0);
            this.gvwData.Name = "gvwData";
            this.gvwData.RowHeadersVisible = false;
            this.gvwData.RowHeadersWidth = 10;
            this.gvwData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwData.Size = new System.Drawing.Size(413, 250);
            this.gvwData.TabIndex = 0;
            this.gvwData.TabStop = false;
            this.gvwData.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwData_CellClick);
            // 
            // gvISelect
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = null;
            this.gvISelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvISelect.HeaderText = " ";
            this.gvISelect.Name = "gvISelect";
            this.gvISelect.ShowInVisibilityMenu = false;
            this.gvISelect.Width = 40;
            // 
            // gvtPCode
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtPCode.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtPCode.HeaderStyle = dataGridViewCellStyle3;
            this.gvtPCode.HeaderText = "Code";
            this.gvtPCode.Name = "gvtPCode";
            this.gvtPCode.Width = 60;
            // 
            // gvtUserId
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtUserId.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtUserId.HeaderStyle = dataGridViewCellStyle5;
            this.gvtUserId.HeaderText = "User Name";
            this.gvtUserId.Name = "gvtUserId";
            this.gvtUserId.Width = 300;
            // 
            // gvtSelect
            // 
            this.gvtSelect.HeaderText = " ";
            this.gvtSelect.Name = "gvtSelect";
            this.gvtSelect.ShowInVisibilityMenu = false;
            this.gvtSelect.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(260, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(323, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlgvwData);
            this.pnlBottom.Controls.Add(this.pnlOK);
            this.pnlBottom.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 128);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(413, 285);
            this.pnlBottom.TabIndex = 0;
            // 
            // pnlgvwData
            // 
            this.pnlgvwData.Controls.Add(this.gvwData);
            this.pnlgvwData.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwData.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwData.Name = "pnlgvwData";
            this.pnlgvwData.Size = new System.Drawing.Size(413, 250);
            this.pnlgvwData.TabIndex = 0;
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.chkUnselectAll);
            this.pnlOK.Controls.Add(this.spacer2);
            this.pnlOK.Controls.Add(this.btnOk);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.chkSelectAll);
            this.pnlOK.Controls.Add(this.btnCancel);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 250);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(413, 35);
            this.pnlOK.TabIndex = 1;
            // 
            // chkUnselectAll
            // 
            this.chkUnselectAll.Dock = Wisej.Web.DockStyle.Left;
            this.chkUnselectAll.Location = new System.Drawing.Point(103, 5);
            this.chkUnselectAll.Name = "chkUnselectAll";
            this.chkUnselectAll.Size = new System.Drawing.Size(96, 25);
            this.chkUnselectAll.TabIndex = 2;
            this.chkUnselectAll.Text = "Unselect All";
            this.chkUnselectAll.CheckedChanged += new System.EventHandler(this.chkUnselectAll_CheckedChanged);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(98, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(320, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Dock = Wisej.Web.DockStyle.Left;
            this.chkSelectAll.Location = new System.Drawing.Point(15, 5);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(83, 25);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlParams);
            this.pnlTop.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(413, 128);
            this.pnlTop.TabIndex = 1;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.panel2);
            this.pnlParams.Controls.Add(this.pnlProcess);
            this.pnlParams.Controls.Add(this.pnlProgram);
            this.pnlParams.Controls.Add(this.pnlUser);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(413, 128);
            this.pnlParams.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbChartType);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(413, 35);
            this.panel2.TabIndex = 3;
            // 
            // cmbChartType
            // 
            this.cmbChartType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbChartType.FormattingEnabled = true;
            this.cmbChartType.Location = new System.Drawing.Point(91, 4);
            this.cmbChartType.Name = "cmbChartType";
            this.cmbChartType.Size = new System.Drawing.Size(115, 25);
            this.cmbChartType.TabIndex = 1;
            // 
            // pnlProcess
            // 
            this.pnlProcess.AppearanceKey = "panel-grdo";
            this.pnlProcess.Controls.Add(this.btnGeneraterpt);
            this.pnlProcess.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlProcess.Location = new System.Drawing.Point(0, 97);
            this.pnlProcess.Name = "pnlProcess";
            this.pnlProcess.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlProcess.Size = new System.Drawing.Size(413, 31);
            this.pnlProcess.TabIndex = 4;
            // 
            // btnGeneraterpt
            // 
            this.btnGeneraterpt.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneraterpt.Location = new System.Drawing.Point(320, 5);
            this.btnGeneraterpt.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnGeneraterpt.Name = "btnGeneraterpt";
            this.btnGeneraterpt.Size = new System.Drawing.Size(78, 25);
            this.btnGeneraterpt.TabIndex = 2;
            this.btnGeneraterpt.Text = "&Process";
            this.btnGeneraterpt.Click += new System.EventHandler(this.btnGeneraterpt_Click);
            // 
            // pnlProgram
            // 
            this.pnlProgram.Controls.Add(this.rdoPSelect);
            this.pnlProgram.Controls.Add(this.rdoPAll);
            this.pnlProgram.Controls.Add(this.lblProgram);
            this.pnlProgram.Dock = Wisej.Web.DockStyle.Top;
            this.pnlProgram.Location = new System.Drawing.Point(0, 31);
            this.pnlProgram.Name = "pnlProgram";
            this.pnlProgram.Size = new System.Drawing.Size(413, 31);
            this.pnlProgram.TabIndex = 0;
            // 
            // rdoPSelect
            // 
            this.rdoPSelect.Location = new System.Drawing.Point(226, 4);
            this.rdoPSelect.Name = "rdoPSelect";
            this.rdoPSelect.Size = new System.Drawing.Size(66, 21);
            this.rdoPSelect.TabIndex = 2;
            this.rdoPSelect.Text = "Select";
            this.rdoPSelect.Click += new System.EventHandler(this.rdoPSelect_Click);
            // 
            // rdoPAll
            // 
            this.rdoPAll.Checked = true;
            this.rdoPAll.Location = new System.Drawing.Point(151, 4);
            this.rdoPAll.Name = "rdoPAll";
            this.rdoPAll.Size = new System.Drawing.Size(46, 21);
            this.rdoPAll.TabIndex = 1;
            this.rdoPAll.TabStop = true;
            this.rdoPAll.Text = "All";
            this.rdoPAll.Click += new System.EventHandler(this.rdoPSelect_Click);
            // 
            // lblProgram
            // 
            this.lblProgram.AutoSize = true;
            this.lblProgram.Location = new System.Drawing.Point(94, 7);
            this.lblProgram.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(51, 16);
            this.lblProgram.TabIndex = 0;
            this.lblProgram.Text = "Program";
            // 
            // pnlUser
            // 
            this.pnlUser.Controls.Add(this.rdoUSelect);
            this.pnlUser.Controls.Add(this.rdoUAll);
            this.pnlUser.Controls.Add(this.lblUsers);
            this.pnlUser.Dock = Wisej.Web.DockStyle.Top;
            this.pnlUser.Location = new System.Drawing.Point(0, 0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(413, 31);
            this.pnlUser.TabIndex = 1;
            // 
            // rdoUSelect
            // 
            this.rdoUSelect.Location = new System.Drawing.Point(226, 6);
            this.rdoUSelect.Name = "rdoUSelect";
            this.rdoUSelect.Size = new System.Drawing.Size(66, 21);
            this.rdoUSelect.TabIndex = 2;
            this.rdoUSelect.Text = "Select";
            this.rdoUSelect.Click += new System.EventHandler(this.rdoUSelect_Click);
            // 
            // rdoUAll
            // 
            this.rdoUAll.Checked = true;
            this.rdoUAll.Location = new System.Drawing.Point(151, 6);
            this.rdoUAll.Name = "rdoUAll";
            this.rdoUAll.Size = new System.Drawing.Size(46, 21);
            this.rdoUAll.TabIndex = 1;
            this.rdoUAll.TabStop = true;
            this.rdoUAll.Text = "All";
            this.rdoUAll.Click += new System.EventHandler(this.rdoUSelect_Click);
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Location = new System.Drawing.Point(94, 9);
            this.lblUsers.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(30, 16);
            this.lblUsers.TabIndex = 0;
            this.lblUsers.Text = "User";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlBottom);
            this.pnlMain.Controls.Add(this.pnlTop);
            this.pnlMain.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(413, 413);
            this.pnlMain.TabIndex = 0;
            // 
            // RdlcUserForm
            // 
            this.ClientSize = new System.Drawing.Size(413, 413);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RdlcUserForm";
            this.Text = "RdlcUserForm";
            ((System.ComponentModel.ISupportInitialize)(this.gvwData)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlgvwData.ResumeLayout(false);
            this.pnlOK.ResumeLayout(false);
            this.pnlOK.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlProcess.ResumeLayout(false);
            this.pnlProgram.ResumeLayout(false);
            this.pnlProgram.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvwData;
        private DataGridViewImageColumn gvISelect;
        private DataGridViewTextBoxColumn gvtUserId;
        private Button btnOk;
        private Button btnCancel;
        private DataGridViewTextBoxColumn gvtSelect;
        private DataGridViewTextBoxColumn gvtPCode;
        private Panel pnlBottom;
        private Panel pnlTop;
        private Button btnGeneraterpt;
        private Panel pnlProgram;
        private RadioButton rdoPSelect;
        private RadioButton rdoPAll;
        private Label lblProgram;
        private Panel pnlUser;
        private RadioButton rdoUSelect;
        private RadioButton rdoUAll;
        private Label lblUsers;
        private ComboBox cmbChartType;
        private CheckBox chkSelectAll;
        private CheckBox chkUnselectAll;
        private Panel pnlMain;
        private Panel pnlgvwData;
        private Panel pnlOK;
        private Spacer spacer1;
        private Spacer spacer2;
        private Panel pnlParams;
        private Panel panel2;
        private Panel pnlProcess;
    }
}