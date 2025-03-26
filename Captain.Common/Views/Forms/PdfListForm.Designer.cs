using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class PdfListForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PdfListForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.btnDelete = new Wisej.Web.Button();
            this.btnDeleteAll = new Wisej.Web.Button();
            this.btnPreview = new Wisej.Web.Button();
            this.btnDownload = new Wisej.Web.Button();
            this.btnMerge = new Wisej.Web.Button();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.pnlbuttons = new Wisej.Web.Panel();
            this.pnlDelOptions = new Wisej.Web.Panel();
            this.btnCancel = new Wisej.Web.Button();
            this.spacer3 = new Wisej.Web.Spacer();
            this.btnShowDelOptions = new Wisej.Web.PictureBox();
            this.spacer2 = new Wisej.Web.Spacer();
            this.spacer1 = new Wisej.Web.Spacer();
            this.SavePanel = new Wisej.Web.Panel();
            this.BtnSave = new Wisej.Web.Button();
            this.CbmFileType = new Wisej.Web.ComboBox();
            this.TxtFileName = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.dgvPrvRpts = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.chkSel = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.FileName = new Wisej.Web.DataGridViewColumn();
            this.FileExtension = new Wisej.Web.DataGridViewColumn();
            this.FileSize = new Captain.Common.Views.Controls.Compatibility.DataGridViewNumberColumn();
            this.sizeUnit = new Wisej.Web.DataGridViewTextBoxColumn();
            this.GeneratedDate = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.filepdfmark = new Wisej.Web.DataGridViewColumn();
            this.pnlDownldOptions = new Wisej.Web.Panel();
            this.pnlbuttons.SuspendLayout();
            this.pnlDelOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowDelOptions)).BeginInit();
            this.SavePanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrvRpts)).BeginInit();
            this.pnlDownldOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.AppearanceKey = "button-reports";
            this.btnDelete.Dock = Wisej.Web.DockStyle.Left;
            this.btnDelete.Location = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.AppearanceKey = "button-reports";
            this.btnDeleteAll.Dock = Wisej.Web.DockStyle.Left;
            this.btnDeleteAll.Location = new System.Drawing.Point(194, 5);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(75, 25);
            this.btnDeleteAll.TabIndex = 2;
            this.btnDeleteAll.Text = "Delete &All";
            this.btnDeleteAll.Visible = false;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.AppearanceKey = "button-reports";
            this.btnPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPreview.Location = new System.Drawing.Point(176, 0);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 25);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "&Preview";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.AppearanceKey = "button-reports";
            this.btnDownload.Dock = Wisej.Web.DockStyle.Right;
            this.btnDownload.Location = new System.Drawing.Point(8, 0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 25);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "D&ownload";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.AppearanceKey = "button-reports";
            this.btnMerge.Dock = Wisej.Web.DockStyle.Right;
            this.btnMerge.Location = new System.Drawing.Point(86, 0);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(85, 25);
            this.btnMerge.TabIndex = 2;
            this.btnMerge.Text = "&Excel Merge";
            this.btnMerge.Visible = false;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            this.contextMenu1.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.listViewPdf_MenuClick);
            // 
            // pnlbuttons
            // 
            this.pnlbuttons.AppearanceKey = "panel-grdo";
            this.pnlbuttons.Controls.Add(this.pnlDownldOptions);
            this.pnlbuttons.Controls.Add(this.btnDeleteAll);
            this.pnlbuttons.Controls.Add(this.pnlDelOptions);
            this.pnlbuttons.Controls.Add(this.btnShowDelOptions);
            this.pnlbuttons.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlbuttons.Location = new System.Drawing.Point(0, 377);
            this.pnlbuttons.Name = "pnlbuttons";
            this.pnlbuttons.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlbuttons.Size = new System.Drawing.Size(626, 35);
            this.pnlbuttons.TabIndex = 5;
            // 
            // pnlDelOptions
            // 
            this.pnlDelOptions.BackColor = System.Drawing.Color.Transparent;
            this.pnlDelOptions.Controls.Add(this.btnCancel);
            this.pnlDelOptions.Controls.Add(this.spacer3);
            this.pnlDelOptions.Controls.Add(this.btnDelete);
            this.pnlDelOptions.Dock = Wisej.Web.DockStyle.Left;
            this.pnlDelOptions.Location = new System.Drawing.Point(35, 5);
            this.pnlDelOptions.Name = "pnlDelOptions";
            this.pnlDelOptions.Size = new System.Drawing.Size(159, 25);
            this.pnlDelOptions.TabIndex = 0;
            this.pnlDelOptions.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-reports";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Left;
            this.btnCancel.Location = new System.Drawing.Point(78, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Left;
            this.spacer3.Location = new System.Drawing.Point(75, 0);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // btnShowDelOptions
            // 
            this.btnShowDelOptions.Cursor = Wisej.Web.Cursors.Hand;
            this.btnShowDelOptions.Dock = Wisej.Web.DockStyle.Left;
            this.btnShowDelOptions.ImageSource = "captain-delete";
            this.btnShowDelOptions.Location = new System.Drawing.Point(15, 5);
            this.btnShowDelOptions.Name = "btnShowDelOptions";
            this.btnShowDelOptions.Size = new System.Drawing.Size(20, 25);
            this.btnShowDelOptions.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.btnShowDelOptions.ToolTipText = "Delete Report(s)";
            this.btnShowDelOptions.Click += new System.EventHandler(this.btnShowDelOptions_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(83, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(171, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // SavePanel
            // 
            this.SavePanel.AppearanceKey = "panel-grdo";
            this.SavePanel.Controls.Add(this.BtnSave);
            this.SavePanel.Controls.Add(this.CbmFileType);
            this.SavePanel.Controls.Add(this.TxtFileName);
            this.SavePanel.Controls.Add(this.label2);
            this.SavePanel.Controls.Add(this.label1);
            this.SavePanel.Dock = Wisej.Web.DockStyle.Bottom;
            this.SavePanel.Location = new System.Drawing.Point(0, 307);
            this.SavePanel.Name = "SavePanel";
            this.SavePanel.Size = new System.Drawing.Size(626, 70);
            this.SavePanel.TabIndex = 6;
            this.SavePanel.TabStop = true;
            this.SavePanel.Visible = false;
            // 
            // BtnSave
            // 
            this.BtnSave.AppearanceKey = "button-reports";
            this.BtnSave.Location = new System.Drawing.Point(477, 34);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 25);
            this.BtnSave.TabIndex = 3;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // CbmFileType
            // 
            this.CbmFileType.Enabled = false;
            this.CbmFileType.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CbmFileType.FormattingEnabled = true;
            this.CbmFileType.Items.AddRange(new object[] {
            "*.pdf",
            "*.txt",
            "*.xlsx"});
            this.CbmFileType.Location = new System.Drawing.Point(80, 36);
            this.CbmFileType.Name = "CbmFileType";
            this.CbmFileType.Size = new System.Drawing.Size(386, 25);
            this.CbmFileType.TabIndex = 2;
            // 
            // TxtFileName
            // 
            this.TxtFileName.Location = new System.Drawing.Point(80, 8);
            this.TxtFileName.MaxLength = 55;
            this.TxtFileName.Name = "TxtFileName";
            this.TxtFileName.Size = new System.Drawing.Size(385, 25);
            this.TxtFileName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "Save as";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Name";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvPrvRpts);
            this.panel2.Dock = Wisej.Web.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(626, 307);
            this.panel2.TabIndex = 7;
            // 
            // dgvPrvRpts
            // 
            this.dgvPrvRpts.AllowUserToResizeColumns = false;
            this.dgvPrvRpts.AllowUserToResizeRows = false;
            this.dgvPrvRpts.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPrvRpts.BorderStyle = Wisej.Web.BorderStyle.None;
            this.dgvPrvRpts.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.chkSel,
            this.FileName,
            this.FileExtension,
            this.FileSize,
            this.sizeUnit,
            this.GeneratedDate,
            this.filepdfmark});
            this.dgvPrvRpts.ContextMenu = this.contextMenu1;
            this.dgvPrvRpts.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvPrvRpts.MultiSelect = false;
            this.dgvPrvRpts.Name = "dgvPrvRpts";
            this.dgvPrvRpts.RowHeadersVisible = false;
            this.dgvPrvRpts.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPrvRpts.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvPrvRpts.ShowColumnVisibilityMenu = false;
            this.dgvPrvRpts.Size = new System.Drawing.Size(626, 307);
            this.dgvPrvRpts.TabIndex = 0;
            this.dgvPrvRpts.SelectionChanged += new System.EventHandler(this.dgvPrvRpts_SelectionChanged);
            this.dgvPrvRpts.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.dgvPrvRpts_DataError);
            // 
            // chkSel
            // 
            this.chkSel.HeaderText = "";
            this.chkSel.Name = "chkSel";
            this.chkSel.Visible = false;
            this.chkSel.Width = 25;
            // 
            // FileName
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.CssStyle = "";
            dataGridViewCellStyle1.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.FileName.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.CssStyle = "";
            dataGridViewCellStyle2.Padding = new Wisej.Web.Padding(15, 0, 0, 0);
            this.FileName.HeaderStyle = dataGridViewCellStyle2;
            this.FileName.HeaderText = "Name";
            this.FileName.Name = "FileName";
            this.FileName.Width = 250;
            // 
            // FileExtension
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.FileExtension.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.FileExtension.HeaderStyle = dataGridViewCellStyle4;
            this.FileExtension.HeaderText = "Ext";
            this.FileExtension.Name = "FileExtension";
            this.FileExtension.Width = 40;
            // 
            // FileSize
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.FileSize.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleRight;
            this.FileSize.HeaderStyle = dataGridViewCellStyle6;
            this.FileSize.HeaderText = "Size";
            this.FileSize.Name = "FileSize";
            this.FileSize.Width = 80;
            // 
            // sizeUnit
            // 
            this.sizeUnit.HeaderText = "";
            this.sizeUnit.Name = "sizeUnit";
            this.sizeUnit.ShowInVisibilityMenu = false;
            this.sizeUnit.SortMode = Wisej.Web.DataGridViewColumnSortMode.NotSortable;
            this.sizeUnit.Width = 25;
            // 
            // GeneratedDate
            // 
            dataGridViewCellStyle7.Format = "MM/dd/yyyy";
            dataGridViewCellStyle7.Padding = new Wisej.Web.Padding(25, 0, 0, 0);
            this.GeneratedDate.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Padding = new Wisej.Web.Padding(25, 0, 0, 0);
            this.GeneratedDate.HeaderStyle = dataGridViewCellStyle8;
            this.GeneratedDate.HeaderText = "Generated On";
            this.GeneratedDate.Name = "GeneratedDate";
            this.GeneratedDate.ReadOnly = true;
            this.GeneratedDate.Width = 170;
            // 
            // filepdfmark
            // 
            this.filepdfmark.HeaderText = "filepdfmark";
            this.filepdfmark.Name = "filepdfmark";
            this.filepdfmark.Visible = false;
            // 
            // pnlDownldOptions
            // 
            this.pnlDownldOptions.BackColor = System.Drawing.Color.Transparent;
            this.pnlDownldOptions.Controls.Add(this.btnDownload);
            this.pnlDownldOptions.Controls.Add(this.spacer2);
            this.pnlDownldOptions.Controls.Add(this.btnMerge);
            this.pnlDownldOptions.Controls.Add(this.spacer1);
            this.pnlDownldOptions.Controls.Add(this.btnPreview);
            this.pnlDownldOptions.Dock = Wisej.Web.DockStyle.Right;
            this.pnlDownldOptions.Location = new System.Drawing.Point(360, 5);
            this.pnlDownldOptions.Name = "pnlDownldOptions";
            this.pnlDownldOptions.Size = new System.Drawing.Size(251, 25);
            this.pnlDownldOptions.TabIndex = 0;
            // 
            // PdfListForm
            // 
            this.ClientSize = new System.Drawing.Size(626, 412);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.SavePanel);
            this.Controls.Add(this.pnlbuttons);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PdfListForm";
            this.Text = "Report Viewer";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.pnlbuttons.ResumeLayout(false);
            this.pnlDelOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnShowDelOptions)).EndInit();
            this.SavePanel.ResumeLayout(false);
            this.SavePanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrvRpts)).EndInit();
            this.pnlDownldOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button btnDelete;
        private Button btnDeleteAll;
        private Button btnPreview;
        private Button btnDownload;
        private Button btnMerge;
        private ContextMenu contextMenu1;
        private Panel pnlbuttons;
        private Spacer spacer3;
        private Spacer spacer2;
        private Spacer spacer1;
        private Panel SavePanel;
        private Button BtnSave;
        private ComboBox CbmFileType;
        private TextBoxWithValidation TxtFileName;
        private Label label2;
        private Label label1;
        private Panel panel2;
        private DataGridViewEx dgvPrvRpts;
        private DataGridViewColumn FileName;
        private DataGridViewColumn FileExtension;
        private DataGridViewColumn filepdfmark;
        private DataGridViewCheckBoxColumn chkSel;
        private DataGridViewDateTimeColumn GeneratedDate;
        private DataGridViewNumberColumn FileSize;
        private DataGridViewTextBoxColumn sizeUnit;
        private Panel pnlDelOptions;
        private Button btnCancel;
        private PictureBox btnShowDelOptions;
        private Panel pnlDownldOptions;
    }
}