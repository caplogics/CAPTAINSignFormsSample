/************************************************************************************
 * Class Name : AddTabPageEventHandler,AddTabPageEventHandlerWithNodeId
 * Author : 
 * Created Date : 
 * Version : 1.0
 * Description : This class file used to define the Eventhandler
 *
 *************************************** ReviewLog ***********************************
 * Author        Version     Date        Description
 *************************************************************************************
 *
 *************************************************************************************/

namespace Captain.Common.Handlers
{
    public delegate void AddTabPageEventHandler(string fileName, string path);
    public delegate void AddTabPageEventHandlerWithNodeId(string fileName, string nodeID, string path);
}
