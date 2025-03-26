//using Wisej.Web;
//using Gizmox.WebGUI.Common;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class ServicesSelectionsForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicesSelectionsForm));
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlGrdServicePlan = new Wisej.Web.Panel();
            this.gvSps = new Wisej.Web.DataGridView();
            this.SP_Sel = new Wisej.Web.DataGridViewImageColumn();
            this.Service_Plan = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SP_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Selected = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlGrdServicePlan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSps)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGrdServicePlan);
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(475, 314);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlGrdServicePlan
            // 
            this.pnlGrdServicePlan.Controls.Add(this.gvSps);
            this.pnlGrdServicePlan.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGrdServicePlan.Location = new System.Drawing.Point(0, 0);
            this.pnlGrdServicePlan.Name = "pnlGrdServicePlan";
            this.pnlGrdServicePlan.Size = new System.Drawing.Size(475, 279);
            this.pnlGrdServicePlan.TabIndex = 8;
            // 
            // gvSps
            // 
            this.gvSps.AllowUserToResizeColumns = false;
            this.gvSps.AllowUserToResizeRows = false;
            this.gvSps.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvSps.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvSps.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSps.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.SP_Sel,
            this.Service_Plan,
            this.SP_Code,
            this.Selected});
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvSps.DefaultCellStyle = dataGridViewCellStyle10;
            this.gvSps.Dock = Wisej.Web.DockStyle.Fill;
            this.gvSps.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvSps.Location = new System.Drawing.Point(0, 0);
            this.gvSps.Name = "gvSps";
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvSps.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.gvSps.RowHeadersWidth = 10;
            this.gvSps.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvSps.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvSps.Size = new System.Drawing.Size(475, 279);
            this.gvSps.TabIndex = 6;
            this.gvSps.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvSps_CellClick);
            // 
            // SP_Sel
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = null;
            this.SP_Sel.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.SP_Sel.HeaderStyle = dataGridViewCellStyle3;
            this.SP_Sel.HeaderText = "";
            this.SP_Sel.Name = "SP_Sel";
            this.SP_Sel.ReadOnly = true;
            this.SP_Sel.Width = 30;
            // 
            // Service_Plan
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Service_Plan.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Service_Plan.HeaderStyle = dataGridViewCellStyle5;
            this.Service_Plan.HeaderText = "Service Plan";
            this.Service_Plan.Name = "Service_Plan";
            this.Service_Plan.ReadOnly = true;
            this.Service_Plan.Width = 385;
            // 
            // SP_Code
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SP_Code.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SP_Code.HeaderStyle = dataGridViewCellStyle7;
            this.SP_Code.HeaderText = "SP_Code";
            this.SP_Code.Name = "SP_Code";
            this.SP_Code.ReadOnly = true;
            this.SP_Code.ShowInVisibilityMenu = false;
            this.SP_Code.Visible = false;
            this.SP_Code.Width = 20;
            // 
            // Selected
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.HeaderStyle = dataGridViewCellStyle9;
            this.Selected.HeaderText = "Selected";
            this.Selected.Name = "Selected";
            this.Selected.ShowInVisibilityMenu = false;
            this.Selected.Visible = false;
            this.Selected.Width = 20;
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 279);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(475, 35);
            this.panel1.TabIndex = 7;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(322, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(382, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(385, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ServicesSelectionsForm
            // 
            this.ClientSize = new System.Drawing.Size(475, 314);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServicesSelectionsForm";
            this.Text = "Services Selections Form";
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlGrdServicePlan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvSps)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Panel pnlCompleteForm;
        private DataGridView gvSps;
        private DataGridViewTextBoxColumn Service_Plan;
        private DataGridViewTextBoxColumn SP_Code;
        private DataGridViewTextBoxColumn Selected;
        private Button btnCancel;
        private Button btnOk;
        private Panel pnlGrdServicePlan;
        private Panel panel1;
        private Spacer spacer1;
        private DataGridViewImageColumn SP_Sel;
    }
}