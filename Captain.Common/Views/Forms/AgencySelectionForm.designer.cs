using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class AgencySelectionForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgencySelectionForm));
            this.btnOk = new Wisej.Web.Button();
            this.gvwHierarchie = new Wisej.Web.DataGridView();
            this.Select = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.DESC = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnCancel = new Wisej.Web.Button();
            this.lblChoose = new Wisej.Web.Label();
            this.gvwPanel = new Wisej.Web.Panel();
            this.okPanel = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlChoose = new Wisej.Web.Panel();
            this.pnlSelection = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).BeginInit();
            this.gvwPanel.SuspendLayout();
            this.okPanel.SuspendLayout();
            this.pnlChoose.SuspendLayout();
            this.pnlSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.Location = new System.Drawing.Point(296, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // gvwHierarchie
            // 
            this.gvwHierarchie.AllowUserToResizeRows = false;
            this.gvwHierarchie.BackColor = System.Drawing.Color.White;
            this.gvwHierarchie.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwHierarchie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwHierarchie.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvwHierarchie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Select,
            this.Code,
            this.DESC});
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwHierarchie.DefaultCellStyle = dataGridViewCellStyle8;
            this.gvwHierarchie.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHierarchie.Location = new System.Drawing.Point(0, 0);
            this.gvwHierarchie.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwHierarchie.MultiSelect = false;
            this.gvwHierarchie.Name = "gvwHierarchie";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwHierarchie.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gvwHierarchie.RowHeadersVisible = false;
            this.gvwHierarchie.RowHeadersWidth = 15;
            this.gvwHierarchie.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHierarchie.Size = new System.Drawing.Size(439, 310);
            this.gvwHierarchie.TabIndex = 0;
            this.gvwHierarchie.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwHierarchie_CellClick);
            // 
            // Select
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.Select.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Select.HeaderStyle = dataGridViewCellStyle3;
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            this.Select.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Select.ShowInVisibilityMenu = false;
            this.Select.Width = 60;
            // 
            // Code
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Code.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Code.HeaderStyle = dataGridViewCellStyle5;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Code.Width = 65;
            // 
            // DESC
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DESC.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DESC.HeaderStyle = dataGridViewCellStyle7;
            this.DESC.HeaderText = "Description";
            this.DESC.Name = "DESC";
            this.DESC.ReadOnly = true;
            this.DESC.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.DESC.ShowInVisibilityMenu = false;
            this.DESC.Width = 285;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(359, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblChoose
            // 
            this.lblChoose.AppearanceKey = "lblsubHeading";
            this.lblChoose.Dock = Wisej.Web.DockStyle.Left;
            this.lblChoose.Location = new System.Drawing.Point(0, 0);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(185, 25);
            this.lblChoose.TabIndex = 5;
            this.lblChoose.Text = "Choose Agency Here";
            this.lblChoose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gvwPanel
            // 
            this.gvwPanel.Controls.Add(this.gvwHierarchie);
            this.gvwPanel.Dock = Wisej.Web.DockStyle.Top;
            this.gvwPanel.Location = new System.Drawing.Point(5, 30);
            this.gvwPanel.Margin = new Wisej.Web.Padding(3, 0, 3, 3);
            this.gvwPanel.Name = "gvwPanel";
            this.gvwPanel.Size = new System.Drawing.Size(439, 310);
            this.gvwPanel.TabIndex = 6;
            // 
            // okPanel
            // 
            this.okPanel.AppearanceKey = "panel-grdo";
            this.okPanel.Controls.Add(this.btnOk);
            this.okPanel.Controls.Add(this.spacer1);
            this.okPanel.Controls.Add(this.btnCancel);
            this.okPanel.Dock = Wisej.Web.DockStyle.Bottom;
            this.okPanel.Location = new System.Drawing.Point(0, 343);
            this.okPanel.Name = "okPanel";
            this.okPanel.Padding = new Wisej.Web.Padding(3, 5, 15, 5);
            this.okPanel.Size = new System.Drawing.Size(449, 35);
            this.okPanel.TabIndex = 7;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(356, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlChoose
            // 
            this.pnlChoose.Controls.Add(this.lblChoose);
            this.pnlChoose.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChoose.Location = new System.Drawing.Point(5, 5);
            this.pnlChoose.Margin = new Wisej.Web.Padding(3, 3, 3, 0);
            this.pnlChoose.Name = "pnlChoose";
            this.pnlChoose.Size = new System.Drawing.Size(439, 25);
            this.pnlChoose.TabIndex = 8;
            // 
            // pnlSelection
            // 
            this.pnlSelection.Controls.Add(this.gvwPanel);
            this.pnlSelection.Controls.Add(this.pnlChoose);
            this.pnlSelection.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSelection.Location = new System.Drawing.Point(0, 0);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Padding = new Wisej.Web.Padding(5);
            this.pnlSelection.Size = new System.Drawing.Size(449, 339);
            this.pnlSelection.TabIndex = 9;
            // 
            // AgencySelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(449, 378);
            this.Controls.Add(this.okPanel);
            this.Controls.Add(this.pnlSelection);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AgencySelectionForm";
            this.Text = "Agency Selection";
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchie)).EndInit();
            this.gvwPanel.ResumeLayout(false);
            this.okPanel.ResumeLayout(false);
            this.pnlChoose.ResumeLayout(false);
            this.pnlSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnOk;
        private DataGridView gvwHierarchie;
        private DataGridViewTextBoxColumn DESC;
        private Button btnCancel;
        private Label lblChoose;
        private DataGridViewTextBoxColumn Code;
        private Panel gvwPanel;
        private Panel okPanel;
        private Spacer spacer1;
        private Panel pnlChoose;
        private Panel pnlSelection;
        private DataGridViewCheckBoxColumn Select;
    }
}