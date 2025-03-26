using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Utilities
{
    public class RowState
    {
        public RowState() { }

        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public string ObjectID { get; set; }
    }
}
