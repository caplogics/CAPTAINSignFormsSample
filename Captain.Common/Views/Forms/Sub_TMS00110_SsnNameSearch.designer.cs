using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class Sub_TMS00110_SsnNameSearch
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

        #region Wisej web Form Designer generated code

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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sub_TMS00110_SsnNameSearch));
            this.panel1 = new Wisej.Web.Panel();
            this.SearchGrid = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Site = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Time = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Slot = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDob = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Phone = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Status = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ApptKey = new Wisej.Web.DataGridViewTextBoxColumn();
            this.StatusCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cmbMonth = new Wisej.Web.ComboBox();
            this.lblMonths = new Wisej.Web.Label();
            this.BtnSelect = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.panel5 = new Wisej.Web.Panel();
            this.panel6 = new Wisej.Web.Panel();
            this.Phone_Panel = new Wisej.Web.Panel();
            this.Txt_Phone = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.button2 = new Wisej.Web.Button();
            this.SsnPanel = new Wisej.Web.Panel();
            this.dtBirth = new Wisej.Web.DateTimePicker();
            this.button3 = new Wisej.Web.Button();
            this.NamePanel = new Wisej.Web.Panel();
            this.button1 = new Wisej.Web.Button();
            this.TxtFirstName = new Wisej.Web.TextBox();
            this.TxtLastName = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.panel4 = new Wisej.Web.Panel();
            this.Cb_All_Sites = new Wisej.Web.CheckBox();
            this.CmbSearchBy = new Wisej.Web.ComboBox();
            this.label1 = new Wisej.Web.Label();
            this.panel3 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SearchGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.Phone_Panel.SuspendLayout();
            this.SsnPanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SearchGrid);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 114);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 388);
            this.panel1.TabIndex = 0;
            // 
            // SearchGrid
            // 
            this.SearchGrid.AllowUserToResizeColumns = false;
            this.SearchGrid.AllowUserToResizeRows = false;
            this.SearchGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.SearchGrid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.SearchGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.SearchGrid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SearchGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Site,
            this.gvDate,
            this.Time,
            this.Slot,
            this.gvtDob,
            this.Name,
            this.Phone,
            this.Status,
            this.ApptKey,
            this.StatusCode});
            this.SearchGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.SearchGrid.Name = "SearchGrid";
            this.SearchGrid.RowHeadersWidth = 15;
            this.SearchGrid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.SearchGrid.Size = new System.Drawing.Size(799, 388);
            this.SearchGrid.TabIndex = 0;
            this.SearchGrid.TabStop = false;
            this.SearchGrid.SelectionChanged += new System.EventHandler(this.SearchGrid_SelectionChanged);
            // 
            // Site
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Site.HeaderStyle = dataGridViewCellStyle2;
            this.Site.HeaderText = "Site";
            this.Site.Name = "Site";
            this.Site.ReadOnly = true;
            this.Site.Width = 50;
            // 
            // gvDate
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvDate.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvDate.HeaderStyle = dataGridViewCellStyle4;
            this.gvDate.HeaderText = "Date";
            this.gvDate.Name = "gvDate";
            this.gvDate.ReadOnly = true;
            // 
            // Time
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Time.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Time.HeaderStyle = dataGridViewCellStyle6;
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Width = 70;
            // 
            // Slot
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.Slot.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Slot.HeaderStyle = dataGridViewCellStyle8;
            this.Slot.HeaderText = "Slot";
            this.Slot.Name = "Slot";
            this.Slot.ReadOnly = true;
            this.Slot.Width = 50;
            // 
            // gvtDob
            // 
            dataGridViewCellStyle9.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvtDob.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvtDob.HeaderStyle = dataGridViewCellStyle10;
            this.gvtDob.HeaderText = "DOB";
            this.gvtDob.Name = "gvtDob";
            this.gvtDob.ReadOnly = true;
            // 
            // Name
            // 
            dataGridViewCellStyle11.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Name.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Name.HeaderStyle = dataGridViewCellStyle12;
            this.Name.HeaderText = "Name";
            this.Name.Name = "Name";
            this.Name.ReadOnly = true;
            this.Name.Width = 180;
            // 
            // Phone
            // 
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Phone.HeaderStyle = dataGridViewCellStyle13;
            this.Phone.HeaderText = "Phone";
            this.Phone.Name = "Phone";
            this.Phone.ReadOnly = true;
            this.Phone.Width = 80;
            // 
            // Status
            // 
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Status.HeaderStyle = dataGridViewCellStyle14;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 135;
            // 
            // ApptKey
            // 
            this.ApptKey.HeaderText = "ApptKey";
            this.ApptKey.Name = "ApptKey";
            this.ApptKey.ShowInVisibilityMenu = false;
            this.ApptKey.Visible = false;
            // 
            // StatusCode
            // 
            this.StatusCode.HeaderText = "StatusCode";
            this.StatusCode.Name = "StatusCode";
            this.StatusCode.ShowInVisibilityMenu = false;
            this.StatusCode.Visible = false;
            this.StatusCode.Width = 20;
            // 
            // cmbMonth
            // 
            this.cmbMonth.Dock = Wisej.Web.DockStyle.Right;
            this.cmbMonth.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(627, 5);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(62, 25);
            this.cmbMonth.TabIndex = 1;
            this.cmbMonth.Visible = false;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // lblMonths
            // 
            this.lblMonths.Dock = Wisej.Web.DockStyle.Right;
            this.lblMonths.Location = new System.Drawing.Point(475, 5);
            this.lblMonths.Name = "lblMonths";
            this.lblMonths.Size = new System.Drawing.Size(152, 25);
            this.lblMonths.TabIndex = 4;
            this.lblMonths.Text = "Move to No. of Months";
            this.lblMonths.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMonths.Visible = false;
            // 
            // BtnSelect
            // 
            this.BtnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.BtnSelect.Location = new System.Drawing.Point(709, 5);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(75, 25);
            this.BtnSelect.TabIndex = 2;
            this.BtnSelect.Text = "S&elect";
            this.BtnSelect.Visible = false;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(799, 114);
            this.panel2.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.NamePanel);
            this.panel5.Dock = Wisej.Web.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(212, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(587, 114);
            this.panel5.TabIndex = 7;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.Phone_Panel);
            this.panel6.Controls.Add(this.SsnPanel);
            this.panel6.Dock = Wisej.Web.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 75);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(587, 39);
            this.panel6.TabIndex = 4;
            // 
            // Phone_Panel
            // 
            this.Phone_Panel.Controls.Add(this.Txt_Phone);
            this.Phone_Panel.Controls.Add(this.button2);
            this.Phone_Panel.Dock = Wisej.Web.DockStyle.Fill;
            this.Phone_Panel.Location = new System.Drawing.Point(234, 0);
            this.Phone_Panel.Name = "Phone_Panel";
            this.Phone_Panel.Size = new System.Drawing.Size(353, 39);
            this.Phone_Panel.TabIndex = 4;
            this.Phone_Panel.Visible = false;
            // 
            // Txt_Phone
            // 
            this.Txt_Phone.Location = new System.Drawing.Point(3, 11);
            this.Txt_Phone.MaxLength = 10;
            this.Txt_Phone.Name = "Txt_Phone";
            this.Txt_Phone.Size = new System.Drawing.Size(85, 25);
            this.Txt_Phone.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 25);
            this.button2.TabIndex = 2;
            this.button2.Text = "&Search";
            this.button2.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SsnPanel
            // 
            this.SsnPanel.Controls.Add(this.dtBirth);
            this.SsnPanel.Controls.Add(this.button3);
            this.SsnPanel.Dock = Wisej.Web.DockStyle.Left;
            this.SsnPanel.Location = new System.Drawing.Point(0, 0);
            this.SsnPanel.Name = "SsnPanel";
            this.SsnPanel.Size = new System.Drawing.Size(234, 39);
            this.SsnPanel.TabIndex = 3;
            this.SsnPanel.Visible = false;
            // 
            // dtBirth
            // 
            this.dtBirth.AutoSize = false;
            this.dtBirth.Checked = false;
            this.dtBirth.CustomFormat = "MM/dd/yyyy";
            this.dtBirth.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtBirth.Location = new System.Drawing.Point(3, 11);
            this.dtBirth.Name = "dtBirth";
            this.dtBirth.ShowCheckBox = true;
            this.dtBirth.ShowToolTips = false;
            this.dtBirth.Size = new System.Drawing.Size(116, 25);
            this.dtBirth.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(122, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 25);
            this.button3.TabIndex = 2;
            this.button3.Text = "&Search";
            this.button3.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.button1);
            this.NamePanel.Controls.Add(this.TxtFirstName);
            this.NamePanel.Controls.Add(this.TxtLastName);
            this.NamePanel.Controls.Add(this.label2);
            this.NamePanel.Controls.Add(this.label3);
            this.NamePanel.Dock = Wisej.Web.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(0, 0);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(587, 75);
            this.NamePanel.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(371, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Search";
            this.button1.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // TxtFirstName
            // 
            this.TxtFirstName.Location = new System.Drawing.Point(76, 42);
            this.TxtFirstName.MaxLength = 30;
            this.TxtFirstName.Name = "TxtFirstName";
            this.TxtFirstName.Size = new System.Drawing.Size(289, 25);
            this.TxtFirstName.TabIndex = 2;
            // 
            // TxtLastName
            // 
            this.TxtLastName.Location = new System.Drawing.Point(76, 11);
            this.TxtLastName.MaxLength = 40;
            this.TxtLastName.Name = "TxtLastName";
            this.TxtLastName.Size = new System.Drawing.Size(289, 25);
            this.TxtLastName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "First Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "Last Name";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Cb_All_Sites);
            this.panel4.Controls.Add(this.CmbSearchBy);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = Wisej.Web.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(212, 114);
            this.panel4.TabIndex = 1;
            // 
            // Cb_All_Sites
            // 
            this.Cb_All_Sites.Location = new System.Drawing.Point(81, 42);
            this.Cb_All_Sites.Name = "Cb_All_Sites";
            this.Cb_All_Sites.Size = new System.Drawing.Size(117, 21);
            this.Cb_All_Sites.TabIndex = 2;
            this.Cb_All_Sites.Text = "Search All Sites";
            // 
            // CmbSearchBy
            // 
            this.CmbSearchBy.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbSearchBy.FormattingEnabled = true;
            this.CmbSearchBy.Location = new System.Drawing.Point(85, 11);
            this.CmbSearchBy.Name = "CmbSearchBy";
            this.CmbSearchBy.Size = new System.Drawing.Size(108, 25);
            this.CmbSearchBy.TabIndex = 1;
            this.CmbSearchBy.SelectedIndexChanged += new System.EventHandler(this.CmbSearchBy_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.MaximumSize = new System.Drawing.Size(0, 21);
            this.label1.MinimumSize = new System.Drawing.Size(0, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search By";
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.lblMonths);
            this.panel3.Controls.Add(this.cmbMonth);
            this.panel3.Controls.Add(this.spacer1);
            this.panel3.Controls.Add(this.BtnSelect);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 502);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel3.Size = new System.Drawing.Size(799, 35);
            this.panel3.TabIndex = 5;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(689, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(20, 25);
            // 
            // Sub_TMS00110_SsnNameSearch
            // 
            this.ClientSize = new System.Drawing.Size(799, 537);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.Name = "Sub_TMS00110_SsnNameSearch";
            this.Text = "Ssn Name Search";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SearchGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.Phone_Panel.ResumeLayout(false);
            this.Phone_Panel.PerformLayout();
            this.SsnPanel.ResumeLayout(false);
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel NamePanel;
        private Button button1;
        private TextBox TxtFirstName;
        private TextBox TxtLastName;
        private Label label2;
        private Label label3;
        private Label label1;
        private ComboBox CmbSearchBy;
        private Button BtnSelect;
        private DataGridViewEx SearchGrid;
        private Panel SsnPanel;
        private Button button3;
        private DataGridViewTextBoxColumn Site;
        private DataGridViewTextBoxColumn Slot;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn ApptKey;
        private CheckBox Cb_All_Sites;
        private DataGridViewTextBoxColumn Phone;
        private Panel Phone_Panel;
        private TextBoxWithValidation Txt_Phone;
        private Button button2;
        private DateTimePicker dtBirth;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn StatusCode;
        private ComboBox cmbMonth;
        private Label lblMonths;
        private Panel panel3;
        private Spacer spacer1;
        private DataGridViewDateTimeColumn gvDate;
        private DataGridViewDateTimeColumn gvtDob;
        private Panel panel5;
        private Panel panel6;
        private Panel panel4;
        private DataGridViewTextBoxColumn Time;
    }
}