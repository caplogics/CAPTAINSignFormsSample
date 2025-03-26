using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSSB2110
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB2110));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlReport = new Wisej.Web.Panel();
            this.lblActive = new Wisej.Web.Label();
            this.cmbActive = new Wisej.Web.ComboBox();
            this.rbSchedule = new Wisej.Web.RadioButton();
            this.rbClass = new Wisej.Web.RadioButton();
            this.lblReport = new Wisej.Web.Label();
            this.lblTypeRep = new Wisej.Web.Label();
            this.pnlType = new Wisej.Web.Panel();
            this.rbCondensed = new Wisej.Web.RadioButton();
            this.rbFull = new Wisej.Web.RadioButton();
            this.lblSite = new Wisej.Web.Label();
            this.pnlSite = new Wisej.Web.Panel();
            this.rbSelectedSite = new Wisej.Web.RadioButton();
            this.rbAll = new Wisej.Web.RadioButton();
            this.label4 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.lblEndMonth = new Wisej.Web.Label();
            this.txtEndMnth = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblEndYear = new Wisej.Web.Label();
            this.txtEndYear = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblStartYear = new Wisej.Web.Label();
            this.txtStrtYear = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtStrtMnth = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblStrtMonth = new Wisej.Web.Label();
            this.panel5 = new Wisej.Web.Panel();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.spacer4 = new Wisej.Web.Spacer();
            this.btnGenPDF = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.btnGetParameters = new Wisej.Web.Button();
            this.button1 = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlReport.SuspendLayout();
            this.pnlType.SuspendLayout();
            this.pnlSite.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
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
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(595, 0);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(580, 25);
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
            this.pnlHie.Size = new System.Drawing.Size(660, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(580, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlReport
            // 
            this.pnlReport.Controls.Add(this.lblActive);
            this.pnlReport.Controls.Add(this.cmbActive);
            this.pnlReport.Controls.Add(this.rbSchedule);
            this.pnlReport.Controls.Add(this.rbClass);
            this.pnlReport.Controls.Add(this.lblReport);
            this.pnlReport.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReport.Location = new System.Drawing.Point(0, 0);
            this.pnlReport.Name = "pnlReport";
            this.pnlReport.Size = new System.Drawing.Size(748, 39);
            this.pnlReport.TabIndex = 1;
            // 
            // lblActive
            // 
            this.lblActive.Location = new System.Drawing.Point(332, 15);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(84, 16);
            this.lblActive.TabIndex = 0;
            this.lblActive.Text = "Active/Inactive";
            this.lblActive.Visible = false;
            // 
            // cmbActive
            // 
            this.cmbActive.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbActive.FormattingEnabled = true;
            this.cmbActive.Location = new System.Drawing.Point(430, 11);
            this.cmbActive.Name = "cmbActive";
            this.cmbActive.Size = new System.Drawing.Size(106, 25);
            this.cmbActive.TabIndex = 3;
            this.cmbActive.Visible = false;
            // 
            // rbSchedule
            // 
            this.rbSchedule.AutoSize = false;
            this.rbSchedule.Location = new System.Drawing.Point(205, 12);
            this.rbSchedule.Name = "rbSchedule";
            this.rbSchedule.Size = new System.Drawing.Size(102, 21);
            this.rbSchedule.TabIndex = 2;
            this.rbSchedule.Text = "Site Schedule";
            this.rbSchedule.CheckedChanged += new System.EventHandler(this.rbSchedule_CheckedChanged);
            // 
            // rbClass
            // 
            this.rbClass.AutoSize = false;
            this.rbClass.Checked = true;
            this.rbClass.Location = new System.Drawing.Point(115, 12);
            this.rbClass.Name = "rbClass";
            this.rbClass.Size = new System.Drawing.Size(82, 21);
            this.rbClass.TabIndex = 1;
            this.rbClass.TabStop = true;
            this.rbClass.Text = "Site Class";
            // 
            // lblReport
            // 
            this.lblReport.Location = new System.Drawing.Point(15, 15);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(41, 16);
            this.lblReport.TabIndex = 1;
            this.lblReport.Text = "Report";
            // 
            // lblTypeRep
            // 
            this.lblTypeRep.Location = new System.Drawing.Point(15, 7);
            this.lblTypeRep.Name = "lblTypeRep";
            this.lblTypeRep.Size = new System.Drawing.Size(85, 16);
            this.lblTypeRep.TabIndex = 1;
            this.lblTypeRep.Text = "Type of Report";
            // 
            // pnlType
            // 
            this.pnlType.Controls.Add(this.rbCondensed);
            this.pnlType.Controls.Add(this.rbFull);
            this.pnlType.Controls.Add(this.lblTypeRep);
            this.pnlType.Dock = Wisej.Web.DockStyle.Top;
            this.pnlType.Location = new System.Drawing.Point(0, 39);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(748, 31);
            this.pnlType.TabIndex = 2;
            // 
            // rbCondensed
            // 
            this.rbCondensed.AutoSize = false;
            this.rbCondensed.Location = new System.Drawing.Point(205, 4);
            this.rbCondensed.Name = "rbCondensed";
            this.rbCondensed.Size = new System.Drawing.Size(94, 21);
            this.rbCondensed.TabIndex = 2;
            this.rbCondensed.Text = "Condensed";
            // 
            // rbFull
            // 
            this.rbFull.AutoSize = false;
            this.rbFull.Checked = true;
            this.rbFull.Location = new System.Drawing.Point(115, 4);
            this.rbFull.Name = "rbFull";
            this.rbFull.Size = new System.Drawing.Size(49, 21);
            this.rbFull.TabIndex = 1;
            this.rbFull.TabStop = true;
            this.rbFull.Text = "Full";
            // 
            // lblSite
            // 
            this.lblSite.Location = new System.Drawing.Point(15, 7);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(22, 16);
            this.lblSite.TabIndex = 1;
            this.lblSite.Text = "Site";
            // 
            // pnlSite
            // 
            this.pnlSite.Controls.Add(this.rbSelectedSite);
            this.pnlSite.Controls.Add(this.rbAll);
            this.pnlSite.Controls.Add(this.label4);
            this.pnlSite.Controls.Add(this.label3);
            this.pnlSite.Controls.Add(this.label2);
            this.pnlSite.Controls.Add(this.label1);
            this.pnlSite.Controls.Add(this.lblEndMonth);
            this.pnlSite.Controls.Add(this.txtEndMnth);
            this.pnlSite.Controls.Add(this.lblEndYear);
            this.pnlSite.Controls.Add(this.txtEndYear);
            this.pnlSite.Controls.Add(this.lblStartYear);
            this.pnlSite.Controls.Add(this.txtStrtYear);
            this.pnlSite.Controls.Add(this.txtStrtMnth);
            this.pnlSite.Controls.Add(this.lblStrtMonth);
            this.pnlSite.Controls.Add(this.lblSite);
            this.pnlSite.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSite.Location = new System.Drawing.Point(0, 70);
            this.pnlSite.Name = "pnlSite";
            this.pnlSite.Size = new System.Drawing.Size(748, 66);
            this.pnlSite.TabIndex = 3;
            this.pnlSite.Visible = false;
            // 
            // rbSelectedSite
            // 
            this.rbSelectedSite.AutoSize = false;
            this.rbSelectedSite.Location = new System.Drawing.Point(205, 4);
            this.rbSelectedSite.Name = "rbSelectedSite";
            this.rbSelectedSite.Size = new System.Drawing.Size(100, 21);
            this.rbSelectedSite.TabIndex = 2;
            this.rbSelectedSite.Text = "Selected Site";
            this.rbSelectedSite.Click += new System.EventHandler(this.rbSelectedSite_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = false;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(115, 4);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(43, 21);
            this.rbAll.TabIndex = 1;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.Click += new System.EventHandler(this.rbAll_Click);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(473, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(6, 10);
            this.label4.TabIndex = 7;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(210, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(7, 11);
            this.label3.TabIndex = 7;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(370, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(7, 11);
            this.label2.TabIndex = 7;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(83, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(7, 11);
            this.label1.TabIndex = 7;
            this.label1.Text = "*";
            // 
            // lblEndMonth
            // 
            this.lblEndMonth.Location = new System.Drawing.Point(308, 37);
            this.lblEndMonth.Name = "lblEndMonth";
            this.lblEndMonth.Size = new System.Drawing.Size(63, 16);
            this.lblEndMonth.TabIndex = 1;
            this.lblEndMonth.Text = "End Month";
            // 
            // txtEndMnth
            // 
            this.txtEndMnth.Location = new System.Drawing.Point(385, 33);
            this.txtEndMnth.MaxLength = 2;
            this.txtEndMnth.Name = "txtEndMnth";
            this.txtEndMnth.Size = new System.Drawing.Size(37, 25);
            this.txtEndMnth.TabIndex = 5;
            this.txtEndMnth.TextChanged += new System.EventHandler(this.txtEndMnth_TextChanged);
            // 
            // lblEndYear
            // 
            this.lblEndYear.Location = new System.Drawing.Point(447, 37);
            this.lblEndYear.Name = "lblEndYear";
            this.lblEndYear.Size = new System.Drawing.Size(25, 16);
            this.lblEndYear.TabIndex = 1;
            this.lblEndYear.Text = "Year";
            // 
            // txtEndYear
            // 
            this.txtEndYear.Location = new System.Drawing.Point(489, 33);
            this.txtEndYear.MaxLength = 4;
            this.txtEndYear.Name = "txtEndYear";
            this.txtEndYear.Size = new System.Drawing.Size(60, 25);
            this.txtEndYear.TabIndex = 6;
            this.txtEndYear.TextChanged += new System.EventHandler(this.txtEndYear_TextChanged);
            // 
            // lblStartYear
            // 
            this.lblStartYear.Location = new System.Drawing.Point(183, 37);
            this.lblStartYear.Name = "lblStartYear";
            this.lblStartYear.Size = new System.Drawing.Size(26, 16);
            this.lblStartYear.TabIndex = 1;
            this.lblStartYear.Text = "Year";
            // 
            // txtStrtYear
            // 
            this.txtStrtYear.Location = new System.Drawing.Point(226, 33);
            this.txtStrtYear.MaxLength = 4;
            this.txtStrtYear.Name = "txtStrtYear";
            this.txtStrtYear.Size = new System.Drawing.Size(55, 25);
            this.txtStrtYear.TabIndex = 4;
            this.txtStrtYear.TextChanged += new System.EventHandler(this.txtStrtYear_TextChanged);
            // 
            // txtStrtMnth
            // 
            this.txtStrtMnth.Location = new System.Drawing.Point(119, 33);
            this.txtStrtMnth.MaxLength = 2;
            this.txtStrtMnth.Name = "txtStrtMnth";
            this.txtStrtMnth.Size = new System.Drawing.Size(37, 25);
            this.txtStrtMnth.TabIndex = 3;
            this.txtStrtMnth.TextChanged += new System.EventHandler(this.txtStrtMnth_TextChanged);
            // 
            // lblStrtMonth
            // 
            this.lblStrtMonth.Location = new System.Drawing.Point(15, 37);
            this.lblStrtMonth.Name = "lblStrtMonth";
            this.lblStrtMonth.Size = new System.Drawing.Size(70, 16);
            this.lblStrtMonth.TabIndex = 1;
            this.lblStrtMonth.Text = "Start Month";
            // 
            // panel5
            // 
            this.panel5.AppearanceKey = "panel-grdo";
            this.panel5.Controls.Add(this.btnSaveParameters);
            this.panel5.Controls.Add(this.spacer4);
            this.panel5.Controls.Add(this.btnGenPDF);
            this.panel5.Controls.Add(this.spacer3);
            this.panel5.Controls.Add(this.btnGetParameters);
            this.panel5.Controls.Add(this.button1);
            this.panel5.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 179);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.panel5.Size = new System.Drawing.Size(748, 35);
            this.panel5.TabIndex = 2;
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.AppearanceKey = "button-reports";
            this.btnSaveParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParameters.Location = new System.Drawing.Point(128, 5);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(110, 25);
            this.btnSaveParameters.TabIndex = 2;
            this.btnSaveParameters.Text = "&Save Parameters";
            this.btnSaveParameters.Click += new System.EventHandler(this.btnSaveParameters_Click);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(125, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // btnGenPDF
            // 
            this.btnGenPDF.AppearanceKey = "button-reports";
            this.btnGenPDF.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPDF.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGenPDF.Location = new System.Drawing.Point(560, 5);
            this.btnGenPDF.Name = "btnGenPDF";
            this.btnGenPDF.Size = new System.Drawing.Size(85, 25);
            this.btnGenPDF.TabIndex = 3;
            this.btnGenPDF.Text = "&Generate";
            this.btnGenPDF.Click += new System.EventHandler(this.btnGenPDF_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(645, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // btnGetParameters
            // 
            this.btnGetParameters.AppearanceKey = "button-reports";
            this.btnGetParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnGetParameters.Location = new System.Drawing.Point(15, 5);
            this.btnGetParameters.Name = "btnGetParameters";
            this.btnGetParameters.Size = new System.Drawing.Size(110, 25);
            this.btnGetParameters.TabIndex = 1;
            this.btnGetParameters.Text = "Get &Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // button1
            // 
            this.button1.AppearanceKey = "button-reports";
            this.button1.Dock = Wisej.Web.DockStyle.Right;
            this.button1.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button1.Location = new System.Drawing.Point(648, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 25);
            this.button1.TabIndex = 4;
            this.button1.Text = "Pre&view";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.panel5);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(748, 214);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlSite);
            this.pnlParams.Controls.Add(this.pnlType);
            this.pnlParams.Controls.Add(this.pnlReport);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(748, 136);
            this.pnlParams.TabIndex = 1;
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
            this.pnlHieFilter.Size = new System.Drawing.Size(748, 43);
            this.pnlHieFilter.TabIndex = 99;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(675, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(64, 25);
            this.pnlFilter.TabIndex = 55;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // HSSB2110
            // 
            this.ClientSize = new System.Drawing.Size(748, 214);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB2110";
            this.Text = "HSSB2110";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "rlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.HSSB2110_ToolClick);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlReport.ResumeLayout(false);
            this.pnlReport.PerformLayout();
            this.pnlType.ResumeLayout(false);
            this.pnlSite.ResumeLayout(false);
            this.pnlSite.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox Pb_Search_Hie;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private Panel pnlReport;
        private Label lblReport;
        private Label lblTypeRep;
        private Panel pnlType;
        private Label lblSite;
        private Panel pnlSite;
        private Label lblEndMonth;
        private TextBoxWithValidation txtEndMnth;
        private Label lblEndYear;
        private TextBoxWithValidation txtEndYear;
        private Label lblStartYear;
        private TextBoxWithValidation txtStrtYear;
        private TextBoxWithValidation txtStrtMnth;
        private Label lblStrtMonth;
        private Panel panel5;
        private Button button1;
        private Button btnGenPDF;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private RadioButton rbSchedule;
        private RadioButton rbClass;
        private RadioButton rbCondensed;
        private RadioButton rbFull;
        private RadioButton rbSelectedSite;
        private RadioButton rbAll;
        private Button btnGetParameters;
        private Button btnSaveParameters;
        private Label lblActive;
        private ComboBox cmbActive;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Panel pnlParams;
    }
}