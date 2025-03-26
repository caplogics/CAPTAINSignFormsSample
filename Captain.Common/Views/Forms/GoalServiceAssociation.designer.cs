using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class GoalServiceAssociation
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
            this.pictureBox2 = new Wisej.Web.PictureBox();
            this.LblHeader = new Wisej.Web.Label();
            this.pictureBox1 = new Wisej.Web.PictureBox();
            this.pnlHeader = new Wisej.Web.Panel();
            this.lblRefPeriod = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.CmbTbl = new Wisej.Web.ComboBox();
            this.CmbGrp = new Wisej.Web.ComboBox();
            this.lblTbl = new Wisej.Web.Label();
            this.lblGrp = new Wisej.Web.Label();
            this.GvGoals = new Wisej.Web.DataGridView();
            this.Check = new Wisej.Web.DataGridViewImageColumn();
            this.Goals = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Agy_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Img_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.lblRefDt = new Wisej.Web.Label();
            this.btnSave = new Wisej.Web.Button();
            this.btnClose = new Wisej.Web.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvGoals)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox2.Cursor = Wisej.Web.Cursors.Hand;
            this.pictureBox2.ImageSource = "icon-help?color=captainBrown";
            this.pictureBox2.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(655, 16);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(19, 22);
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // LblHeader
            // 
            this.LblHeader.AutoSize = true;
            this.LblHeader.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblHeader.ForeColor = System.Drawing.Color.White;
            this.LblHeader.Location = new System.Drawing.Point(61, 16);
            this.LblHeader.Name = "LblHeader";
            this.LblHeader.Size = new System.Drawing.Size(230, 26);
            this.LblHeader.TabIndex = 1;
            this.LblHeader.Text = "Goal/Service Association";
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
            this.pnlHeader.Controls.Add(this.pictureBox2);
            this.pnlHeader.Controls.Add(this.LblHeader);
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(699, 55);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblRefPeriod
            // 
            this.lblRefPeriod.AutoSize = true;
            this.lblRefPeriod.Location = new System.Drawing.Point(13, 70);
            this.lblRefPeriod.Name = "lblRefPeriod";
            this.lblRefPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblRefPeriod.TabIndex = 1;
            this.lblRefPeriod.Text = "Reference Period";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CmbTbl);
            this.panel1.Controls.Add(this.CmbGrp);
            this.panel1.Controls.Add(this.lblTbl);
            this.panel1.Controls.Add(this.lblGrp);
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(698, 63);
            this.panel1.TabIndex = 2;
            // 
            // CmbTbl
            // 
            this.CmbTbl.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbTbl.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CmbTbl.FormattingEnabled = true;
            this.CmbTbl.Location = new System.Drawing.Point(52, 35);
            this.CmbTbl.Name = "CmbTbl";
            this.CmbTbl.Size = new System.Drawing.Size(598, 22);
            this.CmbTbl.TabIndex = 2;
            this.CmbTbl.SelectedIndexChanged += new System.EventHandler(this.CmbTbl_SelectedIndexChanged);
            // 
            // CmbGrp
            // 
            this.CmbGrp.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbGrp.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CmbGrp.FormattingEnabled = true;
            this.CmbGrp.Location = new System.Drawing.Point(52, 9);
            this.CmbGrp.Name = "CmbGrp";
            this.CmbGrp.Size = new System.Drawing.Size(598, 22);
            this.CmbGrp.TabIndex = 2;
            this.CmbGrp.SelectedIndexChanged += new System.EventHandler(this.CmbGrp_SelectedIndexChanged);
            // 
            // lblTbl
            // 
            this.lblTbl.AutoSize = true;
            this.lblTbl.Location = new System.Drawing.Point(13, 38);
            this.lblTbl.Name = "lblTbl";
            this.lblTbl.Size = new System.Drawing.Size(31, 14);
            this.lblTbl.TabIndex = 1;
            this.lblTbl.Text = "Table";
            // 
            // lblGrp
            // 
            this.lblGrp.AutoSize = true;
            this.lblGrp.Location = new System.Drawing.Point(13, 10);
            this.lblGrp.Name = "lblGrp";
            this.lblGrp.Size = new System.Drawing.Size(34, 14);
            this.lblGrp.TabIndex = 1;
            this.lblGrp.Text = "Group";
            // 
            // GvGoals
            // 
            this.GvGoals.AllowUserToResizeColumns = false;
            this.GvGoals.AllowUserToResizeRows = false;
            this.GvGoals.BackColor = System.Drawing.Color.White;
            this.GvGoals.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.GvGoals.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GvGoals.ColumnHeadersHeight = 25;
            this.GvGoals.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvGoals.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Check,
            this.Goals,
            this.Agy_Code,
            this.Img_Code});
            this.GvGoals.Location = new System.Drawing.Point(-1, 0);
            this.GvGoals.MultiSelect = false;
            this.GvGoals.Name = "GvGoals";
            this.GvGoals.RowHeadersWidth = 25;
            this.GvGoals.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.GvGoals.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.GvGoals.Size = new System.Drawing.Size(699, 276);
            this.GvGoals.TabIndex = 3;
            this.GvGoals.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.GvGoals_CellClick);
            // 
            // Check
            // 
            this.Check.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            this.Check.CellImageLayout = Wisej.Web.DataGridViewImageCellLayout.BestFit;
            this.Check.HeaderText = " ";
            this.Check.Name = "Check";
            this.Check.Width = 30;
            // 
            // Goals
            // 
            this.Goals.HeaderText = "Outcomes";
            this.Goals.Name = "Goals";
            this.Goals.ReadOnly = true;
            this.Goals.Width = 640;
            // 
            // Agy_Code
            // 
            this.Agy_Code.Name = "Agy_Code";
            this.Agy_Code.Visible = false;
            this.Agy_Code.Width = 30;
            // 
            // Img_Code
            // 
            this.Img_Code.Name = "Img_Code";
            this.Img_Code.Visible = false;
            this.Img_Code.Width = 30;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GvGoals);
            this.panel2.Location = new System.Drawing.Point(0, 155);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 276);
            this.panel2.TabIndex = 4;
            // 
            // lblRefDt
            // 
            this.lblRefDt.AutoSize = true;
            this.lblRefDt.Location = new System.Drawing.Point(122, 70);
            this.lblRefDt.Name = "lblRefDt";
            this.lblRefDt.Size = new System.Drawing.Size(4, 14);
            this.lblRefDt.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(542, 436);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "S&ave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(617, 436);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // GoalServiceAssociation
            // 
            this.ClientSize = new System.Drawing.Size(699, 425);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblRefDt);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblRefPeriod);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GoalServiceAssociation";
            this.Text = "Goal/Service Association";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvGoals)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureBox2;
        private Label LblHeader;
        private PictureBox pictureBox1;
        private Panel pnlHeader;
        private Label lblRefPeriod;
        private Panel panel1;
        private ComboBox CmbTbl;
        private ComboBox CmbGrp;
        private Label lblTbl;
        private Label lblGrp;
        private DataGridView GvGoals;
        private Panel panel2;
        private Label lblRefDt;
        private Button btnSave;
        private Button btnClose;
        private DataGridViewImageColumn Check;
        private DataGridViewTextBoxColumn Goals;
        private DataGridViewTextBoxColumn Agy_Code;
        private DataGridViewTextBoxColumn Img_Code;


    }
}