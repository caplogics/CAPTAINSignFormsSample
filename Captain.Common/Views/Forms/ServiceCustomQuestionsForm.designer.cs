using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    partial class ServiceCustomQuestionsForm
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
            Wisej.Web.DataGridViewCellStyle dataGridViewCellStyle1 = new Wisej.Web.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceCustomQuestionsForm));
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            this.Btn_MS_Save = new Wisej.Web.Button();
            this.button3 = new Wisej.Web.Button();
            this.contextMenu1 = new Wisej.Web.ContextMenu(this.components);
            this.panel3 = new Wisej.Web.Panel();
            this.Cust_Grid = new Wisej.Web.DataGridView();
            this.panel1 = new Wisej.Web.Panel();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnllblSelHie = new Wisej.Web.Panel();
            this.lblSelected = new Wisej.Web.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cust_Grid)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnllblSelHie.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_MS_Save
            // 
            this.Btn_MS_Save.AppearanceKey = "button-ok";
            this.Btn_MS_Save.Dock = Wisej.Web.DockStyle.Right;
            this.Btn_MS_Save.Location = new System.Drawing.Point(489, 5);
            this.Btn_MS_Save.Name = "Btn_MS_Save";
            this.Btn_MS_Save.Size = new System.Drawing.Size(60, 25);
            this.Btn_MS_Save.TabIndex = 1;
            this.Btn_MS_Save.Text = "&OK";
            this.Btn_MS_Save.Click += new System.EventHandler(this.Btn_MS_Save_Click);
            // 
            // button3
            // 
            this.button3.AppearanceKey = "button-cancel";
            this.button3.Dock = Wisej.Web.DockStyle.Right;
            this.button3.Location = new System.Drawing.Point(554, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 25);
            this.button3.TabIndex = 2;
            this.button3.Text = "&Cancel";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.RightToLeft = Wisej.Web.RightToLeft.No;
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            this.contextMenu1.MenuItemClicked += new Wisej.Web.MenuItemEventHandler(this.CA_Cust_Grid_MenuClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Cust_Grid);
            this.panel3.Dock = Wisej.Web.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 30);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Wisej.Web.Padding(5, 0, 5, 5);
            this.panel3.Size = new System.Drawing.Size(644, 189);
            this.panel3.TabIndex = 19;
            // 
            // Cust_Grid
            // 
            this.Cust_Grid.AutoSizeRowsMode = Wisej.Web.DataGridViewAutoSizeRowsMode.AllCells;
            this.Cust_Grid.BackColor = System.Drawing.Color.FromArgb(253, 253, 253);
            this.Cust_Grid.BorderStyle = Wisej.Web.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = Wisej.Web.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("@default", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = Wisej.Web.DataGridViewTriState.True;
            this.Cust_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Cust_Grid.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Cust_Grid.Dock = Wisej.Web.DockStyle.Fill;
            this.Cust_Grid.EditMode = Wisej.Web.DataGridViewEditMode.EditOnEnter;
            this.Cust_Grid.Location = new System.Drawing.Point(5, 0);
            this.Cust_Grid.Name = "Cust_Grid";
            this.Cust_Grid.RowHeadersWidth = 25;
            this.Cust_Grid.ScrollBars = Wisej.Web.ScrollBars.Vertical;
            this.Cust_Grid.ShowColumnVisibilityMenu = false;
            this.Cust_Grid.Size = new System.Drawing.Size(634, 184);
            this.Cust_Grid.TabIndex = 16;
            this.Cust_Grid.CellEndEdit += new Wisej.Web.DataGridViewCellEventHandler(this.Cust_Grid_CellEndEdit);
            this.Cust_Grid.CellClick += new Wisej.Web.DataGridViewCellEventHandler(this.Cust_Grid1_CellClick);
            // 
            // panel1
            // 
            this.panel1.AppearanceKey = "panel-grdo";
            this.panel1.Controls.Add(this.Btn_MS_Save);
            this.panel1.Controls.Add(this.spacer1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = Wisej.Web.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 219);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Wisej.Web.Padding(5, 5, 15, 5);
            this.panel1.Size = new System.Drawing.Size(644, 35);
            this.panel1.TabIndex = 20;
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Right;
            this.spacer1.Location = new System.Drawing.Point(549, 5);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(5, 25);
            // 
            // pnllblSelHie
            // 
            this.pnllblSelHie.Controls.Add(this.lblSelected);
            this.pnllblSelHie.Dock = Wisej.Web.DockStyle.Top;
            this.pnllblSelHie.Location = new System.Drawing.Point(0, 5);
            this.pnllblSelHie.Name = "pnllblSelHie";
            this.pnllblSelHie.Padding = new Wisej.Web.Padding(5, 0, 5, 0);
            this.pnllblSelHie.Size = new System.Drawing.Size(644, 25);
            this.pnllblSelHie.TabIndex = 21;
            // 
            // lblSelected
            // 
            this.lblSelected.AppearanceKey = "lblsubHeading";
            this.lblSelected.Dock = Wisej.Web.DockStyle.Left;
            this.lblSelected.Font = new System.Drawing.Font("@subHeading", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSelected.Location = new System.Drawing.Point(5, 0);
            this.lblSelected.MinimumSize = new System.Drawing.Size(0, 18);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(139, 25);
            this.lblSelected.TabIndex = 6;
            this.lblSelected.Text = "Custom Questions";
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServiceCustomQuestionsForm
            // 
            this.ClientSize = new System.Drawing.Size(644, 254);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnllblSelHie);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Image)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServiceCustomQuestionsForm";
            this.Padding = new Wisej.Web.Padding(0, 5, 0, 0);
            this.Text = "MembersGridForm";
            componentTool1.ImageSource = "icon-help";
            componentTool1.Name = "Help";
            componentTool1.ToolTipText = "Help";
            this.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1});
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Cust_Grid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnllblSelHie.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private Button Btn_MS_Save;
        private Button button3;
        private Panel panel3;
        private ContextMenu contextMenu1;
        private Panel panel1;
        private Spacer spacer1;
        private Panel pnllblSelHie;
        private Label lblSelected;
        private DataGridView Cust_Grid;
    }
}