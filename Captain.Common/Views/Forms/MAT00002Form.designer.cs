using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class MAT00002Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAT00002Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.btnSave = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.txtGroupDesc = new Wisej.Web.TextBox();
            this.lblIntervalReq = new Wisej.Web.Label();
            this.lblResponce = new Wisej.Web.Label();
            this.txtGroupCode = new Wisej.Web.TextBox();
            this.lblGroupCode = new Wisej.Web.Label();
            this.lblGroupDesc = new Wisej.Web.Label();
            this.pnlGroup = new Wisej.Web.Panel();
            this.cmbResponse = new Wisej.Web.ComboBox();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlGroup.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(347, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(425, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtGroupDesc
            // 
            this.txtGroupDesc.Location = new System.Drawing.Point(80, 43);
            this.txtGroupDesc.MaxLength = 100;
            this.txtGroupDesc.Multiline = true;
            this.txtGroupDesc.Name = "txtGroupDesc";
            this.txtGroupDesc.Size = new System.Drawing.Size(404, 62);
            this.txtGroupDesc.TabIndex = 4;
            // 
            // lblIntervalReq
            // 
            this.lblIntervalReq.ForeColor = System.Drawing.Color.Red;
            this.lblIntervalReq.Location = new System.Drawing.Point(6, 112);
            this.lblIntervalReq.Name = "lblIntervalReq";
            this.lblIntervalReq.Size = new System.Drawing.Size(8, 10);
            this.lblIntervalReq.TabIndex = 28;
            this.lblIntervalReq.Text = "*";
            this.lblIntervalReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblResponce
            // 
            this.lblResponce.Location = new System.Drawing.Point(15, 116);
            this.lblResponce.Name = "lblResponce";
            this.lblResponce.Size = new System.Drawing.Size(55, 16);
            this.lblResponce.TabIndex = 3;
            this.lblResponce.Text = "Response";
            // 
            // txtGroupCode
            // 
            this.txtGroupCode.Enabled = false;
            this.txtGroupCode.Location = new System.Drawing.Point(80, 11);
            this.txtGroupCode.Name = "txtGroupCode";
            this.txtGroupCode.ReadOnly = true;
            this.txtGroupCode.Size = new System.Drawing.Size(404, 25);
            this.txtGroupCode.TabIndex = 3;
            this.txtGroupCode.TabStop = false;
            // 
            // lblGroupCode
            // 
            this.lblGroupCode.Location = new System.Drawing.Point(15, 15);
            this.lblGroupCode.Name = "lblGroupCode";
            this.lblGroupCode.Size = new System.Drawing.Size(37, 16);
            this.lblGroupCode.TabIndex = 1;
            this.lblGroupCode.Text = "Group";
            // 
            // lblGroupDesc
            // 
            this.lblGroupDesc.Location = new System.Drawing.Point(15, 66);
            this.lblGroupDesc.Name = "lblGroupDesc";
            this.lblGroupDesc.Size = new System.Drawing.Size(52, 16);
            this.lblGroupDesc.TabIndex = 1;
            this.lblGroupDesc.Text = "Question";
            // 
            // pnlGroup
            // 
            this.pnlGroup.Controls.Add(this.pnlSave);
            this.pnlGroup.Controls.Add(this.cmbResponse);
            this.pnlGroup.Controls.Add(this.lblIntervalReq);
            this.pnlGroup.Controls.Add(this.lblResponce);
            this.pnlGroup.Controls.Add(this.lblGroupCode);
            this.pnlGroup.Controls.Add(this.lblGroupDesc);
            this.pnlGroup.Controls.Add(this.txtGroupDesc);
            this.pnlGroup.Controls.Add(this.txtGroupCode);
            this.pnlGroup.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGroup.Location = new System.Drawing.Point(0, 0);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(515, 180);
            this.pnlGroup.TabIndex = 1;
            // 
            // cmbResponse
            // 
            this.cmbResponse.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbResponse.FormattingEnabled = true;
            this.cmbResponse.Location = new System.Drawing.Point(80, 112);
            this.cmbResponse.Name = "cmbResponse";
            this.cmbResponse.Size = new System.Drawing.Size(404, 25);
            this.cmbResponse.TabIndex = 29;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 145);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(515, 35);
            this.pnlSave.TabIndex = 30;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(422, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // MAT00002Form
            // 
            this.ClientSize = new System.Drawing.Size(515, 180);
            this.Controls.Add(this.pnlGroup);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MAT00002Form";
            this.Text = "Matrix/Scale Score Sheets";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnSave;
        private Button btnCancel;
        private TextBox txtGroupDesc;
        private Label lblIntervalReq;
        private Label lblResponce;
        private TextBox txtGroupCode;
        private Label lblGroupCode;
        private Label lblGroupDesc;
        private Panel pnlGroup;
        private ComboBox cmbResponse;
        private Panel pnlSave;
        private Spacer spacer1;
    }
}