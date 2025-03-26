using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class SERHIESEL_FORM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SERHIESEL_FORM));
            this.btnOk = new Wisej.Web.Button();
            this.gvwHierarchie = new Wisej.Web.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.lblSelected = new Wisej.Web.Label();
            this.lblChoose = new Wisej.Web.Label();
            this.btnCancel = new Wisej.Web.Button();
            this.gvwSelectedHierarachies = new Wisej.Web.DataGridView();
            this.chkShowAll = new Wisej.Web.CheckBox();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel3 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.panel5 = new Wisej.Web.Panel();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(338, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(73, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // gvwHierarchie
            // 
            this.gvwHierarchie.AllowUserToResizeRows = false;
            this.gvwHierarchie.BackColor = System.Drawing.Color.White;
            this.gvwHierarchie.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwHierarchie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwHierarchie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.Item});
            this.gvwHierarchie.Dock = Wisej.Web.DockStyle.Top;
            this.gvwHierarchie.Location = new System.Drawing.Point(5, 30);
            this.gvwHierarchie.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwHierarchie.MultiSelect = false;
            this.gvwHierarchie.Name = "gvwHierarchie";
            this.gvwHierarchie.RowHeadersVisible = false;
            this.gvwHierarchie.RowHeadersWidth = 15;
            this.gvwHierarchie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHierarchie.Size = new System.Drawing.Size(492, 205);
            this.gvwHierarchie.TabIndex = 0;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.dataGridViewCheckBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ShowInVisibilityMenu = false;
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // lblSelected
            // 
            this.lblSelected.AppearanceKey = "lblsubHeading";
            this.lblSelected.Dock = Wisej.Web.DockStyle.Left;
            this.lblSelected.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSelected.Location = new System.Drawing.Point(0, 0);
            this.lblSelected.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Padding = new Wisej.Web.Padding(5);
            this.lblSelected.Size = new System.Drawing.Size(139, 25);
            this.lblSelected.TabIndex = 4;
            this.lblSelected.Text = "Intake Hierarchies";
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblChoose
            // 
            this.lblChoose.AppearanceKey = "lblsubHeading";
            this.lblChoose.Dock = Wisej.Web.DockStyle.Left;
            this.lblChoose.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblChoose.Location = new System.Drawing.Point(0, 0);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(213, 25);
            this.lblChoose.TabIndex = 5;
            this.lblChoose.Text = "Choose SP Hierarchies here";
            this.lblChoose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(414, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gvwSelectedHierarachies
            // 
            this.gvwSelectedHierarachies.AllowUserToResizeColumns = false;
            this.gvwSelectedHierarachies.AllowUserToResizeRows = false;
            this.gvwSelectedHierarachies.BackColor = System.Drawing.Color.White;
            this.gvwSelectedHierarachies.BorderStyle = Wisej.Web.BorderStyle.None;
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
            this.gvwSelectedHierarachies.Dock = Wisej.Web.DockStyle.Top;
            this.gvwSelectedHierarachies.Location = new System.Drawing.Point(5, 30);
            this.gvwSelectedHierarachies.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwSelectedHierarachies.Name = "gvwSelectedHierarachies";
            this.gvwSelectedHierarachies.ReadOnly = true;
            this.gvwSelectedHierarachies.RowHeadersVisible = false;
            this.gvwSelectedHierarachies.RowHeadersWidth = 15;
            this.gvwSelectedHierarachies.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSelectedHierarachies.Size = new System.Drawing.Size(492, 108);
            this.gvwSelectedHierarachies.TabIndex = 3;
            this.gvwSelectedHierarachies.SelectionChanged += new System.EventHandler(this.gvwSelectedHierarachies_SelectionChanged);
            // 
            // chkShowAll
            // 
            this.chkShowAll.Checked = true;
            this.chkShowAll.Dock = Wisej.Web.DockStyle.Right;
            this.chkShowAll.Location = new System.Drawing.Point(413, 0);
            this.chkShowAll.Name = "chkShowAll";
            this.chkShowAll.Size = new System.Drawing.Size(79, 25);
            this.chkShowAll.TabIndex = 6;
            this.chkShowAll.Text = "Show All";
            this.chkShowAll.Visible = false;
            this.chkShowAll.CheckedChanged += new System.EventHandler(this.chkShowAll_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblChoose);
            this.panel1.Controls.Add(this.chkShowAll);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 25);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 372);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(502, 35);
            this.panel2.TabIndex = 8;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(411, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblSelected);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(492, 25);
            this.panel3.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gvwSelectedHierarachies);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(5);
            this.panel4.Size = new System.Drawing.Size(502, 136);
            this.panel4.TabIndex = 10;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gvwHierarchie);
            this.panel5.Controls.Add(this.panel1);
            this.panel5.Dock = Wisej.Web.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 136);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new Wisej.Web.Padding(5);
            this.panel5.Size = new System.Drawing.Size(502, 236);
            this.panel5.TabIndex = 11;
            // 
            // Item
            // 
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.Item.Width = 320;
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
            this.cellDesc.HeaderText = "Description";
            this.cellDesc.Name = "cellDesc";
            this.cellDesc.ReadOnly = true;
            this.cellDesc.Width = 335;
            // 
            // SERHIESEL_FORM
            // 
            this.ClientSize = new System.Drawing.Size(502, 407);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SERHIESEL_FORM";
            this.Text = "SERHIESEL_FORM";
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Button btnOk;
        private DataGridView gvwHierarchie;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn Item;
        private Label lblSelected;
        private Label lblChoose;
        private Button btnCancel;
        private DataGridViewTextBoxColumn CellCode;
        private DataGridViewTextBoxColumn cellDesc;
        private DataGridView gvwSelectedHierarachies;
        private CheckBox chkShowAll;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Spacer spacer1;
        private Panel panel4;
        private Panel panel5;
    }
}