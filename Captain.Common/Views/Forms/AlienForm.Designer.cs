using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class AlienForm
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

        #region Wisej Designer generated code

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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle15 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle16 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlienForm));
            this.panel1 = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.gvAlienUsers = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.colAgy = new Wisej.Web.DataGridViewColumn();
            this.colDept = new Wisej.Web.DataGridViewColumn();
            this.ColProg = new Wisej.Web.DataGridViewColumn();
            this.ColYear = new Wisej.Web.DataGridViewColumn();
            this.ColAppno = new Wisej.Web.DataGridViewColumn();
            this.ColFamSeq = new Wisej.Web.DataGridViewColumn();
            this.ColName = new Wisej.Web.DataGridViewColumn();
            this.Column0 = new Wisej.Web.DataGridViewColumn();
            this.Column1 = new Wisej.Web.DataGridViewColumn();
            this.ColReason = new Wisej.Web.DataGridViewColumn();
            this.ColDOB = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.ColAge = new Wisej.Web.DataGridViewColumn();
            this.ColCitzen = new Wisej.Web.DataGridViewTextBoxColumn();
            this.Colidentify = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel2 = new Wisej.Web.Panel();
            this.lblEmail = new Wisej.Web.Label();
            this.lblmail = new Wisej.Web.Label();
            this.btnPrint = new Wisej.Web.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAlienUsers)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1195, 436);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5);
            this.panel3.Size = new System.Drawing.Size(1195, 399);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gvAlienUsers);
            this.panel4.CssStyle = "border:1px solid #efefef; border-radius:15px;";
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1185, 389);
            this.panel4.TabIndex = 0;
            // 
            // gvAlienUsers
            // 
            this.gvAlienUsers.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.colAgy,
            this.colDept,
            this.ColProg,
            this.ColYear,
            this.ColAppno,
            this.ColFamSeq,
            this.ColName,
            this.Column0,
            this.Column1,
            this.ColReason,
            this.ColDOB,
            this.ColAge,
            this.ColCitzen,
            this.Colidentify});
            this.gvAlienUsers.Dock = Wisej.Web.DockStyle.Fill;
            this.gvAlienUsers.Name = "gvAlienUsers";
            this.gvAlienUsers.RowHeadersWidth = 25;
            this.gvAlienUsers.ShowColumnVisibilityMenu = false;
            this.gvAlienUsers.Size = new System.Drawing.Size(1185, 389);
            this.gvAlienUsers.TabIndex = 0;
            // 
            // colAgy
            // 
            this.colAgy.HeaderText = "Agy";
            this.colAgy.Name = "colAgy";
            this.colAgy.Visible = false;
            // 
            // colDept
            // 
            this.colDept.HeaderText = "Dept";
            this.colDept.Name = "colDept";
            this.colDept.Visible = false;
            // 
            // ColProg
            // 
            this.ColProg.HeaderText = "Prog";
            this.ColProg.Name = "ColProg";
            this.ColProg.Visible = false;
            // 
            // ColYear
            // 
            this.ColYear.HeaderText = "Year";
            this.ColYear.Name = "ColYear";
            this.ColYear.Visible = false;
            // 
            // ColAppno
            // 
            this.ColAppno.HeaderText = "Appno";
            this.ColAppno.Name = "ColAppno";
            this.ColAppno.Visible = false;
            // 
            // ColFamSeq
            // 
            this.ColFamSeq.HeaderText = "FamSeq";
            this.ColFamSeq.Name = "ColFamSeq";
            this.ColFamSeq.Visible = false;
            // 
            // ColName
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.ColName.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColName.HeaderStyle = dataGridViewCellStyle2;
            this.ColName.HeaderText = "Name";
            this.ColName.Name = "ColName";
            this.ColName.Width = 160;
            // 
            // Column0
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.HeaderStyle = dataGridViewCellStyle4;
            this.Column0.HeaderText = "SS#";
            this.Column0.Name = "Column0";
            this.Column0.Width = 90;
            // 
            // Column1
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.HeaderStyle = dataGridViewCellStyle6;
            this.Column1.HeaderText = "SS# Reason";
            this.Column1.Name = "Column1";
            this.Column1.Width = 160;
            // 
            // ColReason
            // 
            dataGridViewCellStyle7.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColReason.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColReason.HeaderStyle = dataGridViewCellStyle8;
            this.ColReason.HeaderText = "Resident";
            this.ColReason.Name = "ColReason";
            this.ColReason.Width = 140;
            // 
            // ColDOB
            // 
            dataGridViewCellStyle9.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColDOB.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColDOB.HeaderStyle = dataGridViewCellStyle10;
            this.ColDOB.HeaderText = "Birth Date";
            this.ColDOB.Name = "ColDOB";
            this.ColDOB.Width = 80;
            // 
            // ColAge
            // 
            dataGridViewCellStyle11.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColAge.DefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.ColAge.HeaderStyle = dataGridViewCellStyle12;
            this.ColAge.HeaderText = "Age";
            this.ColAge.Name = "ColAge";
            this.ColAge.Width = 60;
            // 
            // ColCitzen
            // 
            this.ColCitzen.AllowHtml = true;
            dataGridViewCellStyle13.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.CssStyle = "border:1px solid #ccc; border-radius:15px;";
            this.ColCitzen.DefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.CssStyle = "";
            this.ColCitzen.HeaderStyle = dataGridViewCellStyle14;
            this.ColCitzen.HeaderText = "Citizenship/Qualified Alien";
            this.ColCitzen.Name = "ColCitzen";
            this.ColCitzen.Width = 220;
            // 
            // Colidentify
            // 
            dataGridViewCellStyle15.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.CssStyle = "border:1px solid #ccc; border-radius:15px;";
            this.Colidentify.DefaultCellStyle = dataGridViewCellStyle15;
            dataGridViewCellStyle16.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Colidentify.HeaderStyle = dataGridViewCellStyle16;
            this.Colidentify.HeaderText = "Identification";
            this.Colidentify.Name = "Colidentify";
            this.Colidentify.Width = 180;
            // 
            // panel2
            // 
            this.panel2.AppearanceKey = "panel-grdo";
            this.panel2.Controls.Add(this.lblEmail);
            this.panel2.Controls.Add(this.lblmail);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 399);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(5);
            this.panel2.Size = new System.Drawing.Size(1195, 37);
            this.panel2.TabIndex = 0;
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEmail.Location = new System.Drawing.Point(35, 8);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(418, 19);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblmail
            // 
            this.lblmail.AutoSize = true;
            this.lblmail.BackColor = System.Drawing.Color.Transparent;
            this.lblmail.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblmail.Location = new System.Drawing.Point(22, 8);
            this.lblmail.MaximumSize = new System.Drawing.Size(100, 50);
            this.lblmail.MinimumSize = new System.Drawing.Size(0, 19);
            this.lblmail.Name = "lblmail";
            this.lblmail.Size = new System.Drawing.Size(4, 19);
            this.lblmail.TabIndex = 2;
            this.lblmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = Wisej.Web.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(1104, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(86, 27);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // AlienForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 436);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlienForm";
            this.Text = "Alien Form";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAlienUsers)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel panel1;
        private Wisej.Web.Panel panel3;
        private Wisej.Web.Panel panel2;
        private Wisej.Web.Panel panel4;
        private DataGridViewEx gvAlienUsers;
        private Wisej.Web.DataGridViewColumn ColName;
        private Wisej.Web.DataGridViewColumn ColAge;
        private Wisej.Web.DataGridViewColumn ColReason;
        private Wisej.Web.Button btnPrint;
        private Wisej.Web.DataGridViewTextBoxColumn ColCitzen;
        private Wisej.Web.DataGridViewTextBoxColumn Colidentify;
        private Wisej.Web.DataGridViewColumn colAgy;
        private Wisej.Web.DataGridViewColumn colDept;
        private Wisej.Web.DataGridViewColumn ColProg;
        private Wisej.Web.DataGridViewColumn ColYear;
        private Wisej.Web.DataGridViewColumn ColAppno;
        private Wisej.Web.DataGridViewColumn ColFamSeq;
        private Wisej.Web.Label lblEmail;
        private Wisej.Web.Label lblmail;
        private Controls.Compatibility.DataGridViewDateTimeColumn ColDOB;
        private Wisej.Web.DataGridViewColumn Colssn;
        private Wisej.Web.DataGridViewColumn colssnReason;
        private Wisej.Web.DataGridViewColumn Column0;
        private Wisej.Web.DataGridViewColumn Column1;
    }
}