/************************************************************************************
 * Class Name    : MinimizeEventArgs
 * Author        : 
 * Created Date  : 
 * Version       : 
 * Description   : This class file used to define the EventArgs
 *************************************************************************************/

using System;

namespace Captain.Common.Handlers
{
    public class MinMaxEventArgs : EventArgs
    {
        private int _height = 0;

        public MinMaxEventArgs()
        {
        }

        public MinMaxEventArgs(int height)
        {
            _height = height;
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
    }
}
