using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class ClientInq_GridControl
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

        #region Visual WebGui UserControl Designer generated code

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
            this.gvwControl = new Wisej.Web.DataGridView();
            this.cellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDescription = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CellPdf = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.CellCasenotes = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.picAdd = new Wisej.Web.PictureBox();
            this.picEdit = new Wisej.Web.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvwControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // gvwControl
            // 
            this.gvwControl.AllowUserToResizeColumns = false;
            this.gvwControl.AllowUserToResizeRows = false;
            this.gvwControl.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwControl.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwControl.ColumnHeadersHeight = 25;
            this.gvwControl.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwControl.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.cellCode,
            this.cellDescription,
            this.CellPdf,
            this.CellCasenotes});
            this.gvwControl.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwControl.Location = new System.Drawing.Point(5, 4);
            this.gvwControl.MultiSelect = false;
            this.gvwControl.Name = "gvwControl";
            this.gvwControl.ReadOnly = true;
            this.gvwControl.RowHeadersWidth = 15;
            this.gvwControl.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwControl.Size = new System.Drawing.Size(624, 173);
            this.gvwControl.TabIndex = 0;
            // 
            // cellCode
            // 
            this.cellCode.HeaderText = "Code";
            this.cellCode.Name = "cellCode";
            this.cellCode.ReadOnly = true;
            this.cellCode.Width = 70;
            // 
            // cellDescription
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.cellDescription.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.cellDescription.HeaderStyle = dataGridViewCellStyle3;
            this.cellDescription.HeaderText = "Description";
            this.cellDescription.Name = "cellDescription";
            this.cellDescription.ReadOnly = true;
            this.cellDescription.Width = 300;
            // 
            // CellPdf
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = false;
            this.CellPdf.DefaultCellStyle = dataGridViewCellStyle4;
            this.CellPdf.HeaderText = "Pdf";
            this.CellPdf.Name = "CellPdf";
            this.CellPdf.ReadOnly = true;
            this.CellPdf.Width = 60;
            // 
            // CellCasenotes
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = false;
            this.CellCasenotes.DefaultCellStyle = dataGridViewCellStyle5;
            this.CellCasenotes.HeaderText = "Case Notes";
            this.CellCasenotes.Name = "CellCasenotes";
            this.CellCasenotes.ReadOnly = true;
            // 
            // picAdd
            // 
            this.picAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.picAdd.ImageSource = "captain-add";
            this.picAdd.Location = new System.Drawing.Point(643, 3);
            this.picAdd.Name = "picAdd";
            this.picAdd.Size = new System.Drawing.Size(20, 20);
            this.picAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAdd.Click += new System.EventHandler(this.OnAddClick);
            // 
            // picEdit
            // 
            this.picEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.picEdit.ImageSource = "captain-edit";
            this.picEdit.Location = new System.Drawing.Point(643, 3);
            this.picEdit.Name = "picEdit";
            this.picEdit.Size = new System.Drawing.Size(20, 20);
            this.picEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picEdit.Click += new System.EventHandler(this.OnEditClick);
            // 
            // ClientInq_GridControl
            // 
            this.Controls.Add(this.picEdit);
            this.Controls.Add(this.picAdd);
            this.Controls.Add(this.gvwControl);
            this.Name = "ClientInq_GridControl";
            this.Size = new System.Drawing.Size(666, 181);
            ((System.ComponentModel.ISupportInitialize)(this.gvwControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvwControl;
        private DataGridViewTextBoxColumn cellCode;
        private DataGridViewTextBoxColumn cellDescription;
        private PictureBox picAdd;
        private PictureBox picEdit;
        private DataGridViewCheckBoxColumn CellPdf;
        private DataGridViewCheckBoxColumn CellCasenotes;


    }
}