using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class Admn0004UserForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Admn0004UserForm));
            this.pblblock1 = new Wisej.Web.Panel();
            this.btnViewUsers = new Wisej.Web.Button();
            this.label1 = new Wisej.Web.Label();
            this.txtPassword = new Wisej.Web.TextBox();
            this.panel2 = new Wisej.Web.Panel();
            this.gvwCustomer = new Wisej.Web.DataGridView();
            this.UserName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Password = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Column0 = new Wisej.Web.DataGridViewColumn();
            this.Column1 = new Wisej.Web.DataGridViewColumn();
            this.btnExit = new Wisej.Web.Button();
            this.btnLanghApplication = new Wisej.Web.Button();
            this.panel3 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlsearch = new Wisej.Web.Panel();
            this.txtSearch = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.spacer2 = new Wisej.Web.Spacer();
            this.chkuserStatus = new Wisej.Web.CheckBox();
            this.pblblock1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomer)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlsearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pblblock1
            // 
            this.pblblock1.Controls.Add(this.btnViewUsers);
            this.pblblock1.Controls.Add(this.label1);
            this.pblblock1.Controls.Add(this.txtPassword);
            this.pblblock1.Dock = Wisej.Web.DockStyle.Top;
            this.pblblock1.Location = new System.Drawing.Point(0, 0);
            this.pblblock1.Name = "pblblock1";
            this.pblblock1.Size = new System.Drawing.Size(649, 38);
            this.pblblock1.TabIndex = 0;
            // 
            // btnViewUsers
            // 
            this.btnViewUsers.Location = new System.Drawing.Point(236, 8);
            this.btnViewUsers.Name = "btnViewUsers";
            this.btnViewUsers.Size = new System.Drawing.Size(75, 23);
            this.btnViewUsers.TabIndex = 2;
            this.btnViewUsers.Text = "&View Users";
            this.btnViewUsers.Click += new System.EventHandler(this.btnViewUsers_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter Password";
            // 
            // txtPassword
            // 
            this.txtPassword.InputType.Type = Wisej.Web.TextBoxType.Password;
            this.txtPassword.Location = new System.Drawing.Point(106, 6);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(122, 25);
            this.txtPassword.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwCustomer);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 73);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(649, 233);
            this.panel2.TabIndex = 1;
            // 
            // gvwCustomer
            // 
            this.gvwCustomer.AllowUserToResizeColumns = false;
            this.gvwCustomer.AllowUserToResizeRows = false;
            this.gvwCustomer.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(122, 122, 122);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwCustomer.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwCustomer.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.UserName,
            this.Password,
            this.Column0,
            this.Column1});
            this.gvwCustomer.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCustomer.Location = new System.Drawing.Point(0, 0);
            this.gvwCustomer.Name = "gvwCustomer";
            this.gvwCustomer.RowHeadersWidth = 14;
            this.gvwCustomer.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCustomer.ShowColumnVisibilityMenu = false;
            this.gvwCustomer.Size = new System.Drawing.Size(649, 233);
            this.gvwCustomer.TabIndex = 0;
            // 
            // UserName
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            this.UserName.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.UserName.HeaderStyle = dataGridViewCellStyle3;
            this.UserName.HeaderText = "User ID";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            this.UserName.Width = 160;
            // 
            // Password
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            this.Password.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Password.HeaderStyle = dataGridViewCellStyle5;
            this.Password.HeaderText = "Password";
            this.Password.Name = "Password";
            this.Password.ReadOnly = true;
            this.Password.Width = 150;
            // 
            // Column0
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.HeaderStyle = dataGridViewCellStyle7;
            this.Column0.HeaderText = "First Name";
            this.Column0.Name = "Column0";
            this.Column0.Width = 150;
            // 
            // Column1
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.HeaderStyle = dataGridViewCellStyle9;
            this.Column1.HeaderText = "Last Name";
            this.Column1.Name = "Column1";
            this.Column1.Width = 150;
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-error";
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(581, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(53, 25);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLanghApplication
            // 
            this.btnLanghApplication.Dock = Wisej.Web.DockStyle.Right;
            this.btnLanghApplication.Location = new System.Drawing.Point(440, 5);
            this.btnLanghApplication.Name = "btnLanghApplication";
            this.btnLanghApplication.Size = new System.Drawing.Size(136, 25);
            this.btnLanghApplication.TabIndex = 3;
            this.btnLanghApplication.Text = "&Launch Application";
            this.btnLanghApplication.Visible = false;
            this.btnLanghApplication.Click += new System.EventHandler(this.btnLanghApplication_Click);
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.btnLanghApplication);
            this.panel3.Controls.Add(this.spacer1);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 306);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel3.Size = new System.Drawing.Size(649, 35);
            this.panel3.TabIndex = 5;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(576, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlsearch);
            this.panel1.Controls.Add(this.chkuserStatus);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(4);
            this.panel1.Size = new System.Drawing.Size(649, 35);
            this.panel1.TabIndex = 6;
            // 
            // pnlsearch
            // 
            this.pnlsearch.Controls.Add(this.txtSearch);
            this.pnlsearch.Controls.Add(this.label2);
            this.pnlsearch.Controls.Add(this.spacer2);
            this.pnlsearch.Dock = Wisej.Web.DockStyle.Left;
            this.pnlsearch.Location = new System.Drawing.Point(146, 4);
            this.pnlsearch.Name = "pnlsearch";
            this.pnlsearch.Size = new System.Drawing.Size(504, 27);
            this.pnlsearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = Wisej.Web.DockStyle.Left;
            this.txtSearch.Location = new System.Drawing.Point(228, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(162, 27);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyDown += new Wisej.Web.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = Wisej.Web.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(177, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new Wisej.Web.Padding(0, 0, 8, 0);
            this.label2.Size = new System.Drawing.Size(51, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "Search ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(177, 27);
            // 
            // chkuserStatus
            // 
            this.chkuserStatus.Dock = Wisej.Web.DockStyle.Left;
            this.chkuserStatus.Location = new System.Drawing.Point(4, 4);
            this.chkuserStatus.Name = "chkuserStatus";
            this.chkuserStatus.Size = new System.Drawing.Size(142, 27);
            this.chkuserStatus.TabIndex = 0;
            this.chkuserStatus.Text = "Show Inactive Users";
            this.chkuserStatus.CheckedChanged += new System.EventHandler(this.chkuserStatus_CheckedChanged);
            // 
            // Admn0004UserForm
            // 
            this.ClientSize = new System.Drawing.Size(649, 341);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pblblock1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Admn0004UserForm";
            this.Text = "Users List";
            this.pblblock1.ResumeLayout(false);
            this.pblblock1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCustomer)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlsearch.ResumeLayout(false);
            this.pnlsearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pblblock1;
        private Button btnViewUsers;
        private Label label1;
        private TextBox txtPassword;
        private Panel panel2;
        private DataGridView gvwCustomer;
        private DataGridViewTextBoxColumn UserName;
        private DataGridViewTextBoxColumn Password;
        private Button btnExit;
        private Button btnLanghApplication;
        private Panel panel3;
        private Spacer spacer1;
        private Panel panel1;
        private CheckBox chkuserStatus;
        private DataGridViewColumn FirstName;
        private DataGridViewColumn LastName;
        private Panel pnlsearch;
        private TextBox txtSearch;
        private Spacer spacer2;
        private DataGridViewColumn Column0;
        private DataGridViewColumn Column1;
        private Label label2;
    }
}