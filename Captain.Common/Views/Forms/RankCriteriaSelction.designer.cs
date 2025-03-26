using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class RankCriteriaSelction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RankCriteriaSelction));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.lblField = new Wisej.Web.Label();
            this.txtField = new Wisej.Web.TextBox();
            this.gvResponse = new Wisej.Web.DataGridView();
            this.btnSave = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.button3 = new Wisej.Web.Button();
            this.button4 = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.lblCntInd = new Wisej.Web.Label();
            this.cmbCntInd = new Wisej.Web.ComboBox();
            this.lblAgeCalInd = new Wisej.Web.Label();
            this.cmbAgeCalInd = new Wisej.Web.ComboBox();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvResponse = new Wisej.Web.Panel();
            this.pnlSelectedField = new Wisej.Web.Panel();
            this.Response = new Wisej.Web.DataGridViewTextBoxColumn();
            this.FromDt = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Frm_Months = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ToDt = new Wisej.Web.DataGridViewTextBoxColumn();
            this.To_Months = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Res_code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Points = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Dup_Points = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Dup_From = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Dup_FrmMonths = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Dup_To = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Dup_ToMonths = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Row_Status = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Seq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.RelationCd = new Wisej.Web.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvResponse)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvResponse.SuspendLayout();
            this.pnlSelectedField.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblField
            // 
            this.lblField.Location = new System.Drawing.Point(15, 15);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(79, 14);
            this.lblField.TabIndex = 1;
            this.lblField.Text = "Selected Field";
            // 
            // txtField
            // 
            this.txtField.Enabled = false;
            this.txtField.Location = new System.Drawing.Point(132, 11);
            this.txtField.Name = "txtField";
            this.txtField.Size = new System.Drawing.Size(524, 25);
            this.txtField.TabIndex = 2;
            // 
            // gvResponse
            // 
            this.gvResponse.AllowUserToResizeColumns = false;
            this.gvResponse.BackColor = System.Drawing.Color.White;
            this.gvResponse.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvResponse.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvResponse.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvResponse.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Response,
            this.FromDt,
            this.Frm_Months,
            this.ToDt,
            this.To_Months,
            this.Res_code,
            this.Points,
            this.Dup_Points,
            this.Dup_From,
            this.Dup_FrmMonths,
            this.Dup_To,
            this.Dup_ToMonths,
            this.Row_Status,
            this.Seq,
            this.RelationCd});
            this.gvResponse.Dock = Wisej.Web.DockStyle.Fill;
            this.gvResponse.Location = new System.Drawing.Point(0, 0);
            this.gvResponse.MultiSelect = false;
            this.gvResponse.Name = "gvResponse";
            this.gvResponse.RowHeadersWidth = 25;
            this.gvResponse.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvResponse.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvResponse.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvResponse.Size = new System.Drawing.Size(685, 403);
            this.gvResponse.TabIndex = 3;
            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
            this.gvResponse.CellEndEdit += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellEndEdit);
            this.gvResponse.CellValidating += new Wisej.Web.DataGridViewCellValidatingEventHandler(this.gvResponse_CellValidating);
            this.gvResponse.CellValidated += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValidated);
            this.gvResponse.SelectionChanged += new System.EventHandler(this.gvResponse_SelectionChanged);
            this.gvResponse.RowsAdded += new Wisej.Web.DataGridViewRowsAddedEventHandler(this.gvResponse_RowsAdded);
            this.gvResponse.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellClick);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(517, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Location = new System.Drawing.Point(14, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(50, 14);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(433, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(67, 25);
            this.button3.TabIndex = 7;
            this.button3.Text = "Can&cel";
            this.button3.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(366, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(67, 25);
            this.button4.TabIndex = 6;
            this.button4.Text = "&Save";
            this.button4.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(595, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCntInd
            // 
            this.lblCntInd.Location = new System.Drawing.Point(15, 47);
            this.lblCntInd.Name = "lblCntInd";
            this.lblCntInd.Size = new System.Drawing.Size(106, 16);
            this.lblCntInd.TabIndex = 1;
            this.lblCntInd.Text = "Counting Indicator";
            // 
            // cmbCntInd
            // 
            this.cmbCntInd.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCntInd.FormattingEnabled = true;
            this.cmbCntInd.Location = new System.Drawing.Point(132, 43);
            this.cmbCntInd.Name = "cmbCntInd";
            this.cmbCntInd.Size = new System.Drawing.Size(193, 25);
            this.cmbCntInd.TabIndex = 5;
            this.cmbCntInd.SelectedIndexChanged += new System.EventHandler(this.cmbCntInd_SelectedIndexChanged);
            // 
            // lblAgeCalInd
            // 
            this.lblAgeCalInd.Location = new System.Drawing.Point(350, 47);
            this.lblAgeCalInd.Name = "lblAgeCalInd";
            this.lblAgeCalInd.Size = new System.Drawing.Size(147, 16);
            this.lblAgeCalInd.TabIndex = 1;
            this.lblAgeCalInd.Text = "Age Calculation Indicator";
            this.lblAgeCalInd.Visible = false;
            // 
            // cmbAgeCalInd
            // 
            this.cmbAgeCalInd.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAgeCalInd.FormattingEnabled = true;
            this.cmbAgeCalInd.Location = new System.Drawing.Point(503, 43);
            this.cmbAgeCalInd.Name = "cmbAgeCalInd";
            this.cmbAgeCalInd.Size = new System.Drawing.Size(153, 25);
            this.cmbAgeCalInd.TabIndex = 5;
            this.cmbAgeCalInd.Visible = false;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.panel1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 479);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(685, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(592, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvResponse);
            this.pnlCompleteForm.Controls.Add(this.pnlSelectedField);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(685, 514);
            this.pnlCompleteForm.TabIndex = 6;
            // 
            // pnlgvResponse
            // 
            this.pnlgvResponse.Controls.Add(this.gvResponse);
            this.pnlgvResponse.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvResponse.Location = new System.Drawing.Point(0, 76);
            this.pnlgvResponse.Name = "pnlgvResponse";
            this.pnlgvResponse.Size = new System.Drawing.Size(685, 403);
            this.pnlgvResponse.TabIndex = 7;
            // 
            // pnlSelectedField
            // 
            this.pnlSelectedField.Controls.Add(this.txtField);
            this.pnlSelectedField.Controls.Add(this.lblField);
            this.pnlSelectedField.Controls.Add(this.lblCntInd);
            this.pnlSelectedField.Controls.Add(this.cmbAgeCalInd);
            this.pnlSelectedField.Controls.Add(this.cmbCntInd);
            this.pnlSelectedField.Controls.Add(this.lblAgeCalInd);
            this.pnlSelectedField.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSelectedField.Location = new System.Drawing.Point(0, 0);
            this.pnlSelectedField.Name = "pnlSelectedField";
            this.pnlSelectedField.Size = new System.Drawing.Size(685, 76);
            this.pnlSelectedField.TabIndex = 7;
            // 
            // Response
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Response.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Response.HeaderStyle = dataGridViewCellStyle3;
            this.Response.HeaderText = "Response";
            this.Response.Name = "Response";
            this.Response.ReadOnly = true;
            this.Response.Width = 580;
            // 
            // FromDt
            // 
            this.FromDt.HeaderText = "From";
            this.FromDt.MaxInputLength = 10;
            this.FromDt.Name = "FromDt";
            this.FromDt.ShowInVisibilityMenu = false;
            this.FromDt.Visible = false;
            this.FromDt.Width = 50;
            // 
            // Frm_Months
            // 
            this.Frm_Months.HeaderText = "Months";
            this.Frm_Months.Name = "Frm_Months";
            this.Frm_Months.ShowInVisibilityMenu = false;
            this.Frm_Months.Visible = false;
            this.Frm_Months.Width = 40;
            // 
            // ToDt
            // 
            this.ToDt.HeaderText = "To";
            this.ToDt.MaxInputLength = 10;
            this.ToDt.Name = "ToDt";
            this.ToDt.ShowInVisibilityMenu = false;
            this.ToDt.Visible = false;
            this.ToDt.Width = 50;
            // 
            // To_Months
            // 
            this.To_Months.HeaderText = "Months";
            this.To_Months.Name = "To_Months";
            this.To_Months.ShowInVisibilityMenu = false;
            this.To_Months.Visible = false;
            this.To_Months.Width = 40;
            // 
            // Res_code
            // 
            this.Res_code.HeaderText = "Res_code";
            this.Res_code.Name = "Res_code";
            this.Res_code.ShowInVisibilityMenu = false;
            this.Res_code.Visible = false;
            this.Res_code.Width = 30;
            // 
            // Points
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Points.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Points.HeaderStyle = dataGridViewCellStyle5;
            this.Points.HeaderText = "Points";
            this.Points.MaxInputLength = 4;
            this.Points.Name = "Points";
            this.Points.Width = 60;
            // 
            // Dup_Points
            // 
            this.Dup_Points.HeaderText = "Dup_Points";
            this.Dup_Points.MaxInputLength = 4;
            this.Dup_Points.Name = "Dup_Points";
            this.Dup_Points.ShowInVisibilityMenu = false;
            this.Dup_Points.Visible = false;
            this.Dup_Points.Width = 40;
            // 
            // Dup_From
            // 
            this.Dup_From.HeaderText = "Dup_From";
            this.Dup_From.Name = "Dup_From";
            this.Dup_From.ShowInVisibilityMenu = false;
            this.Dup_From.Visible = false;
            this.Dup_From.Width = 30;
            // 
            // Dup_FrmMonths
            // 
            this.Dup_FrmMonths.HeaderText = "Dup_FrmMonths";
            this.Dup_FrmMonths.Name = "Dup_FrmMonths";
            this.Dup_FrmMonths.ShowInVisibilityMenu = false;
            this.Dup_FrmMonths.Visible = false;
            this.Dup_FrmMonths.Width = 30;
            // 
            // Dup_To
            // 
            this.Dup_To.HeaderText = "Dup_To";
            this.Dup_To.Name = "Dup_To";
            this.Dup_To.ShowInVisibilityMenu = false;
            this.Dup_To.Visible = false;
            this.Dup_To.Width = 30;
            // 
            // Dup_ToMonths
            // 
            this.Dup_ToMonths.HeaderText = "Dup_ToMonths";
            this.Dup_ToMonths.Name = "Dup_ToMonths";
            this.Dup_ToMonths.ShowInVisibilityMenu = false;
            this.Dup_ToMonths.Visible = false;
            this.Dup_ToMonths.Width = 30;
            // 
            // Row_Status
            // 
            this.Row_Status.HeaderText = "Row_Status";
            this.Row_Status.Name = "Row_Status";
            this.Row_Status.ShowInVisibilityMenu = false;
            this.Row_Status.Visible = false;
            this.Row_Status.Width = 30;
            // 
            // Seq
            // 
            this.Seq.HeaderText = "Seq";
            this.Seq.Name = "Seq";
            this.Seq.ShowInVisibilityMenu = false;
            this.Seq.Visible = false;
            this.Seq.Width = 40;
            // 
            // RelationCd
            // 
            this.RelationCd.HeaderText = "RelationCd";
            this.RelationCd.Name = "RelationCd";
            this.RelationCd.ShowInVisibilityMenu = false;
            this.RelationCd.Visible = false;
            this.RelationCd.Width = 40;
            // 
            // RankCriteriaSelction
            // 
            this.ClientSize = new System.Drawing.Size(685, 514);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RankCriteriaSelction";
            this.Text = "Rank Criteria Selction";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.FormClosing += new Wisej.Web.FormClosingEventHandler(this.RankCriteriaSelction_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gvResponse)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvResponse.ResumeLayout(false);
            this.pnlSelectedField.ResumeLayout(false);
            this.pnlSelectedField.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblField;
        private TextBox txtField;
        private DataGridView gvResponse;
        private DataGridViewTextBoxColumn Response;
        private DataGridViewTextBoxColumn Points;
        private DataGridViewTextBoxColumn Res_code;
        private Button btnSave;
        private Button btnCancel;
        private DataGridViewTextBoxColumn Dup_Points;
        private DataGridViewTextBoxColumn Row_Status;
        private DataGridViewTextBoxColumn Seq;
        private Label lblCntInd;
        private ComboBox cmbCntInd;
        private DataGridViewTextBoxColumn RelationCd;
        private DataGridViewTextBoxColumn FromDt;
        private DataGridViewTextBoxColumn ToDt;
        private Label lblAgeCalInd;
        private ComboBox cmbAgeCalInd;
        private DataGridViewTextBoxColumn Frm_Months;
        private DataGridViewTextBoxColumn To_Months;
        private DataGridViewTextBoxColumn Dup_From;
        private DataGridViewTextBoxColumn Dup_FrmMonths;
        private DataGridViewTextBoxColumn Dup_To;
        private DataGridViewTextBoxColumn Dup_ToMonths;
        private Panel panel1;
        private Button button3;
        private Button button4;
        private Panel pnlSave;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
        private Panel pnlgvResponse;
        private Panel pnlSelectedField;
    }
}