using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PIP00001DocumentViewForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIP00001DocumentViewForm));
            this.chkHouseHold = new Wisej.Web.CheckBox();
            this.pnlLeftPIP = new Wisej.Web.Panel();
            this.pnlgvwPIPFiles = new Wisej.Web.Panel();
            this.gvwPIPFiles = new Wisej.Web.DataGridView();
            this.gvchkSelect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtImageType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtPIPFilename = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSecurity = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtIMGTYPE = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlPIP = new Wisej.Web.Panel();
            this.lblpipmsg = new Wisej.Web.Label();
            this.pnlRightCAPTAIN = new Wisej.Web.Panel();
            this.pnlgvwCaptaindoc = new Wisej.Web.Panel();
            this.gvwCaptaindoc = new Wisej.Web.DataGridView();
            this.gvtCapFileName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlCAPTAIN = new Wisej.Web.Panel();
            this.lblCAPTAIN = new Wisej.Web.Label();
            this.pnlButtons = new Wisej.Web.Panel();
            this.btnUpload = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlChbHouse = new Wisej.Web.Panel();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlLeftPIP.SuspendLayout();
            this.pnlgvwPIPFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwPIPFiles)).BeginInit();
            this.pnlPIP.SuspendLayout();
            this.pnlRightCAPTAIN.SuspendLayout();
            this.pnlgvwCaptaindoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCaptaindoc)).BeginInit();
            this.pnlCAPTAIN.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlChbHouse.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkHouseHold
            // 
            this.chkHouseHold.AutoSize = false;
            this.chkHouseHold.CheckState = Wisej.Web.CheckState.Checked;
            this.chkHouseHold.Location = new System.Drawing.Point(532, 8);
            this.chkHouseHold.Name = "chkHouseHold";
            this.chkHouseHold.Size = new System.Drawing.Size(183, 20);
            this.chkHouseHold.TabIndex = 2;
            this.chkHouseHold.Text = "Household Member Images";
            this.chkHouseHold.CheckedChanged += new System.EventHandler(this.chkHouseHold_CheckedChanged);
            // 
            // pnlLeftPIP
            // 
            this.pnlLeftPIP.Controls.Add(this.pnlgvwPIPFiles);
            this.pnlLeftPIP.Controls.Add(this.pnlPIP);
            this.pnlLeftPIP.Dock = Wisej.Web.DockStyle.Left;
            this.pnlLeftPIP.Location = new System.Drawing.Point(0, 36);
            this.pnlLeftPIP.Name = "pnlLeftPIP";
            this.pnlLeftPIP.Padding = new Wisej.Web.Padding(5);
            this.pnlLeftPIP.Size = new System.Drawing.Size(528, 518);
            this.pnlLeftPIP.TabIndex = 5;
            // 
            // pnlgvwPIPFiles
            // 
            this.pnlgvwPIPFiles.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlgvwPIPFiles.Controls.Add(this.gvwPIPFiles);
            this.pnlgvwPIPFiles.CssStyle = "border-radius: 0 0 15px 15px;";
            this.pnlgvwPIPFiles.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwPIPFiles.Location = new System.Drawing.Point(5, 31);
            this.pnlgvwPIPFiles.Name = "pnlgvwPIPFiles";
            this.pnlgvwPIPFiles.Size = new System.Drawing.Size(518, 482);
            this.pnlgvwPIPFiles.TabIndex = 8;
            // 
            // gvwPIPFiles
            // 
            this.gvwPIPFiles.AllowUserToResizeColumns = false;
            this.gvwPIPFiles.AllowUserToResizeRows = false;
            this.gvwPIPFiles.BackColor = System.Drawing.Color.White;
            this.gvwPIPFiles.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvchkSelect,
            this.gvtImageType,
            this.gvtPIPFilename,
            this.gvtSecurity,
            this.gvtIMGTYPE});
            this.gvwPIPFiles.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwPIPFiles.Location = new System.Drawing.Point(0, 0);
            this.gvwPIPFiles.MultiSelect = false;
            this.gvwPIPFiles.Name = "gvwPIPFiles";
            this.gvwPIPFiles.RowHeadersWidth = 14;
            this.gvwPIPFiles.RowTemplate.Height = 26;
            this.gvwPIPFiles.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwPIPFiles.Size = new System.Drawing.Size(516, 480);
            this.gvwPIPFiles.TabIndex = 3;
            // 
            // gvchkSelect
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.gvchkSelect.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvchkSelect.HeaderStyle = dataGridViewCellStyle2;
            this.gvchkSelect.HeaderText = "  ";
            this.gvchkSelect.Name = "gvchkSelect";
            this.gvchkSelect.ShowInVisibilityMenu = false;
            this.gvchkSelect.Width = 35;
            // 
            // gvtImageType
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtImageType.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtImageType.HeaderStyle = dataGridViewCellStyle4;
            this.gvtImageType.HeaderText = "Document Type";
            this.gvtImageType.Name = "gvtImageType";
            this.gvtImageType.ReadOnly = true;
            this.gvtImageType.Width = 170;
            // 
            // gvtPIPFilename
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtPIPFilename.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtPIPFilename.HeaderStyle = dataGridViewCellStyle6;
            this.gvtPIPFilename.HeaderText = "Name of the Document";
            this.gvtPIPFilename.Name = "gvtPIPFilename";
            this.gvtPIPFilename.ReadOnly = true;
            this.gvtPIPFilename.Width = 250;
            // 
            // gvtSecurity
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSecurity.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSecurity.HeaderStyle = dataGridViewCellStyle8;
            this.gvtSecurity.HeaderText = "gvtSecurity";
            this.gvtSecurity.Name = "gvtSecurity";
            this.gvtSecurity.ShowInVisibilityMenu = false;
            this.gvtSecurity.Visible = false;
            this.gvtSecurity.Width = 10;
            // 
            // gvtIMGTYPE
            // 
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtIMGTYPE.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtIMGTYPE.HeaderStyle = dataGridViewCellStyle10;
            this.gvtIMGTYPE.HeaderText = "gvtIMGTYPE";
            this.gvtIMGTYPE.Name = "gvtIMGTYPE";
            this.gvtIMGTYPE.ShowInVisibilityMenu = false;
            this.gvtIMGTYPE.Visible = false;
            this.gvtIMGTYPE.Width = 10;
            // 
            // pnlPIP
            // 
            this.pnlPIP.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            this.pnlPIP.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlPIP.Controls.Add(this.lblpipmsg);
            this.pnlPIP.CssStyle = "border-radius: 15px 15px 0 0;";
            this.pnlPIP.Dock = Wisej.Web.DockStyle.Top;
            this.pnlPIP.Location = new System.Drawing.Point(5, 5);
            this.pnlPIP.Name = "pnlPIP";
            this.pnlPIP.Size = new System.Drawing.Size(518, 26);
            this.pnlPIP.TabIndex = 7;
            // 
            // lblpipmsg
            // 
            this.lblpipmsg.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblpipmsg.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblpipmsg.Location = new System.Drawing.Point(15, 5);
            this.lblpipmsg.Name = "lblpipmsg";
            this.lblpipmsg.Size = new System.Drawing.Size(106, 16);
            this.lblpipmsg.TabIndex = 6;
            this.lblpipmsg.Text = "Documents in PIP";
            // 
            // pnlRightCAPTAIN
            // 
            this.pnlRightCAPTAIN.Controls.Add(this.pnlgvwCaptaindoc);
            this.pnlRightCAPTAIN.Controls.Add(this.spacer2);
            this.pnlRightCAPTAIN.Controls.Add(this.pnlCAPTAIN);
            this.pnlRightCAPTAIN.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlRightCAPTAIN.Location = new System.Drawing.Point(528, 36);
            this.pnlRightCAPTAIN.Name = "pnlRightCAPTAIN";
            this.pnlRightCAPTAIN.Padding = new Wisej.Web.Padding(5);
            this.pnlRightCAPTAIN.Size = new System.Drawing.Size(534, 518);
            this.pnlRightCAPTAIN.TabIndex = 5;
            // 
            // pnlgvwCaptaindoc
            // 
            this.pnlgvwCaptaindoc.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlgvwCaptaindoc.Controls.Add(this.gvwCaptaindoc);
            this.pnlgvwCaptaindoc.CssStyle = "border-radius: 0 0 15px 15px;";
            this.pnlgvwCaptaindoc.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwCaptaindoc.Location = new System.Drawing.Point(8, 31);
            this.pnlgvwCaptaindoc.Name = "pnlgvwCaptaindoc";
            this.pnlgvwCaptaindoc.Size = new System.Drawing.Size(521, 482);
            this.pnlgvwCaptaindoc.TabIndex = 8;
            // 
            // gvwCaptaindoc
            // 
            this.gvwCaptaindoc.AllowUserToResizeColumns = false;
            this.gvwCaptaindoc.AllowUserToResizeRows = false;
            this.gvwCaptaindoc.BackColor = System.Drawing.Color.White;
            this.gvwCaptaindoc.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCapFileName});
            this.gvwCaptaindoc.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCaptaindoc.Location = new System.Drawing.Point(0, 0);
            this.gvwCaptaindoc.MultiSelect = false;
            this.gvwCaptaindoc.Name = "gvwCaptaindoc";
            this.gvwCaptaindoc.ReadOnly = true;
            this.gvwCaptaindoc.RowHeadersWidth = 14;
            this.gvwCaptaindoc.RowTemplate.Height = 26;
            this.gvwCaptaindoc.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCaptaindoc.ShowColumnVisibilityMenu = false;
            this.gvwCaptaindoc.Size = new System.Drawing.Size(519, 480);
            this.gvwCaptaindoc.TabIndex = 3;
            // 
            // gvtCapFileName
            // 
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCapFileName.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCapFileName.HeaderStyle = dataGridViewCellStyle12;
            this.gvtCapFileName.HeaderText = "Name of the Document";
            this.gvtCapFileName.Name = "gvtCapFileName";
            this.gvtCapFileName.ReadOnly = true;
            this.gvtCapFileName.Width = 480;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(5, 31);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 482);
            // 
            // pnlCAPTAIN
            // 
            this.pnlCAPTAIN.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            this.pnlCAPTAIN.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.pnlCAPTAIN.Controls.Add(this.lblCAPTAIN);
            this.pnlCAPTAIN.CssStyle = "border-radius: 15px 15px 0 0;";
            this.pnlCAPTAIN.Dock = Wisej.Web.DockStyle.Top;
            this.pnlCAPTAIN.Location = new System.Drawing.Point(5, 5);
            this.pnlCAPTAIN.Name = "pnlCAPTAIN";
            this.pnlCAPTAIN.Size = new System.Drawing.Size(524, 26);
            this.pnlCAPTAIN.TabIndex = 7;
            // 
            // lblCAPTAIN
            // 
            this.lblCAPTAIN.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblCAPTAIN.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblCAPTAIN.Location = new System.Drawing.Point(25, 5);
            this.lblCAPTAIN.Name = "lblCAPTAIN";
            this.lblCAPTAIN.Size = new System.Drawing.Size(139, 17);
            this.lblCAPTAIN.TabIndex = 6;
            this.lblCAPTAIN.Text = "Documents in CAPTAIN";
            // 
            // pnlButtons
            // 
            this.pnlButtons.AppearanceKey = "panel-grdo";
            this.pnlButtons.Controls.Add(this.btnUpload);
            this.pnlButtons.Controls.Add(this.spacer1);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 554);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlButtons.Size = new System.Drawing.Size(1062, 35);
            this.pnlButtons.TabIndex = 7;
            // 
            // btnUpload
            // 
            this.btnUpload.Dock = Wisej.Web.DockStyle.Right;
            this.btnUpload.Location = new System.Drawing.Point(844, 5);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(125, 25);
            this.btnUpload.TabIndex = 7;
            this.btnUpload.Tag = "";
            this.btnUpload.Text = "&Move to CAPTAIN";
            this.btnUpload.Visible = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(969, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(972, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlChbHouse
            // 
            this.pnlChbHouse.Controls.Add(this.chkHouseHold);
            this.pnlChbHouse.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChbHouse.Location = new System.Drawing.Point(0, 0);
            this.pnlChbHouse.Name = "pnlChbHouse";
            this.pnlChbHouse.Size = new System.Drawing.Size(1062, 36);
            this.pnlChbHouse.TabIndex = 7;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlRightCAPTAIN);
            this.pnlCompleteForm.Controls.Add(this.pnlLeftPIP);
            this.pnlCompleteForm.Controls.Add(this.pnlButtons);
            this.pnlCompleteForm.Controls.Add(this.pnlChbHouse);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(1062, 589);
            this.pnlCompleteForm.TabIndex = 8;
            // 
            // PIP00001DocumentViewForm
            // 
            this.ClientSize = new System.Drawing.Size(1062, 589);
            this.Controls.Add(this.pnlCompleteForm);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIP00001DocumentViewForm";
            this.Text = "PIP00001DocumentViewForm";
            this.pnlLeftPIP.ResumeLayout(false);
            this.pnlgvwPIPFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwPIPFiles)).EndInit();
            this.pnlPIP.ResumeLayout(false);
            this.pnlRightCAPTAIN.ResumeLayout(false);
            this.pnlgvwCaptaindoc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCaptaindoc)).EndInit();
            this.pnlCAPTAIN.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlChbHouse.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private CheckBox chkHouseHold;
        private Panel pnlLeftPIP;
        private Panel pnlRightCAPTAIN;
        private DataGridView gvwPIPFiles;
        private DataGridView gvwCaptaindoc;
        private Label lblpipmsg;
        private Label lblCAPTAIN;
        private Panel pnlButtons;
        private Panel pnlChbHouse;
        private DataGridViewCheckBoxColumn gvchkSelect;
        private DataGridViewTextBoxColumn gvtImageType;
        private DataGridViewTextBoxColumn gvtPIPFilename;
        private DataGridViewTextBoxColumn gvtCapFileName;
        private Button btnUpload;
        private Button btnCancel;
        private DataGridViewTextBoxColumn gvtSecurity;
        private DataGridViewTextBoxColumn gvtIMGTYPE;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
        private Panel pnlPIP;
        private Panel pnlCAPTAIN;
        private Panel pnlgvwPIPFiles;
        private Panel pnlgvwCaptaindoc;
        private Spacer spacer2;
    }
}