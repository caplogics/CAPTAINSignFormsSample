using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class CASB2530View
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASB2530View));
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            this.listViewRanks = new Wisej.Web.ListView();
            this.lvtMain = new Wisej.Web.ColumnHeader();
            this.lvtName = new Wisej.Web.ColumnHeader();
            this.lvtPoints = new Wisej.Web.ColumnHeader();
            this.lvtRiskDesc = new Wisej.Web.ColumnHeader();
            this.lvtViews = new Wisej.Web.ColumnHeader();
            this.btnUpdateRanks = new Wisej.Web.Button();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.dataGridViewRanks = new Wisej.Web.DataGridView();
            this.colName = new Wisej.Web.DataGridViewColumn();
            this.colPoints = new Wisej.Web.DataGridViewColumn();
            this.colRiskDesc = new Wisej.Web.DataGridViewColumn();
            this.colCode = new Wisej.Web.DataGridViewColumn();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRanks)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewRanks
            // 
            this.listViewRanks.BorderStyle = Wisej.Web.BorderStyle.None;
            this.listViewRanks.Columns.AddRange(new Wisej.Web.ColumnHeader[] {
            this.lvtMain,
            this.lvtName,
            this.lvtPoints,
            this.lvtRiskDesc,
            this.lvtViews});
            this.listViewRanks.Dock = Wisej.Web.DockStyle.Fill;
            this.listViewRanks.Location = new System.Drawing.Point(0, 0);
            this.listViewRanks.Name = "listViewRanks";
            this.listViewRanks.Size = new System.Drawing.Size(896, 405);
            this.listViewRanks.TabIndex = 0;
            this.listViewRanks.View = Wisej.Web.View.Details;
            this.listViewRanks.Visible = false;
            // 
            // lvtMain
            // 
            this.lvtMain.Name = "lvtMain";
            this.lvtMain.Width = 20;
            // 
            // lvtName
            // 
            this.lvtName.Name = "lvtName";
            this.lvtName.Text = "Name";
            this.lvtName.Width = 250;
            // 
            // lvtPoints
            // 
            this.lvtPoints.Name = "lvtPoints";
            this.lvtPoints.Text = "Points";
            this.lvtPoints.Width = 70;
            // 
            // lvtRiskDesc
            // 
            this.lvtRiskDesc.Name = "lvtRiskDesc";
            this.lvtRiskDesc.Text = "Rank";
            this.lvtRiskDesc.Width = 270;
            // 
            // lvtViews
            // 
            this.lvtViews.Name = "lvtViews";
            this.lvtViews.Text = " ";
            this.lvtViews.Width = 22;
            // 
            // btnUpdateRanks
            // 
            this.btnUpdateRanks.AutoSize = true;
            this.btnUpdateRanks.Dock = Wisej.Web.DockStyle.Right;
            this.btnUpdateRanks.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnUpdateRanks.Location = new System.Drawing.Point(785, 6);
            this.btnUpdateRanks.Name = "btnUpdateRanks";
            this.btnUpdateRanks.Size = new System.Drawing.Size(102, 21);
            this.btnUpdateRanks.TabIndex = 1;
            this.btnUpdateRanks.Text = "Update Ranks";
            this.btnUpdateRanks.Click += new System.EventHandler(this.btnUpdateRanks_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnUpdateRanks);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 405);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(896, 35);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // dataGridViewRanks
            // 
            this.dataGridViewRanks.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewRanks.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.colName,
            this.colPoints,
            this.colRiskDesc,
            this.colCode});
            this.dataGridViewRanks.Dock = Wisej.Web.DockStyle.Fill;
            this.dataGridViewRanks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRanks.Name = "dataGridViewRanks";
            this.dataGridViewRanks.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewRanks.ShowColumnVisibilityMenu = false;
            this.dataGridViewRanks.Size = new System.Drawing.Size(896, 405);
            this.dataGridViewRanks.TabIndex = 3;
            // 
            // colName
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.colName.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.colName.HeaderStyle = dataGridViewCellStyle2;
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ResponsiveProfiles.Add(((Wisej.Base.ResponsiveProfile)(resources.GetObject("colName.ResponsiveProfiles"))));
            this.colName.Width = 400;
            // 
            // colPoints
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.colPoints.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.colPoints.HeaderStyle = dataGridViewCellStyle4;
            this.colPoints.HeaderText = "Points";
            this.colPoints.Name = "colPoints";
            this.colPoints.Width = 70;
            // 
            // colRiskDesc
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.colRiskDesc.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.colRiskDesc.HeaderStyle = dataGridViewCellStyle6;
            this.colRiskDesc.HeaderText = "Rank";
            this.colRiskDesc.Name = "colRiskDesc";
            this.colRiskDesc.Width = 270;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "";
            this.colCode.Name = "colCode";
            this.colCode.ShowInVisibilityMenu = false;
            this.colCode.Visible = false;
            this.colCode.Width = 10;
            // 
            // CASB2530View
            // 
            this.ClientSize = new System.Drawing.Size(896, 440);
            this.Controls.Add(this.dataGridViewRanks);
            this.Controls.Add(this.listViewRanks);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASB2530View";
            this.Text = "CASB2530View";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRanks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ListView listViewRanks;
        private ColumnHeader lvtMain;
        private ColumnHeader lvtName;
        private ColumnHeader lvtPoints;
        private ColumnHeader lvtViews;
        private Button btnUpdateRanks;
        private ColumnHeader lvtRiskDesc;
        private FlowLayoutPanel flowLayoutPanel1;
		private DataGridView dataGridViewRanks;
		private DataGridViewColumn colName;
		private DataGridViewColumn colPoints;
		private DataGridViewColumn colRiskDesc;
		private DataGridViewColumn colCode;
	}
}