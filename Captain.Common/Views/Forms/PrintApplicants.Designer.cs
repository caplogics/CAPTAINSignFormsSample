using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class PrintApplicants
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintApplicants));
            this.panel1 = new Wisej.Web.Panel();
            this.lblName = new Wisej.Web.Label();
            this.lblAppNo = new Wisej.Web.Label();
            this.lblN = new Wisej.Web.Label();
            this.lblApp = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.gvApp = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.AppDet = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dgvForm = new Wisej.Web.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnExit = new Wisej.Web.Button();
            this.btnPrev = new Wisej.Web.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvApp)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(175, 244, 244, 244);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.lblAppNo);
            this.panel1.Controls.Add(this.lblN);
            this.panel1.Controls.Add(this.lblApp);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(363, 56);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblName.Location = new System.Drawing.Point(67, 33);
            this.lblName.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblName.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(37, 18);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // lblAppNo
            // 
            this.lblAppNo.AutoSize = true;
            this.lblAppNo.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAppNo.Location = new System.Drawing.Point(67, 8);
            this.lblAppNo.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblAppNo.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblAppNo.Name = "lblAppNo";
            this.lblAppNo.Size = new System.Drawing.Size(33, 18);
            this.lblAppNo.TabIndex = 0;
            this.lblAppNo.Text = "App#";
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblN.Location = new System.Drawing.Point(15, 33);
            this.lblN.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblN.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(43, 18);
            this.lblN.TabIndex = 0;
            this.lblN.Text = "Name :";
            // 
            // lblApp
            // 
            this.lblApp.AutoSize = true;
            this.lblApp.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApp.Location = new System.Drawing.Point(15, 8);
            this.lblApp.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblApp.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblApp.Name = "lblApp";
            this.lblApp.Size = new System.Drawing.Size(39, 18);
            this.lblApp.TabIndex = 0;
            this.lblApp.Text = "App# :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvApp);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(363, 208);
            this.panel2.TabIndex = 1;
            this.panel2.TabStop = true;
            // 
            // gvApp
            // 
            this.gvApp.AllowUserToResizeColumns = false;
            this.gvApp.AllowUserToResizeRows = false;
            this.gvApp.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvApp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvApp.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.AppDet,
            this.dgvForm});
            dataGridViewCellStyle4.CssStyle = "";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvApp.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvApp.Dock = Wisej.Web.DockStyle.Fill;
            this.gvApp.Name = "gvApp";
            this.gvApp.RowHeadersVisible = false;
            this.gvApp.RowHeadersWidth = 25;
            this.gvApp.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvApp.ShowColumnVisibilityMenu = false;
            this.gvApp.Size = new System.Drawing.Size(363, 208);
            this.gvApp.TabIndex = 0;
            this.gvApp.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvApp_CellValueChanged);
            this.gvApp.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvApp_CellClick);
            // 
            // AppDet
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AppDet.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.Padding = new Wisej.Web.Padding(8, 0, 0, 0);
            this.AppDet.HeaderStyle = dataGridViewCellStyle3;
            this.AppDet.HeaderText = "Application Details";
            this.AppDet.Name = "AppDet";
            this.AppDet.ReadOnly = true;
            this.AppDet.Width = 310;
            // 
            // dgvForm
            // 
            this.dgvForm.HeaderText = "Form";
            this.dgvForm.Name = "dgvForm";
            this.dgvForm.ShowInVisibilityMenu = false;
            this.dgvForm.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromName("@control");
            this.flowLayoutPanel1.Controls.Add(this.btnExit);
            this.flowLayoutPanel1.Controls.Add(this.btnPrev);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 264);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(363, 36);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-cancel";
            this.btnExit.BackColor = System.Drawing.Color.FromName("@window");
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExit.Location = new System.Drawing.Point(280, 8);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 25);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "E&xit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.FromName("@window");
            this.btnPrev.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrev.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPrev.Location = new System.Drawing.Point(199, 8);
            this.btnPrev.MaximumSize = new System.Drawing.Size(0, 25);
            this.btnPrev.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 25);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.Text = "Pre&view";
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // PrintApplicants
            // 
            this.ClientSize = new System.Drawing.Size(363, 300);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintApplicants";
            this.Text = "Print Applicants";
            this.Load += new System.EventHandler(this.PrintApplicants_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvApp)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label lblName;
        private Label lblAppNo;
        private Label lblN;
        private Label lblApp;
        private Panel panel2;
        private DataGridViewEx gvApp;
        private DataGridViewTextBoxColumn AppDet;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnExit;
        private Button btnPrev;
        private DataGridViewTextBoxColumn dgvForm;
    }
}