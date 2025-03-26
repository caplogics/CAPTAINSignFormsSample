/**********************************************************************************************************
 * Class Name   : ViewPoint<T>
 * Author       : Chitti
 * Created Date : 
 * Version      : 
 * Description  : Used to get a session value from the asp.net http session.
 **********************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Utilities
{
    public static class Captain<T>
    {
        public static SessionValueState<T> Session = new SessionValueState<T>();
    }
}
