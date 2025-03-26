using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00137AttendancePostingSite
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00137AttendancePostingSite));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.gvwAttendance = new DataGridViewEx();
            this.gvtAppNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtContract = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtPrs = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAbs = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtExc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtFirstName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtLastName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtMiddleName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlMonth = new Wisej.Web.Panel();
            this.label2 = new Wisej.Web.Label();
            this.cmbMonth = new Wisej.Web.ComboBox();
            this.lblMonthDis = new Wisej.Web.Label();
            this.gvtEnrolled = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvtWithdraw = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.pnlCompleteForm.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAttendance)).BeginInit();
            this.pnlMonth.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Controls.Add(this.pnlMonth);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(817, 297);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvwAttendance);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 253);
            this.panel1.TabIndex = 30;
            // 
            // gvwAttendance
            // 
            this.gvwAttendance.AllowUserToResizeColumns = false;
            this.gvwAttendance.AllowUserToResizeRows = false;
            this.gvwAttendance.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwAttendance.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwAttendance.BorderStyle = Wisej.Web.BorderStyle.None;
            this.gvwAttendance.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwAttendance.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtAppNo,
            this.gvtName,
            this.gvtContract,
            this.gvtEnrolled,
            this.gvtWithdraw,
            this.gvtPrs,
            this.gvtAbs,
            this.gvtExc,
            this.gvtFirstName,
            this.gvtLastName,
            this.gvtMiddleName});
            this.gvwAttendance.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwAttendance.Location = new System.Drawing.Point(0, 0);
            this.gvwAttendance.Name = "gvwAttendance";
            this.gvwAttendance.ReadOnly = true;
            this.gvwAttendance.RowHeadersWidth = 14;
            this.gvwAttendance.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwAttendance.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwAttendance.Size = new System.Drawing.Size(817, 253);
            this.gvwAttendance.TabIndex = 0;
            // 
            // gvtAppNo
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtAppNo.HeaderStyle = dataGridViewCellStyle1;
            this.gvtAppNo.HeaderText = "App No#";
            this.gvtAppNo.Name = "gvtAppNo";
            this.gvtAppNo.Width = 100;
            // 
            // gvtName
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtName.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtName.HeaderStyle = dataGridViewCellStyle3;
            this.gvtName.HeaderText = "Child Name";
            this.gvtName.Name = "gvtName";
            this.gvtName.Width = 200;
            // 
            // gvtContract
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtContract.HeaderStyle = dataGridViewCellStyle4;
            this.gvtContract.HeaderText = "Contract";
            this.gvtContract.Name = "gvtContract";
            this.gvtContract.ReadOnly = true;
            this.gvtContract.Width = 90;
            // 
            // gvtPrs
            // 
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvtPrs.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtPrs.HeaderStyle = dataGridViewCellStyle10;
            this.gvtPrs.HeaderText = "Prs";
            this.gvtPrs.Name = "gvtPrs";
            this.gvtPrs.ReadOnly = true;
            this.gvtPrs.Width = 60;
            // 
            // gvtAbs
            // 
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvtAbs.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtAbs.HeaderStyle = dataGridViewCellStyle12;
            this.gvtAbs.HeaderText = "Abs";
            this.gvtAbs.Name = "gvtAbs";
            this.gvtAbs.ReadOnly = true;
            this.gvtAbs.Width = 60;
            // 
            // gvtExc
            // 
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvtExc.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvtExc.HeaderStyle = dataGridViewCellStyle14;
            this.gvtExc.HeaderText = "Exc";
            this.gvtExc.Name = "gvtExc";
            this.gvtExc.ReadOnly = true;
            this.gvtExc.Width = 60;
            // 
            // gvtFirstName
            // 
            this.gvtFirstName.HeaderText = "gvtFirstName";
            this.gvtFirstName.Name = "gvtFirstName";
            this.gvtFirstName.ReadOnly = true;
            this.gvtFirstName.ShowInVisibilityMenu = false;
            this.gvtFirstName.Visible = false;
            // 
            // gvtLastName
            // 
            this.gvtLastName.HeaderText = "gvtLastName";
            this.gvtLastName.Name = "gvtLastName";
            this.gvtLastName.ReadOnly = true;
            this.gvtLastName.ShowInVisibilityMenu = false;
            this.gvtLastName.Visible = false;
            // 
            // gvtMiddleName
            // 
            this.gvtMiddleName.HeaderText = "gvtMiddleName";
            this.gvtMiddleName.Name = "gvtMiddleName";
            this.gvtMiddleName.ReadOnly = true;
            this.gvtMiddleName.ShowInVisibilityMenu = false;
            this.gvtMiddleName.Visible = false;
            // 
            // pnlMonth
            // 
            this.pnlMonth.Controls.Add(this.label2);
            this.pnlMonth.Controls.Add(this.cmbMonth);
            this.pnlMonth.Controls.Add(this.lblMonthDis);
            this.pnlMonth.Dock = Wisej.Web.DockStyle.Top;
            this.pnlMonth.Location = new System.Drawing.Point(0, 0);
            this.pnlMonth.Name = "pnlMonth";
            this.pnlMonth.Size = new System.Drawing.Size(817, 44);
            this.pnlMonth.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(52, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 10);
            this.label2.TabIndex = 28;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Visible = false;
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(73, 11);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(121, 25);
            this.cmbMonth.TabIndex = 1;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // lblMonthDis
            // 
            this.lblMonthDis.AutoSize = true;
            this.lblMonthDis.Location = new System.Drawing.Point(15, 15);
            this.lblMonthDis.Name = "lblMonthDis";
            this.lblMonthDis.Size = new System.Drawing.Size(38, 14);
            this.lblMonthDis.TabIndex = 0;
            this.lblMonthDis.Text = "Month";
            // 
            // gvtEnrolled
            // 
            this.gvtEnrolled.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtEnrolled.HeaderStyle = dataGridViewCellStyle6;
            this.gvtEnrolled.HeaderText = "Enrolled";
            this.gvtEnrolled.Name = "gvtEnrolled";
            this.gvtEnrolled.ReadOnly = true;
            this.gvtEnrolled.Width = 90;
            // 
            // gvtWithdraw
            // 
            this.gvtWithdraw.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtWithdraw.HeaderStyle = dataGridViewCellStyle8;
            this.gvtWithdraw.HeaderText = "Withdrawn";
            this.gvtWithdraw.Name = "gvtWithdraw";
            this.gvtWithdraw.ReadOnly = true;
            this.gvtWithdraw.Width = 90;
            // 
            // HSS00137AttendancePostingSite
            // 
            this.ClientSize = new System.Drawing.Size(817, 297);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00137AttendancePostingSite";
            this.Text = "HSS00137AttendancePostingSite";
            this.pnlCompleteForm.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwAttendance)).EndInit();
            this.pnlMonth.ResumeLayout(false);
            this.pnlMonth.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlCompleteForm;
        private DataGridViewEx gvwAttendance;
        private DataGridViewTextBoxColumn gvtAppNo;
        private DataGridViewTextBoxColumn gvtName;
        private DataGridViewTextBoxColumn gvtContract;
        private DataGridViewTextBoxColumn gvtPrs;
        private DataGridViewTextBoxColumn gvtAbs;
        private DataGridViewTextBoxColumn gvtExc;
        private Label lblMonthDis;
        private Label label2;
        private ComboBox cmbMonth;
        private DataGridViewTextBoxColumn gvtFirstName;
        private DataGridViewTextBoxColumn gvtLastName;
        private DataGridViewTextBoxColumn gvtMiddleName;
        private Panel panel1;
        private Panel pnlMonth;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtEnrolled;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtWithdraw;
    }
}