/************************************************************************************
 * Class Name : DeleteNavigationTreeNodeEventArgs
 * Author : 
 * Created Date : 
 * Version : 1.0
 * Description : This class file used to define the Eventargs
 *************************************************************************************/

using System;
using System.Xml;
namespace Captain.Common.Handlers
{
    public class DeleteNavigationTreeNodeEventArgs : EventArgs
    {
        #region Constructors

        public DeleteNavigationTreeNodeEventArgs()
        {
            CascadeDelete = false;
            NodeID = string.Empty;
            ParentNodeID = string.Empty;
            CurrentNodeXML = null;
            ParentNodeXML = null;
            XmlFileName = string.Empty;
            CurrentNodePath = string.Empty;
            ParentNodePath = string.Empty;
            CurrentModule = string.Empty;
            SCN = string.Empty;
        }

        public DeleteNavigationTreeNodeEventArgs(string nodeID, string parentNodeID, bool cascadeDelete, string scn) : this()
        {
            NodeID = nodeID;
            ParentNodeID = parentNodeID;
            CascadeDelete = cascadeDelete;
            SCN = scn;
        }

        #endregion

        #region Properties

        public string NodeID { get; set; }

        public string ParentNodeID { get; set; }

        public bool CascadeDelete { get; set; }

        public string CurrentNodePath { get; set; }

        public string ParentNodePath { get; set; }

        public XmlNode CurrentNodeXML { get; set; }

        public XmlNode ParentNodeXML { get; set; }

        public string XmlFileName { get; set; }

        public string CurrentModule { get; set; }

        public string SCN { get; set; }

        #endregion

    }
}


