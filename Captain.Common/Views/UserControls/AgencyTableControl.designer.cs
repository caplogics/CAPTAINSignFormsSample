//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.UserControls
{
    partial class AgencyTableControl
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            this.btnConverstion = new Wisej.Web.Button();
            this.RbCode = new Wisej.Web.RadioButton();
            this.RbDesc = new Wisej.Web.RadioButton();
            this.label3 = new Wisej.Web.Label();
            this.Lbl_Req_Controls = new Wisej.Web.Label();
            this.picDelete = new Wisej.Web.PictureBox();
            this.cmbAgencyTable = new Wisej.Web.ComboBox();
            this.cmbModule = new Wisej.Web.ComboBox();
            this.label2 = new Wisej.Web.Label();
            this.label1 = new Wisej.Web.Label();
            this.gvwAgencyTable = new Wisej.Web.DataGridView();
            this.tabUserDetails = new Wisej.Web.TabControl();
            this.tabPageDetails = new Wisej.Web.TabPage();
            this.pnlgvwHie = new Wisej.Web.Panel();
            this.gvwHierarchy = new Wisej.Web.DataGridView();
            this.Hierarchy = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Hiearchy_Dcesription = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlAppNGrid = new Wisej.Web.Panel();
            this.pnlgvwApp = new Wisej.Web.Panel();
            this.pnlApp = new Wisej.Web.Panel();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieDetails = new Wisej.Web.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAgencyTable)).BeginInit();
            this.tabUserDetails.SuspendLayout();
            this.tabPageDetails.SuspendLayout();
            this.pnlgvwHie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchy)).BeginInit();
            this.pnlAppNGrid.SuspendLayout();
            this.pnlgvwApp.SuspendLayout();
            this.pnlApp.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConverstion
            // 
            this.btnConverstion.Location = new System.Drawing.Point(767, 13);
            this.btnConverstion.Name = "btnConverstion";
            this.btnConverstion.Size = new System.Drawing.Size(125, 25);
            this.btnConverstion.TabIndex = 5;
            this.btnConverstion.Text = "&Conversation Data";
            this.btnConverstion.Visible = false;
            // 
            // RbCode
            // 
            this.RbCode.AutoSize = false;
            this.RbCode.Location = new System.Drawing.Point(700, 14);
            this.RbCode.Name = "RbCode";
            this.RbCode.Size = new System.Drawing.Size(56, 20);
            this.RbCode.TabIndex = 4;
            this.RbCode.Text = "Code";
            this.RbCode.CheckedChanged += new System.EventHandler(this.RbCode_CheckedChanged);
            // 
            // RbDesc
            // 
            this.RbDesc.AutoSize = false;
            this.RbDesc.Checked = true;
            this.RbDesc.Location = new System.Drawing.Point(601, 14);
            this.RbDesc.Name = "RbDesc";
            this.RbDesc.Size = new System.Drawing.Size(92, 20);
            this.RbDesc.TabIndex = 3;
            this.RbDesc.TabStop = true;
            this.RbDesc.Text = "Description";
            this.RbDesc.CheckedChanged += new System.EventHandler(this.RbDesc_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(551, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "Sort On";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl_Req_Controls
            // 
            this.Lbl_Req_Controls.ForeColor = System.Drawing.Color.OrangeRed;
            this.Lbl_Req_Controls.Location = new System.Drawing.Point(553, 47);
            this.Lbl_Req_Controls.Name = "Lbl_Req_Controls";
            this.Lbl_Req_Controls.Size = new System.Drawing.Size(210, 16);
            this.Lbl_Req_Controls.TabIndex = 6;
            this.Lbl_Req_Controls.Text = "Required Controls";
            this.Lbl_Req_Controls.Visible = false;
            // 
            // picDelete
            // 
            this.picDelete.ImageSource = "captain-delete";
            this.picDelete.Location = new System.Drawing.Point(828, 44);
            this.picDelete.Name = "picDelete";
            this.picDelete.Size = new System.Drawing.Size(20, 20);
            this.picDelete.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.picDelete.Visible = false;
            this.picDelete.Click += new System.EventHandler(this.menuItemDel_Click);
            // 
            // cmbAgencyTable
            // 
            this.cmbAgencyTable.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbAgencyTable.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAgencyTable.Location = new System.Drawing.Point(104, 43);
            this.cmbAgencyTable.Name = "cmbAgencyTable";
            this.cmbAgencyTable.Size = new System.Drawing.Size(427, 25);
            this.cmbAgencyTable.TabIndex = 6;
            this.cmbAgencyTable.SelectedIndexChanged += new System.EventHandler(this.OnAgencyTableSelectedIndexChanged);
            // 
            // cmbModule
            // 
            this.cmbModule.AutoSize = false;
            this.cmbModule.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbModule.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(104, 11);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(253, 25);
            this.cmbModule.TabIndex = 2;
            this.cmbModule.SelectedIndexChanged += new System.EventHandler(this.cmbModule_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(15, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Agency Table";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("@defaultBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Application";
            // 
            // gvwAgencyTable
            // 
            this.gvwAgencyTable.AllowUserToAddRows = true;
            this.gvwAgencyTable.AllowUserToResizeRows = false;
            this.gvwAgencyTable.BackColor = System.Drawing.Color.White;
            this.gvwAgencyTable.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwAgencyTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvwAgencyTable.ColumnHeadersHeight = 25;
            this.gvwAgencyTable.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwAgencyTable.Cursor = Wisej.Web.Cursors.Arrow;
            this.gvwAgencyTable.DefaultRowHeight = 25;
            this.gvwAgencyTable.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwAgencyTable.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.gvwAgencyTable.Location = new System.Drawing.Point(0, 0);
            this.gvwAgencyTable.MultiSelect = false;
            this.gvwAgencyTable.Name = "gvwAgencyTable";
            this.gvwAgencyTable.RowHeadersWidth = 25;
            this.gvwAgencyTable.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwAgencyTable.Size = new System.Drawing.Size(980, 275);
            this.gvwAgencyTable.TabIndex = 8;
            this.gvwAgencyTable.SelectionChanged += new System.EventHandler(this.gvwAgencyTable_SelectionChanged);
            this.gvwAgencyTable.RowsAdded += new Wisej.Web.DataGridViewRowsAddedEventHandler(this.gvwAgencyTable_RowsAdded);
            this.gvwAgencyTable.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.gvwAgencyTable_CellClick);
            this.gvwAgencyTable.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.OnDataGridViewDataError);
            // 
            // tabUserDetails
            // 
            this.tabUserDetails.Controls.Add(this.tabPageDetails);
            this.tabUserDetails.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.tabUserDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.tabUserDetails.Location = new System.Drawing.Point(0, 5);
            this.tabUserDetails.Name = "tabUserDetails";
            this.tabUserDetails.PageInsets = new Wisej.Web.Padding(0, 27, 0, 0);
            this.tabUserDetails.SelectedIndex = 0;
            this.tabUserDetails.Size = new System.Drawing.Size(980, 322);
            this.tabUserDetails.TabIndex = 10;
            // 
            // tabPageDetails
            // 
            this.tabPageDetails.AutoScroll = true;
            this.tabPageDetails.Controls.Add(this.pnlgvwHie);
            this.tabPageDetails.Cursor = Wisej.Web.Cursors.Arrow;
            this.tabPageDetails.Location = new System.Drawing.Point(0, 27);
            this.tabPageDetails.Name = "tabPageDetails";
            this.tabPageDetails.Size = new System.Drawing.Size(980, 295);
            this.tabPageDetails.Text = "Hierarchy Details";
            // 
            // pnlgvwHie
            // 
            this.pnlgvwHie.Controls.Add(this.gvwHierarchy);
            this.pnlgvwHie.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwHie.Location = new System.Drawing.Point(0, 0);
            this.pnlgvwHie.Name = "pnlgvwHie";
            this.pnlgvwHie.Size = new System.Drawing.Size(980, 295);
            this.pnlgvwHie.TabIndex = 11;
            // 
            // gvwHierarchy
            // 
            this.gvwHierarchy.BackColor = System.Drawing.Color.White;
            this.gvwHierarchy.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle2.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvwHierarchy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvwHierarchy.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwHierarchy.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Hierarchy,
            this.Hiearchy_Dcesription});
            this.gvwHierarchy.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHierarchy.Location = new System.Drawing.Point(0, 0);
            this.gvwHierarchy.Name = "gvwHierarchy";
            this.gvwHierarchy.ReadOnly = true;
            this.gvwHierarchy.RowHeadersWidth = 25;
            this.gvwHierarchy.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.gvwHierarchy.Size = new System.Drawing.Size(980, 295);
            this.gvwHierarchy.TabIndex = 12;
            // 
            // Hierarchy
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.Hierarchy.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Hierarchy.HeaderStyle = dataGridViewCellStyle4;
            this.Hierarchy.HeaderText = "Hierarchy";
            this.Hierarchy.MinimumWidth = 75;
            this.Hierarchy.Name = "Hierarchy";
            this.Hierarchy.ReadOnly = true;
            this.Hierarchy.Resizable = Wisej.Web.DataGridViewTriState.False;
            // 
            // Hiearchy_Dcesription
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Hiearchy_Dcesription.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Hiearchy_Dcesription.HeaderStyle = dataGridViewCellStyle6;
            this.Hiearchy_Dcesription.HeaderText = "Description";
            this.Hiearchy_Dcesription.MinimumWidth = 546;
            this.Hiearchy_Dcesription.Name = "Hiearchy_Dcesription";
            this.Hiearchy_Dcesription.ReadOnly = true;
            this.Hiearchy_Dcesription.Width = 600;
            // 
            // pnlAppNGrid
            // 
            this.pnlAppNGrid.Controls.Add(this.pnlgvwApp);
            this.pnlAppNGrid.Controls.Add(this.pnlApp);
            this.pnlAppNGrid.CssStyle = "border-radius:8px; border:1px solid #ececec; ";
            this.pnlAppNGrid.Dock = Wisej.Web.DockStyle.Top;
            this.pnlAppNGrid.Location = new System.Drawing.Point(10, 10);
            this.pnlAppNGrid.Name = "pnlAppNGrid";
            this.pnlAppNGrid.Size = new System.Drawing.Size(980, 351);
            this.pnlAppNGrid.TabIndex = 8;
            // 
            // pnlgvwApp
            // 
            this.pnlgvwApp.Controls.Add(this.gvwAgencyTable);
            this.pnlgvwApp.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwApp.Location = new System.Drawing.Point(0, 76);
            this.pnlgvwApp.Name = "pnlgvwApp";
            this.pnlgvwApp.Size = new System.Drawing.Size(980, 275);
            this.pnlgvwApp.TabIndex = 7;
            // 
            // pnlApp
            // 
            this.pnlApp.Controls.Add(this.cmbAgencyTable);
            this.pnlApp.Controls.Add(this.picDelete);
            this.pnlApp.Controls.Add(this.btnConverstion);
            this.pnlApp.Controls.Add(this.Lbl_Req_Controls);
            this.pnlApp.Controls.Add(this.label3);
            this.pnlApp.Controls.Add(this.cmbModule);
            this.pnlApp.Controls.Add(this.RbCode);
            this.pnlApp.Controls.Add(this.label2);
            this.pnlApp.Controls.Add(this.label1);
            this.pnlApp.Controls.Add(this.RbDesc);
            this.pnlApp.Dock = Wisej.Web.DockStyle.Top;
            this.pnlApp.Location = new System.Drawing.Point(0, 0);
            this.pnlApp.Name = "pnlApp";
            this.pnlApp.Size = new System.Drawing.Size(980, 76);
            this.pnlApp.TabIndex = 1;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlHieDetails);
            this.pnlCompleteForm.Controls.Add(this.pnlAppNGrid);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 25);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Padding = new Wisej.Web.Padding(10);
            this.pnlCompleteForm.Size = new System.Drawing.Size(1000, 703);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // pnlHieDetails
            // 
            this.pnlHieDetails.Controls.Add(this.tabUserDetails);
            this.pnlHieDetails.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHieDetails.Location = new System.Drawing.Point(10, 361);
            this.pnlHieDetails.Name = "pnlHieDetails";
            this.pnlHieDetails.Padding = new Wisej.Web.Padding(0, 5, 0, 0);
            this.pnlHieDetails.Size = new System.Drawing.Size(980, 327);
            this.pnlHieDetails.TabIndex = 9;
            // 
            // AgencyTableControl
            // 
            this.Controls.Add(this.pnlCompleteForm);
            this.Name = "AgencyTableControl";
            this.Size = new System.Drawing.Size(1000, 728);
            this.Controls.SetChildIndex(this.pnlCompleteForm, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAgencyTable)).EndInit();
            this.tabUserDetails.ResumeLayout(false);
            this.tabPageDetails.ResumeLayout(false);
            this.pnlgvwHie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwHierarchy)).EndInit();
            this.pnlAppNGrid.ResumeLayout(false);
            this.pnlgvwApp.ResumeLayout(false);
            this.pnlApp.ResumeLayout(false);
            this.pnlApp.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieDetails.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DataGridView gvwAgencyTable;
        private TabControl tabUserDetails;
        private TabPage tabPageDetails;
        private ComboBox cmbAgencyTable;
        private ComboBox cmbModule;
        private Label label2;
        private Label label1;
        private DataGridView gvwHierarchy;
        private DataGridViewTextBoxColumn Hiearchy_Dcesription;
        private DataGridViewTextBoxColumn Hierarchy;
        private PictureBox picDelete;
        private Label Lbl_Req_Controls;
        private Label label3;
        private RadioButton RbCode;
        private RadioButton RbDesc;
        private Button btnConverstion;
        private Panel pnlAppNGrid;
        private Panel pnlgvwApp;
        private Panel pnlApp;
        private Panel pnlCompleteForm;
        private Panel pnlgvwHie;
        private Panel pnlHieDetails;
    }
}