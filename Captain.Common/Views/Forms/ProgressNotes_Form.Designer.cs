using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class ProgressNotes_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressNotes_Form));
            this.panel5 = new Wisej.Web.Panel();
            this.Btn_Cancel = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Btn_Save = new Wisej.Web.Button();
            this.spacer6 = new Wisej.Web.Spacer();
            this.panel8 = new Wisej.Web.Panel();
            this.Btn_Close = new Wisej.Web.Button();
            this.btnAdd = new Wisej.Web.Button();
            this.panel4 = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.Txt_ProgEdit = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.panel6 = new Wisej.Web.Panel();
            this.panel7 = new Wisej.Web.Panel();
            this.Btn_Print = new Wisej.Web.Button();
            this.label4 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.LblApp_Name = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.LblAppNo = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.label1 = new Wisej.Web.Label();
            this.panel9 = new Wisej.Web.Panel();
            this.chkSelectAll = new Wisej.Web.CheckBox();
            this.spacer3 = new Wisej.Web.Spacer();
            this.pnlMain = new Wisej.Web.Panel();
            this.panel10 = new Wisej.Web.Panel();
            this.gvCA = new Wisej.Web.DataGridView();
            this.CA_Sel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.CA_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CA_Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Notes_key = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CAMS_Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Txt_ProgText = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.panel11 = new Wisej.Web.Panel();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCA)).BeginInit();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.AppearanceKey = "panel-grdo";
            this.panel5.BackColor = System.Drawing.Color.FromName("@control");
            this.panel5.Controls.Add(this.Btn_Cancel);
            this.panel5.Controls.Add(this.spacer1);
            this.panel5.Controls.Add(this.Btn_Save);
            this.panel5.Controls.Add(this.spacer6);
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.btnAdd);
            this.panel5.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 164);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new Wisej.Web.Padding(5);
            this.panel5.Size = new System.Drawing.Size(896, 35);
            this.panel5.TabIndex = 2;
            this.panel5.TabStop = true;
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.AppearanceKey = "button-cancel";
            this.Btn_Cancel.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Cancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Cancel.Location = new System.Drawing.Point(150, 5);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(75, 25);
            this.Btn_Cancel.TabIndex = 2;
            this.Btn_Cancel.Text = "Ca&ncel";
            this.Btn_Cancel.Visible = false;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(145, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // Btn_Save
            // 
            this.Btn_Save.AppearanceKey = "button-ok";
            this.Btn_Save.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Save.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Save.Location = new System.Drawing.Point(70, 5);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(75, 25);
            this.Btn_Save.TabIndex = 1;
            this.Btn_Save.Text = "&Save";
            this.Btn_Save.Visible = false;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Left;
            this.spacer6.Location = new System.Drawing.Point(65, 5);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(5, 25);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Transparent;
            this.panel8.Controls.Add(this.Btn_Close);
            this.panel8.Dock = Wisej.Web.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(759, 5);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(132, 25);
            this.panel8.TabIndex = 4;
            this.panel8.TabStop = true;
            // 
            // Btn_Close
            // 
            this.Btn_Close.AppearanceKey = "button-cancel";
            this.Btn_Close.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Close.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Close.Location = new System.Drawing.Point(57, 0);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(75, 25);
            this.Btn_Close.TabIndex = 3;
            this.Btn_Close.Text = "&Close";
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = Wisej.Web.DockStyle.Left;
            this.btnAdd.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnAdd.Location = new System.Drawing.Point(5, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 25);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 255);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(896, 199);
            this.panel4.TabIndex = 1;
            this.panel4.TabStop = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Txt_ProgEdit);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(896, 129);
            this.panel1.TabIndex = 7;
            this.panel1.TabStop = true;
            // 
            // Txt_ProgEdit
            // 
            this.Txt_ProgEdit.BackColor = System.Drawing.Color.White;
            this.Txt_ProgEdit.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Txt_ProgEdit.Dock = Wisej.Web.DockStyle.Fill;
            this.Txt_ProgEdit.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Txt_ProgEdit.Multiline = true;
            this.Txt_ProgEdit.Name = "Txt_ProgEdit";
            this.Txt_ProgEdit.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Txt_ProgEdit.Size = new System.Drawing.Size(896, 129);
            this.Txt_ProgEdit.SpellCheck = true;
            this.Txt_ProgEdit.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(245, 224, 235, 241);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = Wisej.Web.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(896, 31);
            this.panel6.TabIndex = 6;
            this.panel6.TabStop = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.Btn_Print);
            this.panel7.Dock = Wisej.Web.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(710, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(186, 31);
            this.panel7.TabIndex = 1;
            this.panel7.TabStop = true;
            // 
            // Btn_Print
            // 
            this.Btn_Print.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Print.Location = new System.Drawing.Point(40, 2);
            this.Btn_Print.Name = "Btn_Print";
            this.Btn_Print.Size = new System.Drawing.Size(85, 26);
            this.Btn_Print.TabIndex = 5;
            this.Btn_Print.Text = "&Print/View";
            this.Btn_Print.Click += new System.EventHandler(this.Btn_Print_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(10, 6);
            this.label4.MaximumSize = new System.Drawing.Size(0, 18);
            this.label4.MinimumSize = new System.Drawing.Size(0, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Enter New Progress Notes Here";
            // 
            // label3
            // 
            this.label3.Dock = Wisej.Web.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(259, 33);
            this.label3.TabIndex = 0;
            this.label3.Text = "Older Posts";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblApp_Name
            // 
            this.LblApp_Name.AutoSize = true;
            this.LblApp_Name.Dock = Wisej.Web.DockStyle.Left;
            this.LblApp_Name.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.LblApp_Name.Location = new System.Drawing.Point(710, 0);
            this.LblApp_Name.Name = "LblApp_Name";
            this.LblApp_Name.Size = new System.Drawing.Size(60, 27);
            this.LblApp_Name.TabIndex = 0;
            this.LblApp_Name.Text = "00000000";
            this.LblApp_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = Wisej.Web.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(609, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Client Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblAppNo
            // 
            this.LblAppNo.AutoSize = true;
            this.LblAppNo.CausesValidation = false;
            this.LblAppNo.Dock = Wisej.Web.DockStyle.Left;
            this.LblAppNo.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.LblAppNo.Location = new System.Drawing.Point(52, 0);
            this.LblAppNo.Name = "LblAppNo";
            this.LblAppNo.Size = new System.Drawing.Size(60, 27);
            this.LblAppNo.TabIndex = 0;
            this.LblAppNo.Text = "00000000";
            this.LblAppNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(224, 235, 241);
            this.panel2.Controls.Add(this.LblApp_Name);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.spacer2);
            this.panel2.Controls.Add(this.LblAppNo);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(896, 27);
            this.panel2.TabIndex = 0;
            this.panel2.TabStop = true;
            this.panel2.Click += new System.EventHandler(this.panel2_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(112, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(497, 27);
            // 
            // label1
            // 
            this.label1.Dock = Wisej.Web.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(52, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "App#";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(251, 244, 244, 244);
            this.panel9.Controls.Add(this.chkSelectAll);
            this.panel9.Controls.Add(this.spacer3);
            this.panel9.Controls.Add(this.label3);
            this.panel9.Dock = Wisej.Web.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(896, 33);
            this.panel9.TabIndex = 7;
            this.panel9.TabStop = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Dock = Wisej.Web.DockStyle.Left;
            this.chkSelectAll.Location = new System.Drawing.Point(335, 0);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(83, 33);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.Visible = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(259, 0);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(76, 33);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Controls.Add(this.panel10);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(896, 454);
            this.pnlMain.TabIndex = 1;
            this.pnlMain.TabStop = true;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.gvCA);
            this.panel10.Controls.Add(this.Txt_ProgText);
            this.panel10.Controls.Add(this.panel9);
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = Wisej.Web.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 27);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(896, 427);
            this.panel10.TabIndex = 0;
            this.panel10.TabStop = true;
            // 
            // gvCA
            // 
            this.gvCA.AllowUserToResizeColumns = false;
            this.gvCA.AllowUserToResizeRows = false;
            this.gvCA.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvCA.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvCA.BorderStyle = Wisej.Web.BorderStyle.None;
            this.gvCA.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.CA_Sel,
            this.CA_Code,
            this.CA_Desc,
            this.Notes_key,
            this.CAMS_Type});
            this.gvCA.Dock = Wisej.Web.DockStyle.Top;
            this.gvCA.Location = new System.Drawing.Point(0, 131);
            this.gvCA.Name = "gvCA";
            this.gvCA.RowHeadersWidth = 25;
            this.gvCA.ShowColumnVisibilityMenu = false;
            this.gvCA.Size = new System.Drawing.Size(896, 98);
            this.gvCA.TabIndex = 8;
            this.gvCA.Visible = false;
            // 
            // CA_Sel
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.CA_Sel.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.CA_Sel.HeaderStyle = dataGridViewCellStyle2;
            this.CA_Sel.HeaderText = " ";
            this.CA_Sel.Name = "CA_Sel";
            this.CA_Sel.ShowInVisibilityMenu = false;
            this.CA_Sel.Width = 40;
            // 
            // CA_Code
            // 
            this.CA_Code.HeaderText = "CA Code";
            this.CA_Code.Name = "CA_Code";
            this.CA_Code.ReadOnly = true;
            this.CA_Code.ShowInVisibilityMenu = false;
            this.CA_Code.Visible = false;
            this.CA_Code.Width = 80;
            // 
            // CA_Desc
            // 
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.CA_Desc.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.CA_Desc.HeaderStyle = dataGridViewCellStyle4;
            this.CA_Desc.HeaderText = "Description";
            this.CA_Desc.Name = "CA_Desc";
            this.CA_Desc.ReadOnly = true;
            this.CA_Desc.Width = 520;
            // 
            // Notes_key
            // 
            this.Notes_key.HeaderText = "Notes_key";
            this.Notes_key.Name = "Notes_key";
            this.Notes_key.ShowInVisibilityMenu = false;
            this.Notes_key.Visible = false;
            // 
            // CAMS_Type
            // 
            this.CAMS_Type.HeaderText = "CAMS_Type";
            this.CAMS_Type.Name = "CAMS_Type";
            this.CAMS_Type.ShowInVisibilityMenu = false;
            this.CAMS_Type.Visible = false;
            // 
            // Txt_ProgText
            // 
            this.Txt_ProgText.BackColor = System.Drawing.Color.White;
            this.Txt_ProgText.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Txt_ProgText.Dock = Wisej.Web.DockStyle.Top;
            this.Txt_ProgText.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Txt_ProgText.Location = new System.Drawing.Point(0, 33);
            this.Txt_ProgText.Multiline = true;
            this.Txt_ProgText.Name = "Txt_ProgText";
            this.Txt_ProgText.ReadOnly = true;
            this.Txt_ProgText.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Txt_ProgText.Size = new System.Drawing.Size(896, 98);
            this.Txt_ProgText.TabIndex = 6;
            this.Txt_ProgText.TextChanged += new System.EventHandler(this.Txt_ProgText_TextChanged);
            // 
            // panel11
            // 
            this.panel11.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(0, 233);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(896, 194);
            this.panel11.TabIndex = 0;
            this.panel11.TabStop = true;
            // 
            // ProgressNotes_Form
            // 
            this.ClientSize = new System.Drawing.Size(896, 454);
            this.Controls.Add(this.pnlMain);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "ProgressNotes_Form";
            this.Text = "ProgressNotes_Form";
            this.Load += new System.EventHandler(this.ProgressNotes_Form_Load);
            this.FormClosing += new Wisej.Web.FormClosingEventHandler(this.ProgressNotes_Form_FormClosing);
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label LblApp_Name;
        private Label label2;
        private Label LblAppNo;
        private Panel panel2;
        private Panel panel4;
        private Panel panel5;
        private Button Btn_Close;
        private Button Btn_Cancel;
        private Button Btn_Save;
        private TextBoxWithValidation Txt_ProgEdit;
        private TextBoxWithValidation Txt_ProgText;
        private Label label3;
        private Label label4;
        private Button Btn_Print;
        private Button btnAdd;
        private Panel panel6;
        private Panel panel7;
        private Panel panel8;
        private Panel panel9;
        private Panel pnlMain;
        private Panel panel10;
        private Panel panel11;
        private Panel panel1;
        private DataGridView gvCA;
        private DataGridViewCheckBoxColumn CA_Sel;
        private DataGridViewTextBoxColumn CA_Code;
        private DataGridViewTextBoxColumn CA_Desc;
        private DataGridViewTextBoxColumn Notes_key;
        private DataGridViewTextBoxColumn CAMS_Type;
        private CheckBox chkSelectAll;
        private Spacer spacer1;
        private Spacer spacer6;
        private Spacer spacer2;
        private Spacer spacer3;
        private Label label1;
    }
}