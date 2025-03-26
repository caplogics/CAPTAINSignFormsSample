using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class HSS00133CustomQuestions
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle8 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            this.label13 = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.label17 = new Wisej.Web.Label();
            this.Btn_Cancel = new Wisej.Web.Button();
            this.Btn_Save = new Wisej.Web.Button();
            this.TxtAccess = new Wisej.Web.TextBox();
            this.TxtType = new Wisej.Web.TextBox();
            this.label2 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.label4 = new Wisej.Web.Label();
            this.label9 = new Wisej.Web.Label();
            this.label10 = new Wisej.Web.Label();
            this.CbActive = new Wisej.Web.CheckBox();
            this.checkBox5 = new Wisej.Web.CheckBox();
            this.TxtCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.CmbType = new Wisej.Web.ComboBox();
            this.TxtQuesDesc = new Wisej.Web.TextBox();
            this.TxtAbbr = new Wisej.Web.TextBox();
            this.CbFDate = new Wisej.Web.CheckBox();
            this.FurtreDtPanel = new Wisej.Web.Panel();
            this.MtxtSeq = new Wisej.Web.MaskedTextBox();
            this.CustCntlPanel = new Wisej.Web.Panel();
            this.CustRespPanel = new Wisej.Web.Panel();
            this.CustRespGrid = new Wisej.Web.DataGridView();
            this.RespCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.RespDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Type = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Changed = new Wisej.Web.DataGridViewTextBoxColumn();
            this.CustSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Empty_Row = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1 = new Wisej.Web.Panel();
            this.FurtreDtPanel.SuspendLayout();
            this.CustCntlPanel.SuspendLayout();
            this.CustRespPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustRespGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(444, 125);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 34;
            this.label13.Text = ".";
            this.label13.Visible = false;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(62, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 8);
            this.label6.TabIndex = 33;
            this.label6.Text = "*";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(62, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 8);
            this.label5.TabIndex = 33;
            this.label5.Text = "*";
            this.label5.Visible = false;
            // 
            // label17
            // 
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(67, 14);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(10, 8);
            this.label17.TabIndex = 33;
            this.label17.Text = "*";
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(382, 4);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(68, 26);
            this.Btn_Cancel.TabIndex = 11;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(310, 4);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(68, 26);
            this.Btn_Save.TabIndex = 10;
            this.Btn_Save.Text = "Save";
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // TxtAccess
            // 
            this.TxtAccess.Enabled = false;
            this.TxtAccess.Location = new System.Drawing.Point(404, 3);
            this.TxtAccess.Name = "TxtAccess";
            this.TxtAccess.Size = new System.Drawing.Size(17, 16);
            this.TxtAccess.TabIndex = 2;
            this.TxtAccess.Visible = false;
            // 
            // TxtType
            // 
            this.TxtType.Enabled = false;
            this.TxtType.Location = new System.Drawing.Point(372, 3);
            this.TxtType.Name = "TxtType";
            this.TxtType.Size = new System.Drawing.Size(26, 16);
            this.TxtType.TabIndex = 2;
            this.TxtType.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(309, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Code";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Sequence";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Ques Desc";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Abbreviation";
            // 
            // CbActive
            // 
            this.CbActive.Location = new System.Drawing.Point(78, 41);
            this.CbActive.Name = "CbActive";
            this.CbActive.Size = new System.Drawing.Size(56, 17);
            this.CbActive.TabIndex = 3;
            this.CbActive.Text = "Active";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(379, 38);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(69, 17);
            this.checkBox5.TabIndex = 1;
            this.checkBox5.Text = "Required";
            this.checkBox5.Visible = false;
            // 
            // TxtCode
            // 
            this.TxtCode.Enabled = false;
            this.TxtCode.Location = new System.Drawing.Point(377, 21);
            this.TxtCode.Name = "TxtCode";
            this.TxtCode.Size = new System.Drawing.Size(47, 17);
            this.TxtCode.TabIndex = 2;
            this.TxtCode.Visible = false;
            // 
            // CmbType
            // 
            this.CmbType.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.CmbType.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.CmbType.FormattingEnabled = true;
            this.CmbType.Location = new System.Drawing.Point(78, 16);
            this.CmbType.Name = "CmbType";
            this.CmbType.Size = new System.Drawing.Size(98, 21);
            this.CmbType.TabIndex = 2;
            this.CmbType.SelectedIndexChanged += new System.EventHandler(this.CmbType_SelectedIndexChanged);
            // 
            // TxtQuesDesc
            // 
            this.TxtQuesDesc.Location = new System.Drawing.Point(78, 87);
            this.TxtQuesDesc.MaxLength = 150;
            this.TxtQuesDesc.Multiline = true;
            this.TxtQuesDesc.Name = "TxtQuesDesc";
            this.TxtQuesDesc.Size = new System.Drawing.Size(370, 33);
            this.TxtQuesDesc.TabIndex = 7;
            // 
            // TxtAbbr
            // 
            this.TxtAbbr.Location = new System.Drawing.Point(78, 62);
            this.TxtAbbr.MaxLength = 25;
            this.TxtAbbr.Name = "TxtAbbr";
            this.TxtAbbr.Size = new System.Drawing.Size(203, 19);
            this.TxtAbbr.TabIndex = 5;
            // 
            // CbFDate
            // 
            this.CbFDate.Location = new System.Drawing.Point(2, 3);
            this.CbFDate.Name = "CbFDate";
            this.CbFDate.Size = new System.Drawing.Size(112, 17);
            this.CbFDate.TabIndex = 6;
            this.CbFDate.Text = "Allow Future Date";
            // 
            // FurtreDtPanel
            // 
            this.FurtreDtPanel.Controls.Add(this.CbFDate);
            this.FurtreDtPanel.Location = new System.Drawing.Point(284, 58);
            this.FurtreDtPanel.Name = "FurtreDtPanel";
            this.FurtreDtPanel.Size = new System.Drawing.Size(122, 24);
            this.FurtreDtPanel.TabIndex = 5;
            this.FurtreDtPanel.Visible = false;
            // 
            // MtxtSeq
            // 
            //this.MtxtSeq.CustomStyle = "Masked";
            this.MtxtSeq.Location = new System.Drawing.Point(19, 148);
            this.MtxtSeq.Name = "MtxtSeq";
            this.MtxtSeq.Size = new System.Drawing.Size(49, 17);
            this.MtxtSeq.TabIndex = 1;
            this.MtxtSeq.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.MtxtSeq.Visible = false;
            // 
            // CustCntlPanel
            // 
            this.CustCntlPanel.BorderStyle = Wisej.Web.BorderStyle.Solid;
            //this.CustCntlPanel.BorderWidth = new Wisej.Web.BorderWidth(0, 1, 0, 0);
            this.CustCntlPanel.Controls.Add(this.TxtQuesDesc);
            this.CustCntlPanel.Controls.Add(this.CustRespPanel);
            this.CustCntlPanel.Controls.Add(this.label13);
            this.CustCntlPanel.Controls.Add(this.label6);
            this.CustCntlPanel.Controls.Add(this.label5);
            this.CustCntlPanel.Controls.Add(this.label17);
            this.CustCntlPanel.Controls.Add(this.TxtAccess);
            this.CustCntlPanel.Controls.Add(this.TxtType);
            this.CustCntlPanel.Controls.Add(this.label2);
            this.CustCntlPanel.Controls.Add(this.label3);
            this.CustCntlPanel.Controls.Add(this.label4);
            this.CustCntlPanel.Controls.Add(this.label9);
            this.CustCntlPanel.Controls.Add(this.label10);
            this.CustCntlPanel.Controls.Add(this.CbActive);
            this.CustCntlPanel.Controls.Add(this.checkBox5);
            this.CustCntlPanel.Controls.Add(this.TxtCode);
            this.CustCntlPanel.Controls.Add(this.CmbType);
            this.CustCntlPanel.Controls.Add(this.TxtAbbr);
            this.CustCntlPanel.Controls.Add(this.FurtreDtPanel);
            this.CustCntlPanel.Controls.Add(this.MtxtSeq);
            this.CustCntlPanel.Location = new System.Drawing.Point(-1, -1);
            this.CustCntlPanel.Name = "CustCntlPanel";
            this.CustCntlPanel.Size = new System.Drawing.Size(469, 242);
            this.CustCntlPanel.TabIndex = 0;
            this.CustCntlPanel.Click += new System.EventHandler(this.CustCntlPanel_Click);
            // 
            // CustRespPanel
            // 
            this.CustRespPanel.Controls.Add(this.CustRespGrid);
            this.CustRespPanel.Location = new System.Drawing.Point(78, 123);
            this.CustRespPanel.Name = "CustRespPanel";
            this.CustRespPanel.Size = new System.Drawing.Size(370, 113);
            this.CustRespPanel.TabIndex = 9;
            // 
            // CustRespGrid
            // 
            this.CustRespGrid.AllowUserToDeleteRows = false;
            //this.CustRespGrid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.CustRespGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CustRespGrid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustRespGrid.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.RespCode,
            this.RespDesc,
            this.Type,
            this.Changed,
            this.CustSeq,
            this.Empty_Row});
            this.CustRespGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.CustRespGrid.Location = new System.Drawing.Point(0, 0);
            this.CustRespGrid.MultiSelect = false;
            this.CustRespGrid.Name = "CustRespGrid";
            this.CustRespGrid.RowHeadersWidth = 15;
            dataGridViewCellStyle8.FormatProvider = new System.Globalization.CultureInfo("en-US");
            //this.CustRespGrid.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.CustRespGrid.RowTemplate.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.CustRespGrid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.CustRespGrid.SelectionMode = Wisej.Web.DataGridViewSelectionMode.FullRowSelect;
            this.CustRespGrid.Size = new System.Drawing.Size(370, 113);
            this.CustRespGrid.TabIndex = 1;
            this.CustRespGrid.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.CustRespGrid_CellValueChanged);
            this.CustRespGrid.SelectionChanged += new System.EventHandler(this.CustRespGrid_SelectionChanged);
            // 
            // RespCode
            // 
            this.RespCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.RespCode.HeaderText = "Code";
            this.RespCode.MaxInputLength = 2;
            this.RespCode.Name = "RespCode";
            this.RespCode.Width = 45;
            // 
            // RespDesc
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.RespDesc.DefaultCellStyle = dataGridViewCellStyle3;
            this.RespDesc.HeaderText = "Response Description";
            this.RespDesc.Name = "RespDesc";
            this.RespDesc.Width = 300;
            // 
            // Type
            // 
            this.Type.DefaultCellStyle = dataGridViewCellStyle4;
            this.Type.Name = "Type";
            this.Type.Visible = false;
            // 
            // Changed
            // 
            this.Changed.DefaultCellStyle = dataGridViewCellStyle5;
            this.Changed.Name = "Changed";
            this.Changed.Visible = false;
            // 
            // CustSeq
            // 
            this.CustSeq.DefaultCellStyle = dataGridViewCellStyle6;
            this.CustSeq.Name = "CustSeq";
            this.CustSeq.Visible = false;
            // 
            // Empty_Row
            // 
            this.Empty_Row.DefaultCellStyle = dataGridViewCellStyle7;
            this.Empty_Row.Name = "Empty_Row";
            this.Empty_Row.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panel1.Controls.Add(this.Btn_Cancel);
            this.panel1.Controls.Add(this.Btn_Save);
            this.panel1.Location = new System.Drawing.Point(-1, 240);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(469, 35);
            this.panel1.TabIndex = 12;
            // 
            // HSS00133CustomQuestions
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CustCntlPanel);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(469, 275);
            this.Text = "HSS00133CustomQuestions";
            this.FurtreDtPanel.ResumeLayout(false);
            this.CustCntlPanel.ResumeLayout(false);
            this.CustRespPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CustRespGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label label13;
        private Label label6;
        private Label label5;
        private Label label17;
        private Button Btn_Cancel;
        private Button Btn_Save;
        private TextBox TxtAccess;
        private TextBox TxtType;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label9;
        private Label label10;
        private CheckBox CbActive;
        private CheckBox checkBox5;
        private TextBoxWithValidation TxtCode;
        private ComboBox CmbType;
        private TextBox TxtQuesDesc;
        private TextBox TxtAbbr;
        private CheckBox CbFDate;
        private Panel FurtreDtPanel;
        private MaskedTextBox MtxtSeq;
        private Panel CustCntlPanel;
        private Panel CustRespPanel;
        private DataGridView CustRespGrid;
        private DataGridViewTextBoxColumn RespCode;
        private DataGridViewTextBoxColumn RespDesc;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn Changed;
        private DataGridViewTextBoxColumn CustSeq;
        private DataGridViewTextBoxColumn Empty_Row;
        private Panel panel1;


    }
}