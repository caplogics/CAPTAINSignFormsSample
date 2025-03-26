namespace Captain.Common.Views.Forms
{
    partial class MATB0004_Form
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

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MATB0004_Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlDateRange = new Wisej.Web.Panel();
            this.dtpTo = new Wisej.Web.DateTimePicker();
            this.lblTo = new Wisej.Web.Label();
            this.spacer5 = new Wisej.Web.Spacer();
            this.dtpFrom = new Wisej.Web.DateTimePicker();
            this.lblDateRangeFrm = new Wisej.Web.Label();
            this.pnlScale = new Wisej.Web.Panel();
            this.cmbScale = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblScale = new Wisej.Web.Label();
            this.pnlMatrix = new Wisej.Web.Panel();
            this.cmbMatrix = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.lblMatrix = new Wisej.Web.Label();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlHie = new Wisej.Web.Panel();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.btnSaveParams = new Wisej.Web.Button();
            this.spacer4 = new Wisej.Web.Spacer();
            this.btnGenerate = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.btnPreview = new Wisej.Web.Button();
            this.btnGetParams = new Wisej.Web.Button();
            this.pnlCounty = new Wisej.Web.Panel();
            this.rdbCounty_Sel = new Wisej.Web.RadioButton();
            this.rdbCounty_All = new Wisej.Web.RadioButton();
            this.lblCounty = new Wisej.Web.Label();
            this.spacer6 = new Wisej.Web.Spacer();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnlDateRange.SuspendLayout();
            this.pnlScale.SuspendLayout();
            this.pnlMatrix.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.pnlCounty.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(693, 210);
            this.pnlCompleteForm.TabIndex = 1;
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlCounty);
            this.pnlParams.Controls.Add(this.pnlDateRange);
            this.pnlParams.Controls.Add(this.pnlScale);
            this.pnlParams.Controls.Add(this.pnlMatrix);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(693, 132);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlDateRange
            // 
            this.pnlDateRange.Controls.Add(this.dtpTo);
            this.pnlDateRange.Controls.Add(this.lblTo);
            this.pnlDateRange.Controls.Add(this.spacer5);
            this.pnlDateRange.Controls.Add(this.dtpFrom);
            this.pnlDateRange.Controls.Add(this.lblDateRangeFrm);
            this.pnlDateRange.Dock = Wisej.Web.DockStyle.Top;
            this.pnlDateRange.Location = new System.Drawing.Point(0, 69);
            this.pnlDateRange.Name = "pnlDateRange";
            this.pnlDateRange.Padding = new Wisej.Web.Padding(15, 0, 5, 5);
            this.pnlDateRange.Size = new System.Drawing.Size(693, 30);
            this.pnlDateRange.TabIndex = 3;
            // 
            // dtpTo
            // 
            this.dtpTo.AllowDrop = true;
            this.dtpTo.AutoSize = false;
            this.dtpTo.CustomFormat = "MM/dd/yyyy";
            this.dtpTo.Dock = Wisej.Web.DockStyle.Left;
            this.dtpTo.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(287, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.ShowToolTips = false;
            this.dtpTo.Size = new System.Drawing.Size(116, 25);
            this.dtpTo.TabIndex = 2;
            // 
            // lblTo
            // 
            this.lblTo.Dock = Wisej.Web.DockStyle.Left;
            this.lblTo.Location = new System.Drawing.Point(264, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 25);
            this.lblTo.TabIndex = 0;
            this.lblTo.Text = "To";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Left;
            this.spacer5.Location = new System.Drawing.Point(239, 0);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(25, 25);
            // 
            // dtpFrom
            // 
            this.dtpFrom.AutoSize = false;
            this.dtpFrom.CustomFormat = "MM/dd/yyyy";
            this.dtpFrom.Dock = Wisej.Web.DockStyle.Left;
            this.dtpFrom.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(123, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.ShowToolTips = false;
            this.dtpFrom.Size = new System.Drawing.Size(116, 25);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblDateRangeFrm
            // 
            this.lblDateRangeFrm.Dock = Wisej.Web.DockStyle.Left;
            this.lblDateRangeFrm.Location = new System.Drawing.Point(15, 0);
            this.lblDateRangeFrm.MaximumSize = new System.Drawing.Size(108, 0);
            this.lblDateRangeFrm.Name = "lblDateRangeFrm";
            this.lblDateRangeFrm.Size = new System.Drawing.Size(108, 25);
            this.lblDateRangeFrm.TabIndex = 0;
            this.lblDateRangeFrm.Text = "Date Range From";
            this.lblDateRangeFrm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlScale
            // 
            this.pnlScale.Controls.Add(this.cmbScale);
            this.pnlScale.Controls.Add(this.lblScale);
            this.pnlScale.Dock = Wisej.Web.DockStyle.Top;
            this.pnlScale.Location = new System.Drawing.Point(0, 39);
            this.pnlScale.Name = "pnlScale";
            this.pnlScale.Padding = new Wisej.Web.Padding(15, 0, 5, 5);
            this.pnlScale.Size = new System.Drawing.Size(693, 30);
            this.pnlScale.TabIndex = 2;
            // 
            // cmbScale
            // 
            this.cmbScale.Dock = Wisej.Web.DockStyle.Left;
            this.cmbScale.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.Location = new System.Drawing.Point(123, 0);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(280, 25);
            this.cmbScale.TabIndex = 1;
            // 
            // lblScale
            // 
            this.lblScale.Dock = Wisej.Web.DockStyle.Left;
            this.lblScale.Location = new System.Drawing.Point(15, 0);
            this.lblScale.MaximumSize = new System.Drawing.Size(108, 0);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(108, 25);
            this.lblScale.TabIndex = 0;
            this.lblScale.Text = "Scale";
            this.lblScale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlMatrix
            // 
            this.pnlMatrix.Controls.Add(this.cmbMatrix);
            this.pnlMatrix.Controls.Add(this.lblMatrix);
            this.pnlMatrix.Dock = Wisej.Web.DockStyle.Top;
            this.pnlMatrix.Location = new System.Drawing.Point(0, 0);
            this.pnlMatrix.Name = "pnlMatrix";
            this.pnlMatrix.Padding = new Wisej.Web.Padding(15, 9, 5, 5);
            this.pnlMatrix.Size = new System.Drawing.Size(693, 39);
            this.pnlMatrix.TabIndex = 1;
            // 
            // cmbMatrix
            // 
            this.cmbMatrix.Dock = Wisej.Web.DockStyle.Left;
            this.cmbMatrix.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbMatrix.FormattingEnabled = true;
            this.cmbMatrix.Location = new System.Drawing.Point(123, 9);
            this.cmbMatrix.Name = "cmbMatrix";
            this.cmbMatrix.Size = new System.Drawing.Size(280, 25);
            this.cmbMatrix.TabIndex = 1;
            this.cmbMatrix.SelectedIndexChanged += new System.EventHandler(this.cmbMatrix_SelectedIndexChanged);
            // 
            // lblMatrix
            // 
            this.lblMatrix.Dock = Wisej.Web.DockStyle.Left;
            this.lblMatrix.Location = new System.Drawing.Point(15, 9);
            this.lblMatrix.MaximumSize = new System.Drawing.Size(108, 0);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(108, 25);
            this.lblMatrix.TabIndex = 0;
            this.lblMatrix.Text = "Matrix";
            this.lblMatrix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHieFilter
            // 
            this.pnlHieFilter.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHieFilter.Controls.Add(this.pnlFilter);
            this.pnlHieFilter.Controls.Add(this.pnlHie);
            this.pnlHieFilter.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHieFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlHieFilter.Name = "pnlHieFilter";
            this.pnlHieFilter.Padding = new Wisej.Web.Padding(15, 9, 9, 9);
            this.pnlHieFilter.Size = new System.Drawing.Size(693, 43);
            this.pnlHieFilter.TabIndex = 110;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(640, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(44, 25);
            this.pnlFilter.TabIndex = 66;
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.Pb_Search_Hie.CssStyle = "border-radius:25px ";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(5, 0);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(5, 25);
            // 
            // pnlHie
            // 
            this.pnlHie.BackColor = System.Drawing.Color.FromArgb(11, 70, 117);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.spacer1);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlHie.Dock = Wisej.Web.DockStyle.Left;
            this.pnlHie.Location = new System.Drawing.Point(15, 9);
            this.pnlHie.Name = "pnlHie";
            this.pnlHie.Size = new System.Drawing.Size(625, 25);
            this.pnlHie.TabIndex = 99;
            // 
            // CmbYear
            // 
            this.CmbYear.Dock = Wisej.Web.DockStyle.Left;
            this.CmbYear.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbYear.FormattingEnabled = true;
            this.CmbYear.Location = new System.Drawing.Point(555, 0);
            this.CmbYear.Name = "CmbYear";
            this.CmbYear.Size = new System.Drawing.Size(65, 25);
            this.CmbYear.TabIndex = 77;
            this.CmbYear.TabStop = false;
            this.CmbYear.Visible = false;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(550, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // Txt_HieDesc
            // 
            this.Txt_HieDesc.BackColor = System.Drawing.Color.Transparent;
            this.Txt_HieDesc.BorderStyle = Wisej.Web.BorderStyle.None;
            this.Txt_HieDesc.Dock = Wisej.Web.DockStyle.Left;
            this.Txt_HieDesc.Font = new System.Drawing.Font("defaultBold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Txt_HieDesc.ForeColor = System.Drawing.Color.White;
            this.Txt_HieDesc.Location = new System.Drawing.Point(0, 0);
            this.Txt_HieDesc.Name = "Txt_HieDesc";
            this.Txt_HieDesc.ReadOnly = true;
            this.Txt_HieDesc.Size = new System.Drawing.Size(550, 25);
            this.Txt_HieDesc.TabIndex = 88;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.btnSaveParams);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.btnGenerate);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnPreview);
            this.pnlGenerate.Controls.Add(this.btnGetParams);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 175);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(693, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // btnSaveParams
            // 
            this.btnSaveParams.AppearanceKey = "button-reports";
            this.btnSaveParams.Dock = Wisej.Web.DockStyle.Left;
            this.btnSaveParams.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSaveParams.Location = new System.Drawing.Point(128, 5);
            this.btnSaveParams.Name = "btnSaveParams";
            this.btnSaveParams.Size = new System.Drawing.Size(115, 25);
            this.btnSaveParams.TabIndex = 2;
            this.btnSaveParams.Text = "&Save Parameters";
            this.btnSaveParams.Click += new System.EventHandler(this.btnSaveParameters_Click);
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(125, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // btnGenerate
            // 
            this.btnGenerate.AppearanceKey = "button-reports";
            this.btnGenerate.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenerate.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGenerate.Location = new System.Drawing.Point(505, 5);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(85, 25);
            this.btnGenerate.TabIndex = 3;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(590, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // btnPreview
            // 
            this.btnPreview.AppearanceKey = "button-reports";
            this.btnPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPreview.Location = new System.Drawing.Point(593, 5);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPreview.TabIndex = 4;
            this.btnPreview.Text = "Pre&view";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnGetParams
            // 
            this.btnGetParams.AppearanceKey = "button-reports";
            this.btnGetParams.Dock = Wisej.Web.DockStyle.Left;
            this.btnGetParams.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGetParams.Location = new System.Drawing.Point(15, 5);
            this.btnGetParams.Name = "btnGetParams";
            this.btnGetParams.Size = new System.Drawing.Size(110, 25);
            this.btnGetParams.TabIndex = 1;
            this.btnGetParams.Text = "Get &Parameters";
            this.btnGetParams.Click += new System.EventHandler(this.btnGetParameters_Click);
            // 
            // pnlCounty
            // 
            this.pnlCounty.Controls.Add(this.rdbCounty_Sel);
            this.pnlCounty.Controls.Add(this.spacer6);
            this.pnlCounty.Controls.Add(this.rdbCounty_All);
            this.pnlCounty.Controls.Add(this.lblCounty);
            this.pnlCounty.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCounty.Location = new System.Drawing.Point(0, 99);
            this.pnlCounty.Name = "pnlCounty";
            this.pnlCounty.Padding = new Wisej.Web.Padding(15, 0, 5, 9);
            this.pnlCounty.Size = new System.Drawing.Size(693, 33);
            this.pnlCounty.TabIndex = 4;
            // 
            // rdbCounty_Sel
            // 
            this.rdbCounty_Sel.AutoSize = false;
            this.rdbCounty_Sel.Dock = Wisej.Web.DockStyle.Left;
            this.rdbCounty_Sel.Location = new System.Drawing.Point(193, 0);
            this.rdbCounty_Sel.Name = "rdbCounty_Sel";
            this.rdbCounty_Sel.Size = new System.Drawing.Size(78, 24);
            this.rdbCounty_Sel.TabIndex = 2;
            this.rdbCounty_Sel.Text = "Selected";
            this.rdbCounty_Sel.Click += new System.EventHandler(this.rdbCounty_Sel_Click);
            // 
            // rdbCounty_All
            // 
            this.rdbCounty_All.AutoSize = false;
            this.rdbCounty_All.Checked = true;
            this.rdbCounty_All.Dock = Wisej.Web.DockStyle.Left;
            this.rdbCounty_All.Location = new System.Drawing.Point(123, 0);
            this.rdbCounty_All.Name = "rdbCounty_All";
            this.rdbCounty_All.Size = new System.Drawing.Size(45, 24);
            this.rdbCounty_All.TabIndex = 1;
            this.rdbCounty_All.TabStop = true;
            this.rdbCounty_All.Text = "All";
            this.rdbCounty_All.Click += new System.EventHandler(this.rdbCounty_All_Click);
            // 
            // lblCounty
            // 
            this.lblCounty.Dock = Wisej.Web.DockStyle.Left;
            this.lblCounty.Location = new System.Drawing.Point(15, 0);
            this.lblCounty.MinimumSize = new System.Drawing.Size(108, 0);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(108, 24);
            this.lblCounty.TabIndex = 2;
            this.lblCounty.Text = "County";
            this.lblCounty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spacer6
            // 
            this.spacer6.Dock = Wisej.Web.DockStyle.Left;
            this.spacer6.Location = new System.Drawing.Point(168, 0);
            this.spacer6.Name = "spacer6";
            this.spacer6.Size = new System.Drawing.Size(25, 24);
            // 
            // MATB0004_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 210);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MATB0004_Form";
            this.Text = "Client Assessment Report";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlParams.ResumeLayout(false);
            this.pnlDateRange.ResumeLayout(false);
            this.pnlScale.ResumeLayout(false);
            this.pnlScale.PerformLayout();
            this.pnlMatrix.ResumeLayout(false);
            this.pnlMatrix.PerformLayout();
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            this.pnlCounty.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlCompleteForm;
        private Wisej.Web.Panel pnlParams;
        private Wisej.Web.Panel pnlScale;
        private Wisej.Web.Label lblScale;
        private Controls.Compatibility.ComboBoxEx cmbScale;
        private Wisej.Web.Panel pnlMatrix;
        private Controls.Compatibility.ComboBoxEx cmbMatrix;
        private Wisej.Web.Label lblMatrix;
        private Wisej.Web.Panel pnlDateRange;
        private Wisej.Web.Label lblDateRangeFrm;
        private Wisej.Web.Label lblTo;
        private Wisej.Web.DateTimePicker dtpTo;
        private Wisej.Web.DateTimePicker dtpFrom;
        private Wisej.Web.Panel pnlHieFilter;
        private Wisej.Web.Panel pnlFilter;
        private Wisej.Web.PictureBox Pb_Search_Hie;
        private Wisej.Web.Spacer spacer2;
        private Wisej.Web.Panel pnlHie;
        private Wisej.Web.ComboBox CmbYear;
        private Wisej.Web.Spacer spacer1;
        private Wisej.Web.TextBox Txt_HieDesc;
        private Wisej.Web.Panel pnlGenerate;
        private Wisej.Web.Button btnSaveParams;
        private Wisej.Web.Spacer spacer4;
        private Wisej.Web.Button btnGenerate;
        private Wisej.Web.Spacer spacer3;
        private Wisej.Web.Button btnPreview;
        private Wisej.Web.Button btnGetParams;
        private Wisej.Web.Spacer spacer5;
        private Wisej.Web.Panel pnlCounty;
        private Wisej.Web.RadioButton rdbCounty_Sel;
        private Wisej.Web.Spacer spacer6;
        private Wisej.Web.RadioButton rdbCounty_All;
        private Wisej.Web.Label lblCounty;
    }
}