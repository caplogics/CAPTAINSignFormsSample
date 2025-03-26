using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class STFBLK10Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STFBLK10Form));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.lblDateComple = new Wisej.Web.Label();
            this.lblProvider = new Wisej.Web.Label();
            this.lblType1Error = new Wisej.Web.Label();
            this.txtCoursetitle = new Wisej.Web.TextBox();
            this.lblHeader1Error = new Wisej.Web.Label();
            this.lblCourseTitle = new Wisej.Web.Label();
            this.pnlParams = new Wisej.Web.Panel();
            this.label8 = new Wisej.Web.Label();
            this.label7 = new Wisej.Web.Label();
            this.label6 = new Wisej.Web.Label();
            this.txtCost = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label5 = new Wisej.Web.Label();
            this.txtSponsor = new Wisej.Web.TextBox();
            this.lblSponsor = new Wisej.Web.Label();
            this.txtPresenter = new Wisej.Web.TextBox();
            this.lblPresenter = new Wisej.Web.Label();
            this.txtLocation = new Wisej.Web.TextBox();
            this.lblLocation = new Wisej.Web.Label();
            this.btnCopy = new Wisej.Web.Button();
            this.btnClear = new Wisej.Web.Button();
            this.txtCollegeCredites = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.lblSuffix = new Wisej.Web.Label();
            this.label28 = new Wisej.Web.Label();
            this.txtCeucreduts = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.txtClockHours = new Captain.Common.Views.Controls.Compatibility.TextBoxWithValidation();
            this.label30 = new Wisej.Web.Label();
            this.dtCompleted = new Wisej.Web.DateTimePicker();
            this.label1 = new Wisej.Web.Label();
            this.cmbProvider = new Captain.Common.Views.Controls.Compatibility.ComboBoxEx();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.pnlSave = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlCompleteForm = new Wisej.Web.Panel();
            this.pnlParams.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlCompleteForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDateComple
            // 
            this.lblDateComple.Location = new System.Drawing.Point(15, 77);
            this.lblDateComple.Name = "lblDateComple";
            this.lblDateComple.Size = new System.Drawing.Size(94, 16);
            this.lblDateComple.TabIndex = 0;
            this.lblDateComple.Text = "Date Completed";
            // 
            // lblProvider
            // 
            this.lblProvider.Location = new System.Drawing.Point(15, 15);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(50, 16);
            this.lblProvider.TabIndex = 0;
            this.lblProvider.Text = "Provider";
            // 
            // lblType1Error
            // 
            this.lblType1Error.AutoSize = true;
            this.lblType1Error.ForeColor = System.Drawing.Color.Red;
            this.lblType1Error.Location = new System.Drawing.Point(65, 13);
            this.lblType1Error.Name = "lblType1Error";
            this.lblType1Error.Size = new System.Drawing.Size(9, 14);
            this.lblType1Error.TabIndex = 1;
            this.lblType1Error.Text = "*";
            // 
            // txtCoursetitle
            // 
            this.txtCoursetitle.Location = new System.Drawing.Point(130, 42);
            this.txtCoursetitle.MaxLength = 100;
            this.txtCoursetitle.Name = "txtCoursetitle";
            this.txtCoursetitle.Size = new System.Drawing.Size(435, 25);
            this.txtCoursetitle.TabIndex = 2;
            this.txtCoursetitle.Leave += new System.EventHandler(this.txtHeader_Leave);
            // 
            // lblHeader1Error
            // 
            this.lblHeader1Error.AutoSize = true;
            this.lblHeader1Error.ForeColor = System.Drawing.Color.Red;
            this.lblHeader1Error.Location = new System.Drawing.Point(83, 44);
            this.lblHeader1Error.Name = "lblHeader1Error";
            this.lblHeader1Error.Size = new System.Drawing.Size(9, 14);
            this.lblHeader1Error.TabIndex = 1;
            this.lblHeader1Error.Text = "*";
            // 
            // lblCourseTitle
            // 
            this.lblCourseTitle.Location = new System.Drawing.Point(15, 46);
            this.lblCourseTitle.Name = "lblCourseTitle";
            this.lblCourseTitle.Size = new System.Drawing.Size(69, 16);
            this.lblCourseTitle.TabIndex = 0;
            this.lblCourseTitle.Text = "Course Title";
            // 
            // pnlParams
            // 
            this.pnlParams.Controls.Add(this.label8);
            this.pnlParams.Controls.Add(this.label7);
            this.pnlParams.Controls.Add(this.label6);
            this.pnlParams.Controls.Add(this.txtCost);
            this.pnlParams.Controls.Add(this.label5);
            this.pnlParams.Controls.Add(this.txtSponsor);
            this.pnlParams.Controls.Add(this.lblSponsor);
            this.pnlParams.Controls.Add(this.txtPresenter);
            this.pnlParams.Controls.Add(this.lblPresenter);
            this.pnlParams.Controls.Add(this.txtLocation);
            this.pnlParams.Controls.Add(this.lblLocation);
            this.pnlParams.Controls.Add(this.btnCopy);
            this.pnlParams.Controls.Add(this.btnClear);
            this.pnlParams.Controls.Add(this.txtCollegeCredites);
            this.pnlParams.Controls.Add(this.lblSuffix);
            this.pnlParams.Controls.Add(this.label28);
            this.pnlParams.Controls.Add(this.txtCeucreduts);
            this.pnlParams.Controls.Add(this.txtClockHours);
            this.pnlParams.Controls.Add(this.label30);
            this.pnlParams.Controls.Add(this.dtCompleted);
            this.pnlParams.Controls.Add(this.label1);
            this.pnlParams.Controls.Add(this.lblDateComple);
            this.pnlParams.Controls.Add(this.cmbProvider);
            this.pnlParams.Controls.Add(this.lblProvider);
            this.pnlParams.Controls.Add(this.lblType1Error);
            this.pnlParams.Controls.Add(this.txtCoursetitle);
            this.pnlParams.Controls.Add(this.lblHeader1Error);
            this.pnlParams.Controls.Add(this.lblCourseTitle);
            this.pnlParams.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlParams.Location = new System.Drawing.Point(0, 0);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new System.Drawing.Size(597, 201);
            this.pnlParams.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(368, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(9, 14);
            this.label8.TabIndex = 1;
            this.label8.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(72, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 14);
            this.label7.TabIndex = 1;
            this.label7.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(64, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(9, 14);
            this.label6.TabIndex = 1;
            this.label6.Text = "*";
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(390, 166);
            this.txtCost.MaxLength = 5;
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(102, 25);
            this.txtCost.TabIndex = 11;
            this.txtCost.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(321, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Total Cost";
            // 
            // txtSponsor
            // 
            this.txtSponsor.Location = new System.Drawing.Point(390, 135);
            this.txtSponsor.MaxLength = 30;
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(175, 25);
            this.txtSponsor.TabIndex = 9;
            // 
            // lblSponsor
            // 
            this.lblSponsor.Location = new System.Drawing.Point(321, 139);
            this.lblSponsor.Name = "lblSponsor";
            this.lblSponsor.Size = new System.Drawing.Size(49, 16);
            this.lblSponsor.TabIndex = 4;
            this.lblSponsor.Text = "Sponsor";
            // 
            // txtPresenter
            // 
            this.txtPresenter.Location = new System.Drawing.Point(129, 166);
            this.txtPresenter.MaxLength = 30;
            this.txtPresenter.Name = "txtPresenter";
            this.txtPresenter.Size = new System.Drawing.Size(166, 25);
            this.txtPresenter.TabIndex = 10;
            // 
            // lblPresenter
            // 
            this.lblPresenter.Location = new System.Drawing.Point(15, 170);
            this.lblPresenter.Name = "lblPresenter";
            this.lblPresenter.Size = new System.Drawing.Size(57, 16);
            this.lblPresenter.TabIndex = 4;
            this.lblPresenter.Text = "Presenter";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(130, 135);
            this.txtLocation.MaxLength = 30;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(166, 25);
            this.txtLocation.TabIndex = 8;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(15, 139);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(49, 16);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "Location";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(506, 73);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(60, 25);
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "C&opy";
            this.btnCopy.Visible = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(505, 166);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(60, 25);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clea&r";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtCollegeCredites
            // 
            this.txtCollegeCredites.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCollegeCredites.Location = new System.Drawing.Point(420, 73);
            this.txtCollegeCredites.MaxLength = 5;
            this.txtCollegeCredites.Name = "txtCollegeCredites";
            this.txtCollegeCredites.Size = new System.Drawing.Size(61, 25);
            this.txtCollegeCredites.TabIndex = 4;
            this.txtCollegeCredites.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtCollegeCredites.Leave += new System.EventHandler(this.txtCollegeCredites_Leave);
            // 
            // lblSuffix
            // 
            this.lblSuffix.Location = new System.Drawing.Point(321, 77);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(86, 16);
            this.lblSuffix.TabIndex = 4;
            this.lblSuffix.Text = "College Credits";
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(15, 108);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(69, 16);
            this.label28.TabIndex = 4;
            this.label28.Text = "CEU Credits";
            // 
            // txtCeucreduts
            // 
            this.txtCeucreduts.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtCeucreduts.Location = new System.Drawing.Point(129, 104);
            this.txtCeucreduts.MaxLength = 5;
            this.txtCeucreduts.Name = "txtCeucreduts";
            this.txtCeucreduts.Size = new System.Drawing.Size(102, 25);
            this.txtCeucreduts.TabIndex = 6;
            this.txtCeucreduts.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtCeucreduts.Leave += new System.EventHandler(this.txtCeucreduts_Leave);
            // 
            // txtClockHours
            // 
            this.txtClockHours.CharacterCasing = Wisej.Web.CharacterCasing.Upper;
            this.txtClockHours.Location = new System.Drawing.Point(420, 104);
            this.txtClockHours.MaxLength = 6;
            this.txtClockHours.Name = "txtClockHours";
            this.txtClockHours.Size = new System.Drawing.Size(61, 25);
            this.txtClockHours.TabIndex = 7;
            this.txtClockHours.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtClockHours.Leave += new System.EventHandler(this.txtClockHours_Leave);
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(321, 108);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(70, 16);
            this.label30.TabIndex = 4;
            this.label30.Text = "Clock Hours";
            // 
            // dtCompleted
            // 
            this.dtCompleted.AutoSize = false;
            this.dtCompleted.CustomFormat = "MM/dd/yyyy";
            this.dtCompleted.Format = Wisej.Web.DateTimePickerFormat.Custom;
            this.dtCompleted.Location = new System.Drawing.Point(130, 73);
            this.dtCompleted.Name = "dtCompleted";
            this.dtCompleted.ShowCheckBox = true;
            this.dtCompleted.ShowToolTips = false;
            this.dtCompleted.Size = new System.Drawing.Size(116, 25);
            this.dtCompleted.TabIndex = 3;
            this.dtCompleted.ValueChanged += new System.EventHandler(this.dtCompleted_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(108, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "*";
            // 
            // cmbProvider
            // 
            this.cmbProvider.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbProvider.FormattingEnabled = true;
            this.cmbProvider.Location = new System.Drawing.Point(130, 11);
            this.cmbProvider.Name = "cmbProvider";
            this.cmbProvider.Size = new System.Drawing.Size(166, 25);
            this.cmbProvider.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-error";
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(507, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(429, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlSave
            // 
            this.pnlSave.AppearanceKey = "panel-grdo";
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.spacer1);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlSave.Location = new System.Drawing.Point(0, 201);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Padding = new Wisej.Web.Padding(15, 5, 15, 5);
            this.pnlSave.Size = new System.Drawing.Size(597, 35);
            this.pnlSave.TabIndex = 2;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(504, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(3, 25);
            // 
            // pnlCompleteForm
            // 
            this.pnlCompleteForm.Controls.Add(this.pnlParams);
            this.pnlCompleteForm.Controls.Add(this.pnlSave);
            this.pnlCompleteForm.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCompleteForm.Location = new System.Drawing.Point(0, 0);
            this.pnlCompleteForm.Name = "pnlCompleteForm";
            this.pnlCompleteForm.Size = new System.Drawing.Size(597, 236);
            this.pnlCompleteForm.TabIndex = 0;
            // 
            // STFBLK10Form
            // 
            this.ClientSize = new System.Drawing.Size(597, 236);
            this.Controls.Add(this.pnlCompleteForm);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "STFBLK10Form";
            this.Text = "Post Course Details Template";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "tlHelp";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.ToolClick += new Wisej.Web.ToolClickEventHandler(this.STFBLK10Form_ToolClick);
            this.pnlParams.ResumeLayout(false);
            this.pnlParams.PerformLayout();
            this.pnlSave.ResumeLayout(false);
            this.pnlCompleteForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblDateComple;
        private ComboBoxEx cmbProvider;
        private Label lblProvider;
        private Label lblType1Error;
        private TextBox txtCoursetitle;
        private Label lblHeader1Error;
        private Label lblCourseTitle;
        private Panel pnlParams;
        private Label label1;
        private DateTimePicker dtCompleted;
        private Label lblSuffix;
        private Label label28;
        private Label label30;
        private Button btnCancel;
        private Button btnSave;
        private Panel pnlSave;
        private Button btnCopy;
        private Button btnClear;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private TextBox txtSponsor;
        private Label lblSponsor;
        private TextBox txtPresenter;
        private Label lblPresenter;
        private TextBox txtLocation;
        private Label lblLocation;
        private TextBoxWithValidation txtCollegeCredites;
        private TextBoxWithValidation txtCeucreduts;
        private TextBoxWithValidation txtClockHours;
        private TextBoxWithValidation txtCost;
        private Panel pnlCompleteForm;
        private Spacer spacer1;
    }
}