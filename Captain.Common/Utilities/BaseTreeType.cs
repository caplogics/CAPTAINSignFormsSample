using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Utilities
{
    public class BaseTreeType
    {
        private TreeType _treeType;

        public TreeType treeType
        {
            get { return _treeType; }
            set { _treeType = value; }
        }

        public BaseTreeType(TreeType treeType)
        {
            _treeType = treeType;
        }
    }
}
