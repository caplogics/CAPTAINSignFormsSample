using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class HsAgeControlForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HsAgeControlForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.gvwSchoolDetails = new Wisej.Web.DataGridView();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcMonth = new Wisej.Web.DataGridViewComboBoxColumn();
            this.gvcDay = new Wisej.Web.DataGridViewComboBoxColumn();
            this.gvcYear = new Wisej.Web.DataGridViewComboBoxColumn();
            this.gvtCode1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwSchoolDetails)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwSchoolDetails
            // 
            this.gvwSchoolDetails.BackColor = System.Drawing.Color.White;
            this.gvwSchoolDetails.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwSchoolDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwSchoolDetails.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvwSchoolDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCode,
            this.gvtDesc,
            this.gvcMonth,
            this.gvcDay,
            this.gvcYear,
            this.gvtCode1});
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle7.WrapMode = Wisej.Web.DataGridViewTriState.False;
            this.gvwSchoolDetails.DefaultCellStyle = dataGridViewCellStyle7;
            this.gvwSchoolDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwSchoolDetails.Location = new System.Drawing.Point(0, 0);
            this.gvwSchoolDetails.MultiSelect = false;
            this.gvwSchoolDetails.Name = "gvwSchoolDetails";
            this.gvwSchoolDetails.RowHeadersWidth = 14;
            this.gvwSchoolDetails.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwSchoolDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwSchoolDetails.Size = new System.Drawing.Size(537, 220);
            this.gvwSchoolDetails.TabIndex = 1;
            // 
            // gvtCode
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtCode.HeaderStyle = dataGridViewCellStyle2;
            this.gvtCode.HeaderText = "Code";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.Width = 70;
            // 
            // gvtDesc
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtDesc.HeaderStyle = dataGridViewCellStyle3;
            this.gvtDesc.HeaderText = "Desc";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.gvtDesc.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvtDesc.Width = 170;
            // 
            // gvcMonth
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvcMonth.HeaderStyle = dataGridViewCellStyle4;
            this.gvcMonth.HeaderText = "Month";
            this.gvcMonth.Name = "gvcMonth";
            this.gvcMonth.Width = 80;
            // 
            // gvcDay
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvcDay.HeaderStyle = dataGridViewCellStyle5;
            this.gvcDay.HeaderText = "Day";
            this.gvcDay.Name = "gvcDay";
            this.gvcDay.Width = 60;
            // 
            // gvcYear
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvcYear.HeaderStyle = dataGridViewCellStyle6;
            this.gvcYear.HeaderText = "Year";
            this.gvcYear.Name = "gvcYear";
            this.gvcYear.Width = 110;
            // 
            // gvtCode1
            // 
            this.gvtCode1.HeaderText = " ";
            this.gvtCode1.Name = "gvtCode1";
            this.gvtCode1.ShowInVisibilityMenu = false;
            this.gvtCode1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 220);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(537, 35);
            this.panel1.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(369, 5);
            this.btnOk.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "&Save";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(444, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(447, 5);
            this.btnCancel.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwSchoolDetails);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(537, 220);
            this.panel2.TabIndex = 3;
            // 
            // HsAgeControlForm
            // 
            this.ClientSize = new System.Drawing.Size(537, 255);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HsAgeControlForm";
            this.Text = "HsAgeControlForm";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            ((System.ComponentModel.ISupportInitialize)(this.gvwSchoolDetails)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvwSchoolDetails;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewComboBoxColumn gvcMonth;
        private DataGridViewComboBoxColumn gvcDay;
        private DataGridViewTextBoxColumn gvtCode;
        private DataGridViewTextBoxColumn gvtCode1;
        private Panel panel1;
        private Panel panel2;
        private Button btnCancel;
        private Button btnOk;
        private DataGridViewComboBoxColumn gvcYear;
        private Spacer spacer1;
    }
}