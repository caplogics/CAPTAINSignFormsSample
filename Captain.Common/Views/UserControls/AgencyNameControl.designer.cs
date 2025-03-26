using Captain.Common.Views.Controls.Compatibility;
using Wisej.Web;

namespace Captain.Common.Views.UserControls
{
    partial class AgencyNameControl
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

        #region Wisej UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new Wisej.Web.Panel();
            this.btnAdvance = new Wisej.Web.Button();
            this.Btn_First = new Wisej.Web.Button();
            this.BtnPrev = new Wisej.Web.Button();
            this.BtnNxt = new Wisej.Web.Button();
            this.BtnN10 = new Wisej.Web.Button();
            this.BtnLast = new Wisej.Web.Button();
            this.BtnP10 = new Wisej.Web.Button();
            this.txtAgencyNo = new TextBoxWithValidation();
            this.lblAgencyHeader = new Wisej.Web.Label();
            this.lblAgencyName = new Wisej.Web.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnAdvance);
            this.panel1.Controls.Add(this.Btn_First);
            this.panel1.Controls.Add(this.BtnPrev);
            this.panel1.Controls.Add(this.BtnNxt);
            this.panel1.Controls.Add(this.BtnN10);
            this.panel1.Controls.Add(this.BtnLast);
            this.panel1.Controls.Add(this.BtnP10);
            this.panel1.Controls.Add(this.txtAgencyNo);
            this.panel1.Controls.Add(this.lblAgencyHeader);
            this.panel1.Controls.Add(this.lblAgencyName);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(849, 20);
            this.panel1.TabIndex = 0;
            // 
            // btnAdvance
            // 
            this.btnAdvance.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdvance.Location = new System.Drawing.Point(370, 0);
            this.btnAdvance.Name = "btnAdvance";
            this.btnAdvance.Size = new System.Drawing.Size(28, 20);
            this.btnAdvance.TabIndex = 3;
            this.btnAdvance.Text = "...";
            this.btnAdvance.Click += new System.EventHandler(this.btnAdvance_Click);
            // 
            // Btn_First
            // 
            this.Btn_First.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_First.Location = new System.Drawing.Point(402, 0);
            this.Btn_First.Name = "Btn_First";
            this.Btn_First.Size = new System.Drawing.Size(28, 20);
            this.Btn_First.TabIndex = 4;
            this.Btn_First.Text = "|<";
            this.Btn_First.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnPrev
            // 
            this.BtnPrev.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrev.Location = new System.Drawing.Point(456, 0);
            this.BtnPrev.Name = "BtnPrev";
            this.BtnPrev.Size = new System.Drawing.Size(28, 20);
            this.BtnPrev.TabIndex = 6;
            this.BtnPrev.Text = "<";
            this.BtnPrev.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnNxt
            // 
            this.BtnNxt.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNxt.Location = new System.Drawing.Point(483, 0);
            this.BtnNxt.Name = "BtnNxt";
            this.BtnNxt.Size = new System.Drawing.Size(28, 20);
            this.BtnNxt.TabIndex = 7;
            this.BtnNxt.Text = ">";
            this.BtnNxt.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnN10
            // 
            this.BtnN10.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnN10.Location = new System.Drawing.Point(510, 0);
            this.BtnN10.Name = "BtnN10";
            this.BtnN10.Size = new System.Drawing.Size(28, 20);
            this.BtnN10.TabIndex = 8;
            this.BtnN10.Text = ">>";
            this.BtnN10.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnLast
            // 
            this.BtnLast.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLast.Location = new System.Drawing.Point(537, 0);
            this.BtnLast.Name = "BtnLast";
            this.BtnLast.Size = new System.Drawing.Size(28, 20);
            this.BtnLast.TabIndex = 9;
            this.BtnLast.Text = ">|";
            this.BtnLast.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // BtnP10
            // 
            this.BtnP10.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnP10.Location = new System.Drawing.Point(429, 0);
            this.BtnP10.Name = "BtnP10";
            this.BtnP10.Size = new System.Drawing.Size(28, 20);
            this.BtnP10.TabIndex = 5;
            this.BtnP10.Text = "<<";
            this.BtnP10.Click += new System.EventHandler(this.Navigation_Click);
            // 
            // txtAgencyNo
            // 
            this.txtAgencyNo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtAgencyNo.Location = new System.Drawing.Point(101, 2);
            this.txtAgencyNo.Name = "txtAgencyNo";
            this.txtAgencyNo.Size = new System.Drawing.Size(69, 15);
            this.txtAgencyNo.TabIndex = 2;
            this.txtAgencyNo.TextAlign = Wisej.Web.HorizontalAlignment.Right;
            this.txtAgencyNo.KeyDown += new Wisej.Web.KeyEventHandler(this.txtAppNo_EnterKeyDown);
            this.txtAgencyNo.LostFocus += new System.EventHandler(this.txtAgenyno_LostFocus);
            // 
            // lblAgencyHeader
            // 
            this.lblAgencyHeader.AutoSize = true;
            this.lblAgencyHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAgencyHeader.Location = new System.Drawing.Point(5, 2);
            this.lblAgencyHeader.Name = "lblAgencyHeader";
            this.lblAgencyHeader.Size = new System.Drawing.Size(35, 13);
            this.lblAgencyHeader.TabIndex = 1;
            this.lblAgencyHeader.Text = "Agency Code #";
            // 
            // lblAgencyName
            // 
            this.lblAgencyName.AutoSize = true;
            this.lblAgencyName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAgencyName.Location = new System.Drawing.Point(172, 3);
            this.lblAgencyName.Name = "lblAgencyName";
            this.lblAgencyName.Size = new System.Drawing.Size(607, 20);
            this.lblAgencyName.TabIndex = 0;
            this.lblAgencyName.Text = "..";
            // 
            // AgencyNameControl
            // 
            this.Controls.Add(this.panel1);
            this.Size = new System.Drawing.Size(849, 20);
            this.Text = "AgencyNameControl";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        public Label lblAgencyName;
        public TextBoxWithValidation txtAgencyNo;
        public Label lblAgencyHeader;
        private Button btnAdvance;
        public Button Btn_First;
        public Button BtnPrev;
        public Button BtnNxt;
        public Button BtnN10;
        public Button BtnLast;
        public Button BtnP10;
    }
}