namespace Captain.Common.Views.UserControls
{
    partial class ListBoxWithDropDownControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListBoxWithDropDownControl));
            this.panelHeader = new Wisej.Web.Panel();
            this.comboBox = new Wisej.Web.ComboBox();
            this.picDeleteItem = new Wisej.Web.PictureBox();
            this.pictureBoxAdd = new Wisej.Web.PictureBox();
            this.internalListBox = new Wisej.Web.ListBox();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BorderStyle = Wisej.Web.BorderStyle.None;
            this.panelHeader.Controls.Add(this.comboBox);
            this.panelHeader.Controls.Add(this.picDeleteItem);
            this.panelHeader.Controls.Add(this.pictureBoxAdd);
            this.panelHeader.Dock = Wisej.Web.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(277, 27);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Visible = false;
            // 
            // comboBox
            // 
            this.comboBox.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.comboBox.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.comboBox.Location = new System.Drawing.Point(0, 0);
            this.comboBox.MaxLength = 8;
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(125, 21);
            this.comboBox.TabIndex = 2;
            // 
            // picDeleteItem
            // 
            this.picDeleteItem.BackColor = System.Drawing.Color.Transparent;
            this.picDeleteItem.BackgroundImageLayout = Wisej.Web.ImageLayout.Center;
            this.picDeleteItem.Cursor = Wisej.Web.Cursors.Hand;
            //this.picDeleteItem.Image = new Gizmox.WebGUI.Common.Resources.IconResourceHandle(resources.GetString("picDeleteItem.Image"));

            this.picDeleteItem.Location = new System.Drawing.Point(151, 3);
            this.picDeleteItem.Name = "picDeleteItem";
            this.picDeleteItem.Size = new System.Drawing.Size(16, 16);
            this.picDeleteItem.SizeMode = Wisej.Web.PictureBoxSizeMode.CenterImage;
            this.picDeleteItem.TabIndex = 1;
            this.picDeleteItem.TabStop = false;
            this.controlToolTip.SetToolTip(this.picDeleteItem, "Delete");
            this.picDeleteItem.Click += new System.EventHandler(this.OnDeleteItem);
            // 
            // pictureBoxAdd
            // 
            this.pictureBoxAdd.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxAdd.BackgroundImageLayout = Wisej.Web.ImageLayout.Center;
            this.pictureBoxAdd.Cursor = Wisej.Web.Cursors.Hand;
            //this.pictureBoxAdd.Image = new Gizmox.WebGUI.Common.Resources.IconResourceHandle(resources.GetString("pictureBoxAdd.Image"));
            this.pictureBoxAdd.Location = new System.Drawing.Point(130, 3);
            this.pictureBoxAdd.Name = "pictureBoxAdd";
            this.pictureBoxAdd.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxAdd.SizeMode = Wisej.Web.PictureBoxSizeMode.CenterImage;
            this.pictureBoxAdd.TabIndex = 1;
            this.pictureBoxAdd.TabStop = false;
            this.controlToolTip.SetToolTip(this.pictureBoxAdd, "Add");
            this.pictureBoxAdd.Click += new System.EventHandler(this.OnAddItem);
            // 
            // internalListBox
            // 
            this.internalListBox.Dock = Wisej.Web.DockStyle.Fill;
            this.internalListBox.Location = new System.Drawing.Point(0, 27);
            this.internalListBox.Name = "internalListBox";
            this.internalListBox.SelectionMode = Wisej.Web.SelectionMode.One;
            this.internalListBox.Size = new System.Drawing.Size(277, 95);
            this.internalListBox.TabIndex = 1;
            // 
            // ListBoxWithDropDownControl
            // 
            this.Controls.Add(this.internalListBox);
            this.Controls.Add(this.panelHeader);
            this.Size = new System.Drawing.Size(277, 129);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel panelHeader;
        private Wisej.Web.PictureBox pictureBoxAdd;
        private Wisej.Web.PictureBox picDeleteItem;
        public Wisej.Web.ComboBox comboBox;
        private Wisej.Web.ListBox internalListBox;
    }
}
