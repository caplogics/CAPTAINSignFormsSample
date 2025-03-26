using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class RankCategoriesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RankCategoriesForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.lblCode = new Wisej.Web.Label();
            this.lbldesc = new Wisej.Web.Label();
            this.lblHeadstart = new Wisej.Web.Label();
            this.txtDesc = new Wisej.Web.TextBox();
            this.cmbRankCat = new Wisej.Web.ComboBox();
            this.lblPointRng = new Wisej.Web.Label();
            this.lblTo = new Wisej.Web.Label();
            this.lblFrom = new Wisej.Web.Label();
            this.lblReqDesc = new Wisej.Web.Label();
            this.lblReqPR = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.btnSave = new Wisej.Web.Button();
            this.spacer1 = new Wisej.Web.Spacer();
            this.btnCancel = new Wisej.Web.Button();
            this.pnlCode = new Wisej.Web.Panel();
            this.lblReqCode = new Wisej.Web.Label();
            this.txtFrom = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtTo = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtCode = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label1 = new Wisej.Web.Label();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel2.SuspendLayout();
            this.pnlCode.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCode
            // 
            this.lblCode.Location = new System.Drawing.Point(15, 15);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(30, 14);
            this.lblCode.TabIndex = 1;
            this.lblCode.Text = "Code";
            // 
            // lbldesc
            // 
            this.lbldesc.Location = new System.Drawing.Point(112, 15);
            this.lbldesc.Name = "lbldesc";
            this.lbldesc.Size = new System.Drawing.Size(66, 16);
            this.lbldesc.TabIndex = 1;
            this.lbldesc.Text = "Description";
            // 
            // lblHeadstart
            // 
            this.lblHeadstart.Location = new System.Drawing.Point(15, 47);
            this.lblHeadstart.Name = "lblHeadstart";
            this.lblHeadstart.Size = new System.Drawing.Size(164, 16);
            this.lblHeadstart.TabIndex = 1;
            this.lblHeadstart.Text = "Head Start Ranking Category";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(191, 11);
            this.txtDesc.MaxLength = 50;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(309, 25);
            this.txtDesc.TabIndex = 2;
            this.txtDesc.LostFocus += new System.EventHandler(this.txtDesc_LostFocus);
            // 
            // cmbRankCat
            // 
            this.cmbRankCat.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbRankCat.FormattingEnabled = true;
            this.cmbRankCat.Location = new System.Drawing.Point(191, 43);
            this.cmbRankCat.Name = "cmbRankCat";
            this.cmbRankCat.Size = new System.Drawing.Size(91, 25);
            this.cmbRankCat.TabIndex = 3;
            // 
            // lblPointRng
            // 
            this.lblPointRng.Location = new System.Drawing.Point(15, 79);
            this.lblPointRng.Name = "lblPointRng";
            this.lblPointRng.Size = new System.Drawing.Size(74, 16);
            this.lblPointRng.TabIndex = 1;
            this.lblPointRng.Text = "Points Range";
            this.lblPointRng.Visible = false;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(266, 79);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(14, 14);
            this.lblTo.TabIndex = 1;
            this.lblTo.Text = "To";
            this.lblTo.Visible = false;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(147, 79);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 14);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.Text = "From";
            this.lblFrom.Visible = false;
            // 
            // lblReqDesc
            // 
            this.lblReqDesc.ForeColor = System.Drawing.Color.Red;
            this.lblReqDesc.Location = new System.Drawing.Point(177, 11);
            this.lblReqDesc.Name = "lblReqDesc";
            this.lblReqDesc.Size = new System.Drawing.Size(7, 10);
            this.lblReqDesc.TabIndex = 7;
            this.lblReqDesc.Text = "*";
            // 
            // lblReqPR
            // 
            this.lblReqPR.ForeColor = System.Drawing.Color.Red;
            this.lblReqPR.Location = new System.Drawing.Point(89, 77);
            this.lblReqPR.Name = "lblReqPR";
            this.lblReqPR.Size = new System.Drawing.Size(6, 10);
            this.lblReqPR.TabIndex = 7;
            this.lblReqPR.Text = "*";
            this.lblReqPR.Visible = false;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 109);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(526, 35);
            this.panel2.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(358, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(433, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(436, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlCode
            // 
            this.pnlCode.Controls.Add(this.lblReqDesc);
            this.pnlCode.Controls.Add(this.lblReqCode);
            this.pnlCode.Controls.Add(this.lblReqPR);
            this.pnlCode.Controls.Add(this.lblHeadstart);
            this.pnlCode.Controls.Add(this.lblFrom);
            this.pnlCode.Controls.Add(this.lblTo);
            this.pnlCode.Controls.Add(this.cmbRankCat);
            this.pnlCode.Controls.Add(this.txtFrom);
            this.pnlCode.Controls.Add(this.lblPointRng);
            this.pnlCode.Controls.Add(this.txtTo);
            this.pnlCode.Controls.Add(this.lblCode);
            this.pnlCode.Controls.Add(this.lbldesc);
            this.pnlCode.Controls.Add(this.txtCode);
            this.pnlCode.Controls.Add(this.txtDesc);
            this.pnlCode.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCode.Location = new System.Drawing.Point(0, 0);
            this.pnlCode.Name = "pnlCode";
            this.pnlCode.Size = new System.Drawing.Size(526, 109);
            this.pnlCode.TabIndex = 1;
            // 
            // lblReqCode
            // 
            this.lblReqCode.ForeColor = System.Drawing.Color.Red;
            this.lblReqCode.Location = new System.Drawing.Point(44, 11);
            this.lblReqCode.Name = "lblReqCode";
            this.lblReqCode.Size = new System.Drawing.Size(7, 10);
            this.lblReqCode.TabIndex = 7;
            this.lblReqCode.Text = "*";
            this.lblReqCode.Visible = false;
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(191, 75);
            this.txtFrom.MaxLength = 7;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.RightToLeft = Wisej.Web.RightToLeft.No;
            this.txtFrom.Size = new System.Drawing.Size(56, 25);
            this.txtFrom.TabIndex = 3;
            this.txtFrom.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtFrom.Visible = false;
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(289, 75);
            this.txtTo.MaxLength = 7;
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(56, 25);
            this.txtTo.TabIndex = 4;
            this.txtTo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtTo.Visible = false;
            this.txtTo.LostFocus += new System.EventHandler(this.txtTo_LostFocus);
            // 
            // txtCode
            // 
            this.txtCode.Cursor = Wisej.Web.Cursors.Arrow;
            this.txtCode.Location = new System.Drawing.Point(61, 11);
            this.txtCode.MaxLength = 2;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(23, 25);
            this.txtCode.TabIndex = 1;
            this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtCode.LostFocus += new System.EventHandler(this.txtCode_LostFocus);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(34, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "*";
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlCode);
            this.pnlCompleteForm.Controls.Add(this.panel2);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(526, 144);
            this.pnlCompleteForm.TabIndex = 3;
            // 
            // RankCategoriesForm
            // 
            this.ClientSize = new System.Drawing.Size(526, 144);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RankCategoriesForm";
            this.Text = "Rank Categories";
            componentTool1.ImageSource = "icon-help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.panel2.ResumeLayout(false);
            this.pnlCode.ResumeLayout(false);
            this.pnlCode.PerformLayout();
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private Label LblHeader;
        //private PictureBox pictureBox1;
        private Label lblCode;
        private Label lbldesc;
        private Label lblHeadstart;
        private TextBoxWithValidation txtCode;
        private TextBox txtDesc;
        private ComboBox cmbRankCat;
        private Label lblPointRng;
        private TextBoxWithValidation txtTo;
        private TextBoxWithValidation txtFrom;
        private Label lblTo;
        private Label lblFrom;
        private Label lblReqDesc;
        private Label lblReqPR;
        private Panel panel2;
        private Button btnSave;
        private Button btnCancel;
        private Panel pnlCode;
        private Label label1;
        private Label lblReqCode;
        private Spacer spacer1;
        private Panel pnlCompleteForm;
    }
}