using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class MembersGridForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MembersGridForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.CA_Members_Grid = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.CA_Sel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.CA_Mem_Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CA_Mem_SSN = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.CA_Mem_Relation = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CA_Mem_Seq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CA_CLID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CA_AppSw = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Is_CAOBO_Rec = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CA_Active_Sw = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CA_Exclude_Sw = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cmb_CA_Benefit = new Wisej.Web.ComboBox();
            this.lblBenefit = new Wisej.Web.Label();
            this.Btn_MS_Save = new Wisej.Web.Button();
            this.button3 = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.CA_Members_Grid)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // CA_Members_Grid
            // 
            this.CA_Members_Grid.AllowUserToResizeColumns = false;
            this.CA_Members_Grid.AllowUserToResizeRows = false;
            this.CA_Members_Grid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.CA_Members_Grid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.CA_Members_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.CA_Members_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CA_Members_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CA_Members_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.CA_Sel,
            this.CA_Mem_Name,
            this.CA_Mem_SSN,
            this.CA_Mem_Relation,
            this.CA_Mem_Seq,
            this.CA_CLID,
            this.CA_AppSw,
            this.Is_CAOBO_Rec,
            this.CA_Active_Sw,
            this.CA_Exclude_Sw});
            this.CA_Members_Grid.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.CA_Members_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.CA_Members_Grid.Location = new System.Drawing.Point(10, 10);
            this.CA_Members_Grid.Name = "CA_Members_Grid";
            this.CA_Members_Grid.RowHeadersVisible = false;
            this.CA_Members_Grid.RowHeadersWidth = 25;
            this.CA_Members_Grid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CA_Members_Grid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.CA_Members_Grid.Size = new System.Drawing.Size(626, 202);
            this.CA_Members_Grid.TabIndex = 3;
            this.CA_Members_Grid.TabStop = false;
            this.CA_Members_Grid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.CA_Members_Grid_CellClick);
            // 
            // CA_Sel
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.CA_Sel.DefaultCellStyle = dataGridViewCellStyle2;
            this.CA_Sel.HeaderText = " ";
            this.CA_Sel.Name = "CA_Sel";
            this.CA_Sel.ShowInVisibilityMenu = false;
            this.CA_Sel.Width = 25;
            // 
            // CA_Mem_Name
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.CA_Mem_Name.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CA_Mem_Name.HeaderStyle = dataGridViewCellStyle4;
            this.CA_Mem_Name.HeaderText = "Member Name";
            this.CA_Mem_Name.Name = "CA_Mem_Name";
            this.CA_Mem_Name.ReadOnly = true;
            this.CA_Mem_Name.Width = 365;
            // 
            // CA_Mem_SSN
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CA_Mem_SSN.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CA_Mem_SSN.HeaderStyle = dataGridViewCellStyle6;
            this.CA_Mem_SSN.HeaderText = "DOB";
            this.CA_Mem_SSN.Name = "CA_Mem_SSN";
            this.CA_Mem_SSN.ReadOnly = true;
            this.CA_Mem_SSN.Width = 110;
            // 
            // CA_Mem_Relation
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CA_Mem_Relation.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CA_Mem_Relation.HeaderStyle = dataGridViewCellStyle8;
            this.CA_Mem_Relation.HeaderText = "Relation";
            this.CA_Mem_Relation.Name = "CA_Mem_Relation";
            this.CA_Mem_Relation.ReadOnly = true;
            this.CA_Mem_Relation.Width = 130;
            // 
            // CA_Mem_Seq
            // 
            this.CA_Mem_Seq.HeaderText = "CA_Mem_Seq";
            this.CA_Mem_Seq.Name = "CA_Mem_Seq";
            this.CA_Mem_Seq.ReadOnly = true;
            this.CA_Mem_Seq.ShowInVisibilityMenu = false;
            this.CA_Mem_Seq.Visible = false;
            // 
            // CA_CLID
            // 
            this.CA_CLID.HeaderText = "CA_CLID";
            this.CA_CLID.Name = "CA_CLID";
            this.CA_CLID.ShowInVisibilityMenu = false;
            this.CA_CLID.Visible = false;
            // 
            // CA_AppSw
            // 
            this.CA_AppSw.HeaderText = "CA_AppSw";
            this.CA_AppSw.Name = "CA_AppSw";
            this.CA_AppSw.ShowInVisibilityMenu = false;
            this.CA_AppSw.Visible = false;
            // 
            // Is_CAOBO_Rec
            // 
            this.Is_CAOBO_Rec.HeaderText = "Is_CAOBO_Rec";
            this.Is_CAOBO_Rec.Name = "Is_CAOBO_Rec";
            this.Is_CAOBO_Rec.ShowInVisibilityMenu = false;
            this.Is_CAOBO_Rec.Visible = false;
            // 
            // CA_Active_Sw
            // 
            this.CA_Active_Sw.HeaderText = "CA_Active_Sw";
            this.CA_Active_Sw.Name = "CA_Active_Sw";
            this.CA_Active_Sw.ShowInVisibilityMenu = false;
            this.CA_Active_Sw.Visible = false;
            // 
            // CA_Exclude_Sw
            // 
            this.CA_Exclude_Sw.HeaderText = "CA_Exclude_Sw";
            this.CA_Exclude_Sw.Name = "CA_Exclude_Sw";
            this.CA_Exclude_Sw.ShowInVisibilityMenu = false;
            this.CA_Exclude_Sw.Visible = false;
            // 
            // cmb_CA_Benefit
            // 
            this.cmb_CA_Benefit.Dock = Wisej.Web.DockStyle.Left;
            this.cmb_CA_Benefit.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmb_CA_Benefit.Enabled = false;
            this.cmb_CA_Benefit.FormattingEnabled = true;
            this.cmb_CA_Benefit.Location = new System.Drawing.Point(137, 5);
            this.cmb_CA_Benefit.Name = "cmb_CA_Benefit";
            this.cmb_CA_Benefit.Size = new System.Drawing.Size(219, 25);
            this.cmb_CA_Benefit.TabIndex = 7;
            this.cmb_CA_Benefit.SelectedIndexChanged += new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);
            // 
            // lblBenefit
            // 
            this.lblBenefit.AutoSize = true;
            this.lblBenefit.Dock = Wisej.Web.DockStyle.Left;
            this.lblBenefit.Enabled = false;
            this.lblBenefit.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBenefit.Location = new System.Drawing.Point(15, 5);
            this.lblBenefit.MinimumSize = new System.Drawing.Size(122, 0);
            this.lblBenefit.Name = "lblBenefit";
            this.lblBenefit.Size = new System.Drawing.Size(122, 25);
            this.lblBenefit.TabIndex = 1;
            this.lblBenefit.Text = "Benefiting from Service";
            this.lblBenefit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Btn_MS_Save
            // 
            this.Btn_MS_Save.AppearanceKey = "button-ok";
            this.Btn_MS_Save.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_MS_Save.Location = new System.Drawing.Point(493, 5);
            this.Btn_MS_Save.Name = "Btn_MS_Save";
            this.Btn_MS_Save.Size = new System.Drawing.Size(60, 25);
            this.Btn_MS_Save.TabIndex = 1;
            this.Btn_MS_Save.Text = "&OK";
            this.Btn_MS_Save.Click += new System.EventHandler(this.Btn_MS_Save_Click);
            // 
            // button3
            // 
            this.button3.AppearanceKey = "button-cancel";
            this.button3.Dock = Wisej.Web.DockStyle.Right;
            this.button3.Location = new System.Drawing.Point(556, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 25);
            this.button3.TabIndex = 2;
            this.button3.Text = "&Cancel";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmb_CA_Benefit);
            this.panel1.Controls.Add(this.lblBenefit);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(15, 5, 5, 5);
            this.panel1.Size = new System.Drawing.Size(646, 35);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CA_Members_Grid);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(10);
            this.panel2.Size = new System.Drawing.Size(646, 222);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.Btn_MS_Save);
            this.panel3.Controls.Add(this.spacer1);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 257);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel3.Size = new System.Drawing.Size(646, 35);
            this.panel3.TabIndex = 3;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(553, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // MembersGridForm
            // 
            this.ClientSize = new System.Drawing.Size(646, 292);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MembersGridForm";
            this.Text = "Members Grid Form";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            ((System.ComponentModel.ISupportInitialize)(this.CA_Members_Grid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private DataGridViewEx CA_Members_Grid;
        private ComboBox cmb_CA_Benefit;
        private Label lblBenefit;
        private Button Btn_MS_Save;
        private Button button3;
        private DataGridViewCheckBoxColumn CA_Sel;
        private DataGridViewTextBoxColumn CA_Mem_Name;
        private DataGridViewTextBoxColumn CA_Mem_Relation;
        private DataGridViewTextBoxColumn CA_Mem_Seq;
        private DataGridViewTextBoxColumn CA_CLID;
        private DataGridViewTextBoxColumn CA_AppSw;
        private DataGridViewTextBoxColumn Is_CAOBO_Rec;
        private DataGridViewTextBoxColumn CA_Active_Sw;
        private DataGridViewTextBoxColumn CA_Exclude_Sw;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Spacer spacer1;
        private Controls.Compatibility.DataGridViewDateTimeColumn CA_Mem_SSN;
    }
}