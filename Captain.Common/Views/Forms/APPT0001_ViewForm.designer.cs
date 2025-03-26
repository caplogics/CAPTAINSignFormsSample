using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class APPT0001_ViewForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle18 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle19 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle20 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APPT0001_ViewForm));
            this.gvScheduled = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Date = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.SchTime = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SchSlot = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnClose = new Wisej.Web.Button();
            this.gvDates = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Dates = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Day = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Times = new Wisej.Web.DataGridViewTextBoxColumn();
            this.strTime = new Wisej.Web.DataGridViewTextBoxColumn();
            this.chkbShowDates = new Wisej.Web.CheckBox();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvScheduled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDates)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvScheduled
            // 
            this.gvScheduled.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvScheduled.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvScheduled.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvScheduled.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvScheduled.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvScheduled.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Date,
            this.SchTime,
            this.SchSlot,
            this.Type,
            this.Name});
            this.gvScheduled.Dock = Wisej.Web.DockStyle.Fill;
            this.gvScheduled.Name = "gvScheduled";
            this.gvScheduled.RowHeadersVisible = false;
            this.gvScheduled.RowHeadersWidth = 14;
            this.gvScheduled.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvScheduled.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvScheduled.ShowColumnVisibilityMenu = false;
            this.gvScheduled.Size = new System.Drawing.Size(622, 223);
            this.gvScheduled.TabIndex = 6;
            // 
            // Date
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.CssStyle = "padding-left:5px;";
            dataGridViewCellStyle2.Padding = new Wisej.Web.Padding(20, 0, 0, 0);
            this.Date.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.CssStyle = "padding-left:5px;";
            dataGridViewCellStyle3.Padding = new Wisej.Web.Padding(20, 0, 0, 0);
            this.Date.HeaderStyle = dataGridViewCellStyle3;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // SchTime
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.SchTime.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.SchTime.HeaderStyle = dataGridViewCellStyle5;
            this.SchTime.HeaderText = "Time";
            this.SchTime.Name = "SchTime";
            this.SchTime.ReadOnly = true;
            this.SchTime.Width = 80;
            // 
            // SchSlot
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.SchSlot.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.SchSlot.HeaderStyle = dataGridViewCellStyle7;
            this.SchSlot.HeaderText = "Slot";
            this.SchSlot.Name = "SchSlot";
            this.SchSlot.ReadOnly = true;
            this.SchSlot.Width = 40;
            // 
            // Type
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Type.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Type.HeaderStyle = dataGridViewCellStyle9;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Name
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Name.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Name.HeaderStyle = dataGridViewCellStyle11;
            this.Name.HeaderText = "Name";
            this.Name.Name = "Name";
            this.Name.ReadOnly = true;
            this.Name.Width = 280;
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-cancel";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(532, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gvDates
            // 
            this.gvDates.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle12.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvDates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gvDates.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Dates,
            this.Day,
            this.Times,
            this.strTime});
            this.gvDates.Dock = Wisej.Web.DockStyle.Fill;
            this.gvDates.Name = "gvDates";
            this.gvDates.RowHeadersVisible = false;
            this.gvDates.RowHeadersWidth = 14;
            this.gvDates.RowTemplate.Height = 26;
            this.gvDates.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvDates.ShowColumnVisibilityMenu = false;
            this.gvDates.Size = new System.Drawing.Size(622, 215);
            this.gvDates.TabIndex = 8;
            this.gvDates.SelectionChanged += new System.EventHandler(this.gvDates_SelectionChanged);
            // 
            // Dates
            // 
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.CssStyle = "padding-left:5px;";
            dataGridViewCellStyle13.Padding = new Wisej.Web.Padding(20, 0, 0, 0);
            this.Dates.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.CssStyle = "padding-left:5px;";
            dataGridViewCellStyle14.Padding = new Wisej.Web.Padding(20, 0, 0, 0);
            this.Dates.HeaderStyle = dataGridViewCellStyle14;
            this.Dates.HeaderText = "Date";
            this.Dates.Name = "Dates";
            this.Dates.ReadOnly = true;
            // 
            // Day
            // 
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Day.DefaultCellStyle = dataGridViewCellStyle15;
            dataGridViewCellStyle16.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Day.HeaderStyle = dataGridViewCellStyle16;
            this.Day.HeaderText = "Day";
            this.Day.Name = "Day";
            this.Day.Width = 60;
            // 
            // Times
            // 
            dataGridViewCellStyle17.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Times.DefaultCellStyle = dataGridViewCellStyle17;
            dataGridViewCellStyle18.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Times.HeaderStyle = dataGridViewCellStyle18;
            this.Times.HeaderText = "Time";
            this.Times.Name = "Times";
            this.Times.ReadOnly = true;
            this.Times.Width = 80;
            // 
            // strTime
            // 
            dataGridViewCellStyle19.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.strTime.DefaultCellStyle = dataGridViewCellStyle19;
            dataGridViewCellStyle20.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.strTime.HeaderStyle = dataGridViewCellStyle20;
            this.strTime.HeaderText = "strTime";
            this.strTime.Name = "strTime";
            this.strTime.ShowInVisibilityMenu = false;
            this.strTime.Visible = false;
            // 
            // chkbShowDates
            // 
            this.chkbShowDates.Dock = Wisej.Web.DockStyle.Right;
            this.chkbShowDates.Location = new System.Drawing.Point(493, 0);
            this.chkbShowDates.MaximumSize = new System.Drawing.Size(0, 25);
            this.chkbShowDates.MinimumSize = new System.Drawing.Size(0, 25);
            this.chkbShowDates.Name = "chkbShowDates";
            this.chkbShowDates.Size = new System.Drawing.Size(114, 25);
            this.chkbShowDates.TabIndex = 9;
            this.chkbShowDates.Text = "Show All Dates";
            this.chkbShowDates.CheckedChanged += new System.EventHandler(this.chkbShowDates_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvDates);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(622, 215);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 464);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(622, 35);
            this.panel2.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvScheduled);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 241);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(622, 223);
            this.panel3.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chkbShowDates);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 215);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(0, 0, 15, 0);
            this.panel4.Size = new System.Drawing.Size(622, 26);
            this.panel4.TabIndex = 12;
            // 
            // APPT0001_ViewForm
            // 
            this.ClientSize = new System.Drawing.Size(622, 499);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.Name = "APPT0001_ViewForm";
            this.Text = "APPT0001 ViewForm";
            this.Load += new System.EventHandler(this.APPT0001_ViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvScheduled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDates)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private DataGridViewEx gvScheduled;
        private DataGridViewTextBoxColumn SchTime;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn SchSlot;
        private Button btnClose;
        private DataGridViewEx gvDates;
        private DataGridViewTextBoxColumn Times;
        private DataGridViewTextBoxColumn strTime;
        private DataGridViewTextBoxColumn Day;
        private CheckBox chkbShowDates;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Controls.Compatibility.DataGridViewDateTimeColumn Date;
        private Controls.Compatibility.DataGridViewDateTimeColumn Dates;
    }
}