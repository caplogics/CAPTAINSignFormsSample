using Wisej.Web;
namespace Captain.Common.Views.UserControls
{
    partial class AdminScreenControls
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle18 = new Wisej.Web.DataGridViewCellStyle();
            this.gvwCateDetails = new Wisej.Web.DataGridView();
            this.gvtCategory = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCatesel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtCateCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvt_sel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvt_MSG = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvwCateHierchy = new Wisej.Web.DataGridView();
            this.gvtCatHierchy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtHierchyDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtcateHierarkey = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlGrids = new Wisej.Web.Panel();
            this.pnlgvwCateDetails = new Wisej.Web.Panel();
            this.pnlgvwCateHierchy = new Wisej.Web.Panel();
            this.pnlNameSearch = new Wisej.Web.Panel();
            this.CmbScreen = new Wisej.Web.ComboBox();
            this.lblScreenName = new Wisej.Web.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateHierchy)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            this.pnlgvwCateDetails.SuspendLayout();
            this.pnlgvwCateHierchy.SuspendLayout();
            this.pnlNameSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvwCateDetails
            // 
            this.gvwCateDetails.AllowUserToResizeColumns = false;
            this.gvwCateDetails.AllowUserToResizeRows = false;
            this.gvwCateDetails.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCateDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwCateDetails.ColumnHeadersHeight = 38;
            this.gvwCateDetails.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCategory,
            this.gvtCatesel,
            this.gvtCateCode,
            this.gvt_sel,
            this.gvt_MSG});
            this.gvwCateDetails.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvwCateDetails.DefaultCellStyle = dataGridViewCellStyle12;
            this.gvwCateDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCateDetails.Location = new System.Drawing.Point(5, 0);
            this.gvwCateDetails.MultiSelect = false;
            this.gvwCateDetails.Name = "gvwCateDetails";
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCateDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.gvwCateDetails.RowHeadersWidth = 15;
            this.gvwCateDetails.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwCateDetails.RowTemplate.Height = 20;
            this.gvwCateDetails.Size = new System.Drawing.Size(1043, 505);
            this.gvwCateDetails.TabIndex = 0;
            // 
            // gvtCategory
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtCategory.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCategory.HeaderStyle = dataGridViewCellStyle3;
            this.gvtCategory.HeaderText = "Screen";
            this.gvtCategory.Name = "gvtCategory";
            this.gvtCategory.ReadOnly = true;
            this.gvtCategory.ShowInVisibilityMenu = false;
            this.gvtCategory.Width = 200;
            // 
            // gvtCatesel
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.NullValue = false;
            this.gvtCatesel.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCatesel.HeaderStyle = dataGridViewCellStyle5;
            this.gvtCatesel.HeaderText = "Message Control Enabled ";
            this.gvtCatesel.Name = "gvtCatesel";
            this.gvtCatesel.ReadOnly = true;
            this.gvtCatesel.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvtCatesel.Width = 120;
            // 
            // gvtCateCode
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCateCode.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCateCode.HeaderStyle = dataGridViewCellStyle7;
            this.gvtCateCode.HeaderText = "gvtCateCode";
            this.gvtCateCode.Name = "gvtCateCode";
            this.gvtCateCode.ShowInVisibilityMenu = false;
            this.gvtCateCode.Visible = false;
            this.gvtCateCode.Width = 50;
            // 
            // gvt_sel
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.NullValue = false;
            dataGridViewCellStyle8.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvt_sel.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvt_sel.HeaderStyle = dataGridViewCellStyle9;
            this.gvt_sel.HeaderText = "Allow Add/Edit";
            this.gvt_sel.Name = "gvt_sel";
            this.gvt_sel.ReadOnly = true;
            this.gvt_sel.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvt_sel.Width = 75;
            // 
            // gvt_MSG
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvt_MSG.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle11.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvt_MSG.HeaderStyle = dataGridViewCellStyle11;
            this.gvt_MSG.HeaderText = "Message";
            this.gvt_MSG.Name = "gvt_MSG";
            this.gvt_MSG.ReadOnly = true;
            this.gvt_MSG.Width = 620;
            // 
            // gvwCateHierchy
            // 
            this.gvwCateHierchy.AllowUserToResizeColumns = false;
            this.gvwCateHierchy.AllowUserToResizeRows = false;
            this.gvwCateHierchy.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwCateHierchy.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwCateHierchy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.gvwCateHierchy.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwCateHierchy.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCatHierchy,
            this.gvtHierchyDesc,
            this.gvtcateHierarkey});
            this.gvwCateHierchy.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.gvwCateHierchy.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCateHierchy.Location = new System.Drawing.Point(0, 0);
            this.gvwCateHierchy.MultiSelect = false;
            this.gvwCateHierchy.Name = "gvwCateHierchy";
            this.gvwCateHierchy.RowHeadersVisible = false;
            this.gvwCateHierchy.RowHeadersWidth = 14;
            this.gvwCateHierchy.RowTemplate.Height = 20;
            this.gvwCateHierchy.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwCateHierchy.Size = new System.Drawing.Size(362, 505);
            this.gvwCateHierchy.TabIndex = 0;
            this.gvwCateHierchy.SelectionChanged += new System.EventHandler(this.gvwCateHierchy_SelectionChanged);
            // 
            // gvtCatHierchy
            // 
            dataGridViewCellStyle15.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle15.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvtCatHierchy.DefaultCellStyle = dataGridViewCellStyle15;
            dataGridViewCellStyle16.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle16.Padding = new Wisej.Web.Padding(10, 0, 0, 0);
            this.gvtCatHierchy.HeaderStyle = dataGridViewCellStyle16;
            this.gvtCatHierchy.HeaderText = "Hierarchy";
            this.gvtCatHierchy.Name = "gvtCatHierchy";
            this.gvtCatHierchy.ReadOnly = true;
            this.gvtCatHierchy.ShowInVisibilityMenu = false;
            this.gvtCatHierchy.Width = 110;
            // 
            // gvtHierchyDesc
            // 
            dataGridViewCellStyle17.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle17.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtHierchyDesc.DefaultCellStyle = dataGridViewCellStyle17;
            dataGridViewCellStyle18.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtHierchyDesc.HeaderStyle = dataGridViewCellStyle18;
            this.gvtHierchyDesc.HeaderText = "Hierarchy Desc";
            this.gvtHierchyDesc.Name = "gvtHierchyDesc";
            this.gvtHierchyDesc.ReadOnly = true;
            this.gvtHierchyDesc.Width = 235;
            // 
            // gvtcateHierarkey
            // 
            this.gvtcateHierarkey.HeaderText = "Hierarchy key";
            this.gvtcateHierarkey.Name = "gvtcateHierarkey";
            this.gvtcateHierarkey.ShowInVisibilityMenu = false;
            this.gvtcateHierarkey.Visible = false;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlGrids);
            this.pnlCompleteForm.Controls.Add(this.pnlNameSearch);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 25);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Padding = new Wisej.Web.Padding(10);
            this.pnlCompleteForm.Size = new System.Drawing.Size(1430, 572);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.pnlgvwCateDetails);
            this.pnlGrids.Controls.Add(this.pnlgvwCateHierchy);
            this.pnlGrids.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(10, 52);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Padding = new Wisej.Web.Padding(0, 5, 0, 0);
            this.pnlGrids.Size = new System.Drawing.Size(1410, 510);
            this.pnlGrids.TabIndex = 7;
            // 
            // pnlgvwCateDetails
            // 
            this.pnlgvwCateDetails.Controls.Add(this.gvwCateDetails);
            this.pnlgvwCateDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwCateDetails.Location = new System.Drawing.Point(362, 5);
            this.pnlgvwCateDetails.Name = "pnlgvwCateDetails";
            this.pnlgvwCateDetails.Padding = new Wisej.Web.Padding(5, 0, 0, 0);
            this.pnlgvwCateDetails.Size = new System.Drawing.Size(1048, 505);
            this.pnlgvwCateDetails.TabIndex = 2;
            // 
            // pnlgvwCateHierchy
            // 
            this.pnlgvwCateHierchy.Controls.Add(this.gvwCateHierchy);
            this.pnlgvwCateHierchy.Dock = Wisej.Web.DockStyle.Left;
            this.pnlgvwCateHierchy.Location = new System.Drawing.Point(0, 5);
            this.pnlgvwCateHierchy.Name = "pnlgvwCateHierchy";
            this.pnlgvwCateHierchy.Size = new System.Drawing.Size(362, 505);
            this.pnlgvwCateHierchy.TabIndex = 1;
            // 
            // pnlNameSearch
            // 
            this.pnlNameSearch.Controls.Add(this.CmbScreen);
            this.pnlNameSearch.Controls.Add(this.lblScreenName);
            this.pnlNameSearch.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlNameSearch.Dock = Wisej.Web.DockStyle.Top;
            this.pnlNameSearch.Location = new System.Drawing.Point(10, 10);
            this.pnlNameSearch.Name = "pnlNameSearch";
            this.pnlNameSearch.Size = new System.Drawing.Size(1410, 42);
            this.pnlNameSearch.TabIndex = 6;
            // 
            // CmbScreen
            // 
            this.CmbScreen.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbScreen.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CmbScreen.FormattingEnabled = true;
            this.CmbScreen.Items.AddRange(new object[] {
            "Client Intake                                             - CASE2001",
            "Income Verification                                   - CASE2003",
            "CASINCOM  - Income Entry",
            "CASE4006   - Service/Activity Details"});
            this.CmbScreen.Location = new System.Drawing.Point(107, 9);
            this.CmbScreen.Name = "CmbScreen";
            this.CmbScreen.Size = new System.Drawing.Size(316, 25);
            this.CmbScreen.TabIndex = 5;
            this.CmbScreen.SelectedIndexChanged += new System.EventHandler(this.CmbScreen_SelectedIndexChanged);
            // 
            // lblScreenName
            // 
            this.lblScreenName.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblScreenName.Location = new System.Drawing.Point(15, 13);
            this.lblScreenName.Name = "lblScreenName";
            this.lblScreenName.Size = new System.Drawing.Size(80, 14);
            this.lblScreenName.TabIndex = 4;
            this.lblScreenName.Text = "Screen Name";
            // 
            // AdminScreenControls
            // 
            this.Controls.Add(this.pnlCompleteForm);
            this.Name = "AdminScreenControls";
            this.Size = new System.Drawing.Size(1430, 597);
            this.Controls.SetChildIndex(this.pnlCompleteForm, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCateHierchy)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlGrids.ResumeLayout(false);
            this.pnlgvwCateDetails.ResumeLayout(false);
            this.pnlgvwCateHierchy.ResumeLayout(false);
            this.pnlNameSearch.ResumeLayout(false);
            this.pnlNameSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView gvwCateDetails;
        private DataGridView gvwCateHierchy;
        private Panel pnlCompleteForm;
        private ComboBox CmbScreen;
        private Label lblScreenName;
        private DataGridViewTextBoxColumn gvtCatHierchy;
        private DataGridViewTextBoxColumn gvtHierchyDesc;
        private DataGridViewTextBoxColumn gvtcateHierarkey;
        private DataGridViewTextBoxColumn gvtCategory;
        private DataGridViewCheckBoxColumn gvtCatesel;
        private DataGridViewTextBoxColumn gvtCateCode;
        private DataGridViewCheckBoxColumn gvt_sel;
        private DataGridViewTextBoxColumn gvt_MSG;
        private Panel pnlGrids;
        private Panel pnlgvwCateDetails;
        private Panel pnlgvwCateHierchy;
        private Panel pnlNameSearch;
    }
}