using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class SpanishCustomQuestions
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle12 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle13 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle14 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpanishCustomQuestions));
            this.lblSpDesc = new Wisej.Web.Label();
            this.TxtQuesDesc = new Wisej.Web.TextBox();
            this.lblQes = new Wisej.Web.Label();
            this.Cmbquestions = new Wisej.Web.ComboBox();
            this.lblResponse = new Wisej.Web.Label();
            this.pnlSave = new Wisej.Web.Panel();
            this.Btn_Save = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnMoveData = new Wisej.Web.Button();
            this.Btn_Cancel = new Wisej.Web.Button();
            this.pnlgridandQues = new Wisej.Web.Panel();
            this.pnlgvwResponses = new Wisej.Web.Panel();
            this.gvwResponses = new Wisej.Web.DataGridView();
            this.gvtCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCustCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtSeq = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCheckPIP = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtCheckStatus = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.pnlQues = new Wisej.Web.Panel();
            this.txtQDescrption = new Wisej.Web.TextBox();
            this.lblAgytabused = new Wisej.Web.Label();
            this.chkPipselectall = new Wisej.Web.CheckBox();
            this.chkActive = new Wisej.Web.CheckBox();
            this.chkSentPip = new Wisej.Web.CheckBox();
            this.Pb_Edit_Cust = new Wisej.Web.PictureBox();
            this.lblDesc = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlSave.SuspendLayout();
            this.pnlgridandQues.SuspendLayout();
            this.pnlgvwResponses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwResponses)).BeginInit();
            this.pnlQues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Edit_Cust)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSpDesc
            // 
            this.lblSpDesc.Location = new System.Drawing.Point(15, 94);
            this.lblSpDesc.Name = "lblSpDesc";
            this.lblSpDesc.Size = new System.Drawing.Size(76, 16);
            this.lblSpDesc.TabIndex = 0;
            this.lblSpDesc.Text = "Spanish Desc";
            // 
            // TxtQuesDesc
            // 
            this.TxtQuesDesc.Enabled = false;
            this.TxtQuesDesc.Location = new System.Drawing.Point(108, 75);
            this.TxtQuesDesc.MaxLength = 100;
            this.TxtQuesDesc.Multiline = true;
            this.TxtQuesDesc.Name = "TxtQuesDesc";
            this.TxtQuesDesc.Size = new System.Drawing.Size(404, 56);
            this.TxtQuesDesc.TabIndex = 7;
            // 
            // lblQes
            // 
            this.lblQes.Location = new System.Drawing.Point(15, 15);
            this.lblQes.Name = "lblQes";
            this.lblQes.Size = new System.Drawing.Size(58, 16);
            this.lblQes.TabIndex = 0;
            this.lblQes.Text = "Questions";
            // 
            // Cmbquestions
            // 
            this.Cmbquestions.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.Cmbquestions.FormattingEnabled = true;
            this.Cmbquestions.Location = new System.Drawing.Point(108, 11);
            this.Cmbquestions.Name = "Cmbquestions";
            this.Cmbquestions.Size = new System.Drawing.Size(404, 25);
            this.Cmbquestions.TabIndex = 2;
            this.Cmbquestions.SelectedIndexChanged += new System.EventHandler(this.Cmbquestions_SelectedIndexChanged);
            // 
            // lblResponse
            // 
            this.lblResponse.Location = new System.Drawing.Point(15, 141);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(61, 16);
            this.lblResponse.TabIndex = 0;
            this.lblResponse.Text = "Responses";
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.Btn_Save);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnMoveData);
            this.pnlSave.Controls.Add(this.Btn_Cancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 462);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(683, 35);
            this.pnlSave.TabIndex = 34;
            // 
            // Btn_Save
            // 
            this.Btn_Save.AppearanceKey = "button-ok";
            this.Btn_Save.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Save.Enabled = false;
            this.Btn_Save.Location = new System.Drawing.Point(515, 5);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(75, 25);
            this.Btn_Save.TabIndex = 10;
            this.Btn_Save.Text = "&Save";
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(590, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnMoveData
            // 
            this.btnMoveData.Dock = Wisej.Web.DockStyle.Left;
            this.btnMoveData.Location = new System.Drawing.Point(15, 5);
            this.btnMoveData.Name = "btnMoveData";
            this.btnMoveData.Size = new System.Drawing.Size(90, 25);
            this.btnMoveData.TabIndex = 12;
            this.btnMoveData.Text = "Copy to &PIP";
            this.btnMoveData.Click += new System.EventHandler(this.btnMoveData_Click);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.AppearanceKey = "button-error";
            this.Btn_Cancel.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_Cancel.Location = new System.Drawing.Point(593, 5);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(75, 25);
            this.Btn_Cancel.TabIndex = 11;
            this.Btn_Cancel.Text = "&Close";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // pnlgridandQues
            // 
            this.pnlgridandQues.Controls.Add(this.pnlgvwResponses);
            this.pnlgridandQues.Controls.Add(this.pnlQues);
            this.pnlgridandQues.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgridandQues.Location = new System.Drawing.Point(0, 0);
            this.pnlgridandQues.Name = "pnlgridandQues";
            this.pnlgridandQues.Size = new System.Drawing.Size(683, 462);
            this.pnlgridandQues.TabIndex = 34;
            // 
            // pnlgvwResponses
            // 
            this.pnlgvwResponses.Controls.Add(this.gvwResponses);
            this.pnlgvwResponses.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwResponses.Location = new System.Drawing.Point(0, 165);
            this.pnlgvwResponses.Name = "pnlgvwResponses";
            this.pnlgvwResponses.Size = new System.Drawing.Size(683, 297);
            this.pnlgvwResponses.TabIndex = 41;
            // 
            // gvwResponses
            // 
            this.gvwResponses.AllowDrop = true;
            this.gvwResponses.AllowUserToResizeColumns = false;
            this.gvwResponses.AllowUserToResizeRows = false;
            this.gvwResponses.BackColor = System.Drawing.Color.White;
            this.gvwResponses.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtCode,
            this.gvtDesc,
            this.gvtSDesc,
            this.gvtCustCode,
            this.gvtSeq,
            this.gvtCheckPIP,
            this.gvtCheckStatus});
            this.gvwResponses.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwResponses.Enabled = false;
            this.gvwResponses.Location = new System.Drawing.Point(0, 0);
            this.gvwResponses.Name = "gvwResponses";
            this.gvwResponses.RowHeadersWidth = 14;
            this.gvwResponses.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwResponses.Size = new System.Drawing.Size(683, 297);
            this.gvwResponses.TabIndex = 34;
            // 
            // gvtCode
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCode.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCode.HeaderStyle = dataGridViewCellStyle2;
            this.gvtCode.HeaderText = "Code";
            this.gvtCode.Name = "gvtCode";
            this.gvtCode.ReadOnly = true;
            this.gvtCode.Width = 55;
            // 
            // gvtDesc
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtDesc.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtDesc.HeaderStyle = dataGridViewCellStyle4;
            this.gvtDesc.HeaderText = "Description";
            this.gvtDesc.Name = "gvtDesc";
            this.gvtDesc.ReadOnly = true;
            this.gvtDesc.Width = 250;
            // 
            // gvtSDesc
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSDesc.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSDesc.HeaderStyle = dataGridViewCellStyle6;
            this.gvtSDesc.HeaderText = "Spanish Description";
            this.gvtSDesc.Name = "gvtSDesc";
            this.gvtSDesc.Width = 210;
            // 
            // gvtCustCode
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCustCode.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCustCode.HeaderStyle = dataGridViewCellStyle8;
            this.gvtCustCode.HeaderText = "gvtCustCode";
            this.gvtCustCode.Name = "gvtCustCode";
            this.gvtCustCode.ReadOnly = true;
            this.gvtCustCode.ShowInVisibilityMenu = false;
            this.gvtCustCode.Visible = false;
            this.gvtCustCode.Width = 20;
            // 
            // gvtSeq
            // 
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSeq.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtSeq.HeaderStyle = dataGridViewCellStyle10;
            this.gvtSeq.HeaderText = "gvtSeq";
            this.gvtSeq.Name = "gvtSeq";
            this.gvtSeq.ReadOnly = true;
            this.gvtSeq.ShowInVisibilityMenu = false;
            this.gvtSeq.Visible = false;
            this.gvtSeq.Width = 20;
            // 
            // gvtCheckPIP
            // 
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle11.NullValue = false;
            this.gvtCheckPIP.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCheckPIP.HeaderStyle = dataGridViewCellStyle12;
            this.gvtCheckPIP.HeaderText = "PIP";
            this.gvtCheckPIP.Name = "gvtCheckPIP";
            this.gvtCheckPIP.Width = 55;
            // 
            // gvtCheckStatus
            // 
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle13.NullValue = false;
            this.gvtCheckStatus.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCheckStatus.HeaderStyle = dataGridViewCellStyle14;
            this.gvtCheckStatus.HeaderText = "Status";
            this.gvtCheckStatus.Name = "gvtCheckStatus";
            this.gvtCheckStatus.Width = 55;
            // 
            // pnlQues
            // 
            this.pnlQues.Controls.Add(this.txtQDescrption);
            this.pnlQues.Controls.Add(this.lblResponse);
            this.pnlQues.Controls.Add(this.lblAgytabused);
            this.pnlQues.Controls.Add(this.lblQes);
            this.pnlQues.Controls.Add(this.chkPipselectall);
            this.pnlQues.Controls.Add(this.TxtQuesDesc);
            this.pnlQues.Controls.Add(this.chkActive);
            this.pnlQues.Controls.Add(this.lblSpDesc);
            this.pnlQues.Controls.Add(this.chkSentPip);
            this.pnlQues.Controls.Add(this.Cmbquestions);
            this.pnlQues.Controls.Add(this.Pb_Edit_Cust);
            this.pnlQues.Controls.Add(this.lblDesc);
            this.pnlQues.Dock = Wisej.Web.DockStyle.Top;
            this.pnlQues.Location = new System.Drawing.Point(0, 0);
            this.pnlQues.Name = "pnlQues";
            this.pnlQues.Size = new System.Drawing.Size(683, 165);
            this.pnlQues.TabIndex = 42;
            // 
            // txtQDescrption
            // 
            this.txtQDescrption.Location = new System.Drawing.Point(108, 43);
            this.txtQDescrption.Name = "txtQDescrption";
            this.txtQDescrption.ReadOnly = true;
            this.txtQDescrption.Size = new System.Drawing.Size(404, 25);
            this.txtQDescrption.TabIndex = 36;
            // 
            // lblAgytabused
            // 
            this.lblAgytabused.ForeColor = System.Drawing.Color.Red;
            this.lblAgytabused.Location = new System.Drawing.Point(72, 12);
            this.lblAgytabused.Name = "lblAgytabused";
            this.lblAgytabused.Size = new System.Drawing.Size(7, 10);
            this.lblAgytabused.TabIndex = 40;
            this.lblAgytabused.Text = "*";
            this.lblAgytabused.Visible = false;
            // 
            // chkPipselectall
            // 
            this.chkPipselectall.AutoSize = false;
            this.chkPipselectall.Enabled = false;
            this.chkPipselectall.Location = new System.Drawing.Point(542, 12);
            this.chkPipselectall.Name = "chkPipselectall";
            this.chkPipselectall.Size = new System.Drawing.Size(78, 20);
            this.chkPipselectall.TabIndex = 39;
            this.chkPipselectall.Text = "Select All";
            this.chkPipselectall.Visible = false;
            this.chkPipselectall.CheckedChanged += new System.EventHandler(this.chkPipselectall_CheckedChanged);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = false;
            this.chkActive.Enabled = false;
            this.chkActive.Location = new System.Drawing.Point(452, 138);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(61, 20);
            this.chkActive.TabIndex = 38;
            this.chkActive.Text = "Active";
            // 
            // chkSentPip
            // 
            this.chkSentPip.AutoSize = false;
            this.chkSentPip.Enabled = false;
            this.chkSentPip.Location = new System.Drawing.Point(332, 138);
            this.chkSentPip.Name = "chkSentPip";
            this.chkSentPip.Size = new System.Drawing.Size(91, 20);
            this.chkSentPip.TabIndex = 37;
            this.chkSentPip.Text = "Send to PIP";
            // 
            // Pb_Edit_Cust
            // 
            this.Pb_Edit_Cust.Cursor = Wisej.Web.Cursors.Hand;
            this.Pb_Edit_Cust.ImageSource = "captain-edit";
            this.Pb_Edit_Cust.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.Pb_Edit_Cust.Location = new System.Drawing.Point(631, 13);
            this.Pb_Edit_Cust.Name = "Pb_Edit_Cust";
            this.Pb_Edit_Cust.Size = new System.Drawing.Size(20, 20);
            this.Pb_Edit_Cust.Click += new System.EventHandler(this.Pb_Edit_Cust_Click);
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(15, 47);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(66, 16);
            this.lblDesc.TabIndex = 35;
            this.lblDesc.Text = "Description";
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgridandQues);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(683, 497);
            this.pnlCompleteForm.TabIndex = 35;
            // 
            // SpanishCustomQuestions
            // 
            this.ClientSize = new System.Drawing.Size(683, 497);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpanishCustomQuestions";
            this.Text = "Spanish Custom Questions";
            this.pnlSave.ResumeLayout(false);
            this.pnlgridandQues.ResumeLayout(false);
            this.pnlgvwResponses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwResponses)).EndInit();
            this.pnlQues.ResumeLayout(false);
            this.pnlQues.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Edit_Cust)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private Label lblSpDesc;
        private TextBox TxtQuesDesc;
        private Label lblQes;
        private ComboBox Cmbquestions;
        private Label lblResponse;
        private Panel pnlSave;
        private Panel pnlgridandQues;
        private Button Btn_Save;
        private Button Btn_Cancel;
        private PictureBox Pb_Edit_Cust;
        private TextBox txtQDescrption;
        private Label lblDesc;
        private DataGridView gvwResponses;
        private DataGridViewTextBoxColumn gvtCode;
        private DataGridViewTextBoxColumn gvtDesc;
        private DataGridViewTextBoxColumn gvtSDesc;
        private DataGridViewTextBoxColumn gvtCustCode;
        private DataGridViewTextBoxColumn gvtSeq;
        private Button btnMoveData;
        private CheckBox chkActive;
        private CheckBox chkSentPip;
        private DataGridViewCheckBoxColumn gvtCheckPIP;
        private DataGridViewCheckBoxColumn gvtCheckStatus;
        private CheckBox chkPipselectall;
        private Label lblAgytabused;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
        private Panel pnlgvwResponses;
        private Panel pnlQues;
    }
}