using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class HSS20001ClientSupplierForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS20001ClientSupplierForm));
            this.gvwSupplierDetails = new Wisej.Web.DataGridView();
            this.gvtSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSupplierType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtVendorName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtPayfor = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAccountNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtPrimaryCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtPayforcode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvPassFail = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gviEdit = new Wisej.Web.DataGridViewImageColumn();
            this.gvtSelected = new Wisej.Web.DataGridViewImageColumn();
            this.gviDel = new Wisej.Web.DataGridViewImageColumn();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.panel1 = new Wisej.Web.Panel();
            this.lblMsg = new Wisej.Web.Label();
            this.lblReverified = new Wisej.Web.Label();
            this.chkReverify = new Wisej.Web.CheckBox();
            this.lblTypeReq = new Wisej.Web.Label();
            this.lblSupplierType = new Wisej.Web.Label();
            this.cmbSupplierType = new Wisej.Web.ComboBox();
            this.txtPrimary = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblAccountReq = new Wisej.Web.Label();
            this.lblBillingReq = new Wisej.Web.Label();
            this.txtVendorName = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtVendorId = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblPayforReq = new Wisej.Web.Label();
            this.lblVendor = new Wisej.Web.Label();
            this.picAddClient = new Wisej.Web.PictureBox();
            this.lblvendorReq = new Wisej.Web.Label();
            this.txtLast = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.btnvendor = new Wisej.Web.Button();
            this.lblLast = new Wisej.Web.Label();
            this.lblFirst = new Wisej.Web.Label();
            this.txtFirst = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtAccountNo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblAccount = new Wisej.Web.Label();
            this.lblBillingName = new Wisej.Web.Label();
            this.cmbBilling = new Wisej.Web.ComboBox();
            this.cmbPayfor = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblPayfor = new Wisej.Web.Label();
            this.pnlAdd = new Wisej.Web.FlowLayoutPanel();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSupplierDetails)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddClient)).BeginInit();
            this.pnlAdd.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwSupplierDetails
            // 
            this.gvwSupplierDetails.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwSupplierDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwSupplierDetails.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwSupplierDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtSeq,
            this.gvtSupplierType,
            this.gvtVendorName,
            this.gvtPayfor,
            this.gvtAccountNo,
            this.gvtPrimaryCode,
            this.gvtPayforcode,
            this.gvPassFail,
            this.gviEdit,
            this.gvtSelected,
            this.gviDel});
            this.gvwSupplierDetails.ContextMenu = this.contextMenu1;
            this.gvwSupplierDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwSupplierDetails.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwSupplierDetails.Location = new System.Drawing.Point(0, 0);
            this.gvwSupplierDetails.Name = "gvwSupplierDetails";
            this.gvwSupplierDetails.ReadOnly = true;
            this.gvwSupplierDetails.RowHeadersVisible = false;
            this.gvwSupplierDetails.RowHeadersWidth = 10;
            this.gvwSupplierDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSupplierDetails.Size = new System.Drawing.Size(797, 237);
            this.gvwSupplierDetails.TabIndex = 0;
            this.gvwSupplierDetails.SelectionChanged += new System.EventHandler(this.gvwSupplierDetails_SelectionChanged);
            this.gvwSupplierDetails.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwSupplierDetails_CellClick);
            // 
            // gvtSeq
            // 
            this.gvtSeq.HeaderText = " ";
            this.gvtSeq.Name = "gvtSeq";
            this.gvtSeq.ReadOnly = true;
            this.gvtSeq.ShowInVisibilityMenu = false;
            this.gvtSeq.Visible = false;
            this.gvtSeq.Width = 10;
            // 
            // gvtSupplierType
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtSupplierType.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtSupplierType.HeaderStyle = dataGridViewCellStyle3;
            this.gvtSupplierType.HeaderText = "Supplier Type";
            this.gvtSupplierType.Name = "gvtSupplierType";
            this.gvtSupplierType.ReadOnly = true;
            this.gvtSupplierType.Width = 120;
            // 
            // gvtVendorName
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtVendorName.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtVendorName.HeaderStyle = dataGridViewCellStyle5;
            this.gvtVendorName.HeaderText = "Vendor Name";
            this.gvtVendorName.Name = "gvtVendorName";
            this.gvtVendorName.ReadOnly = true;
            this.gvtVendorName.Width = 280;
            // 
            // gvtPayfor
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtPayfor.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtPayfor.HeaderStyle = dataGridViewCellStyle7;
            this.gvtPayfor.HeaderText = "Pay for";
            this.gvtPayfor.Name = "gvtPayfor";
            this.gvtPayfor.ReadOnly = true;
            this.gvtPayfor.Width = 106;
            // 
            // gvtAccountNo
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtAccountNo.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtAccountNo.HeaderStyle = dataGridViewCellStyle9;
            this.gvtAccountNo.HeaderText = "Account Number";
            this.gvtAccountNo.Name = "gvtAccountNo";
            this.gvtAccountNo.ReadOnly = true;
            this.gvtAccountNo.Width = 110;
            // 
            // gvtPrimaryCode
            // 
            this.gvtPrimaryCode.HeaderText = " ";
            this.gvtPrimaryCode.Name = "gvtPrimaryCode";
            this.gvtPrimaryCode.ReadOnly = true;
            this.gvtPrimaryCode.ShowInVisibilityMenu = false;
            this.gvtPrimaryCode.Visible = false;
            // 
            // gvtPayforcode
            // 
            this.gvtPayforcode.HeaderText = "  ";
            this.gvtPayforcode.Name = "gvtPayforcode";
            this.gvtPayforcode.ReadOnly = true;
            this.gvtPayforcode.ShowInVisibilityMenu = false;
            this.gvtPayforcode.Visible = false;
            this.gvtPayforcode.Width = 20;
            // 
            // gvPassFail
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvPassFail.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvPassFail.HeaderStyle = dataGridViewCellStyle11;
            this.gvPassFail.HeaderText = "P/F";
            this.gvPassFail.Name = "gvPassFail";
            this.gvPassFail.ReadOnly = true;
            this.gvPassFail.Width = 60;
            // 
            // gviEdit
            // 
            this.gviEdit.CellImageSource = "captain-edit";
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.NullValue = null;
            this.gviEdit.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gviEdit.HeaderStyle = dataGridViewCellStyle13;
            this.gviEdit.HeaderText = "Edit";
            this.gviEdit.Name = "gviEdit";
            this.gviEdit.ReadOnly = true;
            this.gviEdit.ShowInVisibilityMenu = false;
            this.gviEdit.Width = 50;
            // 
            // gvtSelected
            // 
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.NullValue = null;
            this.gvtSelected.DefaultCellStyle = dataGridViewCellStyle14;
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtSelected.HeaderStyle = dataGridViewCellStyle15;
            this.gvtSelected.HeaderText = "Verified";
            this.gvtSelected.Name = "gvtSelected";
            this.gvtSelected.ReadOnly = true;
            this.gvtSelected.ShowInVisibilityMenu = false;
            this.gvtSelected.Visible = false;
            this.gvtSelected.Width = 60;
            // 
            // gviDel
            // 
            this.gviDel.CellImageSource = "captain-delete";
            dataGridViewCellStyle16.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.NullValue = null;
            this.gviDel.DefaultCellStyle = dataGridViewCellStyle16;
            dataGridViewCellStyle17.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gviDel.HeaderStyle = dataGridViewCellStyle17;
            this.gviDel.HeaderText = "Del";
            this.gviDel.Name = "gviDel";
            this.gviDel.ReadOnly = true;
            this.gviDel.ShowInVisibilityMenu = false;
            this.gviDel.Width = 50;
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(251, 251, 251);
            this.panel1.Controls.Add(this.lblMsg);
            this.panel1.Controls.Add(this.lblReverified);
            this.panel1.Controls.Add(this.chkReverify);
            this.panel1.Controls.Add(this.lblTypeReq);
            this.panel1.Controls.Add(this.lblSupplierType);
            this.panel1.Controls.Add(this.cmbSupplierType);
            this.panel1.Controls.Add(this.txtPrimary);
            this.panel1.Controls.Add(this.lblAccountReq);
            this.panel1.Controls.Add(this.lblBillingReq);
            this.panel1.Controls.Add(this.txtVendorName);
            this.panel1.Controls.Add(this.txtVendorId);
            this.panel1.Controls.Add(this.lblPayforReq);
            this.panel1.Controls.Add(this.lblVendor);
            this.panel1.Controls.Add(this.picAddClient);
            this.panel1.Controls.Add(this.lblvendorReq);
            this.panel1.Controls.Add(this.txtLast);
            this.panel1.Controls.Add(this.btnvendor);
            this.panel1.Controls.Add(this.lblLast);
            this.panel1.Controls.Add(this.lblFirst);
            this.panel1.Controls.Add(this.txtFirst);
            this.panel1.Controls.Add(this.txtAccountNo);
            this.panel1.Controls.Add(this.lblAccount);
            this.panel1.Controls.Add(this.lblBillingName);
            this.panel1.Controls.Add(this.cmbBilling);
            this.panel1.Controls.Add(this.cmbPayfor);
            this.panel1.Controls.Add(this.lblPayfor);
            this.panel1.CssStyle = "border-top:1px dotted #f2f2f2;";
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.panel1.Location = new System.Drawing.Point(0, 237);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 123);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMsg.ForeColor = System.Drawing.Color.FromName("@invalid");
            this.lblMsg.Location = new System.Drawing.Point(13, 102);
            this.lblMsg.MaximumSize = new System.Drawing.Size(0, 21);
            this.lblMsg.MinimumSize = new System.Drawing.Size(535, 21);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(535, 21);
            this.lblMsg.TabIndex = 32;
            this.lblMsg.Text = "label1";
            this.lblMsg.Visible = false;
            // 
            // lblReverified
            // 
            this.lblReverified.AutoSize = true;
            this.lblReverified.Location = new System.Drawing.Point(271, 109);
            this.lblReverified.Name = "lblReverified";
            this.lblReverified.Size = new System.Drawing.Size(11, 14);
            this.lblReverified.TabIndex = 31;
            this.lblReverified.Text = " .";
            this.lblReverified.Visible = false;
            // 
            // chkReverify
            // 
            this.chkReverify.Enabled = false;
            this.chkReverify.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkReverify.Location = new System.Drawing.Point(93, 100);
            this.chkReverify.Name = "chkReverify";
            this.chkReverify.Size = new System.Drawing.Size(92, 21);
            this.chkReverify.TabIndex = 30;
            this.chkReverify.Text = "Re Verified";
            this.chkReverify.Visible = false;
            // 
            // lblTypeReq
            // 
            this.lblTypeReq.AutoSize = true;
            this.lblTypeReq.ForeColor = System.Drawing.Color.FromArgb(246, 255, 0, 0);
            this.lblTypeReq.Location = new System.Drawing.Point(528, 6);
            this.lblTypeReq.Name = "lblTypeReq";
            this.lblTypeReq.Size = new System.Drawing.Size(9, 14);
            this.lblTypeReq.TabIndex = 28;
            this.lblTypeReq.Text = "*";
            this.lblTypeReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSupplierType
            // 
            this.lblSupplierType.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSupplierType.Location = new System.Drawing.Point(539, 10);
            this.lblSupplierType.Name = "lblSupplierType";
            this.lblSupplierType.Size = new System.Drawing.Size(33, 18);
            this.lblSupplierType.TabIndex = 0;
            this.lblSupplierType.Text = "Type";
            // 
            // cmbSupplierType
            // 
            this.cmbSupplierType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSupplierType.Enabled = false;
            this.cmbSupplierType.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbSupplierType.FormattingEnabled = true;
            this.cmbSupplierType.Location = new System.Drawing.Point(633, 7);
            this.cmbSupplierType.Name = "cmbSupplierType";
            this.cmbSupplierType.Size = new System.Drawing.Size(119, 25);
            this.cmbSupplierType.TabIndex = 3;
            this.cmbSupplierType.SelectedIndexChanged += new System.EventHandler(this.cmbSupplierType_SelectedIndexChanged);
            // 
            // txtPrimary
            // 
            this.txtPrimary.Enabled = false;
            this.txtPrimary.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPrimary.Location = new System.Drawing.Point(577, 7);
            this.txtPrimary.Name = "txtPrimary";
            this.txtPrimary.Size = new System.Drawing.Size(50, 25);
            this.txtPrimary.TabIndex = 29;
            this.txtPrimary.Visible = false;
            // 
            // lblAccountReq
            // 
            this.lblAccountReq.AutoSize = true;
            this.lblAccountReq.Font = new System.Drawing.Font("@labelText", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountReq.ForeColor = System.Drawing.Color.FromArgb(246, 255, 0, 0);
            this.lblAccountReq.Location = new System.Drawing.Point(266, 8);
            this.lblAccountReq.Name = "lblAccountReq";
            this.lblAccountReq.Size = new System.Drawing.Size(10, 15);
            this.lblAccountReq.TabIndex = 28;
            this.lblAccountReq.Text = "*";
            this.lblAccountReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblAccountReq.Visible = false;
            // 
            // lblBillingReq
            // 
            this.lblBillingReq.AutoSize = true;
            this.lblBillingReq.Font = new System.Drawing.Font("@labelText", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBillingReq.ForeColor = System.Drawing.Color.FromArgb(246, 255, 0, 0);
            this.lblBillingReq.Location = new System.Drawing.Point(3, 70);
            this.lblBillingReq.Name = "lblBillingReq";
            this.lblBillingReq.Size = new System.Drawing.Size(10, 15);
            this.lblBillingReq.TabIndex = 28;
            this.lblBillingReq.Text = "*";
            this.lblBillingReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblBillingReq.Visible = false;
            // 
            // txtVendorName
            // 
            this.txtVendorName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtVendorName.Location = new System.Drawing.Point(338, 38);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.ReadOnly = true;
            this.txtVendorName.Size = new System.Drawing.Size(414, 25);
            this.txtVendorName.TabIndex = 6;
            // 
            // txtVendorId
            // 
            this.txtVendorId.Enabled = false;
            this.txtVendorId.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtVendorId.Location = new System.Drawing.Point(93, 38);
            this.txtVendorId.MaxLength = 5;
            this.txtVendorId.Name = "txtVendorId";
            this.txtVendorId.Size = new System.Drawing.Size(160, 25);
            this.txtVendorId.TabIndex = 4;
            this.txtVendorId.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtVendorId.TextChanged += new System.EventHandler(this.txtVendorId_TextChanged);
            // 
            // lblPayforReq
            // 
            this.lblPayforReq.AutoSize = true;
            this.lblPayforReq.ForeColor = System.Drawing.Color.FromArgb(246, 255, 0, 0);
            this.lblPayforReq.Location = new System.Drawing.Point(4, 8);
            this.lblPayforReq.Name = "lblPayforReq";
            this.lblPayforReq.Size = new System.Drawing.Size(9, 14);
            this.lblPayforReq.TabIndex = 28;
            this.lblPayforReq.Text = "*";
            this.lblPayforReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblPayforReq.Visible = false;
            // 
            // lblVendor
            // 
            this.lblVendor.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVendor.Location = new System.Drawing.Point(13, 42);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(45, 19);
            this.lblVendor.TabIndex = 0;
            this.lblVendor.Text = "Vendor";
            // 
            // picAddClient
            // 
            this.picAddClient.Cursor = Wisej.Web.Cursors.Hand;
            this.picAddClient.ImageSource = "captain-add";
            this.picAddClient.Location = new System.Drawing.Point(753, 8);
            this.picAddClient.Name = "picAddClient";
            this.picAddClient.Size = new System.Drawing.Size(32, 23);
            this.picAddClient.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAddClient.Click += new System.EventHandler(this.picAddScale_Click);
            // 
            // lblvendorReq
            // 
            this.lblvendorReq.AutoSize = true;
            this.lblvendorReq.Font = new System.Drawing.Font("@labelText", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblvendorReq.ForeColor = System.Drawing.Color.FromArgb(246, 255, 0, 0);
            this.lblvendorReq.Location = new System.Drawing.Point(3, 39);
            this.lblvendorReq.Name = "lblvendorReq";
            this.lblvendorReq.Size = new System.Drawing.Size(10, 15);
            this.lblvendorReq.TabIndex = 28;
            this.lblvendorReq.Text = "*";
            this.lblvendorReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblvendorReq.Visible = false;
            // 
            // txtLast
            // 
            this.txtLast.Enabled = false;
            this.txtLast.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLast.Location = new System.Drawing.Point(577, 69);
            this.txtLast.MaxLength = 20;
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(173, 25);
            this.txtLast.TabIndex = 9;
            // 
            // btnvendor
            // 
            this.btnvendor.Display = Wisej.Web.Display.Icon;
            this.btnvendor.Enabled = false;
            this.btnvendor.ImageSource = "captain-more";
            this.btnvendor.Location = new System.Drawing.Point(301, 38);
            this.btnvendor.Name = "btnvendor";
            this.btnvendor.Size = new System.Drawing.Size(30, 23);
            this.btnvendor.TabIndex = 5;
            this.btnvendor.Text = "...";
            this.btnvendor.Click += new System.EventHandler(this.btnvendor_Click);
            // 
            // lblLast
            // 
            this.lblLast.AutoSize = true;
            this.lblLast.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLast.Location = new System.Drawing.Point(543, 73);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(27, 14);
            this.lblLast.TabIndex = 11;
            this.lblLast.Text = "Last";
            // 
            // lblFirst
            // 
            this.lblFirst.AutoSize = true;
            this.lblFirst.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFirst.Location = new System.Drawing.Point(304, 73);
            this.lblFirst.Name = "lblFirst";
            this.lblFirst.Size = new System.Drawing.Size(28, 14);
            this.lblFirst.TabIndex = 11;
            this.lblFirst.Text = "First";
            // 
            // txtFirst
            // 
            this.txtFirst.Enabled = false;
            this.txtFirst.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFirst.Location = new System.Drawing.Point(338, 69);
            this.txtFirst.MaxLength = 20;
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(166, 25);
            this.txtFirst.TabIndex = 8;
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Enabled = false;
            this.txtAccountNo.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAccountNo.Location = new System.Drawing.Point(338, 7);
            this.txtAccountNo.MaxLength = 20;
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.RightToLeft = Wisej.Web.RightToLeft.No;
            this.txtAccountNo.Size = new System.Drawing.Size(166, 25);
            this.txtAccountNo.TabIndex = 2;
            this.txtAccountNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtAccountNo.Leave += new System.EventHandler(this.txtAccountNo_Leave);
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccount.Location = new System.Drawing.Point(276, 11);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(55, 14);
            this.lblAccount.TabIndex = 11;
            this.lblAccount.Text = "Account#";
            // 
            // lblBillingName
            // 
            this.lblBillingName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBillingName.Location = new System.Drawing.Point(12, 73);
            this.lblBillingName.Name = "lblBillingName";
            this.lblBillingName.Size = new System.Drawing.Size(77, 19);
            this.lblBillingName.TabIndex = 11;
            this.lblBillingName.Text = "Billing Name";
            // 
            // cmbBilling
            // 
            this.cmbBilling.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbBilling.Enabled = false;
            this.cmbBilling.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbBilling.FormattingEnabled = true;
            this.cmbBilling.Location = new System.Drawing.Point(93, 69);
            this.cmbBilling.Name = "cmbBilling";
            this.cmbBilling.Size = new System.Drawing.Size(160, 25);
            this.cmbBilling.TabIndex = 7;
            this.cmbBilling.SelectedIndexChanged += new System.EventHandler(this.cmbBilling_SelectedIndexChanged);
            // 
            // cmbPayfor
            // 
            this.cmbPayfor.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbPayfor.Enabled = false;
            this.cmbPayfor.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbPayfor.FormattingEnabled = true;
            this.cmbPayfor.Location = new System.Drawing.Point(93, 7);
            this.cmbPayfor.Name = "cmbPayfor";
            this.cmbPayfor.Size = new System.Drawing.Size(160, 25);
            this.cmbPayfor.TabIndex = 1;
            this.cmbPayfor.SelectedIndexChanged += new System.EventHandler(this.cmbPayfor_SelectedIndexChanged);
            // 
            // lblPayfor
            // 
            this.lblPayfor.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPayfor.Location = new System.Drawing.Point(13, 11);
            this.lblPayfor.Name = "lblPayfor";
            this.lblPayfor.Size = new System.Drawing.Size(47, 18);
            this.lblPayfor.TabIndex = 11;
            this.lblPayfor.Text = "Pay For";
            // 
            // pnlAdd
            // 
            this.pnlAdd.AppearanceKey = "panel-grdo";
            this.pnlAdd.BackColor = System.Drawing.Color.FromName("@control");
            this.pnlAdd.Controls.Add(this.btnCancel);
            this.pnlAdd.Controls.Add(this.btnSave);
            this.pnlAdd.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlAdd.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.pnlAdd.Location = new System.Drawing.Point(0, 360);
            this.pnlAdd.Name = "pnlAdd";
            this.pnlAdd.Size = new System.Drawing.Size(797, 35);
            this.pnlAdd.TabIndex = 3;
            this.pnlAdd.TabStop = true;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.AutoSize = true;
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(730, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.AutoSize = true;
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(670, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwSupplierDetails);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(797, 237);
            this.panel2.TabIndex = 4;
            // 
            // HSS20001ClientSupplierForm
            // 
            this.ClientSize = new System.Drawing.Size(797, 395);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlAdd);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS20001ClientSupplierForm";
            this.Text = "HSS20001ClientSupplierForm";
            this.Load += new System.EventHandler(this.HSS20001ClientSupplierForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvwSupplierDetails)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddClient)).EndInit();
            this.pnlAdd.ResumeLayout(false);
            this.pnlAdd.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvwSupplierDetails;
        private DataGridViewTextBoxColumn gvtSupplierType;
        private DataGridViewTextBoxColumn gvtVendorName;
        private DataGridViewTextBoxColumn gvtPayfor;
        private DataGridViewTextBoxColumn gvtAccountNo;
        private DataGridViewImageColumn gviEdit;
        private DataGridViewImageColumn gviDel;
        private Panel panel1;
        private FlowLayoutPanel pnlAdd;
        private TextBoxWithValidation txtLast;
        private Label lblLast;
        private Label lblFirst;
        private TextBoxWithValidation txtFirst;
        private TextBoxWithValidation txtAccountNo;
        private Label lblAccount;
        private Label lblBillingName;
        private ComboBox cmbBilling;
        private ComboBoxEx cmbPayfor;
        private Label lblPayfor;
        private Button btnvendor;
        private TextBoxWithValidation txtVendorId;
        private Label lblVendor;
        private TextBoxWithValidation txtVendorName;
        private Button btnCancel;
        private Button btnSave;
        private PictureBox picAddClient;
        private Label lblPayforReq;
        private Label lblvendorReq;
        private DataGridViewTextBoxColumn gvtSeq;
        private ContextMenu contextMenu1;
        private DataGridViewTextBoxColumn gvtPrimaryCode;
        private Label lblAccountReq;
        private Label lblBillingReq;
        private TextBoxWithValidation txtPrimary;
        private DataGridViewTextBoxColumn gvtPayforcode;
        private Label lblTypeReq;
        private Label lblSupplierType;
        private ComboBox cmbSupplierType;
        private Label lblReverified;
        private CheckBox chkReverify;
        private DataGridViewImageColumn gvtSelected;
        private DataGridViewTextBoxColumn gvPassFail;
        private Label lblMsg;
        private Panel panel2;
    }
}