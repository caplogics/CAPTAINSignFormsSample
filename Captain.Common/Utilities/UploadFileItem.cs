using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Utilities
{
    [Serializable]
    public class UploadDownloadFileItem
    {
        public UploadDownloadFileItem()
        {
            Files = new List<string>();
            CancelledFiles = new List<string>();
            ErrorFiles = new List<string>();
            ErrorMessages = new List<string>();
        }

        public List<string> Files { get; set; }
        public List<string> CancelledFiles { get; set; }
        public List<string> ErrorFiles { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
