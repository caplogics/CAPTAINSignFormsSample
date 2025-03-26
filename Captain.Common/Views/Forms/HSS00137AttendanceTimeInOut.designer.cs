using Captain.Common.Utilities;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00137AttendanceTimeInOut
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

        #region Visual WebGui Form Designer generated code

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00137AttendanceTimeInOut));
            this.gvwChildDetails = new Wisej.Web.DataGridView();
            this.gvtName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDate = new Wisej.Web.DataGridViewMaskedTextBoxColumn();
            this.gvtTime1Start = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtTime1End = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtTime1Sum = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtTime2Start = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtTime2End = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtTime2Sum = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtTimeTotal = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtFund = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtApplicant = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnSave = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.PbEdit = new Wisej.Web.PictureBox();
            this.btnCalculated = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlgvwChildDetails = new Wisej.Web.Panel();
            this.pnledit = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwChildDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbEdit)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlgvwChildDetails.SuspendLayout();
            this.pnledit.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwChildDetails
            // 
            this.gvwChildDetails.AllowUserToResizeColumns = false;
            this.gvwChildDetails.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwChildDetails.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwChildDetails.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwChildDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtName,
            this.gvtDate,
            this.gvtTime1Start,
            this.gvtTime1End,
            this.gvtTime1Sum,
            this.gvtTime2Start,
            this.gvtTime2End,
            this.gvtTime2Sum,
            this.gvtTimeTotal,
            this.gvtFund,
            this.gvtApplicant});
            this.gvwChildDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwChildDetails.Enabled = false;
            this.gvwChildDetails.Location = new System.Drawing.Point(0, 0);
            this.gvwChildDetails.Name = "gvwChildDetails";
            this.gvwChildDetails.RowHeadersWidth = 10;
            this.gvwChildDetails.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwChildDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwChildDetails.SelectionMode = Wisej.Web.DataGridViewSelectionMode.CellSelect;
            this.gvwChildDetails.Size = new System.Drawing.Size(885, 237);
            this.gvwChildDetails.TabIndex = 0;
            // 
            // gvtName
            // 
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtName.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtName.HeaderStyle = dataGridViewCellStyle2;
            this.gvtName.HeaderText = "Name";
            this.gvtName.Name = "gvtName";
            this.gvtName.ReadOnly = true;
            this.gvtName.Width = 250;
            // 
            // gvtDate
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtDate.HeaderStyle = dataGridViewCellStyle3;
            this.gvtDate.HeaderText = "Date";
            this.gvtDate.Mask = "00/00/0000";
            this.gvtDate.Name = "gvtDate";
            this.gvtDate.ReadOnly = true;
            this.gvtDate.TextMaskFormat = Wisej.Web.MaskFormat.IncludePrompt;
            // 
            // gvtTime1Start
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtTime1Start.HeaderStyle = dataGridViewCellStyle4;
            this.gvtTime1Start.HeaderText = "Start";
            this.gvtTime1Start.Name = "gvtTime1Start";
            this.gvtTime1Start.Width = 75;
            // 
            // gvtTime1End
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtTime1End.HeaderStyle = dataGridViewCellStyle5;
            this.gvtTime1End.HeaderText = "End";
            this.gvtTime1End.Name = "gvtTime1End";
            this.gvtTime1End.Width = 75;
            // 
            // gvtTime1Sum
            // 
            this.gvtTime1Sum.HeaderText = " ";
            this.gvtTime1Sum.Name = "gvtTime1Sum";
            this.gvtTime1Sum.ReadOnly = true;
            this.gvtTime1Sum.Width = 60;
            // 
            // gvtTime2Start
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtTime2Start.HeaderStyle = dataGridViewCellStyle6;
            this.gvtTime2Start.HeaderText = "Start";
            this.gvtTime2Start.Name = "gvtTime2Start";
            this.gvtTime2Start.Width = 75;
            // 
            // gvtTime2End
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtTime2End.HeaderStyle = dataGridViewCellStyle7;
            this.gvtTime2End.HeaderText = "End";
            this.gvtTime2End.Name = "gvtTime2End";
            this.gvtTime2End.Width = 75;
            // 
            // gvtTime2Sum
            // 
            this.gvtTime2Sum.HeaderText = "  ";
            this.gvtTime2Sum.Name = "gvtTime2Sum";
            this.gvtTime2Sum.ReadOnly = true;
            this.gvtTime2Sum.Width = 60;
            // 
            // gvtTimeTotal
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtTimeTotal.HeaderStyle = dataGridViewCellStyle8;
            this.gvtTimeTotal.HeaderText = "Total";
            this.gvtTimeTotal.Name = "gvtTimeTotal";
            this.gvtTimeTotal.ReadOnly = true;
            this.gvtTimeTotal.Width = 75;
            // 
            // gvtFund
            // 
            this.gvtFund.HeaderText = "gvtFund";
            this.gvtFund.Name = "gvtFund";
            this.gvtFund.ShowInVisibilityMenu = false;
            this.gvtFund.Visible = false;
            // 
            // gvtApplicant
            // 
            this.gvtApplicant.HeaderText = "gvtApplicant";
            this.gvtApplicant.Name = "gvtApplicant";
            this.gvtApplicant.ShowInVisibilityMenu = false;
            this.gvtApplicant.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(717, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(795, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PbEdit
            // 
            this.PbEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.PbEdit.Dock = Wisej.Web.DockStyle.Left;
            this.PbEdit.ImageSource = "captain-edit";
            this.PbEdit.Location = new System.Drawing.Point(15, 0);
            this.PbEdit.Name = "PbEdit";
            this.PbEdit.Size = new System.Drawing.Size(16, 25);
            this.PbEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.PbEdit.Visible = false;
            this.PbEdit.Click += new System.EventHandler(this.PbEdit_Click);
            // 
            // btnCalculated
            // 
            this.btnCalculated.Dock = Wisej.Web.DockStyle.Left;
            this.btnCalculated.Location = new System.Drawing.Point(15, 5);
            this.btnCalculated.Name = "btnCalculated";
            this.btnCalculated.Size = new System.Drawing.Size(75, 25);
            this.btnCalculated.TabIndex = 2;
            this.btnCalculated.Text = "C&alculate";
            this.btnCalculated.Visible = false;
            this.btnCalculated.Click += new System.EventHandler(this.btnCalculated_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwChildDetails);
            this.pnlCompleteForm.Controls.Add(this.pnledit);
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(885, 297);
            this.pnlCompleteForm.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnCalculated);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 262);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(885, 35);
            this.panel1.TabIndex = 0;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(792, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlgvwChildDetails
            // 
            this.pnlgvwChildDetails.Controls.Add(this.gvwChildDetails);
            this.pnlgvwChildDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwChildDetails.Location = new System.Drawing.Point(0, 25);
            this.pnlgvwChildDetails.Name = "pnlgvwChildDetails";
            this.pnlgvwChildDetails.Size = new System.Drawing.Size(885, 237);
            this.pnlgvwChildDetails.TabIndex = 1;
            // 
            // pnledit
            // 
            this.pnledit.Controls.Add(this.PbEdit);
            this.pnledit.Dock = Wisej.Web.DockStyle.Top;
            this.pnledit.Location = new System.Drawing.Point(0, 0);
            this.pnledit.Name = "pnledit";
            this.pnledit.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.pnledit.Size = new System.Drawing.Size(885, 25);
            this.pnledit.TabIndex = 2;
            // 
            // HSS00137AttendanceTimeInOut
            // 
            this.ClientSize = new System.Drawing.Size(885, 297);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00137AttendanceTimeInOut";
            this.Text = "Attendance Times ";
            ((System.ComponentModel.ISupportInitialize)(this.gvwChildDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbEdit)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlgvwChildDetails.ResumeLayout(false);
            this.pnledit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvwChildDetails;
        private DataGridViewTextBoxColumn gvtName;
        private DataGridViewMaskedTextBoxColumn gvtDate;
        private DataGridViewTextBoxColumn gvtTime1Sum;
        private DataGridViewTextBoxColumn gvtTime2Sum;
        private DataGridViewTextBoxColumn gvtTimeTotal;
        private Button btnSave;
        private Button btnCancel;
        private DataGridViewTextBoxColumn gvtFund;
        private DataGridViewTextBoxColumn gvtApplicant;
        private PictureBox PbEdit;
        private DataGridViewTextBoxColumn gvtTime1Start;
        private DataGridViewTextBoxColumn gvtTime1End;
        private DataGridViewTextBoxColumn gvtTime2Start;
        private DataGridViewTextBoxColumn gvtTime2End;
        private Button btnCalculated;
        private Panel pnlCompleteForm;
        private Panel panel1;
        private Spacer spacer1;
        private Panel pnlgvwChildDetails;
        private Panel pnledit;
    }
}