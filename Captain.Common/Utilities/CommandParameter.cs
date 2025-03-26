/**********************************************************************************************************
 * Class Name   : CommandParameter
 * Author       : 
 * Created Date :
 * Version      : 
 * Description  : Parameter object for the Command class.
 *               
 *************************************** Review Log *******************************************************
 * Author               Version     Date            Description
 **********************************************************************************************************
 * 
 **********************************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Captain.Common.Utilities
{
    public class CommandParameter
    {
        private object _value = string.Empty;
        private string _name = string.Empty;
        private string _target = string.Empty;

        public CommandParameter()
        {
        }

        public CommandParameter(string name, object value, string target)
        {
            _name = name;
            _value = value;
            _target = target;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public string Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }


    }
}
