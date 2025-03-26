//using Wisej.Web;
//using Gizmox.WebGUI.Common;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class MatScaleGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatScaleGroup));
            this.picAddreason = new Wisej.Web.PictureBox();
            this.lblResnDesc = new Wisej.Web.Label();
            this.Pb_Save_Reason = new Wisej.Web.Button();
            this.Pb_Cancel_Reason = new Wisej.Web.Button();
            this.lblReqGD = new Wisej.Web.Label();
            this.Reasons_Grid = new Wisej.Web.DataGridView();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtShortName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtEdit = new Wisej.Web.DataGridViewImageColumn();
            this.gvtDelete = new Wisej.Web.DataGridViewImageColumn();
            this.pnlReasons = new Wisej.Web.Panel();
            this.lblReqSeq = new Wisej.Web.Label();
            this.lblSeq = new Wisej.Web.Label();
            this.txtSeq = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtShortName = new Wisej.Web.TextBox();
            this.lblReqShrtN = new Wisej.Web.Label();
            this.lblShortName = new Wisej.Web.Label();
            this.txtGroupDesc2 = new Wisej.Web.TextBox();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.picAddreason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Reasons_Grid)).BeginInit();
            this.pnlReasons.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // picAddreason
            // 
            this.picAddreason.BackgroundImageLayout = Wisej.Web.ImageLayout.BestFit;
            this.picAddreason.Cursor = Wisej.Web.Cursors.Hand;
            this.picAddreason.ImageSource = "captain-add";
            this.picAddreason.Location = new System.Drawing.Point(496, 42);
            this.picAddreason.Name = "picAddreason";
            this.picAddreason.Size = new System.Drawing.Size(24, 24);
            this.picAddreason.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAddreason.ToolTipText = "Add Group(s)";
            this.picAddreason.Click += new System.EventHandler(this.picAddreason_Click);
            // 
            // lblResnDesc
            // 
            this.lblResnDesc.Location = new System.Drawing.Point(15, 48);
            this.lblResnDesc.Name = "lblResnDesc";
            this.lblResnDesc.Size = new System.Drawing.Size(67, 16);
            this.lblResnDesc.TabIndex = 1;
            this.lblResnDesc.Text = "Group Desc";
            // 
            // Pb_Save_Reason
            // 
            this.Pb_Save_Reason.AppearanceKey = "button-ok";
            this.Pb_Save_Reason.Dock = Wisej.Web.DockStyle.Right;
            this.Pb_Save_Reason.Location = new System.Drawing.Point(365, 5);
            this.Pb_Save_Reason.Name = "Pb_Save_Reason";
            this.Pb_Save_Reason.Size = new System.Drawing.Size(75, 25);
            this.Pb_Save_Reason.TabIndex = 4;
            this.Pb_Save_Reason.Text = "&Save";
            this.Pb_Save_Reason.Visible = false;
            this.Pb_Save_Reason.Click += new System.EventHandler(this.Pb_Save_Reason_Click);
            // 
            // Pb_Cancel_Reason
            // 
            this.Pb_Cancel_Reason.AppearanceKey = "button-error";
            this.Pb_Cancel_Reason.Dock = Wisej.Web.DockStyle.Right;
            this.Pb_Cancel_Reason.Location = new System.Drawing.Point(443, 5);
            this.Pb_Cancel_Reason.Name = "Pb_Cancel_Reason";
            this.Pb_Cancel_Reason.Size = new System.Drawing.Size(75, 25);
            this.Pb_Cancel_Reason.TabIndex = 5;
            this.Pb_Cancel_Reason.Text = "&Cancel";
            this.Pb_Cancel_Reason.Visible = false;
            this.Pb_Cancel_Reason.Click += new System.EventHandler(this.Pb_Cancel_Reason_Click);
            // 
            // lblReqGD
            // 
            this.lblReqGD.ForeColor = System.Drawing.Color.Red;
            this.lblReqGD.Location = new System.Drawing.Point(83, 45);
            this.lblReqGD.Name = "lblReqGD";
            this.lblReqGD.Size = new System.Drawing.Size(8, 10);
            this.lblReqGD.TabIndex = 8;
            this.lblReqGD.Text = "*";
            // 
            // Reasons_Grid
            // 
            this.Reasons_Grid.AllowUserToResizeColumns = false;
            this.Reasons_Grid.AllowUserToResizeRows = false;
            this.Reasons_Grid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.Reasons_Grid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.Reasons_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Reasons_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Reasons_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Reasons_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCode,
            this.gvtShortName,
            this.gvtDesc,
            this.gvtSeq,
            this.gvtEdit,
            this.gvtDelete});
            this.Reasons_Grid.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.Reasons_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.Reasons_Grid.Location = new System.Drawing.Point(8, 8);
            this.Reasons_Grid.MultiSelect = false;
            this.Reasons_Grid.Name = "Reasons_Grid";
            this.Reasons_Grid.ReadOnly = true;
            this.Reasons_Grid.RowHeadersWidth = 14;
            this.Reasons_Grid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Reasons_Grid.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Reasons_Grid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Reasons_Grid.Size = new System.Drawing.Size(517, 244);
            this.Reasons_Grid.TabIndex = 0;
            this.Reasons_Grid.SelectionChanged += new System.EventHandler(this.Reasons_Grid_SelectionChanged);
            this.Reasons_Grid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.Reasons_Grid_CellClick);
            // 
            // gvtCode
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCode.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCode.HeaderStyle = dataGridViewCellStyle3;
            this.gvtCode.HeaderText = "gvtCode";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.ShowInVisibilityMenu = false;
            this.gvtCode.Visible = false;
            this.gvtCode.Width = 20;
            // 
            // gvtShortName
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtShortName.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtShortName.HeaderStyle = dataGridViewCellStyle5;
            this.gvtShortName.HeaderText = "Name";
            this.gvtShortName.Name = "gvtShortName";
            this.gvtShortName.ReadOnly = true;
            this.gvtShortName.Width = 85;
            // 
            // gvtDesc
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtDesc.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtDesc.HeaderStyle = dataGridViewCellStyle7;
            this.gvtDesc.HeaderText = "Description";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.gvtDesc.Width = 250;
            // 
            // gvtSeq
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtSeq.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSeq.HeaderStyle = dataGridViewCellStyle9;
            this.gvtSeq.HeaderText = "Seq";
            this.gvtSeq.Name = "gvtSeq";
            this.gvtSeq.ReadOnly = true;
            this.gvtSeq.Width = 50;
            // 
            // gvtEdit
            // 
            this.gvtEdit.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.None;
            this.gvtEdit.CellImageSource = "captain-edit";
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.NullValue = null;
            this.gvtEdit.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtEdit.HeaderStyle = dataGridViewCellStyle11;
            this.gvtEdit.HeaderText = "Edit";
            this.gvtEdit.Name = "gvtEdit";
            this.gvtEdit.ReadOnly = true;
            this.gvtEdit.ShowInVisibilityMenu = false;
            this.gvtEdit.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvtEdit.Width = 50;
            // 
            // gvtDelete
            // 
            this.gvtDelete.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.None;
            this.gvtDelete.CellImageSource = "captain-delete";
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle12.NullValue = null;
            this.gvtDelete.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtDelete.HeaderStyle = dataGridViewCellStyle13;
            this.gvtDelete.HeaderText = "Delete";
            this.gvtDelete.Name = "gvtDelete";
            this.gvtDelete.ReadOnly = true;
            this.gvtDelete.ShowInVisibilityMenu = false;
            this.gvtDelete.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvtDelete.Width = 50;
            // 
            // pnlReasons
            // 
            this.pnlReasons.Controls.Add(this.Reasons_Grid);
            this.pnlReasons.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReasons.Location = new System.Drawing.Point(0, 0);
            this.pnlReasons.Name = "pnlReasons";
            this.pnlReasons.Padding = new Wisej.Web.Padding(8);
            this.pnlReasons.Size = new System.Drawing.Size(533, 260);
            this.pnlReasons.TabIndex = 1;
            // 
            // lblReqSeq
            // 
            this.lblReqSeq.ForeColor = System.Drawing.Color.Red;
            this.lblReqSeq.Location = new System.Drawing.Point(245, 10);
            this.lblReqSeq.Name = "lblReqSeq";
            this.lblReqSeq.Size = new System.Drawing.Size(8, 10);
            this.lblReqSeq.TabIndex = 8;
            this.lblReqSeq.Text = "*";
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(224, 15);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(21, 16);
            this.lblSeq.TabIndex = 10;
            this.lblSeq.Text = "Seq";
            // 
            // txtSeq
            // 
            this.txtSeq.Enabled = false;
            this.txtSeq.Location = new System.Drawing.Point(260, 11);
            this.txtSeq.MaxLength = 3;
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Size = new System.Drawing.Size(35, 25);
            this.txtSeq.TabIndex = 2;
            // 
            // txtShortName
            // 
            this.txtShortName.Enabled = false;
            this.txtShortName.Location = new System.Drawing.Point(98, 11);
            this.txtShortName.MaxLength = 10;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(100, 25);
            this.txtShortName.TabIndex = 1;
            // 
            // lblReqShrtN
            // 
            this.lblReqShrtN.ForeColor = System.Drawing.Color.Red;
            this.lblReqShrtN.Location = new System.Drawing.Point(84, 11);
            this.lblReqShrtN.Name = "lblReqShrtN";
            this.lblReqShrtN.Size = new System.Drawing.Size(9, 10);
            this.lblReqShrtN.TabIndex = 8;
            this.lblReqShrtN.Text = "*";
            // 
            // lblShortName
            // 
            this.lblShortName.Location = new System.Drawing.Point(15, 15);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(69, 14);
            this.lblShortName.TabIndex = 10;
            this.lblShortName.Text = "Short Name";
            // 
            // txtGroupDesc2
            // 
            this.txtGroupDesc2.Enabled = false;
            this.txtGroupDesc2.Location = new System.Drawing.Point(98, 43);
            this.txtGroupDesc2.MaxLength = 200;
            this.txtGroupDesc2.Multiline = true;
            this.txtGroupDesc2.Name = "txtGroupDesc2";
            this.txtGroupDesc2.Size = new System.Drawing.Size(372, 56);
            this.txtGroupDesc2.TabIndex = 3;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Controls.Add(this.pnlReasons);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(533, 402);
            this.pnlCompleteForm.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblReqSeq);
            this.panel1.Controls.Add(this.txtGroupDesc2);
            this.panel1.Controls.Add(this.picAddreason);
            this.panel1.Controls.Add(this.lblSeq);
            this.panel1.Controls.Add(this.lblResnDesc);
            this.panel1.Controls.Add(this.lblShortName);
            this.panel1.Controls.Add(this.txtSeq);
            this.panel1.Controls.Add(this.lblReqShrtN);
            this.panel1.Controls.Add(this.lblReqGD);
            this.panel1.Controls.Add(this.txtShortName);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 260);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(533, 107);
            this.panel1.TabIndex = 1;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.Pb_Save_Reason);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.Pb_Cancel_Reason);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 367);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(533, 35);
            this.pnlSave.TabIndex = 0;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(440, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // MatScaleGroup
            // 
            this.ClientSize = new System.Drawing.Size(533, 402);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatScaleGroup";
            this.Text = "Scale Groups";
            ((System.ComponentModel.ISupportInitialize)(this.picAddreason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Reasons_Grid)).EndInit();
            this.pnlReasons.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox picAddreason;       
        private Label lblResnDesc;
        private Button Pb_Save_Reason;
        private Button Pb_Cancel_Reason;
        private Label lblReqGD;
        private DataGridView Reasons_Grid;
        private Panel pnlReasons;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewImageColumn gvtEdit;
        private DataGridViewImageColumn gvtDelete;
        private TextBox txtGroupDesc2;
        private DataGridViewTextBoxColumn gvtCode;
        private Label lblSeq;
        private TextBoxWithValidation txtSeq;
        private TextBox txtShortName;
        private Label lblReqShrtN;
        private Label lblShortName;
        private DataGridViewTextBoxColumn gvtShortName;
        private DataGridViewTextBoxColumn gvtSeq;
        private Label lblReqSeq;
        private Panel pnlCompleteForm;
        private Panel panel1;
        private Panel pnlSave;
        private Spacer spacer1;
    }
}