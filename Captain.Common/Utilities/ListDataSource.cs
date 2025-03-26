/************************************************************************************
* Class Name    : ListDataSource
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
using System.Collections.Generic;
using System.Text;

namespace Captain.Modules.Administration.Model
{
    public class ListDataSource
    {
        private string _displayMember = string.Empty;
        private string _valueMember = string.Empty;
        private string _permissionID = string.Empty;
        private string _teamRoleID = string.Empty;

        public ListDataSource(string valueMember, string displayMember)
        {
            _displayMember = displayMember;
            _valueMember = valueMember;
        }

        public ListDataSource(string valueMember, string displayMember, string permissionID) : this(valueMember, displayMember)
        {
            _permissionID = permissionID;
        }

        public ListDataSource(string valueMember, string displayMember, string permissionID, string teamRoleID) : this(valueMember, displayMember, permissionID)
        {
            _teamRoleID = teamRoleID;
        }

        public override string ToString()
        {
            return _displayMember;
        }

        public string DisplayMember
        {
            get { return _displayMember; }
        }

        public string ValueMember
        {
            get
            {
                return _valueMember;
            }
        } 

        public string PermissionID
        {
            get { return _permissionID; }
        }

        public string TeamRoleID
        {
            get { return _teamRoleID; }
        }
    }
}
