using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ADMN4016PrintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADMN4016PrintForm));
            this.rblCode = new Wisej.Web.RadioButton();
            this.lblSortBy = new Wisej.Web.Label();
            this.rblDesc = new Wisej.Web.RadioButton();
            this.pnlParams = new Wisej.Web.Panel();
            this.chkSel = new Wisej.Web.CheckBox();
            this.dtpFrmDate = new Wisej.Web.DateTimePicker();
            this.pnlPrint = new Wisej.Web.Panel();
            this.btnExcel = new Wisej.Web.Button();
            this.pnlParams.SuspendLayout();
            this.pnlPrint.SuspendLayout();
            this.SuspendLayout();
            // 
            // rblCode
            // 
            this.rblCode.Checked = true;
            this.rblCode.Location = new System.Drawing.Point(186, 17);
            this.rblCode.Name = "rblCode";
            this.rblCode.Size = new System.Drawing.Size(61, 21);
            this.rblCode.TabIndex = 1;
            this.rblCode.TabStop = true;
            this.rblCode.Text = "Code";
            this.rblCode.CheckedChanged += new System.EventHandler(this.rblCode_CheckedChanged);
            // 
            // lblSortBy
            // 
            this.lblSortBy.AutoSize = true;
            this.lblSortBy.Location = new System.Drawing.Point(119, 21);
            this.lblSortBy.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblSortBy.Name = "lblSortBy";
            this.lblSortBy.Size = new System.Drawing.Size(44, 16);
            this.lblSortBy.TabIndex = 2;
            this.lblSortBy.Text = "Sort By";
            // 
            // rblDesc
            // 
            this.rblDesc.Location = new System.Drawing.Point(253, 17);
            this.rblDesc.Name = "rblDesc";
            this.rblDesc.Size = new System.Drawing.Size(93, 21);
            this.rblDesc.TabIndex = 1;
            this.rblDesc.Text = "Description";
            this.rblDesc.CheckedChanged += new System.EventHandler(this.rblCode_CheckedChanged);
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.dtpFrmDate);
            this.pnlParams.Controls.Add(this.rblCode);
            this.pnlParams.Controls.Add(this.chkSel);
            this.pnlParams.Controls.Add(this.rblDesc);
            this.pnlParams.Controls.Add(this.lblSortBy);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(450, 54);
            this.pnlParams.TabIndex = 4;
            // 
            // chkSel
            // 
            this.chkSel.Location = new System.Drawing.Point(115, 56);
            this.chkSel.Name = "chkSel";
            this.chkSel.Size = new System.Drawing.Size(177, 21);
            this.chkSel.TabIndex = 5;
            this.chkSel.Text = "Report Outcome Date After";
            this.chkSel.Visible = false;
            this.chkSel.CheckedChanged += new System.EventHandler(this.chkSel_CheckedChanged);
            // 
            // dtpFrmDate
            // 
            this.dtpFrmDate.Checked = false;
            this.dtpFrmDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpFrmDate.Location = new System.Drawing.Point(245, 54);
            this.dtpFrmDate.Name = "dtpFrmDate";
            this.dtpFrmDate.ShowCheckBox = true;
            this.dtpFrmDate.Size = new System.Drawing.Size(103, 22);
            this.dtpFrmDate.TabIndex = 3;
            this.dtpFrmDate.Visible = false;
            this.dtpFrmDate.Leave += new System.EventHandler(this.dtpFrmDate_Leave);
            // 
            // pnlPrint
            // 
            this.pnlPrint.AppearanceKey = "panel-grdo";
            this.pnlPrint.Controls.Add(this.btnExcel);
            this.pnlPrint.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlPrint.Location = new System.Drawing.Point(0, 54);
            this.pnlPrint.Name = "pnlPrint";
            this.pnlPrint.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlPrint.Size = new System.Drawing.Size(450, 35);
            this.pnlPrint.TabIndex = 6;
            // 
            // btnExcel
            // 
            this.btnExcel.AppearanceKey = "button-reports";
            this.btnExcel.Dock = Wisej.Web.DockStyle.Right;
            this.btnExcel.Location = new System.Drawing.Point(360, 5);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 25);
            this.btnExcel.TabIndex = 0;
            this.btnExcel.Text = "&Generate";
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // ADMN4016PrintForm
            // 
            this.ClientSize = new System.Drawing.Size(450, 89);
            this.Controls.Add(this.pnlParams);
            this.Controls.Add(this.pnlPrint);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADMN4016PrintForm";
            this.Text = "Services or Outcomes";
            this.Resize += new System.EventHandler(this.ADMN4016PrintForm_Resize);
            this.pnlParams.ResumeLayout(false);
            this.pnlParams.PerformLayout();
            this.pnlPrint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private RadioButton rblCode;
        private Label lblSortBy;
        private RadioButton rblDesc;
       // private Gizmox.WebGUI.Reporting.ReportViewer rv;
        private Panel pnlParams;
        private CheckBox chkSel;
        private DateTimePicker dtpFrmDate;
        private Panel pnlPrint;
        private Button btnExcel;
    }
}