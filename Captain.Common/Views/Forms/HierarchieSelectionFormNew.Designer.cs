using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class HierarchieSelectionFormNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HierarchieSelectionFormNew));
            this.pnlSelHie = new Wisej.Web.Panel();
            this.gvwSelectedHierarachies = new Wisej.Web.DataGridView();
            this.CellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnllblSelHie = new Wisej.Web.Panel();
            this.lblSelected = new Wisej.Web.Label();
            this.pnlChooseHie = new Wisej.Web.Panel();
            this.gvwHierarchie = new Wisej.Web.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnllblChooseHie = new Wisej.Web.Panel();
            this.lblChoose = new Wisej.Web.Label();
            this.flowLayoutPanel1 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.lbltestcount = new Wisej.Web.Label();
            this.pnlSelHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).BeginInit();
            this.pnllblSelHie.SuspendLayout();
            this.pnlChooseHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).BeginInit();
            this.pnllblChooseHie.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSelHie
            // 
            this.pnlSelHie.Controls.Add(this.gvwSelectedHierarachies);
            this.pnlSelHie.Controls.Add(this.pnllblSelHie);
            this.pnlSelHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSelHie.Location = new System.Drawing.Point(0, 0);
            this.pnlSelHie.Name = "pnlSelHie";
            this.pnlSelHie.Padding = new Wisej.Web.Padding(5, 3, 5, 5);
            this.pnlSelHie.Size = new System.Drawing.Size(516, 153);
            this.pnlSelHie.TabIndex = 7;
            this.pnlSelHie.TabStop = true;
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
            this.gvwSelectedHierarachies.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwSelectedHierarachies.Location = new System.Drawing.Point(5, 28);
            this.gvwSelectedHierarachies.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwSelectedHierarachies.Name = "gvwSelectedHierarachies";
            this.gvwSelectedHierarachies.ReadOnly = true;
            this.gvwSelectedHierarachies.RowHeadersVisible = false;
            this.gvwSelectedHierarachies.RowHeadersWidth = 15;
            this.gvwSelectedHierarachies.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwSelectedHierarachies.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSelectedHierarachies.Size = new System.Drawing.Size(506, 120);
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
            this.cellDesc.Width = 300;
            // 
            // pnllblSelHie
            // 
            this.pnllblSelHie.Controls.Add(this.lblSelected);
            this.pnllblSelHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnllblSelHie.Location = new System.Drawing.Point(5, 3);
            this.pnllblSelHie.Name = "pnllblSelHie";
            this.pnllblSelHie.Size = new System.Drawing.Size(506, 25);
            this.pnllblSelHie.TabIndex = 7;
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
            // pnlChooseHie
            // 
            this.pnlChooseHie.Controls.Add(this.gvwHierarchie);
            this.pnlChooseHie.Controls.Add(this.pnllblChooseHie);
            this.pnlChooseHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChooseHie.Location = new System.Drawing.Point(0, 153);
            this.pnlChooseHie.Name = "pnlChooseHie";
            this.pnlChooseHie.Padding = new Wisej.Web.Padding(5);
            this.pnlChooseHie.Size = new System.Drawing.Size(516, 278);
            this.pnlChooseHie.TabIndex = 8;
            this.pnlChooseHie.TabStop = true;
            // 
            // gvwHierarchie
            // 
            this.gvwHierarchie.AllowUserToResizeColumns = false;
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
            this.gvwHierarchie.Location = new System.Drawing.Point(5, 30);
            this.gvwHierarchie.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwHierarchie.MultiSelect = false;
            this.gvwHierarchie.Name = "gvwHierarchie";
            this.gvwHierarchie.RowHeadersVisible = false;
            this.gvwHierarchie.RowHeadersWidth = 15;
            this.gvwHierarchie.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwHierarchie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHierarchie.Size = new System.Drawing.Size(506, 243);
            this.gvwHierarchie.TabIndex = 6;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ShowInVisibilityMenu = false;
            this.dataGridViewCheckBoxColumn1.Width = 60;
            // 
            // Item
            // 
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Item.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Item.HeaderStyle = dataGridViewCellStyle6;
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Width = 350;
            // 
            // pnllblChooseHie
            // 
            this.pnllblChooseHie.Controls.Add(this.lblChoose);
            this.pnllblChooseHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnllblChooseHie.Location = new System.Drawing.Point(5, 5);
            this.pnllblChooseHie.Name = "pnllblChooseHie";
            this.pnllblChooseHie.Size = new System.Drawing.Size(506, 25);
            this.pnllblChooseHie.TabIndex = 8;
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
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.lbltestcount);
            this.flowLayoutPanel1.Controls.Add(this.btnOk);
            this.flowLayoutPanel1.Controls.Add(this.spacer1);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 431);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3, 5, 15, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(516, 35);
            this.flowLayoutPanel1.TabIndex = 9;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.Location = new System.Drawing.Point(374, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(61, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(435, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(438, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // lbltestcount
            // 
            this.lbltestcount.AutoSize = true;
            this.lbltestcount.Dock = Wisej.Web.DockStyle.Left;
            this.lbltestcount.Location = new System.Drawing.Point(3, 5);
            this.lbltestcount.Name = "lbltestcount";
            this.lbltestcount.Size = new System.Drawing.Size(4, 25);
            this.lbltestcount.TabIndex = 6;
            // 
            // HierarchieSelectionFormNew
            // 
            this.ClientSize = new System.Drawing.Size(516, 466);
            this.Controls.Add(this.pnlChooseHie);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.pnlSelHie);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HierarchieSelectionFormNew";
            this.Text = "Hierarchy Selection";
            this.pnlSelHie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).EndInit();
            this.pnllblSelHie.ResumeLayout(false);
            this.pnlChooseHie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).EndInit();
            this.pnllblChooseHie.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private Panel pnlSelHie;
        private DataGridView gvwSelectedHierarachies;
        private DataGridViewTextBoxColumn CellCode;
        private DataGridViewTextBoxColumn cellDesc;
        private Label lblSelected;
        private Panel pnlChooseHie;
        private DataGridView gvwHierarchie;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn Item;
        private Label lblChoose;
        private Panel flowLayoutPanel1;
        private Button btnOk;
        private Button btnCancel;
        private Panel pnllblSelHie;
        private Panel pnllblChooseHie;
        private Spacer spacer1;
        private Label lbltestcount;
    }
}