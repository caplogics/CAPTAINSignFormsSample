using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CaseWorkerSelection_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseWorkerSelection_Form));
            this.btnCancel = new Wisej.Web.Button();
            this.chkUnselectAll = new Wisej.Web.CheckBox();
            this.chkSelectAll = new Wisej.Web.CheckBox();
            this.btnSelect = new Wisej.Web.Button();
            this.gvwWorker = new Wisej.Web.DataGridView();
            this.Sel_Img = new Wisej.Web.DataGridViewImageColumn();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Selected = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvStatus = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwWorker = new Wisej.Web.Panel();
            this.pnlOk = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.gvwWorker)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwWorker.SuspendLayout();
            this.pnlOk.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(414, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkUnselectAll
            // 
            this.chkUnselectAll.AutoSize = false;
            this.chkUnselectAll.Dock = Wisej.Web.DockStyle.Left;
            this.chkUnselectAll.Location = new System.Drawing.Point(96, 5);
            this.chkUnselectAll.Name = "chkUnselectAll";
            this.chkUnselectAll.Size = new System.Drawing.Size(93, 25);
            this.chkUnselectAll.TabIndex = 2;
            this.chkUnselectAll.Text = "Unselect All";
            this.chkUnselectAll.CheckedChanged += new System.EventHandler(this.chkUnselectAll_CheckedChanged);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = false;
            this.chkSelectAll.Dock = Wisej.Web.DockStyle.Left;
            this.chkSelectAll.Location = new System.Drawing.Point(15, 5);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(78, 25);
            this.chkSelectAll.TabIndex = 2;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.AppearanceKey = "button-ok";
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(351, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(60, 25);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "&OK";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // gvwWorker
            // 
            this.gvwWorker.AllowUserToResizeColumns = false;
            this.gvwWorker.AllowUserToResizeRows = false;
            this.gvwWorker.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwWorker.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.gvwWorker.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwWorker.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwWorker.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwWorker.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Sel_Img,
            this.gvtCode,
            this.gvtDesc,
            this.Selected,
            this.gvStatus});
            this.gvwWorker.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwWorker.Location = new System.Drawing.Point(0, 0);
            this.gvwWorker.Name = "gvwWorker";
            this.gvwWorker.RowHeadersWidth = 25;
            this.gvwWorker.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwWorker.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwWorker.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwWorker.Size = new System.Drawing.Size(504, 346);
            this.gvwWorker.TabIndex = 0;
            this.gvwWorker.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvsite_CellClick);
            // 
            // Sel_Img
            // 
            this.Sel_Img.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.None;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = null;
            this.Sel_Img.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Sel_Img.HeaderStyle = dataGridViewCellStyle3;
            this.Sel_Img.HeaderText = " ";
            this.Sel_Img.Name = "Sel_Img";
            this.Sel_Img.ShowInVisibilityMenu = false;
            this.Sel_Img.Width = 40;
            // 
            // gvtCode
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtCode.HeaderStyle = dataGridViewCellStyle4;
            this.gvtCode.HeaderText = "Code";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.Width = 80;
            // 
            // gvtDesc
            // 
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtDesc.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtDesc.HeaderStyle = dataGridViewCellStyle6;
            this.gvtDesc.HeaderText = "Name";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.gvtDesc.Width = 240;
            // 
            // Selected
            // 
            this.Selected.HeaderText = "Selected";
            this.Selected.Name = "Selected";
            this.Selected.ShowInVisibilityMenu = false;
            this.Selected.Visible = false;
            this.Selected.Width = 20;
            // 
            // gvStatus
            // 
            this.gvStatus.HeaderText = "Status";
            this.gvStatus.Name = "gvStatus";
            this.gvStatus.ReadOnly = true;
            this.gvStatus.Width = 135;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwWorker);
            this.pnlCompleteForm.Controls.Add(this.pnlOk);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(504, 381);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlgvwWorker
            // 
            this.pnlgvwWorker.Controls.Add(this.gvwWorker);
            this.pnlgvwWorker.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwWorker.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwWorker.Name = "pnlgvwWorker";
            this.pnlgvwWorker.Size = new System.Drawing.Size(504, 346);
            this.pnlgvwWorker.TabIndex = 3;
            // 
            // pnlOk
            // 
            this.pnlOk.AppearanceKey = "panel-grdo";
            this.pnlOk.Controls.Add(this.chkUnselectAll);
            this.pnlOk.Controls.Add(this.spacer2);
            this.pnlOk.Controls.Add(this.btnSelect);
            this.pnlOk.Controls.Add(this.spacer1);
            this.pnlOk.Controls.Add(this.chkSelectAll);
            this.pnlOk.Controls.Add(this.btnCancel);
            this.pnlOk.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOk.Location = new System.Drawing.Point(0, 346);
            this.pnlOk.Name = "pnlOk";
            this.pnlOk.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlOk.Size = new System.Drawing.Size(504, 35);
            this.pnlOk.TabIndex = 4;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(93, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(411, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // CaseWorkerSelection_Form
            // 
            this.ClientSize = new System.Drawing.Size(504, 381);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaseWorkerSelection_Form";
            this.Text = "CaseWorkerSelection_Form";
            ((System.ComponentModel.ISupportInitialize)(this.gvwWorker)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwWorker.ResumeLayout(false);
            this.pnlOk.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnCancel;
        private CheckBox chkUnselectAll;
        private CheckBox chkSelectAll;
        private Button btnSelect;
        private DataGridView gvwWorker;
        private DataGridViewImageColumn Sel_Img;
        private DataGridViewTextBoxColumn gvtCode;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewTextBoxColumn Selected;
        private Panel pnlCompleteForm;
        private Panel pnlOk;
        private Spacer spacer2;
        private Spacer spacer1;
        private Panel pnlgvwWorker;
        private DataGridViewTextBoxColumn gvStatus;
    }
}