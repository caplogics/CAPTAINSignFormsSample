/**********************************************************************************************************
 * Class Name   : SessionValueState<T>
 * Author       : chitti
 * Created Date : 
 * Version      : 
 * Description  : Used to get a session value from the asp.net http session.
 **********************************************************************************************************/

using Wisej.Web;

namespace Captain.Common.Utilities
{
    public class SessionValueState<T>
    {
        public SessionValueState()
        {
        }

        public T this[string name]
        {
            get
            {
                var session = Application.Session;

                T returnValue = default(T);

                try
                {
                    if (session != null && session[name] != null)
                    {
                        returnValue = (T)session[name];
                    }
                }
                catch { }

                if (returnValue == null) { returnValue = default(T); }

                return returnValue;
            }
            set
            {
                var session = Application.Session;
                session[name] = value;
            }
        }

        public T this[string name, T defaultValue]
        {
            get
            {
                var session = Application.Session;

                T returnValue = default(T);

                try
                {
                    if (session != null && session[name] != null)
                    {
                        returnValue = (T)session[name];
                    }
                }
                catch { }

                if (returnValue == null) { returnValue = defaultValue; }

                return returnValue;
            }
            set
            {
                var session = Application.Session;
                session[name] = value;
            }
        }
    }
}
