//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class MatOutcomesReasons
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle17 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle18 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle19 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle20 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle21 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle22 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle23 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatOutcomesReasons));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlOutcomes = new Wisej.Web.Panel();
            this.pnlOutDesc = new Wisej.Web.Panel();
            this.Txt_Out_Desc = new Wisej.Web.TextBox();
            this.lblReqODesc = new Wisej.Web.Label();
            this.label25 = new Wisej.Web.Label();
            this.Txt_Out_Points = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.picAddScale = new Wisej.Web.PictureBox();
            this.lblOutcmePoints = new Wisej.Web.Label();
            this.lblOutCmCode = new Wisej.Web.Label();
            this.Txt_Out_Code = new Wisej.Web.TextBox();
            this.lblOutcmeDesc = new Wisej.Web.Label();
            this.pnlOutGrid = new Wisej.Web.Panel();
            this.Outcomes_Grid = new Wisej.Web.DataGridView();
            this.Out_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Out_Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Out_Points = new Wisej.Web.DataGridViewTextBoxColumn();
            this.EditOutComes = new Wisej.Web.DataGridViewImageColumn();
            this.DelOutCome = new Wisej.Web.DataGridViewImageColumn();
            this.pnlScaleBench = new Wisej.Web.Panel();
            this.Cmb_Benchmarks = new Wisej.Web.ComboBox();
            this.label5 = new Wisej.Web.Label();
            this.lblScale = new Wisej.Web.Label();
            this.cmbScales = new Wisej.Web.ComboBox();
            this.pnlOSave = new Wisej.Web.Panel();
            this.Pb_Save_Out = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Pb_Cancel_Out = new Wisej.Web.Button();
            this.pnlReasons = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.cmbReason = new Wisej.Web.ComboBox();
            this.txtReasonDesc = new Wisej.Web.TextBox();
            this.lblReqR = new Wisej.Web.Label();
            this.lblReasoncd = new Wisej.Web.Label();
            this.picAddreason = new Wisej.Web.PictureBox();
            this.label9 = new Wisej.Web.Label();
            this.lblResnDesc = new Wisej.Web.Label();
            this.txtReason = new Wisej.Web.TextBox();
            this.pnlRSave = new Wisej.Web.Panel();
            this.Pb_Save_Reason = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.Pb_Cancel_Reason = new Wisej.Web.Button();
            this.pnlGridReasons = new Wisej.Web.Panel();
            this.Reasons_Grid = new Wisej.Web.DataGridView();
            this.Reason_Code = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Reason_PN = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Reason_Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.EditReason = new Wisej.Web.DataGridViewImageColumn();
            this.DelReason = new Wisej.Web.DataGridViewImageColumn();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlOutcomes.SuspendLayout();
            this.pnlOutDesc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddScale)).BeginInit();
            this.pnlOutGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Outcomes_Grid)).BeginInit();
            this.pnlScaleBench.SuspendLayout();
            this.pnlOSave.SuspendLayout();
            this.pnlReasons.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddreason)).BeginInit();
            this.pnlRSave.SuspendLayout();
            this.pnlGridReasons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Reasons_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlOutcomes
            // 
            this.pnlOutcomes.Controls.Add(this.pnlOutDesc);
            this.pnlOutcomes.Controls.Add(this.pnlOutGrid);
            this.pnlOutcomes.Controls.Add(this.pnlScaleBench);
            this.pnlOutcomes.Controls.Add(this.pnlOSave);
            this.pnlOutcomes.Dock = Wisej.Web.DockStyle.Top;
            this.pnlOutcomes.Location = new System.Drawing.Point(0, 0);
            this.pnlOutcomes.Name = "pnlOutcomes";
            this.pnlOutcomes.Size = new System.Drawing.Size(586, 347);
            this.pnlOutcomes.TabIndex = 0;
            // 
            // pnlOutDesc
            // 
            this.pnlOutDesc.Controls.Add(this.Txt_Out_Desc);
            this.pnlOutDesc.Controls.Add(this.lblReqODesc);
            this.pnlOutDesc.Controls.Add(this.label25);
            this.pnlOutDesc.Controls.Add(this.Txt_Out_Points);
            this.pnlOutDesc.Controls.Add(this.picAddScale);
            this.pnlOutDesc.Controls.Add(this.lblOutcmePoints);
            this.pnlOutDesc.Controls.Add(this.lblOutCmCode);
            this.pnlOutDesc.Controls.Add(this.Txt_Out_Code);
            this.pnlOutDesc.Controls.Add(this.lblOutcmeDesc);
            this.pnlOutDesc.Dock = Wisej.Web.DockStyle.Top;
            this.pnlOutDesc.Location = new System.Drawing.Point(0, 204);
            this.pnlOutDesc.Name = "pnlOutDesc";
            this.pnlOutDesc.Size = new System.Drawing.Size(586, 108);
            this.pnlOutDesc.TabIndex = 0;
            // 
            // Txt_Out_Desc
            // 
            this.Txt_Out_Desc.Enabled = false;
            this.Txt_Out_Desc.Location = new System.Drawing.Point(112, 11);
            this.Txt_Out_Desc.MaxLength = 500;
            this.Txt_Out_Desc.Multiline = true;
            this.Txt_Out_Desc.Name = "Txt_Out_Desc";
            this.Txt_Out_Desc.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Txt_Out_Desc.Size = new System.Drawing.Size(404, 56);
            this.Txt_Out_Desc.TabIndex = 4;
            // 
            // lblReqODesc
            // 
            this.lblReqODesc.ForeColor = System.Drawing.Color.Red;
            this.lblReqODesc.Location = new System.Drawing.Point(98, 11);
            this.lblReqODesc.Name = "lblReqODesc";
            this.lblReqODesc.Size = new System.Drawing.Size(7, 10);
            this.lblReqODesc.TabIndex = 8;
            this.lblReqODesc.Text = "*";
            // 
            // label25
            // 
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(55, 74);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(7, 10);
            this.label25.TabIndex = 8;
            this.label25.Text = "*";
            // 
            // Txt_Out_Points
            // 
            this.Txt_Out_Points.Enabled = false;
            this.Txt_Out_Points.Location = new System.Drawing.Point(113, 74);
            this.Txt_Out_Points.MaxLength = 3;
            this.Txt_Out_Points.Name = "Txt_Out_Points";
            this.Txt_Out_Points.Size = new System.Drawing.Size(42, 25);
            this.Txt_Out_Points.TabIndex = 5;
            this.Txt_Out_Points.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // picAddScale
            // 
            this.picAddScale.BackgroundImageLayout = Wisej.Web.ImageLayout.BestFit;
            this.picAddScale.Cursor = Wisej.Web.Cursors.Hand;
            this.picAddScale.ImageSource = "captain-add";
            this.picAddScale.Location = new System.Drawing.Point(541, 11);
            this.picAddScale.Name = "picAddScale";
            this.picAddScale.Size = new System.Drawing.Size(24, 24);
            this.picAddScale.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAddScale.Click += new System.EventHandler(this.picAddScale_Click);
            // 
            // lblOutcmePoints
            // 
            this.lblOutcmePoints.Location = new System.Drawing.Point(19, 78);
            this.lblOutcmePoints.Name = "lblOutcmePoints";
            this.lblOutcmePoints.Size = new System.Drawing.Size(37, 14);
            this.lblOutcmePoints.TabIndex = 1;
            this.lblOutcmePoints.Text = "Points";
            // 
            // lblOutCmCode
            // 
            this.lblOutCmCode.Location = new System.Drawing.Point(181, 78);
            this.lblOutCmCode.Name = "lblOutCmCode";
            this.lblOutCmCode.Size = new System.Drawing.Size(85, 14);
            this.lblOutCmCode.TabIndex = 1;
            this.lblOutCmCode.Text = "Outcome Code";
            this.lblOutCmCode.Visible = false;
            // 
            // Txt_Out_Code
            // 
            this.Txt_Out_Code.Enabled = false;
            this.Txt_Out_Code.Location = new System.Drawing.Point(273, 74);
            this.Txt_Out_Code.Name = "Txt_Out_Code";
            this.Txt_Out_Code.Size = new System.Drawing.Size(57, 25);
            this.Txt_Out_Code.TabIndex = 6;
            this.Txt_Out_Code.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.Txt_Out_Code.Visible = false;
            // 
            // lblOutcmeDesc
            // 
            this.lblOutcmeDesc.Location = new System.Drawing.Point(15, 16);
            this.lblOutcmeDesc.Name = "lblOutcmeDesc";
            this.lblOutcmeDesc.Size = new System.Drawing.Size(86, 14);
            this.lblOutcmeDesc.TabIndex = 1;
            this.lblOutcmeDesc.Text = "Outcome Desc";
            // 
            // pnlOutGrid
            // 
            this.pnlOutGrid.Controls.Add(this.Outcomes_Grid);
            this.pnlOutGrid.Dock = Wisej.Web.DockStyle.Top;
            this.pnlOutGrid.Location = new System.Drawing.Point(0, 71);
            this.pnlOutGrid.Name = "pnlOutGrid";
            this.pnlOutGrid.Padding = new Wisej.Web.Padding(0, 8, 0, 8);
            this.pnlOutGrid.Size = new System.Drawing.Size(586, 133);
            this.pnlOutGrid.TabIndex = 0;
            // 
            // Outcomes_Grid
            // 
            this.Outcomes_Grid.AllowUserToResizeColumns = false;
            this.Outcomes_Grid.AllowUserToResizeRows = false;
            this.Outcomes_Grid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.Outcomes_Grid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.Outcomes_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Outcomes_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Outcomes_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Outcomes_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Out_Code,
            this.Out_Desc,
            this.Out_Points,
            this.EditOutComes,
            this.DelOutCome});
            this.Outcomes_Grid.Cursor = Wisej.Web.Cursors.Default;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Outcomes_Grid.DefaultCellStyle = dataGridViewCellStyle12;
            this.Outcomes_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.Outcomes_Grid.Location = new System.Drawing.Point(0, 8);
            this.Outcomes_Grid.Name = "Outcomes_Grid";
            this.Outcomes_Grid.ReadOnly = true;
            this.Outcomes_Grid.RowHeadersWidth = 14;
            this.Outcomes_Grid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Outcomes_Grid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Outcomes_Grid.Size = new System.Drawing.Size(586, 117);
            this.Outcomes_Grid.TabIndex = 3;
            this.Outcomes_Grid.SelectionChanged += new System.EventHandler(this.Outcomes_Grid_SelectionChanged);
            this.Outcomes_Grid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.Outcomes_Grid_CellClick);
            // 
            // Out_Code
            // 
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Out_Code.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Out_Code.HeaderStyle = dataGridViewCellStyle3;
            this.Out_Code.HeaderText = "Code";
            this.Out_Code.Name = "Out_Code";
            this.Out_Code.ReadOnly = true;
            this.Out_Code.ShowInVisibilityMenu = false;
            this.Out_Code.Visible = false;
            this.Out_Code.Width = 50;
            // 
            // Out_Desc
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Out_Desc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Out_Desc.HeaderStyle = dataGridViewCellStyle5;
            this.Out_Desc.HeaderText = "Description";
            this.Out_Desc.Name = "Out_Desc";
            this.Out_Desc.ReadOnly = true;
            this.Out_Desc.Width = 355;
            // 
            // Out_Points
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Out_Points.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Out_Points.HeaderStyle = dataGridViewCellStyle7;
            this.Out_Points.HeaderText = "Points";
            this.Out_Points.Name = "Out_Points";
            this.Out_Points.ReadOnly = true;
            this.Out_Points.Width = 65;
            // 
            // EditOutComes
            // 
            this.EditOutComes.CellImageSource = "captain-edit";
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EditOutComes.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EditOutComes.HeaderStyle = dataGridViewCellStyle9;
            this.EditOutComes.HeaderText = "Edit";
            this.EditOutComes.Name = "EditOutComes";
            this.EditOutComes.ReadOnly = true;
            this.EditOutComes.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.EditOutComes.ShowInVisibilityMenu = false;
            this.EditOutComes.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.EditOutComes.Width = 50;
            // 
            // DelOutCome
            // 
            this.DelOutCome.CellImageSource = "captain-delete";
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DelOutCome.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DelOutCome.HeaderStyle = dataGridViewCellStyle11;
            this.DelOutCome.HeaderText = "Del";
            this.DelOutCome.Name = "DelOutCome";
            this.DelOutCome.ReadOnly = true;
            this.DelOutCome.Resizable = Wisej.Web.DataGridViewTriState.False;
            this.DelOutCome.ShowInVisibilityMenu = false;
            this.DelOutCome.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.DelOutCome.Width = 50;
            // 
            // pnlScaleBench
            // 
            this.pnlScaleBench.Controls.Add(this.Cmb_Benchmarks);
            this.pnlScaleBench.Controls.Add(this.label5);
            this.pnlScaleBench.Controls.Add(this.lblScale);
            this.pnlScaleBench.Controls.Add(this.cmbScales);
            this.pnlScaleBench.Dock = Wisej.Web.DockStyle.Top;
            this.pnlScaleBench.Location = new System.Drawing.Point(0, 0);
            this.pnlScaleBench.Name = "pnlScaleBench";
            this.pnlScaleBench.Size = new System.Drawing.Size(586, 71);
            this.pnlScaleBench.TabIndex = 0;
            // 
            // Cmb_Benchmarks
            // 
            this.Cmb_Benchmarks.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb_Benchmarks.FormattingEnabled = true;
            this.Cmb_Benchmarks.Location = new System.Drawing.Point(113, 43);
            this.Cmb_Benchmarks.Name = "Cmb_Benchmarks";
            this.Cmb_Benchmarks.Size = new System.Drawing.Size(361, 25);
            this.Cmb_Benchmarks.TabIndex = 2;
            this.Cmb_Benchmarks.SelectedIndexChanged += new System.EventHandler(this.Cmb_Benchmarks_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(15, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 14);
            this.label5.TabIndex = 0;
            this.label5.Text = "Benchmark";
            // 
            // lblScale
            // 
            this.lblScale.Location = new System.Drawing.Point(15, 15);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(30, 13);
            this.lblScale.TabIndex = 0;
            this.lblScale.Text = "Scale";
            // 
            // cmbScales
            // 
            this.cmbScales.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbScales.FormattingEnabled = true;
            this.cmbScales.Location = new System.Drawing.Point(113, 11);
            this.cmbScales.Name = "cmbScales";
            this.cmbScales.Size = new System.Drawing.Size(361, 25);
            this.cmbScales.TabIndex = 1;
            this.cmbScales.SelectedIndexChanged += new System.EventHandler(this.cmbScope_SelectedIndexChanged);
            // 
            // pnlOSave
            // 
            this.pnlOSave.AppearanceKey = "panel-grdo";
            this.pnlOSave.Controls.Add(this.Pb_Save_Out);
            this.pnlOSave.Controls.Add(this.spacer1);
            this.pnlOSave.Controls.Add(this.Pb_Cancel_Out);
            this.pnlOSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlOSave.Location = new System.Drawing.Point(0, 312);
            this.pnlOSave.Name = "pnlOSave";
            this.pnlOSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlOSave.Size = new System.Drawing.Size(586, 35);
            this.pnlOSave.TabIndex = 0;
            // 
            // Pb_Save_Out
            // 
            this.Pb_Save_Out.AppearanceKey = "button-ok";
            this.Pb_Save_Out.Dock = Wisej.Web.DockStyle.Right;
            this.Pb_Save_Out.Location = new System.Drawing.Point(418, 5);
            this.Pb_Save_Out.Name = "Pb_Save_Out";
            this.Pb_Save_Out.Size = new System.Drawing.Size(75, 25);
            this.Pb_Save_Out.TabIndex = 6;
            this.Pb_Save_Out.Text = "&Save";
            this.Pb_Save_Out.Visible = false;
            this.Pb_Save_Out.Click += new System.EventHandler(this.Pb_Save_Out_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(493, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // Pb_Cancel_Out
            // 
            this.Pb_Cancel_Out.AppearanceKey = "button-error";
            this.Pb_Cancel_Out.Dock = Wisej.Web.DockStyle.Right;
            this.Pb_Cancel_Out.Location = new System.Drawing.Point(496, 5);
            this.Pb_Cancel_Out.Name = "Pb_Cancel_Out";
            this.Pb_Cancel_Out.Size = new System.Drawing.Size(75, 25);
            this.Pb_Cancel_Out.TabIndex = 7;
            this.Pb_Cancel_Out.Text = "&Cancel";
            this.Pb_Cancel_Out.Visible = false;
            this.Pb_Cancel_Out.Click += new System.EventHandler(this.Pb_Cancel_Out_Click);
            // 
            // pnlReasons
            // 
            this.pnlReasons.Controls.Add(this.panel1);
            this.pnlReasons.Controls.Add(this.pnlRSave);
            this.pnlReasons.Controls.Add(this.pnlGridReasons);
            this.pnlReasons.Dock = Wisej.Web.DockStyle.Top;
            this.pnlReasons.Location = new System.Drawing.Point(0, 347);
            this.pnlReasons.Name = "pnlReasons";
            this.pnlReasons.Size = new System.Drawing.Size(586, 311);
            this.pnlReasons.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbReason);
            this.panel1.Controls.Add(this.txtReasonDesc);
            this.panel1.Controls.Add(this.lblReqR);
            this.panel1.Controls.Add(this.lblReasoncd);
            this.panel1.Controls.Add(this.picAddreason);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.lblResnDesc);
            this.panel1.Controls.Add(this.txtReason);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 172);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(586, 104);
            this.panel1.TabIndex = 0;
            // 
            // cmbReason
            // 
            this.cmbReason.AutoSize = false;
            this.cmbReason.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReason.Enabled = false;
            this.cmbReason.FormattingEnabled = true;
            this.cmbReason.Location = new System.Drawing.Point(103, 69);
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.Size = new System.Drawing.Size(88, 25);
            this.cmbReason.TabIndex = 3;
            // 
            // txtReasonDesc
            // 
            this.txtReasonDesc.Enabled = false;
            this.txtReasonDesc.Location = new System.Drawing.Point(103, 7);
            this.txtReasonDesc.MaxLength = 75;
            this.txtReasonDesc.Multiline = true;
            this.txtReasonDesc.Name = "txtReasonDesc";
            this.txtReasonDesc.Size = new System.Drawing.Size(404, 56);
            this.txtReasonDesc.TabIndex = 2;
            // 
            // lblReqR
            // 
            this.lblReqR.ForeColor = System.Drawing.Color.Red;
            this.lblReqR.Location = new System.Drawing.Point(87, 9);
            this.lblReqR.Name = "lblReqR";
            this.lblReqR.Size = new System.Drawing.Size(7, 10);
            this.lblReqR.TabIndex = 8;
            this.lblReqR.Text = "*";
            // 
            // lblReasoncd
            // 
            this.lblReasoncd.Location = new System.Drawing.Point(213, 74);
            this.lblReasoncd.Name = "lblReasoncd";
            this.lblReasoncd.Size = new System.Drawing.Size(74, 14);
            this.lblReasoncd.TabIndex = 1;
            this.lblReasoncd.Text = "Reason Code";
            this.lblReasoncd.Visible = false;
            // 
            // picAddreason
            // 
            this.picAddreason.BackgroundImageLayout = Wisej.Web.ImageLayout.BestFit;
            this.picAddreason.Cursor = Wisej.Web.Cursors.Hand;
            this.picAddreason.ImageSource = "captain-add";
            this.picAddreason.Location = new System.Drawing.Point(532, 8);
            this.picAddreason.Name = "picAddreason";
            this.picAddreason.Size = new System.Drawing.Size(24, 24);
            this.picAddreason.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picAddreason.Click += new System.EventHandler(this.picAddreason_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(15, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 14);
            this.label9.TabIndex = 1;
            this.label9.Text = "P/N";
            // 
            // lblResnDesc
            // 
            this.lblResnDesc.Location = new System.Drawing.Point(14, 13);
            this.lblResnDesc.Name = "lblResnDesc";
            this.lblResnDesc.Size = new System.Drawing.Size(73, 14);
            this.lblResnDesc.TabIndex = 1;
            this.lblResnDesc.Text = "Reason Desc";
            // 
            // txtReason
            // 
            this.txtReason.Enabled = false;
            this.txtReason.Location = new System.Drawing.Point(294, 70);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(57, 25);
            this.txtReason.TabIndex = 4;
            this.txtReason.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtReason.Visible = false;
            // 
            // pnlRSave
            // 
            this.pnlRSave.AppearanceKey = "panel-grdo";
            this.pnlRSave.Controls.Add(this.Pb_Save_Reason);
            this.pnlRSave.Controls.Add(this.spacer2);
            this.pnlRSave.Controls.Add(this.Pb_Cancel_Reason);
            this.pnlRSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlRSave.Location = new System.Drawing.Point(0, 276);
            this.pnlRSave.Name = "pnlRSave";
            this.pnlRSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlRSave.Size = new System.Drawing.Size(586, 35);
            this.pnlRSave.TabIndex = 0;
            // 
            // Pb_Save_Reason
            // 
            this.Pb_Save_Reason.AppearanceKey = "button-ok";
            this.Pb_Save_Reason.Dock = Wisej.Web.DockStyle.Right;
            this.Pb_Save_Reason.Location = new System.Drawing.Point(418, 5);
            this.Pb_Save_Reason.Name = "Pb_Save_Reason";
            this.Pb_Save_Reason.Size = new System.Drawing.Size(75, 25);
            this.Pb_Save_Reason.TabIndex = 5;
            this.Pb_Save_Reason.Text = "&Save";
            this.Pb_Save_Reason.Visible = false;
            this.Pb_Save_Reason.Click += new System.EventHandler(this.Pb_Save_Reason_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(493, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // Pb_Cancel_Reason
            // 
            this.Pb_Cancel_Reason.AppearanceKey = "button-error";
            this.Pb_Cancel_Reason.Dock = Wisej.Web.DockStyle.Right;
            this.Pb_Cancel_Reason.Location = new System.Drawing.Point(496, 5);
            this.Pb_Cancel_Reason.Name = "Pb_Cancel_Reason";
            this.Pb_Cancel_Reason.Size = new System.Drawing.Size(75, 25);
            this.Pb_Cancel_Reason.TabIndex = 6;
            this.Pb_Cancel_Reason.Text = "&Cancel";
            this.Pb_Cancel_Reason.Visible = false;
            this.Pb_Cancel_Reason.Click += new System.EventHandler(this.Pb_Cancel_Reason_Click);
            // 
            // pnlGridReasons
            // 
            this.pnlGridReasons.Controls.Add(this.Reasons_Grid);
            this.pnlGridReasons.Dock = Wisej.Web.DockStyle.Top;
            this.pnlGridReasons.Location = new System.Drawing.Point(0, 0);
            this.pnlGridReasons.Name = "pnlGridReasons";
            this.pnlGridReasons.Padding = new Wisej.Web.Padding(0, 8, 0, 8);
            this.pnlGridReasons.Size = new System.Drawing.Size(586, 172);
            this.pnlGridReasons.TabIndex = 0;
            // 
            // Reasons_Grid
            // 
            this.Reasons_Grid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.Reasons_Grid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.Reasons_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Reasons_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.Reasons_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Reasons_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Reason_Code,
            this.Reason_PN,
            this.Reason_Desc,
            this.EditReason,
            this.DelReason});
            this.Reasons_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.Reasons_Grid.Location = new System.Drawing.Point(0, 8);
            this.Reasons_Grid.Name = "Reasons_Grid";
            this.Reasons_Grid.ReadOnly = true;
            this.Reasons_Grid.RowHeadersWidth = 14;
            this.Reasons_Grid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Reasons_Grid.Size = new System.Drawing.Size(586, 156);
            this.Reasons_Grid.TabIndex = 1;
            this.Reasons_Grid.SelectionChanged += new System.EventHandler(this.Reasons_Grid_SelectionChanged);
            this.Reasons_Grid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.Reasons_Grid_CellClick);
            // 
            // Reason_Code
            // 
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Reason_Code.DefaultCellStyle = dataGridViewCellStyle14;
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Reason_Code.HeaderStyle = dataGridViewCellStyle15;
            this.Reason_Code.HeaderText = "Code";
            this.Reason_Code.Name = "Reason_Code";
            this.Reason_Code.ReadOnly = true;
            this.Reason_Code.ShowInVisibilityMenu = false;
            this.Reason_Code.Visible = false;
            this.Reason_Code.Width = 30;
            // 
            // Reason_PN
            // 
            dataGridViewCellStyle16.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Reason_PN.DefaultCellStyle = dataGridViewCellStyle16;
            dataGridViewCellStyle17.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Reason_PN.HeaderStyle = dataGridViewCellStyle17;
            this.Reason_PN.HeaderText = "P / N";
            this.Reason_PN.Name = "Reason_PN";
            this.Reason_PN.ReadOnly = true;
            this.Reason_PN.Width = 75;
            // 
            // Reason_Desc
            // 
            dataGridViewCellStyle18.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle18.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Reason_Desc.DefaultCellStyle = dataGridViewCellStyle18;
            dataGridViewCellStyle19.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Reason_Desc.HeaderStyle = dataGridViewCellStyle19;
            this.Reason_Desc.HeaderText = "Description";
            this.Reason_Desc.Name = "Reason_Desc";
            this.Reason_Desc.ReadOnly = true;
            this.Reason_Desc.Width = 350;
            // 
            // EditReason
            // 
            this.EditReason.CellImageSource = "captain-edit";
            dataGridViewCellStyle20.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EditReason.DefaultCellStyle = dataGridViewCellStyle20;
            dataGridViewCellStyle21.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EditReason.HeaderStyle = dataGridViewCellStyle21;
            this.EditReason.HeaderText = "Edit";
            this.EditReason.Name = "EditReason";
            this.EditReason.ReadOnly = true;
            this.EditReason.ShowInVisibilityMenu = false;
            this.EditReason.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.EditReason.Width = 50;
            // 
            // DelReason
            // 
            this.DelReason.CellImageSource = "captain-delete";
            dataGridViewCellStyle22.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DelReason.DefaultCellStyle = dataGridViewCellStyle22;
            dataGridViewCellStyle23.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DelReason.HeaderStyle = dataGridViewCellStyle23;
            this.DelReason.HeaderText = "Del";
            this.DelReason.Name = "DelReason";
            this.DelReason.ReadOnly = true;
            this.DelReason.ShowInVisibilityMenu = false;
            this.DelReason.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.DelReason.Width = 50;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(586, 659);
            this.pnlCompleteForm.TabIndex = 2;
            // 
            // MatOutcomesReasons
            // 
            this.ClientSize = new System.Drawing.Size(586, 659);
            this.Controls.Add(this.pnlReasons);
            this.Controls.Add(this.pnlOutcomes);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatOutcomesReasons";
            this.Text = "Matrix";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.Load += new System.EventHandler(this.MatOutcomesReasons_Load);
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.MatOutcomesReasons_ToolClick);
            this.pnlOutcomes.ResumeLayout(false);
            this.pnlOutDesc.ResumeLayout(false);
            this.pnlOutDesc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddScale)).EndInit();
            this.pnlOutGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Outcomes_Grid)).EndInit();
            this.pnlScaleBench.ResumeLayout(false);
            this.pnlScaleBench.PerformLayout();
            this.pnlOSave.ResumeLayout(false);
            this.pnlReasons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddreason)).EndInit();
            this.pnlRSave.ResumeLayout(false);
            this.pnlGridReasons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Reasons_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel pnlOutcomes;
        private Panel pnlReasons;
        private Label label5;
        private DataGridView Outcomes_Grid;
        private DataGridViewTextBoxColumn Out_Code;
        private DataGridViewTextBoxColumn Out_Desc;
        private DataGridViewTextBoxColumn Out_Points;
        private TextBox Txt_Out_Desc;
        private TextBox Txt_Out_Code;
        private Label lblOutcmeDesc;
        private Label lblOutCmCode;
        private Button Pb_Save_Out;
        private Button Pb_Cancel_Out;
        private Label lblOutcmePoints;
        private TextBoxWithValidation Txt_Out_Points;
        private Label label25;
        private Label lblReqODesc;
        private DataGridView Reasons_Grid;
        private DataGridViewTextBoxColumn Reason_Code;
        private DataGridViewTextBoxColumn Reason_PN;
        private DataGridViewTextBoxColumn Reason_Desc;
        private TextBox txtReasonDesc;
        private ComboBox cmbReason;
        private TextBox txtReason;
        private Label lblResnDesc;
        private Label label9;
        private Label lblReasoncd;
        private Button Pb_Save_Reason;
        private Button Pb_Cancel_Reason;
        private Label lblReqR;
        private PictureBox picAddScale;
        private DataGridViewImageColumn DelOutCome;
        private DataGridViewImageColumn DelReason;
        private PictureBox picAddreason;
        private DataGridViewImageColumn EditOutComes;
        private DataGridViewImageColumn EditReason;
        private ComboBox Cmb_Benchmarks;
        private Label lblScale;
        private ComboBox cmbScales;
        private Panel pnlOSave;
        private Spacer spacer1;
        private Panel pnlCompleteForm;
        private Panel pnlScaleBench;
        private Panel pnlOutDesc;
        private Panel pnlOutGrid;
        private Panel pnlRSave;
        private Spacer spacer2;
        private Panel pnlGridReasons;
        private Panel panel1;
    }
}