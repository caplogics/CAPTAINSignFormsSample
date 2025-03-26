using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class RNG_ResultAssociations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RNG_ResultAssociations));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.lblRefDt = new Wisej.Web.Label();
            this.lblRefPeriod = new Wisej.Web.Label();
            this.pnlGroup = new Wisej.Web.Panel();
            this.CmbRsltHead = new Wisej.Web.ComboBox();
            this.txtDesc = new Wisej.Web.TextBox();
            this.txtCode = new Wisej.Web.TextBox();
            this.lblRsltHead = new Wisej.Web.Label();
            this.lblGrp = new Wisej.Web.Label();
            this.pnlGvResults = new Wisej.Web.Panel();
            this.GvResults = new Wisej.Web.DataGridView();
            this.Grid_Img = new Wisej.Web.DataGridViewImageColumn();
            this.Results = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Agy_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Img_Cd = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlGroup.SuspendLayout();
            this.pnlGvResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvResults)).BeginInit();
            this.pnlSave.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRefDt
            // 
            this.lblRefDt.Location = new System.Drawing.Point(130, 12);
            this.lblRefDt.Name = "lblRefDt";
            this.lblRefDt.Size = new System.Drawing.Size(315, 14);
            this.lblRefDt.TabIndex = 1;
            // 
            // lblRefPeriod
            // 
            this.lblRefPeriod.Location = new System.Drawing.Point(15, 12);
            this.lblRefPeriod.Name = "lblRefPeriod";
            this.lblRefPeriod.Size = new System.Drawing.Size(98, 14);
            this.lblRefPeriod.TabIndex = 1;
            this.lblRefPeriod.Text = "Reference Period";
            // 
            // pnlGroup
            // 
            this.pnlGroup.Controls.Add(this.CmbRsltHead);
            this.pnlGroup.Controls.Add(this.txtDesc);
            this.pnlGroup.Controls.Add(this.txtCode);
            this.pnlGroup.Controls.Add(this.lblRsltHead);
            this.pnlGroup.Controls.Add(this.lblGrp);
            this.pnlGroup.Dock = Wisej.Web.DockStyle.Top;
            this.pnlGroup.Location = new System.Drawing.Point(0, 29);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(706, 73);
            this.pnlGroup.TabIndex = 2;
            // 
            // CmbRsltHead
            // 
            this.CmbRsltHead.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbRsltHead.FormattingEnabled = true;
            this.CmbRsltHead.Location = new System.Drawing.Point(106, 41);
            this.CmbRsltHead.Name = "CmbRsltHead";
            this.CmbRsltHead.Size = new System.Drawing.Size(96, 25);
            this.CmbRsltHead.TabIndex = 2;
            this.CmbRsltHead.SelectedIndexChanged += new System.EventHandler(this.CmbRsltHead_SelectedIndexChanged);
            // 
            // txtDesc
            // 
            this.txtDesc.Enabled = false;
            this.txtDesc.Location = new System.Drawing.Point(212, 9);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(469, 25);
            this.txtDesc.TabIndex = 1;
            // 
            // txtCode
            // 
            this.txtCode.Enabled = false;
            this.txtCode.Location = new System.Drawing.Point(106, 9);
            this.txtCode.MaxLength = 10;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(96, 25);
            this.txtCode.TabIndex = 1;
            // 
            // lblRsltHead
            // 
            this.lblRsltHead.Location = new System.Drawing.Point(15, 42);
            this.lblRsltHead.Name = "lblRsltHead";
            this.lblRsltHead.Size = new System.Drawing.Size(81, 14);
            this.lblRsltHead.TabIndex = 0;
            this.lblRsltHead.Text = "Result Header";
            // 
            // lblGrp
            // 
            this.lblGrp.Location = new System.Drawing.Point(15, 12);
            this.lblGrp.Name = "lblGrp";
            this.lblGrp.Size = new System.Drawing.Size(36, 16);
            this.lblGrp.TabIndex = 0;
            this.lblGrp.Text = "Group";
            // 
            // pnlGvResults
            // 
            this.pnlGvResults.Controls.Add(this.GvResults);
            this.pnlGvResults.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGvResults.Location = new System.Drawing.Point(0, 102);
            this.pnlGvResults.Name = "pnlGvResults";
            this.pnlGvResults.Size = new System.Drawing.Size(706, 268);
            this.pnlGvResults.TabIndex = 3;
            // 
            // GvResults
            // 
            this.GvResults.AllowUserToResizeColumns = false;
            this.GvResults.AllowUserToResizeRows = false;
            this.GvResults.BackColor = System.Drawing.Color.White;
            this.GvResults.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.GvResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GvResults.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvResults.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Grid_Img,
            this.Results,
            this.Agy_Code,
            this.Img_Cd});
            this.GvResults.Dock = Wisej.Web.DockStyle.Fill;
            this.GvResults.Location = new System.Drawing.Point(0, 0);
            this.GvResults.MultiSelect = false;
            this.GvResults.Name = "GvResults";
            this.GvResults.RowHeadersWidth = 25;
            this.GvResults.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.GvResults.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.GvResults.ShowColumnVisibilityMenu = false;
            this.GvResults.Size = new System.Drawing.Size(706, 268);
            this.GvResults.TabIndex = 0;
            this.GvResults.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.GvResults_CellClick);
            // 
            // Grid_Img
            // 
            this.Grid_Img.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.Grid_Img.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.BestFit;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Grid_Img.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Grid_Img.HeaderStyle = dataGridViewCellStyle3;
            this.Grid_Img.HeaderText = " ";
            this.Grid_Img.Name = "Grid_Img";
            this.Grid_Img.ShowInVisibilityMenu = false;
            this.Grid_Img.Width = 30;
            // 
            // Results
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Results.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Results.HeaderStyle = dataGridViewCellStyle5;
            this.Results.HeaderText = "Results Description";
            this.Results.Name = "Results";
            this.Results.ReadOnly = true;
            this.Results.Width = 560;
            // 
            // Agy_Code
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Agy_Code.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Agy_Code.HeaderStyle = dataGridViewCellStyle7;
            this.Agy_Code.HeaderText = "Agy_Code";
            this.Agy_Code.Name = "Agy_Code";
            this.Agy_Code.ShowInVisibilityMenu = false;
            this.Agy_Code.Visible = false;
            this.Agy_Code.Width = 30;
            // 
            // Img_Cd
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Img_Cd.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Img_Cd.HeaderStyle = dataGridViewCellStyle9;
            this.Img_Cd.HeaderText = "Img_Cd";
            this.Img_Cd.Name = "Img_Cd";
            this.Img_Cd.ShowInVisibilityMenu = false;
            this.Img_Cd.Visible = false;
            this.Img_Cd.Width = 30;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(616, 5);
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
            this.btnSave.Location = new System.Drawing.Point(538, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 370);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(706, 35);
            this.pnlSave.TabIndex = 6;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(613, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGvResults);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Controls.Add(this.pnlGroup);
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(706, 405);
            this.pnlCompleteForm.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblRefPeriod);
            this.panel1.Controls.Add(this.lblRefDt);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 29);
            this.panel1.TabIndex = 4;
            // 
            // RNG_ResultAssociations
            // 
            this.ClientSize = new System.Drawing.Size(706, 405);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RNG_ResultAssociations";
            this.Text = "Result Associations";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            this.pnlGvResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GvResults)).EndInit();
            this.pnlSave.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblRefDt;
        private Label lblRefPeriod;
        private Panel pnlGroup;
        private ComboBox CmbRsltHead;
        private TextBox txtDesc;
        private TextBox txtCode;
        private Label lblRsltHead;
        private Label lblGrp;
        private Panel pnlGvResults;
        private DataGridView GvResults;
        private Button btnCancel;
        private Button btnSave;
        private DataGridViewImageColumn Grid_Img;
        private DataGridViewTextBoxColumn Results;
        private DataGridViewTextBoxColumn Agy_Code;
        private DataGridViewTextBoxColumn Img_Cd;
        private Panel pnlSave;
        private Spacer spacer1;
        private Panel pnlCompleteForm;
        private Panel panel1;
    }
}