using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class HierarchieSelection
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HierarchieSelection));
            this.panel1 = new Wisej.Web.Panel();
            this.gvwSelectedHierarachies = new Wisej.Web.DataGridView();
            this.CellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel3 = new Wisej.Web.Panel();
            this.lblSelected = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.gvwHierarchie = new Wisej.Web.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel4 = new Wisej.Web.Panel();
            this.cmbAgency = new Wisej.Web.ComboBox();
            this.lblAgency = new Wisej.Web.Label();
            this.lblChoose = new Wisej.Web.Label();
            this.flowLayoutPanel1 = new Wisej.Web.Panel();
            this.btnSetdefaultHIE = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).BeginInit();
            this.panel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvwSelectedHierarachies);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 3, 5, 5);
            this.panel1.Size = new System.Drawing.Size(512, 153);
            this.panel1.TabIndex = 7;
            this.panel1.TabStop = true;
            // 
            // gvwSelectedHierarachies
            // 
            this.gvwSelectedHierarachies.AllowUserToResizeColumns = false;
            this.gvwSelectedHierarachies.AllowUserToResizeRows = false;
            this.gvwSelectedHierarachies.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwSelectedHierarachies.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwSelectedHierarachies.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwSelectedHierarachies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwSelectedHierarachies.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwSelectedHierarachies.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.CellCode,
            this.cellDesc});
            this.gvwSelectedHierarachies.Cursor = Wisej.Web.Cursors.None;
            this.gvwSelectedHierarachies.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwSelectedHierarachies.Location = new System.Drawing.Point(5, 28);
            this.gvwSelectedHierarachies.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwSelectedHierarachies.Name = "gvwSelectedHierarachies";
            this.gvwSelectedHierarachies.ReadOnly = true;
            this.gvwSelectedHierarachies.RowHeadersVisible = false;
            this.gvwSelectedHierarachies.RowHeadersWidth = 15;
            this.gvwSelectedHierarachies.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwSelectedHierarachies.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSelectedHierarachies.Size = new System.Drawing.Size(502, 120);
            this.gvwSelectedHierarachies.TabIndex = 5;
            // 
            // CellCode
            // 
            this.CellCode.HeaderText = "Code";
            this.CellCode.Name = "CellCode";
            // 
            // cellDesc
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.cellDesc.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.cellDesc.HeaderStyle = dataGridViewCellStyle3;
            this.cellDesc.HeaderText = "Description";
            this.cellDesc.Name = "cellDesc";
            this.cellDesc.Width = 360;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblSelected);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(502, 25);
            this.panel3.TabIndex = 7;
            // 
            // lblSelected
            // 
            this.lblSelected.AppearanceKey = "lblsubHeading";
            this.lblSelected.Dock = Wisej.Web.DockStyle.Left;
            this.lblSelected.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSelected.Location = new System.Drawing.Point(0, 0);
            this.lblSelected.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(139, 25);
            this.lblSelected.TabIndex = 6;
            this.lblSelected.Text = "Selected Hierarchies";
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwHierarchie);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 153);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5);
            this.panel2.Size = new System.Drawing.Size(512, 274);
            this.panel2.TabIndex = 8;
            this.panel2.TabStop = true;
            // 
            // gvwHierarchie
            // 
            this.gvwHierarchie.AllowUserToResizeRows = false;
            this.gvwHierarchie.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwHierarchie.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwHierarchie.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwHierarchie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gvwHierarchie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.Item});
            this.gvwHierarchie.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHierarchie.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.gvwHierarchie.Location = new System.Drawing.Point(5, 62);
            this.gvwHierarchie.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwHierarchie.MultiSelect = false;
            this.gvwHierarchie.Name = "gvwHierarchie";
            this.gvwHierarchie.RowHeadersVisible = false;
            this.gvwHierarchie.RowHeadersWidth = 15;
            this.gvwHierarchie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHierarchie.Size = new System.Drawing.Size(502, 207);
            this.gvwHierarchie.TabIndex = 6;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = false;
            this.dataGridViewCheckBoxColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewCheckBoxColumn1.HeaderStyle = dataGridViewCellStyle6;
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 55;
            // 
            // Item
            // 
            dataGridViewCellStyle7.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Item.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Item.HeaderStyle = dataGridViewCellStyle8;
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.Item.Width = 420;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cmbAgency);
            this.panel4.Controls.Add(this.lblAgency);
            this.panel4.Controls.Add(this.lblChoose);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(5, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(502, 57);
            this.panel4.TabIndex = 8;
            // 
            // cmbAgency
            // 
            this.cmbAgency.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAgency.Location = new System.Drawing.Point(59, 27);
            this.cmbAgency.Name = "cmbAgency";
            this.cmbAgency.Size = new System.Drawing.Size(232, 25);
            this.cmbAgency.TabIndex = 9;
            this.cmbAgency.SelectedIndexChanged += new System.EventHandler(this.cmbAgency_SelectedIndexChanged);
            // 
            // lblAgency
            // 
            this.lblAgency.AutoSize = true;
            this.lblAgency.Location = new System.Drawing.Point(7, 30);
            this.lblAgency.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblAgency.Name = "lblAgency";
            this.lblAgency.Size = new System.Drawing.Size(45, 18);
            this.lblAgency.TabIndex = 8;
            this.lblAgency.Text = "Agency";
            // 
            // lblChoose
            // 
            this.lblChoose.AppearanceKey = "lblsubHeading";
            this.lblChoose.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblChoose.Location = new System.Drawing.Point(0, 0);
            this.lblChoose.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(185, 25);
            this.lblChoose.TabIndex = 7;
            this.lblChoose.Text = "Choose Hierarchies Here";
            this.lblChoose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnSetdefaultHIE);
            this.flowLayoutPanel1.Controls.Add(this.spacer2);
            this.flowLayoutPanel1.Controls.Add(this.btnOk);
            this.flowLayoutPanel1.Controls.Add(this.spacer1);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 427);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(10, 5, 15, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(512, 35);
            this.flowLayoutPanel1.TabIndex = 9;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnSetdefaultHIE
            // 
            this.btnSetdefaultHIE.Dock = Wisej.Web.DockStyle.Left;
            this.btnSetdefaultHIE.Location = new System.Drawing.Point(10, 5);
            this.btnSetdefaultHIE.Name = "btnSetdefaultHIE";
            this.btnSetdefaultHIE.Size = new System.Drawing.Size(142, 25);
            this.btnSetdefaultHIE.TabIndex = 6;
            this.btnSetdefaultHIE.Text = "&Set Default Hierarchy";
            this.btnSetdefaultHIE.Visible = false;
            this.btnSetdefaultHIE.Click += new System.EventHandler(this.btnSetdefaultHIE_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(367, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.Location = new System.Drawing.Point(370, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(61, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(431, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(434, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // HierarchieSelection
            // 
            this.ClientSize = new System.Drawing.Size(512, 462);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HierarchieSelection";
            this.Text = "Hierarchy Selection";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private Panel panel1;
        private DataGridView gvwSelectedHierarachies;
        private DataGridViewTextBoxColumn CellCode;
        private DataGridViewTextBoxColumn cellDesc;
        private Label lblSelected;
        private Panel panel2;
        private DataGridView gvwHierarchie;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn Item;
        private Label lblChoose;
        private Panel flowLayoutPanel1;
        private Button btnOk;
        private Button btnCancel;
        private Panel panel3;
        private Panel panel4;
        private Spacer spacer1;
        private ComboBox cmbAgency;
        private Label lblAgency;
        private Button btnSetdefaultHIE;
        private Spacer spacer2;
    }
}