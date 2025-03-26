using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class EnrollHierarchiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnrollHierarchiesForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.label3 = new Wisej.Web.Label();
            this.lblIntervalReq = new Wisej.Web.Label();
            this.txtHierachydesc = new Wisej.Web.TextBox();
            this.txtHierarchy = new Wisej.Web.TextBox();
            this.lblHierarchy = new Wisej.Web.Label();
            this.PbHierarchies = new Wisej.Web.PictureBox();
            this.lblNofoSlots = new Wisej.Web.Label();
            this.pnlGroup = new Wisej.Web.Panel();
            this.label2 = new Wisej.Web.Label();
            this.dtEndDate = new Wisej.Web.DateTimePicker();
            this.lblEndDate = new Wisej.Web.Label();
            this.lblStartDate = new Wisej.Web.Label();
            this.DOBReq = new Wisej.Web.Label();
            this.dtstartDate = new Wisej.Web.DateTimePicker();
            this.txtNofoSlots = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.PbHierarchies)).BeginInit();
            this.pnlGroup.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(95, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 14);
            this.label3.TabIndex = 28;
            this.label3.Text = "*";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblIntervalReq
            // 
            this.lblIntervalReq.AutoSize = true;
            this.lblIntervalReq.ForeColor = System.Drawing.Color.Red;
            this.lblIntervalReq.Location = new System.Drawing.Point(95, 12);
            this.lblIntervalReq.Name = "lblIntervalReq";
            this.lblIntervalReq.Size = new System.Drawing.Size(9, 14);
            this.lblIntervalReq.TabIndex = 28;
            this.lblIntervalReq.Text = "*";
            this.lblIntervalReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtHierachydesc
            // 
            this.txtHierachydesc.Enabled = false;
            this.txtHierachydesc.Location = new System.Drawing.Point(194, 10);
            this.txtHierachydesc.MaxLength = 100;
            this.txtHierachydesc.Name = "txtHierachydesc";
            this.txtHierachydesc.ReadOnly = true;
            this.txtHierachydesc.Size = new System.Drawing.Size(199, 25);
            this.txtHierachydesc.TabIndex = 2;
            this.txtHierachydesc.TabStop = false;
            // 
            // txtHierarchy
            // 
            this.txtHierarchy.Enabled = false;
            this.txtHierarchy.Location = new System.Drawing.Point(123, 10);
            this.txtHierarchy.MaxLength = 100;
            this.txtHierarchy.Name = "txtHierarchy";
            this.txtHierarchy.ReadOnly = true;
            this.txtHierarchy.Size = new System.Drawing.Size(64, 25);
            this.txtHierarchy.TabIndex = 1;
            this.txtHierarchy.TabStop = false;
            // 
            // lblHierarchy
            // 
            this.lblHierarchy.AutoSize = true;
            this.lblHierarchy.Location = new System.Drawing.Point(40, 14);
            this.lblHierarchy.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblHierarchy.Name = "lblHierarchy";
            this.lblHierarchy.Size = new System.Drawing.Size(56, 18);
            this.lblHierarchy.TabIndex = 3;
            this.lblHierarchy.Text = "Hierarchy";
            // 
            // PbHierarchies
            // 
            this.PbHierarchies.Cursor = Wisej.Web.Cursors.Hand;
            this.PbHierarchies.ImageSource = "captain-filter";
            this.PbHierarchies.Location = new System.Drawing.Point(418, 12);
            this.PbHierarchies.Name = "PbHierarchies";
            this.PbHierarchies.Size = new System.Drawing.Size(18, 20);
            this.PbHierarchies.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.PbHierarchies.Click += new System.EventHandler(this.PbHierarchies_Click);
            // 
            // lblNofoSlots
            // 
            this.lblNofoSlots.AutoSize = true;
            this.lblNofoSlots.Location = new System.Drawing.Point(21, 44);
            this.lblNofoSlots.Name = "lblNofoSlots";
            this.lblNofoSlots.Size = new System.Drawing.Size(75, 14);
            this.lblNofoSlots.TabIndex = 1;
            this.lblNofoSlots.Text = "Funded Slots";
            // 
            // pnlGroup
            // 
            this.pnlGroup.Controls.Add(this.label2);
            this.pnlGroup.Controls.Add(this.dtEndDate);
            this.pnlGroup.Controls.Add(this.lblEndDate);
            this.pnlGroup.Controls.Add(this.lblStartDate);
            this.pnlGroup.Controls.Add(this.DOBReq);
            this.pnlGroup.Controls.Add(this.dtstartDate);
            this.pnlGroup.Controls.Add(this.label3);
            this.pnlGroup.Controls.Add(this.lblIntervalReq);
            this.pnlGroup.Controls.Add(this.txtHierachydesc);
            this.pnlGroup.Controls.Add(this.txtHierarchy);
            this.pnlGroup.Controls.Add(this.lblHierarchy);
            this.pnlGroup.Controls.Add(this.PbHierarchies);
            this.pnlGroup.Controls.Add(this.lblNofoSlots);
            this.pnlGroup.Controls.Add(this.txtNofoSlots);
            this.pnlGroup.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGroup.Location = new System.Drawing.Point(0, 0);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(450, 130);
            this.pnlGroup.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(95, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 14);
            this.label2.TabIndex = 28;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dtEndDate
            // 
            this.dtEndDate.Checked = false;
            this.dtEndDate.CustomFormat = "MM/dd/yyyy";
            this.dtEndDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtEndDate.Location = new System.Drawing.Point(123, 100);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.ShowCheckBox = true;
            this.dtEndDate.ShowToolTips = false;
            this.dtEndDate.Size = new System.Drawing.Size(116, 22);
            this.dtEndDate.TabIndex = 5;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(42, 102);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(54, 14);
            this.lblEndDate.TabIndex = 0;
            this.lblEndDate.Text = "End Date";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(38, 73);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(58, 14);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Start Date";
            // 
            // DOBReq
            // 
            this.DOBReq.AutoSize = true;
            this.DOBReq.ForeColor = System.Drawing.Color.Red;
            this.DOBReq.Location = new System.Drawing.Point(95, 67);
            this.DOBReq.Name = "DOBReq";
            this.DOBReq.Size = new System.Drawing.Size(9, 14);
            this.DOBReq.TabIndex = 28;
            this.DOBReq.Text = "*";
            this.DOBReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dtstartDate
            // 
            this.dtstartDate.Checked = false;
            this.dtstartDate.CustomFormat = "MM/dd/yyyy";
            this.dtstartDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtstartDate.Location = new System.Drawing.Point(123, 72);
            this.dtstartDate.Name = "dtstartDate";
            this.dtstartDate.ShowCheckBox = true;
            this.dtstartDate.ShowToolTips = false;
            this.dtstartDate.Size = new System.Drawing.Size(116, 22);
            this.dtstartDate.TabIndex = 4;
            // 
            // txtNofoSlots
            // 
            this.txtNofoSlots.Location = new System.Drawing.Point(123, 41);
            this.txtNofoSlots.MaxLength = 6;
            this.txtNofoSlots.Name = "txtNofoSlots";
            this.txtNofoSlots.Size = new System.Drawing.Size(64, 25);
            this.txtNofoSlots.TabIndex = 3;
            this.txtNofoSlots.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(360, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(282, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 130);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(0, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(450, 35);
            this.pnlSave.TabIndex = 6;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(357, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // EnrollHierarchiesForm
            // 
            this.ClientSize = new System.Drawing.Size(450, 165);
            this.Controls.Add(this.pnlGroup);
            this.Controls.Add(this.pnlSave);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnrollHierarchiesForm";
            this.Text = "EnrollHierarchiesForm";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            ((System.ComponentModel.ISupportInitialize)(this.PbHierarchies)).EndInit();
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label label3;
        private Label lblIntervalReq;
        private TextBox txtHierachydesc;
        private TextBox txtHierarchy;
        private Label lblHierarchy;
        private PictureBox PbHierarchies;
        private Label lblNofoSlots;
        private TextBoxWithValidation txtNofoSlots;
        private Panel pnlGroup;
        private DateTimePicker dtEndDate;
        private Label lblEndDate;
        private Label lblStartDate;
        private Label DOBReq;
        private DateTimePicker dtstartDate;
        private Button btnCancel;
        private Button btnSave;
        private Panel pnlSave;
        private Label label2;
        private Spacer spacer1;
    }
}