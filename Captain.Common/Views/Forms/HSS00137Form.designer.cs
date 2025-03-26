using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class HSS00137Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSS00137Form));
            this.pnlConpleteForm = new Wisej.Web.Panel();
            this.lblOtherReq = new Wisej.Web.Label();
            this.pnlsSave = new Wisej.Web.Panel();
            this.btnOk = new Wisej.Web.Button();
            this.btnCancel = new Wisej.Web.Button();
            this.chkBillFormeals = new Wisej.Web.CheckBox();
            this.lblOther = new Wisej.Web.Label();
            this.txtOther = new Wisej.Web.TextBox();
            this.rdo1 = new Wisej.Web.RadioButton();
            this.rdo2 = new Wisej.Web.RadioButton();
            this.rdo3 = new Wisej.Web.RadioButton();
            this.rdoFull = new Wisej.Web.RadioButton();
            this.pnlDetails = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlConpleteForm.SuspendLayout();
            this.pnlsSave.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConpleteForm
            // 
            this.pnlConpleteForm.Controls.Add(this.pnlDetails);
            this.pnlConpleteForm.Controls.Add(this.pnlsSave);
            this.pnlConpleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlConpleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlConpleteForm.Name = "pnlConpleteForm";
            this.pnlConpleteForm.Size = new System.Drawing.Size(311, 139);
            this.pnlConpleteForm.TabIndex = 0;
            // 
            // lblOtherReq
            // 
            this.lblOtherReq.ForeColor = System.Drawing.Color.Red;
            this.lblOtherReq.Location = new System.Drawing.Point(48, 10);
            this.lblOtherReq.Name = "lblOtherReq";
            this.lblOtherReq.Size = new System.Drawing.Size(8, 10);
            this.lblOtherReq.TabIndex = 28;
            this.lblOtherReq.Text = "*";
            this.lblOtherReq.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblOtherReq.Visible = false;
            // 
            // pnlsSave
            // 
            this.pnlsSave.AppearanceKey = "panel-grdo";
            this.pnlsSave.Controls.Add(this.btnOk);
            this.pnlsSave.Controls.Add(this.spacer1);
            this.pnlsSave.Controls.Add(this.btnCancel);
            this.pnlsSave.Controls.Add(this.chkBillFormeals);
            this.pnlsSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlsSave.Location = new System.Drawing.Point(0, 104);
            this.pnlsSave.Name = "pnlsSave";
            this.pnlsSave.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlsSave.Size = new System.Drawing.Size(311, 35);
            this.pnlsSave.TabIndex = 10;
            // 
            // btnOk
            // 
            this.btnOk.AppearanceKey = "button-ok";
            this.btnOk.Dock = Wisej.Web.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(183, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(55, 25);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "&Save";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(241, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkBillFormeals
            // 
            this.chkBillFormeals.Dock = Wisej.Web.DockStyle.Left;
            this.chkBillFormeals.Location = new System.Drawing.Point(15, 5);
            this.chkBillFormeals.Name = "chkBillFormeals";
            this.chkBillFormeals.Size = new System.Drawing.Size(101, 25);
            this.chkBillFormeals.TabIndex = 0;
            this.chkBillFormeals.Text = "Bill for Meals";
            this.chkBillFormeals.Visible = false;
            // 
            // lblOther
            // 
            this.lblOther.AutoSize = true;
            this.lblOther.Location = new System.Drawing.Point(15, 12);
            this.lblOther.Name = "lblOther";
            this.lblOther.Size = new System.Drawing.Size(35, 14);
            this.lblOther.TabIndex = 6;
            this.lblOther.Text = "Other";
            // 
            // txtOther
            // 
            this.txtOther.Location = new System.Drawing.Point(65, 8);
            this.txtOther.MaxLength = 50;
            this.txtOther.Name = "txtOther";
            this.txtOther.ReadOnly = true;
            this.txtOther.Size = new System.Drawing.Size(180, 25);
            this.txtOther.TabIndex = 1;
            // 
            // rdo1
            // 
            this.rdo1.Location = new System.Drawing.Point(179, 76);
            this.rdo1.Name = "rdo1";
            this.rdo1.Size = new System.Drawing.Size(66, 21);
            this.rdo1.TabIndex = 5;
            this.rdo1.Text = "1 - 1/4";
            // 
            // rdo2
            // 
            this.rdo2.Location = new System.Drawing.Point(65, 76);
            this.rdo2.Name = "rdo2";
            this.rdo2.Size = new System.Drawing.Size(66, 21);
            this.rdo2.TabIndex = 4;
            this.rdo2.Text = "2 - 1/2";
            // 
            // rdo3
            // 
            this.rdo3.Location = new System.Drawing.Point(179, 42);
            this.rdo3.Name = "rdo3";
            this.rdo3.Size = new System.Drawing.Size(66, 21);
            this.rdo3.TabIndex = 3;
            this.rdo3.Text = "3 - 3/4";
            // 
            // rdoFull
            // 
            this.rdoFull.Location = new System.Drawing.Point(65, 42);
            this.rdoFull.Name = "rdoFull";
            this.rdoFull.Size = new System.Drawing.Size(52, 21);
            this.rdoFull.TabIndex = 2;
            this.rdoFull.Text = "Full";
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.rdo1);
            this.pnlDetails.Controls.Add(this.lblOtherReq);
            this.pnlDetails.Controls.Add(this.rdoFull);
            this.pnlDetails.Controls.Add(this.rdo3);
            this.pnlDetails.Controls.Add(this.lblOther);
            this.pnlDetails.Controls.Add(this.rdo2);
            this.pnlDetails.Controls.Add(this.txtOther);
            this.pnlDetails.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(311, 104);
            this.pnlDetails.TabIndex = 29;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(238, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // HSS00137Form
            // 
            this.ClientSize = new System.Drawing.Size(311, 139);
            this.Controls.Add(this.pnlConpleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSS00137Form";
            this.Text = "HSS00137Form";
            this.pnlConpleteForm.ResumeLayout(false);
            this.pnlsSave.ResumeLayout(false);
            this.pnlsSave.PerformLayout();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnlConpleteForm;
        private Label lblOther;
        private TextBox txtOther;
        private RadioButton rdo1;
        private RadioButton rdo2;
        private RadioButton rdo3;
        private RadioButton rdoFull;
        private CheckBox chkBillFormeals;
        private Panel pnlsSave;
        private Button btnOk;
        private Button btnCancel;
        private Label lblOtherReq;
        private Panel pnlDetails;
        private Spacer spacer1;
    }
}