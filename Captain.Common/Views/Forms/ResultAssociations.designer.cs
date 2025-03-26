using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class ResultAssociations
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
            this.LblHeader = new Wisej.Web.Label();
            this.pictureBox1 = new Wisej.Web.PictureBox();
            this.pnlHeader = new Wisej.Web.Panel();
            this.pictureBox3 = new Wisej.Web.PictureBox();
            this.lblRefDt = new Wisej.Web.Label();
            this.lblRefPeriod = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.CmbRsltHead = new Wisej.Web.ComboBox();
            this.txtDesc = new Wisej.Web.TextBox();
            this.txtCode = new Wisej.Web.TextBox();
            this.lblRsltHead = new Wisej.Web.Label();
            this.lblGrp = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.GvResults = new Wisej.Web.DataGridView();
            this.Grid_Img = new Wisej.Web.DataGridViewImageColumn();
            this.Results = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Agy_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Img_Cd = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // LblHeader
            // 
            this.LblHeader.AutoSize = true;
            this.LblHeader.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblHeader.ForeColor = System.Drawing.Color.White;
            this.LblHeader.Location = new System.Drawing.Point(61, 16);
            this.LblHeader.Name = "LblHeader";
            this.LblHeader.Size = new System.Drawing.Size(171, 26);
            this.LblHeader.TabIndex = 1;
            this.LblHeader.Text = "Result Association";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.ImageSource = "icon-header";
            this.pictureBox1.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(6, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 44);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlHeader.Controls.Add(this.pictureBox3);
            this.pnlHeader.Controls.Add(this.LblHeader);
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(619, 55);
            this.pnlHeader.TabIndex = 0;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.pictureBox3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox3.Cursor = Wisej.Web.Cursors.Hand;
            this.pictureBox3.ImageSource = "icon-help?color=captainBrown";
            this.pictureBox3.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pictureBox3.Location = new System.Drawing.Point(592, 19);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(18, 22);
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // lblRefDt
            // 
            this.lblRefDt.AutoSize = true;
            this.lblRefDt.Location = new System.Drawing.Point(118, 71);
            this.lblRefDt.Name = "lblRefDt";
            this.lblRefDt.Size = new System.Drawing.Size(4, 14);
            this.lblRefDt.TabIndex = 1;
            // 
            // lblRefPeriod
            // 
            this.lblRefPeriod.AutoSize = true;
            this.lblRefPeriod.Location = new System.Drawing.Point(9, 71);
            this.lblRefPeriod.Name = "lblRefPeriod";
            this.lblRefPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblRefPeriod.TabIndex = 1;
            this.lblRefPeriod.Text = "Reference Period";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CmbRsltHead);
            this.panel1.Controls.Add(this.txtDesc);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.lblRsltHead);
            this.panel1.Controls.Add(this.lblGrp);
            this.panel1.Location = new System.Drawing.Point(0, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(618, 67);
            this.panel1.TabIndex = 2;
            // 
            // CmbRsltHead
            // 
            this.CmbRsltHead.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbRsltHead.FormattingEnabled = true;
            this.CmbRsltHead.Location = new System.Drawing.Point(84, 39);
            this.CmbRsltHead.Name = "CmbRsltHead";
            this.CmbRsltHead.Size = new System.Drawing.Size(156, 22);
            this.CmbRsltHead.TabIndex = 2;
            this.CmbRsltHead.SelectedIndexChanged += new System.EventHandler(this.CmbRsltHead_SelectedIndexChanged);
            // 
            // txtDesc
            // 
            this.txtDesc.Enabled = false;
            this.txtDesc.Location = new System.Drawing.Point(143, 9);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(469, 22);
            this.txtDesc.TabIndex = 1;
            // 
            // txtCode
            // 
            this.txtCode.Enabled = false;
            this.txtCode.Location = new System.Drawing.Point(56, 9);
            this.txtCode.MaxLength = 10;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(82, 22);
            this.txtCode.TabIndex = 1;
            // 
            // lblRsltHead
            // 
            this.lblRsltHead.AutoSize = true;
            this.lblRsltHead.Location = new System.Drawing.Point(9, 42);
            this.lblRsltHead.Name = "lblRsltHead";
            this.lblRsltHead.Size = new System.Drawing.Size(73, 14);
            this.lblRsltHead.TabIndex = 0;
            this.lblRsltHead.Text = "Result Header";
            // 
            // lblGrp
            // 
            this.lblGrp.AutoSize = true;
            this.lblGrp.Location = new System.Drawing.Point(9, 12);
            this.lblGrp.Name = "lblGrp";
            this.lblGrp.Size = new System.Drawing.Size(34, 14);
            this.lblGrp.TabIndex = 0;
            this.lblGrp.Text = "Group";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GvResults);
            this.panel2.Location = new System.Drawing.Point(0, 160);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(618, 252);
            this.panel2.TabIndex = 3;
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
            this.GvResults.ColumnHeadersHeight = 25;
            this.GvResults.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvResults.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Grid_Img,
            this.Results,
            this.Agy_Code,
            this.Img_Cd});
            this.GvResults.Location = new System.Drawing.Point(0, 0);
            this.GvResults.MultiSelect = false;
            this.GvResults.Name = "GvResults";
            this.GvResults.RowHeadersWidth = 25;
            this.GvResults.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.GvResults.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.GvResults.Size = new System.Drawing.Size(618, 252);
            this.GvResults.TabIndex = 0;
            this.GvResults.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.GvResults_CellClick);
            // 
            // Grid_Img
            // 
            this.Grid_Img.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.Grid_Img.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.BestFit;
            this.Grid_Img.HeaderText = " ";
            this.Grid_Img.Name = "Grid_Img";
            this.Grid_Img.Width = 30;
            // 
            // Results
            // 
            this.Results.HeaderText = "Results Description";
            this.Results.Name = "Results";
            this.Results.ReadOnly = true;
            this.Results.Width = 560;
            // 
            // Agy_Code
            // 
            this.Agy_Code.Name = "Agy_Code";
            this.Agy_Code.Visible = false;
            this.Agy_Code.Width = 30;
            // 
            // Img_Cd
            // 
            this.Img_Cd.Name = "Img_Cd";
            this.Img_Cd.Visible = false;
            this.Img_Cd.Width = 30;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(544, 413);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(469, 413);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "S&ave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ResultAssociations
            // 
            this.ClientSize = new System.Drawing.Size(619, 402);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblRefPeriod);
            this.Controls.Add(this.lblRefDt);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultAssociations";
            this.Text = "Result Association";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label LblHeader;
        private PictureBox pictureBox1;
        private Panel pnlHeader;
        private Label lblRefDt;
        private Label lblRefPeriod;
        private Panel panel1;
        private ComboBox CmbRsltHead;
        private TextBox txtDesc;
        private TextBox txtCode;
        private Label lblRsltHead;
        private Label lblGrp;
        private Panel panel2;
        private DataGridView GvResults;
        private Button btnCancel;
        private Button btnSave;
        private DataGridViewImageColumn Grid_Img;
        private DataGridViewTextBoxColumn Results;
        private DataGridViewTextBoxColumn Agy_Code;
        private DataGridViewTextBoxColumn Img_Cd;
        private PictureBox pictureBox3;


    }
}