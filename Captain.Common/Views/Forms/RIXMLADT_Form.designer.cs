using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class RIXMLADT_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RIXMLADT_Form));
            this.dtToDate = new Wisej.Web.DateTimePicker();
            this.lblTo = new Wisej.Web.Label();
            this.lblFrom = new Wisej.Web.Label();
            this.dtpFromDate = new Wisej.Web.DateTimePicker();
            this.pnlXml = new Wisej.Web.Panel();
            this.rbTrigger = new Wisej.Web.RadioButton();
            this.rbRIAXML = new Wisej.Web.RadioButton();
            this.lblType = new Wisej.Web.Label();
            this.btnGenrate = new Wisej.Web.Button();
            this.panel1 = new Wisej.Web.Panel();
            this.btnPreview = new Wisej.Web.Button();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlXml.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtToDate
            // 
            this.dtToDate.AutoSize = false;
            this.dtToDate.CustomFormat = "MM/dd/yyyy";
            this.dtToDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtToDate.Location = new System.Drawing.Point(269, 11);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.ShowCheckBox = true;
            this.dtToDate.ShowToolTips = false;
            this.dtToDate.Size = new System.Drawing.Size(116, 25);
            this.dtToDate.TabIndex = 2;
            // 
            // lblTo
            // 
            this.lblTo.AllowDrop = true;
            this.lblTo.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.lblTo.Location = new System.Drawing.Point(240, 16);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(15, 16);
            this.lblTo.TabIndex = 0;
            this.lblTo.Text = "To";
            // 
            // lblFrom
            // 
            this.lblFrom.AllowDrop = true;
            this.lblFrom.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.lblFrom.Location = new System.Drawing.Point(50, 15);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(31, 16);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AutoSize = false;
            this.dtpFromDate.CustomFormat = "MM/dd/yyyy";
            this.dtpFromDate.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(97, 11);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ShowCheckBox = true;
            this.dtpFromDate.ShowToolTips = false;
            this.dtpFromDate.Size = new System.Drawing.Size(116, 25);
            this.dtpFromDate.TabIndex = 1;
            // 
            // pnlXml
            // 
            this.pnlXml.Controls.Add(this.dtToDate);
            this.pnlXml.Controls.Add(this.lblTo);
            this.pnlXml.Controls.Add(this.lblFrom);
            this.pnlXml.Controls.Add(this.dtpFromDate);
            this.pnlXml.Dock = Wisej.Web.DockStyle.Top;
            this.pnlXml.Location = new System.Drawing.Point(0, 0);
            this.pnlXml.Name = "pnlXml";
            this.pnlXml.Size = new System.Drawing.Size(479, 41);
            this.pnlXml.TabIndex = 1;
            // 
            // rbTrigger
            // 
            this.rbTrigger.AutoSize = false;
            this.rbTrigger.Checked = true;
            this.rbTrigger.Location = new System.Drawing.Point(92, 4);
            this.rbTrigger.Name = "rbTrigger";
            this.rbTrigger.Size = new System.Drawing.Size(68, 21);
            this.rbTrigger.TabIndex = 1;
            this.rbTrigger.TabStop = true;
            this.rbTrigger.Text = "Trigger";
            // 
            // rbRIAXML
            // 
            this.rbRIAXML.AutoSize = false;
            this.rbRIAXML.Location = new System.Drawing.Point(171, 4);
            this.rbRIAXML.Name = "rbRIAXML";
            this.rbRIAXML.Size = new System.Drawing.Size(78, 21);
            this.rbRIAXML.TabIndex = 2;
            this.rbRIAXML.Text = "RIAAXML";
            // 
            // lblType
            // 
            this.lblType.AllowDrop = true;
            this.lblType.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.lblType.Location = new System.Drawing.Point(50, 5);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(28, 16);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type";
            // 
            // btnGenrate
            // 
            this.btnGenrate.AppearanceKey = "button-reports";
            this.btnGenrate.Dock = Wisej.Web.DockStyle.Right;
            this.btnGenrate.Location = new System.Drawing.Point(291, 5);
            this.btnGenrate.Name = "btnGenrate";
            this.btnGenrate.Size = new System.Drawing.Size(85, 25);
            this.btnGenrate.TabIndex = 1;
            this.btnGenrate.Text = "&Genrate";
            this.btnGenrate.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblType);
            this.panel1.Controls.Add(this.rbTrigger);
            this.panel1.Controls.Add(this.rbRIAXML);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(479, 65);
            this.panel1.TabIndex = 2;
            // 
            // btnPreview
            // 
            this.btnPreview.AppearanceKey = "button-reports";
            this.btnPreview.Dock = Wisej.Web.DockStyle.Right;
            this.btnPreview.Location = new System.Drawing.Point(379, 5);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(85, 25);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "Pre&view";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.panel2);
            this.pnlCompleteForm.Controls.Add(this.panel1);
            this.pnlCompleteForm.Controls.Add(this.pnlXml);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(479, 106);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.btnGenrate);
            this.panel2.Controls.Add(this.spacer1);
            this.panel2.Controls.Add(this.btnPreview);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 71);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel2.Size = new System.Drawing.Size(479, 35);
            this.panel2.TabIndex = 3;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(376, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // RIXMLADT_Form
            // 
            this.ClientSize = new System.Drawing.Size(479, 106);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RIXMLADT_Form";
            this.Text = "RIXMLADT_Form";
            this.pnlXml.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private DateTimePicker dtToDate;
        private Label lblTo;
        private Label lblFrom;
        private DateTimePicker dtpFromDate;
        private Panel pnlXml;
        private RadioButton rbTrigger;
        private RadioButton rbRIAXML;
        private Label lblType;
        private Button btnGenrate;
        private Panel panel1;
        private Button btnPreview;
        private Panel pnlCompleteForm;
        private Panel panel2;
        private Spacer spacer1;
    }
}