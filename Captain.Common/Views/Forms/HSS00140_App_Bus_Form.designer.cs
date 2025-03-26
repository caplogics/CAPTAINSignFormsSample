using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00140_App_Bus_Form
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            this.Lbl_App_NO = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.panel4 = new Wisej.Web.Panel();
            this.App_Details_Grid = new Wisej.Web.DataGridView();
            this.dataGridViewTextBoxColumn1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.App_Details_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Lbl_App_NO
            // 
            this.Lbl_App_NO.AutoSize = true;
            this.Lbl_App_NO.Dock = Wisej.Web.DockStyle.Left;
            this.Lbl_App_NO.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_App_NO.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            this.Lbl_App_NO.Location = new System.Drawing.Point(37, 0);
            this.Lbl_App_NO.Name = "Lbl_App_NO";
            this.Lbl_App_NO.Size = new System.Drawing.Size(49, 24);
            this.Lbl_App_NO.TabIndex = 1;
            this.Lbl_App_NO.Text = "Number";
            this.Lbl_App_NO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = Wisej.Web.DockStyle.Left;
            this.label6.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 24);
            this.label6.TabIndex = 1;
            this.label6.Text = "App #";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(223, 231, 240);
            this.panel4.Controls.Add(this.Lbl_App_NO);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(695, 24);
            this.panel4.TabIndex = 0;
            // 
            // App_Details_Grid
            // 
            this.App_Details_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.App_Details_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.App_Details_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.App_Details_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.App_Details_Grid.Location = new System.Drawing.Point(0, 24);
            this.App_Details_Grid.Name = "App_Details_Grid";
            this.App_Details_Grid.RowHeadersWidth = 25;
            this.App_Details_Grid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.App_Details_Grid.Size = new System.Drawing.Size(695, 225);
            this.App_Details_Grid.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn1.HeaderStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn1.HeaderText = "BUS";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 250;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn2.HeaderStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn2.HeaderText = "ROUTE";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn3.HeaderStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn3.HeaderText = "PICKUP";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn4.HeaderText = "DROP";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 70;
            // 
            // HSS00140_App_Bus_Form
            // 
            this.ClientSize = new System.Drawing.Size(695, 249);
            this.Controls.Add(this.App_Details_Grid);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00140_App_Bus_Form";
            this.Text = "HSS00140_App_Bus_Form";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.App_Details_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label Lbl_App_NO;
        private Label label6;
        private Panel panel4;
        private DataGridView App_Details_Grid;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;


    }
}