using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class CAPrices_Form
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CAPrices_Form));
            this.pnlgvandDate = new Wisej.Web.Panel();
            this.pnlDates = new Wisej.Web.Panel();
            this.label3 = new Wisej.Web.Label();
            this.lblParitalScoreDesc = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.dtpEndDate = new Wisej.Web.DateTimePicker();
            this.lblAddEdit = new Wisej.Web.Label();
            this.lblStartDate = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.dtAssessmentDate = new Wisej.Web.DateTimePicker();
            this.lblEndDate = new Wisej.Web.Label();
            this.lblEjobTitleReq = new Wisej.Web.Label();
            this.txtUnitPrice = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblTot = new Wisej.Web.Label();
            this.lblTotalScoreDesc = new Wisej.Web.Label();
            this.pnlgvwService = new Wisej.Web.Panel();
            this.gvwService = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.FDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.LDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Unit_Price = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CAP_ID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Delete = new Wisej.Web.DataGridViewImageColumn();
            this.pnlServicelbl = new Wisej.Web.Panel();
            this.lblService = new Wisej.Web.Label();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlgvandDate.SuspendLayout();
            this.pnlDates.SuspendLayout();
            this.pnlgvwService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwService)).BeginInit();
            this.pnlServicelbl.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlgvandDate
            // 
            this.pnlgvandDate.Controls.Add(this.pnlDates);
            this.pnlgvandDate.Controls.Add(this.pnlgvwService);
            this.pnlgvandDate.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvandDate.Location = new System.Drawing.Point(0, 37);
            this.pnlgvandDate.Name = "pnlgvandDate";
            this.pnlgvandDate.Size = new System.Drawing.Size(551, 379);
            this.pnlgvandDate.TabIndex = 3;
            this.pnlgvandDate.Click += new System.EventHandler(this.panel1_Click);
            // 
            // pnlDates
            // 
            this.pnlDates.Controls.Add(this.label3);
            this.pnlDates.Controls.Add(this.lblParitalScoreDesc);
            this.pnlDates.Controls.Add(this.label2);
            this.pnlDates.Controls.Add(this.label1);
            this.pnlDates.Controls.Add(this.label4);
            this.pnlDates.Controls.Add(this.dtpEndDate);
            this.pnlDates.Controls.Add(this.lblAddEdit);
            this.pnlDates.Controls.Add(this.lblStartDate);
            this.pnlDates.Controls.Add(this.label5);
            this.pnlDates.Controls.Add(this.dtAssessmentDate);
            this.pnlDates.Controls.Add(this.lblEndDate);
            this.pnlDates.Controls.Add(this.lblEjobTitleReq);
            this.pnlDates.Controls.Add(this.txtUnitPrice);
            this.pnlDates.Controls.Add(this.lblTot);
            this.pnlDates.Controls.Add(this.lblTotalScoreDesc);
            this.pnlDates.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDates.Location = new System.Drawing.Point(0, 202);
            this.pnlDates.Name = "pnlDates";
            this.pnlDates.Size = new System.Drawing.Size(551, 176);
            this.pnlDates.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(292, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(7, 10);
            this.label3.TabIndex = 0;
            this.label3.Text = "*";
            // 
            // lblParitalScoreDesc
            // 
            this.lblParitalScoreDesc.Location = new System.Drawing.Point(15, 108);
            this.lblParitalScoreDesc.Name = "lblParitalScoreDesc";
            this.lblParitalScoreDesc.Size = new System.Drawing.Size(456, 16);
            this.lblParitalScoreDesc.TabIndex = 1;
            this.lblParitalScoreDesc.Visible = false;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(504, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(6, 11);
            this.label2.TabIndex = 0;
            this.label2.Text = "*";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(72, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(7, 10);
            this.label1.TabIndex = 0;
            this.label1.Text = "*";
            this.label1.Visible = false;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(242, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(7, 11);
            this.label4.TabIndex = 0;
            this.label4.Text = "*";
            this.label4.Visible = false;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.AutoSize = false;
            this.dtpEndDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(312, 37);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.ShowToolTips = false;
            this.dtpEndDate.Size = new System.Drawing.Size(120, 25);
            this.dtpEndDate.TabIndex = 2;
            // 
            // lblAddEdit
            // 
            this.lblAddEdit.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAddEdit.Location = new System.Drawing.Point(15, 15);
            this.lblAddEdit.Name = "lblAddEdit";
            this.lblAddEdit.Size = new System.Drawing.Size(24, 14);
            this.lblAddEdit.TabIndex = 1;
            this.lblAddEdit.Text = "Add";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(15, 41);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(58, 14);
            this.lblStartDate.TabIndex = 4;
            this.lblStartDate.Text = "Start Date";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(71, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(6, 11);
            this.label5.TabIndex = 0;
            this.label5.Text = "*";
            // 
            // dtAssessmentDate
            // 
            this.dtAssessmentDate.AutoSize = false;
            this.dtAssessmentDate.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.dtAssessmentDate.Location = new System.Drawing.Point(92, 37);
            this.dtAssessmentDate.Name = "dtAssessmentDate";
            this.dtAssessmentDate.ShowCheckBox = true;
            this.dtAssessmentDate.ShowToolTips = false;
            this.dtAssessmentDate.Size = new System.Drawing.Size(120, 25);
            this.dtAssessmentDate.TabIndex = 1;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(241, 41);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(52, 14);
            this.lblEndDate.TabIndex = 4;
            this.lblEndDate.Text = "End Date";
            // 
            // lblEjobTitleReq
            // 
            this.lblEjobTitleReq.ForeColor = System.Drawing.Color.Red;
            this.lblEjobTitleReq.Location = new System.Drawing.Point(457, 37);
            this.lblEjobTitleReq.Name = "lblEjobTitleReq";
            this.lblEjobTitleReq.Size = new System.Drawing.Size(6, 10);
            this.lblEjobTitleReq.TabIndex = 0;
            this.lblEjobTitleReq.Text = "*";
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Location = new System.Drawing.Point(92, 69);
            this.txtUnitPrice.MaxLength = 8;
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(120, 25);
            this.txtUnitPrice.TabIndex = 3;
            this.txtUnitPrice.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // lblTot
            // 
            this.lblTot.Location = new System.Drawing.Point(15, 74);
            this.lblTot.Name = "lblTot";
            this.lblTot.Size = new System.Drawing.Size(56, 14);
            this.lblTot.TabIndex = 1;
            this.lblTot.Text = "Unit Price";
            // 
            // lblTotalScoreDesc
            // 
            this.lblTotalScoreDesc.Location = new System.Drawing.Point(15, 137);
            this.lblTotalScoreDesc.Name = "lblTotalScoreDesc";
            this.lblTotalScoreDesc.Size = new System.Drawing.Size(465, 16);
            this.lblTotalScoreDesc.TabIndex = 1;
            this.lblTotalScoreDesc.Visible = false;
            // 
            // pnlgvwService
            // 
            this.pnlgvwService.Controls.Add(this.gvwService);
            this.pnlgvwService.Dock = Wisej.Web.DockStyle.Top;
            this.pnlgvwService.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwService.Name = "pnlgvwService";
            this.pnlgvwService.Size = new System.Drawing.Size(551, 202);
            this.pnlgvwService.TabIndex = 7;
            // 
            // gvwService
            // 
            this.gvwService.AllowUserToAddRows = true;
            this.gvwService.AllowUserToResizeColumns = false;
            this.gvwService.AllowUserToResizeRows = false;
            this.gvwService.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwService.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwService.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwService.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.FDate,
            this.LDate,
            this.Unit_Price,
            this.CAP_ID,
            this.Delete});
            this.gvwService.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwService.Name = "gvwService";
            this.gvwService.RowHeadersWidth = 18;
            this.gvwService.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwService.Size = new System.Drawing.Size(551, 202);
            this.gvwService.TabIndex = 2;
            this.gvwService.SelectionChanged += new System.EventHandler(this.gvwAssessmentDetails_SelectionChanged);
            this.gvwService.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwAssessmentDetails_CellClick);
            // 
            // FDate
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FDate.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FDate.HeaderStyle = dataGridViewCellStyle3;
            this.FDate.HeaderText = "Start Date";
            this.FDate.Name = "FDate";
            this.FDate.ReadOnly = true;
            // 
            // LDate
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LDate.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LDate.HeaderStyle = dataGridViewCellStyle5;
            this.LDate.HeaderText = "End Date";
            this.LDate.Name = "LDate";
            this.LDate.ReadOnly = true;
            // 
            // Unit_Price
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Unit_Price.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Unit_Price.HeaderStyle = dataGridViewCellStyle7;
            this.Unit_Price.HeaderText = "Unit Price";
            this.Unit_Price.Name = "Unit_Price";
            this.Unit_Price.ReadOnly = true;
            // 
            // CAP_ID
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CAP_ID.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CAP_ID.HeaderStyle = dataGridViewCellStyle9;
            this.CAP_ID.HeaderText = "CAP_ID";
            this.CAP_ID.Name = "CAP_ID";
            this.CAP_ID.ShowInVisibilityMenu = false;
            this.CAP_ID.Visible = false;
            this.CAP_ID.Width = 20;
            // 
            // Delete
            // 
            this.Delete.CellImageSource = "captain-delete";
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.NullValue = null;
            this.Delete.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Delete.HeaderStyle = dataGridViewCellStyle11;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ShowInVisibilityMenu = false;
            this.Delete.Width = 50;
            // 
            // pnlServicelbl
            // 
            this.pnlServicelbl.Controls.Add(this.lblService);
            this.pnlServicelbl.Dock = Wisej.Web.DockStyle.Top;
            this.pnlServicelbl.Location = new System.Drawing.Point(0, 0);
            this.pnlServicelbl.Name = "pnlServicelbl";
            this.pnlServicelbl.Size = new System.Drawing.Size(551, 37);
            this.pnlServicelbl.TabIndex = 2;
            // 
            // lblService
            // 
            this.lblService.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblService.Location = new System.Drawing.Point(15, 15);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(49, 13);
            this.lblService.TabIndex = 1;
            this.lblService.Text = "Service:";
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(461, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(383, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvandDate);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Controls.Add(this.pnlServicelbl);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(551, 451);
            this.pnlCompleteForm.TabIndex = 6;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 416);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(551, 35);
            this.pnlSave.TabIndex = 7;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(458, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // CAPrices_Form
            // 
            this.ClientSize = new System.Drawing.Size(551, 451);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CAPrices_Form";
            this.Text = "Assessment Date";
            this.Load += new System.EventHandler(this.MAT00003AssessmentDate_Load);
            this.pnlgvandDate.ResumeLayout(false);
            this.pnlDates.ResumeLayout(false);
            this.pnlDates.PerformLayout();
            this.pnlgvwService.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwService)).EndInit();
            this.pnlServicelbl.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlgvandDate;
        private Panel pnlServicelbl;
        private Label lblStartDate;
        private DataGridViewEx gvwService;
        private DateTimePicker dtAssessmentDate;
        private Label lblService;
        private Button btnCancel;
        private Button btnSave;
        private DataGridViewImageColumn Delete;
        private Label lblEjobTitleReq;
        private Label label1;
        private DataGridViewTextBoxColumn Unit_Price;
        private Label lblTot;
        private TextBoxWithValidation txtUnitPrice;
        private Label lblParitalScoreDesc;
        private Label lblTotalScoreDesc;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;
        private Label label3;
        private Label label2;
        private Label label5;
        private Label label4;
        private Label lblAddEdit;
        private DataGridViewTextBoxColumn CAP_ID;
        private Panel pnlCompleteForm;
        private Panel pnlgvwService;
        private Panel pnlDates;
        private Panel pnlSave;
        private Spacer spacer1;
        private DataGridViewDateTimeColumn FDate;
        private DataGridViewDateTimeColumn LDate;
    }
}