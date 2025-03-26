using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class PIR2000FundSelection
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
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PIR2000FundSelection));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwFunds = new Wisej.Web.Panel();
            this.gvwFunds = new Wisej.Web.DataGridView();
            this.gvtFundCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtFundDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.pnlSave = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnExit = new Wisej.Web.Button();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwFunds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwFunds)).BeginInit();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwFunds);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(438, 373);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlgvwFunds
            // 
            this.pnlgvwFunds.Controls.Add(this.gvwFunds);
            this.pnlgvwFunds.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwFunds.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwFunds.Name = "pnlgvwFunds";
            this.pnlgvwFunds.Size = new System.Drawing.Size(438, 338);
            this.pnlgvwFunds.TabIndex = 2;
            // 
            // gvwFunds
            // 
            this.gvwFunds.AllowUserToResizeColumns = false;
            this.gvwFunds.AllowUserToResizeRows = false;
            this.gvwFunds.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwFunds.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwFunds.BorderStyle = Wisej.Web.BorderStyle.None;
            this.gvwFunds.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwFunds.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtFundCode,
            this.gvtFundDesc,
            this.gvtType});
            this.gvwFunds.ContextMenu = this.contextMenu1;
            this.gvwFunds.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwFunds.Location = new System.Drawing.Point(0, 0);
            this.gvwFunds.Name = "gvwFunds";
            this.gvwFunds.ReadOnly = true;
            this.gvwFunds.RowHeadersWidth = 10;
            this.gvwFunds.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwFunds.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwFunds.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwFunds.Size = new System.Drawing.Size(438, 338);
            this.gvwFunds.TabIndex = 0;
            // 
            // gvtFundCode
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtFundCode.HeaderStyle = dataGridViewCellStyle1;
            this.gvtFundCode.HeaderText = "Fund Code";
            this.gvtFundCode.Name = "gvtFundCode";
            this.gvtFundCode.Width = 90;
            // 
            // gvtFundDesc
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtFundDesc.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtFundDesc.HeaderStyle = dataGridViewCellStyle3;
            this.gvtFundDesc.HeaderText = "Fund Description";
            this.gvtFundDesc.Name = "gvtFundDesc";
            this.gvtFundDesc.Width = 230;
            // 
            // gvtType
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtType.HeaderStyle = dataGridViewCellStyle4;
            this.gvtType.HeaderText = "Type";
            this.gvtType.Name = "gvtType";
            this.gvtType.Width = 80;
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            this.contextMenu1.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.gvwFunds_MenuClick);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnExit);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 338);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(438, 35);
            this.pnlSave.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(300, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(360, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnExit
            // 
            this.btnExit.AppearanceKey = "button-error";
            this.btnExit.Dock = Wisej.Web.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(363, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 25);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "&Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // PIR2000FundSelection
            // 
            this.ClientSize = new System.Drawing.Size(438, 373);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIR2000FundSelection";
            this.Text = "PIR2000FundSelection";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwFunds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwFunds)).EndInit();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlCompleteForm;
        private Panel pnlSave;
        private DataGridView gvwFunds;
        private DataGridViewTextBoxColumn gvtFundCode;
        private DataGridViewTextBoxColumn gvtFundDesc;
        private DataGridViewTextBoxColumn gvtType;
        private Button btnExit;
        private Button btnSave;
        private ContextMenu contextMenu1;
        private Panel pnlgvwFunds;
        private Spacer spacer1;
    }
}