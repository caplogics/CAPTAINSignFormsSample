using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PIP0001SendMailForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIP0001SendMailForm));
            this.lblEmail2 = new Wisej.Web.Label();
            this.lblConf2 = new Wisej.Web.Label();
            this.lblTokenNo = new Wisej.Web.Label();
            this.lblEmailId = new Wisej.Web.Label();
            this.pnlEmail = new Wisej.Web.Panel();
            this.btnEmailHist = new Wisej.Web.Button();
            this.pnlMessageNdGrid = new Wisej.Web.Panel();
            this.pnlReview = new Wisej.Web.Panel();
            this.gvwDocumentreview = new DataGridViewEx();
            this.gvchkselect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtFileName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtRemarks = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtVerId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtdocId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtverstatus = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCreateMessage = new Wisej.Web.Panel();
            this.txtMessage = new Wisej.Web.TextBox();
            this.pnlMessagesLbl = new Wisej.Web.Panel();
            this.rdoReview = new Wisej.Web.RadioButton();
            this.spacer2 = new Wisej.Web.Spacer();
            this.rdoClient = new Wisej.Web.RadioButton();
            this.spacer1 = new Wisej.Web.Spacer();
            this.lblAddmiscnote = new Wisej.Web.Label();
            this.pnlSendEmail = new Wisej.Web.Panel();
            this.btnSendReminder = new Wisej.Web.Button();
            this.spacer4 = new Wisej.Web.Spacer();
            this.btnEmailSend = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.btnCreateMiscMsg = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.gvtverifyDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.pnlEmail.SuspendLayout();
            this.pnlMessageNdGrid.SuspendLayout();
            this.pnlReview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwDocumentreview)).BeginInit();
            this.pnlCreateMessage.SuspendLayout();
            this.pnlMessagesLbl.SuspendLayout();
            this.pnlSendEmail.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmail2
            // 
            this.lblEmail2.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEmail2.ForeColor = System.Drawing.Color.FromName("@windowText");
            this.lblEmail2.Location = new System.Drawing.Point(300, 11);
            this.lblEmail2.Name = "lblEmail2";
            this.lblEmail2.Size = new System.Drawing.Size(340, 16);
            this.lblEmail2.TabIndex = 1;
            // 
            // lblConf2
            // 
            this.lblConf2.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblConf2.ForeColor = System.Drawing.Color.FromName("@windowText");
            this.lblConf2.Location = new System.Drawing.Point(113, 11);
            this.lblConf2.Name = "lblConf2";
            this.lblConf2.Size = new System.Drawing.Size(119, 16);
            this.lblConf2.TabIndex = 0;
            this.lblConf2.Text = " ";
            // 
            // lblTokenNo
            // 
            this.lblTokenNo.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblTokenNo.ForeColor = System.Drawing.Color.FromName("@windowText");
            this.lblTokenNo.Location = new System.Drawing.Point(15, 11);
            this.lblTokenNo.Name = "lblTokenNo";
            this.lblTokenNo.Size = new System.Drawing.Size(95, 16);
            this.lblTokenNo.TabIndex = 0;
            this.lblTokenNo.Text = "Confirmation# :";
            // 
            // lblEmailId
            // 
            this.lblEmailId.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEmailId.ForeColor = System.Drawing.Color.FromName("@windowText");
            this.lblEmailId.Location = new System.Drawing.Point(243, 11);
            this.lblEmailId.Name = "lblEmailId";
            this.lblEmailId.Size = new System.Drawing.Size(55, 16);
            this.lblEmailId.TabIndex = 1;
            this.lblEmailId.Text = "Email Id : ";
            // 
            // pnlEmail
            // 
            this.pnlEmail.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            this.pnlEmail.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlEmail.Controls.Add(this.btnEmailHist);
            this.pnlEmail.Controls.Add(this.lblEmail2);
            this.pnlEmail.Controls.Add(this.lblConf2);
            this.pnlEmail.Controls.Add(this.lblTokenNo);
            this.pnlEmail.Controls.Add(this.lblEmailId);
            this.pnlEmail.Dock = Wisej.Web.DockStyle.Top;
            this.pnlEmail.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.pnlEmail.Location = new System.Drawing.Point(0, 0);
            this.pnlEmail.Name = "pnlEmail";
            this.pnlEmail.Size = new System.Drawing.Size(799, 35);
            this.pnlEmail.TabIndex = 2;
            // 
            // btnEmailHist
            // 
            this.btnEmailHist.Location = new System.Drawing.Point(664, 5);
            this.btnEmailHist.Name = "btnEmailHist";
            this.btnEmailHist.Size = new System.Drawing.Size(90, 25);
            this.btnEmailHist.TabIndex = 6;
            this.btnEmailHist.Text = "Email &History";
            this.btnEmailHist.Visible = false;
            this.btnEmailHist.Click += new System.EventHandler(this.btnEmailHist_Click);
            // 
            // pnlMessageNdGrid
            // 
            this.pnlMessageNdGrid.Controls.Add(this.pnlReview);
            this.pnlMessageNdGrid.Controls.Add(this.pnlCreateMessage);
            this.pnlMessageNdGrid.Controls.Add(this.pnlMessagesLbl);
            this.pnlMessageNdGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMessageNdGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlMessageNdGrid.Name = "pnlMessageNdGrid";
            this.pnlMessageNdGrid.Padding = new Wisej.Web.Padding(5);
            this.pnlMessageNdGrid.Size = new System.Drawing.Size(799, 454);
            this.pnlMessageNdGrid.TabIndex = 3;
            // 
            // pnlReview
            // 
            this.pnlReview.Controls.Add(this.gvwDocumentreview);
            this.pnlReview.CssStyle = "border:1px solid #cccccc; border-radius:8px;";
            this.pnlReview.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlReview.Location = new System.Drawing.Point(5, 258);
            this.pnlReview.Name = "pnlReview";
            this.pnlReview.Size = new System.Drawing.Size(789, 191);
            this.pnlReview.TabIndex = 6;
            this.pnlReview.Visible = false;
            // 
            // gvwDocumentreview
            // 
            this.gvwDocumentreview.AllowUserToResizeColumns = false;
            this.gvwDocumentreview.AllowUserToResizeRows = false;
            this.gvwDocumentreview.BackColor = System.Drawing.Color.White;
            this.gvwDocumentreview.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvchkselect,
            this.gvtName,
            this.gvtverifyDate,
            this.gvtFileName,
            this.gvtRemarks,
            this.gvtVerId,
            this.gvtdocId,
            this.gvtverstatus});
            this.gvwDocumentreview.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwDocumentreview.Location = new System.Drawing.Point(0, 0);
            this.gvwDocumentreview.Name = "gvwDocumentreview";
            this.gvwDocumentreview.RowHeadersWidth = 14;
            this.gvwDocumentreview.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwDocumentreview.RowTemplate.Height = 26;
            this.gvwDocumentreview.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwDocumentreview.Size = new System.Drawing.Size(789, 191);
            this.gvwDocumentreview.TabIndex = 3;
            this.gvwDocumentreview.Click += new System.EventHandler(this.gvwDocumentreview_Click);
            // 
            // gvchkselect
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.gvchkselect.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvchkselect.HeaderStyle = dataGridViewCellStyle2;
            this.gvchkselect.HeaderText = "  ";
            this.gvchkselect.Name = "gvchkselect";
            this.gvchkselect.ShowInVisibilityMenu = false;
            this.gvchkselect.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvchkselect.Width = 30;
            // 
            // gvtName
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtName.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtName.HeaderStyle = dataGridViewCellStyle4;
            this.gvtName.HeaderText = "Name of the Member";
            this.gvtName.Name = "gvtName";
            this.gvtName.ReadOnly = true;
            this.gvtName.Width = 150;
            // 
            // gvtFileName
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtFileName.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtFileName.HeaderStyle = dataGridViewCellStyle8;
            this.gvtFileName.HeaderText = "Name of the Document";
            this.gvtFileName.Name = "gvtFileName";
            this.gvtFileName.ReadOnly = true;
            this.gvtFileName.Width = 180;
            // 
            // gvtRemarks
            // 
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtRemarks.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtRemarks.HeaderStyle = dataGridViewCellStyle10;
            this.gvtRemarks.HeaderText = "Remarks";
            this.gvtRemarks.Name = "gvtRemarks";
            this.gvtRemarks.ReadOnly = true;
            this.gvtRemarks.Width = 310;
            // 
            // gvtVerId
            // 
            this.gvtVerId.HeaderText = "gvtVerId";
            this.gvtVerId.Name = "gvtVerId";
            this.gvtVerId.ReadOnly = true;
            this.gvtVerId.ShowInVisibilityMenu = false;
            this.gvtVerId.Visible = false;
            this.gvtVerId.Width = 10;
            // 
            // gvtdocId
            // 
            this.gvtdocId.HeaderText = "gvtdocId";
            this.gvtdocId.Name = "gvtdocId";
            this.gvtdocId.ReadOnly = true;
            this.gvtdocId.ShowInVisibilityMenu = false;
            this.gvtdocId.Visible = false;
            this.gvtdocId.Width = 10;
            // 
            // gvtverstatus
            // 
            this.gvtverstatus.HeaderText = "gvtverstatus";
            this.gvtverstatus.Name = "gvtverstatus";
            this.gvtverstatus.ReadOnly = true;
            this.gvtverstatus.ShowInVisibilityMenu = false;
            this.gvtverstatus.Visible = false;
            this.gvtverstatus.Width = 10;
            // 
            // pnlCreateMessage
            // 
            this.pnlCreateMessage.Controls.Add(this.txtMessage);
            this.pnlCreateMessage.Dock = Wisej.Web.DockStyle.Top;
            this.pnlCreateMessage.Location = new System.Drawing.Point(5, 33);
            this.pnlCreateMessage.Name = "pnlCreateMessage";
            this.pnlCreateMessage.Padding = new Wisej.Web.Padding(0, 0, 0, 5);
            this.pnlCreateMessage.Size = new System.Drawing.Size(789, 225);
            this.pnlCreateMessage.TabIndex = 6;
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = Wisej.Web.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(0, 0);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(789, 220);
            this.txtMessage.TabIndex = 5;
            // 
            // pnlMessagesLbl
            // 
            this.pnlMessagesLbl.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            this.pnlMessagesLbl.Controls.Add(this.rdoReview);
            this.pnlMessagesLbl.Controls.Add(this.spacer2);
            this.pnlMessagesLbl.Controls.Add(this.rdoClient);
            this.pnlMessagesLbl.Controls.Add(this.spacer1);
            this.pnlMessagesLbl.Controls.Add(this.lblAddmiscnote);
            this.pnlMessagesLbl.Dock = Wisej.Web.DockStyle.Top;
            this.pnlMessagesLbl.Location = new System.Drawing.Point(5, 5);
            this.pnlMessagesLbl.Name = "pnlMessagesLbl";
            this.pnlMessagesLbl.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.pnlMessagesLbl.Size = new System.Drawing.Size(789, 28);
            this.pnlMessagesLbl.TabIndex = 7;
            // 
            // rdoReview
            // 
            this.rdoReview.AutoSize = false;
            this.rdoReview.Checked = true;
            this.rdoReview.Dock = Wisej.Web.DockStyle.Left;
            this.rdoReview.Location = new System.Drawing.Point(570, 0);
            this.rdoReview.Name = "rdoReview";
            this.rdoReview.Size = new System.Drawing.Size(214, 28);
            this.rdoReview.TabIndex = 1;
            this.rdoReview.TabStop = true;
            this.rdoReview.Text = "Review remarks to send to client";
            this.rdoReview.Visible = false;
            this.rdoReview.CheckedChanged += new System.EventHandler(this.rdoClient_CheckedChanged);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(550, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(20, 28);
            // 
            // rdoClient
            // 
            this.rdoClient.AutoSize = false;
            this.rdoClient.Checked = true;
            this.rdoClient.Dock = Wisej.Web.DockStyle.Left;
            this.rdoClient.Location = new System.Drawing.Point(384, 0);
            this.rdoClient.Name = "rdoClient";
            this.rdoClient.Size = new System.Drawing.Size(166, 28);
            this.rdoClient.TabIndex = 0;
            this.rdoClient.TabStop = true;
            this.rdoClient.Text = "Create message to client";
            this.rdoClient.Visible = false;
            this.rdoClient.CheckedChanged += new System.EventHandler(this.rdoClient_CheckedChanged);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(364, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(20, 28);
            // 
            // lblAddmiscnote
            // 
            this.lblAddmiscnote.Dock = Wisej.Web.DockStyle.Left;
            this.lblAddmiscnote.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAddmiscnote.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblAddmiscnote.Location = new System.Drawing.Point(15, 0);
            this.lblAddmiscnote.Name = "lblAddmiscnote";
            this.lblAddmiscnote.Size = new System.Drawing.Size(349, 28);
            this.lblAddmiscnote.TabIndex = 1;
            this.lblAddmiscnote.Text = "View All Misc. Messages";
            this.lblAddmiscnote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSendEmail
            // 
            this.pnlSendEmail.AppearanceKey = "panel-grdo";
            this.pnlSendEmail.Controls.Add(this.btnSendReminder);
            this.pnlSendEmail.Controls.Add(this.spacer4);
            this.pnlSendEmail.Controls.Add(this.btnEmailSend);
            this.pnlSendEmail.Controls.Add(this.spacer3);
            this.pnlSendEmail.Controls.Add(this.btnCreateMiscMsg);
            this.pnlSendEmail.Controls.Add(this.btnCancel);
            this.pnlSendEmail.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSendEmail.Location = new System.Drawing.Point(0, 489);
            this.pnlSendEmail.Name = "pnlSendEmail";
            this.pnlSendEmail.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlSendEmail.Size = new System.Drawing.Size(799, 35);
            this.pnlSendEmail.TabIndex = 4;
            // 
            // btnSendReminder
            // 
            this.btnSendReminder.Dock = Wisej.Web.DockStyle.Left;
            this.btnSendReminder.Location = new System.Drawing.Point(228, 5);
            this.btnSendReminder.Name = "btnSendReminder";
            this.btnSendReminder.Size = new System.Drawing.Size(222, 25);
            this.btnSendReminder.TabIndex = 9;
            this.btnSendReminder.Text = "Send Submission/&Reminder Email";
            this.btnSendReminder.Click += new System.EventHandler(this.btnSendReminder_Click);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(225, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // btnEmailSend
            // 
            this.btnEmailSend.Dock = Wisej.Web.DockStyle.Right;
            this.btnEmailSend.Location = new System.Drawing.Point(626, 5);
            this.btnEmailSend.Name = "btnEmailSend";
            this.btnEmailSend.Size = new System.Drawing.Size(80, 25);
            this.btnEmailSend.TabIndex = 5;
            this.btnEmailSend.Text = "Send &Email";
            this.btnEmailSend.Click += new System.EventHandler(this.btnEmailSend_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(706, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCreateMiscMsg
            // 
            this.btnCreateMiscMsg.Dock = Wisej.Web.DockStyle.Left;
            this.btnCreateMiscMsg.Location = new System.Drawing.Point(15, 5);
            this.btnCreateMiscMsg.Name = "btnCreateMiscMsg";
            this.btnCreateMiscMsg.Size = new System.Drawing.Size(210, 25);
            this.btnCreateMiscMsg.TabIndex = 9;
            this.btnCreateMiscMsg.Text = "Create a Misc. &Message to Client";
            this.btnCreateMiscMsg.Click += new System.EventHandler(this.btnCreateMiscMsg_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(709, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlMessageNdGrid);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 35);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(799, 454);
            this.pnlCompleteForm.TabIndex = 7;
            // 
            // gvtverifyDate
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtverifyDate.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtverifyDate.HeaderStyle = dataGridViewCellStyle6;
            this.gvtverifyDate.HeaderText = "Date";
            this.gvtverifyDate.Name = "gvtverifyDate";
            this.gvtverifyDate.ReadOnly = true;
            this.gvtverifyDate.Width = 90;
            // 
            // PIP0001SendMailForm
            // 
            this.ClientSize = new System.Drawing.Size(799, 524);
            this.Controls.Add(this.pnlCompleteForm);
            this.Controls.Add(this.pnlEmail);
            this.Controls.Add(this.pnlSendEmail);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIP0001SendMailForm";
            this.Text = "PIP0001SendMailForm";
            this.pnlEmail.ResumeLayout(false);
            this.pnlMessageNdGrid.ResumeLayout(false);
            this.pnlReview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwDocumentreview)).EndInit();
            this.pnlCreateMessage.ResumeLayout(false);
            this.pnlCreateMessage.PerformLayout();
            this.pnlMessagesLbl.ResumeLayout(false);
            this.pnlSendEmail.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Label lblEmail2;
        private Label lblConf2;
        private Label lblTokenNo;
        private Label lblEmailId;
        private Panel pnlEmail;
        private Panel pnlMessageNdGrid;
        private RadioButton rdoReview;
        private RadioButton rdoClient;
        private TextBox txtMessage;
        private Panel pnlSendEmail;
        private Button btnEmailSend;
        private Button btnCancel;
        private Panel pnlCreateMessage;
        private Panel pnlReview;
        private DataGridViewEx gvwDocumentreview;
        private DataGridViewTextBoxColumn gvtName;
        private DataGridViewTextBoxColumn gvtFileName;
        private DataGridViewTextBoxColumn gvtRemarks;
        private DataGridViewCheckBoxColumn gvchkselect;
        private DataGridViewTextBoxColumn gvtVerId;
        private DataGridViewTextBoxColumn gvtdocId;
        private DataGridViewTextBoxColumn gvtverstatus;
        private Button btnCreateMiscMsg;
        private Label lblAddmiscnote;
        private Button btnEmailHist;
        private Button btnSendReminder;
        private Panel pnlCompleteForm;
        private Panel pnlMessagesLbl;
        private Spacer spacer2;
        private Spacer spacer1;
        private Spacer spacer3;
        private Spacer spacer4;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtverifyDate;
    }
}