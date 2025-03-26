/************************************************************************************
 * Class Name : AddEventArgs
 * Author : Applabs
 * Created Date : 
 * Version : 1.0
 * Description : This class file used to define the Eventargs
 *
 *************************************** ReviewLog ***********************************
 * Author        Version     Date        Description
 *************************************************************************************
 *
 *************************************************************************************/

using System;

namespace Captain.Common.Handlers
{
    public class AddEventArgs : EventArgs
    {
        private string _addType = string.Empty;
        private string _returnedCommentID = string.Empty;

        public AddEventArgs()
        {
        }

        public AddEventArgs(string addType, string returnedCommentID)
        {
            _addType = addType;
            _returnedCommentID = returnedCommentID;
        }

        public AddEventArgs(string addType)
        {
            _addType = addType;
        }

        public string ReturnedCommentID
        {
            get
            {
                return _returnedCommentID;
            }
            set
            {
                _returnedCommentID = value;
            }
        }

        public string AddType
        {
            get
            {
                return _addType;
            }
            set
            {
                _addType = value;
            }
        }
    } 
}
