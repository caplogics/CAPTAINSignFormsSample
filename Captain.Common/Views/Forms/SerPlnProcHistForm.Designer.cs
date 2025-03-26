using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.Forms
{
    partial class SerPlnProcHistForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle7 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle2 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle3 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle4 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle5 = new Wisej.Web.DataGridViewCellStyle();
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle6 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerPlnProcHistForm));
            this.panel1 = new Wisej.Web.Panel();
            this.pnlHist = new Wisej.Web.Panel();
            this.panel3 = new Wisej.Web.Panel();
            this.panel2 = new Wisej.Web.Panel();
            this.lblHeadmsg = new Wisej.Web.Label();
            this.panel4 = new Wisej.Web.Panel();
            this.dgvProcHist = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.Column0 = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
            this.Column1 = new Wisej.Web.DataGridViewColumn();
            this.Column2 = new Wisej.Web.DataGridViewColumn();
            this.panel1.SuspendLayout();
            this.pnlHist.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcHist)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlHist);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(737, 409);
            this.panel1.TabIndex = 0;
            // 
            // pnlHist
            // 
            this.pnlHist.Controls.Add(this.panel4);
            this.pnlHist.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlHist.Location = new System.Drawing.Point(0, 33);
            this.pnlHist.Name = "pnlHist";
            this.pnlHist.Padding = new Wisej.Web.Padding(5);
            this.pnlHist.Size = new System.Drawing.Size(737, 341);
            this.pnlHist.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.AppearanceKey = "panel-grdo";
            this.panel3.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 374);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(737, 35);
            this.panel3.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.panel2.Controls.Add(this.lblHeadmsg);
            this.panel2.Dock = Wisej.Web.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Wisej.Web.Padding(8);
            this.panel2.Size = new System.Drawing.Size(737, 33);
            this.panel2.TabIndex = 0;
            // 
            // lblHeadmsg
            // 
            this.lblHeadmsg.AutoSize = true;
            this.lblHeadmsg.Dock = Wisej.Web.DockStyle.Fill;
            this.lblHeadmsg.Font = new System.Drawing.Font("default", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblHeadmsg.Location = new System.Drawing.Point(8, 8);
            this.lblHeadmsg.Name = "lblHeadmsg";
            this.lblHeadmsg.Size = new System.Drawing.Size(721, 17);
            this.lblHeadmsg.TabIndex = 0;
            this.lblHeadmsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvProcHist);
            this.panel4.CssStyle = " border:1px solid #ececec; border-radius:10px;";
            this.panel4.Dock = Wisej.Web.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(727, 331);
            this.panel4.TabIndex = 1;
            // 
            // dgvProcHist
            // 
            this.dgvProcHist.AllowUserToResizeColumns = false;
            this.dgvProcHist.AllowUserToResizeRows = false;
            this.dgvProcHist.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvProcHist.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.Column0,
            this.Column1,
            this.Column2});
            this.dgvProcHist.Dock = Wisej.Web.DockStyle.Fill;
            this.dgvProcHist.Name = "dgvProcHist";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(214, 214, 214);
            this.dgvProcHist.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvProcHist.RowHeadersWidth = 20;
            this.dgvProcHist.Size = new System.Drawing.Size(727, 331);
            this.dgvProcHist.TabIndex = 0;
            // 
            // Column0
            // 
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Format = "MM/dd/yyyy";
            this.Column0.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column0.HeaderStyle = dataGridViewCellStyle2;
            this.Column0.HeaderText = "Date";
            this.Column0.Name = "Column0";
            this.Column0.Width = 150;
            // 
            // Column1
            // 
            dataGridViewCellStyle3.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.HeaderStyle = dataGridViewCellStyle4;
            this.Column1.HeaderText = "Worker";
            this.Column1.Name = "Column1";
            this.Column1.Width = 100;
            // 
            // Column2
            // 
            dataGridViewCellStyle5.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleLeft;
            this.Column2.HeaderStyle = dataGridViewCellStyle6;
            this.Column2.HeaderText = "Short Notes";
            this.Column2.Name = "Column2";
            this.Column2.Width = 400;
            // 
            // SerPlnProcHistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 409);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerPlnProcHistForm";
            this.Text = "SerPlnProcHistForm";
            this.panel1.ResumeLayout(false);
            this.pnlHist.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcHist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel panel1;
        private Wisej.Web.Panel pnlHist;
        private DataGridViewEx dgvProcHist;
        private Wisej.Web.Panel panel3;
        private Wisej.Web.Panel panel2;
        private Wisej.Web.Label lblHeadmsg;
        private DataGridViewDateTimeColumn colDate;
        private Wisej.Web.DataGridViewColumn ColWorker;
        private Wisej.Web.DataGridViewColumn ColShtnotes;
        private DataGridViewDateTimeColumn Column0;
        private Wisej.Web.DataGridViewColumn Column1;
        private Wisej.Web.DataGridViewColumn Column2;
        private Wisej.Web.Panel panel4;
    }
}