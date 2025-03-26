/**********************************************************************************************************
 * Class Name   : EditHierarchieEventArgs
 * Author       : 
 * Created Date : 
 * Version      : 
 * Description  : Event argument for EditTeamRoleEventHandler delegate.
 **********************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.EventArg
{
    /// <summary>
    /// The edit TeamRole event argument
    /// </summary>

    public class EditPrivilegesEventArgs : EventArgs
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public EditPrivilegesEventArgs() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TeamRoleId">The TeamRole Id</param>
        public EditPrivilegesEventArgs(string ID, string scn)
        {
            this.ID = ID;
            this.SCN = scn;
        }

        /// <summary>
        /// Gets or sets the TeamRole object
        /// </summary>
        public string ID { get; set; }

        public string SCN { get; set; }
    }
}
