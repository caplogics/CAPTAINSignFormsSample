using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class SSBGMonths_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSBGMonths_Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.btnSave = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.label6 = new Wisej.Web.Label();
            this.lblGPDate = new Wisej.Web.Label();
            this.dtpGPFrom = new Wisej.Web.DateTimePicker();
            this.dtpGPTo = new Wisej.Web.DateTimePicker();
            this.txtHierachydesc = new Wisej.Web.TextBox();
            this.txtHierarchy = new Wisej.Web.TextBox();
            this.lblHier = new Wisej.Web.Label();
            this.PbHierarchies = new Wisej.Web.PictureBox();
            this.lblDesc = new Wisej.Web.Label();
            this.txtGroupDesc = new Wisej.Web.TextBox();
            this.pnlGroup = new Wisej.Web.Panel();
            this.pnlgvMonths = new Wisej.Web.Panel();
            this.gvMonths = new Wisej.Web.DataGridView();
            this.gvMonthName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvTarget = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvMonthCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvYear = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlFields = new Wisej.Web.Panel();
            this.txtTDesc = new Wisej.Web.TextBox();
            this.lblCntlType = new Wisej.Web.Label();
            this.pnlCompForm = new Wisej.Web.Panel();
            this.pnlGSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.PbHierarchies)).BeginInit();
            this.pnlGroup.SuspendLayout();
            this.pnlgvMonths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMonths)).BeginInit();
            this.pnlFields.SuspendLayout();
            this.pnlCompForm.SuspendLayout();
            this.pnlGSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(405, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(483, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(253, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "To";
            // 
            // lblGPDate
            // 
            this.lblGPDate.Location = new System.Drawing.Point(15, 46);
            this.lblGPDate.Name = "lblGPDate";
            this.lblGPDate.Size = new System.Drawing.Size(73, 16);
            this.lblGPDate.TabIndex = 1;
            this.lblGPDate.Text = "Grant Period";
            // 
            // dtpGPFrom
            // 
            this.dtpGPFrom.AutoSize = false;
            this.dtpGPFrom.Checked = false;
            this.dtpGPFrom.Enabled = false;
            this.dtpGPFrom.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpGPFrom.Location = new System.Drawing.Point(105, 42);
            this.dtpGPFrom.Name = "dtpGPFrom";
            this.dtpGPFrom.ShowCheckBox = true;
            this.dtpGPFrom.ShowToolTips = false;
            this.dtpGPFrom.Size = new System.Drawing.Size(116, 25);
            this.dtpGPFrom.TabIndex = 3;
            // 
            // dtpGPTo
            // 
            this.dtpGPTo.AutoSize = false;
            this.dtpGPTo.Checked = false;
            this.dtpGPTo.Enabled = false;
            this.dtpGPTo.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpGPTo.Location = new System.Drawing.Point(280, 42);
            this.dtpGPTo.Name = "dtpGPTo";
            this.dtpGPTo.ShowCheckBox = true;
            this.dtpGPTo.ShowToolTips = false;
            this.dtpGPTo.Size = new System.Drawing.Size(116, 25);
            this.dtpGPTo.TabIndex = 4;
            // 
            // txtHierachydesc
            // 
            this.txtHierachydesc.AutoSize = false;
            this.txtHierachydesc.Enabled = false;
            this.txtHierachydesc.Location = new System.Drawing.Point(179, 11);
            this.txtHierachydesc.MaxLength = 100;
            this.txtHierachydesc.Name = "txtHierachydesc";
            this.txtHierachydesc.ReadOnly = true;
            this.txtHierachydesc.Size = new System.Drawing.Size(335, 25);
            this.txtHierachydesc.TabIndex = 2;
            this.txtHierachydesc.TabStop = false;
            // 
            // txtHierarchy
            // 
            this.txtHierarchy.Enabled = false;
            this.txtHierarchy.Location = new System.Drawing.Point(105, 11);
            this.txtHierarchy.MaxLength = 100;
            this.txtHierarchy.Name = "txtHierarchy";
            this.txtHierarchy.ReadOnly = true;
            this.txtHierarchy.Size = new System.Drawing.Size(64, 25);
            this.txtHierarchy.TabIndex = 1;
            this.txtHierarchy.TabStop = false;
            // 
            // lblHier
            // 
            this.lblHier.Location = new System.Drawing.Point(15, 15);
            this.lblHier.Name = "lblHier";
            this.lblHier.Size = new System.Drawing.Size(57, 16);
            this.lblHier.TabIndex = 3;
            this.lblHier.Text = "Hierarchy";
            // 
            // PbHierarchies
            // 
            this.PbHierarchies.Cursor = Wisej.Web.Cursors.Hand;
            this.PbHierarchies.ImageSource = "captain-filter";
            this.PbHierarchies.Location = new System.Drawing.Point(525, 11);
            this.PbHierarchies.Name = "PbHierarchies";
            this.PbHierarchies.Size = new System.Drawing.Size(20, 25);
            this.PbHierarchies.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.PbHierarchies.ToolTipText = "Select Hierarchy";
            this.PbHierarchies.Visible = false;
            this.PbHierarchies.Click += new System.EventHandler(this.PbHierarchies_Click);
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(15, 77);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(30, 16);
            this.lblDesc.TabIndex = 1;
            this.lblDesc.Text = "Desc";
            // 
            // txtGroupDesc
            // 
            this.txtGroupDesc.Enabled = false;
            this.txtGroupDesc.Location = new System.Drawing.Point(105, 73);
            this.txtGroupDesc.MaxLength = 100;
            this.txtGroupDesc.Name = "txtGroupDesc";
            this.txtGroupDesc.Size = new System.Drawing.Size(409, 25);
            this.txtGroupDesc.TabIndex = 5;
            // 
            // pnlGroup
            // 
            this.pnlGroup.Controls.Add(this.pnlgvMonths);
            this.pnlGroup.Controls.Add(this.pnlFields);
            this.pnlGroup.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGroup.Location = new System.Drawing.Point(0, 0);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(573, 343);
            this.pnlGroup.TabIndex = 1;
            // 
            // pnlgvMonths
            // 
            this.pnlgvMonths.Controls.Add(this.gvMonths);
            this.pnlgvMonths.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvMonths.Location = new System.Drawing.Point(0, 138);
            this.pnlgvMonths.Name = "pnlgvMonths";
            this.pnlgvMonths.Size = new System.Drawing.Size(573, 205);
            this.pnlgvMonths.TabIndex = 99;
            // 
            // gvMonths
            // 
            this.gvMonths.AllowUserToResizeRows = false;
            this.gvMonths.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvMonths.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvMonths.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvMonthName,
            this.gvTarget,
            this.gvMonthCode,
            this.gvYear});
            this.gvMonths.Dock = Wisej.Web.DockStyle.Fill;
            this.gvMonths.Location = new System.Drawing.Point(0, 0);
            this.gvMonths.Name = "gvMonths";
            this.gvMonths.RowHeadersWidth = 23;
            this.gvMonths.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvMonths.Size = new System.Drawing.Size(573, 205);
            this.gvMonths.TabIndex = 88;
            this.gvMonths.TabStop = false;
            this.gvMonths.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvMonths_CellClick);
            // 
            // gvMonthName
            // 
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvMonthName.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvMonthName.HeaderStyle = dataGridViewCellStyle2;
            this.gvMonthName.HeaderText = "Month";
            this.gvMonthName.Name = "gvMonthName";
            this.gvMonthName.ReadOnly = true;
            this.gvMonthName.Width = 250;
            // 
            // gvTarget
            // 
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvTarget.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvTarget.HeaderStyle = dataGridViewCellStyle4;
            this.gvTarget.HeaderText = "Target";
            this.gvTarget.Name = "gvTarget";
            this.gvTarget.Width = 120;
            // 
            // gvMonthCode
            // 
            this.gvMonthCode.HeaderText = "MonthCode";
            this.gvMonthCode.Name = "gvMonthCode";
            this.gvMonthCode.ReadOnly = true;
            this.gvMonthCode.ShowInVisibilityMenu = false;
            this.gvMonthCode.Visible = false;
            // 
            // gvYear
            // 
            this.gvYear.HeaderText = "Year";
            this.gvYear.Name = "gvYear";
            this.gvYear.ShowInVisibilityMenu = false;
            this.gvYear.Visible = false;
            // 
            // pnlFields
            // 
            this.pnlFields.Controls.Add(this.lblHier);
            this.pnlFields.Controls.Add(this.txtGroupDesc);
            this.pnlFields.Controls.Add(this.PbHierarchies);
            this.pnlFields.Controls.Add(this.txtTDesc);
            this.pnlFields.Controls.Add(this.lblDesc);
            this.pnlFields.Controls.Add(this.lblCntlType);
            this.pnlFields.Controls.Add(this.txtHierarchy);
            this.pnlFields.Controls.Add(this.label6);
            this.pnlFields.Controls.Add(this.txtHierachydesc);
            this.pnlFields.Controls.Add(this.lblGPDate);
            this.pnlFields.Controls.Add(this.dtpGPTo);
            this.pnlFields.Controls.Add(this.dtpGPFrom);
            this.pnlFields.Dock = Wisej.Web.DockStyle.Top;
            this.pnlFields.Location = new System.Drawing.Point(0, 0);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(573, 138);
            this.pnlFields.TabIndex = 1;
            // 
            // txtTDesc
            // 
            this.txtTDesc.Enabled = false;
            this.txtTDesc.Location = new System.Drawing.Point(105, 104);
            this.txtTDesc.MaxLength = 100;
            this.txtTDesc.Name = "txtTDesc";
            this.txtTDesc.Size = new System.Drawing.Size(409, 25);
            this.txtTDesc.TabIndex = 6;
            // 
            // lblCntlType
            // 
            this.lblCntlType.Location = new System.Drawing.Point(10, 108);
            this.lblCntlType.Name = "lblCntlType";
            this.lblCntlType.Size = new System.Drawing.Size(74, 16);
            this.lblCntlType.TabIndex = 1;
            this.lblCntlType.Text = "Control Type";
            // 
            // pnlCompForm
            // 
            this.pnlCompForm.Controls.Add(this.pnlGroup);
            this.pnlCompForm.Controls.Add(this.pnlGSave);
            this.pnlCompForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompForm.Name = "pnlCompForm";
            this.pnlCompForm.Size = new System.Drawing.Size(573, 378);
            this.pnlCompForm.TabIndex = 0;
            // 
            // pnlGSave
            // 
            this.pnlGSave.AppearanceKey = "panel-grdo";
            this.pnlGSave.Controls.Add(this.btnSave);
            this.pnlGSave.Controls.Add(this.spacer1);
            this.pnlGSave.Controls.Add(this.btnCancel);
            this.pnlGSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGSave.Location = new System.Drawing.Point(0, 343);
            this.pnlGSave.Name = "pnlGSave";
            this.pnlGSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlGSave.Size = new System.Drawing.Size(573, 35);
            this.pnlGSave.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(480, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // SSBGMonths_Form
            // 
            this.ClientSize = new System.Drawing.Size(573, 378);
            this.Controls.Add(this.pnlCompForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SSBGMonths_Form";
            this.Text = "SSBGMonths_Form";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.SSBGMonths_Form_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.PbHierarchies)).EndInit();
            this.pnlGroup.ResumeLayout(false);
            this.pnlgvMonths.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMonths)).EndInit();
            this.pnlFields.ResumeLayout(false);
            this.pnlFields.PerformLayout();
            this.pnlCompForm.ResumeLayout(false);
            this.pnlGSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnSave;
        private Button btnCancel;
        private Label label6;
        private Label lblGPDate;
        private DateTimePicker dtpGPFrom;
        private DateTimePicker dtpGPTo;
        private TextBox txtHierachydesc;
        private TextBox txtHierarchy;
        private Label lblHier;
        private PictureBox PbHierarchies;
        private Label lblDesc;
        private TextBox txtGroupDesc;
        private Panel pnlGroup;
        private DataGridView gvMonths;
        private DataGridViewTextBoxColumn gvMonthName;
        private DataGridViewTextBoxColumn gvTarget;
        private DataGridViewTextBoxColumn gvMonthCode;
        private DataGridViewTextBoxColumn gvYear;
        private TextBox txtTDesc;
        private Label lblCntlType;
        private Panel pnlgvMonths;
        private Panel pnlCompForm;
        private Panel pnlGSave;
        private Spacer spacer1;
        private Panel pnlFields;
    }
}