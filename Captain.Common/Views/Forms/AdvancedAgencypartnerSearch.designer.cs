using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class AdvancedAgencypartnerSearch
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
            this.BtnSearcRecs = new Wisej.Web.Button();
            this.CmbSearchBy = new Wisej.Web.ComboBox();
            this.label1 = new Wisej.Web.Label();
            this.SeacrhPanel = new Wisej.Web.Panel();
            this.NamePanel = new Wisej.Web.Panel();
            this.label4 = new Wisej.Web.Label();
            this.TxtAgencyName = new Wisej.Web.TextBox();
            this.PhonePanel = new Wisej.Web.Panel();
            this.MtxtPhone = new Wisej.Web.MaskedTextBox();
            this.AppNoPanel = new Wisej.Web.Panel();
            this.MtxtCode = new Wisej.Web.MaskedTextBox();
            this.AddressPanel = new Wisej.Web.Panel();
            this.TxtStreet = new Wisej.Web.TextBox();
            this.label8 = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.TxtCity = new Wisej.Web.TextBox();
            this.TxtState = new Wisej.Web.TextBox();
            this.label9 = new Wisej.Web.Label();
            this.Btn_Clear = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.GvwSearch = new Wisej.Web.DataGridView();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAgyName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtStreet = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCity = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtState = new Wisej.Web.DataGridViewTextBoxColumn();
            this.BtnSelAgency = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.Cb_MST_Prog = new Wisej.Web.CheckBox();
            this.Btn_Curr_Hie = new Wisej.Web.Button();
            this.CbSearch = new Wisej.Web.CheckBox();
            this.CmbAgency = new Wisej.Web.ComboBox();
            this.CmbDept = new Wisej.Web.ComboBox();
            this.CmbProg = new Wisej.Web.ComboBox();
            this.BtnDefHier = new Wisej.Web.Button();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.SeacrhPanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.PhonePanel.SuspendLayout();
            this.AppNoPanel.SuspendLayout();
            this.AddressPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvwSearch)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSearcRecs
            // 
            this.BtnSearcRecs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnSearcRecs.Location = new System.Drawing.Point(606, 2);
            this.BtnSearcRecs.Name = "BtnSearcRecs";
            this.BtnSearcRecs.Size = new System.Drawing.Size(78, 27);
            this.BtnSearcRecs.TabIndex = 12;
            this.BtnSearcRecs.Text = "Search";
            this.BtnSearcRecs.Click += new System.EventHandler(this.BtnSearcRecs_Click);
            // 
            // CmbSearchBy
            // 
            this.CmbSearchBy.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.CmbSearchBy.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbSearchBy.Location = new System.Drawing.Point(61, 5);
            this.CmbSearchBy.Name = "CmbSearchBy";
            this.CmbSearchBy.Size = new System.Drawing.Size(105, 21);
            this.CmbSearchBy.TabIndex = 1;
            this.CmbSearchBy.SelectedIndexChanged += new System.EventHandler(this.CmbSearchBy_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search By";
            // 
            // SeacrhPanel
            // 
            this.SeacrhPanel.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.SeacrhPanel.Controls.Add(this.NamePanel);
            this.SeacrhPanel.Controls.Add(this.PhonePanel);
            this.SeacrhPanel.Controls.Add(this.AppNoPanel);
            this.SeacrhPanel.Controls.Add(this.AddressPanel);
            this.SeacrhPanel.Controls.Add(this.Btn_Clear);
            this.SeacrhPanel.Controls.Add(this.BtnSearcRecs);
            this.SeacrhPanel.Controls.Add(this.CmbSearchBy);
            this.SeacrhPanel.Controls.Add(this.label1);
            this.SeacrhPanel.Location = new System.Drawing.Point(2, 61);
            this.SeacrhPanel.Name = "SeacrhPanel";
            this.SeacrhPanel.Size = new System.Drawing.Size(767, 39);
            this.SeacrhPanel.TabIndex = 6;
            this.SeacrhPanel.Visible = false;
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.label4);
            this.NamePanel.Controls.Add(this.TxtAgencyName);
            this.NamePanel.Location = new System.Drawing.Point(187, 4);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(50, 24);
            this.NamePanel.TabIndex = 2;
            this.NamePanel.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Agency Name";
            // 
            // TxtAgencyName
            // 
            this.TxtAgencyName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.TxtAgencyName.Location = new System.Drawing.Point(86, 1);
            this.TxtAgencyName.MaxLength = 25;
            this.TxtAgencyName.Name = "TxtAgencyName";
            this.TxtAgencyName.Size = new System.Drawing.Size(319, 18);
            this.TxtAgencyName.TabIndex = 1;
            // 
            // PhonePanel
            // 
            this.PhonePanel.Controls.Add(this.MtxtPhone);
            this.PhonePanel.Location = new System.Drawing.Point(291, 3);
            this.PhonePanel.Name = "PhonePanel";
            this.PhonePanel.Size = new System.Drawing.Size(39, 25);
            this.PhonePanel.TabIndex = 4;
            this.PhonePanel.Visible = false;
            // 
            // MtxtPhone
            // 
            this.MtxtPhone.Location = new System.Drawing.Point(3, 1);
            this.MtxtPhone.Mask = "000-000-0000";
            this.MtxtPhone.Name = "MtxtPhone";
            this.MtxtPhone.RightToLeft = Wisej.Web.RightToLeft.No;
            this.MtxtPhone.Size = new System.Drawing.Size(76, 20);
            this.MtxtPhone.TabIndex = 4;
            this.MtxtPhone.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            this.MtxtPhone.GotFocus += new System.EventHandler(this.Controls_Common_GotFocus);
            this.MtxtPhone.LostFocus += new System.EventHandler(this.MtxtPhone_LostFocus);
            // 
            // AppNoPanel
            // 
            this.AppNoPanel.Controls.Add(this.MtxtCode);
            this.AppNoPanel.Location = new System.Drawing.Point(330, 3);
            this.AppNoPanel.Name = "AppNoPanel";
            this.AppNoPanel.Size = new System.Drawing.Size(44, 25);
            this.AppNoPanel.TabIndex = 5;
            this.AppNoPanel.Visible = false;
            // 
            // MtxtCode
            // 
            this.MtxtCode.Location = new System.Drawing.Point(3, 1);
            this.MtxtCode.Name = "MtxtCode";
            this.MtxtCode.RightToLeft = Wisej.Web.RightToLeft.No;
            this.MtxtCode.Size = new System.Drawing.Size(63, 20);
            this.MtxtCode.TabIndex = 1;
            this.MtxtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.MtxtCode.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            this.MtxtCode.KeyDown += new Wisej.Web.KeyEventHandler(this.MtxtAppNo_EnterKeyDown);
            this.MtxtCode.GotFocus += new System.EventHandler(this.Controls_Common_GotFocus);
            this.MtxtCode.LostFocus += new System.EventHandler(this.MtxtAppNo_LostFocus);
            // 
            // AddressPanel
            // 
            this.AddressPanel.Controls.Add(this.TxtStreet);
            this.AddressPanel.Controls.Add(this.label8);
            this.AddressPanel.Controls.Add(this.label7);
            this.AddressPanel.Controls.Add(this.TxtCity);
            this.AddressPanel.Controls.Add(this.TxtState);
            this.AddressPanel.Controls.Add(this.label9);
            this.AddressPanel.Location = new System.Drawing.Point(399, 8);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(39, 20);
            this.AddressPanel.TabIndex = 8;
            this.AddressPanel.Visible = false;
            // 
            // TxtStreet
            // 
            this.TxtStreet.Location = new System.Drawing.Point(37, 2);
            this.TxtStreet.MaxLength = 25;
            this.TxtStreet.Name = "TxtStreet";
            this.TxtStreet.Size = new System.Drawing.Size(137, 16);
            this.TxtStreet.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Street";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(181, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "City";
            // 
            // TxtCity
            // 
            this.TxtCity.Location = new System.Drawing.Point(201, 2);
            this.TxtCity.Name = "TxtCity";
            this.TxtCity.Size = new System.Drawing.Size(101, 16);
            this.TxtCity.TabIndex = 3;
            // 
            // TxtState
            // 
            this.TxtState.Location = new System.Drawing.Point(334, 2);
            this.TxtState.Name = "TxtState";
            this.TxtState.Size = new System.Drawing.Size(22, 16);
            this.TxtState.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(307, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "State";
            // 
            // Btn_Clear
            // 
            this.Btn_Clear.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.Btn_Clear.Location = new System.Drawing.Point(684, 2);
            this.Btn_Clear.Name = "Btn_Clear";
            this.Btn_Clear.Size = new System.Drawing.Size(78, 27);
            this.Btn_Clear.TabIndex = 13;
            this.Btn_Clear.Text = "Clear";
            this.Btn_Clear.Click += new System.EventHandler(this.Btn_Clear_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.GvwSearch);
            this.panel1.Controls.Add(this.BtnSelAgency);
            this.panel1.Location = new System.Drawing.Point(2, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 359);
            this.panel1.TabIndex = 18;
            // 
            // GvwSearch
            // 
            this.GvwSearch.AllowUserToAddRows = false;
            this.GvwSearch.AllowUserToDeleteRows = false;
            this.GvwSearch.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.GvwSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GvwSearch.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvwSearch.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCode,
            this.gvtAgyName,
            this.gvtStreet,
            this.gvtCity,
            this.gvtState});
            this.GvwSearch.Location = new System.Drawing.Point(3, 5);
            this.GvwSearch.MultiSelect = false;
            this.GvwSearch.Name = "GvwSearch";
            this.GvwSearch.ReadOnly = true;
            this.GvwSearch.RowHeadersWidth = 15;
            this.GvwSearch.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GvwSearch.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.GvwSearch.RowTemplate.Height = 19;
            this.GvwSearch.SelectionMode = Wisej.Web.DataGridViewSelectionMode.FullRowSelect;
            this.GvwSearch.Size = new System.Drawing.Size(760, 316);
            this.GvwSearch.TabIndex = 6;
            //this.GvwSearch.VirtualBlockSize = 15;
            this.GvwSearch.SelectionChanged += new System.EventHandler(this.GvwSearch_SelectionChanged);
            // 
            // gvtCode
            // 
            this.gvtCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvtCode.HeaderText = "Code";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.Width = 60;
            // 
            // gvtAgyName
            // 
            this.gvtAgyName.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvtAgyName.HeaderText = "Agency Name";
            this.gvtAgyName.Name = "gvtAgyName";
            this.gvtAgyName.ReadOnly = true;
            this.gvtAgyName.Width = 250;
            // 
            // gvtStreet
            // 
            this.gvtStreet.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvtStreet.HeaderText = "Street";
            this.gvtStreet.Name = "gvtStreet";
            this.gvtStreet.ReadOnly = true;
            this.gvtStreet.Width = 220;
            // 
            // gvtCity
            // 
            this.gvtCity.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvtCity.HeaderText = "City";
            this.gvtCity.Name = "gvtCity";
            this.gvtCity.ReadOnly = true;
            this.gvtCity.Width = 130;
            // 
            // gvtState
            // 
            this.gvtState.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvtState.HeaderText = "State";
            this.gvtState.Name = "gvtState";
            this.gvtState.ReadOnly = true;
            this.gvtState.Width = 60;
            // 
            // BtnSelAgency
            // 
            this.BtnSelAgency.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnSelAgency.Location = new System.Drawing.Point(609, 328);
            this.BtnSelAgency.Name = "BtnSelAgency";
            this.BtnSelAgency.Size = new System.Drawing.Size(155, 27);
            this.BtnSelAgency.TabIndex = 1;
            this.BtnSelAgency.Text = "Select This Agency";
            this.BtnSelAgency.Visible = false;
            this.BtnSelAgency.Click += new System.EventHandler(this.BtnSelApp_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel2.Controls.Add(this.Cb_MST_Prog);
            this.panel2.Controls.Add(this.Btn_Curr_Hie);
            this.panel2.Controls.Add(this.CbSearch);
            this.panel2.Controls.Add(this.CmbAgency);
            this.panel2.Controls.Add(this.CmbDept);
            this.panel2.Controls.Add(this.CmbProg);
            this.panel2.Controls.Add(this.BtnDefHier);
            this.panel2.Controls.Add(this.CmbYear);
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(767, 60);
            this.panel2.TabIndex = 19;
            // 
            // Cb_MST_Prog
            // 
            this.Cb_MST_Prog.AutoSize = true;
            this.Cb_MST_Prog.Checked = true;
            this.Cb_MST_Prog.CheckState = Wisej.Web.CheckState.Checked;
            this.Cb_MST_Prog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.Cb_MST_Prog.Location = new System.Drawing.Point(258, 31);
            this.Cb_MST_Prog.Name = "Cb_MST_Prog";
            this.Cb_MST_Prog.Size = new System.Drawing.Size(188, 17);
            this.Cb_MST_Prog.TabIndex = 20;
            this.Cb_MST_Prog.Text = "Show only intake hierarchies";
            this.Cb_MST_Prog.Visible = false;
            this.Cb_MST_Prog.Click += new System.EventHandler(this.Cb_MST_Prog_Click);
            // 
            // Btn_Curr_Hie
            // 
            this.Btn_Curr_Hie.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.Btn_Curr_Hie.Location = new System.Drawing.Point(447, 29);
            this.Btn_Curr_Hie.Name = "Btn_Curr_Hie";
            this.Btn_Curr_Hie.Size = new System.Drawing.Size(154, 27);
            this.Btn_Curr_Hie.TabIndex = 16;
            this.Btn_Curr_Hie.Text = "Set as Current Hierarchy";
            this.Btn_Curr_Hie.Visible = false;
            this.Btn_Curr_Hie.Click += new System.EventHandler(this.BtnSelApp_Click);
            // 
            // CbSearch
            // 
            this.CbSearch.Location = new System.Drawing.Point(7, 35);
            this.CbSearch.Name = "CbSearch";
            this.CbSearch.Size = new System.Drawing.Size(226, 17);
            this.CbSearch.TabIndex = 4;
            this.CbSearch.Text = "Search Entire Database";
            this.CbSearch.Visible = false;
            this.CbSearch.CheckedChanged += new System.EventHandler(this.CbSearch_CheckedChanged);
            // 
            // CmbAgency
            // 
            this.CmbAgency.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.CmbAgency.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbAgency.Location = new System.Drawing.Point(4, 6);
            this.CmbAgency.Name = "CmbAgency";
            this.CmbAgency.Size = new System.Drawing.Size(233, 21);
            this.CmbAgency.TabIndex = 1;
            this.CmbAgency.SelectedIndexChanged += new System.EventHandler(this.CmbAgency_SelectedIndexChanged);
            // 
            // CmbDept
            // 
            this.CmbDept.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.CmbDept.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbDept.Location = new System.Drawing.Point(238, 6);
            this.CmbDept.Name = "CmbDept";
            this.CmbDept.Size = new System.Drawing.Size(233, 21);
            this.CmbDept.TabIndex = 2;
            this.CmbDept.SelectedIndexChanged += new System.EventHandler(this.CmbDept_SelectedIndexChanged);
            // 
            // CmbProg
            // 
            this.CmbProg.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.CmbProg.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbProg.Location = new System.Drawing.Point(472, 6);
            this.CmbProg.Name = "CmbProg";
            this.CmbProg.Size = new System.Drawing.Size(233, 21);
            this.CmbProg.TabIndex = 3;
            this.CmbProg.SelectedIndexChanged += new System.EventHandler(this.CmbProg_SelectedIndexChanged);
            // 
            // BtnDefHier
            // 
            this.BtnDefHier.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnDefHier.Location = new System.Drawing.Point(600, 29);
            this.BtnDefHier.Name = "BtnDefHier";
            this.BtnDefHier.Size = new System.Drawing.Size(163, 27);
            this.BtnDefHier.TabIndex = 16;
            this.BtnDefHier.Text = "Save as Default Hierarchy";
            this.BtnDefHier.Visible = false;
            this.BtnDefHier.Click += new System.EventHandler(this.BtnDefHier_Click);
            // 
            // CmbYear
            // 
            this.CmbYear.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.Location = new System.Drawing.Point(706, 6);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(54, 21);
            this.CmbYear.TabIndex = 19;
            this.CmbYear.SelectedIndexChanged += new System.EventHandler(this.CmbYear_SelectedIndexChanged);
            // 
            // AdvancedAgencypartnerSearch
            // 
            
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SeacrhPanel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(773, 461);
            this.Text = "Agency Partner -Advanced Search";
            this.Load += new System.EventHandler(this.AdvancedAgencypartnerSearch_Load);
            this.SeacrhPanel.ResumeLayout(false);
            this.NamePanel.ResumeLayout(false);
            this.PhonePanel.ResumeLayout(false);
            this.AppNoPanel.ResumeLayout(false);
            this.AddressPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GvwSearch)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button BtnSearcRecs;
        private ComboBox CmbSearchBy;
        private Label label1;
        private Panel SeacrhPanel;
        private Panel panel1;
        private DataGridView GvwSearch;
        private Button BtnSelAgency;
        private Panel panel2;
        private CheckBox CbSearch;
        private ComboBox CmbAgency;
        private ComboBox CmbDept;
        private ComboBox CmbProg;
        private Button BtnDefHier;
        private ComboBox CmbYear;
        private Button Btn_Curr_Hie;
        private Button Btn_Clear;
        private CheckBox Cb_MST_Prog;
        private DataGridViewTextBoxColumn gvtCode;
        private DataGridViewTextBoxColumn gvtAgyName;
        private DataGridViewTextBoxColumn gvtStreet;
        private DataGridViewTextBoxColumn gvtCity;
        private DataGridViewTextBoxColumn gvtState;
        private Panel NamePanel;
        private Label label4;
        private TextBox TxtAgencyName;
        private Panel PhonePanel;
        private MaskedTextBox MtxtPhone;
        private Panel AppNoPanel;
        private MaskedTextBox MtxtCode;
        private Panel AddressPanel;
        private TextBox TxtStreet;
        private Label label8;
        private Label label7;
        private TextBox TxtCity;
        private TextBox TxtState;
        private Label label9;
    }
}