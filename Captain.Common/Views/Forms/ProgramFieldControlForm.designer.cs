using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class ProgramFieldControlForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramFieldControlForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.gvwProgramCode = new Wisej.Web.DataGridView();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvcEnabled = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvcRequied = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvwProgramCode)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwProgramCode
            // 
            this.gvwProgramCode.AllowUserToResizeColumns = false;
            this.gvwProgramCode.AllowUserToResizeRows = false;
            this.gvwProgramCode.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.gvwProgramCode.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwProgramCode.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwProgramCode.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwProgramCode.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtDesc,
            this.gvcEnabled,
            this.gvcRequied,
            this.gvtCode});
            this.gvwProgramCode.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwProgramCode.Location = new System.Drawing.Point(0, 0);
            this.gvwProgramCode.MultiSelect = false;
            this.gvwProgramCode.Name = "gvwProgramCode";
            this.gvwProgramCode.RowHeadersWidth = 14;
            this.gvwProgramCode.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwProgramCode.Size = new System.Drawing.Size(461, 349);
            this.gvwProgramCode.TabIndex = 4;
            this.gvwProgramCode.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwProgramCode_CellClick);
            // 
            // gvtDesc
            // 
            this.gvtDesc.HeaderText = "Description";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.gvtDesc.Width = 300;
            // 
            // gvcEnabled
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            this.gvcEnabled.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvcEnabled.HeaderText = "Enabled";
            this.gvcEnabled.Name = "gvcEnabled";
            this.gvcEnabled.Width = 60;
            // 
            // gvcRequied
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = false;
            this.gvcRequied.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvcRequied.HeaderText = "Required";
            this.gvcRequied.Name = "gvcRequied";
            this.gvcRequied.Width = 60;
            // 
            // gvtCode
            // 
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.Visible = false;
            this.gvtCode.Width = 10;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.gvwProgramCode);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(463, 388);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 349);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(461, 37);
            this.panel2.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Location = new System.Drawing.Point(330, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(62, 25);
            this.btnOk.TabIndex = 7;
            this.btnOk.Tag = "";
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Location = new System.Drawing.Point(393, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ProgramFieldControlForm
            // 
            this.ClientSize = new System.Drawing.Size(463, 388);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgramFieldControlForm";
            this.Text = "ProgramFieldControlForm";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            ((System.ComponentModel.ISupportInitialize)(this.gvwProgramCode)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private DataGridView gvwProgramCode;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewCheckBoxColumn gvcEnabled;
        private DataGridViewCheckBoxColumn gvcRequied;
        private Panel panel1;
        private Panel panel2;
        private Button btnOk;
        private Button btnCancel;
        private DataGridViewTextBoxColumn gvtCode;
    }
}