using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class RNG_Goalservices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RNG_Goalservices));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.lblRefPeriod = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.CmbTbl = new Wisej.Web.ComboBox();
            this.CmbGrp = new Wisej.Web.ComboBox();
            this.lblTbl = new Wisej.Web.Label();
            this.lblGrp = new Wisej.Web.Label();
            this.GvGoals = new Wisej.Web.DataGridView();
            this.Check = new Wisej.Web.DataGridViewImageColumn();
            this.RNG_SP = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Goals = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Agy_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Img_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Budget = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SP_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlGvGoals = new Wisej.Web.Panel();
            this.lblRefDt = new Wisej.Web.Label();
            this.btnSave = new Wisej.Web.Button();
            this.btnClose = new Wisej.Web.Button();
            this.BtnSearch = new Wisej.Web.Button();
            this.TxtSearch = new Wisej.Web.TextBox();
            this.lblsearchTxt = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.textBox1 = new Wisej.Web.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvGoals)).BeginInit();
            this.pnlGvGoals.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRefPeriod
            // 
            this.lblRefPeriod.Location = new System.Drawing.Point(15, 15);
            this.lblRefPeriod.Name = "lblRefPeriod";
            this.lblRefPeriod.Size = new System.Drawing.Size(35, 14);
            this.lblRefPeriod.TabIndex = 1;
            this.lblRefPeriod.Text = "Name";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CmbTbl);
            this.panel1.Controls.Add(this.CmbGrp);
            this.panel1.Controls.Add(this.lblTbl);
            this.panel1.Controls.Add(this.lblGrp);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(742, 69);
            this.panel1.TabIndex = 2;
            // 
            // CmbTbl
            // 
            this.CmbTbl.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbTbl.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CmbTbl.FormattingEnabled = true;
            this.CmbTbl.Location = new System.Drawing.Point(71, 36);
            this.CmbTbl.Name = "CmbTbl";
            this.CmbTbl.Size = new System.Drawing.Size(640, 25);
            this.CmbTbl.TabIndex = 2;
            this.CmbTbl.SelectedIndexChanged += new System.EventHandler(this.CmbTbl_SelectedIndexChanged);
            // 
            // CmbGrp
            // 
            this.CmbGrp.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbGrp.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CmbGrp.FormattingEnabled = true;
            this.CmbGrp.Location = new System.Drawing.Point(71, 4);
            this.CmbGrp.Name = "CmbGrp";
            this.CmbGrp.Size = new System.Drawing.Size(640, 25);
            this.CmbGrp.TabIndex = 2;
            this.CmbGrp.SelectedIndexChanged += new System.EventHandler(this.CmbGrp_SelectedIndexChanged);
            // 
            // lblTbl
            // 
            this.lblTbl.Location = new System.Drawing.Point(15, 40);
            this.lblTbl.MinimumSize = new System.Drawing.Size(36, 0);
            this.lblTbl.Name = "lblTbl";
            this.lblTbl.Size = new System.Drawing.Size(36, 18);
            this.lblTbl.TabIndex = 1;
            this.lblTbl.Text = "Group";
            // 
            // lblGrp
            // 
            this.lblGrp.Location = new System.Drawing.Point(15, 8);
            this.lblGrp.MinimumSize = new System.Drawing.Size(45, 0);
            this.lblGrp.Name = "lblGrp";
            this.lblGrp.Size = new System.Drawing.Size(45, 16);
            this.lblGrp.TabIndex = 1;
            this.lblGrp.Text = "Domain";
            // 
            // GvGoals
            // 
            this.GvGoals.AllowUserToResizeColumns = false;
            this.GvGoals.AllowUserToResizeRows = false;
            this.GvGoals.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.GvGoals.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.GvGoals.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.GvGoals.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GvGoals.ColumnHeadersHeight = 25;
            this.GvGoals.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvGoals.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Check,
            this.RNG_SP,
            this.Goals,
            this.Agy_Code,
            this.Img_Code,
            this.Budget,
            this.SP_Code});
            this.GvGoals.Dock = Wisej.Web.DockStyle.Fill;
            this.GvGoals.Location = new System.Drawing.Point(0, 0);
            this.GvGoals.MultiSelect = false;
            this.GvGoals.Name = "GvGoals";
            this.GvGoals.RowHeadersWidth = 25;
            this.GvGoals.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GvGoals.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.GvGoals.Size = new System.Drawing.Size(742, 376);
            this.GvGoals.TabIndex = 3;
            this.GvGoals.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellValueChanged);
            this.GvGoals.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellClick);
            // 
            // Check
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = null;
            this.Check.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Check.HeaderStyle = dataGridViewCellStyle3;
            this.Check.HeaderText = " ";
            this.Check.Name = "Check";
            this.Check.ShowInVisibilityMenu = false;
            this.Check.Width = 30;
            // 
            // RNG_SP
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.RNG_SP.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.RNG_SP.HeaderStyle = dataGridViewCellStyle5;
            this.RNG_SP.HeaderText = "Service Plan";
            this.RNG_SP.Name = "RNG_SP";
            this.RNG_SP.Width = 200;
            // 
            // Goals
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Goals.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Goals.HeaderStyle = dataGridViewCellStyle7;
            this.Goals.HeaderText = "Outcomes";
            this.Goals.Name = "Goals";
            this.Goals.ReadOnly = true;
            this.Goals.Width = 280;
            // 
            // Agy_Code
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Agy_Code.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Agy_Code.HeaderStyle = dataGridViewCellStyle9;
            this.Agy_Code.HeaderText = "Code";
            this.Agy_Code.Name = "Agy_Code";
            this.Agy_Code.ReadOnly = true;
            this.Agy_Code.Width = 80;
            // 
            // Img_Code
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Img_Code.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Img_Code.HeaderStyle = dataGridViewCellStyle11;
            this.Img_Code.HeaderText = "Img_Code";
            this.Img_Code.Name = "Img_Code";
            this.Img_Code.ShowInVisibilityMenu = false;
            this.Img_Code.Visible = false;
            this.Img_Code.Width = 30;
            // 
            // Budget
            // 
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.Budget.DefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Budget.HeaderStyle = dataGridViewCellStyle13;
            this.Budget.HeaderText = "Budget";
            this.Budget.Name = "Budget";
            this.Budget.Width = 75;
            // 
            // SP_Code
            // 
            this.SP_Code.HeaderText = "SP_Code";
            this.SP_Code.Name = "SP_Code";
            this.SP_Code.ShowInVisibilityMenu = false;
            this.SP_Code.Visible = false;
            this.SP_Code.Width = 20;
            // 
            // pnlGvGoals
            // 
            this.pnlGvGoals.Controls.Add(this.GvGoals);
            this.pnlGvGoals.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGvGoals.Location = new System.Drawing.Point(0, 108);
            this.pnlGvGoals.Name = "pnlGvGoals";
            this.pnlGvGoals.Size = new System.Drawing.Size(742, 376);
            this.pnlGvGoals.TabIndex = 4;
            // 
            // lblRefDt
            // 
            this.lblRefDt.Location = new System.Drawing.Point(72, 15);
            this.lblRefDt.Name = "lblRefDt";
            this.lblRefDt.Size = new System.Drawing.Size(202, 16);
            this.lblRefDt.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(574, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-error";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(652, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(637, 11);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 25);
            this.BtnSearch.TabIndex = 5;
            this.BtnSearch.Text = "S&earch";
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // TxtSearch
            // 
            this.TxtSearch.Location = new System.Drawing.Point(361, 11);
            this.TxtSearch.MaxLength = 100;
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.Size = new System.Drawing.Size(263, 25);
            this.TxtSearch.TabIndex = 4;
            // 
            // lblsearchTxt
            // 
            this.lblsearchTxt.Location = new System.Drawing.Point(287, 15);
            this.lblsearchTxt.Name = "lblsearchTxt";
            this.lblsearchTxt.Size = new System.Drawing.Size(67, 14);
            this.lblsearchTxt.TabIndex = 1;
            this.lblsearchTxt.Text = "Search Text";
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 484);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(742, 35);
            this.panel2.TabIndex = 6;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(649, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGvGoals);
            this.pnlCompleteForm.Controls.Add(this.panel2);
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Controls.Add(this.panel3);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(742, 519);
            this.pnlCompleteForm.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.TxtSearch);
            this.panel3.Controls.Add(this.lblRefDt);
            this.panel3.Controls.Add(this.lblsearchTxt);
            this.panel3.Controls.Add(this.lblRefPeriod);
            this.panel3.Controls.Add(this.BtnSearch);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(742, 39);
            this.panel3.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Transparent;
            this.textBox1.BorderStyle = Wisej.Web.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.Color.Transparent;
            this.textBox1.Location = new System.Drawing.Point(296, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 25);
            this.textBox1.TabIndex = 8;
            // 
            // RNG_Goalservices
            // 
            this.ClientSize = new System.Drawing.Size(742, 519);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RNG_Goalservices";
            this.Text = "Goal/Service Association";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.FormClosed += new Wisej.Web.FormClosedEventHandler(this.RNG_Goalservices_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvGoals)).EndInit();
            this.pnlGvGoals.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblRefPeriod;
        private Panel panel1;
        private ComboBox CmbTbl;
        private ComboBox CmbGrp;
        private Label lblTbl;
        private Label lblGrp;
        private DataGridView GvGoals;
        private Panel pnlGvGoals;
        private Label lblRefDt;
        private Button btnSave;
        private Button btnClose;
        private DataGridViewImageColumn Check;
        private DataGridViewTextBoxColumn Goals;
        private DataGridViewTextBoxColumn Agy_Code;
        private DataGridViewTextBoxColumn Img_Code;
        private DataGridViewTextBoxColumn Budget;
        private Button BtnSearch;
        private TextBox TxtSearch;
        private Label lblsearchTxt;
        private Panel panel2;
        private Spacer spacer1;
        private Panel pnlCompleteForm;
        private Panel panel3;
        private DataGridViewTextBoxColumn RNG_SP;
        private DataGridViewTextBoxColumn SP_Code;
        private TextBox textBox1;
    }
}