using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class PrivilegesControl
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
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            this.gvwControl = new Wisej.Web.DataGridView();
            this.picEdit = new Wisej.Web.PictureBox();
            this.picAdd = new Wisej.Web.PictureBox();
            this.folderBrowserDialog1 = new Wisej.Web.FolderBrowserDialog(this.components);
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.cellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDescription = new Wisej.Web.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvwControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwControl
            // 
            this.gvwControl.AllowUserToResizeColumns = false;
            this.gvwControl.AllowUserToResizeRows = false;
            this.gvwControl.AutoGenerateColumns = false;
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
            this.cellDescription});
            this.gvwControl.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwControl.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwControl.Location = new System.Drawing.Point(2, 2);
            this.gvwControl.MultiSelect = false;
            this.gvwControl.Name = "gvwControl";
            this.gvwControl.ReadOnly = true;
            this.gvwControl.RowHeadersWidth = 15;
            this.gvwControl.Size = new System.Drawing.Size(837, 175);
            this.gvwControl.TabIndex = 0;
            // 
            // picEdit
            // 
            this.picEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.picEdit.Dock = Wisej.Web.DockStyle.Top;
            this.picEdit.ImageSource = "captain-edit";
            this.picEdit.Location = new System.Drawing.Point(0, 20);
            this.picEdit.Name = "picEdit";
            this.picEdit.Size = new System.Drawing.Size(29, 20);
            this.picEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picEdit.Click += new System.EventHandler(this.OnEditClick);
            // 
            // picAdd
            // 
            this.picAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.picAdd.Dock = Wisej.Web.DockStyle.Top;
            this.picAdd.ImageSource = "captain-add";
            this.picAdd.Location = new System.Drawing.Point(0, 0);
            this.picAdd.Name = "picAdd";
            this.picAdd.Size = new System.Drawing.Size(29, 20);
            this.picAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAdd.Click += new System.EventHandler(this.OnAddClick);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "folderBrowserDialog1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.picEdit);
            this.panel1.Controls.Add(this.picAdd);
            this.panel1.Location = new System.Drawing.Point(843, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(29, 179);
            this.panel1.TabIndex = 1;
            this.panel1.Text = "`";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvwControl);
            this.panel2.Dock = Wisej.Web.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(2);
            this.panel2.Size = new System.Drawing.Size(841, 179);
            this.panel2.TabIndex = 2;
            // 
            // cellCode
            // 
            this.cellCode.HeaderText = "Code";
            this.cellCode.Name = "cellCode";
            this.cellCode.ReadOnly = true;
            // 
            // cellDescription
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.cellDescription.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.cellDescription.HeaderStyle = dataGridViewCellStyle3;
            this.cellDescription.HeaderText = "Description";
            this.cellDescription.Name = "cellDescription";
            this.cellDescription.ReadOnly = true;
            this.cellDescription.Width = 350;
            // 
            // PrivilegesControl
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "PrivilegesControl";
            this.Padding = new Wisej.Web.Padding(3);
            this.Size = new System.Drawing.Size(875, 185);
            ((System.ComponentModel.ISupportInitialize)(this.gvwControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gvwControl;
        private DataGridViewTextBoxColumn cellCode;
        private DataGridViewTextBoxColumn cellDescription;
        private PictureBox picEdit;
        private PictureBox picAdd;
        private FolderBrowserDialog folderBrowserDialog1;
        private Panel panel1;
        private Panel panel2;
    }
}