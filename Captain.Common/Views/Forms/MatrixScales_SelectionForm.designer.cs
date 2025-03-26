//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class MatrixScales_SelectionForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixScales_SelectionForm));
            this.cmbMatrix = new Wisej.Web.ComboBox();
            this.lblMatrix = new Wisej.Web.Label();
            this.Scales_Grid = new Wisej.Web.DataGridView();
            this.Sel_Img = new Wisej.Web.DataGridViewImageColumn();
            this.btnSelectAll = new Wisej.Web.Button();
            this.btnGenPdf = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.btnUnSelect = new Wisej.Web.Button();
            this.lblAssType = new Wisej.Web.Label();
            this.cmbAssessmentType = new Wisej.Web.ComboBox();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlScaleGrid = new Wisej.Web.Panel();
            this.pnlMatrix = new Wisej.Web.Panel();
            this.pnlPDF = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Scale_Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Scale_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Selected = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ScaleAssType = new Wisej.Web.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Scales_Grid)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlScaleGrid.SuspendLayout();
            this.pnlMatrix.SuspendLayout();
            this.pnlPDF.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbMatrix
            // 
            this.cmbMatrix.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMatrix.FormattingEnabled = true;
            this.cmbMatrix.Location = new System.Drawing.Point(129, 8);
            this.cmbMatrix.Name = "cmbMatrix";
            this.cmbMatrix.Size = new System.Drawing.Size(313, 25);
            this.cmbMatrix.TabIndex = 2;
            this.cmbMatrix.SelectedIndexChanged += new System.EventHandler(this.cmbMatrix_SelectedIndexChanged);
            // 
            // lblMatrix
            // 
            this.lblMatrix.Location = new System.Drawing.Point(15, 12);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(37, 14);
            this.lblMatrix.TabIndex = 0;
            this.lblMatrix.Text = "Matrix ";
            // 
            // Scales_Grid
            // 
            this.Scales_Grid.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Scales_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Scales_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Scales_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Sel_Img,
            this.Scale_Desc,
            this.Scale_Code,
            this.Selected,
            this.ScaleAssType});
            this.Scales_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.Scales_Grid.Location = new System.Drawing.Point(0, 0);
            this.Scales_Grid.MultiSelect = false;
            this.Scales_Grid.Name = "Scales_Grid";
            this.Scales_Grid.ReadOnly = true;
            this.Scales_Grid.RowHeadersWidth = 25;
            this.Scales_Grid.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Scales_Grid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Scales_Grid.Size = new System.Drawing.Size(499, 310);
            this.Scales_Grid.TabIndex = 5;
            this.Scales_Grid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.Scales_Grid_CellClick);
            // 
            // Sel_Img
            // 
            this.Sel_Img.CellImageAlignment = Wisej.Web.DataGridViewContentAlignment.NotSet;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = null;
            this.Sel_Img.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Sel_Img.HeaderStyle = dataGridViewCellStyle3;
            this.Sel_Img.HeaderText = " ";
            this.Sel_Img.Name = "Sel_Img";
            this.Sel_Img.ReadOnly = true;
            this.Sel_Img.ShowInVisibilityMenu = false;
            this.Sel_Img.Width = 25;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Dock = Wisej.Web.DockStyle.Left;
            this.btnSelectAll.Location = new System.Drawing.Point(15, 5);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 25);
            this.btnSelectAll.TabIndex = 7;
            this.btnSelectAll.Text = "&Select All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnGenPdf
            // 
            this.btnGenPdf.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenPdf.Location = new System.Drawing.Point(316, 5);
            this.btnGenPdf.Name = "btnGenPdf";
            this.btnGenPdf.Size = new System.Drawing.Size(90, 25);
            this.btnGenPdf.TabIndex = 9;
            this.btnGenPdf.Text = "&Generate PDF";
            this.btnGenPdf.Click += new System.EventHandler(this.btnGenPdf_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(409, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUnSelect
            // 
            this.btnUnSelect.Dock = Wisej.Web.DockStyle.Left;
            this.btnUnSelect.Location = new System.Drawing.Point(93, 5);
            this.btnUnSelect.Name = "btnUnSelect";
            this.btnUnSelect.Size = new System.Drawing.Size(85, 25);
            this.btnUnSelect.TabIndex = 8;
            this.btnUnSelect.Text = "&Unselect All";
            this.btnUnSelect.Click += new System.EventHandler(this.btnUnSelect_Click);
            // 
            // lblAssType
            // 
            this.lblAssType.Location = new System.Drawing.Point(15, 43);
            this.lblAssType.Name = "lblAssType";
            this.lblAssType.Size = new System.Drawing.Size(99, 16);
            this.lblAssType.TabIndex = 0;
            this.lblAssType.Text = "Assessment Type ";
            this.lblAssType.Visible = false;
            // 
            // cmbAssessmentType
            // 
            this.cmbAssessmentType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAssessmentType.FormattingEnabled = true;
            this.cmbAssessmentType.Location = new System.Drawing.Point(129, 39);
            this.cmbAssessmentType.Name = "cmbAssessmentType";
            this.cmbAssessmentType.Size = new System.Drawing.Size(108, 25);
            this.cmbAssessmentType.TabIndex = 3;
            this.cmbAssessmentType.Visible = false;
            this.cmbAssessmentType.SelectedIndexChanged += new System.EventHandler(this.cmbAssessmentType_SelectedIndexChanged);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlScaleGrid);
            this.pnlCompleteForm.Controls.Add(this.pnlMatrix);
            this.pnlCompleteForm.Controls.Add(this.pnlPDF);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(499, 416);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlScaleGrid
            // 
            this.pnlScaleGrid.Controls.Add(this.Scales_Grid);
            this.pnlScaleGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlScaleGrid.Location = new System.Drawing.Point(0, 71);
            this.pnlScaleGrid.Name = "pnlScaleGrid";
            this.pnlScaleGrid.Size = new System.Drawing.Size(499, 310);
            this.pnlScaleGrid.TabIndex = 4;
            // 
            // pnlMatrix
            // 
            this.pnlMatrix.Controls.Add(this.cmbAssessmentType);
            this.pnlMatrix.Controls.Add(this.lblAssType);
            this.pnlMatrix.Controls.Add(this.cmbMatrix);
            this.pnlMatrix.Controls.Add(this.lblMatrix);
            this.pnlMatrix.Dock = Wisej.Web.DockStyle.Top;
            this.pnlMatrix.Location = new System.Drawing.Point(0, 0);
            this.pnlMatrix.Name = "pnlMatrix";
            this.pnlMatrix.Size = new System.Drawing.Size(499, 71);
            this.pnlMatrix.TabIndex = 1;
            // 
            // pnlPDF
            // 
            this.pnlPDF.AppearanceKey = "panel-grdo";
            this.pnlPDF.Controls.Add(this.btnUnSelect);
            this.pnlPDF.Controls.Add(this.spacer2);
            this.pnlPDF.Controls.Add(this.btnSelectAll);
            this.pnlPDF.Controls.Add(this.btnGenPdf);
            this.pnlPDF.Controls.Add(this.spacer1);
            this.pnlPDF.Controls.Add(this.btnCancel);
            this.pnlPDF.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlPDF.Location = new System.Drawing.Point(0, 381);
            this.pnlPDF.Name = "pnlPDF";
            this.pnlPDF.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlPDF.Size = new System.Drawing.Size(499, 35);
            this.pnlPDF.TabIndex = 6;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(90, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(406, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // Scale_Desc
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Scale_Desc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Scale_Desc.HeaderStyle = dataGridViewCellStyle5;
            this.Scale_Desc.HeaderText = "Scale Description";
            this.Scale_Desc.Name = "Scale_Desc";
            this.Scale_Desc.ReadOnly = true;
            this.Scale_Desc.Width = 420;
            // 
            // Scale_Code
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Scale_Code.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Scale_Code.HeaderStyle = dataGridViewCellStyle7;
            this.Scale_Code.HeaderText = "Scale_Code";
            this.Scale_Code.Name = "Scale_Code";
            this.Scale_Code.ReadOnly = true;
            this.Scale_Code.ShowInVisibilityMenu = false;
            this.Scale_Code.Visible = false;
            this.Scale_Code.Width = 40;
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
            this.Selected.ReadOnly = true;
            this.Selected.ShowInVisibilityMenu = false;
            this.Selected.Visible = false;
            this.Selected.Width = 40;
            // 
            // ScaleAssType
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ScaleAssType.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ScaleAssType.HeaderStyle = dataGridViewCellStyle11;
            this.ScaleAssType.HeaderText = "ScaleAssType";
            this.ScaleAssType.Name = "ScaleAssType";
            this.ScaleAssType.ReadOnly = true;
            this.ScaleAssType.ShowInVisibilityMenu = false;
            this.ScaleAssType.Visible = false;
            this.ScaleAssType.Width = 20;
            // 
            // MatrixScales_SelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(499, 416);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatrixScales_SelectionForm";
            this.Text = "Matrix Scales - SelectionForm";
            ((System.ComponentModel.ISupportInitialize)(this.Scales_Grid)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlScaleGrid.ResumeLayout(false);
            this.pnlMatrix.ResumeLayout(false);
            this.pnlMatrix.PerformLayout();
            this.pnlPDF.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox cmbMatrix;
        private Label lblMatrix;
        private DataGridView Scales_Grid;
        private DataGridViewImageColumn Sel_Img;
        private DataGridViewTextBoxColumn Scale_Desc;
        private DataGridViewTextBoxColumn Scale_Code;
        private DataGridViewTextBoxColumn Selected;
        private Button btnSelectAll;
        private Button btnGenPdf;
        private Button btnCancel;
        private Button btnUnSelect;
        private DataGridViewTextBoxColumn ScaleAssType;
        private Label lblAssType;
        private ComboBox cmbAssessmentType;
        private Panel pnlCompleteForm;
        private Panel pnlPDF;
        private Spacer spacer2;
        private Spacer spacer1;
        private Panel pnlScaleGrid;
        private Panel pnlMatrix;
    }
}