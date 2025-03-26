using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class ReportControl
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

        #region Visual WebGui UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.btnGetReport = new Wisej.Web.Button();
            this.cmbRepUserId = new Wisej.Web.ComboBox();
            this.label6 = new Wisej.Web.Label();
            this.lblStart = new Wisej.Web.Label();
            this.dtStartDate = new Wisej.Web.DateTimePicker();
            this.lblChartType = new Wisej.Web.Label();
            this.cmbChartType = new Wisej.Web.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 537);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(818, 490);
            this.panel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnGetReport);
            this.panel2.Controls.Add(this.cmbRepUserId);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.lblStart);
            this.panel2.Controls.Add(this.dtStartDate);
            this.panel2.Controls.Add(this.lblChartType);
            this.panel2.Controls.Add(this.cmbChartType);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(818, 47);
            this.panel2.TabIndex = 0;
            // 
            // btnGetReport
            // 
            this.btnGetReport.Location = new System.Drawing.Point(728, 13);
            this.btnGetReport.Name = "btnGetReport";
            this.btnGetReport.Size = new System.Drawing.Size(75, 23);
            this.btnGetReport.TabIndex = 10;
            this.btnGetReport.Text = "Get Report";
            this.btnGetReport.Click += new System.EventHandler(this.btnGetReport_Click);
            // 
            // cmbRepUserId
            // 
            this.cmbRepUserId.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRepUserId.FormattingEnabled = true;
            this.cmbRepUserId.Location = new System.Drawing.Point(249, 13);
            this.cmbRepUserId.Name = "cmbRepUserId";
            this.cmbRepUserId.Size = new System.Drawing.Size(201, 25);
            this.cmbRepUserId.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(186, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "UserID";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(15, 14);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(30, 14);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "Date";
            // 
            // dtStartDate
            // 
            this.dtStartDate.Checked = false;
            this.dtStartDate.CustomFormat = "MM/dd/yyyy";
            this.dtStartDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtStartDate.Location = new System.Drawing.Point(57, 10);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.ShowCheckBox = true;
            this.dtStartDate.ShowToolTips = false;
            this.dtStartDate.Size = new System.Drawing.Size(115, 22);
            this.dtStartDate.TabIndex = 9;
            // 
            // lblChartType
            // 
            this.lblChartType.AutoSize = true;
            this.lblChartType.Location = new System.Drawing.Point(464, 18);
            this.lblChartType.Name = "lblChartType";
            this.lblChartType.Size = new System.Drawing.Size(64, 14);
            this.lblChartType.TabIndex = 1;
            this.lblChartType.Text = "Chart Type";
            // 
            // cmbChartType
            // 
            this.cmbChartType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbChartType.FormattingEnabled = true;
            this.cmbChartType.Location = new System.Drawing.Point(532, 14);
            this.cmbChartType.Name = "cmbChartType";
            this.cmbChartType.Size = new System.Drawing.Size(181, 25);
            this.cmbChartType.TabIndex = 0;
            this.cmbChartType.SelectedIndexChanged += new System.EventHandler(this.cmbChartType_SelectedIndexChanged);
            // 
            // ReportControl
            // 
            this.Controls.Add(this.panel1);
            this.Name = "ReportControl";
            this.Size = new System.Drawing.Size(818, 537);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private Gizmox.WebGUI.Reporting.ReportViewer reportViewer1;
        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private Label lblChartType;
        private ComboBox cmbChartType;
        private Label lblStart;
        private DateTimePicker dtStartDate;
        private ComboBox cmbRepUserId;
        private Label label6;
        private Button btnGetReport;



    }
}