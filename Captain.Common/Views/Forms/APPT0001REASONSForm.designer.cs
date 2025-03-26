using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class APPT0001REASONSForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APPT0001REASONSForm));
            this.panel1 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.gvwReasons = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvtDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvtReason = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gviDel = new Wisej.Web.DataGridViewImageColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.txtReasons = new Wisej.Web.TextBox();
            this.lblReasonReq = new Wisej.Web.Label();
            this.btnCancel = new Wisej.Web.Button();
            this.lblDtReq = new Wisej.Web.Label();
            this.btnSave = new Wisej.Web.Button();
            this.dtDate = new Wisej.Web.DateTimePicker();
            this.lblDate = new Wisej.Web.Label();
            this.lblReason = new Wisej.Web.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwReasons)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(473, 365);
            this.panel1.TabIndex = 0;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwReasons);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(473, 288);
            this.panel3.TabIndex = 0;
            // 
            // gvwReasons
            // 
            this.gvwReasons.AllowUserToResizeColumns = false;
            this.gvwReasons.AllowUserToResizeRows = false;
            this.gvwReasons.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwReasons.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwReasons.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwReasons.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwReasons.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwReasons.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtDate,
            this.gvtReason,
            this.gviDel});
            this.gvwReasons.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwReasons.Name = "gvwReasons";
            this.gvwReasons.RowHeadersWidth = 14;
            this.gvwReasons.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwReasons.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwReasons.Size = new System.Drawing.Size(473, 288);
            this.gvwReasons.TabIndex = 0;
            this.gvwReasons.TabStop = false;
            this.gvwReasons.SelectionChanged += new System.EventHandler(this.gvwReasons_SelectionChanged);
            this.gvwReasons.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwReasons_CellClick);
            // 
            // gvtDate
            // 
            this.gvtDate.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtDate.HeaderStyle = dataGridViewCellStyle3;
            this.gvtDate.HeaderText = "Date";
            this.gvtDate.Name = "gvtDate";
            // 
            // gvtReason
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtReason.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtReason.HeaderStyle = dataGridViewCellStyle5;
            this.gvtReason.HeaderText = "Reason";
            this.gvtReason.Name = "gvtReason";
            this.gvtReason.Width = 270;
            // 
            // gviDel
            // 
            this.gviDel.CellImageSource = "captain-delete";
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = null;
            this.gviDel.DefaultCellStyle = dataGridViewCellStyle6;
            this.gviDel.HeaderText = "Delete";
            this.gviDel.Name = "gviDel";
            this.gviDel.ShowInVisibilityMenu = false;
            this.gviDel.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gviDel.Width = 60;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.txtReasons);
            this.panel2.Controls.Add(this.lblReasonReq);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.lblDtReq);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.dtDate);
            this.panel2.Controls.Add(this.lblDate);
            this.panel2.Controls.Add(this.lblReason);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 288);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(473, 77);
            this.panel2.TabIndex = 1;
            // 
            // txtReasons
            // 
            this.txtReasons.Enabled = false;
            this.txtReasons.Location = new System.Drawing.Point(72, 41);
            this.txtReasons.MaxLength = 25;
            this.txtReasons.Name = "txtReasons";
            this.txtReasons.Size = new System.Drawing.Size(177, 25);
            this.txtReasons.TabIndex = 2;
            // 
            // lblReasonReq
            // 
            this.lblReasonReq.AutoSize = true;
            this.lblReasonReq.ForeColor = System.Drawing.Color.Red;
            this.lblReasonReq.Location = new System.Drawing.Point(56, 40);
            this.lblReasonReq.Name = "lblReasonReq";
            this.lblReasonReq.Size = new System.Drawing.Size(9, 14);
            this.lblReasonReq.TabIndex = 0;
            this.lblReasonReq.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Location = new System.Drawing.Point(382, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDtReq
            // 
            this.lblDtReq.AutoSize = true;
            this.lblDtReq.ForeColor = System.Drawing.Color.Red;
            this.lblDtReq.Location = new System.Drawing.Point(42, 13);
            this.lblDtReq.Name = "lblDtReq";
            this.lblDtReq.Size = new System.Drawing.Size(9, 14);
            this.lblDtReq.TabIndex = 0;
            this.lblDtReq.Text = "*";
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Location = new System.Drawing.Point(305, 41);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dtDate
            // 
            this.dtDate.AutoSize = false;
            this.dtDate.CustomFormat = "MM/dd/yyyy";
            this.dtDate.Enabled = false;
            this.dtDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(72, 10);
            this.dtDate.Name = "dtDate";
            this.dtDate.ShowCheckBox = true;
            this.dtDate.ShowToolTips = false;
            this.dtDate.Size = new System.Drawing.Size(116, 25);
            this.dtDate.TabIndex = 1;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(15, 15);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(30, 14);
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "Date";
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(15, 45);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(46, 14);
            this.lblReason.TabIndex = 5;
            this.lblReason.Text = "Reason";
            // 
            // APPT0001REASONSForm
            // 
            this.ClientSize = new System.Drawing.Size(473, 365);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "APPT0001REASONSForm";
            this.Text = "Closed - Reasons";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwReasons)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private Panel panel1;
        private DataGridViewEx gvwReasons;
        private Label lblReason;
        private Label lblDate;
        private DateTimePicker dtDate;
        private Label lblDtReq;
        private TextBox txtReasons;
        private Label lblReasonReq;
        private Button btnCancel;
        private Button btnSave;
        private DataGridViewTextBoxColumn gvtReason;
        private DataGridViewImageColumn gviDel;
        private Panel panel3;
        private Panel panel2;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvtDate;
    }
}