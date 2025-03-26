using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class CaseIncomeTotDetails
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseIncomeTotDetails));
            this.panel1 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.gvwIncomeDetails = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvtInterval = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAmount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel5 = new Wisej.Web.Panel();
            this.txtTotal = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblTotal = new Wisej.Web.Label();
            this.panel3 = new Wisej.Web.Panel();
            this.lblIntervalType = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.lbl3 = new Wisej.Web.Label();
            this.lbl2 = new Wisej.Web.Label();
            this.lbl1 = new Wisej.Web.Label();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnClose = new Wisej.Web.Button();
            this.gvtDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwIncomeDetails)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(516, 485);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gvwIncomeDetails);
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(516, 267);
            this.panel4.TabIndex = 14;
            // 
            // gvwIncomeDetails
            // 
            this.gvwIncomeDetails.AllowUserToResizeColumns = false;
            this.gvwIncomeDetails.AllowUserToResizeRows = false;
            this.gvwIncomeDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtInterval,
            this.gvtDate,
            this.gvtAmount});
            this.gvwIncomeDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwIncomeDetails.Name = "gvwIncomeDetails";
            this.gvwIncomeDetails.ReadOnly = true;
            this.gvwIncomeDetails.RowHeadersWidth = 14;
            this.gvwIncomeDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwIncomeDetails.Size = new System.Drawing.Size(516, 267);
            this.gvwIncomeDetails.TabIndex = 7;
            // 
            // gvtInterval
            // 
            this.gvtInterval.HeaderText = "Interval";
            this.gvtInterval.Name = "gvtInterval";
            this.gvtInterval.ReadOnly = true;
            this.gvtInterval.Width = 80;
            // 
            // gvtAmount
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvtAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvtAmount.HeaderText = "Amount";
            this.gvtAmount.Name = "gvtAmount";
            this.gvtAmount.ReadOnly = true;
            this.gvtAmount.Width = 85;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtTotal);
            this.panel5.Controls.Add(this.lblTotal);
            this.panel5.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 288);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(516, 36);
            this.panel5.TabIndex = 15;
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(65, 7);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.TabIndex = 9;
            this.txtTotal.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(11, 10);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(31, 14);
            this.lblTotal.TabIndex = 10;
            this.lblTotal.Text = "Total";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblIntervalType);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(516, 21);
            this.panel3.TabIndex = 13;
            // 
            // lblIntervalType
            // 
            this.lblIntervalType.AutoSize = true;
            this.lblIntervalType.Dock = Wisej.Web.DockStyle.Fill;
            this.lblIntervalType.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblIntervalType.Location = new System.Drawing.Point(0, 0);
            this.lblIntervalType.Name = "lblIntervalType";
            this.lblIntervalType.Size = new System.Drawing.Size(516, 21);
            this.lblIntervalType.TabIndex = 8;
            this.lblIntervalType.Text = "..";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbl3);
            this.panel2.Controls.Add(this.lbl2);
            this.panel2.Controls.Add(this.lbl1);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 324);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(516, 126);
            this.panel2.TabIndex = 11;
            this.panel2.TabStop = true;
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl3.Location = new System.Drawing.Point(11, 61);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(4, 13);
            this.lbl3.TabIndex = 2;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl2.Location = new System.Drawing.Point(11, 37);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(4, 13);
            this.lbl2.TabIndex = 1;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl1.Location = new System.Drawing.Point(11, 11);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(4, 13);
            this.lbl1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 450);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(5, 2, 15, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(516, 35);
            this.flowLayoutPanel1.TabIndex = 12;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-cancel";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(418, 5);
            this.btnClose.MaximumSize = new System.Drawing.Size(0, 25);
            this.btnClose.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gvtDate
            // 
            this.gvtDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvtDate.HeaderText = "Date";
            this.gvtDate.Name = "gvtDate";
            this.gvtDate.ReadOnly = true;
            this.gvtDate.Width = 80;
            // 
            // CaseIncomeTotDetails
            // 
            this.ClientSize = new System.Drawing.Size(516, 485);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaseIncomeTotDetails";
            this.Text = "Case Income To Details";
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwIncomeDetails)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label lblTotal;
        private TextBoxWithValidation txtTotal;
        private Label lblIntervalType;
        private DataGridViewEx gvwIncomeDetails;
        private DataGridViewTextBoxColumn gvtInterval;
        private DataGridViewTextBoxColumn gvtAmount;
        private Panel panel2;
        private Label lbl3;
        private Label lbl2;
        private Label lbl1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnClose;
        private Panel panel4;
        private Panel panel5;
        private Panel panel3;
        private DataGridViewDateTimeColumn gvtDate;
    }
}