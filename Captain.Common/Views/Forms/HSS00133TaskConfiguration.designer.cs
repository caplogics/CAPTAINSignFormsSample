using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00133TaskConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00133TaskConfiguration));
            this.gvTaskConfig = new Wisej.Web.DataGridView();
            this.gvtFields = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gviEnable = new Wisej.Web.DataGridViewImageColumn();
            this.gviReq = new Wisej.Web.DataGridViewImageColumn();
            this.gvcEnable = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcRequire = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnCancel = new Wisej.Web.Button();
            this.btnOk = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.gvTaskConfig)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvTaskConfig
            // 
            this.gvTaskConfig.AllowUserToResizeColumns = false;
            this.gvTaskConfig.AllowUserToResizeRows = false;
            this.gvTaskConfig.BackColor = System.Drawing.Color.White;
            this.gvTaskConfig.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTaskConfig.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtFields,
            this.gviEnable,
            this.gviReq,
            this.gvcEnable,
            this.gvcRequire,
            this.gvtCode});
            this.gvTaskConfig.Dock = Wisej.Web.DockStyle.Fill;
            this.gvTaskConfig.Location = new System.Drawing.Point(0, 0);
            this.gvTaskConfig.Name = "gvTaskConfig";
            this.gvTaskConfig.RowHeadersVisible = false;
            this.gvTaskConfig.RowHeadersWidth = 14;
            this.gvTaskConfig.ScrollBars = Wisej.Web.ScrollBars.None;
            this.gvTaskConfig.SelectionMode = Wisej.Web.DataGridViewSelectionMode.CellSelect;
            this.gvTaskConfig.ShowColumnVisibilityMenu = false;
            this.gvTaskConfig.Size = new System.Drawing.Size(333, 263);
            this.gvTaskConfig.TabIndex = 9;
            this.gvTaskConfig.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvcTaskConfig_CellClick);
            // 
            // gvtFields
            // 
            this.gvtFields.Frozen = true;
            this.gvtFields.HeaderText = "   ";
            this.gvtFields.Name = "gvtFields";
            this.gvtFields.ReadOnly = true;
            this.gvtFields.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvtFields.Width = 200;
            // 
            // gviEnable
            // 
            this.gviEnable.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.gviEnable.HeaderText = "Enable";
            this.gviEnable.Name = "gviEnable";
            this.gviEnable.ReadOnly = true;
            this.gviEnable.Width = 50;
            // 
            // gviReq
            // 
            this.gviReq.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.gviReq.HeaderText = "Require";
            this.gviReq.Name = "gviReq";
            this.gviReq.ReadOnly = true;
            this.gviReq.Width = 50;
            // 
            // gvcEnable
            // 
            this.gvcEnable.HeaderText = "EnableVa";
            this.gvcEnable.Name = "gvcEnable";
            this.gvcEnable.Visible = false;
            // 
            // gvcRequire
            // 
            this.gvcRequire.HeaderText = "Requireva";
            this.gvcRequire.Name = "gvcRequire";
            this.gvcRequire.Visible = false;
            // 
            // gvtCode
            // 
            this.gvtCode.HeaderText = "Code";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(265, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 27);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Exit";
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(196, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 27);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "&OK";
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvTaskConfig);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 263);
            this.panel1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 263);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(4);
            this.panel2.Size = new System.Drawing.Size(333, 35);
            this.panel2.TabIndex = 13;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(260, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // HSS00133TaskConfiguration
            // 
            this.ClientSize = new System.Drawing.Size(333, 297);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00133TaskConfiguration";
            this.Text = "HSS00133TaskConfiguration";
            ((System.ComponentModel.ISupportInitialize)(this.gvTaskConfig)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvTaskConfig;
        protected DataGridViewTextBoxColumn gvtFields;
        private DataGridViewImageColumn gviEnable;
        private DataGridViewImageColumn gviReq;
        private Button btnCancel;
        private DataGridViewTextBoxColumn gvcEnable;
        private DataGridViewTextBoxColumn gvcRequire;
        private DataGridViewTextBoxColumn gvtCode;
        private Button btnOk;
        private Panel panel1;
        private Panel panel2;
        private Spacer spacer1;
    }
}