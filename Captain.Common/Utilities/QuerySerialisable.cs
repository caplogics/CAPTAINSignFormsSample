using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;

namespace Captain.Common.Utilities
{
    /// <summary>
    /// QuerySerialisable class
    /// </summary>
    public class QuerySerialisable
    {
        public QuerySerialisable()
        {
        }

        //public List<QueryLineItem> QueryLineItemList { get; set; }

        public List<string> ResultFieldList { get; set; }
    }
}
