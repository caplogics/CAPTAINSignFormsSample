using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class CASEVERHISTORY
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASEVERHISTORY));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.dataGridCaseIncomeVer = new DataGridViewEx();
            this.gvtPIncome = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtHouseHolds = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtFedOmb = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCMI = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSmi = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtHUD = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCaseIncomeVer)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridCaseIncomeVer
            // 
            this.dataGridCaseIncomeVer.AllowUserToResizeColumns = false;
            this.dataGridCaseIncomeVer.AllowUserToResizeRows = false;
            this.dataGridCaseIncomeVer.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dataGridCaseIncomeVer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridCaseIncomeVer.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCaseIncomeVer.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtDate,
            this.gvtPIncome,
            this.gvtHouseHolds,
            this.gvtFedOmb,
            this.gvtCMI,
            this.gvtSmi,
            this.gvtHUD});
            this.dataGridCaseIncomeVer.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridCaseIncomeVer.Location = new System.Drawing.Point(0, 0);
            this.dataGridCaseIncomeVer.MultiSelect = false;
            this.dataGridCaseIncomeVer.Name = "dataGridCaseIncomeVer";
            this.dataGridCaseIncomeVer.RowHeadersWidth = 15;
            this.dataGridCaseIncomeVer.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridCaseIncomeVer.Size = new System.Drawing.Size(737, 368);
            this.dataGridCaseIncomeVer.TabIndex = 1;
            // 
            // gvtPIncome
            // 
            this.gvtPIncome.HeaderText = "Program Income";
            this.gvtPIncome.Name = "gvtPIncome";
            this.gvtPIncome.ReadOnly = true;
            this.gvtPIncome.Width = 120;
            // 
            // gvtHouseHolds
            // 
            this.gvtHouseHolds.HeaderText = "# in Program";
            this.gvtHouseHolds.Name = "gvtHouseHolds";
            this.gvtHouseHolds.ReadOnly = true;
            this.gvtHouseHolds.Width = 85;
            // 
            // gvtFedOmb
            // 
            this.gvtFedOmb.HeaderText = "Fed OMB";
            this.gvtFedOmb.Name = "gvtFedOmb";
            this.gvtFedOmb.ReadOnly = true;
            this.gvtFedOmb.Width = 75;
            // 
            // gvtCMI
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtCMI.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtCMI.HeaderStyle = dataGridViewCellStyle4;
            this.gvtCMI.HeaderText = "CMI";
            this.gvtCMI.Name = "gvtCMI";
            this.gvtCMI.ReadOnly = true;
            this.gvtCMI.Width = 85;
            // 
            // gvtSmi
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtSmi.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtSmi.HeaderStyle = dataGridViewCellStyle6;
            this.gvtSmi.HeaderText = "SMI";
            this.gvtSmi.Name = "gvtSmi";
            this.gvtSmi.ReadOnly = true;
            this.gvtSmi.Width = 85;
            // 
            // gvtHUD
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtHUD.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtHUD.HeaderStyle = dataGridViewCellStyle8;
            this.gvtHUD.HeaderText = "HUD";
            this.gvtHUD.Name = "gvtHUD";
            this.gvtHUD.ReadOnly = true;
            this.gvtHUD.Width = 85;
            // 
            // gvtDate
            // 
            this.gvtDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvtDate.HeaderText = "Date";
            this.gvtDate.Name = "gvtDate";
            this.gvtDate.ReadOnly = true;
            // 
            // CASEVERHISTORY
            // 
            this.ClientSize = new System.Drawing.Size(737, 368);
            this.Controls.Add(this.dataGridCaseIncomeVer);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASEVERHISTORY";
            this.Text = "CASEVERHISTORY";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCaseIncomeVer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewEx dataGridCaseIncomeVer;
        private DataGridViewTextBoxColumn gvtPIncome;
        private DataGridViewTextBoxColumn gvtHouseHolds;
        private DataGridViewTextBoxColumn gvtFedOmb;
        private DataGridViewTextBoxColumn gvtSmi;
        private DataGridViewTextBoxColumn gvtCMI;
        private DataGridViewTextBoxColumn gvtHUD;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtDate;
    }
}