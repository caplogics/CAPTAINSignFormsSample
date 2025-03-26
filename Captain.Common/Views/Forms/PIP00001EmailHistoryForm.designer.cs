using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PIP00001EmailHistoryForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIP00001EmailHistoryForm));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlMsgApl = new Wisej.Web.Panel();
            this.txtMessage = new Wisej.Web.TextBox();
            this.lblMsg = new Wisej.Web.Label();
            this.btnSend = new Wisej.Web.Button();
            this.label3 = new Wisej.Web.Label();
            this.pnlhtmlBox1 = new Wisej.Web.Panel();
            this.htmlBox1 = new Wisej.Web.HtmlPanel();
            this.pnlgvwEmailDetails = new Wisej.Web.Panel();
            this.gvwEmailDetails = new DataGridViewEx();
            this.colSel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvtSender = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtTypeofmail = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtRemarks = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtEmailType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtEmailRead = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlShow = new Wisej.Web.Panel();
            this.chkSelAll = new Wisej.Web.CheckBox();
            this.cmbShow = new Wisej.Web.ComboBox();
            this.lblShow = new Wisej.Web.Label();
            this.panel3 = new Wisej.Web.Panel();
            this.btnEmailHistPrint = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlMsgApl.SuspendLayout();
            this.pnlhtmlBox1.SuspendLayout();
            this.pnlgvwEmailDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwEmailDetails)).BeginInit();
            this.pnlShow.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlMsgApl);
            this.pnlCompleteForm.Controls.Add(this.pnlhtmlBox1);
            this.pnlCompleteForm.Controls.Add(this.pnlgvwEmailDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlShow);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Padding = new Wisej.Web.Padding(5);
            this.pnlCompleteForm.Size = new System.Drawing.Size(808, 551);
            this.pnlCompleteForm.TabIndex = 0;
            this.pnlCompleteForm.Click += new System.EventHandler(this.panel1_Click);
            // 
            // pnlMsgApl
            // 
            this.pnlMsgApl.Controls.Add(this.txtMessage);
            this.pnlMsgApl.Controls.Add(this.lblMsg);
            this.pnlMsgApl.Controls.Add(this.btnSend);
            this.pnlMsgApl.Controls.Add(this.label3);
            this.pnlMsgApl.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMsgApl.Location = new System.Drawing.Point(5, 451);
            this.pnlMsgApl.Name = "pnlMsgApl";
            this.pnlMsgApl.Size = new System.Drawing.Size(798, 95);
            this.pnlMsgApl.TabIndex = 31;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(157, 7);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(536, 86);
            this.txtMessage.TabIndex = 7;
            // 
            // lblMsg
            // 
            this.lblMsg.Location = new System.Drawing.Point(15, 8);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(122, 16);
            this.lblMsg.TabIndex = 6;
            this.lblMsg.Text = "Message to Applicant";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(726, 8);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(60, 25);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "&Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(137, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(7, 10);
            this.label3.TabIndex = 27;
            this.label3.Text = "*";
            // 
            // pnlhtmlBox1
            // 
            this.pnlhtmlBox1.Controls.Add(this.htmlBox1);
            this.pnlhtmlBox1.Dock = Wisej.Web.DockStyle.Top;
            this.pnlhtmlBox1.Location = new System.Drawing.Point(5, 252);
            this.pnlhtmlBox1.Name = "pnlhtmlBox1";
            this.pnlhtmlBox1.Padding = new Wisej.Web.Padding(5);
            this.pnlhtmlBox1.Size = new System.Drawing.Size(798, 199);
            this.pnlhtmlBox1.TabIndex = 30;
            // 
            // htmlBox1
            // 
            this.htmlBox1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.htmlBox1.CssStyle = "padding:8px;";
            this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
            this.htmlBox1.Focusable = false;
            this.htmlBox1.Html = "<HTML>No content.</HTML>";
            this.htmlBox1.Location = new System.Drawing.Point(5, 5);
            this.htmlBox1.Name = "htmlBox1";
            this.htmlBox1.Padding = new Wisej.Web.Padding(5);
            this.htmlBox1.Size = new System.Drawing.Size(788, 189);
            this.htmlBox1.TabIndex = 5;
            this.htmlBox1.TabStop = false;
            // 
            // pnlgvwEmailDetails
            // 
            this.pnlgvwEmailDetails.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlgvwEmailDetails.Controls.Add(this.gvwEmailDetails);
            this.pnlgvwEmailDetails.CssStyle = "border-radius: 15px 15px 15px 15px;";
            this.pnlgvwEmailDetails.Dock = Wisej.Web.DockStyle.Top;
            this.pnlgvwEmailDetails.Location = new System.Drawing.Point(5, 81);
            this.pnlgvwEmailDetails.Name = "pnlgvwEmailDetails";
            this.pnlgvwEmailDetails.Size = new System.Drawing.Size(798, 171);
            this.pnlgvwEmailDetails.TabIndex = 29;
            // 
            // gvwEmailDetails
            // 
            this.gvwEmailDetails.AllowUserToResizeColumns = false;
            this.gvwEmailDetails.AllowUserToResizeRows = false;
            this.gvwEmailDetails.BackColor = System.Drawing.Color.White;
            this.gvwEmailDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.colSel,
            this.gvtDate,
            this.gvtSender,
            this.gvtTypeofmail,
            this.gvtRemarks,
            this.gvtEmailType,
            this.gvtEmailRead});
            this.gvwEmailDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwEmailDetails.Location = new System.Drawing.Point(0, 0);
            this.gvwEmailDetails.MultiSelect = false;
            this.gvwEmailDetails.Name = "gvwEmailDetails";
            this.gvwEmailDetails.RowHeadersWidth = 14;
            this.gvwEmailDetails.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwEmailDetails.RowTemplate.Height = 26;
            this.gvwEmailDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwEmailDetails.Size = new System.Drawing.Size(796, 169);
            this.gvwEmailDetails.TabIndex = 3;
            this.gvwEmailDetails.SelectionChanged += new System.EventHandler(this.gvwEmailDetails_SelectionChanged);
            this.gvwEmailDetails.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwEmailDetails_CellClick);
            // 
            // colSel
            // 
            this.colSel.HeaderText = "";
            this.colSel.Name = "colSel";
            this.colSel.ShowInVisibilityMenu = false;
            this.colSel.Width = 45;
            // 
            // gvtDate
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtDate.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtDate.HeaderStyle = dataGridViewCellStyle2;
            this.gvtDate.HeaderText = "Date";
            this.gvtDate.Name = "gvtDate";
            this.gvtDate.ReadOnly = true;
            this.gvtDate.Width = 200;
            // 
            // gvtSender
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSender.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSender.HeaderStyle = dataGridViewCellStyle4;
            this.gvtSender.HeaderText = "Sender";
            this.gvtSender.Name = "gvtSender";
            this.gvtSender.ReadOnly = true;
            this.gvtSender.Width = 175;
            // 
            // gvtTypeofmail
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtTypeofmail.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtTypeofmail.HeaderStyle = dataGridViewCellStyle6;
            this.gvtTypeofmail.HeaderText = "Type of Mail";
            this.gvtTypeofmail.Name = "gvtTypeofmail";
            this.gvtTypeofmail.ReadOnly = true;
            this.gvtTypeofmail.Width = 350;
            // 
            // gvtRemarks
            // 
            this.gvtRemarks.HeaderText = "gvtRemarks";
            this.gvtRemarks.Name = "gvtRemarks";
            this.gvtRemarks.ReadOnly = true;
            this.gvtRemarks.ShowInVisibilityMenu = false;
            this.gvtRemarks.Visible = false;
            this.gvtRemarks.Width = 10;
            // 
            // gvtEmailType
            // 
            this.gvtEmailType.HeaderText = "gvtEmailType";
            this.gvtEmailType.Name = "gvtEmailType";
            this.gvtEmailType.ShowInVisibilityMenu = false;
            this.gvtEmailType.Visible = false;
            this.gvtEmailType.Width = 10;
            // 
            // gvtEmailRead
            // 
            this.gvtEmailRead.HeaderText = "gvtEmailRead";
            this.gvtEmailRead.Name = "gvtEmailRead";
            this.gvtEmailRead.ShowInVisibilityMenu = false;
            this.gvtEmailRead.Visible = false;
            this.gvtEmailRead.Width = 10;
            // 
            // pnlShow
            // 
            this.pnlShow.Controls.Add(this.chkSelAll);
            this.pnlShow.Controls.Add(this.cmbShow);
            this.pnlShow.Controls.Add(this.lblShow);
            this.pnlShow.Dock = Wisej.Web.DockStyle.Top;
            this.pnlShow.Location = new System.Drawing.Point(5, 5);
            this.pnlShow.Name = "pnlShow";
            this.pnlShow.Size = new System.Drawing.Size(798, 76);
            this.pnlShow.TabIndex = 28;
            // 
            // chkSelAll
            // 
            this.chkSelAll.Location = new System.Drawing.Point(20, 52);
            this.chkSelAll.Name = "chkSelAll";
            this.chkSelAll.Size = new System.Drawing.Size(83, 21);
            this.chkSelAll.TabIndex = 5;
            this.chkSelAll.Text = "Select All";
            this.chkSelAll.CheckedChanged += new System.EventHandler(this.chkSelAll_CheckedChanged);
            // 
            // cmbShow
            // 
            this.cmbShow.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbShow.FormattingEnabled = true;
            this.cmbShow.Location = new System.Drawing.Point(60, 9);
            this.cmbShow.Name = "cmbShow";
            this.cmbShow.Size = new System.Drawing.Size(192, 25);
            this.cmbShow.TabIndex = 2;
            this.cmbShow.SelectedIndexChanged += new System.EventHandler(this.cmbShow_SelectedIndexChanged);
            // 
            // lblShow
            // 
            this.lblShow.Location = new System.Drawing.Point(15, 13);
            this.lblShow.Name = "lblShow";
            this.lblShow.Size = new System.Drawing.Size(31, 14);
            this.lblShow.TabIndex = 4;
            this.lblShow.Text = "Show";
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.btnEmailHistPrint);
            this.panel3.Controls.Add(this.spacer1);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 551);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel3.Size = new System.Drawing.Size(808, 35);
            this.panel3.TabIndex = 9;
            // 
            // btnEmailHistPrint
            // 
            this.btnEmailHistPrint.Dock = Wisej.Web.DockStyle.Right;
            this.btnEmailHistPrint.Location = new System.Drawing.Point(613, 5);
            this.btnEmailHistPrint.Name = "btnEmailHistPrint";
            this.btnEmailHistPrint.Size = new System.Drawing.Size(100, 25);
            this.btnEmailHistPrint.TabIndex = 9;
            this.btnEmailHistPrint.Text = "&Print";
            this.btnEmailHistPrint.Click += new System.EventHandler(this.btnEmailHistPrint_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(713, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(718, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PIP00001EmailHistoryForm
            // 
            this.ClientSize = new System.Drawing.Size(808, 586);
            this.Controls.Add(this.pnlCompleteForm);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIP00001EmailHistoryForm";
            this.Text = "PIP00001EmailHistoryForm";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlMsgApl.ResumeLayout(false);
            this.pnlMsgApl.PerformLayout();
            this.pnlhtmlBox1.ResumeLayout(false);
            this.pnlgvwEmailDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwEmailDetails)).EndInit();
            this.pnlShow.ResumeLayout(false);
            this.pnlShow.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Panel pnlCompleteForm;
        private DataGridViewEx gvwEmailDetails;
        private DataGridViewTextBoxColumn gvtSender;
        private DataGridViewTextBoxColumn gvtTypeofmail;
        private Panel panel3;
        private Button btnCancel;
        private HtmlPanel htmlBox1;
        private DataGridViewTextBoxColumn gvtRemarks;
        private Label lblShow;
        private ComboBox cmbShow;
        private DataGridViewTextBoxColumn gvtEmailType;
        private DataGridViewTextBoxColumn gvtEmailRead;
        private Button btnSend;
        private TextBox txtMessage;
        private Label lblMsg;
        private Label label3;
        private Panel pnlgvwEmailDetails;
        private Panel pnlShow;
        private Panel pnlhtmlBox1;
        private Panel pnlMsgApl;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtDate;
        private Button btnEmailHistPrint;
        private Spacer spacer1;
        private DataGridViewCheckBoxColumn colSel;
        private CheckBox chkSelAll;
    }
}