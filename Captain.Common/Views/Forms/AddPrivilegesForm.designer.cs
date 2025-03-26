using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class AddPrivilegesForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPrivilegesForm));
            this.lblHierarchie = new Wisej.Web.Label();
            this.btnOK = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.tabControl1 = new Wisej.Web.TabControl();
            this.txtHierarchy = new Wisej.Web.TextBox();
            this.contextMenu1 = new Wisej.Web.ContextMenu();
            this.btnSelectAll = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlCopy = new Wisej.Web.Panel();
            this.chkbCopyFrom = new Wisej.Web.CheckBox();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.panel3 = new Wisej.Web.Panel();
            this.cmbCopyHie = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.cmbHierarchy = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.panel1.SuspendLayout();
            this.pnlCopy.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHierarchie
            // 
            this.lblHierarchie.AutoSize = true;
            this.lblHierarchie.Location = new System.Drawing.Point(23, 10);
            this.lblHierarchie.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblHierarchie.Name = "lblHierarchie";
            this.lblHierarchie.Size = new System.Drawing.Size(56, 18);
            this.lblHierarchie.TabIndex = 2;
            this.lblHierarchie.Text = "Hierarchy";
            // 
            // btnOK
            // 
            this.btnOK.AppearanceKey = "button-ok";
            this.btnOK.Dock = Wisej.Web.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(567, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.OKClick);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.DialogResult = Wisej.Web.DialogResult.Cancel;
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(645, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.CancelClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = Wisej.Web.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new Wisej.Web.Padding(0, 0, 0, 5);
            this.tabControl1.PageInsets = new Wisej.Web.Padding(0, 0, 0, 0);
            this.tabControl1.Size = new System.Drawing.Size(725, 315);
            this.tabControl1.TabIndex = 14;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.OnModulesTabControlSelectedIndexChanged);
            // 
            // txtHierarchy
            // 
            this.txtHierarchy.Location = new System.Drawing.Point(90, 8);
            this.txtHierarchy.Name = "txtHierarchy";
            this.txtHierarchy.ReadOnly = true;
            this.txtHierarchy.Size = new System.Drawing.Size(316, 25);
            this.txtHierarchy.TabIndex = 15;
            this.txtHierarchy.Text = "****** - All Hierarchies";
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            this.contextMenu1.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.DataGrid_MenuClick);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Dock = Wisej.Web.DockStyle.Left;
            this.btnSelectAll.Location = new System.Drawing.Point(8, 5);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 25);
            this.btnSelectAll.TabIndex = 11;
            this.btnSelectAll.Text = "Select &All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlCopy);
            this.panel1.Controls.Add(this.txtHierarchy);
            this.panel1.Controls.Add(this.cmbHierarchy);
            this.panel1.Controls.Add(this.lblHierarchie);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 40);
            this.panel1.TabIndex = 16;
            // 
            // pnlCopy
            // 
            this.pnlCopy.Controls.Add(this.cmbCopyHie);
            this.pnlCopy.Controls.Add(this.chkbCopyFrom);
            this.pnlCopy.Location = new System.Drawing.Point(424, 4);
            this.pnlCopy.Name = "pnlCopy";
            this.pnlCopy.Size = new System.Drawing.Size(228, 30);
            this.pnlCopy.TabIndex = 13;
            // 
            // chkbCopyFrom
            // 
            this.chkbCopyFrom.Location = new System.Drawing.Point(8, 4);
            this.chkbCopyFrom.Name = "chkbCopyFrom";
            this.chkbCopyFrom.Size = new System.Drawing.Size(89, 21);
            this.chkbCopyFrom.TabIndex = 16;
            this.chkbCopyFrom.Text = "CopyFrom";
            this.chkbCopyFrom.CheckedChanged += new System.EventHandler(this.chkbCopyFrom_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSelectAll);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 368);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(8, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(735, 35);
            this.panel2.TabIndex = 17;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(642, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 40);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5, 5, 5, 8);
            this.panel3.Size = new System.Drawing.Size(735, 328);
            this.panel3.TabIndex = 18;
            // 
            // cmbCopyHie
            // 
            this.cmbCopyHie.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCopyHie.Location = new System.Drawing.Point(104, 2);
            this.cmbCopyHie.Name = "cmbCopyHie";
            this.cmbCopyHie.Size = new System.Drawing.Size(114, 25);
            this.cmbCopyHie.TabIndex = 17;
            this.cmbCopyHie.Visible = false;
            this.cmbCopyHie.SelectedIndexChanged += new System.EventHandler(this.cmbCopyHie_SelectedIndexChanged);
            // 
            // cmbHierarchy
            // 
            this.cmbHierarchy.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbHierarchy.Location = new System.Drawing.Point(90, 8);
            this.cmbHierarchy.Name = "cmbHierarchy";
            this.cmbHierarchy.Size = new System.Drawing.Size(316, 25);
            this.cmbHierarchy.TabIndex = 3;
            this.cmbHierarchy.SelectedIndexChanged += new System.EventHandler(this.cmbHierarchy_SelectedIndexChanged);
            // 
            // AddPrivilegesForm
            // 
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(735, 403);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddPrivilegesForm";
            this.Text = "Assign User Privileges";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlCopy.ResumeLayout(false);
            this.pnlCopy.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Label lblHierarchie;
        private ComboBoxEx cmbHierarchy;
        private Wisej.Web.Button btnOK;
        private Wisej.Web.Button btnCancel;
        private Wisej.Web.TabControl tabControl1;
        private Wisej.Web.TextBox txtHierarchy;
        private Wisej.Web.ContextMenu contextMenu1;
        private Wisej.Web.Button btnSelectAll;
        private Panel panel1;
        private Panel panel2;
        private Spacer spacer1;
        private Panel panel3;
        private ComboBoxEx cmbCopyHie;
        private CheckBox chkbCopyFrom;
        private Panel pnlCopy;
    }
}