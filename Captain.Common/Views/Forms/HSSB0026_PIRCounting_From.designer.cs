using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class HSSB0026_PIRCounting_From
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB0026_PIRCounting_From));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlGrid_Questions = new Wisej.Web.Panel();
            this.Grid_Questions = new Wisej.Web.DataGridView();
            this.GD_Sel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.GD_Ques_Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_Ques_Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_Ques_Attn_1Day = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_Sys_Cnt = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_User_Cnt = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_Ques_ID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_Ques_Seq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_Ques_SCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GD_Ques_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlDropdowns = new Wisej.Web.Panel();
            this.lblSection = new Wisej.Web.Label();
            this.cmbFunds = new Wisej.Web.ComboBox();
            this.Cmb_Section = new Wisej.Web.ComboBox();
            this.lblFund = new Wisej.Web.Label();
            this.Cmb_Site = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblSite = new Wisej.Web.Label();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.Btn_Audit = new Wisej.Web.Button();
            this.spacer4 = new Wisej.Web.Spacer();
            this.Btn_Generate_Count = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.Btn_Save_Counts = new Wisej.Web.Button();
            this.Btn_Generate_Report = new Wisej.Web.Button();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlParams.SuspendLayout();
            this.pnlGrid_Questions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Questions)).BeginInit();
            this.pnlDropdowns.SuspendLayout();
            this.pnlHie.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlGrid_Questions);
            this.pnlParams.Controls.Add(this.pnlDropdowns);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(892, 367);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlGrid_Questions
            // 
            this.pnlGrid_Questions.Controls.Add(this.Grid_Questions);
            this.pnlGrid_Questions.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGrid_Questions.Location = new System.Drawing.Point(0, 42);
            this.pnlGrid_Questions.Name = "pnlGrid_Questions";
            this.pnlGrid_Questions.Size = new System.Drawing.Size(892, 325);
            this.pnlGrid_Questions.TabIndex = 44;
            // 
            // Grid_Questions
            // 
            this.Grid_Questions.AllowUserToResizeColumns = false;
            this.Grid_Questions.AllowUserToResizeRows = false;
            this.Grid_Questions.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.Grid_Questions.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.Grid_Questions.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Grid_Questions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Grid_Questions.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Questions.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.GD_Sel,
            this.GD_Ques_Type,
            this.GD_Ques_Desc,
            this.GD_Ques_Attn_1Day,
            this.GD_Sys_Cnt,
            this.GD_User_Cnt,
            this.GD_Ques_ID,
            this.GD_Ques_Seq,
            this.GD_Ques_SCode,
            this.GD_Ques_Code});
            this.Grid_Questions.Dock = Wisej.Web.DockStyle.Fill;
            this.Grid_Questions.Location = new System.Drawing.Point(0, 0);
            this.Grid_Questions.Name = "Grid_Questions";
            this.Grid_Questions.RowHeadersVisible = false;
            this.Grid_Questions.RowHeadersWidth = 25;
            this.Grid_Questions.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Grid_Questions.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Grid_Questions.Size = new System.Drawing.Size(892, 325);
            this.Grid_Questions.TabIndex = 55;
            this.Grid_Questions.TabStop = false;
            this.Grid_Questions.CellEndEdit += new Wisej.Web.DataGridViewCellEventHandler(this.Grid_Questions_CellEndEdit);
            this.Grid_Questions.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.Grid_Questions_CellClick);
            // 
            // GD_Sel
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.GD_Sel.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.GD_Sel.HeaderStyle = dataGridViewCellStyle3;
            this.GD_Sel.HeaderText = "Select";
            this.GD_Sel.Name = "GD_Sel";
            this.GD_Sel.Width = 70;
            // 
            // GD_Ques_Type
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.GD_Ques_Type.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.GD_Ques_Type.HeaderStyle = dataGridViewCellStyle5;
            this.GD_Ques_Type.HeaderText = "Type";
            this.GD_Ques_Type.Name = "GD_Ques_Type";
            this.GD_Ques_Type.ReadOnly = true;
            this.GD_Ques_Type.Width = 50;
            // 
            // GD_Ques_Desc
            // 
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.GD_Ques_Desc.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.GD_Ques_Desc.HeaderStyle = dataGridViewCellStyle7;
            this.GD_Ques_Desc.HeaderText = "Question Description";
            this.GD_Ques_Desc.Name = "GD_Ques_Desc";
            this.GD_Ques_Desc.ReadOnly = true;
            this.GD_Ques_Desc.Width = 520;
            // 
            // GD_Ques_Attn_1Day
            // 
            this.GD_Ques_Attn_1Day.HeaderText = " ";
            this.GD_Ques_Attn_1Day.Name = "GD_Ques_Attn_1Day";
            this.GD_Ques_Attn_1Day.ReadOnly = true;
            this.GD_Ques_Attn_1Day.ShowInVisibilityMenu = false;
            this.GD_Ques_Attn_1Day.Width = 20;
            // 
            // GD_Sys_Cnt
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.GD_Sys_Cnt.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.GD_Sys_Cnt.HeaderStyle = dataGridViewCellStyle9;
            this.GD_Sys_Cnt.HeaderText = "System";
            this.GD_Sys_Cnt.Name = "GD_Sys_Cnt";
            this.GD_Sys_Cnt.ReadOnly = true;
            this.GD_Sys_Cnt.Width = 100;
            // 
            // GD_User_Cnt
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.GD_User_Cnt.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.GD_User_Cnt.HeaderStyle = dataGridViewCellStyle11;
            this.GD_User_Cnt.HeaderText = "User";
            this.GD_User_Cnt.MaxInputLength = 10;
            this.GD_User_Cnt.Name = "GD_User_Cnt";
            this.GD_User_Cnt.ReadOnly = true;
            this.GD_User_Cnt.Width = 100;
            // 
            // GD_Ques_ID
            // 
            this.GD_Ques_ID.HeaderText = "GD_Ques_ID";
            this.GD_Ques_ID.Name = "GD_Ques_ID";
            this.GD_Ques_ID.ShowInVisibilityMenu = false;
            this.GD_Ques_ID.Visible = false;
            // 
            // GD_Ques_Seq
            // 
            this.GD_Ques_Seq.HeaderText = "GD_Ques_Seq";
            this.GD_Ques_Seq.Name = "GD_Ques_Seq";
            this.GD_Ques_Seq.ShowInVisibilityMenu = false;
            this.GD_Ques_Seq.Visible = false;
            // 
            // GD_Ques_SCode
            // 
            this.GD_Ques_SCode.HeaderText = "GD_Ques_SCode";
            this.GD_Ques_SCode.Name = "GD_Ques_SCode";
            this.GD_Ques_SCode.ShowInVisibilityMenu = false;
            this.GD_Ques_SCode.Visible = false;
            // 
            // GD_Ques_Code
            // 
            this.GD_Ques_Code.HeaderText = "GD_Ques_Code";
            this.GD_Ques_Code.Name = "GD_Ques_Code";
            this.GD_Ques_Code.ShowInVisibilityMenu = false;
            this.GD_Ques_Code.Visible = false;
            // 
            // pnlDropdowns
            // 
            this.pnlDropdowns.Controls.Add(this.lblSection);
            this.pnlDropdowns.Controls.Add(this.cmbFunds);
            this.pnlDropdowns.Controls.Add(this.Cmb_Section);
            this.pnlDropdowns.Controls.Add(this.lblFund);
            this.pnlDropdowns.Controls.Add(this.Cmb_Site);
            this.pnlDropdowns.Controls.Add(this.lblSite);
            this.pnlDropdowns.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDropdowns.Location = new System.Drawing.Point(0, 0);
            this.pnlDropdowns.Name = "pnlDropdowns";
            this.pnlDropdowns.Size = new System.Drawing.Size(892, 42);
            this.pnlDropdowns.TabIndex = 1;
            // 
            // lblSection
            // 
            this.lblSection.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSection.Location = new System.Drawing.Point(15, 15);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(42, 16);
            this.lblSection.TabIndex = 0;
            this.lblSection.Text = "Section";
            // 
            // cmbFunds
            // 
            this.cmbFunds.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbFunds.FormattingEnabled = true;
            this.cmbFunds.Location = new System.Drawing.Point(260, 11);
            this.cmbFunds.Name = "cmbFunds";
            this.cmbFunds.Size = new System.Drawing.Size(100, 25);
            this.cmbFunds.TabIndex = 2;
            this.cmbFunds.SelectedIndexChanged += new System.EventHandler(this.Reload_Questions_Grid);
            // 
            // Cmb_Section
            // 
            this.Cmb_Section.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb_Section.FormattingEnabled = true;
            this.Cmb_Section.Location = new System.Drawing.Point(69, 11);
            this.Cmb_Section.Name = "Cmb_Section";
            this.Cmb_Section.Size = new System.Drawing.Size(108, 25);
            this.Cmb_Section.TabIndex = 1;
            this.Cmb_Section.SelectedIndexChanged += new System.EventHandler(this.Reload_Questions_Grid);
            // 
            // lblFund
            // 
            this.lblFund.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFund.Location = new System.Drawing.Point(211, 15);
            this.lblFund.Name = "lblFund";
            this.lblFund.Size = new System.Drawing.Size(36, 16);
            this.lblFund.TabIndex = 0;
            this.lblFund.Text = "Fund";
            // 
            // Cmb_Site
            // 
            this.Cmb_Site.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb_Site.FormattingEnabled = true;
            this.Cmb_Site.Location = new System.Drawing.Point(440, 11);
            this.Cmb_Site.Name = "Cmb_Site";
            this.Cmb_Site.Size = new System.Drawing.Size(310, 25);
            this.Cmb_Site.TabIndex = 3;
            this.Cmb_Site.SelectedIndexChanged += new System.EventHandler(this.Reload_Questions_Grid);
            // 
            // lblSite
            // 
            this.lblSite.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSite.Location = new System.Drawing.Point(403, 15);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(22, 16);
            this.lblSite.TabIndex = 0;
            this.lblSite.Text = "Site";
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(735, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 66;
            this.CmbYear.TabStop = false;
            this.CmbYear.Visible = false;
            this.CmbYear.SelectedIndexChanged += new System.EventHandler(this.CmbYear_SelectedIndexChanged);
            // 
            // Txt_HieDesc
            // 
            this.Txt_HieDesc.BackColor = System.Drawing.Color.Transparent;
            this.Txt_HieDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Txt_HieDesc.Dock = Wisej.Web.DockStyle.Left;
            this.Txt_HieDesc.Font = new System.Drawing.Font("defaultBold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Txt_HieDesc.ForeColor = System.Drawing.Color.White;
            this.Txt_HieDesc.Location = new System.Drawing.Point(0, 0);
            this.Txt_HieDesc.Name = "Txt_HieDesc";
            this.Txt_HieDesc.ReadOnly = true;
            this.Txt_HieDesc.Size = new System.Drawing.Size(720, 25);
            this.Txt_HieDesc.TabIndex = 77;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer1);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(800, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(720, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.Btn_Audit);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.Btn_Generate_Count);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.Btn_Save_Counts);
            this.pnlGenerate.Controls.Add(this.Btn_Generate_Report);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 410);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(892, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // Btn_Audit
            // 
            this.Btn_Audit.AppearanceKey = "button-reports";
            this.Btn_Audit.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Audit.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Audit.Location = new System.Drawing.Point(148, 5);
            this.Btn_Audit.Name = "Btn_Audit";
            this.Btn_Audit.Size = new System.Drawing.Size(140, 25);
            this.Btn_Audit.TabIndex = 2;
            this.Btn_Audit.Text = "Generate &Audit Report";
            this.Btn_Audit.Click += new System.EventHandler(this.Btn_Generate_Report_Click);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(145, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // Btn_Generate_Count
            // 
            this.Btn_Generate_Count.AppearanceKey = "button-reports";
            this.Btn_Generate_Count.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Generate_Count.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Generate_Count.Location = new System.Drawing.Point(614, 5);
            this.Btn_Generate_Count.Name = "Btn_Generate_Count";
            this.Btn_Generate_Count.Size = new System.Drawing.Size(130, 25);
            this.Btn_Generate_Count.TabIndex = 3;
            this.Btn_Generate_Count.Text = "&Generate PIR Counts";
            this.Btn_Generate_Count.Click += new System.EventHandler(this.Btn_Generate_Count_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(744, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // Btn_Save_Counts
            // 
            this.Btn_Save_Counts.AppearanceKey = "button-reports";
            this.Btn_Save_Counts.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Save_Counts.Enabled = false;
            this.Btn_Save_Counts.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Save_Counts.Location = new System.Drawing.Point(747, 5);
            this.Btn_Save_Counts.Name = "Btn_Save_Counts";
            this.Btn_Save_Counts.Size = new System.Drawing.Size(130, 25);
            this.Btn_Save_Counts.TabIndex = 4;
            this.Btn_Save_Counts.Text = "&Save Report Counts";
            this.Btn_Save_Counts.Click += new System.EventHandler(this.Btn_Save_Counts_Click);
            // 
            // Btn_Generate_Report
            // 
            this.Btn_Generate_Report.AppearanceKey = "button-reports";
            this.Btn_Generate_Report.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Generate_Report.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Generate_Report.Location = new System.Drawing.Point(15, 5);
            this.Btn_Generate_Report.Name = "Btn_Generate_Report";
            this.Btn_Generate_Report.Size = new System.Drawing.Size(130, 25);
            this.Btn_Generate_Report.TabIndex = 1;
            this.Btn_Generate_Report.Text = "Generate &PIR Report";
            this.Btn_Generate_Report.Click += new System.EventHandler(this.Btn_Generate_Report_Click);
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.CssStyle = "border-radius:25px";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(15, 0);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(892, 445);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlHieFilter
            // 
            this.pnlHieFilter.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHieFilter.Controls.Add(this.pnlFilter);
            this.pnlHieFilter.Controls.Add(this.pnlHie);
            this.pnlHieFilter.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHieFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlHieFilter.Name = "pnlHieFilter";
            this.pnlHieFilter.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.pnlHieFilter.Size = new System.Drawing.Size(892, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(815, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(68, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB0026_PIRCounting_From
            // 
            this.ClientSize = new System.Drawing.Size(892, 445);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB0026_PIRCounting_From";
            this.Text = "HSSB0026_PIRCounting_From";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.HSSB0026_PIRCounting_From_Load);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB0026_PIRCounting_From_ToolClick);
            this.pnlParams.ResumeLayout(false);
            this.pnlGrid_Questions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Questions)).EndInit();
            this.pnlDropdowns.ResumeLayout(false);
            this.pnlDropdowns.PerformLayout();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlParams;
        private DataGridView Grid_Questions;
        private Label lblSite;
        private ComboBoxEx Cmb_Site;
        private ComboBox Cmb_Section;
        private Label lblSection;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private Panel pnlGenerate;
        private Button Btn_Generate_Count;
        private Button Btn_Audit;
        private Button Btn_Generate_Report;
        private DataGridViewCheckBoxColumn GD_Sel;
        private DataGridViewTextBoxColumn GD_Ques_Type;
        private DataGridViewTextBoxColumn GD_Ques_Desc;
        private DataGridViewTextBoxColumn GD_Sys_Cnt;
        private DataGridViewTextBoxColumn GD_User_Cnt;
        private PictureBox Pb_Search_Hie;
        private Button Btn_Save_Counts;
        private DataGridViewTextBoxColumn GD_Ques_ID;
        private DataGridViewTextBoxColumn GD_Ques_Seq;
        private DataGridViewTextBoxColumn GD_Ques_Attn_1Day;
        private DataGridViewTextBoxColumn GD_Ques_SCode;
        private DataGridViewTextBoxColumn GD_Ques_Code;
        private ComboBox cmbFunds;
        private Label lblFund;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Panel pnlDropdowns;
        private Panel pnlGrid_Questions;
    }
}