using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PaymentFieldControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentFieldControlForm));
            this.gvwProgramCode = new Wisej.Web.DataGridView();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvFEnabled = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvFRequired = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvcEnabled = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvcRequied = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.btnDelete = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.panel4 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.label1 = new Wisej.Web.Label();
            this.lblPFC = new Wisej.Web.Label();
            this.lblCADesc = new Wisej.Web.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvwProgramCode)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwProgramCode
            // 
            this.gvwProgramCode.AllowUserToResizeColumns = false;
            this.gvwProgramCode.AllowUserToResizeRows = false;
            this.gvwProgramCode.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwProgramCode.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwProgramCode.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwProgramCode.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwProgramCode.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtDesc,
            this.gvFEnabled,
            this.gvFRequired,
            this.gvcEnabled,
            this.gvcRequied,
            this.gvtCode});
            this.gvwProgramCode.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwProgramCode.Location = new System.Drawing.Point(0, 0);
            this.gvwProgramCode.MultiSelect = false;
            this.gvwProgramCode.Name = "gvwProgramCode";
            this.gvwProgramCode.RowHeadersWidth = 14;
            this.gvwProgramCode.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwProgramCode.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwProgramCode.Size = new System.Drawing.Size(625, 336);
            this.gvwProgramCode.TabIndex = 4;
            this.gvwProgramCode.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwProgramCode_CellClick);
            // 
            // gvtDesc
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtDesc.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtDesc.HeaderStyle = dataGridViewCellStyle3;
            this.gvtDesc.HeaderText = "Description";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.gvtDesc.Width = 290;
            // 
            // gvFEnabled
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = false;
            this.gvFEnabled.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvFEnabled.HeaderText = "Enabled";
            this.gvFEnabled.Name = "gvFEnabled";
            this.gvFEnabled.ReadOnly = true;
            this.gvFEnabled.Width = 85;
            // 
            // gvFRequired
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = false;
            this.gvFRequired.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvFRequired.HeaderText = "Required";
            this.gvFRequired.Name = "gvFRequired";
            this.gvFRequired.ReadOnly = true;
            this.gvFRequired.Width = 85;
            // 
            // gvcEnabled
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = false;
            this.gvcEnabled.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvcEnabled.HeaderText = "Enabled";
            this.gvcEnabled.Name = "gvcEnabled";
            this.gvcEnabled.Width = 60;
            // 
            // gvcRequied
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = false;
            this.gvcRequied.DefaultCellStyle = dataGridViewCellStyle7;
            this.gvcRequied.HeaderText = "Required";
            this.gvcRequied.Name = "gvcRequied";
            this.gvcRequied.Width = 60;
            // 
            // gvtCode
            // 
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.Visible = false;
            this.gvtCode.Width = 10;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.lblCADesc);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(627, 398);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.spacer2);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 361);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(4);
            this.panel2.Size = new System.Drawing.Size(625, 35);
            this.panel2.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = Wisej.Web.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(208, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(171, 27);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "&Delete Service Definitions";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(379, 4);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(113, 27);
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(492, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(62, 27);
            this.btnOk.TabIndex = 7;
            this.btnOk.Tag = "";
            this.btnOk.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(554, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(559, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 27);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gvwProgramCode);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 25);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(625, 336);
            this.panel4.TabIndex = 8;
            this.panel4.Text = "`";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.lblPFC);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(625, 25);
            this.panel3.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(241, 241, 241);
            this.label1.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(477, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Service Definitions";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPFC
            // 
            this.lblPFC.BackColor = System.Drawing.Color.FromArgb(241, 241, 241);
            this.lblPFC.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblPFC.Location = new System.Drawing.Point(323, 1);
            this.lblPFC.Name = "lblPFC";
            this.lblPFC.Size = new System.Drawing.Size(152, 24);
            this.lblPFC.TabIndex = 6;
            this.lblPFC.Text = "Program Definitions";
            this.lblPFC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCADesc
            // 
            this.lblCADesc.AutoSize = true;
            this.lblCADesc.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCADesc.ForeColor = System.Drawing.Color.Blue;
            this.lblCADesc.Location = new System.Drawing.Point(8, 1);
            this.lblCADesc.Name = "lblCADesc";
            this.lblCADesc.Size = new System.Drawing.Size(4, 13);
            this.lblCADesc.TabIndex = 6;
            // 
            // PaymentFieldControlForm
            // 
            this.ClientSize = new System.Drawing.Size(627, 398);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaymentFieldControlForm";
            this.Text = "ProgramFieldControlForm";
            ((System.ComponentModel.ISupportInitialize)(this.gvwProgramCode)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private DataGridView gvwProgramCode;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewCheckBoxColumn gvcEnabled;
        private DataGridViewCheckBoxColumn gvcRequied;
        private Panel panel1;
        private Panel panel2;
        private Button btnOk;
        private Button btnCancel;
        private DataGridViewTextBoxColumn gvtCode;
        private DataGridViewCheckBoxColumn gvFEnabled;
        private DataGridViewCheckBoxColumn gvFRequired;
        private Label label1;
        private Label lblPFC;
        private Label lblCADesc;
        private Button btnDelete;
        private Spacer spacer2;
        private Spacer spacer1;
        private Panel panel4;
        private Panel panel3;
    }
}