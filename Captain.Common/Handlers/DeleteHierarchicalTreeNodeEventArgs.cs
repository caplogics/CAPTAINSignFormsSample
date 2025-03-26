/************************************************************************************
 * Class Name : DeleteHierarchicalTreeNodeEventArgs
 * Author : 
 * Created Date : 
 * Version : 
 * Description : This class file used to define the Eventargs
 *
 *************************************** ReviewLog ***********************************
 * Author        Version     Date        Description
 *************************************************************************************
 *
 *************************************************************************************/

using System;
using Captain.Common.Utilities;
using Wisej.Web;

namespace Captain.Common.Handlers
{
    public class DeleteHierarchicalTreeNodeEventArgs : EventArgs
    {
        #region Variables & Declarations

        private bool _cascadeDelete = false;
        private TagClass _node = null;
        private DataGridViewRow _parentRow = null;
        private DataGridViewRow _nodeRow = null;


        private string _currentModule = string.Empty;

        #endregion

        #region Properties

        public TagClass Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }

        public DataGridViewRow NodeRow
        {
            get { return _nodeRow; }
            set { _nodeRow = value; }
        }

        public DataGridViewRow ParentRow
        {
            get
            {
                return _parentRow;
            }
            set
            {
                _parentRow = value;
            }
        }

        public bool CascadeDelete
        {
            get
            {
                return _cascadeDelete;
            }
            set
            {
                _cascadeDelete = value;
            }
        }

        private string CurrentModule
        {
            set
            {
                _currentModule = value;
            }
            get
            {
                return _currentModule;
            }
        }

        #endregion

        #region Constructors

        public DeleteHierarchicalTreeNodeEventArgs()
        {
        }

        public DeleteHierarchicalTreeNodeEventArgs(TagClass node, DataGridViewRow nodeRow,  DataGridViewRow parentRow, bool doCascade)
        {
            _node  = node;
            _nodeRow = nodeRow;
            _parentRow = parentRow;
            _cascadeDelete = doCascade;
        }

        #endregion

    }
}


