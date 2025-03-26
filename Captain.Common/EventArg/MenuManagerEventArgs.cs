/************************************************************************************
 * Class Name : AddEventArgs
 * Author : 
 * Created Date : 
 * Version : 
 * Description : This class file used to define the Eventargs
 *************************************************************************************/

using System;
using Wisej.Web;

namespace Captain.Common.Handlers
{
    public class MenuManagerEventArgs : EventArgs 
    {
        public MenuManagerEventArgs()
        {
            //
        }

        public MenuManagerEventArgs(string itemClicked)
        {
            ItemClicked = itemClicked;
            ActionGuid = Guid.NewGuid().ToString();
        }

        public MenuManagerEventArgs(EventArgs eventArgs, string itemClicked)
        {
            EventArgs = eventArgs;
            ItemClicked = itemClicked;
            ActionGuid = Guid.NewGuid().ToString();
        }

        public EventArgs EventArgs { get; set; }

        public string ActionGuid { get; set; }

        public string ItemClicked { get; set; }
    } 
}
