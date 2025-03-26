using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class FldcntlRespDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FldcntlRespDetails));
            this.label1 = new Wisej.Web.Label();
            this.txtQuesDesc = new Wisej.Web.TextBox();
            this.gvwResponses = new Wisej.Web.DataGridView();
            this.gvtStatus = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvCheck = new Wisej.Web.DataGridViewCheckBoxColumn();
            this.gvtResponse = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtCount = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtRespCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlgvwResponses = new Wisej.Web.Panel();
            this.pnlQues = new Wisej.Web.Panel();
            this.pnlSave = new Wisej.Web.Panel();
            this.btnRespSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnResponseDetails = new Wisej.Web.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvwResponses)).BeginInit();
            this.pnlCompleteForm.SuspendLayout();
            this.pnlgvwResponses.SuspendLayout();
            this.pnlQues.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Question";
            // 
            // txtQuesDesc
            // 
            this.txtQuesDesc.Enabled = false;
            this.txtQuesDesc.Location = new System.Drawing.Point(81, 9);
            this.txtQuesDesc.Multiline = true;
            this.txtQuesDesc.Name = "txtQuesDesc";
            this.txtQuesDesc.ReadOnly = true;
            this.txtQuesDesc.Size = new System.Drawing.Size(348, 56);
            this.txtQuesDesc.TabIndex = 1;
            // 
            // gvwResponses
            // 
            this.gvwResponses.AllowUserToResizeColumns = false;
            this.gvwResponses.AllowUserToResizeRows = false;
            this.gvwResponses.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvwResponses.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.gvwResponses.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwResponses.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtStatus,
            this.gvCheck,
            this.gvtResponse,
            this.gvtCount,
            this.gvtRespCode});
            this.gvwResponses.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwResponses.Location = new System.Drawing.Point(0, 0);
            this.gvwResponses.Name = "gvwResponses";
            this.gvwResponses.RowHeadersWidth = 14;
            this.gvwResponses.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwResponses.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwResponses.Size = new System.Drawing.Size(455, 254);
            this.gvwResponses.TabIndex = 2;
            // 
            // gvtStatus
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.NullValue = false;
            this.gvtStatus.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtStatus.HeaderStyle = dataGridViewCellStyle2;
            this.gvtStatus.HeaderText = "Status";
            this.gvtStatus.Name = "gvtStatus";
            this.gvtStatus.Width = 55;
            // 
            // gvCheck
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.NullValue = false;
            this.gvCheck.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvCheck.HeaderStyle = dataGridViewCellStyle4;
            this.gvCheck.HeaderText = "  ";
            this.gvCheck.Name = "gvCheck";
            this.gvCheck.ShowInVisibilityMenu = false;
            this.gvCheck.Width = 40;
            // 
            // gvtResponse
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.gvtResponse.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtResponse.HeaderStyle = dataGridViewCellStyle6;
            this.gvtResponse.HeaderText = "Responses";
            this.gvtResponse.Name = "gvtResponse";
            this.gvtResponse.ReadOnly = true;
            this.gvtResponse.Width = 250;
            // 
            // gvtCount
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCount.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtCount.HeaderStyle = dataGridViewCellStyle8;
            this.gvtCount.HeaderText = "Count";
            this.gvtCount.Name = "gvtCount";
            this.gvtCount.ReadOnly = true;
            this.gvtCount.Width = 80;
            // 
            // gvtRespCode
            // 
            dataGridViewCellStyle9.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtRespCode.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gvtRespCode.HeaderStyle = dataGridViewCellStyle10;
            this.gvtRespCode.HeaderText = " ";
            this.gvtRespCode.Name = "gvtRespCode";
            this.gvtRespCode.ReadOnly = true;
            this.gvtRespCode.ShowInVisibilityMenu = false;
            this.gvtRespCode.Visible = false;
            this.gvtRespCode.Width = 20;
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlgvwResponses);
            this.pnlCompleteForm.Controls.Add(this.pnlQues);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(455, 359);
            this.pnlCompleteForm.TabIndex = 3;
            // 
            // pnlgvwResponses
            // 
            this.pnlgvwResponses.Controls.Add(this.gvwResponses);
            this.pnlgvwResponses.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlgvwResponses.Location = new System.Drawing.Point(0, 70);
            this.pnlgvwResponses.Name = "pnlgvwResponses";
            this.pnlgvwResponses.Size = new System.Drawing.Size(455, 254);
            this.pnlgvwResponses.TabIndex = 6;
            // 
            // pnlQues
            // 
            this.pnlQues.Controls.Add(this.txtQuesDesc);
            this.pnlQues.Controls.Add(this.label1);
            this.pnlQues.Dock = Wisej.Web.DockStyle.Top;
            this.pnlQues.Location = new System.Drawing.Point(0, 0);
            this.pnlQues.Name = "pnlQues";
            this.pnlQues.Size = new System.Drawing.Size(455, 70);
            this.pnlQues.TabIndex = 7;
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnRespSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnResponseDetails);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 324);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(455, 35);
            this.pnlSave.TabIndex = 5;
            // 
            // btnRespSave
            // 
            this.btnRespSave.AppearanceKey = "button-ok";
            this.btnRespSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnRespSave.Location = new System.Drawing.Point(242, 5);
            this.btnRespSave.Name = "btnRespSave";
            this.btnRespSave.Size = new System.Drawing.Size(75, 25);
            this.btnRespSave.TabIndex = 4;
            this.btnRespSave.Text = "&Save";
            this.btnRespSave.Click += new System.EventHandler(this.btnRespSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(317, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnResponseDetails
            // 
            this.btnResponseDetails.Dock = Wisej.Web.DockStyle.Right;
            this.btnResponseDetails.Location = new System.Drawing.Point(320, 5);
            this.btnResponseDetails.Name = "btnResponseDetails";
            this.btnResponseDetails.Size = new System.Drawing.Size(120, 25);
            this.btnResponseDetails.TabIndex = 3;
            this.btnResponseDetails.Text = "&Response Details";
            this.btnResponseDetails.Click += new System.EventHandler(this.btnResponseDetails_Click);
            // 
            // FldcntlRespDetails
            // 
            this.ClientSize = new System.Drawing.Size(455, 359);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FldcntlRespDetails";
            this.Text = "Responses Details";
            ((System.ComponentModel.ISupportInitialize)(this.gvwResponses)).EndInit();
            this.pnlCompleteForm.ResumeLayout(false);
            this.pnlgvwResponses.ResumeLayout(false);
            this.pnlQues.ResumeLayout(false);
            this.pnlQues.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private TextBox txtQuesDesc;
        private DataGridView gvwResponses;
        private DataGridViewTextBoxColumn gvtResponse;
        private DataGridViewTextBoxColumn gvtCount;
        private Panel pnlCompleteForm;
        private Button btnResponseDetails;
        private DataGridViewCheckBoxColumn gvCheck;
        private DataGridViewTextBoxColumn gvtRespCode;
        private Button btnRespSave;
        private DataGridViewCheckBoxColumn gvtStatus;
        private Panel pnlSave;
        private Spacer spacer1;
        private Panel pnlgvwResponses;
        private Panel pnlQues;
    }
}