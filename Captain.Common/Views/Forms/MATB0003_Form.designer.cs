using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class MATB0003_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MATB0003_Form));
            this.Pb_Search_Hie = new Wisej.Web.PictureBox();
            this.CmbYear = new Wisej.Web.ComboBox();
            this.Txt_HieDesc = new Wisej.Web.TextBox();
            this.pnlHie = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnGenerateFile = new Wisej.Web.Button();
            this.btnRepMaintPreview = new Wisej.Web.Button();
            this.Btn_Save_Params = new Wisej.Web.Button();
            this.Btn_Get_Params = new Wisej.Web.Button();
            this.pnlGenerate = new Wisej.Web.Panel();
            this.spacer4 = new Wisej.Web.Spacer();
            this.spacer3 = new Wisej.Web.Spacer();
            this.label8 = new Wisej.Web.Label();
            this.Rb_All_Scales = new Wisej.Web.RadioButton();
            this.Rb_Sel_Scales = new Wisej.Web.RadioButton();
            this.pnlScale = new Wisej.Web.Panel();
            this.label4 = new Wisej.Web.Label();
            this.Rb_Asmt_TDate = new Wisej.Web.RadioButton();
            this.Rb_Asmt_FDate = new Wisej.Web.RadioButton();
            this.panel4 = new Wisej.Web.Panel();
            this.label7 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.label2 = new Wisej.Web.Label();
            this.Asmt_T_Date = new Wisej.Web.DateTimePicker();
            this.Asmt_F_Date = new Wisej.Web.DateTimePicker();
            this.lblChkP1 = new Wisej.Web.Label();
            this.pnlParams = new Wisej.Web.Panel();
            this.pnlCseWorker = new Wisej.Web.Panel();
            this.Cmb_Worker = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.pnlChkPoint3 = new Wisej.Web.Panel();
            this.lblChkP3 = new Wisej.Web.Label();
            this.label9 = new Wisej.Web.Label();
            this.dtpCP3To = new Wisej.Web.DateTimePicker();
            this.dtpCP3From = new Wisej.Web.DateTimePicker();
            this.pnlChkPoint2 = new Wisej.Web.Panel();
            this.lblChkP2 = new Wisej.Web.Label();
            this.label5 = new Wisej.Web.Label();
            this.dtpCP2To = new Wisej.Web.DateTimePicker();
            this.dtpCP2From = new Wisej.Web.DateTimePicker();
            this.pnlChkPoint1 = new Wisej.Web.Panel();
            this.pnlMatrix = new Wisej.Web.Panel();
            this.Cmb_Matrix = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlHieFilter = new Wisej.Web.Panel();
            this.pnlFilter = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).BeginInit();
            this.pnlHie.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.pnlScale.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlParams.SuspendLayout();
            this.pnlCseWorker.SuspendLayout();
            this.pnlChkPoint3.SuspendLayout();
            this.pnlChkPoint2.SuspendLayout();
            this.pnlChkPoint1.SuspendLayout();
            this.pnlMatrix.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlHieFilter.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pb_Search_Hie
            // 
            this.Pb_Search_Hie.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.Pb_Search_Hie.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.Pb_Search_Hie.CssStyle = "border-radius:25px ";
            this.Pb_Search_Hie.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Search_Hie.Dock = Wisej.Web.DockStyle.Left;
            this.Pb_Search_Hie.ImageSource = "captain-filter";
            this.Pb_Search_Hie.Location = new System.Drawing.Point(15, 0);
            this.Pb_Search_Hie.Name = "Pb_Search_Hie";
            this.Pb_Search_Hie.Padding = new Wisej.Web.Padding(4, 5, 4, 4);
            this.Pb_Search_Hie.Size = new System.Drawing.Size(25, 25);
            this.Pb_Search_Hie.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Search_Hie.ToolTipText = "Select Hierarchy";
            this.Pb_Search_Hie.Click += new System.EventHandler(this.Pb_Search_Hie_Click);
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
            this.Txt_HieDesc.Size = new System.Drawing.Size(540, 25);
            this.Txt_HieDesc.TabIndex = 88;
            this.Txt_HieDesc.TabStop = false;
            this.Txt_HieDesc.TextAlign = Wisej.Web.HorizontalAlignment.Center;
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
            this.pnlHie.Size = new System.Drawing.Size(620, 25);
            this.pnlHie.TabIndex = 99;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Left;
            this.spacer1.Location = new System.Drawing.Point(540, 0);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(15, 25);
            // 
            // btnGenerateFile
            // 
            this.btnGenerateFile.AppearanceKey = "button-reports";
            this.btnGenerateFile.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenerateFile.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGenerateFile.Location = new System.Drawing.Point(515, 5);
            this.btnGenerateFile.Name = "btnGenerateFile";
            this.btnGenerateFile.Size = new System.Drawing.Size(85, 25);
            this.btnGenerateFile.TabIndex = 3;
            this.btnGenerateFile.Text = "&Generate";
            this.btnGenerateFile.Click += new System.EventHandler(this.btnGenerateFile_Click);
            // 
            // btnRepMaintPreview
            // 
            this.btnRepMaintPreview.AppearanceKey = "button-reports";
            this.btnRepMaintPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnRepMaintPreview.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnRepMaintPreview.Location = new System.Drawing.Point(603, 5);
            this.btnRepMaintPreview.Name = "btnRepMaintPreview";
            this.btnRepMaintPreview.Size = new System.Drawing.Size(85, 25);
            this.btnRepMaintPreview.TabIndex = 4;
            this.btnRepMaintPreview.Text = "Pre&view";
            this.btnRepMaintPreview.Click += new System.EventHandler(this.btnRepMaintPreview_Click);
            // 
            // Btn_Save_Params
            // 
            this.Btn_Save_Params.AppearanceKey = "button-reports";
            this.Btn_Save_Params.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Save_Params.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Save_Params.Location = new System.Drawing.Point(128, 5);
            this.Btn_Save_Params.Name = "Btn_Save_Params";
            this.Btn_Save_Params.Size = new System.Drawing.Size(115, 25);
            this.Btn_Save_Params.TabIndex = 2;
            this.Btn_Save_Params.Text = "&Save Parameters";
            this.Btn_Save_Params.Click += new System.EventHandler(this.Btn_Save_Params_Click);
            // 
            // Btn_Get_Params
            // 
            this.Btn_Get_Params.AppearanceKey = "button-reports";
            this.Btn_Get_Params.Dock = Wisej.Web.DockStyle.Left;
            this.Btn_Get_Params.Font = new System.Drawing.Font("@buttonTextFont", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Btn_Get_Params.Location = new System.Drawing.Point(15, 5);
            this.Btn_Get_Params.Name = "Btn_Get_Params";
            this.Btn_Get_Params.Size = new System.Drawing.Size(110, 25);
            this.Btn_Get_Params.TabIndex = 1;
            this.Btn_Get_Params.Text = "Get &Parameters";
            this.Btn_Get_Params.Click += new System.EventHandler(this.Btn_Get_Params_Click);
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.AppearanceKey = "panel-grdo";
            this.pnlGenerate.Controls.Add(this.Btn_Save_Params);
            this.pnlGenerate.Controls.Add(this.spacer4);
            this.pnlGenerate.Controls.Add(this.btnGenerateFile);
            this.pnlGenerate.Controls.Add(this.spacer3);
            this.pnlGenerate.Controls.Add(this.btnRepMaintPreview);
            this.pnlGenerate.Controls.Add(this.Btn_Get_Params);
            this.pnlGenerate.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlGenerate.Location = new System.Drawing.Point(0, 244);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlGenerate.Size = new System.Drawing.Size(703, 35);
            this.pnlGenerate.TabIndex = 2;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Left;
            this.spacer4.Location = new System.Drawing.Point(125, 5);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(3, 25);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Right;
            this.spacer3.Location = new System.Drawing.Point(600, 5);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(3, 25);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(15, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "Caseworker";
            // 
            // Rb_All_Scales
            // 
            this.Rb_All_Scales.AllowDrop = true;
            this.Rb_All_Scales.AutoSize = false;
            this.Rb_All_Scales.Checked = true;
            this.Rb_All_Scales.Location = new System.Drawing.Point(146, 4);
            this.Rb_All_Scales.Name = "Rb_All_Scales";
            this.Rb_All_Scales.Size = new System.Drawing.Size(46, 21);
            this.Rb_All_Scales.TabIndex = 1;
            this.Rb_All_Scales.TabStop = true;
            this.Rb_All_Scales.Text = "All";
            this.Rb_All_Scales.Click += new System.EventHandler(this.Rb_All_Scales_Click);
            // 
            // Rb_Sel_Scales
            // 
            this.Rb_Sel_Scales.AutoSize = false;
            this.Rb_Sel_Scales.Location = new System.Drawing.Point(206, 4);
            this.Rb_Sel_Scales.Name = "Rb_Sel_Scales";
            this.Rb_Sel_Scales.Size = new System.Drawing.Size(76, 21);
            this.Rb_Sel_Scales.TabIndex = 2;
            this.Rb_Sel_Scales.Text = "Selected";
            this.Rb_Sel_Scales.Click += new System.EventHandler(this.Rb_Sel_Scales_Click);
            // 
            // pnlScale
            // 
            this.pnlScale.Controls.Add(this.Rb_All_Scales);
            this.pnlScale.Controls.Add(this.Rb_Sel_Scales);
            this.pnlScale.Controls.Add(this.label4);
            this.pnlScale.Dock = Wisej.Web.DockStyle.Top;
            this.pnlScale.Location = new System.Drawing.Point(0, 39);
            this.pnlScale.Name = "pnlScale";
            this.pnlScale.Size = new System.Drawing.Size(703, 31);
            this.pnlScale.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(15, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Scale";
            // 
            // Rb_Asmt_TDate
            // 
            this.Rb_Asmt_TDate.Checked = true;
            this.Rb_Asmt_TDate.Location = new System.Drawing.Point(151, 1);
            this.Rb_Asmt_TDate.Name = "Rb_Asmt_TDate";
            this.Rb_Asmt_TDate.Size = new System.Drawing.Size(152, 21);
            this.Rb_Asmt_TDate.TabIndex = 97;
            this.Rb_Asmt_TDate.TabStop = true;
            this.Rb_Asmt_TDate.Text = "Last Assessment Date";
            // 
            // Rb_Asmt_FDate
            // 
            this.Rb_Asmt_FDate.AllowDrop = true;
            this.Rb_Asmt_FDate.Location = new System.Drawing.Point(4, 1);
            this.Rb_Asmt_FDate.Name = "Rb_Asmt_FDate";
            this.Rb_Asmt_FDate.Size = new System.Drawing.Size(153, 21);
            this.Rb_Asmt_FDate.TabIndex = 98;
            this.Rb_Asmt_FDate.Text = "First Assessment Date";
            this.Rb_Asmt_FDate.CheckedChanged += new System.EventHandler(this.Rb_Asmt_FDate_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Rb_Asmt_TDate);
            this.panel4.Controls.Add(this.Rb_Asmt_FDate);
            this.panel4.Location = new System.Drawing.Point(566, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(297, 20);
            this.panel4.TabIndex = 55;
            this.panel4.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(478, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 14);
            this.label7.TabIndex = 0;
            this.label7.Text = "Date Selection";
            this.label7.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Matrix";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(295, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "To";
            // 
            // Asmt_T_Date
            // 
            this.Asmt_T_Date.AllowDrop = true;
            this.Asmt_T_Date.AutoSize = false;
            this.Asmt_T_Date.CustomFormat = "MM/dd/yyyy";
            this.Asmt_T_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Asmt_T_Date.Location = new System.Drawing.Point(325, 3);
            this.Asmt_T_Date.Name = "Asmt_T_Date";
            this.Asmt_T_Date.ShowCheckBox = true;
            this.Asmt_T_Date.ShowToolTips = false;
            this.Asmt_T_Date.Size = new System.Drawing.Size(116, 25);
            this.Asmt_T_Date.TabIndex = 2;
            // 
            // Asmt_F_Date
            // 
            this.Asmt_F_Date.AutoSize = false;
            this.Asmt_F_Date.CustomFormat = "MM/dd/yyyy";
            this.Asmt_F_Date.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.Asmt_F_Date.Location = new System.Drawing.Point(150, 3);
            this.Asmt_F_Date.Name = "Asmt_F_Date";
            this.Asmt_F_Date.ShowCheckBox = true;
            this.Asmt_F_Date.ShowToolTips = false;
            this.Asmt_F_Date.Size = new System.Drawing.Size(116, 25);
            this.Asmt_F_Date.TabIndex = 1;
            // 
            // lblChkP1
            // 
            this.lblChkP1.Location = new System.Drawing.Point(14, 7);
            this.lblChkP1.Name = "lblChkP1";
            this.lblChkP1.Size = new System.Drawing.Size(117, 16);
            this.lblChkP1.TabIndex = 0;
            this.lblChkP1.Text = "Checkpoint #1 From";
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.pnlCseWorker);
            this.pnlParams.Controls.Add(this.pnlChkPoint3);
            this.pnlParams.Controls.Add(this.pnlChkPoint2);
            this.pnlParams.Controls.Add(this.pnlChkPoint1);
            this.pnlParams.Controls.Add(this.pnlScale);
            this.pnlParams.Controls.Add(this.pnlMatrix);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 43);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(703, 201);
            this.pnlParams.TabIndex = 1;
            // 
            // pnlCseWorker
            // 
            this.pnlCseWorker.Controls.Add(this.label8);
            this.pnlCseWorker.Controls.Add(this.Cmb_Worker);
            this.pnlCseWorker.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCseWorker.Location = new System.Drawing.Point(0, 163);
            this.pnlCseWorker.Name = "pnlCseWorker";
            this.pnlCseWorker.Size = new System.Drawing.Size(703, 38);
            this.pnlCseWorker.TabIndex = 6;
            // 
            // Cmb_Worker
            // 
            this.Cmb_Worker.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb_Worker.FormattingEnabled = true;
            this.Cmb_Worker.Location = new System.Drawing.Point(150, 3);
            this.Cmb_Worker.Name = "Cmb_Worker";
            this.Cmb_Worker.Size = new System.Drawing.Size(291, 25);
            this.Cmb_Worker.TabIndex = 1;
            // 
            // pnlChkPoint3
            // 
            this.pnlChkPoint3.Controls.Add(this.lblChkP3);
            this.pnlChkPoint3.Controls.Add(this.label9);
            this.pnlChkPoint3.Controls.Add(this.dtpCP3To);
            this.pnlChkPoint3.Controls.Add(this.dtpCP3From);
            this.pnlChkPoint3.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChkPoint3.Location = new System.Drawing.Point(0, 132);
            this.pnlChkPoint3.Name = "pnlChkPoint3";
            this.pnlChkPoint3.Size = new System.Drawing.Size(703, 31);
            this.pnlChkPoint3.TabIndex = 5;
            // 
            // lblChkP3
            // 
            this.lblChkP3.Location = new System.Drawing.Point(17, 7);
            this.lblChkP3.Name = "lblChkP3";
            this.lblChkP3.Size = new System.Drawing.Size(115, 16);
            this.lblChkP3.TabIndex = 0;
            this.lblChkP3.Text = "Checkpoint #3 From";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(295, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "To";
            // 
            // dtpCP3To
            // 
            this.dtpCP3To.AllowDrop = true;
            this.dtpCP3To.AutoSize = false;
            this.dtpCP3To.CustomFormat = "MM/dd/yyyy";
            this.dtpCP3To.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpCP3To.Location = new System.Drawing.Point(326, 3);
            this.dtpCP3To.Name = "dtpCP3To";
            this.dtpCP3To.ShowCheckBox = true;
            this.dtpCP3To.ShowToolTips = false;
            this.dtpCP3To.Size = new System.Drawing.Size(116, 25);
            this.dtpCP3To.TabIndex = 2;
            // 
            // dtpCP3From
            // 
            this.dtpCP3From.AutoSize = false;
            this.dtpCP3From.CustomFormat = "MM/dd/yyyy";
            this.dtpCP3From.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpCP3From.Location = new System.Drawing.Point(150, 3);
            this.dtpCP3From.Name = "dtpCP3From";
            this.dtpCP3From.ShowCheckBox = true;
            this.dtpCP3From.ShowToolTips = false;
            this.dtpCP3From.Size = new System.Drawing.Size(116, 25);
            this.dtpCP3From.TabIndex = 1;
            // 
            // pnlChkPoint2
            // 
            this.pnlChkPoint2.Controls.Add(this.lblChkP2);
            this.pnlChkPoint2.Controls.Add(this.label5);
            this.pnlChkPoint2.Controls.Add(this.dtpCP2To);
            this.pnlChkPoint2.Controls.Add(this.dtpCP2From);
            this.pnlChkPoint2.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChkPoint2.Location = new System.Drawing.Point(0, 101);
            this.pnlChkPoint2.Name = "pnlChkPoint2";
            this.pnlChkPoint2.Size = new System.Drawing.Size(703, 31);
            this.pnlChkPoint2.TabIndex = 4;
            // 
            // lblChkP2
            // 
            this.lblChkP2.Location = new System.Drawing.Point(15, 7);
            this.lblChkP2.Name = "lblChkP2";
            this.lblChkP2.Size = new System.Drawing.Size(115, 16);
            this.lblChkP2.TabIndex = 0;
            this.lblChkP2.Text = "Checkpoint #2 From";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(295, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "To";
            // 
            // dtpCP2To
            // 
            this.dtpCP2To.AllowDrop = true;
            this.dtpCP2To.AutoSize = false;
            this.dtpCP2To.CustomFormat = "MM/dd/yyyy";
            this.dtpCP2To.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpCP2To.Location = new System.Drawing.Point(325, 3);
            this.dtpCP2To.Name = "dtpCP2To";
            this.dtpCP2To.ShowCheckBox = true;
            this.dtpCP2To.ShowToolTips = false;
            this.dtpCP2To.Size = new System.Drawing.Size(116, 25);
            this.dtpCP2To.TabIndex = 2;
            // 
            // dtpCP2From
            // 
            this.dtpCP2From.AutoSize = false;
            this.dtpCP2From.CustomFormat = "MM/dd/yyyy";
            this.dtpCP2From.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpCP2From.Location = new System.Drawing.Point(150, 3);
            this.dtpCP2From.Name = "dtpCP2From";
            this.dtpCP2From.ShowCheckBox = true;
            this.dtpCP2From.ShowToolTips = false;
            this.dtpCP2From.Size = new System.Drawing.Size(116, 25);
            this.dtpCP2From.TabIndex = 1;
            // 
            // pnlChkPoint1
            // 
            this.pnlChkPoint1.Controls.Add(this.lblChkP1);
            this.pnlChkPoint1.Controls.Add(this.Asmt_F_Date);
            this.pnlChkPoint1.Controls.Add(this.Asmt_T_Date);
            this.pnlChkPoint1.Controls.Add(this.label2);
            this.pnlChkPoint1.Controls.Add(this.label7);
            this.pnlChkPoint1.Controls.Add(this.panel4);
            this.pnlChkPoint1.Dock = Wisej.Web.DockStyle.Top;
            this.pnlChkPoint1.Location = new System.Drawing.Point(0, 70);
            this.pnlChkPoint1.Name = "pnlChkPoint1";
            this.pnlChkPoint1.Size = new System.Drawing.Size(703, 31);
            this.pnlChkPoint1.TabIndex = 3;
            // 
            // pnlMatrix
            // 
            this.pnlMatrix.Controls.Add(this.Cmb_Matrix);
            this.pnlMatrix.Controls.Add(this.label3);
            this.pnlMatrix.Dock = Wisej.Web.DockStyle.Top;
            this.pnlMatrix.Location = new System.Drawing.Point(0, 0);
            this.pnlMatrix.Name = "pnlMatrix";
            this.pnlMatrix.Size = new System.Drawing.Size(703, 39);
            this.pnlMatrix.TabIndex = 1;
            // 
            // Cmb_Matrix
            // 
            this.Cmb_Matrix.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmb_Matrix.FormattingEnabled = true;
            this.Cmb_Matrix.Location = new System.Drawing.Point(150, 11);
            this.Cmb_Matrix.Name = "Cmb_Matrix";
            this.Cmb_Matrix.Size = new System.Drawing.Size(291, 25);
            this.Cmb_Matrix.TabIndex = 1;
            this.Cmb_Matrix.SelectedIndexChanged += new System.EventHandler(this.Cmb_Matrix_SelectedIndexChanged);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlHieFilter);
            this.pnlCompleteForm.Controls.Add(this.pnlGenerate);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(703, 279);
            this.pnlCompleteForm.TabIndex = 0;
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
            this.pnlHieFilter.Size = new System.Drawing.Size(703, 43);
            this.pnlHieFilter.TabIndex = 110;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.Pb_Search_Hie);
            this.pnlFilter.Controls.Add(this.spacer2);
            this.pnlFilter.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(635, 9);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(59, 25);
            this.pnlFilter.TabIndex = 66;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Left;
            this.spacer2.Location = new System.Drawing.Point(0, 0);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(15, 25);
            // 
            // MATB0003_Form
            // 
            this.ClientSize = new System.Drawing.Size(703, 279);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MATB0003_Form";
            this.Text = "MATB0003_Form";
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Search_Hie)).EndInit();
            this.pnlHie.ResumeLayout(false);
            this.pnlHie.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            this.pnlScale.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlParams.ResumeLayout(false);
            this.pnlCseWorker.ResumeLayout(false);
            this.pnlCseWorker.PerformLayout();
            this.pnlChkPoint3.ResumeLayout(false);
            this.pnlChkPoint2.ResumeLayout(false);
            this.pnlChkPoint1.ResumeLayout(false);
            this.pnlChkPoint1.PerformLayout();
            this.pnlMatrix.ResumeLayout(false);
            this.pnlMatrix.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlHieFilter.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox Pb_Search_Hie;
        private ComboBox CmbYear;
        private TextBox Txt_HieDesc;
        private Panel pnlHie;
        private Button btnGenerateFile;
        private Button btnRepMaintPreview;
        private Button Btn_Save_Params;
        private Button Btn_Get_Params;
        private Panel pnlGenerate;
        private ComboBoxEx Cmb_Worker;
        private Label label8;
        private RadioButton Rb_All_Scales;
        private RadioButton Rb_Sel_Scales;
        private Panel pnlScale;
        private RadioButton Rb_Asmt_TDate;
        private RadioButton Rb_Asmt_FDate;
        private Panel panel4;
        private Label label7;
        private Label label4;
        private ComboBoxEx Cmb_Matrix;
        private Label label3;
        private Label label2;
        private DateTimePicker Asmt_T_Date;
        private DateTimePicker Asmt_F_Date;
        private Label lblChkP1;
        private Panel pnlParams;
        private Label lblChkP3;
        private DateTimePicker dtpCP3From;
        private DateTimePicker dtpCP3To;
        private Label label9;
        private Label lblChkP2;
        private DateTimePicker dtpCP2From;
        private DateTimePicker dtpCP2To;
        private Label label5;
        private Panel pnlCompleteForm;
        private Panel pnlHieFilter;
        private Spacer spacer1;
        private Panel pnlFilter;
        private Spacer spacer2;
        private Spacer spacer3;
        private Spacer spacer4;
        private Panel pnlMatrix;
        private Panel pnlChkPoint1;
        private Panel pnlChkPoint2;
        private Panel pnlChkPoint3;
        private Panel pnlCseWorker;
    }
}