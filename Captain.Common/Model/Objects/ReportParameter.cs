using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Model.Objects
{
    public class ReportParameter
    {
        #region Constructors

        public ReportParameter()
        {
            Name = string.Empty;
            Values = new List<string>();
            MultiValuesAllowed = false;
        }

        public ReportParameter(string name)
            : this()
        {
            Name = name;
        }

        #endregion

        #region Properties

        public string Name { get; set; }
        public List<string> Values { get; set; }
        public bool MultiValuesAllowed { get; set; }

        #endregion
    }
}
