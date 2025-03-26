using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class CASE0021Control
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

        #region Wisej UserControl Designer generated code

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
            this.pnlAlertcode = new Wisej.Web.Panel();
            this.gvwServices = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvtServicewkr = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtService = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAmount2 = new Wisej.Web.DataGridViewNumericUpDownColumn();
            this.gvtDate2 = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvtwkr2 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.panel8 = new Wisej.Web.Panel();
            this.txt5 = new Wisej.Web.TextBox();
            this.txt6 = new Wisej.Web.TextBox();
            this.txt7 = new Wisej.Web.TextBox();
            this.txt8 = new Wisej.Web.TextBox();
            this.txt4 = new Wisej.Web.TextBox();
            this.txt3 = new Wisej.Web.TextBox();
            this.txt2 = new Wisej.Web.TextBox();
            this.txt1 = new Wisej.Web.TextBox();
            this.panel3 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwServices)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAlertcode
            // 
            this.pnlAlertcode.Dock = Wisej.Web.DockStyle.Left;
            this.pnlAlertcode.Location = new System.Drawing.Point(0, 0);
            this.pnlAlertcode.Name = "pnlAlertcode";
            this.pnlAlertcode.Size = new System.Drawing.Size(251, 32);
            this.pnlAlertcode.TabIndex = 0;
            // 
            // gvwServices
            // 
            this.gvwServices.AllowUserToResizeColumns = false;
            this.gvwServices.AllowUserToResizeRows = false;
            this.gvwServices.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwServices.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwServices.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwServices.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtServicewkr,
            this.gvtService,
            this.gvtAmount2,
            this.gvtDate2,
            this.gvtwkr2});
            this.gvwServices.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwServices.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwServices.Location = new System.Drawing.Point(5, 5);
            this.gvwServices.MultiSelect = false;
            this.gvwServices.Name = "gvwServices";
            this.gvwServices.ReadOnly = true;
            this.gvwServices.RowHeadersWidth = 15;
            this.gvwServices.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwServices.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwServices.Size = new System.Drawing.Size(1472, 290);
            this.gvwServices.TabIndex = 0;
            this.gvwServices.SelectionChanged += new System.EventHandler(this.gvwServices_SelectionChanged);
            this.gvwServices.DoubleClick += new System.EventHandler(this.gvwServices_DoubleClick);
            // 
            // gvtServicewkr
            // 
            this.gvtServicewkr.HeaderText = "Worker";
            this.gvtServicewkr.Name = "gvtServicewkr";
            this.gvtServicewkr.ReadOnly = true;
            this.gvtServicewkr.Width = 80;
            // 
            // gvtService
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtService.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtService.HeaderStyle = dataGridViewCellStyle3;
            this.gvtService.HeaderText = "Service";
            this.gvtService.Name = "gvtService";
            this.gvtService.ReadOnly = true;
            this.gvtService.Width = 530;
            // 
            // gvtAmount2
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvtAmount2.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvtAmount2.HeaderStyle = dataGridViewCellStyle5;
            this.gvtAmount2.HeaderText = "Amount";
            this.gvtAmount2.HideUpDownButtons = true;
            this.gvtAmount2.Name = "gvtAmount2";
            this.gvtAmount2.ReadOnly = true;
            this.gvtAmount2.SortMode = Wisej.Web.DataGridViewColumnSortMode.Automatic;
            this.gvtAmount2.Width = 110;
            // 
            // gvtDate2
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvtDate2.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.gvtDate2.HeaderStyle = dataGridViewCellStyle7;
            this.gvtDate2.HeaderText = "Date";
            this.gvtDate2.Name = "gvtDate2";
            this.gvtDate2.ReadOnly = true;
            this.gvtDate2.Width = 90;
            // 
            // gvtwkr2
            // 
            this.gvtwkr2.HeaderText = "Worker";
            this.gvtwkr2.Name = "gvtwkr2";
            this.gvtwkr2.ReadOnly = true;
            this.gvtwkr2.ShowInVisibilityMenu = false;
            this.gvtwkr2.Visible = false;
            this.gvtwkr2.Width = 60;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1482, 709);
            this.panel1.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.txt5);
            this.panel8.Controls.Add(this.txt6);
            this.panel8.Controls.Add(this.txt7);
            this.panel8.Controls.Add(this.txt8);
            this.panel8.Controls.Add(this.txt4);
            this.panel8.Controls.Add(this.txt3);
            this.panel8.Controls.Add(this.txt2);
            this.panel8.Controls.Add(this.txt1);
            this.panel8.Dock = Wisej.Web.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 332);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1482, 84);
            this.panel8.TabIndex = 7;
            // 
            // txt5
            // 
            this.txt5.Enabled = false;
            this.txt5.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt5.Location = new System.Drawing.Point(12, 41);
            this.txt5.MaxLength = 14;
            this.txt5.Name = "txt5";
            this.txt5.Size = new System.Drawing.Size(297, 25);
            this.txt5.TabIndex = 10;
            // 
            // txt6
            // 
            this.txt6.Enabled = false;
            this.txt6.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt6.Location = new System.Drawing.Point(312, 41);
            this.txt6.MaxLength = 14;
            this.txt6.Name = "txt6";
            this.txt6.Size = new System.Drawing.Size(160, 25);
            this.txt6.TabIndex = 10;
            // 
            // txt7
            // 
            this.txt7.Enabled = false;
            this.txt7.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt7.Location = new System.Drawing.Point(476, 41);
            this.txt7.MaxLength = 14;
            this.txt7.Name = "txt7";
            this.txt7.Size = new System.Drawing.Size(160, 25);
            this.txt7.TabIndex = 10;
            this.txt7.Text = "Date :     ";
            // 
            // txt8
            // 
            this.txt8.Enabled = false;
            this.txt8.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt8.Location = new System.Drawing.Point(642, 41);
            this.txt8.MaxLength = 14;
            this.txt8.Name = "txt8";
            this.txt8.Size = new System.Drawing.Size(160, 25);
            this.txt8.TabIndex = 10;
            this.txt8.Text = "Balance     :        ";
            // 
            // txt4
            // 
            this.txt4.Enabled = false;
            this.txt4.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt4.Location = new System.Drawing.Point(642, 12);
            this.txt4.MaxLength = 14;
            this.txt4.Name = "txt4";
            this.txt4.Size = new System.Drawing.Size(160, 25);
            this.txt4.TabIndex = 10;
            this.txt4.Text = "Amount     :        ";
            // 
            // txt3
            // 
            this.txt3.Enabled = false;
            this.txt3.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt3.Location = new System.Drawing.Point(476, 12);
            this.txt3.MaxLength = 14;
            this.txt3.Name = "txt3";
            this.txt3.Size = new System.Drawing.Size(160, 25);
            this.txt3.TabIndex = 10;
            this.txt3.Text = "Fund:";
            // 
            // txt2
            // 
            this.txt2.Enabled = false;
            this.txt2.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt2.Location = new System.Drawing.Point(312, 12);
            this.txt2.MaxLength = 14;
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(160, 25);
            this.txt2.TabIndex = 10;
            this.txt2.Text = "Case Worker :  ";
            // 
            // txt1
            // 
            this.txt1.Enabled = false;
            this.txt1.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt1.Location = new System.Drawing.Point(12, 12);
            this.txt1.MaxLength = 14;
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(297, 25);
            this.txt1.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwServices);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5);
            this.panel3.Size = new System.Drawing.Size(1482, 300);
            this.panel3.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlAlertcode);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1482, 32);
            this.panel2.TabIndex = 8;
            // 
            // CASE0021Control
            // 
            this.Controls.Add(this.panel1);
            this.Name = "CASE0021Control";
            this.Size = new System.Drawing.Size(1482, 734);
            this.Controls.SetChildIndex(this.panel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gvwServices)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnlAlertcode;
        private DataGridViewEx gvwServices;
        private DataGridViewTextBoxColumn gvtServicewkr;
        private DataGridViewTextBoxColumn gvtService;
        private DataGridViewNumericUpDownColumn gvtAmount2;
        private DataGridViewTextBoxColumn gvtwkr2;
        private Panel panel1;
        private Panel panel8;
        private TextBox txt5;
        private TextBox txt6;
        private TextBox txt7;
        private TextBox txt8;
        private TextBox txt4;
        private TextBox txt3;
        private TextBox txt2;
        private TextBox txt1;
        private Panel panel3;
        private Panel panel2;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtDate2;
    }
}