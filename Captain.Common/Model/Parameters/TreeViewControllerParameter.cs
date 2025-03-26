using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Controllers;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Wisej.Web.Ext.NavigationBar;

namespace Captain.Common.Model.Parameters
{
    public class TreeViewControllerParameter
    {
        public TreeViewControllerParameter()
        {
 
        }

        public NavigationBar NavigationBar { get; set; }

        public TreeView TreeView { get; set; }

        public TreeType  TreeType { get; set; }

        public string BusinessModuleID { get; set; }

        public string Hierarchy { get; set; }

        public TagClass ExpectedRootNode { get; set; }
    }
}
