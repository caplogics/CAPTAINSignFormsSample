using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ScreenControlAssignmentForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenControlAssignmentForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwCateDetails = new Wisej.Web.Panel();
            this.gvwCateDetails = new Wisej.Web.DataGridView();
            this.gvtCategory = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCatesel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtCateCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvt_Sel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvt_MSG = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlScreenAndHie = new Wisej.Web.Panel();
            this.CmbScreen = new Wisej.Web.ComboBox();
            this.lblScreenName = new Wisej.Web.Label();
            this.lblReqHie = new Wisej.Web.Label();
            this.lblHierarchy = new Wisej.Web.Label();
            this.PBHierarchy = new Wisej.Web.PictureBox();
            this.TxtHieDeSC = new Wisej.Web.TextBox();
            this.TxtHierarchy = new Wisej.Web.TextBox();
            this.pnlUpdatebtn = new Wisej.Web.Panel();
            this.BtnUpdate = new Wisej.Web.Button();
            this.gvwCateHierchy = new Wisej.Web.DataGridView();
            this.gvtCatHierchy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtHierchyDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtcateHierarkey = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwCateDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateDetails)).BeginInit();
            this.pnlScreenAndHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBHierarchy)).BeginInit();
            this.pnlUpdatebtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateHierchy)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwCateDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlScreenAndHie);
            this.pnlCompleteForm.Controls.Add(this.pnlUpdatebtn);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(983, 479);
            this.pnlCompleteForm.TabIndex = 3;
            // 
            // pnlgvwCateDetails
            // 
            this.pnlgvwCateDetails.Controls.Add(this.gvwCateDetails);
            this.pnlgvwCateDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwCateDetails.Location = new System.Drawing.Point(0, 75);
            this.pnlgvwCateDetails.Name = "pnlgvwCateDetails";
            this.pnlgvwCateDetails.Size = new System.Drawing.Size(983, 369);
            this.pnlgvwCateDetails.TabIndex = 11;
            // 
            // gvwCateDetails
            // 
            this.gvwCateDetails.AllowUserToResizeColumns = false;
            this.gvwCateDetails.AllowUserToResizeRows = false;
            this.gvwCateDetails.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwCateDetails.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCateDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwCateDetails.ColumnHeadersHeight = 35;
            this.gvwCateDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCategory,
            this.gvtCatesel,
            this.gvtCateCode,
            this.gvt_Sel,
            this.gvt_MSG});
            this.gvwCateDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCateDetails.Location = new System.Drawing.Point(0, 0);
            this.gvwCateDetails.MultiSelect = false;
            this.gvwCateDetails.Name = "gvwCateDetails";
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCateDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gvwCateDetails.RowHeadersWidth = 14;
            this.gvwCateDetails.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwCateDetails.RowTemplate.Height = 20;
            this.gvwCateDetails.Size = new System.Drawing.Size(983, 369);
            this.gvwCateDetails.TabIndex = 0;
            // 
            // gvtCategory
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtCategory.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCategory.HeaderStyle = dataGridViewCellStyle3;
            this.gvtCategory.HeaderText = "Screen";
            this.gvtCategory.Name = "gvtCategory";
            this.gvtCategory.ReadOnly = true;
            this.gvtCategory.Width = 200;
            // 
            // gvtCatesel
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = false;
            this.gvtCatesel.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCatesel.HeaderStyle = dataGridViewCellStyle5;
            this.gvtCatesel.HeaderText = "Message Control Enabled";
            this.gvtCatesel.Name = "gvtCatesel";
            this.gvtCatesel.Width = 120;
            // 
            // gvtCateCode
            // 
            this.gvtCateCode.HeaderText = "gvtCateCode";
            this.gvtCateCode.Name = "gvtCateCode";
            this.gvtCateCode.ShowInVisibilityMenu = false;
            this.gvtCateCode.Visible = false;
            this.gvtCateCode.Width = 10;
            // 
            // gvt_Sel
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = false;
            this.gvt_Sel.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.TopCenter;
            this.gvt_Sel.HeaderStyle = dataGridViewCellStyle7;
            this.gvt_Sel.HeaderText = "Allow Add/Edit";
            this.gvt_Sel.Name = "gvt_Sel";
            this.gvt_Sel.Width = 65;
            // 
            // gvt_MSG
            // 
            dataGridViewCellStyle8.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvt_MSG.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvt_MSG.HeaderStyle = dataGridViewCellStyle9;
            this.gvt_MSG.HeaderText = "Message";
            this.gvt_MSG.Name = "gvt_MSG";
            this.gvt_MSG.Width = 580;
            // 
            // pnlScreenAndHie
            // 
            this.pnlScreenAndHie.Controls.Add(this.CmbScreen);
            this.pnlScreenAndHie.Controls.Add(this.lblScreenName);
            this.pnlScreenAndHie.Controls.Add(this.lblReqHie);
            this.pnlScreenAndHie.Controls.Add(this.lblHierarchy);
            this.pnlScreenAndHie.Controls.Add(this.PBHierarchy);
            this.pnlScreenAndHie.Controls.Add(this.TxtHieDeSC);
            this.pnlScreenAndHie.Controls.Add(this.TxtHierarchy);
            this.pnlScreenAndHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnlScreenAndHie.Location = new System.Drawing.Point(0, 0);
            this.pnlScreenAndHie.Name = "pnlScreenAndHie";
            this.pnlScreenAndHie.Size = new System.Drawing.Size(983, 75);
            this.pnlScreenAndHie.TabIndex = 10;
            // 
            // CmbScreen
            // 
            this.CmbScreen.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbScreen.Enabled = false;
            this.CmbScreen.FormattingEnabled = true;
            this.CmbScreen.Items.AddRange(new object[] {
            "Client Intake                                             - CASE2001",
            "Income Verification                                   - CASE2003",
            "CASINCOM  - Income Entry",
            "CASE4006   - Service/Activity Details"});
            this.CmbScreen.Location = new System.Drawing.Point(110, 11);
            this.CmbScreen.Name = "CmbScreen";
            this.CmbScreen.Size = new System.Drawing.Size(349, 25);
            this.CmbScreen.TabIndex = 5;
            // 
            // lblScreenName
            // 
            this.lblScreenName.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblScreenName.Location = new System.Drawing.Point(15, 15);
            this.lblScreenName.Name = "lblScreenName";
            this.lblScreenName.Size = new System.Drawing.Size(80, 14);
            this.lblScreenName.TabIndex = 4;
            this.lblScreenName.Text = "Screen Name";
            // 
            // lblReqHie
            // 
            this.lblReqHie.AutoSize = true;
            this.lblReqHie.Location = new System.Drawing.Point(466, 46);
            this.lblReqHie.Name = "lblReqHie";
            this.lblReqHie.Size = new System.Drawing.Size(8, 14);
            this.lblReqHie.TabIndex = 1;
            this.lblReqHie.Text = ".";
            this.lblReqHie.Visible = false;
            this.lblReqHie.Click += new System.EventHandler(this.label13_Click);
            // 
            // lblHierarchy
            // 
            this.lblHierarchy.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblHierarchy.Location = new System.Drawing.Point(15, 47);
            this.lblHierarchy.Name = "lblHierarchy";
            this.lblHierarchy.Size = new System.Drawing.Size(59, 16);
            this.lblHierarchy.TabIndex = 4;
            this.lblHierarchy.Text = "Hierarchy";
            // 
            // PBHierarchy
            // 
            this.PBHierarchy.BackColor = System.Drawing.Color.Transparent;
            this.PBHierarchy.Cursor = Wisej.Web.Cursors.Hand;
            this.PBHierarchy.ImageSource = "captain-filter";
            this.PBHierarchy.Location = new System.Drawing.Point(481, 46);
            this.PBHierarchy.Name = "PBHierarchy";
            this.PBHierarchy.Size = new System.Drawing.Size(20, 20);
            this.PBHierarchy.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.PBHierarchy.Visible = false;
            this.PBHierarchy.Click += new System.EventHandler(this.PBHierarchy_Click);
            // 
            // TxtHieDeSC
            // 
            this.TxtHieDeSC.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.TxtHieDeSC.Location = new System.Drawing.Point(179, 43);
            this.TxtHieDeSC.Name = "TxtHieDeSC";
            this.TxtHieDeSC.ReadOnly = true;
            this.TxtHieDeSC.Size = new System.Drawing.Size(280, 25);
            this.TxtHieDeSC.TabIndex = 7;
            // 
            // TxtHierarchy
            // 
            this.TxtHierarchy.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.TxtHierarchy.Location = new System.Drawing.Point(110, 43);
            this.TxtHierarchy.Name = "TxtHierarchy";
            this.TxtHierarchy.ReadOnly = true;
            this.TxtHierarchy.Size = new System.Drawing.Size(62, 25);
            this.TxtHierarchy.TabIndex = 7;
            // 
            // pnlUpdatebtn
            // 
            this.pnlUpdatebtn.AppearanceKey = "panel-grdo";
            this.pnlUpdatebtn.Controls.Add(this.BtnUpdate);
            this.pnlUpdatebtn.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlUpdatebtn.Location = new System.Drawing.Point(0, 444);
            this.pnlUpdatebtn.Name = "pnlUpdatebtn";
            this.pnlUpdatebtn.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlUpdatebtn.Size = new System.Drawing.Size(983, 35);
            this.pnlUpdatebtn.TabIndex = 9;
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Dock = Wisej.Web.DockStyle.Right;
            this.BtnUpdate.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BtnUpdate.Location = new System.Drawing.Point(744, 5);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(224, 25);
            this.BtnUpdate.TabIndex = 8;
            this.BtnUpdate.Text = "&Update Incomplete Intake Controls ";
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // gvwCateHierchy
            // 
            this.gvwCateHierchy.AllowUserToResizeColumns = false;
            this.gvwCateHierchy.AllowUserToResizeRows = false;
            this.gvwCateHierchy.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle11.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCateHierchy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.gvwCateHierchy.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwCateHierchy.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCatHierchy,
            this.gvtHierchyDesc,
            this.gvtcateHierarkey});
            this.gvwCateHierchy.Location = new System.Drawing.Point(4, 42);
            this.gvwCateHierchy.MultiSelect = false;
            this.gvwCateHierchy.Name = "gvwCateHierchy";
            this.gvwCateHierchy.RowHeadersVisible = false;
            this.gvwCateHierchy.RowHeadersWidth = 14;
            this.gvwCateHierchy.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwCateHierchy.RowTemplate.Height = 20;
            this.gvwCateHierchy.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCateHierchy.Size = new System.Drawing.Size(276, 382);
            this.gvwCateHierchy.TabIndex = 0;
            // 
            // gvtCatHierchy
            // 
            this.gvtCatHierchy.HeaderText = "Hierarchy";
            this.gvtCatHierchy.Name = "gvtCatHierchy";
            this.gvtCatHierchy.ReadOnly = true;
            this.gvtCatHierchy.Width = 55;
            // 
            // gvtHierchyDesc
            // 
            this.gvtHierchyDesc.HeaderText = "Hierarchy Description";
            this.gvtHierchyDesc.Name = "gvtHierchyDesc";
            this.gvtHierchyDesc.Width = 197;
            // 
            // gvtcateHierarkey
            // 
            this.gvtcateHierarkey.HeaderText = "Hierarchy key";
            this.gvtcateHierarkey.Name = "gvtcateHierarkey";
            this.gvtcateHierarkey.Visible = false;
            // 
            // ScreenControlAssignmentForm
            // 
            this.ClientSize = new System.Drawing.Size(983, 479);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScreenControlAssignmentForm";
            this.Text = "Incomplete Intake Controls";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.ScreenControlAssignmentForm_Load);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwCateDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateDetails)).EndInit();
            this.pnlScreenAndHie.ResumeLayout(false);
            this.pnlScreenAndHie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBHierarchy)).EndInit();
            this.pnlUpdatebtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateHierchy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlCompleteForm;
        private DataGridView gvwCateHierchy;
        private DataGridView gvwCateDetails;
        private Button BtnUpdate;
        private DataGridViewTextBoxColumn gvtCatHierchy;
        private DataGridViewTextBoxColumn gvtHierchyDesc;
        private DataGridViewTextBoxColumn gvtcateHierarkey;
        private DataGridViewTextBoxColumn gvtCategory;
        private DataGridViewCheckBoxColumn gvtCatesel;
        private DataGridViewTextBoxColumn gvtCateCode;
        private Label lblScreenName;
        private ComboBox CmbScreen;
        private TextBox TxtHieDeSC;
        private TextBox TxtHierarchy;
        private PictureBox PBHierarchy;
        private Label lblReqHie;
        private Label lblHierarchy;
        private DataGridViewCheckBoxColumn gvt_Sel;
        private DataGridViewTextBoxColumn gvt_MSG;
        private Panel pnlUpdatebtn;
        private Panel pnlScreenAndHie;
        private Panel pnlgvwCateDetails;
    }
}