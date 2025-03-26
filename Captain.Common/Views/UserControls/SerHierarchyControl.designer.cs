using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class SerHierarchyControl
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            this.picAdd = new Wisej.Web.PictureBox();
            this.picEdit = new Wisej.Web.PictureBox();
            this.panel1 = new Wisej.Web.Panel();
            this.pnlSerHie = new Wisej.Web.Panel();
            this.gvSerHie = new Wisej.Web.DataGridView();
            this.IntakeCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.IntakeDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SerCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SerDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.line1 = new Wisej.Web.Line();
            this.panel3 = new Wisej.Web.Panel();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlSerHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSerHie)).BeginInit();
            this.gvSerHie.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // picAdd
            // 
            this.picAdd.Cursor = Wisej.Web.Cursors.Hand;
            this.picAdd.Dock = Wisej.Web.DockStyle.Top;
            this.picAdd.ImageSource = "captain-add";
            this.picAdd.Location = new System.Drawing.Point(0, 8);
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
            this.picEdit.Location = new System.Drawing.Point(0, 28);
            this.picEdit.Name = "picEdit";
            this.picEdit.Size = new System.Drawing.Size(28, 20);
            this.picEdit.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picEdit.Click += new System.EventHandler(this.OnEditClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlSerHie);
            this.panel1.Dock = Wisej.Web.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(520, 172);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(555, 175);
            this.panel1.TabIndex = 2;
            // 
            // pnlSerHie
            // 
            this.pnlSerHie.Controls.Add(this.gvSerHie);
            this.pnlSerHie.Controls.Add(this.panel3);
            this.pnlSerHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlSerHie.Location = new System.Drawing.Point(0, 0);
            this.pnlSerHie.Name = "pnlSerHie";
            this.pnlSerHie.Size = new System.Drawing.Size(555, 175);
            this.pnlSerHie.TabIndex = 1;
            // 
            // gvSerHie
            // 
            this.gvSerHie.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvSerHie.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvSerHie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvSerHie.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.IntakeCode,
            this.IntakeDesc,
            this.SerCode,
            this.SerDesc});
            this.gvSerHie.Controls.Add(this.line1);
            this.gvSerHie.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvSerHie.Location = new System.Drawing.Point(5, 24);
            this.gvSerHie.Name = "gvSerHie";
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvSerHie.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gvSerHie.RowHeadersWidth = 15;
            this.gvSerHie.Size = new System.Drawing.Size(542, 146);
            this.gvSerHie.TabIndex = 1;
            this.gvSerHie.MouseClick += new Wisej.Web.MouseEventHandler(this.gvSerHie_MouseClick);
            // 
            // IntakeCode
            // 
            this.IntakeCode.HeaderText = "Code";
            this.IntakeCode.Name = "IntakeCode";
            this.IntakeCode.ReadOnly = true;
            this.IntakeCode.Width = 70;
            // 
            // IntakeDesc
            // 
            dataGridViewCellStyle2.CssStyle = "\"border-right\":\"1px solid #cccccc\"";
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.IntakeDesc.DefaultCellStyle = dataGridViewCellStyle2;
            this.IntakeDesc.HeaderText = "Description";
            this.IntakeDesc.Name = "IntakeDesc";
            this.IntakeDesc.ReadOnly = true;
            this.IntakeDesc.Width = 180;
            // 
            // SerCode
            // 
            dataGridViewCellStyle3.CssStyle = "\"border-left\":\"1px solid #cccccc\"";
            this.SerCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.SerCode.HeaderText = "Code";
            this.SerCode.Name = "SerCode";
            this.SerCode.ReadOnly = true;
            this.SerCode.Width = 70;
            // 
            // SerDesc
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.SerDesc.DefaultCellStyle = dataGridViewCellStyle4;
            this.SerDesc.FillWeight = 180F;
            this.SerDesc.HeaderText = "Description";
            this.SerDesc.Name = "SerDesc";
            this.SerDesc.ReadOnly = true;
            this.SerDesc.Width = 180;
            // 
            // line1
            // 
            this.line1.LineColor = System.Drawing.Color.FromArgb(183, 183, 183);
            this.line1.LineSize = 1;
            this.line1.Location = new System.Drawing.Point(227, 26);
            this.line1.Name = "line1";
            this.line1.Orientation = Wisej.Web.Orientation.Vertical;
            this.line1.Size = new System.Drawing.Size(7, 119);
            this.line1.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = Wisej.Web.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(555, 22);
            this.panel3.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Dock = Wisej.Web.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(262, 0);
            this.label2.MinimumSize = new System.Drawing.Size(0, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Service Plan Hierarchy";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = Wisej.Web.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.MinimumSize = new System.Drawing.Size(0, 18);
            this.label1.Name = "label1";
            this.label1.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(262, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Intake Hierarchy";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.picEdit);
            this.panel2.Controls.Add(this.picAdd);
            this.panel2.Dock = Wisej.Web.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(555, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(0, 8, 0, 0);
            this.panel2.Size = new System.Drawing.Size(28, 175);
            this.panel2.TabIndex = 3;
            // 
            // SerHierarchyControl
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SerHierarchyControl";
            this.Size = new System.Drawing.Size(583, 175);
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdit)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlSerHie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvSerHie)).EndInit();
            this.gvSerHie.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private PictureBox picAdd;
        private PictureBox picEdit;
        private Panel panel1;
        private Panel panel2;
        private Panel pnlSerHie;
        private Line line1;
        private Label label2;
        private Label label1;
        private DataGridView gvSerHie;
        private DataGridViewTextBoxColumn IntakeCode;
        private DataGridViewTextBoxColumn IntakeDesc;
        private DataGridViewTextBoxColumn SerCode;
        private DataGridViewTextBoxColumn SerDesc;
        private Panel panel3;
    }
}