using Wisej.Web;
using Wisej.Design;

namespace Captain.Common.Views.Forms
{
    partial class FuelTypes_Form
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuelTypes_Form));
            this.btnClose = new Wisej.Web.Button();
            this.btnSelect = new Wisej.Web.Button();
            this.gvFuels = new Wisej.Web.DataGridView();
            this.Check = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Item = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Selected = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlOk = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlFuel = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvFuels)).BeginInit();
            this.pnlOk.SuspendLayout();
            this.pnlFuel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.AppearanceKey = "button-error";
            this.btnClose.Dock = Wisej.Web.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(177, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.AppearanceKey = "button-ok";
            this.btnSelect.Dock = Wisej.Web.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(114, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(60, 25);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "&OK";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // gvFuels
            // 
            this.gvFuels.AllowUserToResizeRows = false;
            this.gvFuels.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(2, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvFuels.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvFuels.ColumnHeadersHeight = 25;
            this.gvFuels.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Check,
            this.Code,
            this.Item,
            this.Selected});
            this.gvFuels.Dock = Wisej.Web.DockStyle.Fill;
            this.gvFuels.Location = new System.Drawing.Point(0, 0);
            this.gvFuels.MultiSelect = false;
            this.gvFuels.Name = "gvFuels";
            this.gvFuels.RowHeadersVisible = false;
            this.gvFuels.RowHeadersWidth = 15;
            this.gvFuels.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvFuels.Size = new System.Drawing.Size(267, 295);
            this.gvFuels.TabIndex = 0;
            this.gvFuels.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvFuels_CellClick);
            // 
            // Check
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.NullValue = false;
            this.Check.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Check.HeaderStyle = dataGridViewCellStyle3;
            this.Check.HeaderText = "  ";
            this.Check.Name = "Check";
            this.Check.ShowInVisibilityMenu = false;
            this.Check.Width = 25;
            // 
            // Code
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Code.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Code.HeaderStyle = dataGridViewCellStyle5;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 60;
            // 
            // Item
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Item.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Item.HeaderStyle = dataGridViewCellStyle7;
            this.Item.HeaderText = "Fuel Types";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Resizable = Wisej.Web.DataGridViewTriState.True;
            this.Item.ShowInVisibilityMenu = false;
            this.Item.Width = 165;
            // 
            // Selected
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Selected.HeaderStyle = dataGridViewCellStyle9;
            this.Selected.HeaderText = "Selected";
            this.Selected.Name = "Selected";
            this.Selected.ReadOnly = true;
            this.Selected.ShowInVisibilityMenu = false;
            this.Selected.Visible = false;
            // 
            // pnlOk
            // 
            this.pnlOk.AppearanceKey = "panel-grdo";
            this.pnlOk.Controls.Add(this.btnSelect);
            this.pnlOk.Controls.Add(this.spacer1);
            this.pnlOk.Controls.Add(this.btnClose);
            this.pnlOk.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOk.Location = new System.Drawing.Point(0, 295);
            this.pnlOk.Name = "pnlOk";
            this.pnlOk.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOk.Size = new System.Drawing.Size(267, 35);
            this.pnlOk.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(174, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlFuel
            // 
            this.pnlFuel.Controls.Add(this.gvFuels);
            this.pnlFuel.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFuel.Location = new System.Drawing.Point(0, 0);
            this.pnlFuel.Name = "pnlFuel";
            this.pnlFuel.Size = new System.Drawing.Size(267, 295);
            this.pnlFuel.TabIndex = 1;
            // 
            // FuelTypes_Form
            // 
            this.ClientSize = new System.Drawing.Size(267, 330);
            this.Controls.Add(this.pnlFuel);
            this.Controls.Add(this.pnlOk);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FuelTypes_Form";
            this.Text = "Fuel Types";
            ((System.ComponentModel.ISupportInitialize)(this.gvFuels)).EndInit();
            this.pnlOk.ResumeLayout(false);
            this.pnlFuel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnClose;
        private Button btnSelect;
        private DataGridView gvFuels;
        private DataGridViewCheckBoxColumn Check;
        private DataGridViewTextBoxColumn Item;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Selected;
        private Panel pnlOk;
        private Spacer spacer1;
        private Panel pnlFuel;
    }
}