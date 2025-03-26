namespace Captain.Common.Views.Forms
{
    partial class PrivilegeSelectionForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrivilegeSelectionForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.gvwPrivileges = new Wisej.Web.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnCancel = new Wisej.Web.Button();
            this.btnOk = new Wisej.Web.Button();
            this.lblSelected = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel2 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwPrivileges)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwPrivileges
            // 
            this.gvwPrivileges.AllowUserToResizeColumns = false;
            this.gvwPrivileges.AllowUserToResizeRows = false;
            this.gvwPrivileges.BackColor = System.Drawing.Color.White;
            this.gvwPrivileges.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwPrivileges.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwPrivileges.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.Item});
            this.gvwPrivileges.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwPrivileges.Location = new System.Drawing.Point(0, 0);
            this.gvwPrivileges.MultiSelect = false;
            this.gvwPrivileges.Name = "gvwPrivileges";
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwPrivileges.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvwPrivileges.RowHeadersVisible = false;
            this.gvwPrivileges.RowHeadersWidth = 15;
            this.gvwPrivileges.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwPrivileges.ShowColumnVisibilityMenu = false;
            this.gvwPrivileges.Size = new System.Drawing.Size(437, 274);
            this.gvwPrivileges.TabIndex = 0;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // Item
            // 
            this.Item.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.Fill;
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(365, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(304, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(58, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // lblSelected
            // 
            this.lblSelected.AppearanceKey = "lblsubHeading";
            this.lblSelected.Dock = Wisej.Web.DockStyle.Left;
            this.lblSelected.Location = new System.Drawing.Point(0, 0);
            this.lblSelected.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(148, 25);
            this.lblSelected.TabIndex = 4;
            this.lblSelected.Text = "Select Template User";
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 309);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(447, 35);
            this.panel1.TabIndex = 5;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(362, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5);
            this.panel2.Size = new System.Drawing.Size(447, 309);
            this.panel2.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gvwPrivileges);
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 30);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(437, 274);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblSelected);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(437, 25);
            this.panel3.TabIndex = 0;
            // 
            // PrivilegeSelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(447, 344);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrivilegeSelectionForm";
            this.Text = "Global Privilege Selection - Copy From";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "TL_HELP";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.PrivilegeSelectionForm_Load);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.PrivilegeSelectionForm_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.gvwPrivileges)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.DataGridView gvwPrivileges;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Button btnOk;
        private Wisej.Web.DataGridViewTextBoxColumn Item;
        private Wisej.Web.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private Wisej.Web.Label lblSelected;
        private Wisej.Web.Panel panel1;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.Panel panel2;
        private Wisej.Web.Panel panel3;
        private Wisej.Web.Panel panel4;
    }
}