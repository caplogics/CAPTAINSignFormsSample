using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class AgencyPartnerControl
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

        #region Wisej UserControl Designer generated code

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
            this.textBox2 = new Wisej.Web.TextBox();
            this.label3 = new Wisej.Web.Label();
            this.textBox3 = new Wisej.Web.TextBox();
            this.textBox11 = new Wisej.Web.TextBox();
            this.MainPanel = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.gvwAgencyData = new Wisej.Web.DataGridView();
            this.gvtAgyCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAgencyname = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtStreet = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCity = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtState = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.btnAgencyEnquiry = new Wisej.Web.Button();
            this.BtnP10 = new Wisej.Web.Button();
            this.BtnLast = new Wisej.Web.Button();
            this.BtnN10 = new Wisej.Web.Button();
            this.BtnNxt = new Wisej.Web.Button();
            this.BtnPrev = new Wisej.Web.Button();
            this.Btn_First = new Wisej.Web.Button();
            this.BtnAddApp = new Wisej.Web.Button();
            this.label11 = new Wisej.Web.Label();
            this.TxtAgencyCode = new TextBoxWithValidation();
            this.BtnSearchApp = new Wisej.Web.Button();
            this.BtnAdv_Search = new Wisej.Web.Button();
            this.MainPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAgencyData)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(608, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(171, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(317, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "First Name";
            this.label3.Visible = false;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(89, -4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(171, 20);
            this.textBox3.TabIndex = 3;
            this.textBox3.Visible = false;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(40, 2);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(178, 20);
            this.textBox11.TabIndex = 5;
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.panel3);
            this.MainPanel.Controls.Add(this.panel2);
            this.MainPanel.Location = new System.Drawing.Point(-1, 2);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(838, 507);
            this.MainPanel.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel3.Controls.Add(this.gvwAgencyData);
            this.panel3.Location = new System.Drawing.Point(2, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(834, 428);
            this.panel3.TabIndex = 17;
            // 
            // gvwAgencyData
            // 
            this.gvwAgencyData.AllowUserToAddRows = false;
            this.gvwAgencyData.AllowUserToDeleteRows = false;
            this.gvwAgencyData.BackColor = System.Drawing.Color.White;
            this.gvwAgencyData.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwAgencyData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwAgencyData.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwAgencyData.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtAgyCode,
            this.gvtAgencyname,
            this.gvtStreet,
            this.gvtCity,
            this.gvtState});
            this.gvwAgencyData.Location = new System.Drawing.Point(1, 6);
            this.gvwAgencyData.MultiSelect = false;
            this.gvwAgencyData.Name = "gvwAgencyData";
            this.gvwAgencyData.RowHeadersWidth = 15;
            this.gvwAgencyData.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwAgencyData.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.gvwAgencyData.RowTemplate.Height = 19;
            this.gvwAgencyData.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwAgencyData.SelectionMode = Wisej.Web.DataGridViewSelectionMode.FullRowSelect;
            this.gvwAgencyData.Size = new System.Drawing.Size(830, 364);
            this.gvwAgencyData.TabIndex = 9;
            this.gvwAgencyData.Click += new System.EventHandler(this.GvwAppHou_Click);
            // 
            // gvtAgyCode
            // 
            this.gvtAgyCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvtAgyCode.HeaderText = "Code";
            this.gvtAgyCode.Name = "gvtAgyCode";
            this.gvtAgyCode.Width = 60;
            // 
            // gvtAgencyname
            // 
            this.gvtAgencyname.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvtAgencyname.HeaderText = "Agency Name";
            this.gvtAgencyname.Name = "gvtAgencyname";
            this.gvtAgencyname.Width = 300;
            // 
            // gvtStreet
            // 
            this.gvtStreet.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvtStreet.HeaderText = "Street";
            this.gvtStreet.Name = "gvtStreet";
            this.gvtStreet.Width = 200;
            // 
            // gvtCity
            // 
            this.gvtCity.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvtCity.HeaderText = "City";
            this.gvtCity.Name = "gvtCity";
            this.gvtCity.Width = 180;
            // 
            // gvtState
            // 
            this.gvtState.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvtState.HeaderText = "State";
            this.gvtState.Name = "gvtState";
            this.gvtState.Width = 50;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel2.Controls.Add(this.btnAgencyEnquiry);
            this.panel2.Controls.Add(this.BtnP10);
            this.panel2.Controls.Add(this.BtnLast);
            this.panel2.Controls.Add(this.BtnN10);
            this.panel2.Controls.Add(this.BtnNxt);
            this.panel2.Controls.Add(this.BtnPrev);
            this.panel2.Controls.Add(this.Btn_First);
            this.panel2.Controls.Add(this.BtnAddApp);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.TxtAgencyCode);
            this.panel2.Controls.Add(this.BtnSearchApp);
            this.panel2.Controls.Add(this.BtnAdv_Search);
            this.panel2.Location = new System.Drawing.Point(2, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(834, 45);
            this.panel2.TabIndex = 22;
            // 
            // btnAgencyEnquiry
            // 
            this.btnAgencyEnquiry.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAgencyEnquiry.Location = new System.Drawing.Point(498, 8);
            this.btnAgencyEnquiry.Name = "btnAgencyEnquiry";
            this.btnAgencyEnquiry.Size = new System.Drawing.Size(99, 28);
            this.btnAgencyEnquiry.TabIndex = 21;
            this.btnAgencyEnquiry.Text = "Agency Inquiry";
            this.btnAgencyEnquiry.Visible = false;
            this.btnAgencyEnquiry.Click += new System.EventHandler(this.btnAgencyEnquiry_Click);
            // 
            // BtnP10
            // 
            this.BtnP10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnP10.Location = new System.Drawing.Point(637, 8);
            this.BtnP10.Name = "BtnP10";
            this.BtnP10.Size = new System.Drawing.Size(38, 28);
            this.BtnP10.TabIndex = 4;
            this.BtnP10.Text = "<<";
            this.controlToolTip.SetToolTip(this.BtnP10, "10 Recs Backward");
            this.BtnP10.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnLast
            // 
            this.BtnLast.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLast.Location = new System.Drawing.Point(789, 8);
            this.BtnLast.Name = "BtnLast";
            this.BtnLast.Size = new System.Drawing.Size(38, 28);
            this.BtnLast.TabIndex = 8;
            this.BtnLast.Text = ">|";
            this.controlToolTip.SetToolTip(this.BtnLast, "Last Record");
            this.BtnLast.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnN10
            // 
            this.BtnN10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnN10.Location = new System.Drawing.Point(751, 8);
            this.BtnN10.Name = "BtnN10";
            this.BtnN10.Size = new System.Drawing.Size(38, 28);
            this.BtnN10.TabIndex = 7;
            this.BtnN10.Text = ">>";
            this.controlToolTip.SetToolTip(this.BtnN10, "10 Recs Forward");
            this.BtnN10.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnNxt
            // 
            this.BtnNxt.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNxt.Location = new System.Drawing.Point(713, 8);
            this.BtnNxt.Name = "BtnNxt";
            this.BtnNxt.Size = new System.Drawing.Size(38, 28);
            this.BtnNxt.TabIndex = 6;
            this.BtnNxt.Text = ">";
            this.controlToolTip.SetToolTip(this.BtnNxt, "Next Record");
            this.BtnNxt.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnPrev
            // 
            this.BtnPrev.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrev.Location = new System.Drawing.Point(675, 8);
            this.BtnPrev.Name = "BtnPrev";
            this.BtnPrev.Size = new System.Drawing.Size(38, 28);
            this.BtnPrev.TabIndex = 5;
            this.BtnPrev.Text = "<";
            this.controlToolTip.SetToolTip(this.BtnPrev, "Previous Record");
            this.BtnPrev.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // Btn_First
            // 
            this.Btn_First.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_First.Location = new System.Drawing.Point(598, 8);
            this.Btn_First.Name = "Btn_First";
            this.Btn_First.Size = new System.Drawing.Size(39, 28);
            this.Btn_First.TabIndex = 3;
            this.Btn_First.Text = "|<";
            this.controlToolTip.SetToolTip(this.Btn_First, "First Record");
            this.Btn_First.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnAddApp
            // 
            this.BtnAddApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.BtnAddApp.Location = new System.Drawing.Point(381, 2);
            this.BtnAddApp.Name = "BtnAddApp";
            this.BtnAddApp.Size = new System.Drawing.Size(113, 33);
            this.BtnAddApp.TabIndex = 20;
            this.BtnAddApp.Text = "Add Applicant";
            this.BtnAddApp.Visible = false;
            this.BtnAddApp.Click += new System.EventHandler(this.BtnAddApp_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(1, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 28);
            this.label11.TabIndex = 19;
            this.label11.Text = "Agency Code #";
            // 
            // TxtAgencyCode
            // 
            this.TxtAgencyCode.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtAgencyCode.Location = new System.Drawing.Point(139, 8);
            this.TxtAgencyCode.MaxLength = 8;
            this.TxtAgencyCode.Name = "TxtAgencyCode";
            this.TxtAgencyCode.Size = new System.Drawing.Size(94, 25);
            this.TxtAgencyCode.TabIndex = 1;
            this.TxtAgencyCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            //this.TxtAgencyCode.KeyDown += new Wisej.Web.KeyEventHandler(this.TxtAgencyNo_EnterKeyDown);
            this.TxtAgencyCode.LostFocus += new System.EventHandler(this.TxtAgencyNo_LostFocus);
            
            // 
            // BtnSearchApp
            // 
            this.BtnSearchApp.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSearchApp.Location = new System.Drawing.Point(485, 21);
            this.BtnSearchApp.Name = "BtnSearchApp";
            this.BtnSearchApp.Size = new System.Drawing.Size(76, 22);
            this.BtnSearchApp.TabIndex = 2;
            this.BtnSearchApp.Text = "Search...";
            this.BtnSearchApp.Visible = false;
            this.BtnSearchApp.Click += new System.EventHandler(this.BtnSearchApp_Click);
            // 
            // BtnAdv_Search
            // 
            this.BtnAdv_Search.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdv_Search.Location = new System.Drawing.Point(234, 4);
            this.BtnAdv_Search.Name = "BtnAdv_Search";
            this.BtnAdv_Search.Size = new System.Drawing.Size(142, 33);
            this.BtnAdv_Search.TabIndex = 2;
            this.BtnAdv_Search.Text = "Advanced Search...";
            this.BtnAdv_Search.Click += new System.EventHandler(this.BtnAdv_Search_Click);
            // 
            // AgencyPartnerControl
            // 
            this.Controls.Add(this.MainPanel);
            this.Location = new System.Drawing.Point(0, -91);
            this.Size = new System.Drawing.Size(839, 509);
            this.Text = "AgencyPartnerControl";
            this.Load += new System.EventHandler(this.AgencyPartnerControl_Load);
            this.MainPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwAgencyData)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox textBox2;
        private Label label3;
        private TextBox textBox3;
        private TextBox textBox11;
        private Panel MainPanel;
        private Panel panel3;
        private DataGridView gvwAgencyData;
        private Panel panel2;
        private Label label11;
        private Button BtnSearchApp;
        private Button BtnAdv_Search;
        private Button BtnAddApp;
        private Button BtnP10;
        private Button BtnLast;
        private Button BtnN10;
        private Button BtnNxt;
        private Button BtnPrev;
        private Button Btn_First;
        public TextBoxWithValidation TxtAgencyCode;
        private Button btnAgencyEnquiry;
        private DataGridViewTextBoxColumn gvtAgyCode;
        private DataGridViewTextBoxColumn gvtAgencyname;
        private DataGridViewTextBoxColumn gvtStreet;
        private DataGridViewTextBoxColumn gvtCity;
        private DataGridViewTextBoxColumn gvtState;
    }
}