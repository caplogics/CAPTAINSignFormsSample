using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class AddHierarchyForm
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

        #region WiseJ Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddHierarchyForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlName = new Wisej.Web.Panel();
            this.txtShortName = new Wisej.Web.TextBox();
            this.lblReqShortName = new Wisej.Web.Label();
            this.lblReqAgencyName = new Wisej.Web.Label();
            this.cbIntake = new Wisej.Web.CheckBox();
            this.lblReqCode = new Wisej.Web.Label();
            this.lblShortName = new Wisej.Web.Label();
            this.txtName = new Wisej.Web.TextBox();
            this.lblName = new Wisej.Web.Label();
            this.lblCode = new Wisej.Web.Label();
            this.cmbCaseWorkName = new Wisej.Web.ComboBox();
            this.cmbClientName = new Wisej.Web.ComboBox();
            this.cmbAgencyRep = new Wisej.Web.ComboBox();
            this.lblCWFormat = new Wisej.Web.Label();
            this.lblCNFormat = new Wisej.Web.Label();
            this.lblAgencyRep = new Wisej.Web.Label();
            this.btnOk = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlFormat = new Wisej.Web.Panel();
            this.txtCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.pnlName.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlName
            // 
            this.pnlName.BackColor = System.Drawing.Color.FromName("@window");
            this.pnlName.Controls.Add(this.txtShortName);
            this.pnlName.Controls.Add(this.lblReqShortName);
            this.pnlName.Controls.Add(this.lblReqAgencyName);
            this.pnlName.Controls.Add(this.cbIntake);
            this.pnlName.Controls.Add(this.lblReqCode);
            this.pnlName.Controls.Add(this.lblShortName);
            this.pnlName.Controls.Add(this.txtName);
            this.pnlName.Controls.Add(this.lblName);
            this.pnlName.Controls.Add(this.txtCode);
            this.pnlName.Controls.Add(this.lblCode);
            this.pnlName.Dock = Wisej.Web.DockStyle.Top;
            this.pnlName.Location = new System.Drawing.Point(0, 0);
            this.pnlName.Name = "pnlName";
            this.pnlName.Padding = new Wisej.Web.Padding(5);
            this.pnlName.Size = new System.Drawing.Size(448, 132);
            this.pnlName.TabIndex = 1;
            // 
            // txtShortName
            // 
            this.txtShortName.AutoSize = false;
            this.txtShortName.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtShortName.Location = new System.Drawing.Point(180, 88);
            this.txtShortName.MaxLength = 6;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(120, 25);
            this.txtShortName.TabIndex = 3;
            this.txtShortName.LostFocus += new System.EventHandler(this.CommonTextField_LostFocus);
            // 
            // lblReqShortName
            // 
            this.lblReqShortName.ForeColor = System.Drawing.Color.Red;
            this.lblReqShortName.Location = new System.Drawing.Point(81, 90);
            this.lblReqShortName.Name = "lblReqShortName";
            this.lblReqShortName.Size = new System.Drawing.Size(8, 10);
            this.lblReqShortName.TabIndex = 33;
            this.lblReqShortName.Text = "*";
            // 
            // lblReqAgencyName
            // 
            this.lblReqAgencyName.ForeColor = System.Drawing.Color.Red;
            this.lblReqAgencyName.Location = new System.Drawing.Point(90, 41);
            this.lblReqAgencyName.Name = "lblReqAgencyName";
            this.lblReqAgencyName.Size = new System.Drawing.Size(8, 10);
            this.lblReqAgencyName.TabIndex = 33;
            this.lblReqAgencyName.Text = "*";
            // 
            // cbIntake
            // 
            this.cbIntake.AutoSize = false;
            this.cbIntake.Location = new System.Drawing.Point(343, 90);
            this.cbIntake.Name = "cbIntake";
            this.cbIntake.Size = new System.Drawing.Size(82, 20);
            this.cbIntake.TabIndex = 4;
            this.cbIntake.Text = "Alt Intake";
            // 
            // lblReqCode
            // 
            this.lblReqCode.ForeColor = System.Drawing.Color.Red;
            this.lblReqCode.Location = new System.Drawing.Point(42, 10);
            this.lblReqCode.Name = "lblReqCode";
            this.lblReqCode.Size = new System.Drawing.Size(14, 11);
            this.lblReqCode.TabIndex = 33;
            this.lblReqCode.Text = "*";
            // 
            // lblShortName
            // 
            this.lblShortName.Location = new System.Drawing.Point(13, 92);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(69, 14);
            this.lblShortName.TabIndex = 4;
            this.lblShortName.Text = "Short Name";
            // 
            // txtName
            // 
            this.txtName.AutoSize = false;
            this.txtName.Location = new System.Drawing.Point(180, 40);
            this.txtName.MaxLength = 100;
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(245, 41);
            this.txtName.TabIndex = 2;
            this.txtName.LostFocus += new System.EventHandler(this.CommonTextField_LostFocus);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(13, 46);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(108, 16);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Agency Name";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(13, 13);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(33, 14);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "Code";
            // 
            // cmbCaseWorkName
            // 
            this.cmbCaseWorkName.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCaseWorkName.FormattingEnabled = true;
            this.cmbCaseWorkName.Location = new System.Drawing.Point(180, 67);
            this.cmbCaseWorkName.Name = "cmbCaseWorkName";
            this.cmbCaseWorkName.Size = new System.Drawing.Size(245, 25);
            this.cmbCaseWorkName.TabIndex = 7;
            // 
            // cmbClientName
            // 
            this.cmbClientName.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbClientName.FormattingEnabled = true;
            this.cmbClientName.Location = new System.Drawing.Point(180, 35);
            this.cmbClientName.Name = "cmbClientName";
            this.cmbClientName.Size = new System.Drawing.Size(245, 25);
            this.cmbClientName.TabIndex = 6;
            // 
            // cmbAgencyRep
            // 
            this.cmbAgencyRep.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAgencyRep.FormattingEnabled = true;
            this.cmbAgencyRep.Location = new System.Drawing.Point(180, 3);
            this.cmbAgencyRep.Name = "cmbAgencyRep";
            this.cmbAgencyRep.Size = new System.Drawing.Size(245, 25);
            this.cmbAgencyRep.TabIndex = 5;
            // 
            // lblCWFormat
            // 
            this.lblCWFormat.Location = new System.Drawing.Point(13, 68);
            this.lblCWFormat.Name = "lblCWFormat";
            this.lblCWFormat.Size = new System.Drawing.Size(161, 14);
            this.lblCWFormat.TabIndex = 9;
            this.lblCWFormat.Text = "Case Worker Name Format";
            // 
            // lblCNFormat
            // 
            this.lblCNFormat.Location = new System.Drawing.Point(13, 35);
            this.lblCNFormat.Name = "lblCNFormat";
            this.lblCNFormat.Size = new System.Drawing.Size(119, 14);
            this.lblCNFormat.TabIndex = 8;
            this.lblCNFormat.Text = "Client Name Format";
            // 
            // lblAgencyRep
            // 
            this.lblAgencyRep.Location = new System.Drawing.Point(13, 2);
            this.lblAgencyRep.Name = "lblAgencyRep";
            this.lblAgencyRep.Size = new System.Drawing.Size(139, 23);
            this.lblAgencyRep.TabIndex = 7;
            this.lblAgencyRep.Text = "Agency Representation";
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(280, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "&Save";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(358, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnOk);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 233);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(448, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(355, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlFormat
            // 
            this.pnlFormat.BackColor = System.Drawing.Color.FromName("@window");
            this.pnlFormat.Controls.Add(this.cmbCaseWorkName);
            this.pnlFormat.Controls.Add(this.cmbClientName);
            this.pnlFormat.Controls.Add(this.lblCWFormat);
            this.pnlFormat.Controls.Add(this.cmbAgencyRep);
            this.pnlFormat.Controls.Add(this.lblCNFormat);
            this.pnlFormat.Controls.Add(this.lblAgencyRep);
            this.pnlFormat.Dock = Wisej.Web.DockStyle.Top;
            this.pnlFormat.Location = new System.Drawing.Point(0, 132);
            this.pnlFormat.Name = "pnlFormat";
            this.pnlFormat.Size = new System.Drawing.Size(448, 101);
            this.pnlFormat.TabIndex = 10;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(180, 8);
            this.txtCode.MaxLength = 2;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(25, 25);
            this.txtCode.TabIndex = 1;
            this.txtCode.LostFocus += new System.EventHandler(this.CommonTextField_LostFocus);
            // 
            // AddHierarchyForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.ClientSize = new System.Drawing.Size(448, 268);
            this.Controls.Add(this.pnlFormat);
            this.Controls.Add(this.pnlSave);
            this.Controls.Add(this.pnlName);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddHierarchyForm";
            this.Text = "Agency Definition";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlName.ResumeLayout(false);
            this.pnlName.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlFormat.ResumeLayout(false);
            this.pnlFormat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlName;
        private TextBoxWithValidation txtCode;
        private Label lblCode;
        private Button btnCancel;
        private ComboBox cmbCaseWorkName;
        private ComboBox cmbClientName;
        private ComboBox cmbAgencyRep;
        private Label lblCWFormat;
        private Label lblCNFormat;
        private Label lblAgencyRep;
        private CheckBox cbIntake;
        private TextBox txtShortName;
        private Label lblShortName;
        private TextBox txtName;
        private Label lblName;
        private Button btnOk;
        private Label lblReqCode;
        private Label lblReqShortName;
        private Label lblReqAgencyName;
        private Panel pnlSave;
        private Spacer spacer1;
        private Panel pnlFormat;
    }
}