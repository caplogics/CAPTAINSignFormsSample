namespace Captain.Common.Views.Forms
{
    partial class HSSB0125_Report
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

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSSB0125_Report));
            this.pnlForm = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlLoc = new Wisej.Web.Panel();
            this.pnlCredType = new Wisej.Web.Panel();
            this.rdbSelCredType = new Wisej.Web.RadioButton();
            this.spacer18 = new Wisej.Web.Spacer();
            this.rdbAllCredType = new Wisej.Web.RadioButton();
            this.spacer11 = new Wisej.Web.Spacer();
            this.lblCredType = new Wisej.Web.Label();
            this.spacer15 = new Wisej.Web.Spacer();
            this.rdbSelLocation = new Wisej.Web.RadioButton();
            this.spacer17 = new Wisej.Web.Spacer();
            this.rdbAllLocation = new Wisej.Web.RadioButton();
            this.spacer7 = new Wisej.Web.Spacer();
            this.lblLoc = new Wisej.Web.Label();
            this.pnlLevel = new Wisej.Web.Panel();
            this.rdbSelFormat = new Wisej.Web.RadioButton();
            this.spacer16 = new Wisej.Web.Spacer();
            this.rdbAllFormat = new Wisej.Web.RadioButton();
            this.spacer10 = new Wisej.Web.Spacer();
            this.lblFormat = new Wisej.Web.Label();
            this.pnlServArea = new Wisej.Web.Panel();
            this.cmbUserServArea = new Wisej.Web.UserComboBox();
            this.ServAreaListBox = new Wisej.Web.CheckedListBox();
            this.spacer5 = new Wisej.Web.Spacer();
            this.lblServArea = new Wisej.Web.Label();
            this.pnlTopic = new Wisej.Web.Panel();
            this.rdbSelTopic = new Wisej.Web.RadioButton();
            this.spacer14 = new Wisej.Web.Spacer();
            this.rdbAllTopic = new Wisej.Web.RadioButton();
            this.spacer4 = new Wisej.Web.Spacer();
            this.lblTopic = new Wisej.Web.Label();
            this.pnlName = new Wisej.Web.Panel();
            this.rdbSelTNames = new Wisej.Web.RadioButton();
            this.spacer13 = new Wisej.Web.Spacer();
            this.rdbAllTNames = new Wisej.Web.RadioButton();
            this.spacer3 = new Wisej.Web.Spacer();
            this.lblTrainName = new Wisej.Web.Label();
            this.panel1 = new Wisej.Web.Panel();
            this.rdbSelSName = new Wisej.Web.RadioButton();
            this.spacer19 = new Wisej.Web.Spacer();
            this.rdbAllSName = new Wisej.Web.RadioButton();
            this.spacer20 = new Wisej.Web.Spacer();
            this.lblStaffName = new Wisej.Web.Label();
            this.pnlDates = new Wisej.Web.Panel();
            this.dtpToDte = new Wisej.Web.DateTimePicker();
            this.lblToDate = new Wisej.Web.Label();
            this.spacer2 = new Wisej.Web.Spacer();
            this.dtpFromDte = new Wisej.Web.DateTimePicker();
            this.spacer12 = new Wisej.Web.Spacer();
            this.lblFromDte = new Wisej.Web.Label();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.spacer8 = new Wisej.Web.Spacer();
            this.pnlHie = new Wisej.Web.Panel();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.spacer9 = new Wisej.Web.Spacer();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlSave = new Wisej.Web.Panel();
            this.cmbServArea = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.btnSaveParameters = new Wisej.Web.Button();
            this.spacer6 = new Wisej.Web.Spacer();
            this.btnGetParameters = new Wisej.Web.Button();
            this.btnGeneratePdf = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnPdfPreview = new Wisej.Web.Button();
            this.pnlForm.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnlLoc.SuspendLayout();
            this.pnlCredType.SuspendLayout();
            this.pnlLevel.SuspendLayout();
            this.pnlServArea.SuspendLayout();
            this.pnlTopic.SuspendLayout();
            this.pnlName.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlDates.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Controls.Add(this.pnlParams);
            this.pnlForm.Controls.Add(this.pnlSave);
            this.pnlForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(748, 288);
            this.pnlForm.TabIndex = 1;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlLoc);
            this.pnlParams.Controls.Add(this.pnlLevel);
            this.pnlParams.Controls.Add(this.pnlServArea);
            this.pnlParams.Controls.Add(this.pnlTopic);
            this.pnlParams.Controls.Add(this.pnlName);
            this.pnlParams.Controls.Add(this.panel1);
            this.pnlParams.Controls.Add(this.pnlDates);
            this.pnlParams.Controls.Add(this.pnlHieFilter);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(748, 253);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlLoc
            // 
            this.pnlLoc.Controls.Add(this.pnlCredType);
            this.pnlLoc.Controls.Add(this.spacer15);
            this.pnlLoc.Controls.Add(this.rdbSelLocation);
            this.pnlLoc.Controls.Add(this.spacer17);
            this.pnlLoc.Controls.Add(this.rdbAllLocation);
            this.pnlLoc.Controls.Add(this.spacer7);
            this.pnlLoc.Controls.Add(this.lblLoc);
            this.pnlLoc.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlLoc.Location = new System.Drawing.Point(0, 224);
            this.pnlLoc.Name = "pnlLoc";
            this.pnlLoc.Padding = new Wisej.Web.Padding(15, 0, 0, 6);
            this.pnlLoc.Size = new System.Drawing.Size(748, 29);
            this.pnlLoc.TabIndex = 6;
            // 
            // pnlCredType
            // 
            this.pnlCredType.Controls.Add(this.rdbSelCredType);
            this.pnlCredType.Controls.Add(this.spacer18);
            this.pnlCredType.Controls.Add(this.rdbAllCredType);
            this.pnlCredType.Controls.Add(this.spacer11);
            this.pnlCredType.Controls.Add(this.lblCredType);
            this.pnlCredType.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCredType.Location = new System.Drawing.Point(267, 0);
            this.pnlCredType.Name = "pnlCredType";
            this.pnlCredType.Size = new System.Drawing.Size(481, 23);
            this.pnlCredType.TabIndex = 3;
            // 
            // rdbSelCredType
            // 
            this.rdbSelCredType.Dock = Wisej.Web.DockStyle.Left;
            this.rdbSelCredType.Location = new System.Drawing.Point(151, 0);
            this.rdbSelCredType.Name = "rdbSelCredType";
            this.rdbSelCredType.Size = new System.Drawing.Size(66, 23);
            this.rdbSelCredType.TabIndex = 2;
            this.rdbSelCredType.Text = "Select";
            this.rdbSelCredType.Click += new System.EventHandler(this.rdbSelCredType_Click);
            // 
            // spacer18
            // 
            this.spacer18.Dock = Wisej.Web.DockStyle.Left;
            this.spacer18.Location = new System.Drawing.Point(126, 0);
            this.spacer18.Name = "spacer18";
            this.spacer18.Size = new System.Drawing.Size(25, 23);
            // 
            // rdbAllCredType
            // 
            this.rdbAllCredType.Checked = true;
            this.rdbAllCredType.Dock = Wisej.Web.DockStyle.Left;
            this.rdbAllCredType.Location = new System.Drawing.Point(80, 0);
            this.rdbAllCredType.Name = "rdbAllCredType";
            this.rdbAllCredType.Size = new System.Drawing.Size(46, 23);
            this.rdbAllCredType.TabIndex = 1;
            this.rdbAllCredType.TabStop = true;
            this.rdbAllCredType.Text = "All";
            this.rdbAllCredType.Click += new System.EventHandler(this.rdbAllCredType_Click);
            // 
            // spacer11
            // 
            this.spacer11.Dock = Wisej.Web.DockStyle.Left;
            this.spacer11.Location = new System.Drawing.Point(66, 0);
            this.spacer11.Name = "spacer11";
            this.spacer11.Size = new System.Drawing.Size(14, 23);
            // 
            // lblCredType
            // 
            this.lblCredType.AutoSize = true;
            this.lblCredType.Dock = Wisej.Web.DockStyle.Left;
            this.lblCredType.Location = new System.Drawing.Point(0, 0);
            this.lblCredType.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblCredType.Name = "lblCredType";
            this.lblCredType.Size = new System.Drawing.Size(66, 23);
            this.lblCredType.TabIndex = 17;
            this.lblCredType.Text = "Credit Type";
            this.lblCredType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer15
            // 
            this.spacer15.Dock = Wisej.Web.DockStyle.Left;
            this.spacer15.Location = new System.Drawing.Point(242, 0);
            this.spacer15.Name = "spacer15";
            this.spacer15.Size = new System.Drawing.Size(25, 23);
            // 
            // rdbSelLocation
            // 
            this.rdbSelLocation.Dock = Wisej.Web.DockStyle.Left;
            this.rdbSelLocation.Location = new System.Drawing.Point(176, 0);
            this.rdbSelLocation.Name = "rdbSelLocation";
            this.rdbSelLocation.Size = new System.Drawing.Size(66, 23);
            this.rdbSelLocation.TabIndex = 2;
            this.rdbSelLocation.Text = "Select";
            this.rdbSelLocation.Click += new System.EventHandler(this.rdbSelLocation_Click);
            // 
            // spacer17
            // 
            this.spacer17.Dock = Wisej.Web.DockStyle.Left;
            this.spacer17.Location = new System.Drawing.Point(151, 0);
            this.spacer17.Name = "spacer17";
            this.spacer17.Size = new System.Drawing.Size(25, 23);
            // 
            // rdbAllLocation
            // 
            this.rdbAllLocation.Checked = true;
            this.rdbAllLocation.Dock = Wisej.Web.DockStyle.Left;
            this.rdbAllLocation.Location = new System.Drawing.Point(105, 0);
            this.rdbAllLocation.Name = "rdbAllLocation";
            this.rdbAllLocation.Size = new System.Drawing.Size(46, 23);
            this.rdbAllLocation.TabIndex = 1;
            this.rdbAllLocation.TabStop = true;
            this.rdbAllLocation.Text = "All";
            this.rdbAllLocation.Click += new System.EventHandler(this.rdbAllLocation_Click);
            // 
            // spacer7
            // 
            this.spacer7.Dock = Wisej.Web.DockStyle.Left;
            this.spacer7.Location = new System.Drawing.Point(65, 0);
            this.spacer7.Name = "spacer7";
            this.spacer7.Size = new System.Drawing.Size(40, 23);
            // 
            // lblLoc
            // 
            this.lblLoc.AutoSize = true;
            this.lblLoc.Dock = Wisej.Web.DockStyle.Left;
            this.lblLoc.Location = new System.Drawing.Point(15, 0);
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.Size = new System.Drawing.Size(50, 23);
            this.lblLoc.TabIndex = 4;
            this.lblLoc.Text = "Location";
            this.lblLoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlLevel
            // 
            this.pnlLevel.Controls.Add(this.rdbSelFormat);
            this.pnlLevel.Controls.Add(this.spacer16);
            this.pnlLevel.Controls.Add(this.rdbAllFormat);
            this.pnlLevel.Controls.Add(this.spacer10);
            this.pnlLevel.Controls.Add(this.lblFormat);
            this.pnlLevel.Dock = Wisej.Web.DockStyle.Top;
            this.pnlLevel.Location = new System.Drawing.Point(0, 195);
            this.pnlLevel.Name = "pnlLevel";
            this.pnlLevel.Padding = new Wisej.Web.Padding(15, 0, 0, 4);
            this.pnlLevel.Size = new System.Drawing.Size(748, 29);
            this.pnlLevel.TabIndex = 5;
            // 
            // rdbSelFormat
            // 
            this.rdbSelFormat.Dock = Wisej.Web.DockStyle.Left;
            this.rdbSelFormat.Location = new System.Drawing.Point(176, 0);
            this.rdbSelFormat.Name = "rdbSelFormat";
            this.rdbSelFormat.Size = new System.Drawing.Size(66, 25);
            this.rdbSelFormat.TabIndex = 2;
            this.rdbSelFormat.Text = "Select";
            this.rdbSelFormat.Click += new System.EventHandler(this.rdbSelFormat_Click);
            // 
            // spacer16
            // 
            this.spacer16.Dock = Wisej.Web.DockStyle.Left;
            this.spacer16.Location = new System.Drawing.Point(151, 0);
            this.spacer16.Name = "spacer16";
            this.spacer16.Size = new System.Drawing.Size(25, 25);
            // 
            // rdbAllFormat
            // 
            this.rdbAllFormat.Checked = true;
            this.rdbAllFormat.Dock = Wisej.Web.DockStyle.Left;
            this.rdbAllFormat.Location = new System.Drawing.Point(105, 0);
            this.rdbAllFormat.Name = "rdbAllFormat";
            this.rdbAllFormat.Size = new System.Drawing.Size(46, 25);
            this.rdbAllFormat.TabIndex = 1;
            this.rdbAllFormat.TabStop = true;
            this.rdbAllFormat.Text = "All";
            this.rdbAllFormat.Click += new System.EventHandler(this.rdbAllFormat_Click);
            // 
            // spacer10
            // 
            this.spacer10.Dock = Wisej.Web.DockStyle.Left;
            this.spacer10.Location = new System.Drawing.Point(58, 0);
            this.spacer10.Name = "spacer10";
            this.spacer10.Size = new System.Drawing.Size(47, 25);
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Dock = Wisej.Web.DockStyle.Left;
            this.lblFormat.Location = new System.Drawing.Point(15, 0);
            this.lblFormat.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(43, 25);
            this.lblFormat.TabIndex = 12;
            this.lblFormat.Text = "Format";
            this.lblFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlServArea
            // 
            this.pnlServArea.Controls.Add(this.cmbUserServArea);
            this.pnlServArea.Controls.Add(this.spacer5);
            this.pnlServArea.Controls.Add(this.lblServArea);
            this.pnlServArea.Dock = Wisej.Web.DockStyle.Top;
            this.pnlServArea.Location = new System.Drawing.Point(0, 166);
            this.pnlServArea.Name = "pnlServArea";
            this.pnlServArea.Padding = new Wisej.Web.Padding(15, 0, 0, 4);
            this.pnlServArea.Size = new System.Drawing.Size(748, 29);
            this.pnlServArea.TabIndex = 4;
            // 
            // cmbUserServArea
            // 
            this.cmbUserServArea.Dock = Wisej.Web.DockStyle.Left;
            this.cmbUserServArea.DropDownControl = this.ServAreaListBox;
            this.cmbUserServArea.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbUserServArea.Location = new System.Drawing.Point(105, 0);
            this.cmbUserServArea.Name = "cmbUserServArea";
            this.cmbUserServArea.Size = new System.Drawing.Size(600, 25);
            this.cmbUserServArea.TabIndex = 1;
            // 
            // ServAreaListBox
            // 
            this.ServAreaListBox.Dock = Wisej.Web.DockStyle.Left;
            this.ServAreaListBox.Location = new System.Drawing.Point(238, 5);
            this.ServAreaListBox.Name = "ServAreaListBox";
            this.ServAreaListBox.Size = new System.Drawing.Size(175, 25);
            this.ServAreaListBox.TabIndex = 3;
            this.ServAreaListBox.AfterItemCheck += new Wisej.Web.ItemCheckEventHandler(this.ServAreaListBox_AfterItemCheck);
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(88, 0);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(17, 25);
            // 
            // lblServArea
            // 
            this.lblServArea.AutoSize = true;
            this.lblServArea.Dock = Wisej.Web.DockStyle.Left;
            this.lblServArea.Location = new System.Drawing.Point(15, 0);
            this.lblServArea.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblServArea.Name = "lblServArea";
            this.lblServArea.Size = new System.Drawing.Size(73, 25);
            this.lblServArea.TabIndex = 4;
            this.lblServArea.Text = "Service Area";
            this.lblServArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTopic
            // 
            this.pnlTopic.Controls.Add(this.rdbSelTopic);
            this.pnlTopic.Controls.Add(this.spacer14);
            this.pnlTopic.Controls.Add(this.rdbAllTopic);
            this.pnlTopic.Controls.Add(this.spacer4);
            this.pnlTopic.Controls.Add(this.lblTopic);
            this.pnlTopic.Dock = Wisej.Web.DockStyle.Top;
            this.pnlTopic.Location = new System.Drawing.Point(0, 137);
            this.pnlTopic.Name = "pnlTopic";
            this.pnlTopic.Padding = new Wisej.Web.Padding(15, 0, 0, 4);
            this.pnlTopic.Size = new System.Drawing.Size(748, 29);
            this.pnlTopic.TabIndex = 3;
            // 
            // rdbSelTopic
            // 
            this.rdbSelTopic.Dock = Wisej.Web.DockStyle.Left;
            this.rdbSelTopic.Location = new System.Drawing.Point(176, 0);
            this.rdbSelTopic.Name = "rdbSelTopic";
            this.rdbSelTopic.Size = new System.Drawing.Size(66, 25);
            this.rdbSelTopic.TabIndex = 2;
            this.rdbSelTopic.Text = "Select";
            this.rdbSelTopic.Click += new System.EventHandler(this.rdbSelTopic_Click);
            // 
            // spacer14
            // 
            this.spacer14.Dock = Wisej.Web.DockStyle.Left;
            this.spacer14.Location = new System.Drawing.Point(151, 0);
            this.spacer14.Name = "spacer14";
            this.spacer14.Size = new System.Drawing.Size(25, 25);
            // 
            // rdbAllTopic
            // 
            this.rdbAllTopic.Checked = true;
            this.rdbAllTopic.Dock = Wisej.Web.DockStyle.Left;
            this.rdbAllTopic.Location = new System.Drawing.Point(105, 0);
            this.rdbAllTopic.Name = "rdbAllTopic";
            this.rdbAllTopic.Size = new System.Drawing.Size(46, 25);
            this.rdbAllTopic.TabIndex = 1;
            this.rdbAllTopic.TabStop = true;
            this.rdbAllTopic.Text = "All";
            this.rdbAllTopic.CheckedChanged += new System.EventHandler(this.rdbAllTopic_CheckedChanged);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(49, 0);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(56, 25);
            // 
            // lblTopic
            // 
            this.lblTopic.AutoSize = true;
            this.lblTopic.Dock = Wisej.Web.DockStyle.Left;
            this.lblTopic.Location = new System.Drawing.Point(15, 0);
            this.lblTopic.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblTopic.Name = "lblTopic";
            this.lblTopic.Size = new System.Drawing.Size(34, 25);
            this.lblTopic.TabIndex = 1;
            this.lblTopic.Text = "Topic";
            this.lblTopic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlName
            // 
            this.pnlName.Controls.Add(this.rdbSelTNames);
            this.pnlName.Controls.Add(this.spacer13);
            this.pnlName.Controls.Add(this.rdbAllTNames);
            this.pnlName.Controls.Add(this.spacer3);
            this.pnlName.Controls.Add(this.lblTrainName);
            this.pnlName.Dock = Wisej.Web.DockStyle.Top;
            this.pnlName.Location = new System.Drawing.Point(0, 108);
            this.pnlName.Name = "pnlName";
            this.pnlName.Padding = new Wisej.Web.Padding(15, 0, 0, 4);
            this.pnlName.Size = new System.Drawing.Size(748, 29);
            this.pnlName.TabIndex = 2;
            // 
            // rdbSelTNames
            // 
            this.rdbSelTNames.Dock = Wisej.Web.DockStyle.Left;
            this.rdbSelTNames.Location = new System.Drawing.Point(176, 0);
            this.rdbSelTNames.Name = "rdbSelTNames";
            this.rdbSelTNames.Size = new System.Drawing.Size(66, 25);
            this.rdbSelTNames.TabIndex = 2;
            this.rdbSelTNames.Text = "Select";
            this.rdbSelTNames.Click += new System.EventHandler(this.rdbSelTNames_Click);
            // 
            // spacer13
            // 
            this.spacer13.Dock = Wisej.Web.DockStyle.Left;
            this.spacer13.Location = new System.Drawing.Point(151, 0);
            this.spacer13.Name = "spacer13";
            this.spacer13.Size = new System.Drawing.Size(25, 25);
            // 
            // rdbAllTNames
            // 
            this.rdbAllTNames.Checked = true;
            this.rdbAllTNames.Dock = Wisej.Web.DockStyle.Left;
            this.rdbAllTNames.Location = new System.Drawing.Point(105, 0);
            this.rdbAllTNames.Name = "rdbAllTNames";
            this.rdbAllTNames.Size = new System.Drawing.Size(46, 25);
            this.rdbAllTNames.TabIndex = 1;
            this.rdbAllTNames.TabStop = true;
            this.rdbAllTNames.Text = "All";
            this.rdbAllTNames.Click += new System.EventHandler(this.rdbAllTNames_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(100, 0);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(5, 25);
            // 
            // lblTrainName
            // 
            this.lblTrainName.AutoSize = true;
            this.lblTrainName.Dock = Wisej.Web.DockStyle.Left;
            this.lblTrainName.Location = new System.Drawing.Point(15, 0);
            this.lblTrainName.MinimumSize = new System.Drawing.Size(85, 16);
            this.lblTrainName.Name = "lblTrainName";
            this.lblTrainName.Size = new System.Drawing.Size(85, 25);
            this.lblTrainName.TabIndex = 0;
            this.lblTrainName.Text = "Training Name";
            this.lblTrainName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbSelSName);
            this.panel1.Controls.Add(this.spacer19);
            this.panel1.Controls.Add(this.rdbAllSName);
            this.panel1.Controls.Add(this.spacer20);
            this.panel1.Controls.Add(this.lblStaffName);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 79);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(15, 0, 0, 4);
            this.panel1.Size = new System.Drawing.Size(748, 29);
            this.panel1.TabIndex = 101;
            // 
            // rdbSelSName
            // 
            this.rdbSelSName.Dock = Wisej.Web.DockStyle.Left;
            this.rdbSelSName.Location = new System.Drawing.Point(176, 0);
            this.rdbSelSName.Name = "rdbSelSName";
            this.rdbSelSName.Size = new System.Drawing.Size(66, 25);
            this.rdbSelSName.TabIndex = 2;
            this.rdbSelSName.Text = "Select";
            this.rdbSelSName.Click += new System.EventHandler(this.rdbSelSName_Click);
            // 
            // spacer19
            // 
            this.spacer19.Dock = Wisej.Web.DockStyle.Left;
            this.spacer19.Location = new System.Drawing.Point(151, 0);
            this.spacer19.Name = "spacer19";
            this.spacer19.Size = new System.Drawing.Size(25, 25);
            // 
            // rdbAllSName
            // 
            this.rdbAllSName.Checked = true;
            this.rdbAllSName.Dock = Wisej.Web.DockStyle.Left;
            this.rdbAllSName.Location = new System.Drawing.Point(105, 0);
            this.rdbAllSName.Name = "rdbAllSName";
            this.rdbAllSName.Size = new System.Drawing.Size(46, 25);
            this.rdbAllSName.TabIndex = 1;
            this.rdbAllSName.TabStop = true;
            this.rdbAllSName.Text = "All";
            this.rdbAllSName.Click += new System.EventHandler(this.rdbAllSName_Click);
            // 
            // spacer20
            // 
            this.spacer20.Dock = Wisej.Web.DockStyle.Left;
            this.spacer20.Location = new System.Drawing.Point(100, 0);
            this.spacer20.Name = "spacer20";
            this.spacer20.Size = new System.Drawing.Size(5, 25);
            // 
            // lblStaffName
            // 
            this.lblStaffName.AutoSize = true;
            this.lblStaffName.Dock = Wisej.Web.DockStyle.Left;
            this.lblStaffName.Location = new System.Drawing.Point(15, 0);
            this.lblStaffName.MinimumSize = new System.Drawing.Size(85, 16);
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.Size = new System.Drawing.Size(85, 25);
            this.lblStaffName.TabIndex = 0;
            this.lblStaffName.Text = "Staff Members";
            this.lblStaffName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDates
            // 
            this.pnlDates.Controls.Add(this.dtpToDte);
            this.pnlDates.Controls.Add(this.lblToDate);
            this.pnlDates.Controls.Add(this.spacer2);
            this.pnlDates.Controls.Add(this.dtpFromDte);
            this.pnlDates.Controls.Add(this.spacer12);
            this.pnlDates.Controls.Add(this.lblFromDte);
            this.pnlDates.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDates.Location = new System.Drawing.Point(0, 43);
            this.pnlDates.Name = "pnlDates";
            this.pnlDates.Padding = new Wisej.Web.Padding(15, 7, 0, 4);
            this.pnlDates.Size = new System.Drawing.Size(748, 36);
            this.pnlDates.TabIndex = 1;
            // 
            // dtpToDte
            // 
            this.dtpToDte.AutoSize = false;
            this.dtpToDte.CustomFormat = "MM/dd/yyyy";
            this.dtpToDte.Dock = Wisej.Web.DockStyle.Left;
            this.dtpToDte.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpToDte.Location = new System.Drawing.Point(296, 7);
            this.dtpToDte.Name = "dtpToDte";
            this.dtpToDte.ShowCheckBox = true;
            this.dtpToDte.ShowToolTips = false;
            this.dtpToDte.Size = new System.Drawing.Size(116, 25);
            this.dtpToDte.TabIndex = 2;
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Dock = Wisej.Web.DockStyle.Left;
            this.lblToDate.Location = new System.Drawing.Point(246, 7);
            this.lblToDate.MinimumSize = new System.Drawing.Size(50, 0);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(50, 25);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(221, 7);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(25, 25);
            // 
            // dtpFromDte
            // 
            this.dtpFromDte.AutoSize = false;
            this.dtpFromDte.CustomFormat = "MM/dd/yyyy";
            this.dtpFromDte.Dock = Wisej.Web.DockStyle.Left;
            this.dtpFromDte.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpFromDte.Location = new System.Drawing.Point(105, 7);
            this.dtpFromDte.Name = "dtpFromDte";
            this.dtpFromDte.ShowCheckBox = true;
            this.dtpFromDte.ShowToolTips = false;
            this.dtpFromDte.Size = new System.Drawing.Size(116, 25);
            this.dtpFromDte.TabIndex = 1;
            // 
            // spacer12
            // 
            this.spacer12.Dock = Wisej.Web.DockStyle.Left;
            this.spacer12.Location = new System.Drawing.Point(76, 7);
            this.spacer12.Name = "spacer12";
            this.spacer12.Size = new System.Drawing.Size(29, 25);
            // 
            // lblFromDte
            // 
            this.lblFromDte.AutoSize = true;
            this.lblFromDte.Dock = Wisej.Web.DockStyle.Left;
            this.lblFromDte.Location = new System.Drawing.Point(15, 7);
            this.lblFromDte.Name = "lblFromDte";
            this.lblFromDte.Size = new System.Drawing.Size(61, 25);
            this.lblFromDte.TabIndex = 2;
            this.lblFromDte.Text = "From Date";
            this.lblFromDte.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.pnlHieFilter.TabIndex = 100;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer8);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(690, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(49, 25);
            this.pnlFilter.TabIndex = 33;
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
            // 
            // spacer8
            // 
            this.spacer8.Dock = Wisej.Web.DockStyle.Left;
            this.spacer8.Location = new System.Drawing.Point(0, 0);
            this.spacer8.Name = "spacer8";
            this.spacer8.Size = new System.Drawing.Size(15, 25);
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer9);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(675, 25);
            this.pnlHie.TabIndex = 88;
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(610, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 55;
            this.CmbYear.TabStop = false;
            this.CmbYear.Visible = false;
            // 
            // spacer9
            // 
            this.spacer9.Dock = Wisej.Web.DockStyle.Left;
            this.spacer9.Location = new System.Drawing.Point(595, 0);
            this.spacer9.Name = "spacer9";
            this.spacer9.Size = new System.Drawing.Size(15, 25);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(595, 25);
            this.Txt_HieDesc.TabIndex = 77;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.cmbServArea);
            this.pnlSave.Controls.Add(this.ServAreaListBox);
            this.pnlSave.Controls.Add(this.btnSaveParameters);
            this.pnlSave.Controls.Add(this.spacer6);
            this.pnlSave.Controls.Add(this.btnGetParameters);
            this.pnlSave.Controls.Add(this.btnGeneratePdf);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnPdfPreview);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 253);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(748, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // cmbServArea
            // 
            this.cmbServArea.Dock = Wisej.Web.DockStyle.Left;
            this.cmbServArea.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbServArea.Location = new System.Drawing.Point(413, 5);
            this.cmbServArea.Name = "cmbServArea";
            this.cmbServArea.Size = new System.Drawing.Size(30, 25);
            this.cmbServArea.TabIndex = 120;
            this.cmbServArea.TabStop = false;
            this.cmbServArea.Visible = false;
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.AppearanceKey = "button-reports";
            this.btnSaveParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParameters.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSaveParameters.Location = new System.Drawing.Point(128, 5);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(110, 25);
            this.btnSaveParameters.TabIndex = 2;
            this.btnSaveParameters.Text = "&Save Parameters";
            this.btnSaveParameters.Click += new System.EventHandler(this.btnSaveParameters_Click);
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Left;
            this.spacer6.Location = new System.Drawing.Point(125, 5);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(3, 25);
            // 
            // btnGetParameters
            // 
            this.btnGetParameters.AppearanceKey = "button-reports";
            this.btnGetParameters.Dock = Wisej.Web.DockStyle.Left;
            this.btnGetParameters.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGetParameters.Location = new System.Drawing.Point(15, 5);
            this.btnGetParameters.Name = "btnGetParameters";
            this.btnGetParameters.Size = new System.Drawing.Size(110, 25);
            this.btnGetParameters.TabIndex = 1;
            this.btnGetParameters.Text = "Get &Parameters";
            this.btnGetParameters.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.AppearanceKey = "button-reports";
            this.btnGeneratePdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGeneratePdf.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGeneratePdf.Location = new System.Drawing.Point(560, 5);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(85, 25);
            this.btnGeneratePdf.TabIndex = 3;
            this.btnGeneratePdf.Text = "&Generate";
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(645, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnPdfPreview
            // 
            this.btnPdfPreview.AppearanceKey = "button-reports";
            this.btnPdfPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPdfPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPdfPreview.Location = new System.Drawing.Point(648, 5);
            this.btnPdfPreview.Name = "btnPdfPreview";
            this.btnPdfPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPdfPreview.TabIndex = 4;
            this.btnPdfPreview.Text = "Pre&view";
            this.btnPdfPreview.Click += new System.EventHandler(this.btnPdfPreview_Click);
            // 
            // HSSB0125_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 288);
            this.Controls.Add(this.pnlForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSSB0125_Report";
            this.Text = "Individual Training (Grid) Report";
            this.pnlForm.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.pnlLoc.ResumeLayout(false);
            this.pnlLoc.PerformLayout();
            this.pnlCredType.ResumeLayout(false);
            this.pnlCredType.PerformLayout();
            this.pnlLevel.ResumeLayout(false);
            this.pnlLevel.PerformLayout();
            this.pnlServArea.ResumeLayout(false);
            this.pnlServArea.PerformLayout();
            this.pnlTopic.ResumeLayout(false);
            this.pnlTopic.PerformLayout();
            this.pnlName.ResumeLayout(false);
            this.pnlName.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlDates.ResumeLayout(false);
            this.pnlDates.PerformLayout();
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlSave.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlForm;
        private Wisej.Web.Panel pnlParams;
        private Wisej.Web.Panel pnlLoc;
        private Wisej.Web.Spacer spacer15;
        private Wisej.Web.Spacer spacer7;
        private Wisej.Web.Label lblLoc;
        private Wisej.Web.Panel pnlLevel;
        private Wisej.Web.Spacer spacer10;
        private Wisej.Web.Label lblFormat;
        private Wisej.Web.Panel pnlServArea;
        private Wisej.Web.UserComboBox cmbUserServArea;
        private Wisej.Web.Spacer spacer5;
        private Wisej.Web.Label lblServArea;
        private Wisej.Web.Panel pnlTopic;
        private Wisej.Web.Spacer spacer4;
        private Wisej.Web.Label lblTopic;
        private Wisej.Web.Panel pnlName;
        private Wisej.Web.Spacer spacer3;
        private Wisej.Web.Label lblTrainName;
        private Wisej.Web.Panel pnlSave;
        private Wisej.Web.CheckedListBox ServAreaListBox;
        private Controls.Compatibility.ComboBoxEx cmbServArea;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.Panel pnlHieFilter;
        private Wisej.Web.Panel pnlFilter;
        private Wisej.Web.PictureBox Pb_Search_Hie;
        private Wisej.Web.Spacer spacer8;
        private Wisej.Web.Panel pnlHie;
        private Wisej.Web.ComboBox CmbYear;
        private Wisej.Web.Spacer spacer9;
        private Wisej.Web.TextBox Txt_HieDesc;
        private Wisej.Web.Panel pnlDates;
        private Wisej.Web.Label lblFromDte;
        private Wisej.Web.Label lblToDate;
        private Wisej.Web.DateTimePicker dtpFromDte;
        private Wisej.Web.DateTimePicker dtpToDte;
        private Wisej.Web.Spacer spacer12;
        private Wisej.Web.Spacer spacer2;
        private Wisej.Web.RadioButton rdbSelTNames;
        private Wisej.Web.Spacer spacer13;
        private Wisej.Web.RadioButton rdbAllTNames;
        private Wisej.Web.Button btnPdfPreview;
        private Wisej.Web.Button btnGeneratePdf;
        private Wisej.Web.Button btnSaveParameters;
        private Wisej.Web.Spacer spacer6;
        private Wisej.Web.Button btnGetParameters;
        private Wisej.Web.RadioButton rdbSelLocation;
        private Wisej.Web.Spacer spacer17;
        private Wisej.Web.RadioButton rdbAllLocation;
        private Wisej.Web.RadioButton rdbSelFormat;
        private Wisej.Web.Spacer spacer16;
        private Wisej.Web.RadioButton rdbAllFormat;
        private Wisej.Web.RadioButton rdbSelTopic;
        private Wisej.Web.Spacer spacer14;
        private Wisej.Web.RadioButton rdbAllTopic;
        private Wisej.Web.Panel pnlCredType;
        private Wisej.Web.RadioButton rdbSelCredType;
        private Wisej.Web.Spacer spacer18;
        private Wisej.Web.RadioButton rdbAllCredType;
        private Wisej.Web.Spacer spacer11;
        private Wisej.Web.Label lblCredType;
        private Wisej.Web.Panel panel1;
        private Wisej.Web.RadioButton rdbSelSName;
        private Wisej.Web.Spacer spacer19;
        private Wisej.Web.RadioButton rdbAllSName;
        private Wisej.Web.Spacer spacer20;
        private Wisej.Web.Label lblStaffName;
    }
}