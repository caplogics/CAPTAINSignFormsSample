using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Utilities
{
    public class ToolTipItem
    {
        public ToolTipItem()
        {
            Name = string.Empty;
            Value = string.Empty;
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Concat(Name, Consts.Common.Colon, Consts.Common.Space, Value);
        }
    }
}
