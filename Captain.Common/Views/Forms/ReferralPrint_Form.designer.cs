using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ReferralPrint_Form
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle18 = new Wisej.Web.DataGridViewCellStyle();
            this.lblCustomer = new Wisej.Web.Label();
            this.cmbName = new Wisej.Web.ComboBox();
            this.Ref_Grid = new Wisej.Web.DataGridView();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Name = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Name2 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Street = new Wisej.Web.DataGridViewTextBoxColumn();
            this.City = new Wisej.Web.DataGridViewTextBoxColumn();
            this.State = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtNameIndex = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Active = new Wisej.Web.DataGridViewTextBoxColumn();
            this.btnPDFprev = new Wisej.Web.Button();
            this.btnGenPdf = new Wisej.Web.Button();
            this.lblDate = new Wisej.Web.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Ref_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCustomer
            // 
            this.lblCustomer.Location = new System.Drawing.Point(5, 10);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(108, 17);
            this.lblCustomer.TabIndex = 1;
            this.lblCustomer.Text = "Customer Name";
            // 
            // cmbName
            // 
            this.cmbName.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbName.FormattingEnabled = true;
            this.cmbName.Location = new System.Drawing.Point(89, 6);
            this.cmbName.Name = "cmbName";
            this.cmbName.Size = new System.Drawing.Size(263, 25);
            this.cmbName.TabIndex = 2;
            // 
            // Ref_Grid
            // 
            this.Ref_Grid.AllowUserToAddRows = false;
            this.Ref_Grid.AllowUserToDeleteRows = false;
            this.Ref_Grid.AllowUserToOrderColumns = true;
            this.Ref_Grid.AllowUserToResizeRows = false;
            this.Ref_Grid.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Ref_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Ref_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.Ref_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Ref_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Code,
            this.Name,
            this.Name2,
            this.Street,
            this.City,
            this.State,
            this.gvtNameIndex,
            this.Active});
            this.Ref_Grid.Location = new System.Drawing.Point(3, 37);
            this.Ref_Grid.Name = "Ref_Grid";
            this.Ref_Grid.RowHeadersWidth = 25;
            this.Ref_Grid.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Ref_Grid.ScrollBars = Wisej.Web.ScrollBars.Horizontal;
            this.Ref_Grid.SelectionMode = Wisej.Web.DataGridViewSelectionMode.FullRowSelect;
            this.Ref_Grid.Size = new System.Drawing.Size(763, 184);
            this.Ref_Grid.TabIndex = 3;
            // 
            // Code
            // 
            this.Code.DefaultCellStyle = dataGridViewCellStyle11;
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 40;
            // 
            // Name
            // 
            this.Name.DefaultCellStyle = dataGridViewCellStyle12;
            this.Name.Name = "Name";
            this.Name.ReadOnly = true;
            this.Name.Width = 205;
            // 
            // Name2
            // 
            this.Name2.DefaultCellStyle = dataGridViewCellStyle13;
            this.Name2.Name = "Name2";
            this.Name2.ReadOnly = true;
            this.Name2.Width = 150;
            // 
            // Street
            // 
            this.Street.DefaultCellStyle = dataGridViewCellStyle14;
            this.Street.Name = "Street";
            this.Street.ReadOnly = true;
            this.Street.Visible = false;
            this.Street.Width = 165;
            // 
            // City
            // 
            this.City.DefaultCellStyle = dataGridViewCellStyle15;
            this.City.Name = "City";
            this.City.ReadOnly = true;
            this.City.Visible = false;
            this.City.Width = 120;
            // 
            // State
            // 
            this.State.DefaultCellStyle = dataGridViewCellStyle16;
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.Visible = false;
            this.State.Width = 40;
            // 
            // gvtNameIndex
            // 
            this.gvtNameIndex.DefaultCellStyle = dataGridViewCellStyle17;
            this.gvtNameIndex.HeaderText = "NameIndex";
            this.gvtNameIndex.Name = "gvtNameIndex";
            this.gvtNameIndex.ReadOnly = true;
            this.gvtNameIndex.Visible = false;
            this.gvtNameIndex.Width = 150;
            // 
            // Active
            // 
            this.Active.DefaultCellStyle = dataGridViewCellStyle18;
            this.Active.Name = "Active";
            this.Active.ReadOnly = true;
            this.Active.Visible = false;
            this.Active.Width = 45;
            // 
            // btnPDFprev
            // 
            this.btnPDFprev.Location = new System.Drawing.Point(666, 227);
            this.btnPDFprev.Name = "btnPDFprev";
            this.btnPDFprev.Size = new System.Drawing.Size(99, 29);
            this.btnPDFprev.TabIndex = 3;
            this.btnPDFprev.Text = "Cancel";
            this.btnPDFprev.Click += new System.EventHandler(this.btnPDFprev_Click);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.Location = new System.Drawing.Point(569, 227);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(99, 29);
            this.btnGenPdf.TabIndex = 3;
            this.btnGenPdf.Text = "&Print";
            this.btnGenPdf.Click += new System.EventHandler(this.btnGenPdf_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(435, 9);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(42, 17);
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "Date";
            // 
            // ReferralPrint_Form
            // 
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.btnGenPdf);
            this.Controls.Add(this.btnPDFprev);
            this.Controls.Add(this.Ref_Grid);
            this.Controls.Add(this.cmbName);
            this.Controls.Add(this.lblCustomer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(766, 256);
            this.Text = "ReferralPrint_Form";
            ((System.ComponentModel.ISupportInitialize)(this.Ref_Grid)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private Label lblCustomer;
        private ComboBox cmbName;
        private DataGridView Ref_Grid;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn Name2;
        private DataGridViewTextBoxColumn Street;
        private DataGridViewTextBoxColumn City;
        private DataGridViewTextBoxColumn State;
        private DataGridViewTextBoxColumn gvtNameIndex;
        private DataGridViewTextBoxColumn Active;
        private Button btnPDFprev;
        private Button btnGenPdf;
        private Label lblDate;
    }
}