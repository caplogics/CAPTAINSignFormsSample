using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class SelStaffPosCodesForm
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

        #region Wisej Web Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelStaffPosCodesForm));
            this.panel1 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.gvPosCode = new Wisej.Web.DataGridView();
            this.chkAppcode = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.PosCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.PosDescription = new Wisej.Web.DataGridViewTextBoxColumn();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.SaveCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPosCode)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 199);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvPosCode);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(483, 164);
            this.panel3.TabIndex = 37;
            // 
            // gvPosCode
            // 
            this.gvPosCode.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvPosCode.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvPosCode.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvPosCode.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvPosCode.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.chkAppcode,
            this.PosCode,
            this.PosDescription,
            this.SaveCode});
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvPosCode.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvPosCode.Dock = Wisej.Web.DockStyle.Fill;
            this.gvPosCode.Location = new System.Drawing.Point(0, 0);
            this.gvPosCode.Name = "gvPosCode";
            this.gvPosCode.RowHeadersWidth = 15;
            this.gvPosCode.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvPosCode.Size = new System.Drawing.Size(483, 164);
            this.gvPosCode.TabIndex = 0;
            // 
            // chkAppcode
            // 
            this.chkAppcode.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.chkAppcode.HeaderStyle = dataGridViewCellStyle2;
            this.chkAppcode.HeaderText = "  ";
            this.chkAppcode.MinimumWidth = 40;
            this.chkAppcode.Name = "chkAppcode";
            this.chkAppcode.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.chkAppcode.ShowInVisibilityMenu = false;
            this.chkAppcode.Width = 40;
            // 
            // PosCode
            // 
            this.PosCode.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            this.PosCode.HeaderText = " ";
            this.PosCode.Name = "PosCode";
            this.PosCode.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.PosCode.ShowInVisibilityMenu = false;
            this.PosCode.Visible = false;
            this.PosCode.Width = 10;
            // 
            // PosDescription
            // 
            this.PosDescription.AutoSizeMode = Wisej.Web.DataGridViewAutoSizeColumnMode.None;
            this.PosDescription.ContextMenu = this.contextMenu1;
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.PosDescription.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.PosDescription.HeaderStyle = dataGridViewCellStyle4;
            this.PosDescription.HeaderText = "Description";
            this.PosDescription.MinimumWidth = 300;
            this.PosDescription.Name = "PosDescription";
            this.PosDescription.ReadOnly = true;
            this.PosDescription.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.PosDescription.Width = 300;
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // SaveCode
            // 
            this.SaveCode.HeaderText = "SaveCode";
            this.SaveCode.Name = "SaveCode";
            this.SaveCode.ShowInVisibilityMenu = false;
            this.SaveCode.Visible = false;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 164);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(4);
            this.panel2.Size = new System.Drawing.Size(483, 35);
            this.panel2.TabIndex = 36;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(324, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(399, 4);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 27);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(404, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SelStaffPosCodesForm
            // 
            this.ClientSize = new System.Drawing.Size(483, 199);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelStaffPosCodesForm";
            this.Text = "Sel_staff_pos_codes_Form";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvPosCode)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView gvPosCode;
        private DataGridViewCheckBoxColumn chkAppcode;
        private DataGridViewTextBoxColumn PosCode;
        private DataGridViewTextBoxColumn PosDescription;
        private DataGridViewTextBoxColumn SaveCode;
        private ContextMenu contextMenu1;
        private Button btnSave;
        private Button btnCancel;
        private Panel panel3;
        private Panel panel2;
        private Spacer spacer1;
    }
}