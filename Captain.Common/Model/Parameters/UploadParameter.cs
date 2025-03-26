using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Model.Parameters
{
    public class UploadParameter
    {
        public UploadParameter()
        {
            Description = string.Empty;
            ObjectId = string.Empty;
            ObjectName = string.Empty;
            UploadOperationType = string.Empty;
            IsClientRepository = false;
        }

        public string[] Paths { get; set; }
        public string[] Types { get; set; }
        public string Description { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string UploadOperationType { get; set; }
        public bool IsClientRepository { get; set; }
    }
}
