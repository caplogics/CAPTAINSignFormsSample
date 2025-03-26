using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASB2012_AdhocPageSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASB2012_AdhocPageSetup));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.lblFunSour3Req = new Wisej.Web.Label();
            this.Txt_Report_Name = new Wisej.Web.TextBox();
            this.label4 = new Wisej.Web.Label();
            this.Cb_Save_Criteria = new Wisej.Web.CheckBox();
            this.Txt_Header_Title = new Wisej.Web.TextBox();
            this.label9 = new Wisej.Web.Label();
            this.groupBox3 = new Wisej.Web.GroupBox();
            this.panel3 = new Wisej.Web.Panel();
            this.Lbl_Rep_Type = new Wisej.Web.Label();
            this.Lbl_SelCol_Width = new Wisej.Web.Label();
            this.Lbl_SelCol_Cnt = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.Rb_Custom = new Wisej.Web.RadioButton();
            this.label7 = new Wisej.Web.Label();
            this.Rb_A4_Port = new Wisej.Web.RadioButton();
            this.Rb_A4_Land = new Wisej.Web.RadioButton();
            this.groupBox2 = new Wisej.Web.GroupBox();
            this.Cb_Inc_PageCount = new Wisej.Web.CheckBox();
            this.Cb_Footer = new Wisej.Web.CheckBox();
            this.groupBox1 = new Wisej.Web.GroupBox();
            this.Cb_Inc_Image = new Wisej.Web.CheckBox();
            this.Cb_Inc_Title = new Wisej.Web.CheckBox();
            this.Cb_Header = new Wisej.Web.CheckBox();
            this.panel4 = new Wisej.Web.Panel();
            this.Btn_Ok = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Btn_Cancel = new Wisej.Web.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(616, 354);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblFunSour3Req);
            this.panel2.Controls.Add(this.Txt_Report_Name);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.Cb_Save_Criteria);
            this.panel2.Controls.Add(this.Txt_Header_Title);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(616, 319);
            this.panel2.TabIndex = 1;
            // 
            // lblFunSour3Req
            // 
            this.lblFunSour3Req.ForeColor = System.Drawing.Color.Red;
            this.lblFunSour3Req.Location = new System.Drawing.Point(57, 9);
            this.lblFunSour3Req.Name = "lblFunSour3Req";
            this.lblFunSour3Req.Size = new System.Drawing.Size(9, 13);
            this.lblFunSour3Req.TabIndex = 0;
            this.lblFunSour3Req.Text = "*";
            // 
            // Txt_Report_Name
            // 
            this.Txt_Report_Name.Location = new System.Drawing.Point(81, 8);
            this.Txt_Report_Name.MaxLength = 50;
            this.Txt_Report_Name.Name = "Txt_Report_Name";
            this.Txt_Report_Name.Size = new System.Drawing.Size(502, 25);
            this.Txt_Report_Name.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(14, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "Save as";
            // 
            // Cb_Save_Criteria
            // 
            this.Cb_Save_Criteria.AutoSize = false;
            this.Cb_Save_Criteria.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Cb_Save_Criteria.Location = new System.Drawing.Point(388, 289);
            this.Cb_Save_Criteria.Name = "Cb_Save_Criteria";
            this.Cb_Save_Criteria.Size = new System.Drawing.Size(194, 20);
            this.Cb_Save_Criteria.TabIndex = 4;
            this.Cb_Save_Criteria.Text = "Save Current Adhoc Criteria ";
            this.Cb_Save_Criteria.Visible = false;
            // 
            // Txt_Header_Title
            // 
            this.Txt_Header_Title.Location = new System.Drawing.Point(85, 131);
            this.Txt_Header_Title.MaxLength = 50;
            this.Txt_Header_Title.Name = "Txt_Header_Title";
            this.Txt_Header_Title.Size = new System.Drawing.Size(498, 25);
            this.Txt_Header_Title.TabIndex = 4;
            this.Txt_Header_Title.LostFocus += new System.EventHandler(this.Txt_Rep_Name_LostFocus);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(10, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 14);
            this.label9.TabIndex = 0;
            this.label9.Text = "Header Title";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel3);
            this.groupBox3.Controls.Add(this.Rb_Custom);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.Rb_A4_Port);
            this.groupBox3.Controls.Add(this.Rb_A4_Land);
            this.groupBox3.Location = new System.Drawing.Point(7, 162);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(576, 115);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.Text = "Page Settings";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Lbl_Rep_Type);
            this.panel3.Controls.Add(this.Lbl_SelCol_Width);
            this.panel3.Controls.Add(this.Lbl_SelCol_Cnt);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.panel3.Location = new System.Drawing.Point(232, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(338, 82);
            this.panel3.TabIndex = 7;
            // 
            // Lbl_Rep_Type
            // 
            this.Lbl_Rep_Type.AutoSize = true;
            this.Lbl_Rep_Type.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_Rep_Type.ForeColor = System.Drawing.Color.FromArgb(16, 106, 177);
            this.Lbl_Rep_Type.Location = new System.Drawing.Point(235, 57);
            this.Lbl_Rep_Type.Name = "Lbl_Rep_Type";
            this.Lbl_Rep_Type.Size = new System.Drawing.Size(18, 14);
            this.Lbl_Rep_Type.TabIndex = 0;
            this.Lbl_Rep_Type.Text = "00";
            // 
            // Lbl_SelCol_Width
            // 
            this.Lbl_SelCol_Width.AutoSize = true;
            this.Lbl_SelCol_Width.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_SelCol_Width.ForeColor = System.Drawing.Color.FromArgb(16, 106, 177);
            this.Lbl_SelCol_Width.Location = new System.Drawing.Point(235, 35);
            this.Lbl_SelCol_Width.Name = "Lbl_SelCol_Width";
            this.Lbl_SelCol_Width.Size = new System.Drawing.Size(18, 14);
            this.Lbl_SelCol_Width.TabIndex = 0;
            this.Lbl_SelCol_Width.Text = "00";
            // 
            // Lbl_SelCol_Cnt
            // 
            this.Lbl_SelCol_Cnt.AutoSize = true;
            this.Lbl_SelCol_Cnt.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_SelCol_Cnt.ForeColor = System.Drawing.Color.FromArgb(16, 106, 177);
            this.Lbl_SelCol_Cnt.Location = new System.Drawing.Point(235, 11);
            this.Lbl_SelCol_Cnt.Name = "Lbl_SelCol_Cnt";
            this.Lbl_SelCol_Cnt.Size = new System.Drawing.Size(18, 14);
            this.Lbl_SelCol_Cnt.TabIndex = 0;
            this.Lbl_SelCol_Cnt.Text = "00";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(14, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Recommended Page Orientation ";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(14, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total Width of Selected columns";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Columns To Display in O/P";
            // 
            // Rb_Custom
            // 
            this.Rb_Custom.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Rb_Custom.Location = new System.Drawing.Point(110, 67);
            this.Rb_Custom.Name = "Rb_Custom";
            this.Rb_Custom.Size = new System.Drawing.Size(108, 21);
            this.Rb_Custom.TabIndex = 3;
            this.Rb_Custom.Text = "Custom Setup";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(7, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 19);
            this.label7.TabIndex = 0;
            this.label7.Text = "Page Orientation";
            // 
            // Rb_A4_Port
            // 
            this.Rb_A4_Port.Checked = true;
            this.Rb_A4_Port.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Rb_A4_Port.Location = new System.Drawing.Point(110, 25);
            this.Rb_A4_Port.Name = "Rb_A4_Port";
            this.Rb_A4_Port.Size = new System.Drawing.Size(99, 21);
            this.Rb_A4_Port.TabIndex = 1;
            this.Rb_A4_Port.TabStop = true;
            this.Rb_A4_Port.Text = "A4 -  Portrait";
            // 
            // Rb_A4_Land
            // 
            this.Rb_A4_Land.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Rb_A4_Land.Location = new System.Drawing.Point(110, 46);
            this.Rb_A4_Land.Name = "Rb_A4_Land";
            this.Rb_A4_Land.Size = new System.Drawing.Size(119, 21);
            this.Rb_A4_Land.TabIndex = 2;
            this.Rb_A4_Land.Text = "A4 -  Landscape";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Cb_Inc_PageCount);
            this.groupBox2.Controls.Add(this.Cb_Footer);
            this.groupBox2.Location = new System.Drawing.Point(325, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 91);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.Text = "Footer";
            // 
            // Cb_Inc_PageCount
            // 
            this.Cb_Inc_PageCount.AutoSize = false;
            this.Cb_Inc_PageCount.Checked = true;
            this.Cb_Inc_PageCount.Location = new System.Drawing.Point(62, 44);
            this.Cb_Inc_PageCount.Name = "Cb_Inc_PageCount";
            this.Cb_Inc_PageCount.Size = new System.Drawing.Size(174, 29);
            this.Cb_Inc_PageCount.TabIndex = 2;
            this.Cb_Inc_PageCount.Text = "Include Page Numbers";
            // 
            // Cb_Footer
            // 
            this.Cb_Footer.Checked = true;
            this.Cb_Footer.Location = new System.Drawing.Point(41, 19);
            this.Cb_Footer.Name = "Cb_Footer";
            this.Cb_Footer.Size = new System.Drawing.Size(109, 21);
            this.Cb_Footer.TabIndex = 1;
            this.Cb_Footer.Text = "Include Footer";
            this.Cb_Footer.CheckedChanged += new System.EventHandler(this.Cb_Footer_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Cb_Inc_Image);
            this.groupBox1.Controls.Add(this.Cb_Inc_Title);
            this.groupBox1.Controls.Add(this.Cb_Header);
            this.groupBox1.Location = new System.Drawing.Point(7, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 91);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.Text = "Header";
            // 
            // Cb_Inc_Image
            // 
            this.Cb_Inc_Image.AutoSize = false;
            this.Cb_Inc_Image.Location = new System.Drawing.Point(68, 67);
            this.Cb_Inc_Image.Name = "Cb_Inc_Image";
            this.Cb_Inc_Image.Size = new System.Drawing.Size(185, 21);
            this.Cb_Inc_Image.TabIndex = 3;
            this.Cb_Inc_Image.Text = "Include Header Image";
            // 
            // Cb_Inc_Title
            // 
            this.Cb_Inc_Title.AutoSize = false;
            this.Cb_Inc_Title.Checked = true;
            this.Cb_Inc_Title.Location = new System.Drawing.Point(68, 44);
            this.Cb_Inc_Title.Name = "Cb_Inc_Title";
            this.Cb_Inc_Title.Size = new System.Drawing.Size(175, 21);
            this.Cb_Inc_Title.TabIndex = 2;
            this.Cb_Inc_Title.Text = "Include Header Title";
            this.Cb_Inc_Title.CheckedChanged += new System.EventHandler(this.Cb_Inc_Title_CheckedChanged);
            // 
            // Cb_Header
            // 
            this.Cb_Header.Checked = true;
            this.Cb_Header.Location = new System.Drawing.Point(48, 19);
            this.Cb_Header.Name = "Cb_Header";
            this.Cb_Header.Size = new System.Drawing.Size(114, 21);
            this.Cb_Header.TabIndex = 1;
            this.Cb_Header.Text = "Include Header";
            this.Cb_Header.CheckedChanged += new System.EventHandler(this.Cb_Header_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.AppearanceKey = "panel-grdo";
            this.panel4.Controls.Add(this.Btn_Ok);
            this.panel4.Controls.Add(this.spacer1);
            this.panel4.Controls.Add(this.Btn_Cancel);
            this.panel4.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 319);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel4.Size = new System.Drawing.Size(616, 35);
            this.panel4.TabIndex = 2;
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.AppearanceKey = "button-reports";
            this.Btn_Ok.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Ok.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Btn_Ok.Location = new System.Drawing.Point(448, 5);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(71, 25);
            this.Btn_Ok.TabIndex = 1;
            this.Btn_Ok.Text = "&OK";
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(519, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.AppearanceKey = "button-reports";
            this.Btn_Cancel.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Cancel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Btn_Cancel.Location = new System.Drawing.Point(524, 5);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(77, 25);
            this.Btn_Cancel.TabIndex = 2;
            this.Btn_Cancel.Text = "&Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // CASB2012_AdhocPageSetup
            // 
            this.ClientSize = new System.Drawing.Size(616, 354);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASB2012_AdhocPageSetup";
            this.Text = "Report Page Setup";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel panel1;
        private Panel panel2;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private CheckBox Cb_Footer;
        private CheckBox Cb_Header;
        private Label label9;
        private TextBox Txt_Header_Title;
        private RadioButton Rb_Custom;
        private Label label7;
        private RadioButton Rb_A4_Port;
        private RadioButton Rb_A4_Land;
        private CheckBox Cb_Save_Criteria;
        private CheckBox Cb_Inc_Image;
        private CheckBox Cb_Inc_Title;
        private CheckBox Cb_Inc_PageCount;
        private Button Btn_Cancel;
        private Button Btn_Ok;
        private Panel panel3;
        private Label Lbl_Rep_Type;
        private Label Lbl_SelCol_Width;
        private Label Lbl_SelCol_Cnt;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox Txt_Report_Name;
        private Label label4;
        private Label lblFunSour3Req;
        private Panel panel4;
        private Spacer spacer1;
    }
}