using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class AlertCodeForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertCodeForm));
            this.gvwAlertCode = new Wisej.Web.DataGridView();
            this.Select = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.AlertCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtInterval = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnOk = new Wisej.Web.Button();
            this.btnClose = new Wisej.Web.Button();
            this.pnlGrdIncometypes = new Wisej.Web.Panel();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlOK = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAlertCode)).BeginInit();
            this.pnlGrdIncometypes.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwAlertCode
            // 
            this.gvwAlertCode.AllowUserToResizeColumns = false;
            this.gvwAlertCode.AllowUserToResizeRows = false;
            this.gvwAlertCode.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwAlertCode.BackColor = System.Drawing.Color.White;
            this.gvwAlertCode.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwAlertCode.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwAlertCode.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Select,
            this.Item,
            this.AlertCode,
            this.gvtInterval});
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwAlertCode.DefaultCellStyle = dataGridViewCellStyle10;
            this.gvwAlertCode.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwAlertCode.Location = new System.Drawing.Point(0, 0);
            this.gvwAlertCode.MultiSelect = false;
            this.gvwAlertCode.Name = "gvwAlertCode";
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwAlertCode.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.gvwAlertCode.RowHeadersVisible = false;
            this.gvwAlertCode.RowHeadersWidth = 15;
            this.gvwAlertCode.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwAlertCode.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwAlertCode.Size = new System.Drawing.Size(394, 269);
            this.gvwAlertCode.TabIndex = 0;
            this.gvwAlertCode.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwAlertCode_CellClick);
            this.gvwAlertCode.KeyPress += new Wisej.Web.KeyPressEventHandler(this.gvwAlertCode_KeyPress);
            // 
            // Select
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.Select.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Select.HeaderStyle = dataGridViewCellStyle3;
            this.Select.HeaderText = "  ";
            this.Select.Name = "Select";
            this.Select.ShowInVisibilityMenu = false;
            this.Select.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Select.Width = 33;
            // 
            // Item
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Item.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Item.HeaderStyle = dataGridViewCellStyle5;
            this.Item.HeaderText = "Income Types";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.Item.Width = 235;
            // 
            // AlertCode
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.AlertCode.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AlertCode.HeaderStyle = dataGridViewCellStyle7;
            this.AlertCode.HeaderText = "Code";
            this.AlertCode.Name = "AlertCode";
            // 
            // gvtInterval
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtInterval.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtInterval.HeaderStyle = dataGridViewCellStyle9;
            this.gvtInterval.HeaderText = "Interval";
            this.gvtInterval.Name = "gvtInterval";
            this.gvtInterval.ShowInVisibilityMenu = false;
            this.gvtInterval.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Enabled = false;
            this.btnOk.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.Location = new System.Drawing.Point(241, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-error";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(304, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlGrdIncometypes
            // 
            this.pnlGrdIncometypes.Controls.Add(this.gvwAlertCode);
            this.pnlGrdIncometypes.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGrdIncometypes.Location = new System.Drawing.Point(0, 0);
            this.pnlGrdIncometypes.Name = "pnlGrdIncometypes";
            this.pnlGrdIncometypes.Size = new System.Drawing.Size(394, 269);
            this.pnlGrdIncometypes.TabIndex = 0;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGrdIncometypes);
            this.pnlCompleteForm.Controls.Add(this.pnlOK);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(394, 304);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlOK
            // 
            this.pnlOK.AppearanceKey = "panel-grdo";
            this.pnlOK.Controls.Add(this.btnOk);
            this.pnlOK.Controls.Add(this.spacer1);
            this.pnlOK.Controls.Add(this.btnClose);
            this.pnlOK.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(0, 269);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOK.Size = new System.Drawing.Size(394, 35);
            this.pnlOK.TabIndex = 0;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(301, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // AlertCodeForm
            // 
            this.ClientSize = new System.Drawing.Size(394, 304);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlertCodeForm";
            this.Text = "Alert Codes";
            ((System.ComponentModel.ISupportInitialize)(this.gvwAlertCode)).EndInit();
            this.pnlGrdIncometypes.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlOK.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvwAlertCode;
        private DataGridViewCheckBoxColumn Select;
        private DataGridViewTextBoxColumn Item;
        private DataGridViewTextBoxColumn AlertCode;
        private Button btnOk;
        private Button btnClose;
        private DataGridViewTextBoxColumn gvtInterval;
        private Panel pnlGrdIncometypes;
        private Panel pnlCompleteForm;
        private Panel pnlOK;
        private Spacer spacer1;
    }
}