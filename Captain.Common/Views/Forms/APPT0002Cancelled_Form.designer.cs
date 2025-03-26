using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class APPT0002Cancelled_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APPT0002Cancelled_Form));
            this.LblReq = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.Btn_Update = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.BtnCancel = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.btnHist = new Wisej.Web.Button();
            this.spacer7 = new Wisej.Web.Spacer();
            this.BtnNextSlot = new Wisej.Web.Button();
            this.spacer6 = new Wisej.Web.Spacer();
            this.BtnMemSearch = new Wisej.Web.Button();
            this.spacer5 = new Wisej.Web.Spacer();
            this.BtnRefSch = new Wisej.Web.Button();
            this.spacer4 = new Wisej.Web.Spacer();
            this.AppDate = new Wisej.Web.DateTimePicker();
            this.label2 = new Wisej.Web.Label();
            this.LblDate = new Wisej.Web.Label();
            this.spacer3 = new Wisej.Web.Spacer();
            this.BtnSearch = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.TxtSiteCode = new Wisej.Web.TextBox();
            this.label1 = new Wisej.Web.Label();
            this.LblSite = new Wisej.Web.Label();
            this.PnlGrid = new Wisej.Web.Panel();
            this.SchAppGrid = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Delete = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Time = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Telephone = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvStatus = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Person = new Wisej.Web.DataGridViewTextBoxColumn();
            this.DOB = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.SlotNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.KeyTime = new Wisej.Web.DataGridViewTextBoxColumn();
            this.AppStatus = new Wisej.Web.DataGridViewTextBoxColumn();
            this.KeySsn = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Rec_Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvt_TempId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.PnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SchAppGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // LblReq
            // 
            this.LblReq.AutoSize = true;
            this.LblReq.ForeColor = System.Drawing.Color.Red;
            this.LblReq.Location = new System.Drawing.Point(187, 2);
            this.LblReq.Name = "LblReq";
            this.LblReq.Size = new System.Drawing.Size(10, 10);
            this.LblReq.TabIndex = 7;
            this.LblReq.Text = "*";
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.Btn_Update);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 286);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(914, 35);
            this.panel1.TabIndex = 1;
            // 
            // Btn_Update
            // 
            this.Btn_Update.AppearanceKey = "button-ok";
            this.Btn_Update.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Update.Location = new System.Drawing.Point(744, 5);
            this.Btn_Update.Name = "Btn_Update";
            this.Btn_Update.Size = new System.Drawing.Size(75, 25);
            this.Btn_Update.TabIndex = 29;
            this.Btn_Update.Text = "&Save";
            this.Btn_Update.Visible = false;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(819, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // BtnCancel
            // 
            this.BtnCancel.AppearanceKey = "button-cancel";
            this.BtnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.BtnCancel.Location = new System.Drawing.Point(824, 5);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 25);
            this.BtnCancel.TabIndex = 30;
            this.BtnCancel.Text = "&Close";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnHist);
            this.panel2.Controls.Add(this.spacer7);
            this.panel2.Controls.Add(this.BtnNextSlot);
            this.panel2.Controls.Add(this.spacer6);
            this.panel2.Controls.Add(this.BtnMemSearch);
            this.panel2.Controls.Add(this.spacer5);
            this.panel2.Controls.Add(this.BtnRefSch);
            this.panel2.Controls.Add(this.spacer4);
            this.panel2.Controls.Add(this.AppDate);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.LblDate);
            this.panel2.Controls.Add(this.spacer3);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Controls.Add(this.spacer2);
            this.panel2.Controls.Add(this.TxtSiteCode);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.LblSite);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(15, 5, 5, 5);
            this.panel2.Size = new System.Drawing.Size(914, 35);
            this.panel2.TabIndex = 4;
            // 
            // btnHist
            // 
            this.btnHist.Dock = Wisej.Web.DockStyle.Left;
            this.btnHist.Location = new System.Drawing.Point(767, 5);
            this.btnHist.Name = "btnHist";
            this.btnHist.Size = new System.Drawing.Size(124, 25);
            this.btnHist.TabIndex = 6;
            this.btnHist.Text = "&View Cancelled";
            this.btnHist.Visible = false;
            this.btnHist.Click += new System.EventHandler(this.btnHist_Click);
            // 
            // spacer7
            // 
            this.spacer7.Dock = Wisej.Web.DockStyle.Left;
            this.spacer7.Location = new System.Drawing.Point(762, 5);
            this.spacer7.Name = "spacer7";
            this.spacer7.Size = new System.Drawing.Size(5, 25);
            // 
            // BtnNextSlot
            // 
            this.BtnNextSlot.Dock = Wisej.Web.DockStyle.Left;
            this.BtnNextSlot.Location = new System.Drawing.Point(640, 5);
            this.BtnNextSlot.Name = "BtnNextSlot";
            this.BtnNextSlot.Size = new System.Drawing.Size(122, 25);
            this.BtnNextSlot.TabIndex = 5;
            this.BtnNextSlot.Text = "Next &Available Slot";
            this.BtnNextSlot.Visible = false;
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Left;
            this.spacer6.Location = new System.Drawing.Point(635, 5);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(5, 25);
            // 
            // BtnMemSearch
            // 
            this.BtnMemSearch.Dock = Wisej.Web.DockStyle.Left;
            this.BtnMemSearch.Location = new System.Drawing.Point(489, 5);
            this.BtnMemSearch.Name = "BtnMemSearch";
            this.BtnMemSearch.Size = new System.Drawing.Size(146, 25);
            this.BtnMemSearch.TabIndex = 6;
            this.BtnMemSearch.Text = "Search by Name/&DOB";
            this.BtnMemSearch.Visible = false;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(464, 5);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(25, 25);
            // 
            // BtnRefSch
            // 
            this.BtnRefSch.Dock = Wisej.Web.DockStyle.Left;
            this.BtnRefSch.Location = new System.Drawing.Point(347, 5);
            this.BtnRefSch.Name = "BtnRefSch";
            this.BtnRefSch.Size = new System.Drawing.Size(117, 25);
            this.BtnRefSch.TabIndex = 4;
            this.BtnRefSch.Text = "&Refresh Schedule";
            this.BtnRefSch.Visible = false;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(322, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(25, 25);
            // 
            // AppDate
            // 
            this.AppDate.Dock = Wisej.Web.DockStyle.Left;
            this.AppDate.Enabled = false;
            this.AppDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.AppDate.Location = new System.Drawing.Point(226, 5);
            this.AppDate.Name = "AppDate";
            this.AppDate.ShowToolTips = false;
            this.AppDate.Size = new System.Drawing.Size(96, 25);
            this.AppDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Dock = Wisej.Web.DockStyle.Left;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(216, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Visible = false;
            // 
            // LblDate
            // 
            this.LblDate.AutoSize = true;
            this.LblDate.Dock = Wisej.Web.DockStyle.Left;
            this.LblDate.Location = new System.Drawing.Point(186, 5);
            this.LblDate.Name = "LblDate";
            this.LblDate.Size = new System.Drawing.Size(30, 25);
            this.LblDate.TabIndex = 3;
            this.LblDate.Text = "Date";
            this.LblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(170, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(16, 25);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Dock = Wisej.Web.DockStyle.Left;
            this.BtnSearch.Enabled = false;
            this.BtnSearch.Location = new System.Drawing.Point(104, 5);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(66, 25);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "S&earch";
            this.BtnSearch.Visible = false;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(99, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 25);
            // 
            // TxtSiteCode
            // 
            this.TxtSiteCode.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.TxtSiteCode.Dock = Wisej.Web.DockStyle.Left;
            this.TxtSiteCode.Enabled = false;
            this.TxtSiteCode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TxtSiteCode.Location = new System.Drawing.Point(49, 5);
            this.TxtSiteCode.MaxLength = 4;
            this.TxtSiteCode.Name = "TxtSiteCode";
            this.TxtSiteCode.Size = new System.Drawing.Size(50, 25);
            this.TxtSiteCode.TabIndex = 1;
            this.TxtSiteCode.LostFocus += new System.EventHandler(this.TxtSiteCode_LostFocus);
            // 
            // label1
            // 
            this.label1.Dock = Wisej.Web.DockStyle.Left;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(40, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblSite
            // 
            this.LblSite.AutoSize = true;
            this.LblSite.Dock = Wisej.Web.DockStyle.Left;
            this.LblSite.Location = new System.Drawing.Point(15, 5);
            this.LblSite.Name = "LblSite";
            this.LblSite.Size = new System.Drawing.Size(25, 25);
            this.LblSite.TabIndex = 3;
            this.LblSite.Text = "Site";
            this.LblSite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlGrid
            // 
            this.PnlGrid.Controls.Add(this.SchAppGrid);
            this.PnlGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.PnlGrid.Location = new System.Drawing.Point(0, 35);
            this.PnlGrid.Name = "PnlGrid";
            this.PnlGrid.Size = new System.Drawing.Size(914, 251);
            this.PnlGrid.TabIndex = 5;
            // 
            // SchAppGrid
            // 
            this.SchAppGrid.AllowUserToResizeColumns = false;
            this.SchAppGrid.AllowUserToResizeRows = false;
            this.SchAppGrid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.SchAppGrid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.SchAppGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.SchAppGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Delete,
            this.Time,
            this.Name,
            this.Telephone,
            this.gvStatus,
            this.Person,
            this.DOB,
            this.SlotNo,
            this.KeyTime,
            this.AppStatus,
            this.KeySsn,
            this.Rec_Type,
            this.gvt_TempId});
            this.SchAppGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.SchAppGrid.MultiSelect = false;
            this.SchAppGrid.Name = "SchAppGrid";
            this.SchAppGrid.RowHeadersWidth = 25;
            this.SchAppGrid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.SchAppGrid.Size = new System.Drawing.Size(914, 251);
            this.SchAppGrid.TabIndex = 0;
            this.SchAppGrid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.SchAppGrid_CellClick);
            // 
            // Delete
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.Delete.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Delete.HeaderStyle = dataGridViewCellStyle3;
            this.Delete.HeaderText = "Del";
            this.Delete.Name = "Delete";
            this.Delete.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Delete.ShowInVisibilityMenu = false;
            this.Delete.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Delete.Visible = false;
            this.Delete.Width = 40;
            // 
            // Time
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Time.HeaderStyle = dataGridViewCellStyle4;
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Width = 60;
            // 
            // Name
            // 
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Name.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Name.HeaderStyle = dataGridViewCellStyle6;
            this.Name.HeaderText = "Name";
            this.Name.Name = "Name";
            this.Name.ReadOnly = true;
            this.Name.Width = 180;
            // 
            // Telephone
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Telephone.HeaderStyle = dataGridViewCellStyle7;
            this.Telephone.HeaderText = "Telephone";
            this.Telephone.Name = "Telephone";
            this.Telephone.ReadOnly = true;
            this.Telephone.Width = 95;
            // 
            // gvStatus
            // 
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvStatus.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvStatus.HeaderStyle = dataGridViewCellStyle9;
            this.gvStatus.HeaderText = "Status";
            this.gvStatus.Name = "gvStatus";
            this.gvStatus.Width = 200;
            // 
            // Person
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Person.HeaderStyle = dataGridViewCellStyle10;
            this.Person.HeaderText = "Cancelled By";
            this.Person.Name = "Person";
            this.Person.ReadOnly = true;
            this.Person.Width = 150;
            // 
            // DOB
            // 
            this.DOB.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.DOB.HeaderStyle = dataGridViewCellStyle12;
            this.DOB.HeaderText = "Cancelled Date";
            this.DOB.Name = "DOB";
            this.DOB.ReadOnly = true;
            this.DOB.Width = 120;
            // 
            // SlotNo
            // 
            this.SlotNo.HeaderText = "SlotNo";
            this.SlotNo.Name = "SlotNo";
            this.SlotNo.ReadOnly = true;
            this.SlotNo.ShowInVisibilityMenu = false;
            this.SlotNo.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.SlotNo.Visible = false;
            // 
            // KeyTime
            // 
            this.KeyTime.HeaderText = "KeyTime";
            this.KeyTime.Name = "KeyTime";
            this.KeyTime.ShowInVisibilityMenu = false;
            this.KeyTime.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.KeyTime.Visible = false;
            // 
            // AppStatus
            // 
            this.AppStatus.HeaderText = "AppStatus";
            this.AppStatus.Name = "AppStatus";
            this.AppStatus.ShowInVisibilityMenu = false;
            this.AppStatus.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.AppStatus.Visible = false;
            // 
            // KeySsn
            // 
            this.KeySsn.HeaderText = "KeySsn";
            this.KeySsn.Name = "KeySsn";
            this.KeySsn.ShowInVisibilityMenu = false;
            this.KeySsn.Visible = false;
            // 
            // Rec_Type
            // 
            this.Rec_Type.HeaderText = "Rec_Type";
            this.Rec_Type.Name = "Rec_Type";
            this.Rec_Type.ShowInVisibilityMenu = false;
            this.Rec_Type.Visible = false;
            // 
            // gvt_TempId
            // 
            this.gvt_TempId.HeaderText = "gvt_TempId";
            this.gvt_TempId.Name = "gvt_TempId";
            this.gvt_TempId.ShowInVisibilityMenu = false;
            this.gvt_TempId.Visible = false;
            // 
            // APPT0002Cancelled_Form
            // 
            this.ClientSize = new System.Drawing.Size(914, 321);
            this.Controls.Add(this.PnlGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.Name = "APPT0002Cancelled_Form";
            this.Text = " Scheduled Cancel Appointments ";
            this.Load += new System.EventHandler(this.TMS00110Form_Load);
            this.FormClosed += new Wisej.Web.FormClosedEventHandler(this.TMS00110Form_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SchAppGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Label LblReq;
        private Panel panel1;
        private Panel panel2;
        private DateTimePicker AppDate;
        private Label label2;
        private Label label1;
        private Button BtnMemSearch;
        private Button BtnNextSlot;
        private Button BtnRefSch;
        private Label LblDate;
        private Button BtnSearch;
        private Label LblSite;
        private TextBox TxtSiteCode;
        private Panel PnlGrid;
        private DataGridViewEx SchAppGrid;
        private DataGridViewTextBoxColumn Time;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn Telephone;
        private DataGridViewTextBoxColumn Person;
      //  private MaskedTextBox MskSsn;
      //  private Label LblSsn;
        private Button BtnCancel;
        private Button Btn_Update;
        private DataGridViewTextBoxColumn SlotNo;
        private DataGridViewTextBoxColumn KeyTime;
        private DataGridViewTextBoxColumn AppStatus;
        private DataGridViewCheckBoxColumn Delete;
        private DataGridViewTextBoxColumn KeySsn;
        private DataGridViewTextBoxColumn Rec_Type;
        private DataGridViewTextBoxColumn gvt_TempId;
        private DataGridViewTextBoxColumn gvStatus;
        private Button btnHist;
        private Spacer spacer1;
        private Spacer spacer7;
        private Spacer spacer6;
        private Spacer spacer5;
        private Spacer spacer4;
        private Spacer spacer3;
        private Spacer spacer2;
        private Controls.Compatibility.DataGridViewDateTimeColumn DOB;
    }
}