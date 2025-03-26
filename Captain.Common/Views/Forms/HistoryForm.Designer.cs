using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class HistoryForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryForm));
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnExit = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.dataGridChangeFieds = new Wisej.Web.DataGridView();
            this.Field = new Wisej.Web.DataGridViewTextBoxColumn();
            this.OldValue = new Wisej.Web.DataGridViewTextBoxColumn();
            this.NewValue = new Wisej.Web.DataGridViewTextBoxColumn();
            this.lblChanges = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.dataGridHistory = new Wisej.Web.DataGridView();
            this.DateTime = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ChangedBy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSubscr = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Seq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtVendor = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtInvdate = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAmount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtdetails = new Wisej.Web.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridChangeFieds)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromName("@control");
            this.flowLayoutPanel1.Controls.Add(this.btnExit);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 433);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(699, 35);
            this.flowLayoutPanel1.TabIndex = 4;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-error";
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExit.Location = new System.Drawing.Point(606, 6);
            this.btnExit.MaximumSize = new System.Drawing.Size(0, 25);
            this.btnExit.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(84, 25);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridChangeFieds);
            this.panel1.Controls.Add(this.lblChanges);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 243);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 190);
            this.panel1.TabIndex = 5;
            this.panel1.TabStop = true;
            // 
            // dataGridChangeFieds
            // 
            this.dataGridChangeFieds.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridChangeFieds.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dataGridChangeFieds.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridChangeFieds.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridChangeFieds.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Field,
            this.OldValue,
            this.NewValue});
            this.dataGridChangeFieds.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridChangeFieds.Location = new System.Drawing.Point(0, 25);
            this.dataGridChangeFieds.Name = "dataGridChangeFieds";
            this.dataGridChangeFieds.RowHeadersWidth = 15;
            this.dataGridChangeFieds.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridChangeFieds.ShowColumnVisibilityMenu = false;
            this.dataGridChangeFieds.Size = new System.Drawing.Size(699, 165);
            this.dataGridChangeFieds.TabIndex = 2;
            this.dataGridChangeFieds.Click += new System.EventHandler(this.dataGridChangeFieds_Click);
            // 
            // Field
            // 
            this.Field.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Field.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Field.HeaderStyle = dataGridViewCellStyle3;
            this.Field.HeaderText = "Field";
            this.Field.Name = "Field";
            this.Field.ReadOnly = true;
            this.Field.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Field.Width = 250;
            // 
            // OldValue
            // 
            this.OldValue.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.OldValue.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.OldValue.HeaderStyle = dataGridViewCellStyle5;
            this.OldValue.HeaderText = "Old Value";
            this.OldValue.Name = "OldValue";
            this.OldValue.ReadOnly = true;
            this.OldValue.Width = 200;
            // 
            // NewValue
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.NewValue.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.NewValue.HeaderStyle = dataGridViewCellStyle7;
            this.NewValue.HeaderText = "New Value";
            this.NewValue.Name = "NewValue";
            this.NewValue.ReadOnly = true;
            this.NewValue.Width = 200;
            // 
            // lblChanges
            // 
            this.lblChanges.BackColor = System.Drawing.Color.FromArgb(237, 243, 249);
            this.lblChanges.Dock = Wisej.Web.DockStyle.Top;
            this.lblChanges.Font = new System.Drawing.Font("defaultBold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblChanges.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblChanges.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblChanges.Location = new System.Drawing.Point(0, 0);
            this.lblChanges.Name = "lblChanges";
            this.lblChanges.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.lblChanges.Size = new System.Drawing.Size(699, 25);
            this.lblChanges.TabIndex = 3;
            this.lblChanges.Text = "Changes";
            this.lblChanges.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridHistory);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(699, 243);
            this.panel2.TabIndex = 6;
            this.panel2.TabStop = true;
            // 
            // dataGridHistory
            // 
            this.dataGridHistory.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridHistory.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dataGridHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridHistory.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridHistory.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.DateTime,
            this.ChangedBy,
            this.gvtSubscr,
            this.Seq,
            this.gvtVendor,
            this.gvtInvdate,
            this.gvtAmount,
            this.gvtdetails});
            this.dataGridHistory.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridHistory.Location = new System.Drawing.Point(0, 0);
            this.dataGridHistory.Name = "dataGridHistory";
            this.dataGridHistory.RowHeadersWidth = 15;
            this.dataGridHistory.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridHistory.ShowColumnVisibilityMenu = false;
            this.dataGridHistory.Size = new System.Drawing.Size(699, 243);
            this.dataGridHistory.TabIndex = 1;
            this.dataGridHistory.SelectionChanged += new System.EventHandler(this.dataGridHistory_SelectionChanged);
            // 
            // DateTime
            // 
            this.DateTime.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.DateTime.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.DateTime.HeaderStyle = dataGridViewCellStyle10;
            this.DateTime.HeaderText = "Date & Time";
            this.DateTime.Name = "DateTime";
            this.DateTime.ReadOnly = true;
            this.DateTime.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.DateTime.Width = 150;
            // 
            // ChangedBy
            // 
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ChangedBy.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ChangedBy.HeaderStyle = dataGridViewCellStyle12;
            this.ChangedBy.HeaderText = "Changed By";
            this.ChangedBy.Name = "ChangedBy";
            this.ChangedBy.ReadOnly = true;
            this.ChangedBy.Width = 130;
            // 
            // gvtSubscr
            // 
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtSubscr.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtSubscr.HeaderStyle = dataGridViewCellStyle14;
            this.gvtSubscr.HeaderText = "Screen";
            this.gvtSubscr.Name = "gvtSubscr";
            this.gvtSubscr.Width = 200;
            // 
            // Seq
            // 
            this.Seq.Name = "Seq";
            this.Seq.ReadOnly = true;
            this.Seq.Visible = false;
            this.Seq.Width = 50;
            // 
            // gvtVendor
            // 
            this.gvtVendor.HeaderText = "Vendor Name";
            this.gvtVendor.Name = "gvtVendor";
            this.gvtVendor.Width = 200;
            // 
            // gvtInvdate
            // 
            this.gvtInvdate.HeaderText = "Inv Date";
            this.gvtInvdate.Name = "gvtInvdate";
            this.gvtInvdate.Width = 80;
            // 
            // gvtAmount
            // 
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvtAmount.DefaultCellStyle = dataGridViewCellStyle15;
            this.gvtAmount.HeaderText = "Amount";
            this.gvtAmount.Name = "gvtAmount";
            this.gvtAmount.Width = 75;
            // 
            // gvtdetails
            // 
            this.gvtdetails.HeaderText = "details";
            this.gvtdetails.Name = "gvtdetails";
            this.gvtdetails.Visible = false;
            // 
            // HistoryForm
            // 
            this.ClientSize = new System.Drawing.Size(699, 468);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistoryForm";
            this.Text = "History";
            this.Load += new System.EventHandler(this.HistoryForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridChangeFieds)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnExit;
        private Panel panel1;
        private DataGridView dataGridChangeFieds;
        private DataGridViewTextBoxColumn Field;
        private DataGridViewTextBoxColumn OldValue;
        private DataGridViewTextBoxColumn NewValue;
        private Label lblChanges;
        private Panel panel2;
        private DataGridView dataGridHistory;
        private DataGridViewTextBoxColumn DateTime;
        private DataGridViewTextBoxColumn ChangedBy;
        private DataGridViewTextBoxColumn gvtSubscr;
        private DataGridViewTextBoxColumn Seq;
        private DataGridViewTextBoxColumn gvtVendor;
        private DataGridViewTextBoxColumn gvtInvdate;
        private DataGridViewTextBoxColumn gvtAmount;
        private DataGridViewTextBoxColumn gvtdetails;
    }
}