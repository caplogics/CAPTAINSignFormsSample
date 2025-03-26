using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class CA_DuplicatesScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CA_DuplicatesScreen));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlDuplicate = new Wisej.Web.Panel();
            this.txtdupDesc = new Wisej.Web.TextBox();
            this.lblDup = new Wisej.Web.Label();
            this.pnlgvDupCA = new Wisej.Web.Panel();
            this.gvDupCA = new Wisej.Web.DataGridView();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Dup_Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlDuplicate.SuspendLayout();
            this.pnlgvDupCA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDupCA)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDuplicate
            // 
            this.pnlDuplicate.Controls.Add(this.txtdupDesc);
            this.pnlDuplicate.Controls.Add(this.lblDup);
            this.pnlDuplicate.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDuplicate.Location = new System.Drawing.Point(0, 0);
            this.pnlDuplicate.Name = "pnlDuplicate";
            this.pnlDuplicate.Size = new System.Drawing.Size(685, 57);
            this.pnlDuplicate.TabIndex = 0;
            // 
            // txtdupDesc
            // 
            this.txtdupDesc.BackColor = System.Drawing.Color.Transparent;
            this.txtdupDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.txtdupDesc.Location = new System.Drawing.Point(107, 11);
            this.txtdupDesc.Multiline = true;
            this.txtdupDesc.Name = "txtdupDesc";
            this.txtdupDesc.ReadOnly = true;
            this.txtdupDesc.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.txtdupDesc.Size = new System.Drawing.Size(555, 40);
            this.txtdupDesc.TabIndex = 1;
            // 
            // lblDup
            // 
            this.lblDup.Location = new System.Drawing.Point(15, 13);
            this.lblDup.Name = "lblDup";
            this.lblDup.Size = new System.Drawing.Size(83, 16);
            this.lblDup.TabIndex = 0;
            this.lblDup.Text = "Duplicates for:";
            // 
            // pnlgvDupCA
            // 
            this.pnlgvDupCA.Controls.Add(this.gvDupCA);
            this.pnlgvDupCA.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvDupCA.Location = new System.Drawing.Point(0, 57);
            this.pnlgvDupCA.Name = "pnlgvDupCA";
            this.pnlgvDupCA.Size = new System.Drawing.Size(685, 399);
            this.pnlgvDupCA.TabIndex = 1;
            // 
            // gvDupCA
            // 
            this.gvDupCA.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvDupCA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvDupCA.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDupCA.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Code,
            this.Dup_Desc});
            this.gvDupCA.DefaultRowHeight = 40;
            this.gvDupCA.Dock = Wisej.Web.DockStyle.Fill;
            this.gvDupCA.Location = new System.Drawing.Point(0, 0);
            this.gvDupCA.Name = "gvDupCA";
            this.gvDupCA.RowHeadersWidth = 25;
            this.gvDupCA.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvDupCA.RowTemplate.Height = 28;
            this.gvDupCA.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvDupCA.Size = new System.Drawing.Size(685, 399);
            this.gvDupCA.TabIndex = 0;
            // 
            // Code
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Code.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Code.HeaderStyle = dataGridViewCellStyle3;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 80;
            // 
            // Dup_Desc
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle4.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Dup_Desc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            this.Dup_Desc.HeaderStyle = dataGridViewCellStyle5;
            this.Dup_Desc.HeaderText = "Service Description";
            this.Dup_Desc.Name = "Dup_Desc";
            this.Dup_Desc.ReadOnly = true;
            this.Dup_Desc.Width = 550;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvDupCA);
            this.pnlCompleteForm.Controls.Add(this.pnlDuplicate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(685, 456);
            this.pnlCompleteForm.TabIndex = 2;
            // 
            // CA_DuplicatesScreen
            // 
            this.ClientSize = new System.Drawing.Size(685, 456);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CA_DuplicatesScreen";
            this.Text = "Service - Duplicate Descriptions";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlDuplicate.ResumeLayout(false);
            this.pnlDuplicate.PerformLayout();
            this.pnlgvDupCA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDupCA)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlDuplicate;
        private Label lblDup;
        private Panel pnlgvDupCA;
        private DataGridView gvDupCA;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Dup_Desc;
        private TextBox txtdupDesc;
        private Panel pnlCompleteForm;
    }
}