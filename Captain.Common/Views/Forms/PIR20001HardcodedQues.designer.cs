using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PIR20001HardcodedQues
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIR20001HardcodedQues));
            this.lblAge = new Wisej.Web.Label();
            this.cmbAgetype = new Wisej.Web.ComboBox();
            this.pnlTop = new Wisej.Web.Panel();
            this.pnlgvEducation = new Wisej.Web.Panel();
            this.gvEducation = new Wisej.Web.DataGridView();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Education = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Priorty = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Priority_Dup = new Wisej.Web.DataGridViewTextBoxColumn();
            this.lblEducation = new Wisej.Web.Label();
            this.gvSites = new Wisej.Web.DataGridView();
            this.Site_Check = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Site = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Site_Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Room = new Wisej.Web.DataGridViewTextBoxColumn();
            this.AMPM = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Sites = new Wisej.Web.DataGridViewTextBoxColumn();
            this.lblSites = new Wisej.Web.Label();
            this.pnlBtm = new Wisej.Web.Panel();
            this.pnlgvSites = new Wisej.Web.Panel();
            this.pnlA29 = new Wisej.Web.Panel();
            this.chkbSites = new Wisej.Web.CheckBox();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnExit = new Wisej.Web.Button();
            this.chkdisgred = new Wisej.Web.CheckBox();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlC8 = new Wisej.Web.Panel();
            this.pnlTop.SuspendLayout();
            this.pnlgvEducation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvEducation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSites)).BeginInit();
            this.pnlBtm.SuspendLayout();
            this.pnlgvSites.SuspendLayout();
            this.pnlA29.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlC8.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAge
            // 
            this.lblAge.Location = new System.Drawing.Point(15, 15);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(69, 16);
            this.lblAge.TabIndex = 0;
            this.lblAge.Text = "C8 Age Type";
            // 
            // cmbAgetype
            // 
            this.cmbAgetype.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAgetype.FormattingEnabled = true;
            this.cmbAgetype.Location = new System.Drawing.Point(99, 11);
            this.cmbAgetype.Name = "cmbAgetype";
            this.cmbAgetype.Size = new System.Drawing.Size(203, 25);
            this.cmbAgetype.TabIndex = 1;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlgvEducation);
            this.pnlTop.Controls.Add(this.lblEducation);
            this.pnlTop.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 41);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(696, 227);
            this.pnlTop.TabIndex = 2;
            // 
            // pnlgvEducation
            // 
            this.pnlgvEducation.Controls.Add(this.gvEducation);
            this.pnlgvEducation.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvEducation.Location = new System.Drawing.Point(0, 25);
            this.pnlgvEducation.Name = "pnlgvEducation";
            this.pnlgvEducation.Size = new System.Drawing.Size(696, 202);
            this.pnlgvEducation.TabIndex = 2;
            // 
            // gvEducation
            // 
            this.gvEducation.AllowUserToResizeColumns = false;
            this.gvEducation.AllowUserToResizeRows = false;
            this.gvEducation.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvEducation.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvEducation.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvEducation.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Code,
            this.Education,
            this.Priorty,
            this.Priority_Dup});
            this.gvEducation.Dock = Wisej.Web.DockStyle.Fill;
            this.gvEducation.Location = new System.Drawing.Point(0, 0);
            this.gvEducation.Name = "gvEducation";
            this.gvEducation.RowHeadersWidth = 15;
            this.gvEducation.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvEducation.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvEducation.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvEducation.Size = new System.Drawing.Size(696, 202);
            this.gvEducation.TabIndex = 1;
            this.gvEducation.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvEducation_CellValueChanged);
            // 
            // Code
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Code.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Code.HeaderStyle = dataGridViewCellStyle2;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Code.Width = 60;
            // 
            // Education
            // 
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Education.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Education.HeaderStyle = dataGridViewCellStyle4;
            this.Education.HeaderText = "Education";
            this.Education.Name = "Education";
            this.Education.ReadOnly = true;
            this.Education.Width = 520;
            // 
            // Priorty
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Priorty.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Priorty.HeaderStyle = dataGridViewCellStyle6;
            this.Priorty.HeaderText = "Priority";
            this.Priorty.MaxInputLength = 2;
            this.Priorty.Name = "Priorty";
            this.Priorty.Width = 70;
            // 
            // Priority_Dup
            // 
            this.Priority_Dup.HeaderText = "Priority_Dup";
            this.Priority_Dup.MaxInputLength = 2;
            this.Priority_Dup.Name = "Priority_Dup";
            this.Priority_Dup.ReadOnly = true;
            this.Priority_Dup.ShowInVisibilityMenu = false;
            this.Priority_Dup.Visible = false;
            this.Priority_Dup.Width = 20;
            // 
            // lblEducation
            // 
            this.lblEducation.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            this.lblEducation.Dock = Wisej.Web.DockStyle.Top;
            this.lblEducation.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEducation.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblEducation.Location = new System.Drawing.Point(0, 0);
            this.lblEducation.Name = "lblEducation";
            this.lblEducation.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.lblEducation.Size = new System.Drawing.Size(696, 25);
            this.lblEducation.TabIndex = 0;
            this.lblEducation.Text = "C43 Education Priorities";
            this.lblEducation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gvSites
            // 
            this.gvSites.AllowUserToResizeColumns = false;
            this.gvSites.AllowUserToResizeRows = false;
            this.gvSites.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvSites.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvSites.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSites.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Site_Check,
            this.Site,
            this.Site_Name,
            this.Room,
            this.AMPM,
            this.Sites});
            this.gvSites.Dock = Wisej.Web.DockStyle.Fill;
            this.gvSites.Location = new System.Drawing.Point(0, 0);
            this.gvSites.Name = "gvSites";
            this.gvSites.RowHeadersWidth = 15;
            this.gvSites.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvSites.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvSites.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvSites.Size = new System.Drawing.Size(696, 217);
            this.gvSites.TabIndex = 1;
            // 
            // Site_Check
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = false;
            this.Site_Check.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Site_Check.HeaderStyle = dataGridViewCellStyle8;
            this.Site_Check.HeaderText = " ";
            this.Site_Check.Name = "Site_Check";
            this.Site_Check.ShowInVisibilityMenu = false;
            this.Site_Check.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Site_Check.Width = 35;
            // 
            // Site
            // 
            dataGridViewCellStyle9.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Site.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Site.HeaderStyle = dataGridViewCellStyle10;
            this.Site.HeaderText = "Site";
            this.Site.Name = "Site";
            this.Site.ReadOnly = true;
            this.Site.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Site.Width = 75;
            // 
            // Site_Name
            // 
            dataGridViewCellStyle11.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Site_Name.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Site_Name.HeaderStyle = dataGridViewCellStyle12;
            this.Site_Name.HeaderText = "Site Name";
            this.Site_Name.Name = "Site_Name";
            this.Site_Name.ReadOnly = true;
            this.Site_Name.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Site_Name.Width = 400;
            // 
            // Room
            // 
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Room.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Room.HeaderStyle = dataGridViewCellStyle14;
            this.Room.HeaderText = "Room";
            this.Room.Name = "Room";
            this.Room.ReadOnly = true;
            this.Room.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Room.Width = 60;
            // 
            // AMPM
            // 
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.AMPM.DefaultCellStyle = dataGridViewCellStyle15;
            dataGridViewCellStyle16.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.AMPM.HeaderStyle = dataGridViewCellStyle16;
            this.AMPM.HeaderText = "AM-PM";
            this.AMPM.Name = "AMPM";
            this.AMPM.ReadOnly = true;
            this.AMPM.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.AMPM.Width = 60;
            // 
            // Sites
            // 
            this.Sites.HeaderText = "Sites";
            this.Sites.Name = "Sites";
            this.Sites.ReadOnly = true;
            this.Sites.ShowInVisibilityMenu = false;
            this.Sites.Visible = false;
            // 
            // lblSites
            // 
            this.lblSites.Dock = Wisej.Web.DockStyle.Left;
            this.lblSites.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSites.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblSites.Location = new System.Drawing.Point(15, 0);
            this.lblSites.Name = "lblSites";
            this.lblSites.Size = new System.Drawing.Size(562, 25);
            this.lblSites.TabIndex = 0;
            this.lblSites.Text = "A 29 Sites";
            this.lblSites.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBtm
            // 
            this.pnlBtm.Controls.Add(this.pnlgvSites);
            this.pnlBtm.Controls.Add(this.pnlA29);
            this.pnlBtm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlBtm.Location = new System.Drawing.Point(0, 268);
            this.pnlBtm.Name = "pnlBtm";
            this.pnlBtm.Size = new System.Drawing.Size(696, 242);
            this.pnlBtm.TabIndex = 2;
            // 
            // pnlgvSites
            // 
            this.pnlgvSites.Controls.Add(this.gvSites);
            this.pnlgvSites.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvSites.Location = new System.Drawing.Point(0, 25);
            this.pnlgvSites.Name = "pnlgvSites";
            this.pnlgvSites.Size = new System.Drawing.Size(696, 217);
            this.pnlgvSites.TabIndex = 4;
            // 
            // pnlA29
            // 
            this.pnlA29.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            this.pnlA29.Controls.Add(this.chkbSites);
            this.pnlA29.Controls.Add(this.lblSites);
            this.pnlA29.Dock = Wisej.Web.DockStyle.Top;
            this.pnlA29.Location = new System.Drawing.Point(0, 0);
            this.pnlA29.Name = "pnlA29";
            this.pnlA29.Padding = new Wisej.Web.Padding(15, 0, 15, 0);
            this.pnlA29.Size = new System.Drawing.Size(696, 25);
            this.pnlA29.TabIndex = 3;
            // 
            // chkbSites
            // 
            this.chkbSites.Dock = Wisej.Web.DockStyle.Right;
            this.chkbSites.Location = new System.Drawing.Point(605, 0);
            this.chkbSites.Name = "chkbSites";
            this.chkbSites.Size = new System.Drawing.Size(76, 25);
            this.chkbSites.TabIndex = 2;
            this.chkbSites.Text = "All Sites";
            this.chkbSites.CheckedChanged += new System.EventHandler(this.chkbSites_CheckedChanged);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnExit);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 510);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(696, 35);
            this.pnlSave.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(558, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(618, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-error";
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(621, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 25);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // chkdisgred
            // 
            this.chkdisgred.AutoSize = false;
            this.chkdisgred.Location = new System.Drawing.Point(330, 12);
            this.chkdisgred.Name = "chkdisgred";
            this.chkdisgred.Size = new System.Drawing.Size(143, 21);
            this.chkdisgred.TabIndex = 2;
            this.chkdisgred.Text = "Disregard Task Fund";
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlBtm);
            this.pnlCompleteForm.Controls.Add(this.pnlTop);
            this.pnlCompleteForm.Controls.Add(this.pnlC8);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(696, 545);
            this.pnlCompleteForm.TabIndex = 4;
            // 
            // pnlC8
            // 
            this.pnlC8.Controls.Add(this.lblAge);
            this.pnlC8.Controls.Add(this.chkdisgred);
            this.pnlC8.Controls.Add(this.cmbAgetype);
            this.pnlC8.Dock = Wisej.Web.DockStyle.Top;
            this.pnlC8.Location = new System.Drawing.Point(0, 0);
            this.pnlC8.Name = "pnlC8";
            this.pnlC8.Size = new System.Drawing.Size(696, 41);
            this.pnlC8.TabIndex = 4;
            // 
            // PIR20001HardcodedQues
            // 
            this.ClientSize = new System.Drawing.Size(696, 545);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIR20001HardcodedQues";
            this.Text = "PIR200001HardcodedQues";
            this.pnlTop.ResumeLayout(false);
            this.pnlgvEducation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvEducation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSites)).EndInit();
            this.pnlBtm.ResumeLayout(false);
            this.pnlgvSites.ResumeLayout(false);
            this.pnlA29.ResumeLayout(false);
            this.pnlA29.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlC8.ResumeLayout(false);
            this.pnlC8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblAge;
        private ComboBox cmbAgetype;
        private Panel pnlTop;
        private DataGridView gvEducation;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Education;
        private DataGridViewTextBoxColumn Priorty;
        private Label lblEducation;
        private DataGridView gvSites;
        private DataGridViewCheckBoxColumn Site_Check;
        private DataGridViewTextBoxColumn Site;
        private DataGridViewTextBoxColumn Site_Name;
        private DataGridViewTextBoxColumn Room;
        private DataGridViewTextBoxColumn AMPM;
        private Label lblSites;
        private Panel pnlBtm;
        private CheckBox chkbSites;
        private Panel pnlSave;
        private Button btnExit;
        private Button btnSave;
        private DataGridViewTextBoxColumn Sites;
        private DataGridViewTextBoxColumn Priority_Dup;
        private CheckBox chkdisgred;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
        private Panel pnlC8;
        private Panel pnlgvEducation;
        private Panel pnlA29;
        private Panel pnlgvSites;
    }
}