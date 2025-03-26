/**********************************************************************************************************
 * Class Name   : RoleEntity
 * Author       : 
 * Created Date : 12/04/2009
 * Version      : 1.0.0
 * Description  : Entity object to extend RolesType.
 **********************************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity object to extend IssueType.
    /// </summary>
    [Serializable]
    public class RoleEntity
    {
        #region Constructors

        public RoleEntity()
        {
            Description = string.Empty;
            RoleID = string.Empty;
            RoleName = string.Empty;
        }

        #endregion

        #region Properties

        public string Description { get; set; }

        public string RoleID { get; set; }

        public string RoleName { get; set; }

        #endregion
    }
}
