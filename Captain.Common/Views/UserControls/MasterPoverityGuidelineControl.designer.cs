//using Wisej.Web;
//using Gizmox.WebGUI.Common;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Views.UserControls
{
    partial class MasterPoverityGuidelineControl
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

        #region Visual WebGui UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new Wisej.Web.TabControl();
            this.tabFed = new Wisej.Web.TabPage();
            this.pnlFebOmb = new Wisej.Web.Panel();
            this.panel1 = new Wisej.Web.Panel();
            this.pbFedHelp = new Wisej.Web.PictureBox();
            this.tabHud = new Wisej.Web.TabPage();
            this.pnlTabHUD = new Wisej.Web.Panel();
            this.pnlHUDSubGrid = new Wisej.Web.Panel();
            this.spacer2 = new Wisej.Web.Spacer();
            this.pnlgvwHud = new Wisej.Web.Panel();
            this.gvwHud = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.spacer1 = new Wisej.Web.Spacer();
            this.pnlHUDCounty = new Wisej.Web.Panel();
            this.btnCopyFrom = new Wisej.Web.Button();
            this.cmbCounty = new Wisej.Web.ComboBox();
            this.lblCounty = new Wisej.Web.Label();
            this.panel2 = new Wisej.Web.Panel();
            this.pbHUDHelp = new Wisej.Web.PictureBox();
            this.tabCMI = new Wisej.Web.TabPage();
            this.pnlTabCMI = new Wisej.Web.Panel();
            this.pnlCMISubGrid = new Wisej.Web.Panel();
            this.spacer4 = new Wisej.Web.Spacer();
            this.pnlgvwCMI = new Wisej.Web.Panel();
            this.gvwCMI = new Captain.Common.Views.Controls.Compatibility.DataGridViewEx();
            this.spacer3 = new Wisej.Web.Spacer();
            this.pnlCMICounty = new Wisej.Web.Panel();
            this.btnCMICopyFrom = new Wisej.Web.Button();
            this.lblCMICounty = new Wisej.Web.Label();
            this.cmbCMICounty = new Wisej.Web.ComboBox();
            this.panel3 = new Wisej.Web.Panel();
            this.pbCMIHelp = new Wisej.Web.PictureBox();
            this.tabSMI = new Wisej.Web.TabPage();
            this.pnlSMI = new Wisej.Web.Panel();
            this.pnlSMISubGrid = new Wisej.Web.Panel();
            this.spacer5 = new Wisej.Web.Spacer();
            this.pnlSMIGrid = new Wisej.Web.Panel();
            this.panel4 = new Wisej.Web.Panel();
            this.pbSMIhelp = new Wisej.Web.PictureBox();
            this.comboBox2 = new Wisej.Web.ComboBox();
            this.label2 = new Wisej.Web.Label();
            this.label3 = new Wisej.Web.Label();
            this.comboBox3 = new Wisej.Web.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabFed.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFedHelp)).BeginInit();
            this.tabHud.SuspendLayout();
            this.pnlTabHUD.SuspendLayout();
            this.pnlgvwHud.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwHud)).BeginInit();
            this.pnlHUDCounty.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHUDHelp)).BeginInit();
            this.tabCMI.SuspendLayout();
            this.pnlTabCMI.SuspendLayout();
            this.pnlgvwCMI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvwCMI)).BeginInit();
            this.pnlCMICounty.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCMIHelp)).BeginInit();
            this.tabSMI.SuspendLayout();
            this.pnlSMI.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSMIhelp)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = Wisej.Web.TabAlignment.Top;
            this.tabControl1.Controls.Add(this.tabFed);
            this.tabControl1.Controls.Add(this.tabHud);
            this.tabControl1.Controls.Add(this.tabCMI);
            this.tabControl1.Controls.Add(this.tabSMI);
            this.tabControl1.Dock = Wisej.Web.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.PageInsets = new Wisej.Web.Padding(0, 27, 0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(904, 614);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.OnModulesTabControlSelectedIndexChanged);
            // 
            // tabFed
            // 
            this.tabFed.Controls.Add(this.pnlFebOmb);
            this.tabFed.Controls.Add(this.panel1);
            this.tabFed.Location = new System.Drawing.Point(0, 27);
            this.tabFed.Name = "tabFed";
            this.tabFed.Padding = new Wisej.Web.Padding(10, 5, 10, 10);
            this.tabFed.RightToLeft = Wisej.Web.RightToLeft.No;
            this.tabFed.Size = new System.Drawing.Size(904, 587);
            this.tabFed.Tag = "FED";
            this.tabFed.Text = "FPL";
            // 
            // pnlFebOmb
            // 
            this.pnlFebOmb.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlFebOmb.Cursor = Wisej.Web.Cursors.Default;
            this.pnlFebOmb.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlFebOmb.Location = new System.Drawing.Point(10, 5);
            this.pnlFebOmb.Name = "pnlFebOmb";
            this.pnlFebOmb.Size = new System.Drawing.Size(854, 572);
            this.pnlFebOmb.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.panel1.Controls.Add(this.pbFedHelp);
            this.panel1.Dock = Wisej.Web.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(864, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(30, 572);
            this.panel1.TabIndex = 1;
            // 
            // pbFedHelp
            // 
            this.pbFedHelp.Cursor = Wisej.Web.Cursors.Hand;
            this.pbFedHelp.Dock = Wisej.Web.DockStyle.Top;
            this.pbFedHelp.ImageSource = "icon-help";
            this.pbFedHelp.Location = new System.Drawing.Point(0, 0);
            this.pbFedHelp.Name = "pbFedHelp";
            this.pbFedHelp.Padding = new Wisej.Web.Padding(6);
            this.pbFedHelp.Size = new System.Drawing.Size(30, 30);
            this.pbFedHelp.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pbFedHelp.ToolTipText = "Federal Poverty Chart Help";
            this.pbFedHelp.Click += new System.EventHandler(this.pbFedHelp_Click);
            // 
            // tabHud
            // 
            this.tabHud.Controls.Add(this.pnlTabHUD);
            this.tabHud.Controls.Add(this.panel2);
            this.tabHud.Location = new System.Drawing.Point(0, 27);
            this.tabHud.Name = "tabHud";
            this.tabHud.Padding = new Wisej.Web.Padding(10, 5, 10, 10);
            this.tabHud.Size = new System.Drawing.Size(904, 587);
            this.tabHud.Tag = "HUD";
            this.tabHud.Text = "HUD                   ";
            // 
            // pnlTabHUD
            // 
            this.pnlTabHUD.Controls.Add(this.pnlHUDSubGrid);
            this.pnlTabHUD.Controls.Add(this.spacer2);
            this.pnlTabHUD.Controls.Add(this.pnlgvwHud);
            this.pnlTabHUD.Controls.Add(this.spacer1);
            this.pnlTabHUD.Controls.Add(this.pnlHUDCounty);
            this.pnlTabHUD.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlTabHUD.Location = new System.Drawing.Point(10, 5);
            this.pnlTabHUD.Name = "pnlTabHUD";
            this.pnlTabHUD.Size = new System.Drawing.Size(854, 572);
            this.pnlTabHUD.TabIndex = 3;
            // 
            // pnlHUDSubGrid
            // 
            this.pnlHUDSubGrid.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlHUDSubGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlHUDSubGrid.Location = new System.Drawing.Point(0, 231);
            this.pnlHUDSubGrid.Name = "pnlHUDSubGrid";
            this.pnlHUDSubGrid.Size = new System.Drawing.Size(854, 341);
            this.pnlHUDSubGrid.TabIndex = 4;
            // 
            // spacer2
            // 
            this.spacer2.Dock = Wisej.Web.DockStyle.Top;
            this.spacer2.Location = new System.Drawing.Point(0, 226);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(854, 5);
            // 
            // pnlgvwHud
            // 
            this.pnlgvwHud.Controls.Add(this.gvwHud);
            this.pnlgvwHud.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlgvwHud.Dock = Wisej.Web.DockStyle.Top;
            this.pnlgvwHud.Location = new System.Drawing.Point(0, 46);
            this.pnlgvwHud.Name = "pnlgvwHud";
            this.pnlgvwHud.Size = new System.Drawing.Size(854, 180);
            this.pnlgvwHud.TabIndex = 3;
            // 
            // gvwHud
            // 
            this.gvwHud.AllowUserToResizeColumns = false;
            this.gvwHud.AllowUserToResizeRows = false;
            this.gvwHud.BackColor = System.Drawing.Color.White;
            this.gvwHud.BorderStyle = Wisej.Web.BorderStyle.None;
            this.gvwHud.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwHud.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwHud.MultiSelect = false;
            this.gvwHud.Name = "gvwHud";
            this.gvwHud.RowHeadersWidth = 14;
            this.gvwHud.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwHud.Size = new System.Drawing.Size(854, 180);
            this.gvwHud.TabIndex = 0;
            this.gvwHud.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // spacer1
            // 
            this.spacer1.Dock = Wisej.Web.DockStyle.Top;
            this.spacer1.Location = new System.Drawing.Point(0, 41);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(854, 5);
            // 
            // pnlHUDCounty
            // 
            this.pnlHUDCounty.Controls.Add(this.btnCopyFrom);
            this.pnlHUDCounty.Controls.Add(this.cmbCounty);
            this.pnlHUDCounty.Controls.Add(this.lblCounty);
            this.pnlHUDCounty.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlHUDCounty.Dock = Wisej.Web.DockStyle.Top;
            this.pnlHUDCounty.Location = new System.Drawing.Point(0, 0);
            this.pnlHUDCounty.Name = "pnlHUDCounty";
            this.pnlHUDCounty.Size = new System.Drawing.Size(854, 41);
            this.pnlHUDCounty.TabIndex = 2;
            // 
            // btnCopyFrom
            // 
            this.btnCopyFrom.Location = new System.Drawing.Point(279, 8);
            this.btnCopyFrom.Name = "btnCopyFrom";
            this.btnCopyFrom.Size = new System.Drawing.Size(80, 25);
            this.btnCopyFrom.TabIndex = 2;
            this.btnCopyFrom.Text = "&Copy From";
            this.btnCopyFrom.Click += new System.EventHandler(this.btnCopyFrom_Click);
            // 
            // cmbCounty
            // 
            this.cmbCounty.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCounty.FormattingEnabled = true;
            this.cmbCounty.Location = new System.Drawing.Point(72, 8);
            this.cmbCounty.Name = "cmbCounty";
            this.cmbCounty.Size = new System.Drawing.Size(175, 25);
            this.cmbCounty.TabIndex = 1;
            this.cmbCounty.SelectedIndexChanged += new System.EventHandler(this.cmbCounty_SelectedIndexChanged);
            // 
            // lblCounty
            // 
            this.lblCounty.Location = new System.Drawing.Point(15, 12);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(41, 16);
            this.lblCounty.TabIndex = 0;
            this.lblCounty.Text = "County";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.panel2.Controls.Add(this.pbHUDHelp);
            this.panel2.Dock = Wisej.Web.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(864, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(30, 572);
            this.panel2.TabIndex = 4;
            // 
            // pbHUDHelp
            // 
            this.pbHUDHelp.Cursor = Wisej.Web.Cursors.Hand;
            this.pbHUDHelp.Dock = Wisej.Web.DockStyle.Top;
            this.pbHUDHelp.ImageSource = "icon-help";
            this.pbHUDHelp.Location = new System.Drawing.Point(0, 0);
            this.pbHUDHelp.Name = "pbHUDHelp";
            this.pbHUDHelp.Padding = new Wisej.Web.Padding(6);
            this.pbHUDHelp.Size = new System.Drawing.Size(30, 30);
            this.pbHUDHelp.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pbHUDHelp.ToolTipText = "HUD Help";
            this.pbHUDHelp.Click += new System.EventHandler(this.pbHUDHelp_Click);
            // 
            // tabCMI
            // 
            this.tabCMI.Controls.Add(this.pnlTabCMI);
            this.tabCMI.Controls.Add(this.panel3);
            this.tabCMI.Location = new System.Drawing.Point(0, 27);
            this.tabCMI.Name = "tabCMI";
            this.tabCMI.Padding = new Wisej.Web.Padding(10, 5, 10, 10);
            this.tabCMI.Size = new System.Drawing.Size(904, 587);
            this.tabCMI.Tag = "CMI";
            this.tabCMI.Text = "CMI                   ";
            // 
            // pnlTabCMI
            // 
            this.pnlTabCMI.Controls.Add(this.pnlCMISubGrid);
            this.pnlTabCMI.Controls.Add(this.spacer4);
            this.pnlTabCMI.Controls.Add(this.pnlgvwCMI);
            this.pnlTabCMI.Controls.Add(this.spacer3);
            this.pnlTabCMI.Controls.Add(this.pnlCMICounty);
            this.pnlTabCMI.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlTabCMI.Location = new System.Drawing.Point(10, 5);
            this.pnlTabCMI.Name = "pnlTabCMI";
            this.pnlTabCMI.Size = new System.Drawing.Size(854, 572);
            this.pnlTabCMI.TabIndex = 2;
            // 
            // pnlCMISubGrid
            // 
            this.pnlCMISubGrid.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlCMISubGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlCMISubGrid.Location = new System.Drawing.Point(0, 231);
            this.pnlCMISubGrid.Name = "pnlCMISubGrid";
            this.pnlCMISubGrid.Size = new System.Drawing.Size(854, 341);
            this.pnlCMISubGrid.TabIndex = 3;
            // 
            // spacer4
            // 
            this.spacer4.Dock = Wisej.Web.DockStyle.Top;
            this.spacer4.Location = new System.Drawing.Point(0, 226);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(854, 5);
            // 
            // pnlgvwCMI
            // 
            this.pnlgvwCMI.Controls.Add(this.gvwCMI);
            this.pnlgvwCMI.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlgvwCMI.Dock = Wisej.Web.DockStyle.Top;
            this.pnlgvwCMI.Location = new System.Drawing.Point(0, 46);
            this.pnlgvwCMI.Name = "pnlgvwCMI";
            this.pnlgvwCMI.Size = new System.Drawing.Size(854, 180);
            this.pnlgvwCMI.TabIndex = 2;
            // 
            // gvwCMI
            // 
            this.gvwCMI.AllowUserToResizeColumns = false;
            this.gvwCMI.AllowUserToResizeRows = false;
            this.gvwCMI.BackColor = System.Drawing.Color.White;
            this.gvwCMI.BorderStyle = Wisej.Web.BorderStyle.None;
            this.gvwCMI.ColumnHeadersHeightSizeMode = Wisej.Web.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvwCMI.Dock = Wisej.Web.DockStyle.Fill;
            this.gvwCMI.MultiSelect = false;
            this.gvwCMI.Name = "gvwCMI";
            this.gvwCMI.RowHeadersWidth = 14;
            this.gvwCMI.RowHeadersWidthSizeMode = Wisej.Web.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvwCMI.Size = new System.Drawing.Size(854, 180);
            this.gvwCMI.TabIndex = 0;
            this.gvwCMI.Click += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // spacer3
            // 
            this.spacer3.Dock = Wisej.Web.DockStyle.Top;
            this.spacer3.Location = new System.Drawing.Point(0, 41);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(854, 5);
            // 
            // pnlCMICounty
            // 
            this.pnlCMICounty.Controls.Add(this.btnCMICopyFrom);
            this.pnlCMICounty.Controls.Add(this.lblCMICounty);
            this.pnlCMICounty.Controls.Add(this.cmbCMICounty);
            this.pnlCMICounty.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlCMICounty.Dock = Wisej.Web.DockStyle.Top;
            this.pnlCMICounty.Location = new System.Drawing.Point(0, 0);
            this.pnlCMICounty.Name = "pnlCMICounty";
            this.pnlCMICounty.Size = new System.Drawing.Size(854, 41);
            this.pnlCMICounty.TabIndex = 1;
            // 
            // btnCMICopyFrom
            // 
            this.btnCMICopyFrom.Location = new System.Drawing.Point(277, 8);
            this.btnCMICopyFrom.Name = "btnCMICopyFrom";
            this.btnCMICopyFrom.Size = new System.Drawing.Size(80, 25);
            this.btnCMICopyFrom.TabIndex = 2;
            this.btnCMICopyFrom.Text = "&Copy From";
            this.btnCMICopyFrom.Click += new System.EventHandler(this.btnCMICopyFrom_Click);
            // 
            // lblCMICounty
            // 
            this.lblCMICounty.Location = new System.Drawing.Point(15, 12);
            this.lblCMICounty.Name = "lblCMICounty";
            this.lblCMICounty.Size = new System.Drawing.Size(42, 16);
            this.lblCMICounty.TabIndex = 0;
            this.lblCMICounty.Text = "County";
            // 
            // cmbCMICounty
            // 
            this.cmbCMICounty.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbCMICounty.FormattingEnabled = true;
            this.cmbCMICounty.Location = new System.Drawing.Point(72, 8);
            this.cmbCMICounty.Name = "cmbCMICounty";
            this.cmbCMICounty.Size = new System.Drawing.Size(175, 25);
            this.cmbCMICounty.TabIndex = 1;
            this.cmbCMICounty.SelectedIndexChanged += new System.EventHandler(this.cmbCMICounty_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.panel3.Controls.Add(this.pbCMIHelp);
            this.panel3.Dock = Wisej.Web.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(864, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(30, 572);
            this.panel3.TabIndex = 3;
            // 
            // pbCMIHelp
            // 
            this.pbCMIHelp.Cursor = Wisej.Web.Cursors.Hand;
            this.pbCMIHelp.Dock = Wisej.Web.DockStyle.Top;
            this.pbCMIHelp.ImageSource = "icon-help";
            this.pbCMIHelp.Location = new System.Drawing.Point(0, 0);
            this.pbCMIHelp.Name = "pbCMIHelp";
            this.pbCMIHelp.Padding = new Wisej.Web.Padding(6);
            this.pbCMIHelp.Size = new System.Drawing.Size(30, 30);
            this.pbCMIHelp.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pbCMIHelp.ToolTipText = "CMI Help";
            this.pbCMIHelp.Click += new System.EventHandler(this.pbCMIHelp_Click);
            // 
            // tabSMI
            // 
            this.tabSMI.Controls.Add(this.pnlSMI);
            this.tabSMI.Controls.Add(this.panel4);
            this.tabSMI.Location = new System.Drawing.Point(0, 27);
            this.tabSMI.Name = "tabSMI";
            this.tabSMI.Padding = new Wisej.Web.Padding(10, 5, 10, 10);
            this.tabSMI.Size = new System.Drawing.Size(904, 587);
            this.tabSMI.Tag = "SMI";
            this.tabSMI.Text = "SMI                   ";
            // 
            // pnlSMI
            // 
            this.pnlSMI.Controls.Add(this.pnlSMISubGrid);
            this.pnlSMI.Controls.Add(this.spacer5);
            this.pnlSMI.Controls.Add(this.pnlSMIGrid);
            this.pnlSMI.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSMI.Location = new System.Drawing.Point(10, 5);
            this.pnlSMI.Name = "pnlSMI";
            this.pnlSMI.Size = new System.Drawing.Size(854, 572);
            this.pnlSMI.TabIndex = 0;
            // 
            // pnlSMISubGrid
            // 
            this.pnlSMISubGrid.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlSMISubGrid.Dock = Wisej.Web.DockStyle.Fill;
            this.pnlSMISubGrid.Location = new System.Drawing.Point(0, 326);
            this.pnlSMISubGrid.Name = "pnlSMISubGrid";
            this.pnlSMISubGrid.Size = new System.Drawing.Size(854, 246);
            this.pnlSMISubGrid.TabIndex = 1;
            // 
            // spacer5
            // 
            this.spacer5.Dock = Wisej.Web.DockStyle.Top;
            this.spacer5.Location = new System.Drawing.Point(0, 321);
            this.spacer5.Name = "spacer5";
            this.spacer5.Size = new System.Drawing.Size(854, 5);
            // 
            // pnlSMIGrid
            // 
            this.pnlSMIGrid.CssStyle = "border-radius:8px; border:1px solid #ececec;";
            this.pnlSMIGrid.Dock = Wisej.Web.DockStyle.Top;
            this.pnlSMIGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlSMIGrid.Name = "pnlSMIGrid";
            this.pnlSMIGrid.Size = new System.Drawing.Size(854, 321);
            this.pnlSMIGrid.TabIndex = 0;
            this.pnlSMIGrid.SizeChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.panel4.Controls.Add(this.pbSMIhelp);
            this.panel4.Dock = Wisej.Web.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(864, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(30, 572);
            this.panel4.TabIndex = 2;
            // 
            // pbSMIhelp
            // 
            this.pbSMIhelp.Cursor = Wisej.Web.Cursors.Hand;
            this.pbSMIhelp.Dock = Wisej.Web.DockStyle.Top;
            this.pbSMIhelp.ImageSource = "icon-help";
            this.pbSMIhelp.Location = new System.Drawing.Point(0, 0);
            this.pbSMIhelp.Name = "pbSMIhelp";
            this.pbSMIhelp.Padding = new Wisej.Web.Padding(6);
            this.pbSMIhelp.Size = new System.Drawing.Size(30, 30);
            this.pbSMIhelp.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pbSMIhelp.ToolTipText = "SMI Help";
            this.pbSMIhelp.Click += new System.EventHandler(this.pbSMIhelp_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(366, 193);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(169, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Meal Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Meal Type";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(136, 14);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(169, 21);
            this.comboBox3.TabIndex = 1;
            // 
            // MasterPoverityGuidelineControl
            // 
            this.Controls.Add(this.tabControl1);
            this.Name = "MasterPoverityGuidelineControl";
            this.Size = new System.Drawing.Size(904, 639);
            this.Load += new System.EventHandler(this.MasterPoverityGuidelineControl_Load);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabFed.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbFedHelp)).EndInit();
            this.tabHud.ResumeLayout(false);
            this.pnlTabHUD.ResumeLayout(false);
            this.pnlgvwHud.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwHud)).EndInit();
            this.pnlHUDCounty.ResumeLayout(false);
            this.pnlHUDCounty.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbHUDHelp)).EndInit();
            this.tabCMI.ResumeLayout(false);
            this.pnlTabCMI.ResumeLayout(false);
            this.pnlgvwCMI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvwCMI)).EndInit();
            this.pnlCMICounty.ResumeLayout(false);
            this.pnlCMICounty.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCMIHelp)).EndInit();
            this.tabSMI.ResumeLayout(false);
            this.pnlSMI.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSMIhelp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabFed;
        private TabPage tabHud;
        private TabPage tabCMI;
        private TabPage tabSMI;
        private ComboBox comboBox2;
        private Label label2;
        private Label label3;
        private ComboBox comboBox3;
        private ComboBox cmbCounty;
        private Label lblCounty;
        private ComboBox cmbCMICounty;
        private Label lblCMICounty;
        private Panel pnlFebOmb;
        private Panel pnlHUDCounty;
        private DataGridViewEx gvwHud;
        private Panel pnlCMICounty;
        private DataGridViewEx gvwCMI;
        private Panel pnlTabHUD;
        private Panel pnlgvwHud;
        private Panel pnlTabCMI;
        private Panel pnlgvwCMI;
        private Panel pnlHUDSubGrid;
        private Panel pnlCMISubGrid;
        private Panel pnlSMI;
        private Panel pnlSMIGrid;
        private Panel pnlSMISubGrid;
        private Panel panel1;
        private PictureBox pbFedHelp;
        private Panel panel2;
        private PictureBox pbHUDHelp;
        private Panel panel3;
        private PictureBox pbCMIHelp;
        private Panel panel4;
        private PictureBox pbSMIhelp;
        private Spacer spacer2;
        private Spacer spacer1;
        private Spacer spacer4;
        private Spacer spacer3;
        private Spacer spacer5;
        private Button btnCopyFrom;
        private Button btnCMICopyFrom;
    }
}