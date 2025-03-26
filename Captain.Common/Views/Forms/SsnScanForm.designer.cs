using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class SsnScanForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SsnScanForm));
            this.btnSelect = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.gvwssnscansearch = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.ApplicantNumber = new Wisej.Web.DataGridViewTextBoxColumn();
            this.NameOftheHHMember = new Wisej.Web.DataGridViewTextBoxColumn();
            this.DOB = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.SsnNo = new Wisej.Web.DataGridViewTextBoxColumn();
            this.MemSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnSSNSelect = new Wisej.Web.Button();
            this.pnlgvwssnscansearch = new Wisej.Web.Panel();
            this.LastUpdate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvwssnscansearch)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlgvwssnscansearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(351, 385);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "&Select";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 55);
            this.panel1.TabIndex = 2;
            this.panel1.TabStop = true;
            // 
            // gvwssnscansearch
            // 
            this.gvwssnscansearch.AllowUserToResizeColumns = false;
            this.gvwssnscansearch.AllowUserToResizeRows = false;
            this.gvwssnscansearch.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwssnscansearch.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwssnscansearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwssnscansearch.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwssnscansearch.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.ApplicantNumber,
            this.NameOftheHHMember,
            this.DOB,
            this.LastUpdate,
            this.SsnNo,
            this.MemSeq});
            this.gvwssnscansearch.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwssnscansearch.MultiSelect = false;
            this.gvwssnscansearch.Name = "gvwssnscansearch";
            this.gvwssnscansearch.ReadOnly = true;
            this.gvwssnscansearch.RowHeadersWidth = 20;
            this.gvwssnscansearch.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwssnscansearch.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwssnscansearch.Size = new System.Drawing.Size(599, 318);
            this.gvwssnscansearch.TabIndex = 88;
            this.gvwssnscansearch.TabStop = false;
            // 
            // ApplicantNumber
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.ApplicantNumber.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ApplicantNumber.HeaderStyle = dataGridViewCellStyle3;
            this.ApplicantNumber.HeaderText = "Applicant Number";
            this.ApplicantNumber.Name = "ApplicantNumber";
            this.ApplicantNumber.ReadOnly = true;
            this.ApplicantNumber.Width = 160;
            // 
            // NameOftheHHMember
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.NameOftheHHMember.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.NameOftheHHMember.HeaderStyle = dataGridViewCellStyle5;
            this.NameOftheHHMember.HeaderText = "Name Of The HH Member";
            this.NameOftheHHMember.Name = "NameOftheHHMember";
            this.NameOftheHHMember.ReadOnly = true;
            this.NameOftheHHMember.Width = 200;
            // 
            // DOB
            // 
            this.DOB.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.DOB.HeaderStyle = dataGridViewCellStyle7;
            this.DOB.Name = "DOB";
            this.DOB.ReadOnly = true;
            // 
            // SsnNo
            // 
            this.SsnNo.HeaderText = "SsnNo";
            this.SsnNo.Name = "SsnNo";
            this.SsnNo.ReadOnly = true;
            this.SsnNo.ShowInVisibilityMenu = false;
            this.SsnNo.Visible = false;
            // 
            // MemSeq
            // 
            this.MemSeq.HeaderText = "MemSeq";
            this.MemSeq.Name = "MemSeq";
            this.MemSeq.ReadOnly = true;
            this.MemSeq.ShowInVisibilityMenu = false;
            this.MemSeq.Visible = false;
            this.MemSeq.Width = 10;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnSSNSelect);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 318);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(0, 3, 15, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(599, 35);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnSSNSelect
            // 
            this.btnSSNSelect.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSSNSelect.Location = new System.Drawing.Point(506, 6);
            this.btnSSNSelect.Name = "btnSSNSelect";
            this.btnSSNSelect.Size = new System.Drawing.Size(75, 25);
            this.btnSSNSelect.TabIndex = 1;
            this.btnSSNSelect.Text = "&Select";
            this.btnSSNSelect.Click += new System.EventHandler(this.btnSSNSelect_Click);
            // 
            // pnlgvwssnscansearch
            // 
            this.pnlgvwssnscansearch.Controls.Add(this.gvwssnscansearch);
            this.pnlgvwssnscansearch.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwssnscansearch.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwssnscansearch.Name = "pnlgvwssnscansearch";
            this.pnlgvwssnscansearch.Size = new System.Drawing.Size(599, 318);
            this.pnlgvwssnscansearch.TabIndex = 99;
            // 
            // LastUpdate
            // 
            this.LastUpdate.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.LastUpdate.HeaderStyle = dataGridViewCellStyle9;
            this.LastUpdate.HeaderText = "Last Update";
            this.LastUpdate.Name = "LastUpdate";
            this.LastUpdate.ReadOnly = true;
            // 
            // SsnScanForm
            // 
            this.ClientSize = new System.Drawing.Size(599, 353);
            this.Controls.Add(this.pnlgvwssnscansearch);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SsnScanForm";
            this.Text = "SSN Scan Search";
            ((System.ComponentModel.ISupportInitialize)(this.gvwssnscansearch)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlgvwssnscansearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnSelect;
        private Panel panel1;
        private DataGridViewEx gvwssnscansearch;
        private DataGridViewTextBoxColumn ApplicantNumber;
        private DataGridViewTextBoxColumn NameOftheHHMember;
        private DataGridViewTextBoxColumn SsnNo;
        private DataGridViewTextBoxColumn MemSeq;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnSSNSelect;
        private Controls.Compatibility.DataGridViewDateTimeColumn DOB;
        private Panel pnlgvwssnscansearch;
        private DataGridViewDateTimeColumn LastUpdate;
    }
}