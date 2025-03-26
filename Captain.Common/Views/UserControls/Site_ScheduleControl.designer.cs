using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class Site_ScheduleControl
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

        #region Wisej Web UserControl Designer generated code

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
            this.panel1 = new Wisej.Web.Panel();
            this.panel6 = new Wisej.Web.Panel();
            this.gvsiteSchedule = new Wisej.Web.DataGridView();
            this.Month = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Sch_Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Site = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Site_Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Room = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Am_Pm = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Funding_Source = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Month_No = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Fund = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Calander_Year = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Add_Date = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Add_Operator = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Lstc_Date = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Lstc_Operator = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Attm_ID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cmbYear = new Wisej.Web.ComboBox();
            this.lblYear = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.lblSort = new Wisej.Web.Label();
            this.cmbSort = new Wisej.Web.ComboBox();
            this.gvHie = new Wisej.Web.DataGridView();
            this.Hierarchy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Site_Hierarchy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Hier_Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel3 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.panel5 = new Wisej.Web.Panel();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvsiteSchedule)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvHie)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(112, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(1043, 505);
            this.panel1.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.gvsiteSchedule);
            this.panel6.CssStyle = "border-radius:8px; border:1px solid #cccccc;";
            this.panel6.Dock = Wisej.Web.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(5, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1038, 505);
            this.panel6.TabIndex = 1;
            // 
            // gvsiteSchedule
            // 
            this.gvsiteSchedule.AllowUserToResizeColumns = false;
            this.gvsiteSchedule.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvsiteSchedule.BorderStyle = Wisej.Web.BorderStyle.Dotted;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvsiteSchedule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvsiteSchedule.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvsiteSchedule.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Month,
            this.Sch_Type,
            this.Site,
            this.Site_Name,
            this.Room,
            this.Am_Pm,
            this.Funding_Source,
            this.Month_No,
            this.Fund,
            this.Calander_Year,
            this.Add_Date,
            this.Add_Operator,
            this.Lstc_Date,
            this.Lstc_Operator,
            this.Attm_ID});
            this.gvsiteSchedule.Dock = Wisej.Web.DockStyle.Fill;
            this.gvsiteSchedule.Location = new System.Drawing.Point(0, 0);
            this.gvsiteSchedule.Name = "gvsiteSchedule";
            this.gvsiteSchedule.RowHeadersWidth = 25;
            this.gvsiteSchedule.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvsiteSchedule.Size = new System.Drawing.Size(1038, 505);
            this.gvsiteSchedule.TabIndex = 0;
            this.gvsiteSchedule.DoubleClick += new System.EventHandler(this.gvsiteSchedule_DoubleClick);
            // 
            // Month
            // 
            this.Month.HeaderText = "Month";
            this.Month.Name = "Month";
            this.Month.ReadOnly = true;
            this.Month.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Month.Width = 100;
            // 
            // Sch_Type
            // 
            this.Sch_Type.HeaderText = "Sch Type";
            this.Sch_Type.Name = "Sch_Type";
            this.Sch_Type.ReadOnly = true;
            this.Sch_Type.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Sch_Type.Width = 100;
            // 
            // Site
            // 
            this.Site.HeaderText = "Site";
            this.Site.Name = "Site";
            this.Site.ReadOnly = true;
            this.Site.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Site.Width = 80;
            // 
            // Site_Name
            // 
            this.Site_Name.HeaderText = "Site Name";
            this.Site_Name.Name = "Site_Name";
            this.Site_Name.ReadOnly = true;
            this.Site_Name.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Site_Name.Width = 220;
            // 
            // Room
            // 
            this.Room.HeaderText = "Room";
            this.Room.Name = "Room";
            this.Room.ReadOnly = true;
            this.Room.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Room.Width = 70;
            // 
            // Am_Pm
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.Am_Pm.DefaultCellStyle = dataGridViewCellStyle2;
            this.Am_Pm.HeaderText = "AM/PM";
            this.Am_Pm.Name = "Am_Pm";
            this.Am_Pm.ReadOnly = true;
            this.Am_Pm.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Am_Pm.Width = 70;
            // 
            // Funding_Source
            // 
            this.Funding_Source.HeaderText = "Funding Source";
            this.Funding_Source.Name = "Funding_Source";
            this.Funding_Source.ReadOnly = true;
            this.Funding_Source.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Funding_Source.Width = 170;
            // 
            // Month_No
            // 
            this.Month_No.HeaderText = "Month_No";
            this.Month_No.Name = "Month_No";
            this.Month_No.ShowInVisibilityMenu = false;
            this.Month_No.Visible = false;
            this.Month_No.Width = 20;
            // 
            // Fund
            // 
            this.Fund.HeaderText = "Fund";
            this.Fund.Name = "Fund";
            this.Fund.ShowInVisibilityMenu = false;
            this.Fund.Visible = false;
            this.Fund.Width = 30;
            // 
            // Calander_Year
            // 
            this.Calander_Year.HeaderText = "Calander_Year";
            this.Calander_Year.Name = "Calander_Year";
            this.Calander_Year.ShowInVisibilityMenu = false;
            this.Calander_Year.Visible = false;
            this.Calander_Year.Width = 25;
            // 
            // Add_Date
            // 
            this.Add_Date.HeaderText = "Add_Date";
            this.Add_Date.Name = "Add_Date";
            this.Add_Date.ShowInVisibilityMenu = false;
            this.Add_Date.Visible = false;
            this.Add_Date.Width = 30;
            // 
            // Add_Operator
            // 
            this.Add_Operator.HeaderText = "Add_Operator";
            this.Add_Operator.Name = "Add_Operator";
            this.Add_Operator.ShowInVisibilityMenu = false;
            this.Add_Operator.Visible = false;
            this.Add_Operator.Width = 30;
            // 
            // Lstc_Date
            // 
            this.Lstc_Date.HeaderText = "Lstc_Date";
            this.Lstc_Date.Name = "Lstc_Date";
            this.Lstc_Date.ShowInVisibilityMenu = false;
            this.Lstc_Date.Visible = false;
            this.Lstc_Date.Width = 30;
            // 
            // Lstc_Operator
            // 
            this.Lstc_Operator.HeaderText = "Lstc_Operator";
            this.Lstc_Operator.Name = "Lstc_Operator";
            this.Lstc_Operator.ShowInVisibilityMenu = false;
            this.Lstc_Operator.Visible = false;
            this.Lstc_Operator.Width = 30;
            // 
            // Attm_ID
            // 
            this.Attm_ID.HeaderText = "Attm_ID";
            this.Attm_ID.Name = "Attm_ID";
            this.Attm_ID.ShowInVisibilityMenu = false;
            this.Attm_ID.Visible = false;
            this.Attm_ID.Width = 10;
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(339, 6);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(66, 25);
            this.cmbYear.TabIndex = 1;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(307, 9);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(30, 14);
            this.lblYear.TabIndex = 0;
            this.lblYear.Text = "Year";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblSort);
            this.panel2.Controls.Add(this.cmbSort);
            this.panel2.Controls.Add(this.cmbYear);
            this.panel2.Controls.Add(this.lblYear);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1155, 37);
            this.panel2.TabIndex = 0;
            // 
            // lblSort
            // 
            this.lblSort.Location = new System.Drawing.Point(8, 9);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(47, 19);
            this.lblSort.TabIndex = 0;
            this.lblSort.Text = "Sort by";
            // 
            // cmbSort
            // 
            this.cmbSort.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Location = new System.Drawing.Point(58, 6);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(217, 25);
            this.cmbSort.TabIndex = 1;
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbSort_SelectedIndexChanged);
            // 
            // gvHie
            // 
            this.gvHie.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvHie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvHie.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvHie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Hierarchy,
            this.Site_Hierarchy,
            this.Hier_Desc});
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvHie.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvHie.Dock = Wisej.Web.DockStyle.Fill;
            this.gvHie.Location = new System.Drawing.Point(0, 0);
            this.gvHie.MultiSelect = false;
            this.gvHie.Name = "gvHie";
            this.gvHie.ReadOnly = true;
            this.gvHie.RowHeadersVisible = false;
            this.gvHie.RowHeadersWidth = 25;
            this.gvHie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvHie.Size = new System.Drawing.Size(112, 505);
            this.gvHie.TabIndex = 0;
            this.gvHie.SelectionChanged += new System.EventHandler(this.gvHie_SelectionChanged);
            // 
            // Hierarchy
            // 
            this.Hierarchy.HeaderText = "Hierarchy";
            this.Hierarchy.Name = "Hierarchy";
            this.Hierarchy.ReadOnly = true;
            this.Hierarchy.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Hierarchy.Width = 85;
            // 
            // Site_Hierarchy
            // 
            this.Site_Hierarchy.HeaderText = "Site_Hierarchy";
            this.Site_Hierarchy.Name = "Site_Hierarchy";
            this.Site_Hierarchy.ReadOnly = true;
            this.Site_Hierarchy.Visible = false;
            this.Site_Hierarchy.Width = 70;
            // 
            // Hier_Desc
            // 
            this.Hier_Desc.Name = "Hier_Desc";
            this.Hier_Desc.ReadOnly = true;
            this.Hier_Desc.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(239, 242, 246);
            this.panel3.Controls.Add(this.gvHie);
            this.panel3.CssStyle = "border-radius:8px; border:1px solid #cccccc;";
            this.panel3.Dock = Wisej.Web.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(112, 505);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 25);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(5, 0, 0, 15);
            this.panel4.Size = new System.Drawing.Size(1160, 557);
            this.panel4.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel1);
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Dock = Wisej.Web.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 37);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1155, 505);
            this.panel5.TabIndex = 1;
            // 
            // Site_ScheduleControl
            // 
            this.Controls.Add(this.panel4);
            this.Name = "Site_ScheduleControl";
            this.Size = new System.Drawing.Size(1160, 582);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvsiteSchedule)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvHie)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private DataGridView gvsiteSchedule;
        private DataGridViewTextBoxColumn Month;
        private DataGridViewTextBoxColumn Sch_Type;
        private DataGridViewTextBoxColumn Site;
        private DataGridViewTextBoxColumn Site_Name;
        private DataGridViewTextBoxColumn Room;
        private DataGridViewTextBoxColumn Am_Pm;
        private DataGridViewTextBoxColumn Funding_Source;
        private ComboBox cmbYear;
        private Label lblYear;
        private Panel panel2;
        private DataGridView gvHie;
        private DataGridViewTextBoxColumn Hierarchy;
        private DataGridViewTextBoxColumn Site_Hierarchy;
        private DataGridViewTextBoxColumn Hier_Desc;
        private Panel panel3;
        private DataGridViewTextBoxColumn Month_No;
        private Label lblSort;
        private ComboBox cmbSort;
        private DataGridViewTextBoxColumn Add_Date;
        private DataGridViewTextBoxColumn Add_Operator;
        private DataGridViewTextBoxColumn Lstc_Date;
        private DataGridViewTextBoxColumn Lstc_Operator;
        private DataGridViewTextBoxColumn Fund;
        private DataGridViewTextBoxColumn Attm_ID;
        private DataGridViewTextBoxColumn Calander_Year;
        private Panel panel6;
        private Panel panel4;
        private Panel panel5;
    }
}