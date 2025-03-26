using Captain.Common.Views.Controls.Compatibility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captain.Common.Views.Controls.Compatibiliy
{
    internal class TextBoxWithValidation : Wisej.Web.TextBox
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBoxValidation Validator { get; set; }
    }
}
