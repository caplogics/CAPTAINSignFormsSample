using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class AddProgramContactForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProgramContactForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel2 = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.label3 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.label45 = new Wisej.Web.Label();
            this.txtEmail = new Wisej.Web.TextBox();
            this.lblEmail = new Wisej.Web.Label();
            this.msktxtFax = new Wisej.Web.MaskedTextBox();
            this.label1 = new Wisej.Web.Label();
            this.lbltelephone = new Wisej.Web.Label();
            this.msktxtPhone1 = new Wisej.Web.MaskedTextBox();
            this.lblfax = new Wisej.Web.Label();
            this.msktxtPhone2 = new Wisej.Web.MaskedTextBox();
            this.txtLastName = new Wisej.Web.TextBox();
            this.lblShortName = new Wisej.Web.Label();
            this.txtFirstName = new Wisej.Web.TextBox();
            this.lblName = new Wisej.Web.Label();
            this.txtCode = new Wisej.Web.TextBox();
            this.lblCode = new Wisej.Web.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label45);
            this.panel2.Controls.Add(this.txtEmail);
            this.panel2.Controls.Add(this.lblEmail);
            this.panel2.Controls.Add(this.msktxtFax);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbltelephone);
            this.panel2.Controls.Add(this.msktxtPhone1);
            this.panel2.Controls.Add(this.lblfax);
            this.panel2.Controls.Add(this.msktxtPhone2);
            this.panel2.Controls.Add(this.txtLastName);
            this.panel2.Controls.Add(this.lblShortName);
            this.panel2.Controls.Add(this.txtFirstName);
            this.panel2.Controls.Add(this.lblName);
            this.panel2.Controls.Add(this.txtCode);
            this.panel2.Controls.Add(this.lblCode);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(449, 239);
            this.panel2.TabIndex = 1;
            this.panel2.Click += new System.EventHandler(this.panel2_Click);
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 204);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(0, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(449, 35);
            this.panel1.TabIndex = 28;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(295, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 25);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "&Save";
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(363, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(366, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(31, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 14);
            this.label3.TabIndex = 27;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(31, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 14);
            this.label2.TabIndex = 27;
            this.label2.Text = "*";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.ForeColor = System.Drawing.Color.Red;
            this.label45.Location = new System.Drawing.Point(31, 7);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(9, 14);
            this.label45.TabIndex = 27;
            this.label45.Text = "*";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(125, 174);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(279, 25);
            this.txtEmail.TabIndex = 7;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(43, 177);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(48, 14);
            this.lblEmail.TabIndex = 21;
            this.lblEmail.Text = "Email Id";
            // 
            // msktxtFax
            // 
            this.msktxtFax.Location = new System.Drawing.Point(125, 146);
            this.msktxtFax.Mask = "000-000-0000";
            this.msktxtFax.MaxLength = 10;
            this.msktxtFax.Name = "msktxtFax";
            this.msktxtFax.Size = new System.Drawing.Size(88, 25);
            this.msktxtFax.TabIndex = 6;
            this.msktxtFax.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 20;
            this.label1.Text = "Fax No";
            // 
            // lbltelephone
            // 
            this.lbltelephone.AutoSize = true;
            this.lbltelephone.Location = new System.Drawing.Point(43, 94);
            this.lbltelephone.Name = "lbltelephone";
            this.lbltelephone.Size = new System.Drawing.Size(53, 14);
            this.lbltelephone.TabIndex = 18;
            this.lbltelephone.Text = "Phone#1";
            // 
            // msktxtPhone1
            // 
            this.msktxtPhone1.Location = new System.Drawing.Point(125, 90);
            this.msktxtPhone1.Mask = "000-000-0000";
            this.msktxtPhone1.MaxLength = 10;
            this.msktxtPhone1.Name = "msktxtPhone1";
            this.msktxtPhone1.Size = new System.Drawing.Size(88, 25);
            this.msktxtPhone1.TabIndex = 4;
            this.msktxtPhone1.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lblfax
            // 
            this.lblfax.AutoSize = true;
            this.lblfax.Location = new System.Drawing.Point(43, 123);
            this.lblfax.Name = "lblfax";
            this.lblfax.Size = new System.Drawing.Size(53, 14);
            this.lblfax.TabIndex = 20;
            this.lblfax.Text = "Phone#2";
            // 
            // msktxtPhone2
            // 
            this.msktxtPhone2.Location = new System.Drawing.Point(125, 118);
            this.msktxtPhone2.Mask = "000-000-0000";
            this.msktxtPhone2.MaxLength = 10;
            this.msktxtPhone2.Name = "msktxtPhone2";
            this.msktxtPhone2.Size = new System.Drawing.Size(88, 25);
            this.msktxtPhone2.TabIndex = 5;
            this.msktxtPhone2.TextMaskFormat = Wisej.Web.MaskFormat.ExcludePromptAndLiterals;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(125, 62);
            this.txtLastName.MaxLength = 40;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(201, 25);
            this.txtLastName.TabIndex = 3;
            // 
            // lblShortName
            // 
            this.lblShortName.AutoSize = true;
            this.lblShortName.Location = new System.Drawing.Point(43, 65);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(62, 14);
            this.lblShortName.TabIndex = 4;
            this.lblShortName.Text = "Last Name";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(125, 34);
            this.txtFirstName.MaxLength = 40;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(201, 25);
            this.txtFirstName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(43, 37);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(63, 14);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "First Name";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(125, 6);
            this.txtCode.MaxLength = 8;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(63, 25);
            this.txtCode.TabIndex = 1;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(43, 9);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(61, 14);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "Staff Code";
            // 
            // AddProgramContactForm
            // 
            this.ClientSize = new System.Drawing.Size(449, 239);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddProgramContactForm";
            this.Text = "AddHierarchyForm";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel panel2;
        private TextBox txtCode;
        private Label lblCode;
        private Button btnCancel;
        private TextBox txtLastName;
        private Label lblShortName;
        private TextBox txtFirstName;
        private Label lblName;
        private Button btnOk;
        private MaskedTextBox msktxtFax;
        private Label label1;
        private Label lbltelephone;
        private MaskedTextBox msktxtPhone1;
        private Label lblfax;
        private MaskedTextBox msktxtPhone2;
        private TextBox txtEmail;
        private Label lblEmail;
        private Label label3;
        private Label label2;
        private Label label45;
        private Panel panel1;
        private Spacer spacer1;
    }
}