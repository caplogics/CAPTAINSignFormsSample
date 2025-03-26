using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class Invoice_Documents
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

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Invoice_Documents));
            this.pnlForm = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnldgvInvDocs = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.dgvInvDocs = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvInvDocName = new Wisej.Web.DataGridViewColumn();
            this.gvDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvInvDocID = new Wisej.Web.DataGridViewColumn();
            this.gvDel = new Wisej.Web.DataGridViewImageColumn();
            this.gvUploadBy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlForm.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnldgvInvDocs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvDocs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Controls.Add(this.pnlParams);
            this.pnlForm.Controls.Add(this.pnlSave);
            this.pnlForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(613, 342);
            this.pnlForm.TabIndex = 0;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnldgvInvDocs);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Padding = new Wisej.Web.Padding(5);
            this.pnlParams.Size = new System.Drawing.Size(613, 311);
            this.pnlParams.TabIndex = 1;
            // 
            // pnldgvInvDocs
            // 
            this.pnldgvInvDocs.Controls.Add(this.dgvInvDocs);
            this.pnldgvInvDocs.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.pnldgvInvDocs.Dock = Wisej.Web.DockStyle.Fill;
            this.pnldgvInvDocs.Location = new System.Drawing.Point(5, 5);
            this.pnldgvInvDocs.Name = "pnldgvInvDocs";
            this.pnldgvInvDocs.Size = new System.Drawing.Size(603, 301);
            this.pnldgvInvDocs.TabIndex = 0;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 311);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(613, 31);
            this.pnlSave.TabIndex = 0;
            // 
            // dgvInvDocs
            // 
            this.dgvInvDocs.AllowUserToResizeColumns = false;
            this.dgvInvDocs.AllowUserToResizeRows = false;
            this.dgvInvDocs.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvInvDocName,
            this.gvDate,
            this.gvUploadBy,
            this.gvDel,
            this.gvInvDocID});
            this.dgvInvDocs.Cursor = Wisej.Web.Cursors.Hand;
            this.dgvInvDocs.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvInvDocs.Name = "dgvInvDocs";
            this.dgvInvDocs.RowHeadersWidth = 15;
            this.dgvInvDocs.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvInvDocs.Size = new System.Drawing.Size(603, 301);
            this.dgvInvDocs.TabIndex = 0;
            this.dgvInvDocs.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.dgvInvDocs_CellClick);
            // 
            // gvInvDocName
            // 
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvInvDocName.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvInvDocName.HeaderStyle = dataGridViewCellStyle2;
            this.gvInvDocName.HeaderText = "Invoice Document Name";
            this.gvInvDocName.Name = "gvInvDocName";
            this.gvInvDocName.Width = 350;
            // 
            // gvDate
            // 
            this.gvDate.HeaderText = "Date";
            this.gvDate.Name = "gvDate";
            this.gvDate.ReadOnly = true;
            this.gvDate.Width = 70;
            // 
            // gvInvDocID
            // 
            this.gvInvDocID.HeaderText = "InvDoc ID";
            this.gvInvDocID.Name = "gvInvDocID";
            this.gvInvDocID.ShowInVisibilityMenu = false;
            this.gvInvDocID.Visible = false;
            // 
            // gvDel
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvDel.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvDel.HeaderText = "";
            this.gvDel.Name = "gvDel";
            this.gvDel.ShowInVisibilityMenu = false;
            this.gvDel.Width = 30;
            // 
            // gvUploadBy
            // 
            this.gvUploadBy.HeaderText = "Upload By";
            this.gvUploadBy.Name = "gvUploadBy";
            this.gvUploadBy.Width = 80;
            // 
            // Invoice_Documents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 342);
            this.Controls.Add(this.pnlForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Invoice_Documents";
            this.Text = "Invoice Documents";
            this.pnlForm.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.pnldgvInvDocs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvDocs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlForm;
        private Wisej.Web.Panel pnlParams;
        private Wisej.Web.Panel pnlSave;
        private Wisej.Web.Panel pnldgvInvDocs;
        private DataGridViewEx dgvInvDocs;
        private Wisej.Web.DataGridViewColumn gvInvDocName;
        private Controls.Compatibility.DataGridViewDateTimeColumn gvDate;
        private Wisej.Web.DataGridViewImageColumn gvDel;
        private Wisej.Web.DataGridViewColumn gvInvDocID;
        private Wisej.Web.DataGridViewTextBoxColumn gvUploadBy;
    }
}