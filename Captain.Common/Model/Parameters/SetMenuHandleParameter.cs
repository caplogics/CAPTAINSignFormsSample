using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wisej.Web;

namespace Captain.Common.Model.Parameters
{
    public class SetMenuHandleParameter
    {
        public SetMenuHandleParameter()
        {
            MenuTag = string.Empty;
            MenuName = string.Empty;
            MenuIconText = string.Empty;
            ToolTip = string.Empty;
        }

        public MenuItem MenuItem { get; set; }

        public string MenuTag { get; set; }

        public string MenuName { get; set; }

        public bool IsEventDriven { get; set; }

        public string MenuIconText { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsChecked { get; set; }

        public bool IsVisible { get; set; }

        public bool IsMenuNameResource { get; set; }

        public string ToolTip { get; set; }
    }
}
