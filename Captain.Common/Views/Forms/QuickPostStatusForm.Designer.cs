namespace Captain.Common.Views.Forms
{
    partial class QuickPostStatusForm
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

        #region Wisej Designer generated code

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickPostStatusForm));
            this.pnlTab1 = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.gvServices = new Wisej.Web.DataGridView();
            this.Sel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.pnlBtns = new Wisej.Web.Panel();
            this.btnTemplate = new Wisej.Web.Button();
            this.btnService = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.btnOk = new Wisej.Web.Button();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Fund = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Amount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Status = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvTemplate = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlTab1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvServices)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlBtns.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTab1
            // 
            this.pnlTab1.Controls.Add(this.panel1);
            this.pnlTab1.Controls.Add(this.panel2);
            this.pnlTab1.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlTab1.Location = new System.Drawing.Point(0, 0);
            this.pnlTab1.Name = "pnlTab1";
            this.pnlTab1.Size = new System.Drawing.Size(705, 260);
            this.pnlTab1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvServices);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(705, 225);
            this.panel1.TabIndex = 1;
            // 
            // gvServices
            // 
            this.gvServices.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvServices.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Sel,
            this.Code,
            this.Desc,
            this.Fund,
            this.Amount,
            this.Status,
            this.gvTemplate});
            this.gvServices.Dock = Wisej.Web.DockStyle.Fill;
            this.gvServices.Location = new System.Drawing.Point(0, 0);
            this.gvServices.Name = "gvServices";
            this.gvServices.RowHeadersWidth = 25;
            this.gvServices.Size = new System.Drawing.Size(705, 225);
            this.gvServices.TabIndex = 0;
            this.gvServices.SelectionChanged += new System.EventHandler(this.gvServices_SelectionChanged);
            this.gvServices.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvServices_CellClick);
            // 
            // Sel
            // 
            this.Sel.HeaderText = " ";
            this.Sel.Name = "Sel";
            this.Sel.Width = 40;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.pnlBtns);
            this.panel2.Controls.Add(this.pnlSave);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 225);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(4);
            this.panel2.Size = new System.Drawing.Size(705, 35);
            this.panel2.TabIndex = 2;
            // 
            // pnlBtns
            // 
            this.pnlBtns.BackColor = System.Drawing.Color.Transparent;
            this.pnlBtns.Controls.Add(this.btnTemplate);
            this.pnlBtns.Controls.Add(this.btnService);
            this.pnlBtns.Controls.Add(this.btnCancel);
            this.pnlBtns.Dock = Wisej.Web.DockStyle.Right;
            this.pnlBtns.Location = new System.Drawing.Point(200, 4);
            this.pnlBtns.Name = "pnlBtns";
            this.pnlBtns.Size = new System.Drawing.Size(355, 27);
            this.pnlBtns.TabIndex = 3;
            // 
            // btnTemplate
            // 
            this.btnTemplate.Dock = Wisej.Web.DockStyle.Right;
            this.btnTemplate.Location = new System.Drawing.Point(25, 0);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(179, 27);
            this.btnTemplate.TabIndex = 1;
            this.btnTemplate.Text = "Move Template Service & Save";
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnService
            // 
            this.btnService.Dock = Wisej.Web.DockStyle.Right;
            this.btnService.Location = new System.Drawing.Point(204, 0);
            this.btnService.Name = "btnService";
            this.btnService.Size = new System.Drawing.Size(87, 27);
            this.btnService.TabIndex = 2;
            this.btnService.Text = "Edit Service";
            this.btnService.Click += new System.EventHandler(this.btnService_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(291, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 27);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.BackColor = System.Drawing.Color.Transparent;
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Right;
            this.pnlSave.Location = new System.Drawing.Point(555, 4);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(82, 27);
            this.pnlSave.TabIndex = 2;
            this.pnlSave.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(18, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 27);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOk
            // 
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(637, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 27);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // Code
            // 
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ShowInVisibilityMenu = false;
            this.Code.Visible = false;
            // 
            // Desc
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Desc.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Desc.HeaderStyle = dataGridViewCellStyle2;
            this.Desc.HeaderText = "Desc";
            this.Desc.Name = "Desc";
            this.Desc.Width = 320;
            // 
            // Fund
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Fund.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Fund.HeaderStyle = dataGridViewCellStyle4;
            this.Fund.HeaderText = "Fund";
            this.Fund.Name = "Fund";
            // 
            // Amount
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Amount.HeaderStyle = dataGridViewCellStyle6;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.Width = 80;
            // 
            // Status
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Status.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Status.HeaderStyle = dataGridViewCellStyle8;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // gvTemplate
            // 
            this.gvTemplate.HeaderText = " ";
            this.gvTemplate.Name = "gvTemplate";
            this.gvTemplate.Width = 30;
            // 
            // QuickPostStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 260);
            this.Controls.Add(this.pnlTab1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickPostStatusForm";
            this.Text = "Quick Post Status ";
            this.pnlTab1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvServices)).EndInit();
            this.panel2.ResumeLayout(false);
            this.pnlBtns.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlTab1;
        private Wisej.Web.DataGridView gvServices;
        private Wisej.Web.DataGridViewTextBoxColumn Code;
        private Wisej.Web.DataGridViewTextBoxColumn Desc;
        private Wisej.Web.DataGridViewTextBoxColumn Status;
        private Wisej.Web.Panel panel1;
        private Wisej.Web.Panel panel2;
        private Wisej.Web.Button btnOk;
        private Wisej.Web.DataGridViewTextBoxColumn Fund;
        private Wisej.Web.DataGridViewTextBoxColumn Amount;
        private Wisej.Web.DataGridViewCheckBoxColumn Sel;
        private Wisej.Web.Panel pnlSave;
        private Wisej.Web.Button btnSave;
        private Wisej.Web.DataGridViewTextBoxColumn gvTemplate;
        private Wisej.Web.Panel pnlBtns;
        private Wisej.Web.Button btnTemplate;
        private Wisej.Web.Button btnService;
        private Wisej.Web.Button btnCancel;
    }
}