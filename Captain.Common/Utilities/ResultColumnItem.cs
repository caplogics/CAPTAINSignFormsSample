/************************************************************************************
* Class Name    : ResultColumnItem
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

using System;

namespace Captain.Common.Utilities
{
    public class ResultColumnItem
    {
        private string _columnName = string.Empty;
        private string _columnDescription = string.Empty;

        public ResultColumnItem()
        {
        }

        public ResultColumnItem(string columnName, string columnDescription)
        {
            _columnName = columnName;
            _columnDescription = columnDescription;
        }

        public string ColumnDescription
        {
            get
            {
                return _columnDescription;
            }
            set
            {
                _columnDescription = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
            }
        }

    }
}
