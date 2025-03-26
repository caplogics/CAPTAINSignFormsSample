namespace Captain.Common.Views.Forms
{
    partial class HierarchieSelectionForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HierarchieSelectionForm));
            this.gvwHierarchie = new Wisej.Web.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnCancel = new Wisej.Web.Button();
            this.btnOk = new Wisej.Web.Button();
            this.gvwSelectedHierarachies = new Wisej.Web.DataGridView();
            this.CellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel2 = new Wisej.Web.Panel();
            this.pnllblChooseHie = new Wisej.Web.Panel();
            this.lblChoose = new Wisej.Web.Label();
            this.panel3 = new Wisej.Web.Panel();
            this.pnllblSelHie = new Wisej.Web.Panel();
            this.lblSelected = new Wisej.Web.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnllblChooseHie.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnllblSelHie.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwHierarchie
            // 
            this.gvwHierarchie.AllowUserToResizeColumns = false;
            this.gvwHierarchie.AllowUserToResizeRows = false;
            this.gvwHierarchie.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwHierarchie.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwHierarchie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwHierarchie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.Item});
            this.gvwHierarchie.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHierarchie.Location = new System.Drawing.Point(0, 25);
            this.gvwHierarchie.MultiSelect = false;
            this.gvwHierarchie.Name = "gvwHierarchie";
            this.gvwHierarchie.RowHeadersVisible = false;
            this.gvwHierarchie.RowHeadersWidth = 15;
            this.gvwHierarchie.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwHierarchie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHierarchie.Size = new System.Drawing.Size(451, 200);
            this.gvwHierarchie.TabIndex = 0;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // Item
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Item.DefaultCellStyle = dataGridViewCellStyle2;
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.Item.Width = 320;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(363, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(300, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // gvwSelectedHierarachies
            // 
            this.gvwSelectedHierarachies.AllowUserToResizeColumns = false;
            this.gvwSelectedHierarachies.AllowUserToResizeRows = false;
            this.gvwSelectedHierarachies.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwSelectedHierarachies.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwSelectedHierarachies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvwSelectedHierarachies.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwSelectedHierarachies.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.CellCode,
            this.cellDesc});
            this.gvwSelectedHierarachies.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwSelectedHierarachies.Location = new System.Drawing.Point(0, 28);
            this.gvwSelectedHierarachies.Name = "gvwSelectedHierarachies";
            this.gvwSelectedHierarachies.ReadOnly = true;
            this.gvwSelectedHierarachies.RowHeadersVisible = false;
            this.gvwSelectedHierarachies.RowHeadersWidth = 15;
            this.gvwSelectedHierarachies.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwSelectedHierarachies.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSelectedHierarachies.Size = new System.Drawing.Size(451, 127);
            this.gvwSelectedHierarachies.TabIndex = 3;
            this.gvwSelectedHierarachies.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.DataGridViewDataError);
            // 
            // CellCode
            // 
            this.CellCode.HeaderText = "Code";
            this.CellCode.Name = "CellCode";
            this.CellCode.ReadOnly = true;
            this.CellCode.Width = 70;
            // 
            // cellDesc
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.cellDesc.DefaultCellStyle = dataGridViewCellStyle4;
            this.cellDesc.HeaderText = "Description";
            this.cellDesc.Name = "cellDesc";
            this.cellDesc.ReadOnly = true;
            this.cellDesc.Width = 335;
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 380);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(451, 35);
            this.panel1.TabIndex = 6;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(360, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwHierarchie);
            this.panel2.Controls.Add(this.pnllblChooseHie);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 155);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 225);
            this.panel2.TabIndex = 7;
            // 
            // pnllblChooseHie
            // 
            this.pnllblChooseHie.Controls.Add(this.lblChoose);
            this.pnllblChooseHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnllblChooseHie.Location = new System.Drawing.Point(0, 0);
            this.pnllblChooseHie.Name = "pnllblChooseHie";
            this.pnllblChooseHie.Size = new System.Drawing.Size(451, 25);
            this.pnllblChooseHie.TabIndex = 9;
            // 
            // lblChoose
            // 
            this.lblChoose.AppearanceKey = "lblsubHeading";
            this.lblChoose.Dock = Wisej.Web.DockStyle.Left;
            this.lblChoose.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblChoose.Location = new System.Drawing.Point(0, 0);
            this.lblChoose.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(185, 25);
            this.lblChoose.TabIndex = 7;
            this.lblChoose.Text = "Choose Hierarchies Here";
            this.lblChoose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwSelectedHierarachies);
            this.panel3.Controls.Add(this.pnllblSelHie);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(0, 3, 0, 0);
            this.panel3.Size = new System.Drawing.Size(451, 155);
            this.panel3.TabIndex = 8;
            // 
            // pnllblSelHie
            // 
            this.pnllblSelHie.Controls.Add(this.lblSelected);
            this.pnllblSelHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnllblSelHie.Location = new System.Drawing.Point(0, 3);
            this.pnllblSelHie.Name = "pnllblSelHie";
            this.pnllblSelHie.Size = new System.Drawing.Size(451, 25);
            this.pnllblSelHie.TabIndex = 8;
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
            // HierarchieSelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(451, 414);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HierarchieSelectionForm";
            this.Text = "Hierarchy Selection";
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnllblChooseHie.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnllblSelHie.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.DataGridView gvwHierarchie;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.Button btnOk;
        private Wisej.Web.DataGridViewTextBoxColumn Item;
        private Wisej.Web.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private Wisej.Web.DataGridView gvwSelectedHierarachies;
        private Wisej.Web.DataGridViewTextBoxColumn CellCode;
        private Wisej.Web.DataGridViewTextBoxColumn cellDesc;
        private Wisej.Web.Panel panel1;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.Panel panel2;
        private Wisej.Web.Panel pnllblChooseHie;
        private Wisej.Web.Label lblChoose;
        private Wisej.Web.Panel panel3;
        private Wisej.Web.Panel pnllblSelHie;
        private Wisej.Web.Label lblSelected;
    }
}