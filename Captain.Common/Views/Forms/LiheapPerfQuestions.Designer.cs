using Wisej.Web;


namespace Captain.Common.Views.Forms
{
    partial class LiheapPerfQuestions
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiheapPerfQuestions));
            this.panel1 = new Wisej.Web.Panel();
            this.gvwHouseingQues = new Wisej.Web.DataGridView();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.btnCancel = new Wisej.Web.Button();
            this.btnSave = new Wisej.Web.Button();
            this.gvtHeadcode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtQuesDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtResponseDesc = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtQuesCode = new Wisej.Web.DataGridViewTextBoxColumn();
            this.gvtQType = new Wisej.Web.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHouseingQues)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvwHouseingQues);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = Wisej.Web.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(717, 304);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // gvwHouseingQues
            // 
            this.gvwHouseingQues.AllowUserToResizeColumns = false;
            this.gvwHouseingQues.AllowUserToResizeRows = false;
            this.gvwHouseingQues.BackColor = System.Drawing.Color.White;
            this.gvwHouseingQues.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.gvtHeadcode,
            this.gvtQuesDesc,
            this.gvtResponseDesc,
            this.gvtQuesCode,
            this.gvtQType});
            this.gvwHouseingQues.ContextMenu = this.contextMenu1;
            this.gvwHouseingQues.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHouseingQues.Location = new System.Drawing.Point(0, 0);
            this.gvwHouseingQues.Name = "gvwHouseingQues";
            this.gvwHouseingQues.RowHeadersWidth = 10;
            this.gvwHouseingQues.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.gvwHouseingQues.ShowColumnVisibilityMenu = false;
            this.gvwHouseingQues.Size = new System.Drawing.Size(717, 269);
            this.gvwHouseingQues.TabIndex = 0;
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            this.contextMenu1.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.gvwHouseingQues_MenuClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AppearanceKey = "panel-grdo";
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 269);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(5, 0, 5, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(717, 35);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // btnCancel
            // 
            this.btnCancel.AppearanceKey = "button-cancel";
            this.btnCancel.AutoSize = true;
            this.btnCancel.Dock = Wisej.Web.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(640, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 30);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AppearanceKey = "button-ok";
            this.btnSave.AutoSize = true;
            this.btnSave.Dock = Wisej.Web.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(590, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(44, 30);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&OK";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gvtHeadcode
            // 
            this.gvtHeadcode.HeaderText = " ";
            this.gvtHeadcode.Name = "gvtHeadcode";
            this.gvtHeadcode.ReadOnly = true;
            this.gvtHeadcode.Visible = false;
            this.gvtHeadcode.Width = 10;
            // 
            // gvtQuesDesc
            // 
            this.gvtQuesDesc.HeaderText = "Questions";
            this.gvtQuesDesc.Name = "gvtQuesDesc";
            this.gvtQuesDesc.ReadOnly = true;
            this.gvtQuesDesc.Width = 580;
            // 
            // gvtResponseDesc
            // 
            this.gvtResponseDesc.HeaderText = "Response ";
            this.gvtResponseDesc.Name = "gvtResponseDesc";
            this.gvtResponseDesc.ReadOnly = true;
            // 
            // gvtQuesCode
            // 
            this.gvtQuesCode.HeaderText = " ";
            this.gvtQuesCode.Name = "gvtQuesCode";
            this.gvtQuesCode.ReadOnly = true;
            this.gvtQuesCode.Visible = false;
            // 
            // gvtQType
            // 
            this.gvtQType.HeaderText = "QType";
            this.gvtQType.Name = "gvtQType";
            this.gvtQType.Visible = false;
            // 
            // LiheapPerfQuestions
            // 
            this.ClientSize = new System.Drawing.Size(717, 304);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = Wisej.Web.FormBorderStyle.Fixed;
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LiheapPerfQuestions";
            this.Text = "Liheap Perf Questions";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwHouseingQues)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView gvwHouseingQues;
        private DataGridViewTextBoxColumn gvtHeadcode;
        private DataGridViewTextBoxColumn gvtQuesDesc;
        private DataGridViewTextBoxColumn gvtResponseDesc;
        private DataGridViewTextBoxColumn gvtQuesCode;
        private ContextMenu contextMenu1;
        private DataGridViewTextBoxColumn gvtQType;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnSave;
        private Button btnCancel;
    }
}