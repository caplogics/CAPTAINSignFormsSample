using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PrintLetters
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

        #region WISEJ Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintLetters));
            this.panel1 = new Wisej.Web.Panel();
            this.lblEmail = new Wisej.Web.Label();
            this.lblmail = new Wisej.Web.Label();
            this.lblName = new Wisej.Web.Label();
            this.lblAppNo = new Wisej.Web.Label();
            this.lblN = new Wisej.Web.Label();
            this.lblApp = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.gvApp = new DataGridView();
            this.Check = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.AppDet = new Wisej.Web.DataGridViewTextBoxColumn();
            this.RecentWorker = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvStatus = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvSign = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnPrev = new Wisej.Web.Button();
            this.btnExit = new Wisej.Web.Button();
            this.btnHistory = new Wisej.Web.Button();
            this.panel3 = new Wisej.Web.Panel();
            this.chkbSend = new Wisej.Web.CheckBox();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel4 = new Wisej.Web.Panel();
            this.RecentDate = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvApp)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblEmail);
            this.panel1.Controls.Add(this.lblmail);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.lblAppNo);
            this.panel1.Controls.Add(this.lblN);
            this.panel1.Controls.Add(this.lblApp);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(694, 56);
            this.panel1.TabIndex = 0;
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(348, 20);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(339, 19);
            this.lblEmail.TabIndex = 1;
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblmail
            // 
            this.lblmail.AutoSize = true;
            this.lblmail.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblmail.Location = new System.Drawing.Point(303, 20);
            this.lblmail.MaximumSize = new System.Drawing.Size(100, 50);
            this.lblmail.MinimumSize = new System.Drawing.Size(0, 19);
            this.lblmail.Name = "lblmail";
            this.lblmail.Size = new System.Drawing.Size(41, 19);
            this.lblmail.TabIndex = 0;
            this.lblmail.Text = "Email :";
            this.lblmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblName.Location = new System.Drawing.Point(60, 33);
            this.lblName.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 18);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // lblAppNo
            // 
            this.lblAppNo.AutoSize = true;
            this.lblAppNo.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAppNo.Location = new System.Drawing.Point(60, 8);
            this.lblAppNo.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(35, 18);
            this.lblAppNo.TabIndex = 0;
            this.lblAppNo.Text = "App#";
            // 
            // lblN
            // 
            this.lblN.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblN.Location = new System.Drawing.Point(8, 33);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(45, 18);
            this.lblN.TabIndex = 0;
            this.lblN.Text = "Name :";
            // 
            // lblApp
            // 
            this.lblApp.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApp.Location = new System.Drawing.Point(8, 8);
            this.lblApp.Name = "lblApp";
            this.lblApp.Size = new System.Drawing.Size(42, 19);
            this.lblApp.TabIndex = 0;
            this.lblApp.Text = "App# :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvApp);
            this.panel2.CssStyle = "border:1px solid #ccc; border-radius:10px;";
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(7, 1);
            this.panel2.Margin = new Wisej.Web.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(680, 280);
            this.panel2.TabIndex = 1;
            // 
            // gvApp
            // 
            this.gvApp.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvApp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvApp.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvApp.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Check,
            this.AppDet,
            this.RecentDate,
            this.RecentWorker,
            this.gvCode,
            this.gvStatus,
            this.gvSign});
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvApp.DefaultCellStyle = dataGridViewCellStyle9;
            this.gvApp.Dock = Wisej.Web.DockStyle.Fill;
            this.gvApp.Name = "gvApp";
            this.gvApp.RowHeadersVisible = false;
            this.gvApp.RowHeadersWidth = 25;
            this.gvApp.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvApp.ShowColumnVisibilityMenu = false;
            this.gvApp.Size = new System.Drawing.Size(680, 280);
            this.gvApp.TabIndex = 0;
            this.gvApp.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvApp_CellValueChanged);
            this.gvApp.SelectionChanged += new System.EventHandler(this.gvApp_SelectionChanged);
            this.gvApp.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvApp_CellClick);
            // 
            // Check
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.Check.DefaultCellStyle = dataGridViewCellStyle2;
            this.Check.HeaderText = " ";
            this.Check.Name = "Check";
            this.Check.Visible = false;
            this.Check.Width = 30;
            // 
            // AppDet
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.Padding = new Wisej.Web.Padding(7, 0, 0, 0);
            this.AppDet.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.Padding = new Wisej.Web.Padding(7, 0, 0, 0);
            this.AppDet.HeaderStyle = dataGridViewCellStyle4;
            this.AppDet.HeaderText = "Application Details";
            this.AppDet.Name = "AppDet";
            this.AppDet.ReadOnly = true;
            this.AppDet.Width = 280;
            // 
            // RecentWorker
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentWorker.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentWorker.HeaderStyle = dataGridViewCellStyle8;
            this.RecentWorker.HeaderText = "Recent Printed Worker";
            this.RecentWorker.Name = "RecentWorker";
            this.RecentWorker.ReadOnly = true;
            this.RecentWorker.Width = 145;
            // 
            // gvCode
            // 
            this.gvCode.HeaderText = "gvCode";
            this.gvCode.Name = "gvCode";
            this.gvCode.Visible = false;
            this.gvCode.Width = 20;
            // 
            // gvStatus
            // 
            this.gvStatus.HeaderText = "gvStatus";
            this.gvStatus.Name = "gvStatus";
            this.gvStatus.Visible = false;
            this.gvStatus.Width = 20;
            // 
            // gvSign
            // 
            this.gvSign.HeaderText = "gvSign";
            this.gvSign.Name = "gvSign";
            this.gvSign.ShowInVisibilityMenu = false;
            this.gvSign.Visible = false;
            this.gvSign.Width = 30;
            // 
            // btnPrev
            // 
            this.btnPrev.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrev.Location = new System.Drawing.Point(569, 4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(59, 27);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "Pre&view";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-cancel";
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(633, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(57, 27);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Dock = Wisej.Web.DockStyle.Left;
            this.btnHistory.Location = new System.Drawing.Point(4, 4);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(59, 27);
            this.btnHistory.TabIndex = 3;
            this.btnHistory.Text = "&History";
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.chkbSend);
            this.panel3.Controls.Add(this.btnPrev);
            this.panel3.Controls.Add(this.btnHistory);
            this.panel3.Controls.Add(this.spacer1);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 344);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(4);
            this.panel3.Size = new System.Drawing.Size(694, 35);
            this.panel3.TabIndex = 4;
            // 
            // chkbSend
            // 
            this.chkbSend.BackColor = System.Drawing.Color.Transparent;
            this.chkbSend.Location = new System.Drawing.Point(305, 7);
            this.chkbSend.Name = "chkbSend";
            this.chkbSend.Size = new System.Drawing.Size(126, 21);
            this.chkbSend.TabIndex = 5;
            this.chkbSend.Text = "Send to Applicant";
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(628, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 56);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(7, 1, 7, 7);
            this.panel4.Size = new System.Drawing.Size(694, 288);
            this.panel4.TabIndex = 5;
            // 
            // RecentDate
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentDate.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RecentDate.HeaderStyle = dataGridViewCellStyle6;
            this.RecentDate.HeaderText = "Recent Printed Date";
            this.RecentDate.Name = "RecentDate";
            this.RecentDate.ReadOnly = true;
            this.RecentDate.Width = 200;
            // 
            // PrintLetters
            // 
            this.ClientSize = new System.Drawing.Size(694, 379);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintLetters";
            this.Text = "PrintApplicants";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvApp)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label lblName;
        private Label lblAppNo;
        private Label lblN;
        private Label lblApp;
        private Panel panel2;
        private DataGridView gvApp;
        private DataGridViewCheckBoxColumn Check;
        private DataGridViewTextBoxColumn AppDet;
        private Button btnPrev;
        private Button btnExit;
        private Label lblmail;
        private DataGridViewTextBoxColumn RecentWorker;
        private DataGridViewTextBoxColumn gvCode;
        private DataGridViewTextBoxColumn gvStatus;
        private Button btnHistory;
        private Panel panel3;
        private Spacer spacer1;
        private Panel panel4;
        private CheckBox chkbSend;
        private DataGridViewTextBoxColumn gvSign;
        private Label lblEmail;
        private DataGridViewTextBoxColumn RecentDate;
    }
}