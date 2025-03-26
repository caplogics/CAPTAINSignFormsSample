using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Captain.Common.Utilities
{
    public class CaptainHttpUtility
    {
        public static NameValueCollection ParseQueryString(string qryString)
        {
            qryString = qryString.Replace("=", "");

            NameValueCollection nvc = new NameValueCollection();

            return nvc;
        }

    }
}
