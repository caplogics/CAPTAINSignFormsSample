using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00134Popup
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

        #region Wisej  Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00134Popup));
            this.panel1 = new Wisej.Web.Panel();
            this.gvwEnrl = new Wisej.Web.DataGridView();
            this.gvtFund = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvEnrollDate = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSite = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtRoom = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAmpm = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnSelect = new Wisej.Web.Button();
            this.gvwTaskDetails = new Wisej.Web.DataGridView();
            this.gvcSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtYear = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcSBCB = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcAddressed = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcCompleted = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcFollowup = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcDiagnose = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcSeqList = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcTaskCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwEnrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwTaskDetails)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 232);
            this.panel1.TabIndex = 0;
            // 
            // gvwEnrl
            // 
            this.gvwEnrl.AllowUserToResizeColumns = false;
            this.gvwEnrl.BackColor = System.Drawing.Color.White;
            this.gvwEnrl.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwEnrl.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtFund,
            this.gvEnrollDate,
            this.gvtSite,
            this.gvtRoom,
            this.gvtAmpm});
            this.gvwEnrl.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwEnrl.Location = new System.Drawing.Point(0, 88);
            this.gvwEnrl.Name = "gvwEnrl";
            this.gvwEnrl.ReadOnly = true;
            this.gvwEnrl.RowHeadersWidth = 10;
            this.gvwEnrl.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwEnrl.Size = new System.Drawing.Size(623, 107);
            this.gvwEnrl.TabIndex = 4;
            // 
            // gvtFund
            // 
            this.gvtFund.HeaderText = "Fund";
            this.gvtFund.Name = "gvtFund";
            this.gvtFund.ReadOnly = true;
            // 
            // gvEnrollDate
            // 
            this.gvEnrollDate.HeaderText = "Enroll Dt";
            this.gvEnrollDate.Name = "gvEnrollDate";
            this.gvEnrollDate.ReadOnly = true;
            // 
            // gvtSite
            // 
            this.gvtSite.HeaderText = "Site";
            this.gvtSite.Name = "gvtSite";
            this.gvtSite.ReadOnly = true;
            this.gvtSite.Width = 60;
            // 
            // gvtRoom
            // 
            this.gvtRoom.HeaderText = "Room";
            this.gvtRoom.Name = "gvtRoom";
            this.gvtRoom.ReadOnly = true;
            this.gvtRoom.Width = 60;
            // 
            // gvtAmpm
            // 
            this.gvtAmpm.HeaderText = "Ampm";
            this.gvtAmpm.Name = "gvtAmpm";
            this.gvtAmpm.ReadOnly = true;
            this.gvtAmpm.Width = 50;
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(544, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 27);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // gvwTaskDetails
            // 
            this.gvwTaskDetails.AllowUserToResizeColumns = false;
            this.gvwTaskDetails.AllowUserToResizeRows = false;
            this.gvwTaskDetails.BackColor = System.Drawing.Color.White;
            this.gvwTaskDetails.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwTaskDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvcSeq,
            this.gvtYear,
            this.gvcSBCB,
            this.gvcAddressed,
            this.gvcCompleted,
            this.gvcFollowup,
            this.gvcDiagnose,
            this.gvcSeqList,
            this.gvcTaskCode});
            this.gvwTaskDetails.Dock = Wisej.Web.DockStyle.Top;
            this.gvwTaskDetails.Location = new System.Drawing.Point(0, 0);
            this.gvwTaskDetails.MultiSelect = false;
            this.gvwTaskDetails.Name = "gvwTaskDetails";
            this.gvwTaskDetails.RowHeadersWidth = 14;
            this.gvwTaskDetails.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwTaskDetails.Size = new System.Drawing.Size(623, 88);
            this.gvwTaskDetails.TabIndex = 2;
            // 
            // gvcSeq
            // 
            this.gvcSeq.HeaderText = "Seq.";
            this.gvcSeq.Name = "gvcSeq";
            this.gvcSeq.ReadOnly = true;
            this.gvcSeq.Visible = false;
            this.gvcSeq.Width = 10;
            // 
            // gvtYear
            // 
            this.gvtYear.HeaderText = "Year";
            this.gvtYear.Name = "gvtYear";
            this.gvtYear.ReadOnly = true;
            this.gvtYear.Width = 40;
            // 
            // gvcSBCB
            // 
            this.gvcSBCB.HeaderText = "SBCB";
            this.gvcSBCB.Name = "gvcSBCB";
            this.gvcSBCB.ReadOnly = true;
            this.gvcSBCB.Width = 60;
            // 
            // gvcAddressed
            // 
            this.gvcAddressed.HeaderText = "Addressed";
            this.gvcAddressed.Name = "gvcAddressed";
            this.gvcAddressed.ReadOnly = true;
            this.gvcAddressed.Width = 65;
            // 
            // gvcCompleted
            // 
            this.gvcCompleted.HeaderText = "Completed";
            this.gvcCompleted.Name = "gvcCompleted";
            this.gvcCompleted.ReadOnly = true;
            this.gvcCompleted.Width = 65;
            // 
            // gvcFollowup
            // 
            this.gvcFollowup.HeaderText = "Followup";
            this.gvcFollowup.Name = "gvcFollowup";
            this.gvcFollowup.ReadOnly = true;
            this.gvcFollowup.Width = 65;
            // 
            // gvcDiagnose
            // 
            this.gvcDiagnose.HeaderText = "Diagnose";
            this.gvcDiagnose.Name = "gvcDiagnose";
            this.gvcDiagnose.ReadOnly = true;
            this.gvcDiagnose.Width = 65;
            // 
            // gvcSeqList
            // 
            this.gvcSeqList.Name = "gvcSeqList";
            this.gvcSeqList.ReadOnly = true;
            this.gvcSeqList.Visible = false;
            this.gvcSeqList.Width = 10;
            // 
            // gvcTaskCode
            // 
            this.gvcTaskCode.HeaderText = "TaskCode";
            this.gvcTaskCode.Name = "gvcTaskCode";
            this.gvcTaskCode.ReadOnly = true;
            this.gvcTaskCode.Visible = false;
            this.gvcTaskCode.Width = 50;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwEnrl);
            this.panel2.Controls.Add(this.gvwTaskDetails);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(623, 195);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 195);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(4);
            this.panel3.Size = new System.Drawing.Size(623, 35);
            this.panel3.TabIndex = 6;
            // 
            // HSS00134Popup
            // 
            this.ClientSize = new System.Drawing.Size(625, 232);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00134Popup";
            this.Text = "HSS00134 - Popup";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwEnrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwTaskDetails)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Button btnSelect;
        private DataGridView gvwTaskDetails;
        private DataGridViewTextBoxColumn gvcSeq;
        private DataGridViewTextBoxColumn gvcSBCB;
        private DataGridViewTextBoxColumn gvcAddressed;
        private DataGridViewTextBoxColumn gvcCompleted;
        private DataGridViewTextBoxColumn gvcFollowup;
        private DataGridViewTextBoxColumn gvcDiagnose;
        private DataGridViewTextBoxColumn gvcSeqList;
        private DataGridViewTextBoxColumn gvcTaskCode;
        private DataGridView gvwEnrl;
        private DataGridViewTextBoxColumn gvtFund;
        private DataGridViewTextBoxColumn gvtSite;
        private DataGridViewTextBoxColumn gvtRoom;
        private DataGridViewTextBoxColumn gvtAmpm;
        private DataGridViewTextBoxColumn gvEnrollDate;
        private DataGridViewTextBoxColumn gvtYear;
        private Panel panel2;
        private Panel panel3;
    }
}