using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class SiteSearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteSearchForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel2 = new Wisej.Web.Panel();
            this.dataGridSiteSearch = new Wisej.Web.DataGridView();
            this.Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Description = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Active = new Wisej.Web.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnSelect = new Wisej.Web.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSiteSearch)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridSiteSearch);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(469, 373);
            this.panel2.TabIndex = 1;
            this.panel2.TabStop = true;
            // 
            // dataGridSiteSearch
            // 
            this.dataGridSiteSearch.AllowUserToResizeColumns = false;
            this.dataGridSiteSearch.AllowUserToResizeRows = false;
            this.dataGridSiteSearch.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridSiteSearch.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dataGridSiteSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridSiteSearch.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSiteSearch.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Code,
            this.Description,
            this.Active});
            this.dataGridSiteSearch.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridSiteSearch.Location = new System.Drawing.Point(0, 0);
            this.dataGridSiteSearch.Name = "dataGridSiteSearch";
            this.dataGridSiteSearch.RowHeadersWidth = 14;
            this.dataGridSiteSearch.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridSiteSearch.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridSiteSearch.Size = new System.Drawing.Size(469, 373);
            this.dataGridSiteSearch.TabIndex = 0;
            this.dataGridSiteSearch.CellDoubleClick += new Wisej.Web.DataGridViewCellEventHandler(this.dataGridSiteSearch_CellDoubleClick);
            // 
            // Code
            // 
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Code.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Code.HeaderStyle = dataGridViewCellStyle3;
            this.Code.HeaderText = "Code";
            this.Code.MinimumWidth = 30;
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 70;
            // 
            // Description
            // 
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Description.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.NotSet;
            this.Description.HeaderStyle = dataGridViewCellStyle5;
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 100;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 300;
            // 
            // Active
            // 
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.ReadOnly = true;
            this.Active.ShowInVisibilityMenu = false;
            this.Active.Visible = false;
            this.Active.Width = 40;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnSelect);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 373);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(5, 1, 15, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(469, 35);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSelect.Location = new System.Drawing.Point(346, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(100, 27);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // SiteSearchForm
            // 
            this.ClientSize = new System.Drawing.Size(469, 408);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SiteSearchForm";
            this.StartPosition = Wisej.Web.FormStartPosition.CenterScreen;
            this.Text = "Site Search Form";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.SiteSearchForm_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSiteSearch)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel panel2;
        private DataGridView dataGridSiteSearch;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Active;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnSelect;
    }
}