using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class Report_Get_SaveParams_Form
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

        #region Wisej Web Form Designer generated code

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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle9 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_Get_SaveParams_Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.GetParams_Panel = new Wisej.Web.Panel();
            this.pnlIDs_Grid = new Wisej.Web.Panel();
            this.IDs_Grid = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.ID = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Desc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.LAST_UPDATED = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Associations_Count = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlSelect = new Wisej.Web.Panel();
            this.Btn_Select = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.Btn_Delete = new Wisej.Web.Button();
            this.SaveParams_Panel = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.Btn_Save = new Wisej.Web.Button();
            this.spacer2 = new Wisej.Web.Spacer();
            this.Btn_Cancel = new Wisej.Web.Button();
            this.pnlIDDesc = new Wisej.Web.Panel();
            this.btnOverwrite = new Wisej.Web.Button();
            this.TxtDesc = new Wisej.Web.TextBox();
            this.label1 = new Wisej.Web.Label();
            this.TxtID = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.columnFilter1 = new Wisej.Web.Ext.ColumnFilter.ColumnFilter(this.components);
            this.GetParams_Panel.SuspendLayout();
            this.pnlIDs_Grid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IDs_Grid)).BeginInit();
            this.pnlSelect.SuspendLayout();
            this.SaveParams_Panel.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlIDDesc.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnFilter1)).BeginInit();
            this.SuspendLayout();
            // 
            // GetParams_Panel
            // 
            this.GetParams_Panel.Controls.Add(this.pnlIDs_Grid);
            this.GetParams_Panel.Controls.Add(this.pnlSelect);
            this.GetParams_Panel.Dock = Wisej.Web.DockStyle.Top;
            this.GetParams_Panel.Location = new System.Drawing.Point(0, 0);
            this.GetParams_Panel.Name = "GetParams_Panel";
            this.GetParams_Panel.Size = new System.Drawing.Size(512, 314);
            this.GetParams_Panel.TabIndex = 1;
            this.GetParams_Panel.Visible = false;
            // 
            // pnlIDs_Grid
            // 
            this.pnlIDs_Grid.Controls.Add(this.IDs_Grid);
            this.pnlIDs_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlIDs_Grid.Location = new System.Drawing.Point(0, 0);
            this.pnlIDs_Grid.Name = "pnlIDs_Grid";
            this.pnlIDs_Grid.Size = new System.Drawing.Size(512, 279);
            this.pnlIDs_Grid.TabIndex = 1;
            // 
            // IDs_Grid
            // 
            this.IDs_Grid.AllowUserToResizeColumns = false;
            this.IDs_Grid.AllowUserToResizeRows = false;
            this.IDs_Grid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.IDs_Grid.BackColor = System.Drawing.SystemColors.Window;
            this.IDs_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.IDs_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.IDs_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IDs_Grid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.ID,
            this.Desc,
            this.LAST_UPDATED,
            this.Associations_Count});
            this.IDs_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.IDs_Grid.MultiSelect = false;
            this.IDs_Grid.Name = "IDs_Grid";
            this.IDs_Grid.RowHeadersWidth = 25;
            this.IDs_Grid.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.IDs_Grid.Size = new System.Drawing.Size(512, 279);
            this.IDs_Grid.TabIndex = 2;
            this.IDs_Grid.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(this.IDs_Grid_DataError);
            // 
            // ID
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ID.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ID.HeaderStyle = dataGridViewCellStyle3;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            this.ID.Width = 60;
            // 
            // Desc
            // 
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Desc.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Desc.HeaderStyle = dataGridViewCellStyle5;
            this.Desc.HeaderText = "Description";
            this.Desc.Name = "Desc";
            this.Desc.ReadOnly = true;
            this.columnFilter1.SetShowFilter(this.Desc, true);
            this.Desc.ValueType = typeof(string);
            this.Desc.Width = 310;
            // 
            // LAST_UPDATED
            // 
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LAST_UPDATED.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LAST_UPDATED.HeaderStyle = dataGridViewCellStyle7;
            this.LAST_UPDATED.HeaderText = "Last Updated";
            this.LAST_UPDATED.Name = "LAST_UPDATED";
            this.LAST_UPDATED.ReadOnly = true;
            this.columnFilter1.SetShowFilter(this.LAST_UPDATED, true);
            this.LAST_UPDATED.Width = 150;
            // 
            // Associations_Count
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Associations_Count.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Associations_Count.HeaderStyle = dataGridViewCellStyle9;
            this.Associations_Count.HeaderText = "Associations_Count";
            this.Associations_Count.Name = "Associations_Count";
            this.Associations_Count.ShowInVisibilityMenu = false;
            this.Associations_Count.Visible = false;
            // 
            // pnlSelect
            // 
            this.pnlSelect.AppearanceKey = "panel-grdo";
            this.pnlSelect.Controls.Add(this.Btn_Select);
            this.pnlSelect.Controls.Add(this.spacer1);
            this.pnlSelect.Controls.Add(this.Btn_Delete);
            this.pnlSelect.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSelect.Location = new System.Drawing.Point(0, 279);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSelect.Size = new System.Drawing.Size(512, 35);
            this.pnlSelect.TabIndex = 3;
            // 
            // Btn_Select
            // 
            this.Btn_Select.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Select.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Select.Location = new System.Drawing.Point(344, 5);
            this.Btn_Select.Name = "Btn_Select";
            this.Btn_Select.Size = new System.Drawing.Size(75, 25);
            this.Btn_Select.TabIndex = 4;
            this.Btn_Select.Text = "&Select";
            this.Btn_Select.Visible = false;
            this.Btn_Select.Click += new System.EventHandler(this.Btn_Select_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(419, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // Btn_Delete
            // 
            this.Btn_Delete.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Delete.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Delete.Location = new System.Drawing.Point(422, 5);
            this.Btn_Delete.Name = "Btn_Delete";
            this.Btn_Delete.Size = new System.Drawing.Size(75, 25);
            this.Btn_Delete.TabIndex = 5;
            this.Btn_Delete.Text = "&Delete";
            this.Btn_Delete.Visible = false;
            this.Btn_Delete.Click += new System.EventHandler(this.Btn_Delete_Click);
            // 
            // SaveParams_Panel
            // 
            this.SaveParams_Panel.Controls.Add(this.pnlSave);
            this.SaveParams_Panel.Controls.Add(this.pnlIDDesc);
            this.SaveParams_Panel.Dock = Wisej.Web.DockStyle.Top;
            this.SaveParams_Panel.Location = new System.Drawing.Point(0, 314);
            this.SaveParams_Panel.Name = "SaveParams_Panel";
            this.SaveParams_Panel.Size = new System.Drawing.Size(512, 113);
            this.SaveParams_Panel.TabIndex = 6;
            this.SaveParams_Panel.Visible = false;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.Btn_Save);
            this.pnlSave.Controls.Add(this.spacer2);
            this.pnlSave.Controls.Add(this.Btn_Cancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 78);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(512, 35);
            this.pnlSave.TabIndex = 10;
            // 
            // Btn_Save
            // 
            this.Btn_Save.AppearanceKey = "button-ok";
            this.Btn_Save.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Save.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Save.Location = new System.Drawing.Point(344, 5);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(75, 25);
            this.Btn_Save.TabIndex = 11;
            this.Btn_Save.Text = "&Save";
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Right;
            this.spacer2.Location = new System.Drawing.Point(419, 5);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(3, 25);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.AppearanceKey = "button-error";
            this.Btn_Cancel.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Cancel.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Cancel.Location = new System.Drawing.Point(422, 5);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(75, 25);
            this.Btn_Cancel.TabIndex = 12;
            this.Btn_Cancel.Text = "&Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // pnlIDDesc
            // 
            this.pnlIDDesc.Controls.Add(this.btnOverwrite);
            this.pnlIDDesc.Controls.Add(this.TxtDesc);
            this.pnlIDDesc.Controls.Add(this.label1);
            this.pnlIDDesc.Controls.Add(this.TxtID);
            this.pnlIDDesc.Controls.Add(this.label2);
            this.pnlIDDesc.Dock = Wisej.Web.DockStyle.Top;
            this.pnlIDDesc.Location = new System.Drawing.Point(0, 0);
            this.pnlIDDesc.Name = "pnlIDDesc";
            this.pnlIDDesc.Size = new System.Drawing.Size(512, 78);
            this.pnlIDDesc.TabIndex = 2;
            // 
            // btnOverwrite
            // 
            this.btnOverwrite.Location = new System.Drawing.Point(169, 2);
            this.btnOverwrite.Name = "btnOverwrite";
            this.btnOverwrite.Size = new System.Drawing.Size(75, 25);
            this.btnOverwrite.TabIndex = 8;
            this.btnOverwrite.Text = "&Overwrite";
            this.btnOverwrite.ToolTipText = "Overwrite Selected ID";
            this.btnOverwrite.Visible = false;
            this.btnOverwrite.Click += new System.EventHandler(this.btnOverwrite_Click);
            // 
            // TxtDesc
            // 
            this.TxtDesc.Location = new System.Drawing.Point(91, 33);
            this.TxtDesc.MaxLength = 50;
            this.TxtDesc.Name = "TxtDesc";
            this.TxtDesc.Size = new System.Drawing.Size(388, 25);
            this.TxtDesc.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            this.label1.Visible = false;
            // 
            // TxtID
            // 
            this.TxtID.Location = new System.Drawing.Point(89, 2);
            this.TxtID.MaxLength = 6;
            this.TxtID.Name = "TxtID";
            this.TxtID.Size = new System.Drawing.Size(52, 25);
            this.TxtID.TabIndex = 7;
            this.TxtID.Visible = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Description";
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.SaveParams_Panel);
            this.pnlCompleteForm.Controls.Add(this.GetParams_Panel);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(512, 427);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // columnFilter1
            // 
            this.columnFilter1.FilterPanelType = typeof(Wisej.Web.Ext.ColumnFilter.WhereColumnFilterPanel);
            this.columnFilter1.ImageSource = "grid-filter";
            // 
            // Report_Get_SaveParams_Form
            // 
            this.ClientSize = new System.Drawing.Size(512, 427);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Report_Get_SaveParams_Form";
            this.Text = "Save Parameters";
            componentTool1.ImageSource = "icon-help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.GetParams_Panel.ResumeLayout(false);
            this.pnlIDs_Grid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IDs_Grid)).EndInit();
            this.pnlSelect.ResumeLayout(false);
            this.SaveParams_Panel.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.pnlIDDesc.ResumeLayout(false);
            this.pnlIDDesc.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.columnFilter1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel GetParams_Panel;
        private Panel pnlSelect;
        private DataGridViewEx IDs_Grid;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Desc;
        private DataGridViewDateTimeColumn LAST_UPDATED;
        private Panel SaveParams_Panel;
        private Label label2;
        private Label label1;
        private Button Btn_Select;
        private Button Btn_Delete;
        private TextBox TxtDesc;
        private TextBox TxtID;
        private Panel pnlSave;
        private Button Btn_Cancel;
        private Button Btn_Save;
        private DataGridViewTextBoxColumn Associations_Count;
        private Button btnOverwrite;
        private Panel pnlCompleteForm;
        private Panel pnlIDs_Grid;
        private Spacer spacer1;
        private Spacer spacer2;
        private Panel pnlIDDesc;
        private Wisej.Web.Ext.ColumnFilter.ColumnFilter columnFilter1;
    }
}