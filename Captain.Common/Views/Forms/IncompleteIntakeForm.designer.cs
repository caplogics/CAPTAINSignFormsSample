using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class IncompleteIntakeForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncompleteIntakeForm));
            this.panel1 = new Wisej.Web.Panel();
            this.btnClose = new Wisej.Web.Button();
            this.pnlDetails = new Wisej.Web.Panel();
            this.txtLine3 = new Wisej.Web.TextBox();
            this.label3 = new Wisej.Web.Label();
            this.txtLine2 = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.txtLine1 = new Wisej.Web.TextBox();
            this.label24 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.gvwIncompletedata = new Wisej.Web.DataGridView();
            this.gvchkSelect = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtLine1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtLine2 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtLine3 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.txtLine3.SuspendLayout();
            this.txtLine2.SuspendLayout();
            this.txtLine1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwIncompletedata)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.pnlDetails);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(674, 315);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-error";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(621, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(49, 27);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlDetails
            // 
            this.pnlDetails.BackColor = System.Drawing.Color.FromArgb(247, 247, 247);
            this.pnlDetails.Controls.Add(this.txtLine3);
            this.pnlDetails.Controls.Add(this.txtLine2);
            this.pnlDetails.Controls.Add(this.txtLine1);
            this.pnlDetails.Controls.Add(this.label1);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlDetails.Location = new System.Drawing.Point(0, 182);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(674, 98);
            this.pnlDetails.TabIndex = 2;
            // 
            // txtLine3
            // 
            this.txtLine3.Controls.Add(this.label3);
            this.txtLine3.Location = new System.Drawing.Point(58, 67);
            this.txtLine3.MaxLength = 80;
            this.txtLine3.Name = "txtLine3";
            this.txtLine3.ReadOnly = true;
            this.txtLine3.Size = new System.Drawing.Size(591, 25);
            this.txtLine3.TabIndex = 5;
            this.txtLine3.Leave += new System.EventHandler(this.txtLine3_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 354);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(4, 14);
            this.label3.TabIndex = 8;
            // 
            // txtLine2
            // 
            this.txtLine2.Controls.Add(this.label2);
            this.txtLine2.Location = new System.Drawing.Point(58, 39);
            this.txtLine2.MaxLength = 80;
            this.txtLine2.Name = "txtLine2";
            this.txtLine2.ReadOnly = true;
            this.txtLine2.Size = new System.Drawing.Size(591, 25);
            this.txtLine2.TabIndex = 4;
            this.txtLine2.Leave += new System.EventHandler(this.txtLine2_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 354);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(4, 14);
            this.label2.TabIndex = 8;
            // 
            // txtLine1
            // 
            this.txtLine1.Controls.Add(this.label24);
            this.txtLine1.Location = new System.Drawing.Point(58, 11);
            this.txtLine1.MaxLength = 80;
            this.txtLine1.Name = "txtLine1";
            this.txtLine1.ReadOnly = true;
            this.txtLine1.Size = new System.Drawing.Size(591, 25);
            this.txtLine1.TabIndex = 3;
            this.txtLine1.Leave += new System.EventHandler(this.txtLine1_Leave);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(4, 354);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(4, 14);
            this.label24.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Details";
            // 
            // gvwIncompletedata
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwIncompletedata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwIncompletedata.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvchkSelect,
            this.gvtDesc,
            this.gvtCode,
            this.gvtLine1,
            this.gvtLine2,
            this.gvtLine3});
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.False;
            this.gvwIncompletedata.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvwIncompletedata.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwIncompletedata.Location = new System.Drawing.Point(0, 0);
            this.gvwIncompletedata.MultiSelect = false;
            this.gvwIncompletedata.Name = "gvwIncompletedata";
            this.gvwIncompletedata.RowHeadersWidth = 14;
            this.gvwIncompletedata.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwIncompletedata.SelectionMode = Wisej.Web.DataGridViewSelectionMode.CellSelect;
            this.gvwIncompletedata.ShowColumnVisibilityMenu = false;
            this.gvwIncompletedata.Size = new System.Drawing.Size(674, 182);
            this.gvwIncompletedata.TabIndex = 1;
            this.gvwIncompletedata.SelectionChanged += new System.EventHandler(this.gvwIncompletedata_SelectionChanged);
            this.gvwIncompletedata.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwIncompletedata_CellClick);
            // 
            // gvchkSelect
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.gvchkSelect.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvchkSelect.HeaderText = "   ";
            this.gvchkSelect.Name = "gvchkSelect";
            this.gvchkSelect.ReadOnly = true;
            this.gvchkSelect.Width = 30;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 280);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(4);
            this.panel2.Size = new System.Drawing.Size(674, 35);
            this.panel2.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwIncompletedata);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(674, 182);
            this.panel3.TabIndex = 9;
            // 
            // gvtDesc
            // 
            this.gvtDesc.HeaderText = "Description";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.gvtDesc.Width = 535;
            // 
            // gvtCode
            // 
            this.gvtCode.HeaderText = "gvtCode";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.Visible = false;
            this.gvtCode.Width = 10;
            // 
            // gvtLine1
            // 
            this.gvtLine1.HeaderText = "gvtLine1";
            this.gvtLine1.Name = "gvtLine1";
            this.gvtLine1.ReadOnly = true;
            this.gvtLine1.Visible = false;
            this.gvtLine1.Width = 10;
            // 
            // gvtLine2
            // 
            this.gvtLine2.HeaderText = "gvtLine2";
            this.gvtLine2.Name = "gvtLine2";
            this.gvtLine2.ReadOnly = true;
            this.gvtLine2.Visible = false;
            this.gvtLine2.Width = 10;
            // 
            // gvtLine3
            // 
            this.gvtLine3.HeaderText = "gvtLine3";
            this.gvtLine3.Name = "gvtLine3";
            this.gvtLine3.ReadOnly = true;
            this.gvtLine3.Visible = false;
            this.gvtLine3.Width = 10;
            // 
            // IncompleteIntakeForm
            // 
            this.ClientSize = new System.Drawing.Size(674, 315);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IncompleteIntakeForm";
            this.Text = "IncompleteIntakeForm";
            this.panel1.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.txtLine3.ResumeLayout(false);
            this.txtLine3.PerformLayout();
            this.txtLine2.ResumeLayout(false);
            this.txtLine2.PerformLayout();
            this.txtLine1.ResumeLayout(false);
            this.txtLine1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwIncompletedata)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Panel panel1;
        private DataGridView gvwIncompletedata;
        private Panel pnlDetails;
        private Label label1;
        private TextBox txtLine3;
        private Label label3;
        private TextBox txtLine2;
        private Label label2;
        private TextBox txtLine1;
        private Label label24;
        private Button btnClose;
        private DataGridViewCheckBoxColumn gvchkSelect;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewTextBoxColumn gvtLine1;
        private DataGridViewTextBoxColumn gvtLine2;
        private DataGridViewTextBoxColumn gvtLine3;
        private DataGridViewTextBoxColumn gvtCode;
        private Panel panel3;
        private Panel panel2;
    }
}