using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00134LoadCriteria
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00134LoadCriteria));
            this.panel1 = new Wisej.Web.Panel();
            this.pnlFastload = new Wisej.Web.Panel();
            this.rdoAllYear = new Wisej.Web.RadioButton();
            this.rdoThisYear = new Wisej.Web.RadioButton();
            this.rdoPrivious = new Wisej.Web.RadioButton();
            this.rdoTaskSeq = new Wisej.Web.RadioButton();
            this.rdoTaksDesc = new Wisej.Web.RadioButton();
            this.rdoTaskCode = new Wisej.Web.RadioButton();
            this.lblLoadData = new Wisej.Web.Label();
            this.lblTaskSeq = new Wisej.Web.Label();
            this.chkCalculateSBCB = new Wisej.Web.CheckBox();
            this.chkFastLoad = new Wisej.Web.CheckBox();
            this.panel2 = new Wisej.Web.Panel();
            this.btnAdd = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel1.SuspendLayout();
            this.pnlFastload.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.pnlFastload);
            this.panel1.Controls.Add(this.rdoTaskSeq);
            this.panel1.Controls.Add(this.rdoTaksDesc);
            this.panel1.Controls.Add(this.rdoTaskCode);
            this.panel1.Controls.Add(this.lblLoadData);
            this.panel1.Controls.Add(this.lblTaskSeq);
            this.panel1.Controls.Add(this.chkCalculateSBCB);
            this.panel1.Controls.Add(this.chkFastLoad);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 239);
            this.panel1.TabIndex = 0;
            // 
            // pnlFastload
            // 
            this.pnlFastload.Controls.Add(this.rdoAllYear);
            this.pnlFastload.Controls.Add(this.rdoPrivious);
            this.pnlFastload.Controls.Add(this.rdoThisYear);
            this.pnlFastload.Location = new System.Drawing.Point(136, 6);
            this.pnlFastload.Name = "pnlFastload";
            this.pnlFastload.Size = new System.Drawing.Size(235, 67);
            this.pnlFastload.TabIndex = 5;
            // 
            // rdoAllYear
            // 
            this.rdoAllYear.Dock = Wisej.Web.DockStyle.Top;
            this.rdoAllYear.Location = new System.Drawing.Point(0, 44);
            this.rdoAllYear.Name = "rdoAllYear";
            this.rdoAllYear.Size = new System.Drawing.Size(235, 21);
            this.rdoAllYear.TabIndex = 5;
            this.rdoAllYear.Text = "All Years";
            // 
            // rdoThisYear
            // 
            this.rdoThisYear.AutoSize = false;
            this.rdoThisYear.Dock = Wisej.Web.DockStyle.Top;
            this.rdoThisYear.Location = new System.Drawing.Point(0, 0);
            this.rdoThisYear.Name = "rdoThisYear";
            this.rdoThisYear.Size = new System.Drawing.Size(235, 23);
            this.rdoThisYear.TabIndex = 3;
            this.rdoThisYear.Text = "This Program Year Only";
            // 
            // rdoPrivious
            // 
            this.rdoPrivious.AutoSize = false;
            this.rdoPrivious.Dock = Wisej.Web.DockStyle.Top;
            this.rdoPrivious.Location = new System.Drawing.Point(0, 23);
            this.rdoPrivious.Name = "rdoPrivious";
            this.rdoPrivious.Size = new System.Drawing.Size(235, 21);
            this.rdoPrivious.TabIndex = 4;
            this.rdoPrivious.Text = "Prior Program Years Only";
            // 
            // rdoTaskSeq
            // 
            this.rdoTaskSeq.Location = new System.Drawing.Point(117, 173);
            this.rdoTaskSeq.Name = "rdoTaskSeq";
            this.rdoTaskSeq.Size = new System.Drawing.Size(83, 21);
            this.rdoTaskSeq.TabIndex = 8;
            this.rdoTaskSeq.Text = "Task Seq";
            this.rdoTaskSeq.Visible = false;
            // 
            // rdoTaksDesc
            // 
            this.rdoTaksDesc.Location = new System.Drawing.Point(117, 146);
            this.rdoTaksDesc.Name = "rdoTaksDesc";
            this.rdoTaksDesc.Size = new System.Drawing.Size(122, 21);
            this.rdoTaksDesc.TabIndex = 7;
            this.rdoTaksDesc.Text = "Task Description";
            this.rdoTaksDesc.Visible = false;
            // 
            // rdoTaskCode
            // 
            this.rdoTaskCode.Location = new System.Drawing.Point(117, 119);
            this.rdoTaskCode.Name = "rdoTaskCode";
            this.rdoTaskCode.Size = new System.Drawing.Size(90, 21);
            this.rdoTaskCode.TabIndex = 6;
            this.rdoTaskCode.Text = "Task Code";
            this.rdoTaskCode.Visible = false;
            // 
            // lblLoadData
            // 
            this.lblLoadData.AutoSize = true;
            this.lblLoadData.Location = new System.Drawing.Point(5, 42);
            this.lblLoadData.MinimumSize = new System.Drawing.Size(125, 18);
            this.lblLoadData.Name = "lblLoadData";
            this.lblLoadData.Size = new System.Drawing.Size(125, 18);
            this.lblLoadData.TabIndex = 3;
            this.lblLoadData.Text = "Load Data Posted for";
            // 
            // lblTaskSeq
            // 
            this.lblTaskSeq.AutoSize = true;
            this.lblTaskSeq.Location = new System.Drawing.Point(13, 121);
            this.lblTaskSeq.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblTaskSeq.Name = "lblTaskSeq";
            this.lblTaskSeq.Size = new System.Drawing.Size(88, 18);
            this.lblTaskSeq.TabIndex = 2;
            this.lblTaskSeq.Text = "Task Sequence";
            this.lblTaskSeq.Visible = false;
            // 
            // chkCalculateSBCB
            // 
            this.chkCalculateSBCB.Location = new System.Drawing.Point(8, 92);
            this.chkCalculateSBCB.Name = "chkCalculateSBCB";
            this.chkCalculateSBCB.Size = new System.Drawing.Size(153, 21);
            this.chkCalculateSBCB.TabIndex = 2;
            this.chkCalculateSBCB.Text = "Calculate SBCB Dates";
            // 
            // chkFastLoad
            // 
            this.chkFastLoad.Location = new System.Drawing.Point(8, 6);
            this.chkFastLoad.Name = "chkFastLoad";
            this.chkFastLoad.Size = new System.Drawing.Size(86, 21);
            this.chkFastLoad.TabIndex = 1;
            this.chkFastLoad.Text = "Fast Load";
            this.chkFastLoad.CheckedChanged += new System.EventHandler(this.chkFastLoad_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 202);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(4);
            this.panel2.Size = new System.Drawing.Size(388, 35);
            this.panel2.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.AppearanceKey = "button-ok";
            this.btnAdd.Dock = Wisej.Web.DockStyle.Right;
            this.btnAdd.Location = new System.Drawing.Point(216, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(108, 27);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "&Save as Default";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(329, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 27);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(324, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // HSS00134LoadCriteria
            // 
            this.ClientSize = new System.Drawing.Size(390, 239);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00134LoadCriteria";
            this.Text = "HSS00134LoadCriteria";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlFastload.ResumeLayout(false);
            this.pnlFastload.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private RadioButton rdoTaskSeq;
        private RadioButton rdoTaksDesc;
        private RadioButton rdoTaskCode;
        private Label lblLoadData;
        private Label lblTaskSeq;
        private CheckBox chkCalculateSBCB;
        private CheckBox chkFastLoad;
        private Panel panel2;
        private Button btnAdd;
        private Button btnCancel;
        private Panel pnlFastload;
        private RadioButton rdoAllYear;
        private RadioButton rdoThisYear;
        private RadioButton rdoPrivious;
        private Spacer spacer1;
    }
}