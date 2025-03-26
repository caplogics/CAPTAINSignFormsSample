using Wisej.Web;


namespace Captain.Common.Views.UserControls
{
    partial class ApplicationDetailsControl
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

        #region Wisej UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            this.dataGridAppNo = new Wisej.Web.DataGridView();
            this.MainName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.DOB = new Wisej.Web.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAppNo)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridAppNo
            // 
            this.dataGridAppNo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridAppNo.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("default", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dataGridAppNo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridAppNo.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAppNo.ColumnHeadersVisible = false;
            this.dataGridAppNo.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.MainName,
            this.DOB});
            this.dataGridAppNo.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridAppNo.Location = new System.Drawing.Point(0, 0);
            this.dataGridAppNo.MultiSelect = false;
            this.dataGridAppNo.Name = "dataGridAppNo";
            this.dataGridAppNo.RowHeadersVisible = false;
            this.dataGridAppNo.RowHeadersWidth = 14;
            this.dataGridAppNo.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridAppNo.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-IN");
            this.dataGridAppNo.RowTemplate.Height = 20;
            this.dataGridAppNo.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridAppNo.Size = new System.Drawing.Size(229, 77);
            this.dataGridAppNo.TabIndex = 6;
            this.dataGridAppNo.Visible = false;
            // 
            // MainName
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.MainName.DefaultCellStyle = dataGridViewCellStyle2;
            this.MainName.HeaderText = "Name";
            this.MainName.MinimumWidth = 155;
            this.MainName.Name = "MainName";
            this.MainName.ReadOnly = true;
            this.MainName.Width = 155;
            // 
            // DOB
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.DOB.DefaultCellStyle = dataGridViewCellStyle3;
            this.DOB.HeaderText = "DOB";
            this.DOB.Name = "DOB";
            this.DOB.ReadOnly = true;
            this.DOB.Width = 60;
            // 
            // ApplicationDetailsControl
            // 
            this.Controls.Add(this.dataGridAppNo);
            this.Name = "ApplicationDetailsControl";
            this.Size = new System.Drawing.Size(286, 96);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAppNo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewTextBoxColumn MainName;
        private DataGridViewTextBoxColumn DOB;
        public DataGridView dataGridAppNo;


    }
}