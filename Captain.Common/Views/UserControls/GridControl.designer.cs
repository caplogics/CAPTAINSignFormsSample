using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class GridControl
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
            this.gvwControl = new Wisej.Web.DataGridView();
            this.cellCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellDescription = new Wisej.Web.DataGridViewTextBoxColumn();
            this.cellSites = new Wisej.Web.DataGridViewTextBoxColumn();
            this.picAdd = new Wisej.Web.PictureBox();
            this.picEdit = new Wisej.Web.PictureBox();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.lblRequired = new Wisej.Web.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvwControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwControl
            // 
            this.gvwControl.AllowUserToResizeColumns = false;
            this.gvwControl.AllowUserToResizeRows = false;
            this.gvwControl.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwControl.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwControl.BorderStyle = Wisej.Web.BorderStyle.None;
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
            this.cellSites});
            this.gvwControl.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwControl.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwControl.Location = new System.Drawing.Point(5, 5);
            this.gvwControl.MultiSelect = false;
            this.gvwControl.Name = "gvwControl";
            this.gvwControl.ReadOnly = true;
            this.gvwControl.RowHeadersWidth = 15;
            this.gvwControl.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwControl.ShowCellToolTips = false;
            this.gvwControl.ShowColumnVisibilityMenu = false;
            this.gvwControl.Size = new System.Drawing.Size(493, 162);
            this.gvwControl.TabIndex = 0;
            this.gvwControl.MouseClick += new Wisej.Web.MouseEventHandler(this.gvwControl_MouseClick);
            // 
            // cellCode
            // 
            this.cellCode.HeaderText = "Code";
            this.cellCode.Name = "cellCode";
            this.cellCode.ReadOnly = true;
            // 
            // cellDescription
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.cellDescription.DefaultCellStyle = dataGridViewCellStyle2;
            this.cellDescription.HeaderText = "Description";
            this.cellDescription.Name = "cellDescription";
            this.cellDescription.ReadOnly = true;
            this.cellDescription.Width = 250;
            // 
            // cellSites
            // 
            this.cellSites.HeaderText = "Sites";
            this.cellSites.Name = "cellSites";
            this.cellSites.ReadOnly = true;
            this.cellSites.Visible = false;
            this.cellSites.Width = 150;
            // 
            // picAdd
            // 
            this.picAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.picAdd.Dock = Wisej.Web.DockStyle.Top;
            this.picAdd.ImageSource = "captain-add";
            this.picAdd.Location = new System.Drawing.Point(0, 0);
            this.picAdd.Name = "picAdd";
            this.picAdd.Size = new System.Drawing.Size(28, 20);
            this.picAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAdd.Click += new System.EventHandler(this.OnAddClick);
            // 
            // picEdit
            // 
            this.picEdit.Cursor = Wisej.Web.Cursors.Hand;
            this.picEdit.Dock = Wisej.Web.DockStyle.Top;
            this.picEdit.ImageSource = "captain-edit";
            this.picEdit.Location = new System.Drawing.Point(0, 20);
            this.picEdit.Name = "picEdit";
            this.picEdit.Size = new System.Drawing.Size(28, 20);
            this.picEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picEdit.Click += new System.EventHandler(this.OnEditClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvwControl);
            this.panel1.Dock = Wisej.Web.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5);
            this.panel1.Size = new System.Drawing.Size(503, 172);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.picEdit);
            this.panel2.Controls.Add(this.picAdd);
            this.panel2.Location = new System.Drawing.Point(504, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(28, 172);
            this.panel2.TabIndex = 3;
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Location = new System.Drawing.Point(537, 14);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(4, 14);
            this.lblRequired.TabIndex = 4;
            // 
            // GridControl
            // 
            this.Controls.Add(this.lblRequired);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "GridControl";
            this.Size = new System.Drawing.Size(549, 172);
            ((System.ComponentModel.ISupportInitialize)(this.gvwControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView gvwControl;
        private DataGridViewTextBoxColumn cellCode;
        private DataGridViewTextBoxColumn cellDescription;
        private PictureBox picAdd;
        private PictureBox picEdit;
        private DataGridViewTextBoxColumn cellSites;
        private Panel panel1;
        private Panel panel2;
        private Label lblRequired;
    }
}