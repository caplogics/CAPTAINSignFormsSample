using Wisej.Web;
namespace Captain.Common.Views.Forms
{
    partial class MAT00003ClientAssesment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAT00003ClientAssesment));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool2 = new Wisej.Web.ComponentTool();
            this.lblScale = new Wisej.Web.Label();
            this.cmbScale = new Wisej.Web.ComboBox();
            this.cmbMatrix = new Wisej.Web.ComboBox();
            this.lblMatrix = new Wisej.Web.Label();
            this.dtAssessmentDate = new Wisej.Web.DateTimePicker();
            this.lblAssessmentDate = new Wisej.Web.Label();
            this.chkBypassScale = new Wisej.Web.CheckBox();
            this.gvwQuestions = new Wisej.Web.DataGridView();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.lblViewBenchmark = new Wisej.Web.Label();
            this.txtBenchMarkDesc = new Wisej.Web.TextBox();
            this.lblScore = new Wisej.Web.Label();
            this.txtScore = new Wisej.Web.TextBox();
            this.btnReasonCodes = new Wisej.Web.Button();
            this.lblChangeDimesion = new Wisej.Web.Label();
            this.cmbMembers = new Wisej.Web.ComboBox();
            this.lblContactName = new Wisej.Web.Label();
            this.pnlScaleType = new Wisej.Web.Panel();
            this.rdoIndividual = new Wisej.Web.RadioButton();
            this.rdoHouseHold = new Wisej.Web.RadioButton();
            this.pnlTop = new Wisej.Web.Panel();
            this.pnlgvwQuestions = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.gvwQuestions)).BeginInit();
            this.pnlScaleType.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlgvwQuestions.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblScale
            // 
            this.lblScale.Location = new System.Drawing.Point(15, 39);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(33, 14);
            this.lblScale.TabIndex = 0;
            this.lblScale.Text = "Scale";
            // 
            // cmbScale
            // 
            this.cmbScale.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.Location = new System.Drawing.Point(93, 35);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(254, 25);
            this.cmbScale.TabIndex = 1;
            this.cmbScale.SelectedIndexChanged += new System.EventHandler(this.cmbScale_SelectedIndexChanged);
            // 
            // cmbMatrix
            // 
            this.cmbMatrix.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMatrix.FormattingEnabled = true;
            this.cmbMatrix.Location = new System.Drawing.Point(93, 6);
            this.cmbMatrix.Name = "cmbMatrix";
            this.cmbMatrix.Size = new System.Drawing.Size(254, 25);
            this.cmbMatrix.TabIndex = 1;
            this.cmbMatrix.SelectedIndexChanged += new System.EventHandler(this.cmbMatrix_SelectedIndexChanged);
            // 
            // lblMatrix
            // 
            this.lblMatrix.AutoSize = true;
            this.lblMatrix.Location = new System.Drawing.Point(15, 9);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(37, 14);
            this.lblMatrix.TabIndex = 0;
            this.lblMatrix.Text = "Matrix ";
            // 
            // dtAssessmentDate
            // 
            this.dtAssessmentDate.CustomFormat = "MM/dd/yyyy";
            this.dtAssessmentDate.Enabled = false;
            this.dtAssessmentDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtAssessmentDate.Location = new System.Drawing.Point(482, 6);
            this.dtAssessmentDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.dtAssessmentDate.Name = "dtAssessmentDate";
            this.dtAssessmentDate.ShowCheckBox = true;
            this.dtAssessmentDate.ShowToolTips = false;
            this.dtAssessmentDate.Size = new System.Drawing.Size(121, 25);
            this.dtAssessmentDate.TabIndex = 9;
            // 
            // lblAssessmentDate
            // 
            this.lblAssessmentDate.AutoSize = true;
            this.lblAssessmentDate.Location = new System.Drawing.Point(375, 9);
            this.lblAssessmentDate.Name = "lblAssessmentDate";
            this.lblAssessmentDate.Size = new System.Drawing.Size(98, 14);
            this.lblAssessmentDate.TabIndex = 4;
            this.lblAssessmentDate.Text = "Assessment Date";
            // 
            // chkBypassScale
            // 
            this.chkBypassScale.Location = new System.Drawing.Point(618, 36);
            this.chkBypassScale.Name = "chkBypassScale";
            this.chkBypassScale.Size = new System.Drawing.Size(105, 21);
            this.chkBypassScale.TabIndex = 10;
            this.chkBypassScale.Text = "Bypass Scale";
            // 
            // gvwQuestions
            // 
            this.gvwQuestions.AllowUserToResizeColumns = false;
            this.gvwQuestions.AllowUserToResizeRows = false;
            this.gvwQuestions.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwQuestions.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwQuestions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwQuestions.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.gvwQuestions.Location = new System.Drawing.Point(0, 0);
            this.gvwQuestions.Name = "gvwQuestions";
            this.gvwQuestions.RowHeadersWidth = 14;
            this.gvwQuestions.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwQuestions.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwQuestions.Size = new System.Drawing.Size(744, 232);
            this.gvwQuestions.TabIndex = 0;
            this.gvwQuestions.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwQuestions_CellClick);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            this.contextMenu1.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.gvwQuestions_MenuClick);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(654, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(574, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblViewBenchmark
            // 
            this.lblViewBenchmark.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblViewBenchmark.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
            this.lblViewBenchmark.Location = new System.Drawing.Point(367, 72);
            this.lblViewBenchmark.Name = "lblViewBenchmark";
            this.lblViewBenchmark.Size = new System.Drawing.Size(263, 16);
            this.lblViewBenchmark.TabIndex = 36;
            // 
            // txtBenchMarkDesc
            // 
            this.txtBenchMarkDesc.BackColor = System.Drawing.Color.Transparent;
            this.txtBenchMarkDesc.Location = new System.Drawing.Point(15, 95);
            this.txtBenchMarkDesc.Multiline = true;
            this.txtBenchMarkDesc.Name = "txtBenchMarkDesc";
            this.txtBenchMarkDesc.ReadOnly = true;
            this.txtBenchMarkDesc.Size = new System.Drawing.Size(701, 48);
            this.txtBenchMarkDesc.TabIndex = 37;
            this.txtBenchMarkDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(641, 9);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(36, 14);
            this.lblScore.TabIndex = 38;
            this.lblScore.Text = "Score";
            this.lblScore.Visible = false;
            // 
            // txtScore
            // 
            this.txtScore.Location = new System.Drawing.Point(682, 6);
            this.txtScore.Name = "txtScore";
            this.txtScore.ReadOnly = true;
            this.txtScore.Size = new System.Drawing.Size(34, 25);
            this.txtScore.TabIndex = 39;
            this.txtScore.Visible = false;
            // 
            // btnReasonCodes
            // 
            this.btnReasonCodes.Dock = Wisej.Web.DockStyle.Left;
            this.btnReasonCodes.Location = new System.Drawing.Point(15, 5);
            this.btnReasonCodes.Name = "btnReasonCodes";
            this.btnReasonCodes.Size = new System.Drawing.Size(109, 25);
            this.btnReasonCodes.TabIndex = 34;
            this.btnReasonCodes.Text = "&Reason Codes";
            this.btnReasonCodes.Visible = false;
            this.btnReasonCodes.Click += new System.EventHandler(this.btnReasonCodes_Click);
            // 
            // lblChangeDimesion
            // 
            this.lblChangeDimesion.AutoSize = true;
            this.lblChangeDimesion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblChangeDimesion.Location = new System.Drawing.Point(115, 598);
            this.lblChangeDimesion.Name = "lblChangeDimesion";
            this.lblChangeDimesion.Size = new System.Drawing.Size(4, 14);
            this.lblChangeDimesion.TabIndex = 0;
            this.lblChangeDimesion.Visible = false;
            // 
            // cmbMembers
            // 
            this.cmbMembers.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMembers.FormattingEnabled = true;
            this.cmbMembers.Location = new System.Drawing.Point(93, 65);
            this.cmbMembers.Name = "cmbMembers";
            this.cmbMembers.Size = new System.Drawing.Size(254, 25);
            this.cmbMembers.TabIndex = 2;
            this.cmbMembers.Visible = false;
            this.cmbMembers.SelectedIndexChanged += new System.EventHandler(this.cmbMembers_SelectedIndexChanged);
            // 
            // lblContactName
            // 
            this.lblContactName.AutoSize = true;
            this.lblContactName.Location = new System.Drawing.Point(15, 69);
            this.lblContactName.Name = "lblContactName";
            this.lblContactName.Size = new System.Drawing.Size(55, 14);
            this.lblContactName.TabIndex = 0;
            this.lblContactName.Text = "Members";
            this.lblContactName.Visible = false;
            // 
            // pnlScaleType
            // 
            this.pnlScaleType.Controls.Add(this.rdoIndividual);
            this.pnlScaleType.Controls.Add(this.rdoHouseHold);
            this.pnlScaleType.Location = new System.Drawing.Point(367, 32);
            this.pnlScaleType.Name = "pnlScaleType";
            this.pnlScaleType.Size = new System.Drawing.Size(234, 26);
            this.pnlScaleType.TabIndex = 40;
            this.pnlScaleType.Visible = false;
            // 
            // rdoIndividual
            // 
            this.rdoIndividual.AutoSize = false;
            this.rdoIndividual.Location = new System.Drawing.Point(108, 4);
            this.rdoIndividual.Name = "rdoIndividual";
            this.rdoIndividual.Size = new System.Drawing.Size(82, 20);
            this.rdoIndividual.TabIndex = 1;
            this.rdoIndividual.Text = "Individual";
            this.rdoIndividual.Click += new System.EventHandler(this.rdoIndividual_Click);
            // 
            // rdoHouseHold
            // 
            this.rdoHouseHold.AutoSize = false;
            this.rdoHouseHold.Checked = true;
            this.rdoHouseHold.Location = new System.Drawing.Point(3, 4);
            this.rdoHouseHold.Name = "rdoHouseHold";
            this.rdoHouseHold.Size = new System.Drawing.Size(90, 20);
            this.rdoHouseHold.TabIndex = 0;
            this.rdoHouseHold.TabStop = true;
            this.rdoHouseHold.Text = "HouseHold";
            this.rdoHouseHold.Click += new System.EventHandler(this.rdoIndividual_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblMatrix);
            this.pnlTop.Controls.Add(this.pnlScaleType);
            this.pnlTop.Controls.Add(this.lblScale);
            this.pnlTop.Controls.Add(this.lblContactName);
            this.pnlTop.Controls.Add(this.cmbScale);
            this.pnlTop.Controls.Add(this.cmbMembers);
            this.pnlTop.Controls.Add(this.cmbMatrix);
            this.pnlTop.Controls.Add(this.dtAssessmentDate);
            this.pnlTop.Controls.Add(this.lblAssessmentDate);
            this.pnlTop.Controls.Add(this.txtScore);
            this.pnlTop.Controls.Add(this.chkBypassScale);
            this.pnlTop.Controls.Add(this.lblScore);
            this.pnlTop.Controls.Add(this.lblViewBenchmark);
            this.pnlTop.Controls.Add(this.txtBenchMarkDesc);
            this.pnlTop.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(744, 150);
            this.pnlTop.TabIndex = 41;
            // 
            // pnlgvwQuestions
            // 
            this.pnlgvwQuestions.Controls.Add(this.gvwQuestions);
            this.pnlgvwQuestions.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwQuestions.Location = new System.Drawing.Point(0, 150);
            this.pnlgvwQuestions.Name = "pnlgvwQuestions";
            this.pnlgvwQuestions.Size = new System.Drawing.Size(744, 232);
            this.pnlgvwQuestions.TabIndex = 42;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Controls.Add(this.btnReasonCodes);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 382);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(744, 35);
            this.pnlSave.TabIndex = 43;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(649, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // MAT00003ClientAssesment
            // 
            this.ClientSize = new System.Drawing.Size(744, 417);
            this.Controls.Add(this.pnlgvwQuestions);
            this.Controls.Add(this.pnlSave);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.lblChangeDimesion);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MAT00003ClientAssesment";
            this.Text = "MAT00003ClientAssesment";
            componentTool1.ImageSource = "captain-pdf";
            componentTool1.Name = "tlPDF";
            componentTool2.ImageSource = "icon-help";
            componentTool2.Name = "tlHelp";
            componentTool2.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1,
            componentTool2});
            this.Load += new System.EventHandler(this.MAT00003ClientAssesment_Load);
            this.FormClosed += new Wisej.Web.FormClosedEventHandler(this.MAT00003ClientAssesment_FormClosed);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.MAT00003ClientAssesment_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.gvwQuestions)).EndInit();
            this.pnlScaleType.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlgvwQuestions.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label lblScale;
        private ComboBox cmbScale;
        private ComboBox cmbMatrix;
        private Label lblMatrix;
        private DateTimePicker dtAssessmentDate;
        private Label lblAssessmentDate;
        private CheckBox chkBypassScale;
        private DataGridView gvwQuestions;
        private ContextMenu contextMenu1;
        private Button btnCancel;
        private Button btnSave;
        private Label lblViewBenchmark;
        private TextBox txtBenchMarkDesc;
        private Label lblScore;
        private TextBox txtScore;
        private Button btnReasonCodes;
        private Label lblChangeDimesion;
        private ComboBox cmbMembers;
        private Label lblContactName;
        private Panel pnlScaleType;
        private RadioButton rdoIndividual;
        private RadioButton rdoHouseHold;
        private Panel pnlTop;
        private Panel pnlgvwQuestions;
        private Panel pnlSave;
        private Spacer spacer1;
    }
}