using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Utilities
{
    public class AdminPasswords
    {
        public AdminPasswords()
        {
            CustomerName = string.Empty;
            GroupName = string.Empty;
            DBServiceName = string.Empty;
            VirtualSoaName = string.Empty;
            OC4JPassword = string.Empty;
            OrclAdminPassword = string.Empty;
            OC4JAdminPassword = string.Empty;
            VP40Password = string.Empty;
            ODSPassword = string.Empty;
            SysPassword = string.Empty;
            SystemPassword = string.Empty;
            SysmanPassword = string.Empty;
        }

        public string CustomerName { get; set; }
        public string GroupName { get; set; }
        public string DBServiceName { get; set; }
        public string VirtualSoaName { get; set; }
        public string OC4JPassword { get; set; }
        public string OrclAdminPassword { get; set; }
        public string OC4JAdminPassword { get; set; }
        public string VP40Password { get; set; }
        public string ODSPassword { get; set; }
        public string SysPassword { get; set; }
        public string SystemPassword { get; set; }
        public string SysmanPassword { get; set; }
    }
}
