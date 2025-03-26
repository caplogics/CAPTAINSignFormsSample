using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class CASE0013_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CASE0013_Form));
            this.panel1 = new Wisej.Web.Panel();
            this.lblDateReq = new Wisej.Web.Label();
            this.lblRefFromToReq = new Wisej.Web.Label();
            this.txtCode = new Wisej.Web.TextBox();
            this.Pb_Ref_Agency = new Wisej.Web.PictureBox();
            this.txtAgencyName = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.lblAgencyName = new Wisej.Web.Label();
            this.lblDate = new Wisej.Web.Label();
            this.calDate = new Wisej.Web.DateTimePicker();
            this.lblReferFromto = new Wisej.Web.Label();
            this.cmbReferFromTo = new Wisej.Web.ComboBox();
            this.pnlDetails = new Wisej.Web.Panel();
            this.pnlRefDet_Grid = new Wisej.Web.Panel();
            this.RefDet_Grid = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.cmbAgyRep = new Wisej.Web.DataGridViewComboBoxColumn();
            this.cmbCatg = new Wisej.Web.DataGridViewComboBoxColumn();
            this.cmbStatus = new Wisej.Web.DataGridViewComboBoxColumn();
            this.RefStat_Date = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvRefCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvRefID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvDelete = new Wisej.Web.DataGridViewImageColumn();
            this.pnlAddRepBtn = new Wisej.Web.Panel();
            this.lblDetails = new Wisej.Web.Label();
            this.btnAddRep = new Wisej.Web.Button();
            this.BtnSave = new Wisej.Web.Button();
            this.BtnCancel = new Wisej.Web.Button();
            this.Save_Panel = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlMain = new Wisej.Web.Panel();
            this.pnlData = new Wisej.Web.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Ref_Agency)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.pnlRefDet_Grid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RefDet_Grid)).BeginInit();
            this.pnlAddRepBtn.SuspendLayout();
            this.Save_Panel.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlData.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDateReq);
            this.panel1.Controls.Add(this.lblRefFromToReq);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.Pb_Ref_Agency);
            this.panel1.Controls.Add(this.txtAgencyName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblAgencyName);
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Controls.Add(this.calDate);
            this.panel1.Controls.Add(this.lblReferFromto);
            this.panel1.Controls.Add(this.cmbReferFromTo);
            this.panel1.Dock = Wisej.Web.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(673, 72);
            this.panel1.TabIndex = 1;
            // 
            // lblDateReq
            // 
            this.lblDateReq.AutoSize = true;
            this.lblDateReq.ForeColor = System.Drawing.Color.Red;
            this.lblDateReq.Location = new System.Drawing.Point(476, 8);
            this.lblDateReq.Name = "lblDateReq";
            this.lblDateReq.Size = new System.Drawing.Size(9, 14);
            this.lblDateReq.TabIndex = 3;
            this.lblDateReq.Text = "*";
            // 
            // lblRefFromToReq
            // 
            this.lblRefFromToReq.AutoSize = true;
            this.lblRefFromToReq.ForeColor = System.Drawing.Color.Red;
            this.lblRefFromToReq.Location = new System.Drawing.Point(117, 8);
            this.lblRefFromToReq.Name = "lblRefFromToReq";
            this.lblRefFromToReq.Size = new System.Drawing.Size(9, 14);
            this.lblRefFromToReq.TabIndex = 3;
            this.lblRefFromToReq.Text = "*";
            // 
            // txtCode
            // 
            this.txtCode.Enabled = false;
            this.txtCode.Location = new System.Drawing.Point(655, 5);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(10, 25);
            this.txtCode.TabIndex = 99;
            this.txtCode.TabStop = false;
            this.txtCode.Visible = false;
            // 
            // Pb_Ref_Agency
            // 
            this.Pb_Ref_Agency.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Ref_Agency.ImageSource = "captain-filter";
            this.Pb_Ref_Agency.Location = new System.Drawing.Point(618, 41);
            this.Pb_Ref_Agency.Name = "Pb_Ref_Agency";
            this.Pb_Ref_Agency.Size = new System.Drawing.Size(20, 20);
            this.Pb_Ref_Agency.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Ref_Agency.ToolTipText = "Select Agency Name";
            this.Pb_Ref_Agency.Click += new System.EventHandler(this.Pb_Ref_Agency_Click);
            // 
            // txtAgencyName
            // 
            this.txtAgencyName.Enabled = false;
            this.txtAgencyName.Location = new System.Drawing.Point(135, 38);
            this.txtAgencyName.Name = "txtAgencyName";
            this.txtAgencyName.Size = new System.Drawing.Size(475, 25);
            this.txtAgencyName.TabIndex = 3;
            this.txtAgencyName.TextChanged += new System.EventHandler(this.txtAgencyName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(95, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "*";
            // 
            // lblAgencyName
            // 
            this.lblAgencyName.AutoSize = true;
            this.lblAgencyName.Location = new System.Drawing.Point(15, 42);
            this.lblAgencyName.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblAgencyName.Name = "lblAgencyName";
            this.lblAgencyName.Size = new System.Drawing.Size(80, 16);
            this.lblAgencyName.TabIndex = 13;
            this.lblAgencyName.Text = "Agency Name";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(448, 11);
            this.lblDate.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(30, 16);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Date";
            // 
            // calDate
            // 
            this.calDate.CustomFormat = "MM/dd/yyyy";
            this.calDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.calDate.Location = new System.Drawing.Point(494, 7);
            this.calDate.MinimumSize = new System.Drawing.Size(116, 25);
            this.calDate.Name = "calDate";
            this.calDate.ShowCheckBox = true;
            this.calDate.ShowToolTips = false;
            this.calDate.Size = new System.Drawing.Size(116, 25);
            this.calDate.TabIndex = 2;
            // 
            // lblReferFromto
            // 
            this.lblReferFromto.AutoSize = true;
            this.lblReferFromto.Location = new System.Drawing.Point(15, 11);
            this.lblReferFromto.MinimumSize = new System.Drawing.Size(105, 16);
            this.lblReferFromto.Name = "lblReferFromto";
            this.lblReferFromto.Size = new System.Drawing.Size(105, 16);
            this.lblReferFromto.TabIndex = 13;
            this.lblReferFromto.Text = "Referred From/To";
            // 
            // cmbReferFromTo
            // 
            this.cmbReferFromTo.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbReferFromTo.FormattingEnabled = true;
            this.cmbReferFromTo.Location = new System.Drawing.Point(135, 7);
            this.cmbReferFromTo.Name = "cmbReferFromTo";
            this.cmbReferFromTo.SelectOnEnter = true;
            this.cmbReferFromTo.Size = new System.Drawing.Size(174, 25);
            this.cmbReferFromTo.TabIndex = 1;
            this.cmbReferFromTo.SelectedIndexChanged += new System.EventHandler(this.cmbReferFromTo_SelectedIndexChanged);
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.pnlRefDet_Grid);
            this.pnlDetails.Controls.Add(this.pnlAddRepBtn);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 72);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(673, 252);
            this.pnlDetails.TabIndex = 2;
            this.pnlDetails.Visible = false;
            // 
            // pnlRefDet_Grid
            // 
            this.pnlRefDet_Grid.Controls.Add(this.RefDet_Grid);
            this.pnlRefDet_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlRefDet_Grid.Location = new System.Drawing.Point(0, 27);
            this.pnlRefDet_Grid.Name = "pnlRefDet_Grid";
            this.pnlRefDet_Grid.Size = new System.Drawing.Size(673, 225);
            this.pnlRefDet_Grid.TabIndex = 100;
            // 
            // RefDet_Grid
            // 
            this.RefDet_Grid.AllowUserToResizeColumns = false;
            this.RefDet_Grid.AllowUserToResizeRows = false;
            this.RefDet_Grid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.RefDet_Grid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.RefDet_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.RefDet_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.RefDet_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RefDet_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.cmbAgyRep,
            this.cmbCatg,
            this.cmbStatus,
            this.RefStat_Date,
            this.gvRefCode,
            this.gvRefID,
            this.gvDelete});
            this.RefDet_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.RefDet_Grid.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.RefDet_Grid.MultiSelect = false;
            this.RefDet_Grid.Name = "RefDet_Grid";
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.RefDet_Grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.RefDet_Grid.RowHeadersWidth = 25;
            this.RefDet_Grid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RefDet_Grid.Size = new System.Drawing.Size(673, 225);
            this.RefDet_Grid.TabIndex = 0;
            this.RefDet_Grid.TabStop = false;
            this.RefDet_Grid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.RefDet_Grid_CellValueChanged);
            this.RefDet_Grid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.RefDet_Grid_CellClick);
            this.RefDet_Grid.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.RefDet_Grid_DataError);
            // 
            // cmbAgyRep
            // 
            dataGridViewCellStyle2.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackgroundImageSource = "combo-arrow";
            this.cmbAgyRep.DefaultCellStyle = dataGridViewCellStyle2;
            this.cmbAgyRep.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.cmbAgyRep.HeaderStyle = dataGridViewCellStyle3;
            this.cmbAgyRep.HeaderText = "Agency Representative";
            this.cmbAgyRep.Name = "cmbAgyRep";
            this.cmbAgyRep.ShowInVisibilityMenu = false;
            this.cmbAgyRep.ValueType = typeof(object);
            this.cmbAgyRep.Visible = false;
            this.cmbAgyRep.Width = 150;
            // 
            // cmbCatg
            // 
            dataGridViewCellStyle4.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackgroundImageSource = "combo-arrow";
            this.cmbCatg.DefaultCellStyle = dataGridViewCellStyle4;
            this.cmbCatg.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.cmbCatg.HeaderStyle = dataGridViewCellStyle5;
            this.cmbCatg.HeaderText = "Categories";
            this.cmbCatg.Name = "cmbCatg";
            this.cmbCatg.ShowInVisibilityMenu = false;
            this.cmbCatg.ValueType = typeof(object);
            this.cmbCatg.Visible = false;
            this.cmbCatg.Width = 280;
            // 
            // cmbStatus
            // 
            dataGridViewCellStyle6.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            dataGridViewCellStyle6.BackgroundImageSource = "combo-arrow";
            this.cmbStatus.DefaultCellStyle = dataGridViewCellStyle6;
            this.cmbStatus.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.cmbStatus.HeaderStyle = dataGridViewCellStyle7;
            this.cmbStatus.HeaderText = "Status";
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.ShowInVisibilityMenu = false;
            this.cmbStatus.ValueType = typeof(object);
            this.cmbStatus.Visible = false;
            this.cmbStatus.Width = 140;
            // 
            // RefStat_Date
            // 
            dataGridViewCellStyle8.BackgroundImageSource = "";
            this.RefStat_Date.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.RefStat_Date.HeaderStyle = dataGridViewCellStyle9;
            this.RefStat_Date.HeaderText = "Referral Expiration Date";
            this.RefStat_Date.Name = "RefStat_Date";
            this.RefStat_Date.Width = 130;
            // 
            // gvRefCode
            // 
            this.gvRefCode.HeaderText = "gvRefCode";
            this.gvRefCode.Name = "gvRefCode";
            this.gvRefCode.ShowInVisibilityMenu = false;
            this.gvRefCode.Visible = false;
            this.gvRefCode.Width = 40;
            // 
            // gvRefID
            // 
            this.gvRefID.HeaderText = "gvRefID";
            this.gvRefID.Name = "gvRefID";
            this.gvRefID.ShowInVisibilityMenu = false;
            this.gvRefID.Visible = false;
            this.gvRefID.Width = 40;
            // 
            // gvDelete
            // 
            this.gvDelete.CellImageSource = "captain-delete";
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.NullValue = null;
            this.gvDelete.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            this.gvDelete.HeaderStyle = dataGridViewCellStyle11;
            this.gvDelete.HeaderText = "Delete";
            this.gvDelete.Name = "gvDelete";
            this.gvDelete.ShowInVisibilityMenu = false;
            this.gvDelete.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.gvDelete.Visible = false;
            this.gvDelete.Width = 50;
            // 
            // pnlAddRepBtn
            // 
            this.pnlAddRepBtn.BackColor = System.Drawing.Color.FromArgb(237, 243, 249);
            this.pnlAddRepBtn.Controls.Add(this.lblDetails);
            this.pnlAddRepBtn.Controls.Add(this.btnAddRep);
            this.pnlAddRepBtn.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAddRepBtn.Location = new System.Drawing.Point(0, 0);
            this.pnlAddRepBtn.Name = "pnlAddRepBtn";
            this.pnlAddRepBtn.Padding = new Wisej.Web.Padding(15, 2, 15, 2);
            this.pnlAddRepBtn.Size = new System.Drawing.Size(673, 27);
            this.pnlAddRepBtn.TabIndex = 1;
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Dock = Wisej.Web.DockStyle.Left;
            this.lblDetails.Font = new System.Drawing.Font("defaultBold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblDetails.ForeColor = System.Drawing.Color.FromName("@buttonFace");
            this.lblDetails.Location = new System.Drawing.Point(15, 2);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(49, 23);
            this.lblDetails.TabIndex = 0;
            this.lblDetails.Text = "Details";
            this.lblDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddRep
            // 
            this.btnAddRep.Dock = Wisej.Web.DockStyle.Right;
            this.btnAddRep.Location = new System.Drawing.Point(506, 2);
            this.btnAddRep.Name = "btnAddRep";
            this.btnAddRep.Size = new System.Drawing.Size(152, 23);
            this.btnAddRep.TabIndex = 1;
            this.btnAddRep.Text = "Add &Representative";
            this.btnAddRep.Visible = false;
            this.btnAddRep.Click += new System.EventHandler(this.btnAddRep_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.AppearanceKey = "button-ok";
            this.BtnSave.Dock = Wisej.Web.DockStyle.Right;
            this.BtnSave.Location = new System.Drawing.Point(520, 5);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(60, 25);
            this.BtnSave.TabIndex = 1;
            this.BtnSave.Text = "&OK";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.AppearanceKey = "button-error";
            this.BtnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.BtnCancel.Location = new System.Drawing.Point(583, 5);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 25);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // Save_Panel
            // 
            this.Save_Panel.AppearanceKey = "panel-grdo";
            this.Save_Panel.Controls.Add(this.BtnSave);
            this.Save_Panel.Controls.Add(this.spacer1);
            this.Save_Panel.Controls.Add(this.BtnCancel);
            this.Save_Panel.Dock = Wisej.Web.DockStyle.Bottom;
            this.Save_Panel.Location = new System.Drawing.Point(0, 324);
            this.Save_Panel.Name = "Save_Panel";
            this.Save_Panel.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.Save_Panel.Size = new System.Drawing.Size(673, 35);
            this.Save_Panel.TabIndex = 3;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(580, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlData);
            this.pnlMain.Controls.Add(this.Save_Panel);
            this.pnlMain.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(673, 359);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.pnlDetails);
            this.pnlData.Controls.Add(this.panel1);
            this.pnlData.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 0);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(673, 324);
            this.pnlData.TabIndex = 0;
            // 
            // CASE0013_Form
            // 
            this.ClientSize = new System.Drawing.Size(673, 359);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CASE0013_Form";
            this.Text = "Referred From/To Details";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Ref_Agency)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.pnlRefDet_Grid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RefDet_Grid)).EndInit();
            this.pnlAddRepBtn.ResumeLayout(false);
            this.pnlAddRepBtn.PerformLayout();
            this.Save_Panel.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlData.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Panel panel1;
        private Label lblDate;
        private Label lblDateReq;
        private DateTimePicker calDate;
        private Label lblReferFromto;
        private Label lblRefFromToReq;
        private ComboBox cmbReferFromTo;
        private Label label2;
        private Label lblAgencyName;
        private Panel pnlDetails;
        private Label lblDetails;
        private DataGridViewEx RefDet_Grid;
        private DataGridViewComboBoxColumn cmbAgyRep;
        private DataGridViewComboBoxColumn cmbCatg;
        private DataGridViewComboBoxColumn cmbStatus;
        private Button BtnSave;
        private Button BtnCancel;
        private Panel Save_Panel;
        private TextBox txtAgencyName;
        private PictureBox Pb_Ref_Agency;
        private TextBox txtCode;
        private DataGridViewTextBoxColumn gvRefID;
        private DataGridViewTextBoxColumn gvRefCode;
        private DataGridViewImageColumn gvDelete;
        private Button btnAddRep;
        private Panel pnlMain;
        private Panel pnlData;
        private Panel pnlAddRepBtn;
        private Panel pnlRefDet_Grid;
        private Spacer spacer1;
        private DataGridViewTextBoxColumn RefStat_Date;
    }
}