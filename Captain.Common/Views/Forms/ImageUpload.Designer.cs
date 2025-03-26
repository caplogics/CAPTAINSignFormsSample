using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class ImageUpload
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
            this.components = new System.ComponentModel.Container();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle10 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle11 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle18 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle19 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle20 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle21 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageUpload));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.panel3 = new Wisej.Web.Panel();
            this.gvwDoclist = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.gvtdocimageType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtAssigndoc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtdocDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.gvtSecurity = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtdocId = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtdocdate1 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtimgType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.contextmenuTypes = new Wisej.Web.ContextMenu(this.components);
            this.panel6 = new Wisej.Web.Panel();
            this.btnDelete = new Wisej.Web.Button();
            this.btnPreview = new Wisej.Web.Button();
            this.openFileDialog1 = new Wisej.Web.OpenFileDialog(this.components);
            this.panel4 = new Wisej.Web.Panel();
            this.panel5 = new Wisej.Web.Panel();
            this.rdoUPloadMode = new Wisej.Web.RadioButton();
            this.rdoViewMode = new Wisej.Web.RadioButton();
            this.chkHouseHold = new Wisej.Web.CheckBox();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnBrowse = new Wisej.Web.Button();
            this.M_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SSNNO = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Relation = new Wisej.Web.DataGridViewTextBoxColumn();
            this.MemberName = new Wisej.Web.DataGridViewTextBoxColumn();
            this.SNP_FAMILY_SEQ = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridCaseSnp = new Wisej.Web.DataGridView();
            this.Mem_Seq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Other_Hie_Access = new Wisej.Web.DataGridViewTextBoxColumn();
            this.ImagesCount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Program = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Year = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Hierarchy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.dataGridHierchys = new Wisej.Web.DataGridView();
            this.panel1 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.btnUpload = new Wisej.Web.Button();
            this.uploadBrowser = new Wisej.Web.Upload();
            this.panel7 = new Wisej.Web.Panel();
            this.txtUploadAs = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label2 = new Wisej.Web.Label();
            this.lblImgReq = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.lblFileName = new Wisej.Web.Label();
            this.cmbImageType = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.txtUploadFileName = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblImageTypes = new Wisej.Web.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwDoclist)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCaseSnp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHierchys)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.lblImgReq.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvwDoclist);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 485);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(812, 144);
            this.panel3.TabIndex = 1;
            this.panel3.TabStop = true;
            // 
            // gvwDoclist
            // 
            this.gvwDoclist.BackColor = System.Drawing.Color.White;
            this.gvwDoclist.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwDoclist.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwDoclist.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwDoclist.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtdocimageType,
            this.gvtAssigndoc,
            this.gvtdocDate,
            this.gvtSecurity,
            this.gvtdocId,
            this.gvtdocdate1,
            this.gvtimgType});
            this.gvwDoclist.ContextMenu = this.contextmenuTypes;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("default", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = Wisej.Web.DataGridViewTriState.False;
            this.gvwDoclist.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvwDoclist.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwDoclist.Location = new System.Drawing.Point(82, 0);
            this.gvwDoclist.MultiSelect = false;
            this.gvwDoclist.Name = "gvwDoclist";
            this.gvwDoclist.RowHeadersWidth = 14;
            this.gvwDoclist.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwDoclist.RowTemplate.Height = 20;
            this.gvwDoclist.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwDoclist.Size = new System.Drawing.Size(730, 144);
            this.gvwDoclist.TabIndex = 1;
            // 
            // gvtdocimageType
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtdocimageType.HeaderStyle = dataGridViewCellStyle2;
            this.gvtdocimageType.HeaderText = "Doc/Image Type";
            this.gvtdocimageType.Name = "gvtdocimageType";
            this.gvtdocimageType.ReadOnly = true;
            this.gvtdocimageType.Width = 200;
            // 
            // gvtAssigndoc
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtAssigndoc.HeaderStyle = dataGridViewCellStyle3;
            this.gvtAssigndoc.HeaderText = "Assigned Docs/Images";
            this.gvtAssigndoc.Name = "gvtAssigndoc";
            this.gvtAssigndoc.ReadOnly = true;
            this.gvtAssigndoc.Width = 280;
            // 
            // gvtdocDate
            // 
            this.gvtdocDate.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.gvtdocDate.HeaderStyle = dataGridViewCellStyle5;
            this.gvtdocDate.HeaderText = "Date";
            this.gvtdocDate.Name = "gvtdocDate";
            this.gvtdocDate.ReadOnly = true;
            this.gvtdocDate.Width = 116;
            // 
            // gvtSecurity
            // 
            this.gvtSecurity.HeaderText = "gvtSecurity";
            this.gvtSecurity.Name = "gvtSecurity";
            this.gvtSecurity.ShowInVisibilityMenu = false;
            this.gvtSecurity.Visible = false;
            this.gvtSecurity.Width = 10;
            // 
            // gvtdocId
            // 
            this.gvtdocId.HeaderText = "gvtdocId";
            this.gvtdocId.Name = "gvtdocId";
            this.gvtdocId.ShowInVisibilityMenu = false;
            this.gvtdocId.Visible = false;
            this.gvtdocId.Width = 10;
            // 
            // gvtdocdate1
            // 
            this.gvtdocdate1.HeaderText = "gvtdocdate1";
            this.gvtdocdate1.Name = "gvtdocdate1";
            this.gvtdocdate1.ShowInVisibilityMenu = false;
            this.gvtdocdate1.Visible = false;
            this.gvtdocdate1.Width = 10;
            // 
            // gvtimgType
            // 
            this.gvtimgType.HeaderText = "gvtimgType";
            this.gvtimgType.Name = "gvtimgType";
            this.gvtimgType.ShowInVisibilityMenu = false;
            this.gvtimgType.Visible = false;
            this.gvtimgType.Width = 10;
            // 
            // contextmenuTypes
            // 
            this.contextmenuTypes.Name = "contextmenuTypes";
            this.contextmenuTypes.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextmenuTypes.Popup += new System.EventHandler(this.contextmenuTypes_Popup);
            this.contextmenuTypes.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.gvwDoclist_MenuClick);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnDelete);
            this.panel6.Controls.Add(this.btnPreview);
            this.panel6.Dock = Wisej.Web.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(82, 144);
            this.panel6.TabIndex = 2;
            this.panel6.TabStop = true;
            // 
            // btnDelete
            // 
            this.btnDelete.AppearanceKey = "button-error";
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(8, 71);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(66, 68);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.AppearanceKey = "button-ok";
            this.btnPreview.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPreview.Location = new System.Drawing.Point(8, 3);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(66, 65);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "Pre&view";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.chkHouseHold);
            this.panel4.Dock = Wisej.Web.DockStyle.Top;
            this.panel4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(812, 45);
            this.panel4.TabIndex = 2;
            this.panel4.TabStop = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.rdoUPloadMode);
            this.panel5.Controls.Add(this.rdoViewMode);
            this.panel5.Dock = Wisej.Web.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(519, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(293, 45);
            this.panel5.TabIndex = 9;
            this.panel5.TabStop = true;
            // 
            // rdoUPloadMode
            // 
            this.rdoUPloadMode.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rdoUPloadMode.Location = new System.Drawing.Point(158, 9);
            this.rdoUPloadMode.Name = "rdoUPloadMode";
            this.rdoUPloadMode.Size = new System.Drawing.Size(104, 21);
            this.rdoUPloadMode.TabIndex = 0;
            this.rdoUPloadMode.Text = "Upload Mode";
            this.rdoUPloadMode.CheckedChanged += new System.EventHandler(this.rdoUPloadMode_CheckedChanged);
            // 
            // rdoViewMode
            // 
            this.rdoViewMode.Checked = true;
            this.rdoViewMode.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rdoViewMode.Location = new System.Drawing.Point(8, 9);
            this.rdoViewMode.Name = "rdoViewMode";
            this.rdoViewMode.Size = new System.Drawing.Size(92, 21);
            this.rdoViewMode.TabIndex = 0;
            this.rdoViewMode.TabStop = true;
            this.rdoViewMode.Text = "View Mode";
            this.rdoViewMode.CheckedChanged += new System.EventHandler(this.rdoViewMode_CheckedChanged);
            // 
            // chkHouseHold
            // 
            this.chkHouseHold.AutoSize = false;
            this.chkHouseHold.CheckState = Wisej.Web.CheckState.Checked;
            this.chkHouseHold.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkHouseHold.Location = new System.Drawing.Point(4, 8);
            this.chkHouseHold.Name = "chkHouseHold";
            this.chkHouseHold.Size = new System.Drawing.Size(226, 24);
            this.chkHouseHold.TabIndex = 8;
            this.chkHouseHold.Text = "Household Member Images";
            this.chkHouseHold.CheckedChanged += new System.EventHandler(this.chkHouseHold_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromName("@control");
            this.flowLayoutPanel1.Controls.Add(this.btnBrowse);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 629);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(5, 2, 5, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(812, 35);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Dock = Wisej.Web.DockStyle.Right;
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnBrowse.Location = new System.Drawing.Point(699, 5);
            this.btnBrowse.MaximumSize = new System.Drawing.Size(0, 25);
            this.btnBrowse.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 25);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // M_Code
            // 
            this.M_Code.HeaderText = "M_Code";
            this.M_Code.Name = "M_Code";
            this.M_Code.ReadOnly = true;
            this.M_Code.ShowInVisibilityMenu = false;
            this.M_Code.SortMode = Wisej.Web.DataGridViewColumnSortMode.Programmatic;
            this.M_Code.Visible = false;
            this.M_Code.Width = 10;
            // 
            // SSNNO
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.SSNNO.DefaultCellStyle = dataGridViewCellStyle7;
            this.SSNNO.HeaderText = "SS#";
            this.SSNNO.Name = "SSNNO";
            this.SSNNO.ReadOnly = true;
            this.SSNNO.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.SSNNO.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.SSNNO.Visible = false;
            this.SSNNO.Width = 80;
            // 
            // Relation
            // 
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Relation.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Relation.HeaderStyle = dataGridViewCellStyle9;
            this.Relation.HeaderText = "Relation";
            this.Relation.Name = "Relation";
            this.Relation.ReadOnly = true;
            this.Relation.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.Relation.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.Relation.Width = 340;
            // 
            // MemberName
            // 
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.MemberName.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.WrapMode = Wisej.Web.DataGridViewTriState.False;
            this.MemberName.HeaderStyle = dataGridViewCellStyle11;
            this.MemberName.HeaderText = "Member Name";
            this.MemberName.Name = "MemberName";
            this.MemberName.ReadOnly = true;
            this.MemberName.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.MemberName.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.MemberName.Width = 350;
            // 
            // SNP_FAMILY_SEQ
            // 
            this.SNP_FAMILY_SEQ.DataPropertyName = "SNP_FAMILY_SEQ";
            this.SNP_FAMILY_SEQ.HeaderText = " ";
            this.SNP_FAMILY_SEQ.Name = "SNP_FAMILY_SEQ";
            this.SNP_FAMILY_SEQ.ReadOnly = true;
            this.SNP_FAMILY_SEQ.ShowInVisibilityMenu = false;
            this.SNP_FAMILY_SEQ.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.SNP_FAMILY_SEQ.Visible = false;
            this.SNP_FAMILY_SEQ.Width = 10;
            // 
            // dataGridCaseSnp
            // 
            this.dataGridCaseSnp.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridCaseSnp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridCaseSnp.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCaseSnp.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.SNP_FAMILY_SEQ,
            this.MemberName,
            this.Relation,
            this.SSNNO,
            this.M_Code});
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dataGridCaseSnp.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridCaseSnp.Dock = Wisej.Web.DockStyle.Top;
            this.dataGridCaseSnp.Location = new System.Drawing.Point(0, 0);
            this.dataGridCaseSnp.MultiSelect = false;
            this.dataGridCaseSnp.Name = "dataGridCaseSnp";
            this.dataGridCaseSnp.RowHeadersWidth = 15;
            this.dataGridCaseSnp.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridCaseSnp.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridCaseSnp.Size = new System.Drawing.Size(812, 115);
            this.dataGridCaseSnp.TabIndex = 1;
            this.dataGridCaseSnp.SelectionChanged += new System.EventHandler(this.dataGridCaseSnp_SelectionChanged);
            // 
            // Mem_Seq
            // 
            this.Mem_Seq.HeaderText = "Seq";
            this.Mem_Seq.Name = "Mem_Seq";
            this.Mem_Seq.ReadOnly = true;
            this.Mem_Seq.ShowInVisibilityMenu = false;
            this.Mem_Seq.Visible = false;
            this.Mem_Seq.Width = 40;
            // 
            // Other_Hie_Access
            // 
            this.Other_Hie_Access.HeaderText = "Other_Hie_Access";
            this.Other_Hie_Access.Name = "Other_Hie_Access";
            this.Other_Hie_Access.ReadOnly = true;
            this.Other_Hie_Access.ShowInVisibilityMenu = false;
            this.Other_Hie_Access.Visible = false;
            // 
            // ImagesCount
            // 
            this.ImagesCount.HeaderText = "# of Images";
            this.ImagesCount.Name = "ImagesCount";
            this.ImagesCount.ReadOnly = true;
            this.ImagesCount.Width = 150;
            // 
            // Program
            // 
            dataGridViewCellStyle13.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Program.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Program.HeaderStyle = dataGridViewCellStyle14;
            this.Program.HeaderText = "Program";
            this.Program.Name = "Program";
            this.Program.ReadOnly = true;
            this.Program.Width = 380;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle15;
            dataGridViewCellStyle16.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn9.HeaderStyle = dataGridViewCellStyle16;
            this.dataGridViewTextBoxColumn9.HeaderText = "App#";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 90;
            // 
            // Year
            // 
            dataGridViewCellStyle17.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Year.DefaultCellStyle = dataGridViewCellStyle17;
            dataGridViewCellStyle18.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Year.HeaderStyle = dataGridViewCellStyle18;
            this.Year.HeaderText = "Year";
            this.Year.Name = "Year";
            this.Year.ReadOnly = true;
            this.Year.Width = 60;
            // 
            // Hierarchy
            // 
            dataGridViewCellStyle19.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Hierarchy.DefaultCellStyle = dataGridViewCellStyle19;
            dataGridViewCellStyle20.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Hierarchy.HeaderStyle = dataGridViewCellStyle20;
            this.Hierarchy.HeaderText = "Hierarchy";
            this.Hierarchy.Name = "Hierarchy";
            this.Hierarchy.ReadOnly = true;
            this.Hierarchy.Width = 100;
            // 
            // dataGridHierchys
            // 
            this.dataGridHierchys.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridHierchys.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle21.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.dataGridHierchys.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridHierchys.ColumnHeadersHeight = 25;
            this.dataGridHierchys.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Hierarchy,
            this.Year,
            this.dataGridViewTextBoxColumn9,
            this.Program,
            this.ImagesCount,
            this.Other_Hie_Access,
            this.Mem_Seq});
            this.dataGridHierchys.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataGridHierchys.Location = new System.Drawing.Point(0, 251);
            this.dataGridHierchys.MultiSelect = false;
            this.dataGridHierchys.Name = "dataGridHierchys";
            this.dataGridHierchys.RowHeadersWidth = 15;
            this.dataGridHierchys.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridHierchys.RowTemplate.Height = 20;
            this.dataGridHierchys.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.dataGridHierchys.Size = new System.Drawing.Size(812, 189);
            this.dataGridHierchys.TabIndex = 6;
            this.dataGridHierchys.SelectionChanged += new System.EventHandler(this.dataGridHierchys_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.dataGridHierchys);
            this.panel1.Controls.Add(this.dataGridCaseSnp);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(812, 440);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUpload);
            this.panel2.Controls.Add(this.uploadBrowser);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 115);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(812, 136);
            this.panel2.TabIndex = 7;
            this.panel2.TabStop = true;
            // 
            // btnUpload
            // 
            this.btnUpload.AppearanceKey = "button-ok";
            this.btnUpload.Enabled = false;
            this.btnUpload.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnUpload.Location = new System.Drawing.Point(652, 57);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(146, 40);
            this.btnUpload.TabIndex = 7;
            this.btnUpload.Text = "&Upload";
            this.btnUpload.Visible = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // uploadBrowser
            // 
            this.uploadBrowser.AllowedFileTypes = "file_extension|image/*";
            this.uploadBrowser.HideValue = true;
            this.uploadBrowser.Location = new System.Drawing.Point(652, 13);
            this.uploadBrowser.Name = "uploadBrowser";
            this.uploadBrowser.Size = new System.Drawing.Size(146, 40);
            this.uploadBrowser.TabIndex = 8;
            this.uploadBrowser.Text = "  ";
            this.uploadBrowser.Visible = false;
            this.uploadBrowser.Uploaded += new Wisej.Web.UploadedEventHandler(this.uploadBrowser_Uploaded);
            // 
            // panel7
            // 
            this.panel7.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.panel7.Controls.Add(this.txtUploadAs);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.lblImgReq);
            this.panel7.Controls.Add(this.lblFileName);
            this.panel7.Controls.Add(this.cmbImageType);
            this.panel7.Controls.Add(this.txtUploadFileName);
            this.panel7.Controls.Add(this.lblImageTypes);
            this.panel7.Location = new System.Drawing.Point(9, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(634, 102);
            this.panel7.TabIndex = 6;
            this.panel7.TabStop = true;
            // 
            // txtUploadAs
            // 
            this.txtUploadAs.Location = new System.Drawing.Point(88, 68);
            this.txtUploadAs.Name = "txtUploadAs";
            this.txtUploadAs.Size = new System.Drawing.Size(541, 25);
            this.txtUploadAs.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(5, 71);
            this.label2.MinimumSize = new System.Drawing.Size(0, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Upload As";
            // 
            // lblImgReq
            // 
            this.lblImgReq.AutoSize = true;
            this.lblImgReq.Controls.Add(this.label1);
            this.lblImgReq.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblImgReq.ForeColor = System.Drawing.Color.FromArgb(248, 255, 0, 0);
            this.lblImgReq.Location = new System.Drawing.Point(71, 12);
            this.lblImgReq.Name = "lblImgReq";
            this.lblImgReq.Size = new System.Drawing.Size(9, 14);
            this.lblImgReq.TabIndex = 0;
            this.lblImgReq.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(-11, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "*";
            this.label1.Visible = false;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFileName.Location = new System.Drawing.Point(5, 42);
            this.lblFileName.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(59, 18);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "File Name";
            // 
            // cmbImageType
            // 
            this.cmbImageType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbImageType.FormattingEnabled = true;
            this.cmbImageType.Location = new System.Drawing.Point(88, 9);
            this.cmbImageType.Name = "cmbImageType";
            this.cmbImageType.Size = new System.Drawing.Size(541, 25);
            this.cmbImageType.TabIndex = 3;
            // 
            // txtUploadFileName
            // 
            this.txtUploadFileName.Location = new System.Drawing.Point(88, 38);
            this.txtUploadFileName.Name = "txtUploadFileName";
            this.txtUploadFileName.ReadOnly = true;
            this.txtUploadFileName.Size = new System.Drawing.Size(541, 25);
            this.txtUploadFileName.TabIndex = 2;
            // 
            // lblImageTypes
            // 
            this.lblImageTypes.AutoSize = true;
            this.lblImageTypes.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblImageTypes.Location = new System.Drawing.Point(4, 13);
            this.lblImageTypes.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblImageTypes.Name = "lblImageTypes";
            this.lblImageTypes.Size = new System.Drawing.Size(68, 18);
            this.lblImageTypes.TabIndex = 1;
            this.lblImageTypes.Text = "Image Type";
            // 
            // ImageUpload
            // 
            this.ClientSize = new System.Drawing.Size(812, 664);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageUpload";
            this.Text = "ImageUpload";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "imguploadhlp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.ImageUpload_Load);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.ImageUpload_ToolClick);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwDoclist)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCaseSnp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHierchys)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.lblImgReq.ResumeLayout(false);
            this.lblImgReq.PerformLayout();
            this.ResumeLayout(false);

        }

		#endregion
		private Panel panel3;
        private OpenFileDialog openFileDialog1;
        private DataGridViewEx gvwDoclist;
        private DataGridViewTextBoxColumn gvtdocimageType;
        private DataGridViewTextBoxColumn gvtAssigndoc;
        private DataGridViewTextBoxColumn gvtSecurity;
        private DataGridViewTextBoxColumn gvtdocId;
        private DataGridViewTextBoxColumn gvtdocdate1;
        private ContextMenu contextmenuTypes;
        private DataGridViewTextBoxColumn gvtimgType;
        private Panel panel6;
        private Button btnDelete;
        private Button btnPreview;
        private Panel panel4;
        private Panel panel5;
        private RadioButton rdoUPloadMode;
        private RadioButton rdoViewMode;
        private CheckBox chkHouseHold;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnBrowse;
        private DataGridViewTextBoxColumn M_Code;
        private DataGridViewTextBoxColumn SSNNO;
        private DataGridViewTextBoxColumn Relation;
        private DataGridViewTextBoxColumn MemberName;
        private DataGridViewTextBoxColumn SNP_FAMILY_SEQ;
        private DataGridView dataGridCaseSnp;
        private DataGridViewTextBoxColumn Mem_Seq;
        private DataGridViewTextBoxColumn Other_Hie_Access;
        private DataGridViewTextBoxColumn ImagesCount;
        private DataGridViewTextBoxColumn Program;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn Year;
        private DataGridViewTextBoxColumn Hierarchy;
        private DataGridView dataGridHierchys;
        private Panel panel1;
        private Panel panel2;
        private Upload uploadBrowser;
        private Panel panel7;
        private TextBoxWithValidation txtUploadAs;
        private Label label2;
        private Label lblImgReq;
        private Label label1;
        private Label lblFileName;
        private ComboBoxEx cmbImageType;
        private TextBoxWithValidation txtUploadFileName;
        private Label lblImageTypes;
        private Button btnUpload;
        private DataGridViewDateTimeColumn gvtdocDate;
    }
}