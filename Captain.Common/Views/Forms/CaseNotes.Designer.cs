using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class CaseNotes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseNotes));
            this.panel1 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.txtDesc = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.panel2 = new Wisej.Web.Panel();
            this.pictureBox2 = new Wisej.Web.PictureBox();
            this.lblYear = new Wisej.Web.Label();
            this.cmbYear = new Wisej.Web.ComboBox();
            this.lblApplicationNo = new Wisej.Web.Label();
            this.lblClientNameD = new Wisej.Web.Label();
            this.lblApplicationNon = new Wisej.Web.Label();
            this.lblClientName = new Wisej.Web.Label();
            this.panel4 = new Wisej.Web.Panel();
            this.btnPrint = new Wisej.Web.Button();
            this.spacer6 = new Wisej.Web.Spacer();
            this.btnDelete = new Wisej.Web.Button();
            this.spacer5 = new Wisej.Web.Spacer();
            this.btnOk = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.btnClose = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.label1 = new Wisej.Web.Label();
            this.cmbsize = new Wisej.Web.ComboBox();
            this.btnChange = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnPreview = new Wisej.Web.Button();
            this.spacer4 = new Wisej.Web.Spacer();
            this.btnAdd = new Wisej.Web.Button();
            this.panel5 = new Wisej.Web.Panel();
            this.panel6 = new Wisej.Web.Panel();
            this.pnlMiddle = new Wisej.Web.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(896, 490);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel3.Controls.Add(this.txtDesc);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 51);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(896, 404);
            this.panel3.TabIndex = 5;
            this.panel3.TabStop = true;
            // 
            // txtDesc
            // 
            this.txtDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtDesc.Dock = Wisej.Web.DockStyle.Fill;
            this.txtDesc.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDesc.MaxLength = 150000;
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.txtDesc.Size = new System.Drawing.Size(894, 402);
            this.txtDesc.SpellCheck = true;
            this.txtDesc.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(253, 244, 244, 244);
            this.panel2.Controls.Add(this.pnlMiddle);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(896, 51);
            this.panel2.TabIndex = 4;
            this.panel2.TabStop = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = Wisej.Web.Cursors.Hand;
            this.pictureBox2.ImageSource = "captain-casenotes";
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(41, 40);
            this.pictureBox2.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(14, 16);
            this.lblYear.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblYear.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(30, 18);
            this.lblYear.TabIndex = 5;
            this.lblYear.Text = "Year";
            this.lblYear.Visible = false;
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(54, 13);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(100, 25);
            this.cmbYear.TabIndex = 4;
            this.cmbYear.Visible = false;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            // 
            // lblApplicationNo
            // 
            this.lblApplicationNo.AutoSize = true;
            this.lblApplicationNo.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApplicationNo.Location = new System.Drawing.Point(90, 16);
            this.lblApplicationNo.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblApplicationNo.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblApplicationNo.Name = "lblApplicationNo";
            this.lblApplicationNo.Size = new System.Drawing.Size(36, 18);
            this.lblApplicationNo.TabIndex = 0;
            this.lblApplicationNo.Text = "App#:";
            // 
            // lblClientNameD
            // 
            this.lblClientNameD.AutoSize = true;
            this.lblClientNameD.Font = new System.Drawing.Font("default", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblClientNameD.Location = new System.Drawing.Point(374, 16);
            this.lblClientNameD.Name = "lblClientNameD";
            this.lblClientNameD.Size = new System.Drawing.Size(4, 14);
            this.lblClientNameD.TabIndex = 3;
            // 
            // lblApplicationNon
            // 
            this.lblApplicationNon.AutoSize = true;
            this.lblApplicationNon.Font = new System.Drawing.Font("default", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblApplicationNon.Location = new System.Drawing.Point(130, 16);
            this.lblApplicationNon.Name = "lblApplicationNon";
            this.lblApplicationNon.Size = new System.Drawing.Size(4, 14);
            this.lblApplicationNon.TabIndex = 1;
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClientName.Location = new System.Drawing.Point(322, 16);
            this.lblClientName.MaximumSize = new System.Drawing.Size(0, 18);
            this.lblClientName.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(43, 18);
            this.lblClientName.TabIndex = 2;
            this.lblClientName.Text = "Name :";
            // 
            // panel4
            // 
            this.panel4.AppearanceKey = "panel-grdo";
            this.panel4.Controls.Add(this.btnPrint);
            this.panel4.Controls.Add(this.spacer6);
            this.panel4.Controls.Add(this.btnDelete);
            this.panel4.Controls.Add(this.spacer5);
            this.panel4.Controls.Add(this.btnOk);
            this.panel4.Controls.Add(this.spacer3);
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.spacer2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.cmbsize);
            this.panel4.Controls.Add(this.btnChange);
            this.panel4.Controls.Add(this.spacer1);
            this.panel4.Controls.Add(this.btnPreview);
            this.panel4.Controls.Add(this.spacer4);
            this.panel4.Controls.Add(this.btnAdd);
            this.panel4.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 455);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.panel4.Size = new System.Drawing.Size(896, 35);
            this.panel4.TabIndex = 6;
            this.panel4.TabStop = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = Wisej.Web.DockStyle.Left;
            this.btnPrint.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPrint.Location = new System.Drawing.Point(234, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(60, 25);
            this.btnPrint.TabIndex = 18;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Left;
            this.spacer6.Location = new System.Drawing.Point(231, 5);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(3, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.AppearanceKey = "button-error";
            this.btnDelete.Dock = Wisej.Web.DockStyle.Left;
            this.btnDelete.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(156, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(153, 5);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(3, 25);
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.BackColor = System.Drawing.Color.FromName("@captainBlue");
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.ForeColor = System.Drawing.Color.FromName("@window");
            this.btnOk.Location = new System.Drawing.Point(483, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "&Save";
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(558, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-cancel";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(561, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "&Cancel";
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(636, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // label1
            // 
            this.label1.Dock = Wisej.Web.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(651, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 25);
            this.label1.TabIndex = 22;
            this.label1.Text = "Size";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbsize
            // 
            this.cmbsize.Dock = Wisej.Web.DockStyle.Right;
            this.cmbsize.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbsize.FormattingEnabled = true;
            this.cmbsize.Location = new System.Drawing.Point(679, 5);
            this.cmbsize.Name = "cmbsize";
            this.cmbsize.Size = new System.Drawing.Size(97, 25);
            this.cmbsize.TabIndex = 14;
            // 
            // btnChange
            // 
            this.btnChange.AppearanceKey = "button-alert";
            this.btnChange.Dock = Wisej.Web.DockStyle.Left;
            this.btnChange.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnChange.Location = new System.Drawing.Point(78, 5);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 25);
            this.btnChange.TabIndex = 16;
            this.btnChange.Text = "&Edit";
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(776, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // btnPreview
            // 
            this.btnPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPreview.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPreview.Location = new System.Drawing.Point(781, 5);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(100, 25);
            this.btnPreview.TabIndex = 19;
            this.btnPreview.Text = "Print/&View";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(75, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = Wisej.Web.DockStyle.Left;
            this.btnAdd.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnAdd.Location = new System.Drawing.Point(15, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 25);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.pictureBox2);
            this.panel5.Dock = Wisej.Web.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(48, 51);
            this.panel5.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.lblYear);
            this.panel6.Controls.Add(this.cmbYear);
            this.panel6.Dock = Wisej.Web.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(726, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(170, 51);
            this.panel6.TabIndex = 2;
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.Controls.Add(this.lblClientName);
            this.pnlMiddle.Controls.Add(this.lblApplicationNon);
            this.pnlMiddle.Controls.Add(this.lblApplicationNo);
            this.pnlMiddle.Controls.Add(this.lblClientNameD);
            this.pnlMiddle.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(48, 0);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(678, 51);
            this.pnlMiddle.TabIndex = 2;
            // 
            // CaseNotes
            // 
            this.ClientSize = new System.Drawing.Size(896, 490);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaseNotes";
            this.Text = "Case Notes";
            this.FormClosing += new Wisej.Web.FormClosingEventHandler(this.CaseNotes_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.pnlMiddle.ResumeLayout(false);
            this.pnlMiddle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private Label lblApplicationNo;
        private Label lblClientNameD;
        private Label lblApplicationNon;
        private Label lblClientName;
        private TextBoxWithValidation txtDesc;
        private Label lblYear;
        private ComboBox cmbYear;
        private PictureBox pictureBox2;
        private Panel panel4;
        private Label label1;
        private ComboBox cmbsize;
        private Button btnAdd;
        private Button btnOk;
        private Button btnClose;
        private Button btnPrint;
        private Button btnPreview;
        private Button btnDelete;
        private Button btnChange;
        private Spacer spacer6;
        private Spacer spacer5;
        private Spacer spacer3;
        private Spacer spacer2;
        private Spacer spacer1;
        private Spacer spacer4;
        private Panel pnlMiddle;
        private Panel panel5;
        private Panel panel6;
    }
}