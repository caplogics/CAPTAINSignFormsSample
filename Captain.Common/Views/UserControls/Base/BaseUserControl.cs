/************************************************************************************
* Class Name    : BaseUserControl
* Author        : Chitti
* Created Date  : 
* Version       : 1.0
* Description   : Base user control for all controls placed in a content tab.
************************************************************************************/

#region Using

using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
//using Wisej.Web;
using Wisej.Web;
using Wisej.Core;
using Wisej.Base;
using Captain.Common.Handlers;
//using Captain.Common.Menus;
using Captain.Common.Model.Objects;
using Captain.Common.Resources;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Interfaces;
//using Gizmox.WebGUI.Common.Resources;

#endregion

namespace Captain.Common.Views.UserControls.Base
{
    public partial class BaseUserControl : UserControl
    {
        #region Variables

        private TreeNode _selectedTreeNode = null;
        internal ToolBar oToolbarMnustrip;

        #endregion

        #region Events

        //

        #endregion

        #region Contructors


        /// <summary>
        /// Default contructor.
        /// </summary>
        /// 

       // public virtual ToolBar ToolbarMnustrip() { return toolBar1; }
        public BaseUserControl()
        {
            InitializeComponent();
            oToolbarMnustrip= new ToolBar();
            oToolbarMnustrip = toolBar1;
           // this.oToolbarMnustrip.Location = new System.Drawing.Point(0, 0);
        }

        /// <summary>
        /// Contructor with properties initialization.
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="usesDetailsPane"></param>
        /// <param name="detailsPaneHeight"></param>
        public BaseUserControl(BaseForm baseForm)
        {
            BaseForm = baseForm;

            InitializeComponent();

            //CaptainResourceManager.Culture = CommonFunctions.GetBrowserCulture(Context.HttpContext);
            UserID = baseForm.UserID;
            oToolbarMnustrip = new ToolBar();
            oToolbarMnustrip = toolBar1;



        }
        public virtual void PopulateToolbar(ToolBar toolBar)
        {
            if (toolBar == null)
            {
                return;
            }
            toolBar.Buttons.Clear();
            return;
        }

        #endregion

        #region Properties

        public int DetailsPaneHeight { get; set; }

        //public PropertyAttributesShortControl PropertyAttributesShort { get; set; }

        //public PropertyAttributesControl PropertyAttributes { get; set; }

        public TagClass SelectedTreeNodeTagClass { get; set; }

        public TreeNode SelectedTreeNode
        {
            get
            {
                return _selectedTreeNode;
            }
            set
            {
                _selectedTreeNode = value;
                
            }
        }

        public TagClass SelectedNodeTagClass { get; set; }

        public BaseForm BaseForm { get; set; }

        public bool IsNoContentBase { get; set; }

        public int LastHeight { get; set; }

        public int TopPadding { get; set; }

        public bool IsControlPanelVisible { get; set; }

        public bool IsSplitterEnabled { get; set; }

        public bool IsShortControl { get; set; }

        public string UserID { get; set; }

        public string Locale { get; set; }

        public bool UsesDetailsPane { get; set; }

        public bool IsDirtySplitter { get; set; }

        public int SplitterTop { get; set; }

        #endregion

        #region Public / Virtual Methods

        /// <summary>
        /// To populate the toolbar.
        /// </summary>
        /// <param name="toolBar"></param>
        /// <param name="pdfAccessMode"></param>
        public virtual void PopulateToolbar(ToolBar toolBar, PDFAccessMode pdfAccessMode)
        {
            PopulateToolbar(toolBar);
        }

        /// <summary>
        /// To populate the toolbar.
        /// </summary>
        /// <param name="toolBar"></param>
        //public virtual void PopulateToolbar(ToolBar toolBar)
        //{
        //    if (toolBar == null)
        //    {
        //        return;
        //    }
        //    toolBar.Buttons.Clear();
        //    return;
        //}

        /// <summary>
        /// Used to Uniquely Identify specific controls.
        /// </summary>
        /// <returns>128 bit integer</returns>
        public string GetGuid()
        {
            return System.Guid.NewGuid().ToString();
        }

  
        ///// <summary>
        ///// Refresh
        ///// </summary>
        //public virtual void Refresh() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshInterval"></param>
        public virtual void UpdateRefreshIntervals(int refreshInterval) { }

        /// <summary>
        /// To get the property value form the control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual T GetFormProperty<T>(string propertyName)
        {
            return default(T);
        }

        /// <summary>
        /// To set the property value form the control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public virtual void SetFormProperty<T>(string propertyName, T propertyValue)
        {
            object property = propertyValue;

            switch (propertyName)
            {
                default:
                    break;
            }
        }

 
        /// <summary>
        /// Used to set the icon of the item column for a specific row.
        /// </summary>
        /// <param name="dataGridViewRow"></param>
        /// <param name="icon"></param>
        public virtual void SetRowIcon(DataGridViewRow dataGridViewRow, string icon)
        {
            //(dataGridViewRow.Cells[1] as DataGridViewHierarchicalCell).Image = new IconResourceHandle(icon);
            dataGridViewRow.Cells[1].Update();
        }

        /// <summary>
        /// Refreshes the DataGridViewRow.
        /// </summary>
        /// <param name="dataGridViewRow"></param>
        public virtual void Refresh(DataGridViewRow dataGridViewRow)
        {

        }

        /// <summary>
        /// Get a DataGridViewRow for a TagClass
        /// </summary>
        /// <param name="tagClass"></param>
        /// <returns></returns>
        public virtual object GetRowByTagClass(TagClass tagClass)
        {
            return null;
        }

        /// <summary>
        /// Executes a menu action sent to the business module
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public virtual bool ExecuteMenuAction(MenuItem menuItem)
        {
            return false;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Handled Events

        /// <summary>
        /// switch between propertyattributes control and propertyattributes short control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSwitchToSimpleFooterClicked(object sender, EventArgs e)
        {
            //SetDetailsPaneState(Consts.Common.SimpleFooterDefaultHeight, true, true);
            //SwitchAttributes();
        }

        /// <summary>
        /// switch between propertyattributes control and propertyattributes short control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSwitchToTabsClicked(object sender, EventArgs e)
        {
            //SetDetailsPaneState(LastHeight, false, IsControlPanelVisible);
            //SwitchAttributes();
            
        }
      
        /// <summary>
        /// Resizing on minimized click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinimizeClicked(object sender, MinMaxEventArgs e)
        {
            //SetDetailsPaneState(Consts.Common.DetailsPaneMinimizedHeight, true, true);
        }

        /// <summary>
        /// Resizing on minimized click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaximizeClicked(object sender, MinMaxEventArgs e)
        {
            //SetDetailsPaneState(LastHeight, false, IsControlPanelVisible);
        }

        #endregion
    }
}
