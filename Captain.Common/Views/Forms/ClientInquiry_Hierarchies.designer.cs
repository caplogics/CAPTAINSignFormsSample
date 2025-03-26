using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ClientInquiry_Hierarchies
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientInquiry_Hierarchies));
            this.Pdfchk = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.CasenotesChk = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.btnOk = new Wisej.Web.Button();
            this.gvwHierarchie = new Wisej.Web.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.lblSelected = new Wisej.Web.Label();
            this.lblChoose = new Wisej.Web.Label();
            this.btnCancel = new Wisej.Web.Button();
            this.gvwSelectedHierarachies = new Wisej.Web.DataGridView();
            this.CellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CellPdf = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.CellCasenotes = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel2 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel4 = new Wisej.Web.Panel();
            this.panel5 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pdfchk
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.Pdfchk.DefaultCellStyle = dataGridViewCellStyle1;
            this.Pdfchk.HeaderText = "Pdf";
            this.Pdfchk.Name = "Pdfchk";
            this.Pdfchk.Width = 40;
            // 
            // CasenotesChk
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.CasenotesChk.DefaultCellStyle = dataGridViewCellStyle2;
            this.CasenotesChk.HeaderText = "Case Notes";
            this.CasenotesChk.Name = "CasenotesChk";
            this.CasenotesChk.Width = 80;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(329, 5);
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
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwHierarchie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvwHierarchie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.Item});
            this.gvwHierarchie.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHierarchie.Location = new System.Drawing.Point(5, 30);
            this.gvwHierarchie.MultiSelect = false;
            this.gvwHierarchie.Name = "gvwHierarchie";
            this.gvwHierarchie.RowHeadersVisible = false;
            this.gvwHierarchie.RowHeadersWidth = 15;
            this.gvwHierarchie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHierarchie.ShowColumnVisibilityMenu = false;
            this.gvwHierarchie.Size = new System.Drawing.Size(483, 195);
            this.gvwHierarchie.TabIndex = 0;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = false;
            this.dataGridViewCheckBoxColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // Item
            // 
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.Item.Width = 320;
            // 
            // lblSelected
            // 
            this.lblSelected.AppearanceKey = "lblsubHeading";
            this.lblSelected.Dock = Wisej.Web.DockStyle.Left;
            this.lblSelected.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSelected.Location = new System.Drawing.Point(0, 0);
            this.lblSelected.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(159, 25);
            this.lblSelected.TabIndex = 4;
            this.lblSelected.Text = "Selected Hierarchies";
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblChoose
            // 
            this.lblChoose.AppearanceKey = "lblsubHeading";
            this.lblChoose.Dock = Wisej.Web.DockStyle.Left;
            this.lblChoose.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblChoose.Location = new System.Drawing.Point(0, 0);
            this.lblChoose.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(168, 25);
            this.lblChoose.TabIndex = 5;
            this.lblChoose.Text = "Choose Hierarchies Here";
            this.lblChoose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(405, 5);
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
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwSelectedHierarachies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gvwSelectedHierarachies.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwSelectedHierarachies.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.CellCode,
            this.cellDesc,
            this.CellPdf,
            this.CellCasenotes});
            this.gvwSelectedHierarachies.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwSelectedHierarachies.Location = new System.Drawing.Point(5, 30);
            this.gvwSelectedHierarachies.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwSelectedHierarachies.Name = "gvwSelectedHierarachies";
            this.gvwSelectedHierarachies.ReadOnly = true;
            this.gvwSelectedHierarachies.RowHeadersVisible = false;
            this.gvwSelectedHierarachies.RowHeadersWidth = 15;
            this.gvwSelectedHierarachies.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSelectedHierarachies.ShowColumnVisibilityMenu = false;
            this.gvwSelectedHierarachies.Size = new System.Drawing.Size(483, 112);
            this.gvwSelectedHierarachies.TabIndex = 3;
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
            this.cellDesc.Width = 280;
            // 
            // CellPdf
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = false;
            this.CellPdf.DefaultCellStyle = dataGridViewCellStyle6;
            this.CellPdf.HeaderText = "Pdf";
            this.CellPdf.Name = "CellPdf";
            this.CellPdf.ReadOnly = true;
            this.CellPdf.Width = 40;
            // 
            // CellCasenotes
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = false;
            this.CellCasenotes.DefaultCellStyle = dataGridViewCellStyle7;
            this.CellCasenotes.HeaderText = "Case Notes";
            this.CellCasenotes.Name = "CellCasenotes";
            this.CellCasenotes.ReadOnly = true;
            this.CellCasenotes.Width = 73;
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 377);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(3, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(493, 35);
            this.panel1.TabIndex = 6;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(402, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblSelected);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(483, 25);
            this.panel2.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblChoose);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(483, 25);
            this.panel3.TabIndex = 8;
            // 
            // Code
            // 
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.Width = 70;
            // 
            // Desc
            // 
            this.Desc.HeaderText = "Description";
            this.Desc.Name = "Desc";
            this.Desc.Width = 250;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gvwSelectedHierarachies);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(5);
            this.panel4.Size = new System.Drawing.Size(493, 147);
            this.panel4.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gvwHierarchie);
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Dock = Wisej.Web.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 147);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new Wisej.Web.Padding(5);
            this.panel5.Size = new System.Drawing.Size(493, 230);
            this.panel5.TabIndex = 10;
            // 
            // ClientInquiry_Hierarchies
            // 
            this.ClientSize = new System.Drawing.Size(493, 412);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientInquiry_Hierarchies";
            this.Text = "Client Inquiry Hierarchies";
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSelectedHierarachies)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Desc;
        private DataGridViewCheckBoxColumn Pdfchk;
        private DataGridViewCheckBoxColumn CasenotesChk;
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
        private DataGridViewCheckBoxColumn CellPdf;
        private DataGridViewCheckBoxColumn CellCasenotes;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Spacer spacer1;
        private Panel panel4;
        private Panel panel5;
    }
}