namespace Captain.Common.Views.Forms
{
    partial class Remove_HS_Rollover
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

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Remove_HS_Rollover));
            this.pnlMain = new Wisej.Web.Panel();
            this.pnlDelete = new Wisej.Web.Panel();
            this.pnlTables = new Wisej.Web.Panel();
            this.btnDelete = new Wisej.Web.Button();
            this.panel2 = new Wisej.Web.Panel();
            this.chkbApp = new Wisej.Web.CheckBox();
            this.chkbMember = new Wisej.Web.CheckBox();
            this.chkbIncome = new Wisej.Web.CheckBox();
            this.chkbChild = new Wisej.Web.CheckBox();
            this.chkbEnrollment = new Wisej.Web.CheckBox();
            this.chkbCond = new Wisej.Web.CheckBox();
            this.chkbMailingAdd = new Wisej.Web.CheckBox();
            this.lblYear = new Wisej.Web.Label();
            this.chkbIncVer = new Wisej.Web.CheckBox();
            this.chldEmergncy = new Wisej.Web.CheckBox();
            this.chkbBio = new Wisej.Web.CheckBox();
            this.chkbSiteandRoom = new Wisej.Web.CheckBox();
            this.chkbCusQues = new Wisej.Web.CheckBox();
            this.pnlMain.SuspendLayout();
            this.pnlDelete.SuspendLayout();
            this.pnlTables.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlTables);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Controls.Add(this.pnlDelete);
            this.pnlMain.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(512, 301);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlDelete
            // 
            this.pnlDelete.AppearanceKey = "panel-grdo";
            this.pnlDelete.Controls.Add(this.btnDelete);
            this.pnlDelete.Dock = Wisej.Web.DockStyle.Bottom;
            this.pnlDelete.Location = new System.Drawing.Point(0, 266);
            this.pnlDelete.Name = "pnlDelete";
            this.pnlDelete.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.pnlDelete.Size = new System.Drawing.Size(512, 35);
            this.pnlDelete.TabIndex = 2;
            // 
            // pnlTables
            // 
            this.pnlTables.Controls.Add(this.chkbCusQues);
            this.pnlTables.Controls.Add(this.chkbSiteandRoom);
            this.pnlTables.Controls.Add(this.chkbBio);
            this.pnlTables.Controls.Add(this.chldEmergncy);
            this.pnlTables.Controls.Add(this.chkbIncVer);
            this.pnlTables.Controls.Add(this.chkbMailingAdd);
            this.pnlTables.Controls.Add(this.chkbCond);
            this.pnlTables.Controls.Add(this.chkbEnrollment);
            this.pnlTables.Controls.Add(this.chkbChild);
            this.pnlTables.Controls.Add(this.chkbIncome);
            this.pnlTables.Controls.Add(this.chkbMember);
            this.pnlTables.Controls.Add(this.chkbApp);
            this.pnlTables.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlTables.Location = new System.Drawing.Point(0, 18);
            this.pnlTables.Name = "pnlTables";
            this.pnlTables.Size = new System.Drawing.Size(512, 248);
            this.pnlTables.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.AppearanceKey = "button-error";
            this.btnDelete.Dock = Wisej.Web.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(422, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblYear);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(512, 18);
            this.panel2.TabIndex = 99;
            this.panel2.Visible = false;
            // 
            // chkbApp
            // 
            this.chkbApp.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbApp.Enabled = false;
            this.chkbApp.Location = new System.Drawing.Point(15, 10);
            this.chkbApp.Name = "chkbApp";
            this.chkbApp.Size = new System.Drawing.Size(151, 21);
            this.chkbApp.TabIndex = 1;
            this.chkbApp.Text = "Applicant (CASEMST)";
            // 
            // chkbMember
            // 
            this.chkbMember.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbMember.Enabled = false;
            this.chkbMember.Location = new System.Drawing.Point(15, 50);
            this.chkbMember.Name = "chkbMember";
            this.chkbMember.Size = new System.Drawing.Size(151, 21);
            this.chkbMember.TabIndex = 2;
            this.chkbMember.Text = "Members (CASESNP)";
            // 
            // chkbIncome
            // 
            this.chkbIncome.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbIncome.Enabled = false;
            this.chkbIncome.Location = new System.Drawing.Point(15, 90);
            this.chkbIncome.Name = "chkbIncome";
            this.chkbIncome.Size = new System.Drawing.Size(164, 21);
            this.chkbIncome.TabIndex = 3;
            this.chkbIncome.Text = "Income (CASEINCOME)";
            // 
            // chkbChild
            // 
            this.chkbChild.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbChild.Enabled = false;
            this.chkbChild.Location = new System.Drawing.Point(15, 130);
            this.chkbChild.Name = "chkbChild";
            this.chkbChild.Size = new System.Drawing.Size(98, 21);
            this.chkbChild.TabIndex = 4;
            this.chkbChild.Text = "(CHLDMST)";
            // 
            // chkbEnrollment
            // 
            this.chkbEnrollment.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbEnrollment.Enabled = false;
            this.chkbEnrollment.Location = new System.Drawing.Point(15, 170);
            this.chkbEnrollment.Name = "chkbEnrollment";
            this.chkbEnrollment.Size = new System.Drawing.Size(166, 21);
            this.chkbEnrollment.TabIndex = 5;
            this.chkbEnrollment.Text = "Enrollment (CASEENRL)";
            // 
            // chkbCond
            // 
            this.chkbCond.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbCond.Enabled = false;
            this.chkbCond.Location = new System.Drawing.Point(15, 210);
            this.chkbCond.Name = "chkbCond";
            this.chkbCond.Size = new System.Drawing.Size(109, 21);
            this.chkbCond.TabIndex = 6;
            this.chkbCond.Text = "(CASECOND)";
            // 
            // chkbMailingAdd
            // 
            this.chkbMailingAdd.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbMailingAdd.Enabled = false;
            this.chkbMailingAdd.Location = new System.Drawing.Point(258, 10);
            this.chkbMailingAdd.Name = "chkbMailingAdd";
            this.chkbMailingAdd.Size = new System.Drawing.Size(188, 21);
            this.chkbMailingAdd.TabIndex = 7;
            this.chkbMailingAdd.Text = "Mailing Address (CASEDIFF)";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(318, 1);
            this.lblYear.MinimumSize = new System.Drawing.Size(0, 16);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(30, 16);
            this.lblYear.TabIndex = 88;
            this.lblYear.Text = "Year";
            this.lblYear.Visible = false;
            // 
            // chkbIncVer
            // 
            this.chkbIncVer.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbIncVer.Enabled = false;
            this.chkbIncVer.Location = new System.Drawing.Point(258, 50);
            this.chkbIncVer.Name = "chkbIncVer";
            this.chkbIncVer.Size = new System.Drawing.Size(200, 21);
            this.chkbIncVer.TabIndex = 8;
            this.chkbIncVer.Text = "IncomeVerification (CASEVER)";
            // 
            // chldEmergncy
            // 
            this.chldEmergncy.CheckState = Wisej.Web.CheckState.Checked;
            this.chldEmergncy.Enabled = false;
            this.chldEmergncy.Location = new System.Drawing.Point(258, 130);
            this.chldEmergncy.Name = "chldEmergncy";
            this.chldEmergncy.Size = new System.Drawing.Size(172, 21);
            this.chldEmergncy.TabIndex = 10;
            this.chldEmergncy.Text = "Emergency (CHLDEMER)";
            // 
            // chkbBio
            // 
            this.chkbBio.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbBio.Enabled = false;
            this.chkbBio.Location = new System.Drawing.Point(258, 170);
            this.chkbBio.Name = "chkbBio";
            this.chkbBio.Size = new System.Drawing.Size(94, 21);
            this.chkbBio.TabIndex = 11;
            this.chkbBio.Text = "(CASEBIO)";
            // 
            // chkbSiteandRoom
            // 
            this.chkbSiteandRoom.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbSiteandRoom.Enabled = false;
            this.chkbSiteandRoom.Location = new System.Drawing.Point(258, 210);
            this.chkbSiteandRoom.Name = "chkbSiteandRoom";
            this.chkbSiteandRoom.Size = new System.Drawing.Size(162, 21);
            this.chkbSiteandRoom.TabIndex = 12;
            this.chkbSiteandRoom.Text = "Site & Room (CASESITE)";
            // 
            // chkbCusQues
            // 
            this.chkbCusQues.CheckState = Wisej.Web.CheckState.Checked;
            this.chkbCusQues.Enabled = false;
            this.chkbCusQues.Location = new System.Drawing.Point(258, 90);
            this.chkbCusQues.Name = "chkbCusQues";
            this.chkbCusQues.Size = new System.Drawing.Size(200, 21);
            this.chkbCusQues.TabIndex = 9;
            this.chkbCusQues.Text = "Custom Questions (ADDCUST)";
            // 
            // DeleteRollover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 301);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteRollover";
            this.StartPosition = Wisej.Web.FormStartPosition.CenterScreen;
            this.Text = "Remove HS Rollover";
            this.pnlMain.ResumeLayout(false);
            this.pnlDelete.ResumeLayout(false);
            this.pnlTables.ResumeLayout(false);
            this.pnlTables.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel pnlMain;
        private Wisej.Web.Panel pnlTables;
        private Wisej.Web.Panel pnlDelete;
        private Wisej.Web.Button btnDelete;
        private Wisej.Web.Panel panel2;
        private Wisej.Web.CheckBox chkbMailingAdd;
        private Wisej.Web.CheckBox chkbCond;
        private Wisej.Web.CheckBox chkbEnrollment;
        private Wisej.Web.CheckBox chkbChild;
        private Wisej.Web.CheckBox chkbIncome;
        private Wisej.Web.CheckBox chkbMember;
        private Wisej.Web.CheckBox chkbApp;
        private Wisej.Web.Label lblYear;
        private Wisej.Web.CheckBox chkbIncVer;
        private Wisej.Web.CheckBox chkbCusQues;
        private Wisej.Web.CheckBox chkbSiteandRoom;
        private Wisej.Web.CheckBox chkbBio;
        private Wisej.Web.CheckBox chldEmergncy;
    }
}