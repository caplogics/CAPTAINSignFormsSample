/************************************************************************************
* Class Name    : EncodedStringClass
* Author        : 
* Created Date  : 
* Version       : 1.0
* Description   : 
* 
*****************************************ReviewLog***********************************
* Author Version Date Description
*************************************************************************************
*
*************************************************************************************/

#region Using

using System.Text;
using System.IO; 

#endregion

namespace Captain.Common.Utilities
{
    public class EncodedStringClass : StringWriter
    {
        private Encoding _encoding;

        public EncodedStringClass(StringBuilder stringBuilder, Encoding encoding) : base(stringBuilder)
        {
            _encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return _encoding;
            }
        }
    }
}
