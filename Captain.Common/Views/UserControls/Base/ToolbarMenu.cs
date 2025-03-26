using System;
using Wisej.Web;

namespace Captain.Common.Views.UserControls.Base
{
    public partial class ToolbarMenu : Wisej.Web.UserControl
    {
        internal ToolBar oToolbarMenustrip;
        public ToolbarMenu()
        {
            InitializeComponent();
            oToolbarMenustrip =  toolBar1;
        }
        //public virtual void PopulateToolbar(ToolBar toolBar, PDFAccessMode pdfAccessMode)
        //{
        //    PopulateToolbar(toolBar);
        //}

        /// <summary>
        /// To populate the toolbar.
        /// </summary>
        /// <param name="toolBar"></param>
        public virtual void PopulateToolbar(ToolBar toolBar)
        {
            if (toolBar == null)
            {
                return;

            }
            toolBar.Buttons.Clear();
            return;
        }

    }
}
