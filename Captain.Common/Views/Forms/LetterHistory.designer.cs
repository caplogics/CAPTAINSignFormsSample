using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class LetterHistory
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LetterHistory));
            this.panel2 = new Wisej.Web.Panel();
            this.gvApp = new Wisej.Web.DataGridView();
            this.AppDet = new Wisej.Web.DataGridViewTextBoxColumn();
            this.RecentWorker = new Wisej.Web.DataGridViewTextBoxColumn();
            this.RecentDate = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Column0 = new Wisej.Web.DataGridViewColumn();
            this.Column1 = new Wisej.Web.DataGridViewColumn();
            this.gvmovCAP = new Wisej.Web.DataGridViewColumn();
            this.colmovby = new Wisej.Web.DataGridViewColumn();
            this.gvCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvStatus = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnExit = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.spacer3 = new Wisej.Web.Spacer();
            this.panel4 = new Wisej.Web.Panel();
            this.cmbLetterTypes = new Wisej.Web.ComboBox();
            this.spacer2 = new Wisej.Web.Spacer();
            this.label2 = new Wisej.Web.Label();
            this.spacer1 = new Wisej.Web.Spacer();
            this.cmbStatusType = new Wisej.Web.ComboBox();
            this.label1 = new Wisej.Web.Label();
            this.colEmailSentStatus = new Wisej.Web.DataGridViewColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvApp)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvApp);
            this.panel2.CssStyle = "border:1px solid #ccc; border-radius:10px;";
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(7, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1155, 416);
            this.panel2.TabIndex = 1;
            // 
            // gvApp
            // 
            this.gvApp.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvApp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvApp.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvApp.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.AppDet,
            this.RecentWorker,
            this.RecentDate,
            this.colEmailSentStatus,
            this.Column0,
            this.Column1,
            this.gvmovCAP,
            this.colmovby,
            this.gvCode,
            this.gvStatus});
            dataGridViewCellStyle15.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvApp.DefaultCellStyle = dataGridViewCellStyle15;
            this.gvApp.Dock = Wisej.Web.DockStyle.Fill;
            this.gvApp.Location = new System.Drawing.Point(0, 0);
            this.gvApp.Name = "gvApp";
            this.gvApp.RowHeadersVisible = false;
            this.gvApp.RowHeadersWidth = 25;
            this.gvApp.ShowColumnVisibilityMenu = false;
            this.gvApp.Size = new System.Drawing.Size(1155, 416);
            this.gvApp.TabIndex = 0;
            this.gvApp.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvApp_CellValueChanged);
            this.gvApp.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvApp_CellClick);
            // 
            // AppDet
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.AppDet.DefaultCellStyle = dataGridViewCellStyle2;
            this.AppDet.HeaderText = " Name of the Letter";
            this.AppDet.Name = "AppDet";
            this.AppDet.ReadOnly = true;
            this.AppDet.ShowInVisibilityMenu = false;
            this.AppDet.Width = 200;
            // 
            // RecentWorker
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentWorker.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentWorker.HeaderStyle = dataGridViewCellStyle4;
            this.RecentWorker.HeaderText = "Printed By";
            this.RecentWorker.Name = "RecentWorker";
            this.RecentWorker.ReadOnly = true;
            this.RecentWorker.Width = 110;
            // 
            // RecentDate
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentDate.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentDate.HeaderStyle = dataGridViewCellStyle6;
            this.RecentDate.HeaderText = "Printed On";
            this.RecentDate.Name = "RecentDate";
            this.RecentDate.ReadOnly = true;
            this.RecentDate.Width = 150;
            // 
            // Column0
            // 
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.HeaderStyle = dataGridViewCellStyle10;
            this.Column0.HeaderText = "Downloaded On";
            this.Column0.Name = "Column0";
            this.Column0.Width = 150;
            // 
            // Column1
            // 
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.HeaderStyle = dataGridViewCellStyle12;
            this.Column1.HeaderText = "Signed On";
            this.Column1.Name = "Column1";
            this.Column1.Width = 150;
            // 
            // gvmovCAP
            // 
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvmovCAP.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvmovCAP.HeaderStyle = dataGridViewCellStyle14;
            this.gvmovCAP.HeaderText = "Moved to CAPTAIN ";
            this.gvmovCAP.Name = "gvmovCAP";
            this.gvmovCAP.Width = 150;
            // 
            // colmovby
            // 
            this.colmovby.HeaderText = "Moved By";
            this.colmovby.Name = "colmovby";
            this.colmovby.Width = 120;
            // 
            // gvCode
            // 
            this.gvCode.HeaderText = "gvCode";
            this.gvCode.Name = "gvCode";
            this.gvCode.ShowInVisibilityMenu = false;
            this.gvCode.Visible = false;
            this.gvCode.Width = 20;
            // 
            // gvStatus
            // 
            this.gvStatus.HeaderText = "gvStatus";
            this.gvStatus.Name = "gvStatus";
            this.gvStatus.ShowInVisibilityMenu = false;
            this.gvStatus.Visible = false;
            this.gvStatus.Width = 20;
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-cancel";
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1108, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(57, 27);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 462);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(4);
            this.panel1.Size = new System.Drawing.Size(1169, 35);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.spacer3);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(7);
            this.panel3.Size = new System.Drawing.Size(1169, 462);
            this.panel3.TabIndex = 4;
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Top;
            this.spacer3.Location = new System.Drawing.Point(7, 34);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(1155, 5);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cmbLetterTypes);
            this.panel4.Controls.Add(this.spacer2);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.spacer1);
            this.panel4.Controls.Add(this.cmbStatusType);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(7, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1155, 27);
            this.panel4.TabIndex = 2;
            // 
            // cmbLetterTypes
            // 
            this.cmbLetterTypes.Dock = Wisej.Web.DockStyle.Left;
            this.cmbLetterTypes.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbLetterTypes.Location = new System.Drawing.Point(377, 0);
            this.cmbLetterTypes.Name = "cmbLetterTypes";
            this.cmbLetterTypes.Size = new System.Drawing.Size(297, 27);
            this.cmbLetterTypes.TabIndex = 4;
            this.cmbLetterTypes.SelectedIndexChanged += new System.EventHandler(this.cmbLetterTypes_SelectedIndexChanged);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(372, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 27);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = Wisej.Web.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(331, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "Letters  ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(301, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(30, 27);
            // 
            // cmbStatusType
            // 
            this.cmbStatusType.Dock = Wisej.Web.DockStyle.Left;
            this.cmbStatusType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbStatusType.Location = new System.Drawing.Point(4, 0);
            this.cmbStatusType.Name = "cmbStatusType";
            this.cmbStatusType.Size = new System.Drawing.Size(297, 27);
            this.cmbStatusType.TabIndex = 1;
            this.cmbStatusType.SelectedIndexChanged += new System.EventHandler(this.cmbStatusType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = Wisej.Web.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(4, 27);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // colEmailSentStatus
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.colEmailSentStatus.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.colEmailSentStatus.HeaderStyle = dataGridViewCellStyle8;
            this.colEmailSentStatus.HeaderText = "Email Status";
            this.colEmailSentStatus.Name = "Column2";
            this.colEmailSentStatus.Width = 105;
            // 
            // LetterHistory
            // 
            this.ClientSize = new System.Drawing.Size(1169, 497);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LetterHistory";
            this.Text = "Print Applicants";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvApp)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel panel2;
        private DataGridView gvApp;
        private DataGridViewTextBoxColumn AppDet;
        private Button btnExit;
        private DataGridViewTextBoxColumn RecentDate;
        private DataGridViewTextBoxColumn RecentWorker;
        private DataGridViewTextBoxColumn gvCode;
        private DataGridViewTextBoxColumn gvStatus;
        private Panel panel1;
        private Panel panel3;
        private Panel panel4;
        private Label label1;
        private Spacer spacer3;
        private ComboBox cmbLetterTypes;
        private Spacer spacer2;
        private Label label2;
        private Spacer spacer1;
        private ComboBox cmbStatusType;
        private DataGridViewColumn Column0;
        private DataGridViewColumn Column1;
        private DataGridViewColumn gvmovCAP;
        private DataGridViewColumn colmovby;
        private DataGridViewColumn colEmailSentStatus;
    }
}